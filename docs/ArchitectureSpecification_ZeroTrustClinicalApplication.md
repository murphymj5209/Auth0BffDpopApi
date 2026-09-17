# Architecture Specification: Zero Trust Clinical Application (Mortho)

## 1. Executive Summary & Infrastructure Placement

- **Host Environment:** Single Google Compute Engine (GCE) VM running a pre-configured Windows Server + Microsoft SQL Server image.
- **Data Engine:** Microsoft SQL Server co-located on the same VM host using local NVMe storage with Transparent Data Encryption (TDE) enabled via Google Cloud KMS. Inter-process communication between the ASP.NET Core backend and SQL Server communicates locally via Shared Memory (`LPC`) or `localhost:1433` for sub-millisecond query execution.
- **Perimeter & Ingress:** Cloudflare Anycast Edge routing inbound requests exclusively through an outbound-initiated Cloudflare Tunnel (`cloudflared.exe` Windows Service). All inbound public ports (80, 443, 1433, 3389) on the GCE VPC firewall remain permanently closed.
- **Identity & Token Authority:** FusionAuth (OIDC/OAuth 2.0 provider) managing clinical authentication, passkeys/WebAuthn, hospital enterprise SSO federation, and DPoP-bound token issuance.
- **Client Security Boundary:** Backend-for-Frontend (BFF) pattern using ASP.NET Core and YARP (Yet Another Reverse Proxy) following Damien Bod's reference architecture. Raw JWTs are never exposed to browser storage.
- **Real-Time Communication:** Stream Chat (HIPAA-compliant managed messaging) for bidirectional doctor-to-doctor and care team chat, session management, and push notifications.
- **Asset Storage:** Google Cloud Storage (GCS) or Cloudflare R2 for clinical scans, X-rays, and operative note PDFs accessed via short-lived pre-signed v4 URLs.

---

## 2. OSI Model Layer Mapping

### Layer 3: Network Layer
- **Primary System:** Cloudflare Global Edge.
- **Functions:** Anycast routing and volumetric DDoS defense (Magic Transit). Absorbs Layer 3 floods (SYN, UDP amplification) across global Points of Presence before packets reach Google Cloud VPC infrastructure.

### Layer 4: Transport Layer
- **Primary System:** Cloudflare Tunnel (`cloudflared.exe`).
- **Functions:** Origin shielding and firewall cloaking. A persistent, outbound-only QUIC/HTTPS tunnel runs as a Windows Service on GCE. Inbound firewall ports (TCP 80, 443, 1433, 3389) are disabled. Internet scanners cannot detect the server.

### Layer 5: Session Layer
- **Primary System:** Stream Chat & ASP.NET Core BFF Cookie Manager.
- **Functions:** 
  - *Stream Chat:* Stateful, bidirectional WebSockets for presence tracking, typing indicators, and message delivery receipts.
  - *ASP.NET Core BFF:* Manages server-side sessions using an encrypted, `HttpOnly`, `SameSite=Strict`, `Secure` cookie (`__Host-mortho-session`) with sliding expiration.

### Layer 6: Presentation Layer
- **Primary System:** Cloudflare, FusionAuth, ASP.NET Core BFF (DPoP Signer), and Syncfusion Engines.
- **Functions:**
  - *Cloudflare:* Public TLS 1.3 termination at the edge.
  - *FusionAuth:* Manages asymmetric key pairs (RS256/ES256) and exposes JWKS (`/.well-known/jwks.json`).
  - *ASP.NET Core BFF:* Cryptographically signs an ephemeral proof JWT (`DPoP` HTTP header) bound to the HTTP verb and target URL for every forwarded request using an in-memory private key.
  - *SQL Server:* Transparent Data Encryption (TDE) at rest and encrypted TDS memory transport.
  - *Syncfusion Engines:* Parses binary PDF streams, vector markup, digital signatures, and Word `.docx` structures client-side.

### Layer 7: Application Layer (Perimeter Defense)
- **Primary System:** Cloudflare WAF & API Shield.
- **Functions:** Inspects HTTP payloads, enforces OWASP Core Rule Sets, blocks SQL injection/XSS attempts, challenges automated scrapers via Turnstile, and enforces strict rate limits on authentication and API endpoints.

### Layer 7: Application Layer (Identity & Authentication)
- **Primary System:** FusionAuth.
- **Functions:** Hosted login flows, passkeys/WebAuthn, biometric MFA, enterprise SAML/OIDC federated SSO broker for hospital directories, and DPoP-bound token issuance.

### Layer 7: Application Layer (Backend-for-Frontend & Reverse Proxy)
- **Primary System:** ASP.NET Core with YARP.
- **Functions:** Serves the Angular application, validates anti-CSRF headers (`X-CSRF: 1`), intercepts secure session cookies, attaches the DPoP proof and access token, and reverse-proxies requests to the core API.

### Layer 7: Application Layer (Real-Time Communication)
- **Primary System:** Stream Chat.
- **Functions:** Care team channels, patient-to-provider consultation threads, content moderation, and offline push notification dispatch (Apple APNs / Google FCM).

### Layer 7: Application Layer (Clinical UI Suites)
- **Primary System:** Syncfusion UI Suites (Flutter & Angular).
- **Functions:**
  - *Syncfusion Flutter (Bedside / Mobile):* Interactive range-of-motion charts, recovery sparklines, and mobile touch schedules.
  - *Syncfusion Angular (Desktop Workstations):* Multi-resource OR timeline scheduling boards, virtualized DataGrids with frozen panes, operative note text editors, and PDF annotation tools with digital signature capture.

### Application Logic & Data Layer: Processing & Zero Trust Enforcement
- **Primary System:** ASP.NET Core API + Microsoft SQL Server.
- **Functions:** Validates the incoming DPoP proof thumbprint (`jkt`), enforces coarse RBAC, and applies in-process fine-grained authorization (ReBAC/ABAC) via SQL queries using the local `CaseAccessGrants` table.

---

## 3. Separation of Administrative Duties & Roles

| Role / Title | Tool / Workspace | Scope of Responsibility |
| :--- | :--- | :--- |
| **SecOps / Cloudflare Admin** | Cloudflare Dashboard & `config.yml` | Manages DNS, Cloudflare Tunnel routes, WAF rate-limiting, edge bot challenges, and Cloudflare Access rules. |
| **Identity Admin** | FusionAuth Console (`:9011`) | Configures OIDC client apps, hospital SAML/OIDC identity providers, MFA policies, and JWT Populate Lambdas. |
| **Clinic / Tenant Admin** | Mortho Web Portal (Angular) | Onboards clinical staff, assigns surgical team roles (Surgeon, PA, Tech), and manages clinic rosters via Mortho API. |
| **Systems Admin / DBA** | Google Cloud Console (IAP) & SSMS | Manages GCE VM sizing, disk backups, Cloud KMS keys, and administers SQL Server over Google IAP tunnel without opening port 1433. |
| **Application Developer** | Visual Studio / JetBrains Rider | Implements BFF YARP pipelines, DPoP validation handlers, and EF Core ReBAC relational filters. |

---

## 4. Technical Implementation & Code Patterns

### 4.1 FusionAuth JWT Populate Lambda
Extracts and injects tenant, role, and provider metadata into the token:

```javascript
function populate(jwt, user, registration) {
  jwt.tenant_id = registration.tenantId;
  jwt.npi = user.data ? user.data.npiNumber : null;
  jwt.roles = registration.roles || [];
  jwt.mortho_tier = registration.data ? registration.data.tier : 'standard';
  jwt.origin_auth = 'FusionAuth-ZeroTrust';
}
```
