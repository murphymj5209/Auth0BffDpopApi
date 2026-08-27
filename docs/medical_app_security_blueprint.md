# High-Security Medical Application Architecture Blueprint
## Based on Auth0, BFF, OAuth DPoP, PAR & Private Key JWT

Building a healthcare/medical application handling **ePHI (Electronic Protected Health Information)** requires adherence to strict regulatory standards (e.g., **HIPAA Security Rule §164.312**, **GDPR Article 32**, **HITECH**).

This document outlines:
1. **How the code in this repository works** (step-by-step execution flow).
2. **Why this architecture is ideal for Healthcare (ePHI)**.
3. **Pure Technical Security Controls: What is missing in the base code**.
4. **How to adapt this codebase for Web, Mobile, and Microservice architectures**.
5. **HIPAA / Medical compliance enhancements checklist**.

---

## 1. Codebase Architecture & Flow Outline

The pattern implemented in this repository eliminates the most dangerous web vulnerability in medical apps: **Token Theft via XSS (Cross-Site Scripting)**.

```mermaid
flowchart TD
    subgraph Client Layer
        A[Angular / Web / Mobile UI]
    end

    subgraph BFF Layer (ASP.NET Core)
        B[BFF Server / Session Manager]
        C[OIDC & PAR Client Assertion Service]
        D[DPoP Token Manager Engine]
        E[YARP Reverse Proxy]
    end

    subgraph Identity Provider
        F[Auth0 IdP]
    end

    subgraph Medical Resource Server
        G[Protected Downstream Medical API]
        H[EHR / Patient DB / FHIR Store]
    end

    A -- 1. Same-Origin HTTPS + Cookie --> B
    B -- 2. PAR + Private Key JWT --> F
    F -- 3. Auth Code & Tokens --> C
    B -- 4. Encrypted HttpOnly Session Cookie --> A
    B -- 5. Access Token + DPoP Proof (ES256 Signature) --> G
    E -- 5b. Reverse Proxied DPoP Requests --> G
    G -- 6. Validate Token + DPoP Thumbprint --> H
```

### Step-by-Step Code Execution Flow

#### Step 1: User Initiates Login ([AccountController.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/Controllers/AccountController.cs#L11-L25))
- The user clicks **Login** in the browser.
- The request hits `GET /api/Account/Login`.
- The BFF server triggers an OIDC `Challenge("Auth0")`.

#### Step 2: Server-to-Server Pushed Authorization Request (PAR) & Client Assertion ([OidcEventHandlers.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/OidcEventHandlers.cs#L77-L91) & [AssertionService.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/AssertionService.cs#L11-L51))
- Before any browser redirection occurs, `OnPushAuthorization` is fired.
- `AssertionService.CreateClientToken` signs a JWT containing the client ID, audience, and timestamp using an **RSA-2048 Private Key** (`rsa256-private.pem`).
- The signed client assertion is sent directly to Auth0’s `/oauth/par` endpoint over server-to-server TLS.
- Auth0 validates the signature against the public key uploaded to the Auth0 dashboard and returns a short-lived `request_uri`.
- The BFF redirects the user's browser to `https://<auth0-domain>/authorize?request_uri=...`.

#### Step 3: Authorization Code Exchange & Token Issuance
- The user authenticates at Auth0 (with MFA / Biometrics / SAML / Enterprise SSO).
- Auth0 redirects back to the BFF callback endpoint `/callback?code=...`.
- In `OnAuthorizationCodeReceivedHandler`, the BFF sends the authorization code + another RSA Client Assertion to Auth0's `/oauth/token` endpoint.
- Auth0 issues:
  1. **ID Token** (User identity claims).
  2. **Access Token** (Sender-constrained with DPoP confirmation claim `cnf.jkt`).
  3. **Refresh Token** (For offline session extension).

#### Step 4: Secure Cookie Session Creation ([Program.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/Program.cs#L71-L77))
- The BFF stores the tokens in server-side session memory (or encrypted distributed cache like Redis).
- The BFF responds to the browser with an encrypted cookie named `__Host-Http-Auth0-Web`:
  - `HttpOnly`: Unreadable by JavaScript (completely immune to XSS token theft).
  - `SameSite=Lax` / `Strict`: Protects against Cross-Site Request Forgery (CSRF).
  - `Secure`: HTTPS-only transmission.

#### Step 5: Invoking Protected Medical APIs with DPoP ([DownstreamDataController.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/Controllers/DownstreamDataController.cs#L41-L59) & [Program.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/Program.cs#L116-L129))
- The Angular app calls `GET /api/DownstreamData`. The browser automatically includes the `__Host-` session cookie and `X-XSRF-TOKEN` header ([secureApiInterceptor.ts](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/ui/src/app/secure-api.interceptor.ts)).
- The BFF retrieves the user's Access Token from session storage.
- `Duende.AccessTokenManagement` signs a **DPoP Proof JWT** using an **ECDSA P-256 (ES256) Private Key** (`ecdsa256-private.pem`).
- The HTTP request sent to the Web API contains:
  ```http
  GET /api/DownstreamData HTTP/1.1
  Host: api.medical.local
  Authorization: DPoP eyJhbGciOiJFUzI1NiIs... (Access Token)
  DPoP: eyJhbGciOiJFUzI1NiIs... (DPoP Proof JWT)
  ```

#### Step 6: Medical API DPoP Guard Validation ([Program.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/Api/Program.cs#L48-L59))
- The downstream Web API validates:
  1. Standard Auth0 JWT claims (Issuer, Audience, Expiration, Signature).
  2. DPoP Proof JWT signature (verifies it was signed by the key matching `cnf.jkt`).
  3. DPoP Proof claims (`htu` matches HTTP URI, `htm` matches HTTP method `GET`, `iat` within freshness window).
- If an attacker steals the Access Token from an API gateway log, **they cannot use it** without the matching ES256 Private Key!

---

## 2. Pure Technical Security Controls: What is Missing in the Base Code

While the identity protocol foundation (BFF, DPoP, PAR, Private Key JWT) is excellent, **from a pure technical application security standpoint**, the codebase is missing 6 critical security controls:

### 1. Key Management & Vault Integration (Hardware Security)
- **Missing**: In the current code ([Program.cs:L55](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/Program.cs#L55) & [AssertionService.cs:L18](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/AssertionService.cs#L18)), private RSA and ECDSA keys are read directly from local `.pem` files on disk.
- **Required Security Defense**: Use **Hardware Security Modules (HSM)**, **Azure Key Vault**, or **AWS KMS** with X.509 Certificate Store integration. Automated key rotation policies must be established to rotate RSA/ECDSA signing keys periodically without application downtime.

### 2. Encrypted Distributed Session Store & Revocation System
- **Missing**: Sessions currently reside in-memory. If a user session is hijacked or compromised, there is no centralized Session Revocation API to immediately terminate all active tokens and cookies across distributed web nodes.
- **Required Security Defense**: Distributed Redis session cache encrypted via **ASP.NET Core Data Protection API (PersistKeysToAzureBlobStorage / AWS S3)** with real-time session revocation lists (SRL).

### 3. Rate Limiting & Throttling (DoS & Automated Attack Defense)
- **Missing**: There is **zero rate limiting** configured on endpoints like `/api/Account/Login`, `/api/User`, or the Auth0 callback routes.
- **Required Security Defense**: Enforce `Microsoft.AspNetCore.RateLimiting` middleware (Fixed Window & Token Bucket rate limiters) to protect login endpoints against credential stuffing, automated scraping, and brute force attacks.

### 4. Zero-Trust Network & Mutual TLS (mTLS) Between Microservices
- **Missing**: Communication between the BFF server and the downstream Web API relies solely on HTTP/HTTPS without server-to-server client authentication.
- **Required Security Defense**: Implement **mTLS (Mutual TLS)** via service mesh (Linkerd / Istio) or X.509 client certificates between BFF and Web API so unauthorized internal network traffic cannot reach internal endpoints.

### 5. YARP Reverse Proxy Authorization & Header Stripping Defenses
- **Missing**: In [YarpConfigurations.cs:L16](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/YarpConfigurations.cs#L16), proxied routes default to `AuthorizationPolicy = "Anonymous"`. Additionally, YARP does not strip internal proxy headers (`X-Forwarded-For`, `X-Forwarded-Host`) which can lead to IP spoofing.
- **Required Security Defense**: Enforce `AuthorizationPolicy = "CookieAuthenticated"` on all YARP routes and configure explicit YARP header transforms to sanitize incoming headers.

### 6. Mobile Certificate Pinning & Secure Enclave Hardening
- **Missing**: The repository focuses on the web BFF, but does not specify mobile network security defenses.
- **Required Security Defense**: Mobile native apps MUST implement **SSL/TLS Certificate Pinning** (HPKP) to mitigate Man-in-the-Middle (MitM) attacks via user-installed proxy CA certificates (e.g. Charles Proxy / Burp Suite), and generate DPoP keys exclusively inside the **Hardware Secure Enclave** (iOS) or **Android Keystore System**.

---

## 3. Web vs Mobile Architecture Strategy

When building a medical application for both **Web** and **Mobile** (iOS / Android / Flutter / React Native), you have two proven architectural patterns:

```
                  +-------------------------------------------------------+
                  |                  Identity Provider                    |
                  |                       (Auth0)                         |
                  +--------------------------+----------------------------+
                                             |
                   PAR + RSA Private Key     |     OIDC Code + PKCE
                   Client Assertions         |     + DPoP (Hardware Key)
                                             |
                  +--------------------------+----------------------------+
                  |                                                       |
        +---------v---------+                                   +---------v---------+
        |   Web BFF Host    |                                   | Native Mobile App |
        |  (ASP.NET Core)   |                                   |  (iOS / Android)  |
        +---------+---------+                                   +---------+---------+
                  |                                                       |
     Cookie Session | DPoP Token                                 Direct DPoP | Hardware Key
     (__Host-Http) | Forwarding                                Token Request | (Secure Enclave)
                  |                                                       |
        +---------v---------+                                             |
        |   Angular SPA     |                                             |
        +-------------------+                                             |
                  |                                                       |
                  +--------------------+----------------------------------+
                                       |
                             DPoP Sender-Constrained
                                 Access Token
                                       |
                            +----------v----------+
                            | Downstream Medical  |
                            |  API (FHIR / ePHI)  |
                            +---------------------+
```

### Pattern A: Web Applications (BFF Pattern - Standard in this repo)
- **Client**: Browser SPA (Angular, React, Vue).
- **Session**: Cookie-based session (`__Host-` cookie) between Browser and BFF.
- **Tokens**: Held exclusively by the BFF server.
- **DPoP & PAR**: Handled entirely on the BFF server.

### Pattern B: Native Mobile Applications (Direct Native DPoP Client)
- **Client**: Native iOS (Swift), Android (Kotlin), or Cross-Platform (Flutter / React Native).
- **Key Storage**: The mobile app generates its own **ECDSA P-256 Key Pair inside the Device Hardware Security Module**:
  - **iOS**: Secure Enclave (`kSecAttrAccessControlPrivate` / Keychain).
  - **Android**: Android Keystore System (`KeyGenParameterSpec` with Hardware-backed security).
- **Auth Flow**: Native App performs Authorization Code Flow with **PKCE** directly with Auth0.
- **DPoP Execution**: When invoking the Medical API, the Mobile App signs the `DPoP` proof header using the key stored in the hardware Secure Enclave.
- **Security Benefit**: Even if the mobile device is rooted/jailbroken, the private key in the hardware enclave cannot be extracted!

---

## 4. HIPAA & Medical Security Compliance Checklist

To ensure your application meets **HIPAA Security Rule** and **GDPR** compliance when adapting this codebase:

| Compliance Area | HIPAA Requirement | Implementation Strategy in this Architecture |
| :--- | :--- | :--- |
| **Access Control** | §164.312(a)(1) Unique User Identification | Auth0 OIDC ID Tokens with unique `sub` identifiers. Enforce Multi-Factor Authentication (MFA / WebAuthn / Passkeys). |
| **Transmission Security** | §164.312(e)(1) Encryption in Transit | TLS 1.3 only, HSTS preload, HTTPS redirection ([Program.cs:L207](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/Program.cs#L207)). |
| **Token Theft Protection** | Zero Trust & Least Privilege | **DPoP (RFC 9449)** prevents stolen access tokens from being replayed. |
| **Client Vulnerabilities** | Prevent ePHI Leakage via XSS | **BFF Cookie Pattern**: Zero tokens or medical data in browser `localStorage` or `sessionStorage`. |
| **Audit Controls** | §164.312(b) Audit Logs | Log all API accesses with User ID (`sub`), Patient ID (`patient_id`), Client IP, Timestamp, and Action. Use OpenTelemetry / Serilog with immutable storage (Azure Monitor / AWS CloudWatch). |
| **Automatic Logoff** | §164.312(a)(2)(iii) Inactivity Timeout | Configure BFF session cookie expiration (e.g. 15-30 minutes of inactivity) and sliding expiration handling. |
| **Data at Rest Security** | §164.312(a)(2)(iv) Encryption at Rest | Store certificate private keys in Azure KeyVault / AWS KMS. Encrypt Redis session cache and backend databases (AES-256 / Transparent Data Encryption). |
| **Network Isolation** | §164.312(e)(2) Boundary Protection | Use YARP proxy to shield internal microservices. Downstream APIs must not be exposed to the public internet without passing through API Gateway / Mesh. |

---

## 5. Key Refactoring & Production Steps for Your Project

To turn this repository into your production medical app template, execute these steps:

1. **Unify Private Key Certificate Management**:
   - Store RSA (Client Assertion) and ECDSA (DPoP) certificates in **Azure KeyVault** or **AWS Secrets Manager**.
   - Update [AssertionService.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/AssertionService.cs) and [Program.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/Program.cs) to read certificates from X509 store / configuration instead of local `.pem` files.

2. **Secure YARP Authorization Policies**:
   - Update [YarpConfigurations.cs](file:///e:/Github/Damienbod/Auth0BffDpopApi/bff/server/YarpConfigurations.cs) to require an authenticated cookie policy on all proxied medical API routes.

3. **Implement FHIR / Medical Scopes**:
   - Define granular OAuth scopes in Auth0 (e.g. `read:patients`, `write:prescriptions`, `read:lab_results`).
   - Add scope checking policies on downstream API controllers:
     ```csharp
     [Authorize(Policy = "ReadPatientsPolicy")]
     ```

4. **Enable OpenTelemetry Audit Logging**:
   - Leverage `AppsAspire.ServiceDefaults` to send audit traces containing user `sub` and requested resource IDs to a centralized SIEM / log accumulator.
