# 02a - Cloudflare Work Effort: Edge Security, Zero-Trust Tunnel & Perimeter Defense

## 1. Overview & Architectural Role

Cloudflare acts as the external security perimeter and Anycast ingress gateway for the Mortho clinical platform. By utilizing **Cloudflare Tunnel (`cloudflared.exe`)**, all inbound ports on the Google Compute Engine (GCE) host VM remain permanently closed to the internet. This technique (Origin Cloaking) completely hides the server IP address and neutralizes internet-wide port scans (Shodan, Censys, masscan).

```mermaid
flowchart LR
    subgraph Internet["Public Internet & Clinical Users"]
        U1[Surgeon Desktop - Angular]
        U2[Mobile App - Flutter]
        U3[Device Manufacturer Portal]
    end

    subgraph CloudflareEdge["Cloudflare Global Anycast Edge"]
        DDoS[L3/L4 DDoS Mitigation - Magic Transit]
        WAF[WAF + OWASP Core Rule Set]
        Turnstile[Turnstile Bot Detection]
        APIShield[API Shield & Rate Limiting]
        EdgeTLS[TLS 1.3 Strict Termination]
    end

    subgraph GCE_Host["Google Compute Engine VM (Windows Server)"]
        Tunnel[cloudflared.exe Windows Service]
        BFF[ASP.NET Core BFF Gateway :5000]
    end

    Internet -->|HTTPS / Port 443| DDoS
    DDoS --> WAF --> Turnstile --> APIShield --> EdgeTLS
    EdgeTLS <== Persistent Outbound QUIC/HTTPS Tunnel ==> Tunnel
    Tunnel -->|HTTP 127.0.0.1:5000 (Loopback)| BFF
```

---

## 2. Cloudflare Tunnel (`cloudflared`) Implementation on Windows Server

### Step 1: Install `cloudflared.exe` as a Managed Windows Service
Download and install the official Cloudflare Tunnel daemon on the GCE Windows Server host:

```powershell
# Run inside Administrator PowerShell on GCE VM
New-Item -ItemType Directory -Force -Path "C:\Program Files\Cloudflare"
Invoke-WebRequest -Uri "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-windows-amd64.exe" `
                  -OutFile "C:\Program Files\Cloudflare\cloudflared.exe"

# Install as Windows Service with Tunnel Token
& "C:\Program Files\Cloudflare\cloudflared.exe" service install "<CLOUDFLARE_TUNNEL_TOKEN>"
Start-Service cloudflared
Set-Service -Name cloudflared -StartupType Automatic
```

### Step 2: Declarative Tunnel Configuration (`config.yml`)
Create `C:\Program Files\Cloudflare\config.yml` for local service routing:

```yaml
tunnel: <TUNNEL_UUID>
credentials-file: C:\Program Files\Cloudflare\<TUNNEL_UUID>.json

ingress:
  # Route production web workstation traffic to ASP.NET Core BFF
  - hostname: portal.morthoclinical.com
    service: http://127.0.0.1:5000
    originRequest:
      connectTimeout: 10s
      noTLSVerify: false
      httpHostHeader: portal.morthoclinical.com

  # Route identity management traffic to self-hosted FusionAuth instance
  - hostname: auth.morthoclinical.com
    service: http://127.0.0.1:9011
    originRequest:
      connectTimeout: 10s
      httpHostHeader: auth.morthoclinical.com

  # Default catch-all (Drop all unmapped hostnames)
  - service: http_status:404
```

---

## 3. Web Application Firewall (WAF) & OWASP Rule Configuration

### OWASP Core Rule Set (CRS) Paranoia Level
- **Paranoia Level:** Enforced at **Level 2** for all clinical API routes.
- **Anomaly Score Threshold:** 25 (Blocks suspicious payloads, SQLi attempts, and cross-site scripting strings before reaching BFF).

### Custom WAF Firewall Rules (Terraform / Cloudflare Dashboard)

1. **Strict Rate Limiting on Authentication Routes**:
   - **Target URI:** `/api/Account/Login`, `/oauth/*`, `/api/User/forgot-password`
   - **Rule:** Max **5 requests per minute** per IP address. Exceeding threshold triggers a Cloudflare Turnstile Managed Challenge for 1 hour.

2. **Geo-Blocking / IP Reputation Filter**:
   - Block requests originating from high-risk Autonomous System Numbers (ASNs) and non-operating geographic zones where surgical clinics and device manufacturers do not operate.

3. **Mandatory Header Verification**:
   - Block any state-changing HTTP request (`POST`, `PUT`, `DELETE`, `PATCH`) to `/api/*` that lacks the `X-CSRF: 1` or `X-XSRF-TOKEN` custom header.

---

## 4. API Shield & Mobile Request Hardening

To safeguard native mobile and desktop clients from API scraping and automated probing:

1. **Schema Validation**:
   - Import OpenAPI v3 schemas for `/api/v1/cases/*` and `/api/v1/implants/*`.
   - Reject malformed JSON bodies at the Cloudflare Edge before processing by the ASP.NET Core model binder.

2. **Bot Management & Turnstile Integration**:
   - Public patient self-registration and clinic lead capture forms enforce invisible **Cloudflare Turnstile** tokens verified server-side.

3. **SSL/TLS 1.3 & Cipher Suite Locking**:
   - Minimum TLS Version: **TLS 1.3**.
   - Strict SNI matching enabled.
   - HTTP Strict Transport Security (HSTS): `max-age=63072000; includeSubDomains; preload`.

---

## 5. Summary of Penetration Testing Defense Capabilities

| Threat Vector | Pentest.com Test Procedure | Cloudflare Zero-Trust Defense |
| :--- | :--- | :--- |
| **Port Scanning / Host Discovery** | Nmap / Masscan against server IP | **Neutralized**: Host IP has zero public DNS entries and all VPC inbound ports are closed. |
| **Volumetric DDoS** | UDP flood / SYN flood > 100 Gbps | **Neutralized**: Absorbed automatically by Cloudflare Magic Transit Anycast nodes. |
| **Credential Stuffing** | Rapid automated POSTs to login endpoints | **Neutralized**: Rate limiter triggers Turnstile CAPTCHA after 5 failed attempts/min. |
| **Payload Injection (SQLi / XSS)** | Encoded malicious SQL/Script payloads | **Neutralized**: Inspected and blocked at Edge via WAF OWASP CRS Level 2 rules. |
| **HTTP Request Smuggling** | Chunked transfer encoding desynchronization | **Neutralized**: Edge HTTP/2 and HTTP/3 parsers normalize all incoming headers and bodies. |
