# HIPAA-Compliant Healthcare Architecture Blueprint
## .NET 10, Azure Native Stack, Microsoft Entra ID, BFF Pattern, and Flutter (Mobile & Web)

---

## 📋 Table of Contents / Document Index

1. [Executive Summary & Core Security Pillars](#1-executive-summary--core-security-pillars)
   - [1.1 Project Objectives & High-Level Architecture](#11-project-objectives--high-level-architecture)
   - [1.2 Key Architectural Pillars](#12-key-architectural-pillars)
   - [1.3 Threat Model & Anti-Hacker / Anti-Attacker Defenses](#13-threat-model--anti-hacker--anti-attacker-defenses)
   - [1.4 Strategic Business Case: Why ASP.NET Zero / ABP.io Renewal & Upgrade Will Not Work](#14-strategic-business-case-why-aspnet-zero--abpio-renewal--upgrade-will-not-work)
   - [1.5 The 5-Layer API Defense-in-Depth Protection Shield](#15-the-5-layer-api-defense-in-depth-protection-shield)
2. [Technology Stack & Framework Choices](#2-technology-stack--framework-choices)
   - [2.1 .NET 10 & ASP.NET Core Execution Engine](#21-net-10--aspnet-core-execution-engine)
   - [2.2 Entity Framework Core 10 (EF Core 10) & Data Isolation](#22-entity-framework-core-10-ef-core-10--data-isolation)
   - [2.3 Caching Architecture: Azure Cache for Redis vs. In-Memory](#23-caching-architecture-azure-cache-for-redis-vs-in-memory)
   - [2.4 gRPC & Protocol Buffers (Google) for High-Speed Microservices](#24-grpc--protocol-buffers-google-for-high-speed-microservices)
   - [2.5 OpenFGA / OPA (CNCF Zanzibar Model) for Relationship Authorization](#25-openfga--opa-cncf-zanzibar-model-for-relationship-authorization)
   - [2.6 OpenTelemetry & Polly for Observability & Fault Tolerance](#26-opentelemetry--polly-for-observability--fault-tolerance)
   - [2.7 JetBrains Qodana & Snyk for DevSecOps Security Scanning](#27-jetbrains-qodana--snyk-for-devsecops-security-scanning)
   - [2.8 Infrastructure as Code (IaC) — Microsoft Bicep](#28-infrastructure-as-code-iac--microsoft-bicep)
   - [2.9 Database Simplification & De-Bloating Strategy (ASP.NET Zero 35+ Tables vs Cloud-Native 7 Tables)](#29-database-simplification--de-bloating-strategy-aspnet-zero-35-tables-vs-cloud-native-7-tables)
3. [Master List of 15 Azure / Microsoft Components](#3-master-list-of-15-azure--microsoft-components)
   - [3.1 System Architecture Flow Diagram](#31-system-architecture-flow-diagram)
   - [3.2 Detailed Component Matrix (WHEN / WHERE / WHAT)](#32-detailed-component-matrix-when--where--what)
4. [Identity & Authorization Hierarchy (Tenant ➔ Org ➔ User)](#4-identity--authorization-hierarchy-tenant--org--user)
   - [4.1 Multi-Tenant Hierarchy Mapping](#41-multi-tenant-hierarchy-mapping)
   - [4.2 Authorization Layering: RBAC + ReBAC + ABAC](#42-authorization-layering-rbac--rebac--abac)
   - [4.3 User Onboarding & Provisioning Architecture (Graph API Integration)](#43-user-onboarding--provisioning-architecture-graph-api-integration)
5. [Token Management & Security: Web vs. Flutter Mobile](#5-token-management--security-web-vs-flutter-mobile)
   - [5.1 Web Architecture (ASP.NET Core BFF + DPoP)](#51-web-architecture-aspnet-core-bff--dpop)
   - [5.2 Flutter Mobile Architecture (iOS Keychain / Android KeyStore + Biometrics)](#52-flutter-mobile-architecture-ios-keychain--android-keystore--biometrics)
6. [Implementation Sequence & Environment Strategy](#6-implementation-sequence--environment-strategy)
   - [6.1 Phase 1: Local Development Environment (PC Setup)](#phase-1-local-development-environment-pc-setup)
   - [6.2 Phase 2: QA Environment on Azure (Cloud-Native QA)](#phase-2-qa-environment-on-azure-cloud-native-qa)
   - [6.3 Phase 3: Production Environment (Azure Enterprise)](#phase-3-production-environment-azure-enterprise)
7. [Bank-Grade Financial Security Standards (FAPI 2.0 & High-Assurance Controls)](#7-bank-grade-financial-security-standards-fapi-20--high-assurance-controls)
   - [7.1 Financial-Grade API (FAPI 2.0) Protocol Alignment](#71-financial-grade-api-fapi-20-protocol-alignment)
   - [7.2 Cryptographic Non-Repudiation for Critical Actions](#72-cryptographic-non-repudiation-for-critical-actions)
   - [7.3 Dedicated Hardware Security Modules (FIPS 140-2 Level 3 / 140-3 HSM)](#73-dedicated-hardware-security-modules-fips-140-2-level-3--140-3-hsm)
   - [7.4 Real-Time Fraud & Anomaly Velocity Detection](#74-real-time-fraud--anomaly-velocity-detection)
8. [Chief Security Officer (CSO) Governance & HIPAA Safeguards Mapping](#8-chief-security-officer-cso-governance--hipaa-safeguards-mapping)
   - [8.1 HIPAA § 164.312 Technical Safeguards Mapping Matrix](#81-hipaa--164312-technical-safeguards-mapping-matrix)
   - [8.2 CSO Emergency Cryptographic Revocation Playbook (The 30-Second Kill-Switch)](#82-cso-emergency-cryptographic-revocation-playbook-the-30-second-kill-switch)
   - [8.3 Mortho Chief Security Officer Architectural Sign-Off](#83-mortho-chief-security-officer-architectural-sign-off)
9. [Operational Project Plan: DEV ➔ TEST ➔ QA ➔ PROD Roadmap](#9-operational-project-plan-dev--test--qa--prod-roadmap)
   - [9.1 Inexpensive Security & QA Testing Toolchain](#91-inexpensive-security--qa-testing-toolchain)
   - [9.2 Stage 1: DEV Stage (Local Workstation Setup — Exactly 10 Steps)](#92-stage-1-dev-stage-local-workstation-setup--exactly-10-steps)
   - [9.3 Stage 2: TEST Stage (Automated Testing, Playwright E2E & CI/CD Pipeline)](#93-stage-2-test-stage-automated-testing-playwright-e2e--cicd-pipeline)
   - [9.4 Stage 3: QA Stage (Azure Cloud QA, Playwright Validation & External Pen Testing)](#94-stage-3-qa-stage-azure-cloud-qa-playwright-validation--external-pen-testing)
   - [9.5 Stage 4: PROD Stage (Production Hardening, Playwright Verification & Go-Live)](#95-stage-4-prod-stage-production-hardening-playwright-verification--go-live)
   - [9.6 Critical Failure Traps & Real-World Engineering Mitigations](#96-critical-failure-traps--real-world-engineering-mitigations)
10. [High Availability, Multi-Region Failover & Incident Notification (Business Continuity)](#10-high-availability-multi-region-failover--incident-notification-business-continuity)
   - [10.1 Real-Time Incident Notifications & Azure Service Health](#101-real-time-incident-notifications--azure-service-health)
   - [10.2 Availability Zones (Zone-Redundant 99.99% SLA)](#102-availability-zones-zone-redundant-9999-sla)
   - [10.3 Multi-Region Automated Failover (Azure Front Door)](#103-multi-region-automated-failover-azure-front-door)
   - [10.4 Disaster Recovery Targets (RTO & RPO)](#104-disaster-recovery-targets-rto--rpo)

---

## 1. Executive Summary & Core Security Pillars

### 1.1 Project Objectives & High-Level Architecture
This document serves as the master architectural blueprint and project plan foundation for a high-security, HIPAA-compliant healthcare application at **Mortho Healthcare**. The platform transitions away from legacy SQL-based authentication (such as custom ASP.NET Zero or ASP.NET Core Identity database tables) to a **Cloud-Native Azure Ecosystem** leveraging **Microsoft Entra External ID**, **Azure Front Door Premium**, an **ASP.NET Core Backend-For-Frontend (BFF)**, and **Flutter** for cross-platform client delivery.

### 1.2 Key Architectural Pillars
1. **Zero Credentials in Application Database**: Passwords, hashes, and MFA secrets live strictly inside Microsoft Entra External ID.
2. **Passwordless Service Authentication**: All internal Azure services communicate via **Microsoft Entra Managed Identities** without hardcoded connection strings or API keys in configuration files.
3. **Sender-Constrained Tokens (DPoP - RFC 9449)**: Prevents stolen access tokens from being used on unauthorized client devices or IP addresses.
4. **Private Network Isolation (Azure Private Link)**: All backend services (App Service, SQL, Storage, Key Vault, Redis) have public internet access disabled and communicate over internal Azure Virtual Networks (VNet).
5. **Unified HIPAA Legal Coverage**: Every cloud component is hosted under a single **Microsoft HIPAA Business Associate Agreement (BAA)**.

---

### 1.3 Threat Model & Anti-Hacker / Anti-Attacker Defenses

To satisfy HIPAA Security Rule § 164.312 and protect sensitive electronic Protected Health Information (ePHI), the architecture incorporates explicit, multi-layered defenses against major hacker attack vectors:

```
[ ATTACK VECTOR ]                  [ DEFENSE-IN-DEPTH MECHANISM ]
─────────────────────────────────────────────────────────────────────────────────
1. Credential Stuffing / Spraying ──► Entra ID AI Risk Engine + AFD Bot Manager
2. Database Dump / Theft         ──► Zero Passwords in DB + CMK Key Vault Encryption
3. XSS Token Harvesting          ──► ASP.NET Core BFF HttpOnly SameSite Cookies
4. Man-in-the-Middle / Token Reuse──► Sender-Constrained DPoP Proofs (RFC 9449)
5. Config / Secret Leaks         ──► Entra Managed Identities (No DB Passwords)
6. Direct Port / IP Bypass       ──► Azure Private Link (Zero Public IPs on DB/Storage)
7. Log Tampering / Insider Threat ──► WORM Immutable Log Analytics + Sentinel SIEM
8. Cross-Tenant Data Leaks       ──► EF Core Global Query Filters + Azure SQL RLS
```

#### Detailed Breakdown of Hacker Countermeasures

* **Anti-Hacker Defense 1: Protection Against Credential Stuffing & Brute Force**
  * *Hacker Goal*: Use stolen passwords from third-party leaks to compromise patient or doctor accounts.
  * *Defensive Countermeasure*: Authentication is offloaded to **Microsoft Entra External ID**. Entra's threat intelligence engine evaluates billions of authentications daily to automatically detect breached passwords, block IP botnets, and enforce risk-based Adaptive MFA. **Azure Front Door WAF** rate-limits login endpoints before traffic reaches your servers.

* **Anti-Hacker Defense 2: Protection Against Database Dumps & SQL Injection (SQLi)**
  * *Hacker Goal*: Exploit application vulnerabilities to dump the SQL database or steal database backups.
  * *Defensive Countermeasure*: 
    1. **Zero Passwords in SQL**: The application database stores zero password hashes, security stamps, or login credentials. A complete database dump reveals no authentication credentials.
    2. **Envelope Encryption with Customer-Managed Keys (CMK)**: All data at rest in Azure SQL and Blob Storage is encrypted using master RSA keys stored in **Azure Key Vault**. Unencrypted data cannot be read from raw disk files.

* **Anti-Hacker Defense 3: Protection Against Cross-Site Scripting (XSS) Token Theft**
  * *Hacker Goal*: Inject malicious JavaScript into the browser to read `localStorage`/`sessionStorage` and steal OAuth access tokens.
  * *Defensive Countermeasure*: The **ASP.NET Core Backend-For-Frontend (BFF)** pattern stores OAuth tokens exclusively in server memory / Redis. Web browsers receive only an `HttpOnly`, `SameSite=Strict`, `Secure` session cookie. Browser JavaScript has **zero access to raw tokens**, neutralizing XSS token exfiltration.

* **Anti-Hacker Defense 4: Protection Against Man-in-the-Middle (MitM) & Token Replay**
  * *Hacker Goal*: Intercept an active bearer token over the network and replay it from an attacker-controlled machine.
  * *Defensive Countermeasure*: Implementation of **Sender-Constrained Tokens via DPoP (RFC 9449)**. Access tokens are cryptographically bound to the client's public key (Key Vault RSA key on BFF, or hardware iOS Keychain / Android KeyStore key on Mobile). Even if a bearer token is intercepted, it is **cryptographically useless** when replayed from a different machine.

* **Anti-Hacker Defense 5: Protection Against Source Code & Config Secret Leaks**
  * *Hacker Goal*: Scan GitHub repositories or server config files (`appsettings.json`) for hardcoded SQL passwords or API secrets.
  * *Defensive Countermeasure*: **Microsoft Entra Managed Identities**. C# code authenticates to Azure SQL, Key Vault, Storage, and Redis passwordlessly via Microsoft Entra tokens generated dynamically by the Azure host runtime. **Zero database passwords or API keys exist in source code or configuration files.**

* **Anti-Hacker Defense 6: Protection Against Direct Port & Public IP Scanning**
  * *Hacker Goal*: Bypass Azure Front Door WAF and attack backend App Services, SQL, or Storage directly via IP scanning.
  * *Defensive Countermeasure*: **Azure Private Link & Private Endpoints**. All backend services have public internet access completely disabled. They only accept traffic over internal Azure Virtual Networks (VNet) routed through Azure Front Door.

* **Anti-Hacker Defense 7: Protection Against Insider Threats & Audit Log Erasure**
  * *Hacker Goal*: Alter or erase system audit logs after exfiltrating patient data to cover tracks.
  * *Defensive Countermeasure*: Audit events stream directly to **Azure Log Analytics** configured with **WORM (Write Once Read Many) Immutability** and monitored by **Microsoft Sentinel SIEM**. Neither application developers nor Azure subscription admins have permission to edit or delete historical audit logs.

* **Anti-Hacker Defense 8: Protection Against Cross-Tenant & Cross-Organization Data Contamination**
  * *Hacker Goal*: Manipulate API parameters (e.g. changing `orgId=1` to `orgId=2`) to view another clinic's patient records.
  * *Defensive Countermeasure*: Dual-tier data isolation:
    1. **EF Core Global Query Filters**: Automatically append `WHERE TenantId = @TenantId AND OrgId = @OrgId` to every LINQ query at the application layer.
    2. **Azure SQL Row-Level Security (RLS)**: Database engine security predicates block unauthorized cross-tenant row access even if application code is bypassed.

---

### 1.4 Strategic Business Case: Why ASP.NET Zero / ABP.io Renewal & Upgrade Will Not Work

#### The Financial & Commercial Reality
* **Escalating Subscription Costs**: ASP.NET Zero pricing has increased significantly to **$2,300+ annually per developer seat** and is being merged into the commercial **ABP.io** framework ecosystem.
* **Low Return on Investment (ROI)**: Renewing subscription licensing for an application template framework delivers zero infrastructure security, zero HIPAA compliance coverage, and zero cloud hosting automation out of the box.

#### The Technical & Security Failure of Upgrading ASP.NET Zero / ABP.io

| Security & Architectural Requirement | Legacy / Upgraded ASP.NET Zero & ABP.io | Cloud-Native Azure + Entra ID + BFF Architecture |
| :--- | :--- | :--- |
| **Password & Credential Storage** | ❌ **Stored in SQL Database (`AbpUsers`)**. Upgrading to latest ABP releases still leaves password hashes co-located in the application DB. | ✅ **Zero passwords in DB**. 100% offloaded to Microsoft Entra External ID. |
| **HIPAA BAA Legal Coverage** | ❌ **No BAA**. ASP.NET Zero is software code, not a cloud service. It cannot sign a BAA. | ✅ **Unified BAA**. Signed directly with Microsoft covering all 15 Azure services. |
| **XSS Token Theft Protection** | ❌ **Tokens stored in browser `localStorage`**. Vulnerable to script injection token theft. | ✅ **ASP.NET Core BFF**. Tokens stored in server memory; browser JavaScript gets `HttpOnly` cookie. |
| **Sender-Constrained Tokens** | ❌ **No native DPoP support**. Stolen bearer tokens can be replayed from any device. | ✅ **Turnkey DPoP (RFC 9449)**. Cryptographically binds tokens to hardware keys. |
| **Secret & Config Management** | ❌ Connection strings & DB passwords stored in `appsettings.json`. | ✅ **Entra Managed Identities**. Passwordless service-to-service authentication. |
| **Network & Edge Defense** | ❌ Requires custom WAF, manual server hardening, and public IP management. | ✅ **Azure Front Door Premium + Private Link**. Zero public IPs on database or storage. |

#### Strategic Recommendation: Migrate Rather Than Upgrade
1. **Upgrading Will Not Solve Fundamental Vulnerabilities**: Our current codebase is two releases behind. However, performing a complex codebase migration to the newest ASP.NET Zero / ABP.io version **will not resolve core security vulnerabilities** (SQL credential storage, lack of HIPAA BAA, browser token exposure).
2. **Expending Migration Effort Wisely**: Since migrating off a two-release-old ASP.NET Zero codebase requires development effort regardless, that effort is far better spent transitioning to an enterprise-grade **Cloud-Native Azure + Entra ID + BFF + Flutter** platform.
3. **Eliminating Recurring Licensing Fees**: By replacing commercial framework templates with native .NET 10, EF Core 10, and Azure services, we eliminate recurring $2,300/dev framework license costs while achieving strict HIPAA compliance.

---

### 1.5 The 5-Layer API Defense-in-Depth Protection Shield

To guarantee that Mortho's application programming interfaces (APIs) are fully protected against unauthorized access, data extraction, and security breaches, the architecture enforces a **5-Layer API Shield**:

```
[ UNTRUSTED INTERNET / ATTACKER ]
               │
               ▼
 🛡️ LAYER 1: AZURE FRONT DOOR PREMIUM (Edge WAF & DDoS)
               │ Blocks SQLi, XSS, Botnets, & DDoS before hitting your servers
               ▼
 🛡️ LAYER 2: AZURE PRIVATE LINK (Network Isolation)
               │ Removes Public IPs — API does NOT exist on the public internet
               ▼
 🛡️ LAYER 3: DPoP SENDER-CONSTRAINED TOKENS (RFC 9449)
               │ Validates cryptographic token proofs bound to hardware keys
               ▼
 🛡️ LAYER 4: ENTRA MANAGED IDENTITIES (Passwordless Service Auth)
               │ API talks to SQL & Key Vault with ZERO passwords in config files
               ▼
 🛡️ LAYER 5: APPLICATION AUTHORIZATION (RBAC + ReBAC + ABAC + SQL RLS)
               │ Enforces Tenant, Org, Doctor-Patient CareTeam, and Row-Level Security
               ▼
[ SECURE PATIENT ePHI DATA ]
```

#### Detailed Layer Responsibilities

1. **🛡️ Layer 1: Edge WAF (Azure Front Door Premium)**
   * Inspects every HTTP request hitting `https://api.mortho.com` at Microsoft's global edge network.
   * Filters out OWASP Top 10 threats (SQLi, XSS, CSRF, HTTP Request Smuggling) and rate-limits IP connections to prevent brute force before traffic ever reaches app servers.
2. **🛡️ Layer 2: Network Isolation (Azure Private Link & Private Endpoints)**
   * Completely disables public internet IP addresses on Azure App Service, Azure SQL, Azure Storage, Key Vault, and Redis.
   * The API and database **literally do not exist on the public internet** and can only be reached over internal private Azure Virtual Networks (VNet).
3. **🛡️ Layer 3: Identity & DPoP Token Binding (RFC 9449)**
   * Verifies the OAuth 2.0 Access Token signature issued by Microsoft Entra External ID.
   * Enforces DPoP (RFC 9449) cryptographic proof verification. If a token is stolen, it is **cryptographically useless** when replayed from any other machine.
4. **🛡️ Layer 4: Passwordless Infrastructure Access (Entra Managed Identities)**
   * When C# API endpoints query Azure SQL, Key Vault, or Storage, authentication occurs via **Managed Identities**.
   * Zero database passwords or secret keys exist in `appsettings.json` or source control.
5. **🛡️ Layer 5: Data-Level Authorization (RBAC + ReBAC + ABAC + SQL RLS)**
   * Evaluates user App Roles (RBAC), Doctor-Patient CareTeam relationships (ReBAC via Redis/SQL), and dynamic runtime conditions (ABAC).
   * Enforces EF Core Global Query Filters and Azure SQL Row-Level Security (RLS) so users can **never** access another organization's patient data.

---

## 2. Technology Stack & Framework Choices

### 2.1 .NET 10 & ASP.NET Core Execution Engine
* **Target Framework**: `.net10.0` (LTS release).
* **C# 14 Features**: Enhanced pattern matching, primary constructors, extension members, and optimized Span/ReadOnlySpan memory management for zero-allocation HTTP request handling.
* **Minimal APIs & Controllers**:
  * **BFF Tier**: Standard ASP.NET Core Controllers with `Duende.BFF` or custom OIDC cookie authentication handlers.
  * **Core API Tier**: High-performance Minimal APIs utilizing Native AOT compilation options for ultra-fast startup times and reduced memory footprints in container environments.

### 2.2 Entity Framework Core 10 (EF Core 10) & Data Isolation
* **ORM Strategy**: EF Core 10 with Azure SQL Database.
* **Multi-Tenancy & Data Isolation**:
  * **Global Query Filters**: Automatically append `WHERE TenantId = @CurrentTenantId AND OrgId = @CurrentOrgId` to every LINQ query, eliminating developer error in multi-tenant data leaks.
  * **Row-Level Security (RLS)**: Enforces organization data isolation directly in Azure SQL database engine as a secondary layer of defense.
* **Audit Interceptors**: EF Core `SaveChangesInterceptor` automatically captures `CreatedBy`, `CreatedAt`, `LastModifiedBy`, `LastModifiedAt`, and writes immutable audit entries to the audit trail before committing transactions.
* **Performance Tuning**: Compiled queries (`EF.CompileAsyncQuery`) for hot paths (e.g., patient lookup, permission checks), `AsNoTracking()` for read-only APIs, and Split Queries (`AsSplitQuery()`) for complex 1-to-many relationship graphs.

### 2.3 Caching Architecture: Azure Cache for Redis vs. In-Memory
* **In-Memory Caching (`IMemoryCache`)**:
  * *Use Case*: Local development, static lookup data (e.g., medical code sets, ICD-10 state tables), and single-instance local caching.
  * *Limitation*: Does not sync across multiple scaled instances of Azure App Service / Container Apps.
* **Azure Cache for Redis (Enterprise Tier)**:
  * *Use Case*: **Production Requirement**.
  * **ReBAC Permission Cache**: Caches complex relationship permissions (e.g., *Is Dr. Smith authorized to view Dr. Miller's patient right now?*) in RAM with expiration TTLs (e.g., 5 minutes), keeping permission check latencies under **1 millisecond**.
  * **Distributed Rate Limiting**: Tracks IP and user request rates across all scaled App Service instances to prevent brute force or denial-of-service attempts.
  * **BFF Server Session Store**: Stores encrypted user session state across auto-scaled web instances.

### 2.4 gRPC & Protocol Buffers (Google) for High-Speed Microservices
* **Protocol**: gRPC over HTTP/2 using binary Protocol Buffers (Protobuf).
* **Use Case**: Internal service-to-service communication between the ASP.NET Core BFF and downstream Core APIs / Medical Data Microservices.
* **Benefits**: 
  * **7x to 10x Faster than REST/JSON**: Binary serialization decreases CPU overhead and network payload size by up to 80%.
  * **Binary Data Streaming**: Supports native bidirectional streaming for heavy medical images (DICOM) and patient telemetry.
  * **Contract-First Code Generation**: `.proto` definitions serve as the single source of truth for C# and Flutter clients.

### 2.5 OpenFGA / OPA (CNCF Zanzibar Model) for Relationship Authorization
* **Technology**: **OpenFGA** (Fine-Grained Authorization, open-source CNCF project based on Google's *Zanzibar* paper).
* **Use Case**: Advanced Relationship-Based Access Control (ReBAC).
* **Benefits**: Elegantly models complex healthcare access relationships (e.g., *"Is Dr. Smith allowed to read Patient X's record because Dr. Smith is on Dr. Miller's coverage list for Hospital Y?"*) in a fast, graph-based authorization engine.

### 2.6 OpenTelemetry & Polly for Observability & Fault Tolerance
* **OpenTelemetry (CNCF Standard)**: Vendor-neutral distributed tracing, metrics, and logging framework. Integrates seamlessly with Azure Application Insights, Jaeger, or Datadog.
* **Polly (.NET Resilience Library)**: Implements Circuit Breaker, Exponential Backoff, Retry, and Bulkhead Isolation policies for internal HTTP and gRPC inter-service communication to guarantee high availability.

### 2.7 JetBrains Qodana & Snyk for DevSecOps Security Scanning
* **JetBrains Qodana Ultimate Plus**: Static Application Security Testing (SAST) engine required prior to QA cloud deployment. Operates across both C# .NET 10 and Flutter/Dart codebases.
* **Snyk / GitHub Dependency Graph**: Software Bill of Materials (SBOM) and Software Composition Analysis (SCA) to detect known CVE vulnerabilities in third-party packages automatically.

#### Why Qodana Ultimate Plus is Needed for this Healthcare Architecture
While basic static analysis checks for standard syntax bugs, a HIPAA-compliant healthcare application handling sensitive Protected Health Information (ePHI) requires the advanced enterprise capabilities exclusive to **Qodana Ultimate Plus**:

1. **Advanced Taint Analysis (Data Exfiltration & Injection Defense)**:
   - *Requirement*: HIPAA Technical Safeguards § 164.312(c)(1) (Data Integrity) and § 164.312(e)(1) (Transmission Security).
   - *Mechanism*: Taint analysis tracks the exact flow of untrusted user input from HTTP API endpoints through business logic handlers down to EF Core database queries, gRPC payloads, and Azure Blob Storage uploads.
   - *Impact*: Automatically detects potential SQL injection (SQLi), Cross-Site Scripting (XSS), and accidental ePHI logging/exfiltration vulnerabilities before code merges into QA/PROD.

##### Deep-Dive: How Advanced Taint Analysis Protects This Healthcare Architecture

**Advanced Taint Analysis** treats any external data entering from an untrusted boundary as "tainted" (potentially dangerous) and tracks its movement across variables, methods, and classes to ensure it never reaches a sensitive execution point (a "sink") without sanitization.

```
[ 1. TAINT SOURCE ] ──────────► [ 2. DATA PROPAGATION ] ──────────► [ 3. SECURITY SINK ]
Untrusted User Input             Moves through DTOs, Methods,        Dangerous Execution Point
(HTTP Query, Form, Header)        & Services across layers            (SQL, Logger, HTML, File System)
                                                                               │
                                                                 ⚠️ ALERTS IF UNSANITIZED!
```

* **The 3-Part Taint Analysis Lifecycle**:
  1. **Taint Source**: Entry points where external data enters the system (HTTP parameters, JSON bodies, headers, Flutter form inputs, external webhooks).
  2. **Data Propagation**: As C# or Dart code passes values across DTOs, domain services, or helper functions, Qodana tracks the spread of untrusted data across files and layers.
  3. **Security Sink**: Critical execution or storage points where unvalidated data can trigger security breaches (raw SQL queries, system loggers, HTML rendering engines).

* **Concrete Healthcare Security Scenarios Guarded by Taint Analysis**:
  - **Scenario A: Accidental ePHI Exfiltration in Audit Logs (HIPAA § 164.312(b))**:
    - *Flow*: Patient inputs SSN/MRN ➔ Passed into `PatientRegistrationService` ➔ Executed via `_logger.LogInformation("Processing {SSN}", ssn)`.
    - *Taint Alert*: Qodana flags sensitive ePHI reaching an unencrypted logging sink before code merges.
  - **Scenario B: Indirect SQL Injection in Medical Record Search**:
    - *Flow*: User inputs search string ➔ Passed across 3 helper methods ➔ Executed via `ExecuteSqlRaw($"WHERE MRN = '{mrn}'")`.
    - *Taint Alert*: Qodana detects raw user string reaching a database execution sink without SQL parameterization.
  - **Scenario C: Cross-Site Scripting (XSS) in Doctor Web Dashboard**:
    - *Flow*: Patient uploads clinical notes with malicious `<script>` ➔ Saved in Azure SQL ➔ Rendered in Doctor Web Dashboard.
    - *Taint Alert*: Qodana detects stored untrusted string reaching an HTML DOM rendering sink without output encoding.

2. **Automated License Compliance Auditing**:
   - *Requirement*: Enterprise DevSecOps Governance and Software Bill of Materials (SBOM) verification.
   - *Mechanism*: Automatically audits all third-party open-source licenses across C# NuGet packages and Flutter/Dart `pubspec` dependencies.
   - *Impact*: Prevents copyleft or GPL-restricted licenses from entering the codebase, protecting Mortho Healthcare from legal compliance liability.

3. **Permanent Historical Security Audit Trail**:
   - *Requirement*: CSO Governance Sign-Off (§ 8.3) and HIPAA § 164.312(b) Audit Controls.
   - *Mechanism*: Unlimited historical data retention in Qodana Cloud.
   - *Impact*: Provides Chief Security Officer (CSO) and external HIPAA compliance auditors with immutable historical reports proving continuous security quality gates were enforced on every release.

### 2.8 Infrastructure as Code (IaC) — Microsoft Bicep
* **Standard**: All Azure cloud infrastructure (Azure Front Door, App Service, Azure SQL, Key Vault, Storage, Redis, Log Analytics) is defined and provisioned strictly via **Microsoft Bicep (`.bicep`)** scripts.
* **Key Advantages**:
  * **Microsoft Native**: 100% supported by Microsoft out of the box with zero state-file management overhead.
  * **Identical Environments**: Ensures `rg-mortho-qa` and `rg-mortho-prod` are 100% identical in security configuration.
  * **Automated CI/CD Deployment**: Executed natively via Azure CLI in GitHub Actions pipelines.

### 2.9 Database Simplification & De-Bloating Strategy (ASP.NET Zero 35+ Tables vs Cloud-Native 7 Tables)

```
ASP.NET ZERO DATABASE:
[ 35+ Complex Framework Tables ] ──► Slow, bloated, stores passwords, hard to maintain

NEW CLOUD-NATIVE DATABASE:
[ ~7 Clean Domain & ReBAC Tables ] ──► Blazing fast, zero passwords, ultra-lean!
```

#### Framework Tables Eliminated (~35+ Tables Removed)
* **Identity & Credential Tables Removed**: `AbpUsers`, `AbpUserRoles`, `AbpUserClaims`, `AbpUserLogins`, `AbpUserTokens`, `AbpRoleClaims`, `AbpRoles`, `AbpUserLoginAttempts`, `AppRecentPasswords`, `AppUserDelegations`. *(Offloaded 100% to Microsoft Entra External ID)*.
* **Audit & Logging Tables Removed**: `AbpAuditLogs`, `AbpEntityChanges`, `AbpEntityChangeSets`, `AbpEntityPropertyChanges`. *(Offloaded to Azure Log Analytics WORM Immutable Vault & Sentinel SIEM)*.
* **Notification & Job Tables Removed**: `AbpNotifications`, `AbpNotificationSubscriptions`, `AbpTenantNotifications`, `AbpUserNotifications`, `AbpBackgroundJobs`, `AbpWebhookEvents`. *(Offloaded to Azure Service Bus & Azure Communication Services)*.

#### The ONLY 7 Clean Tables Required in Application Database

1. **`Tenants`**: (`Id`, `Name`, `IsActive`, `CreatedAt`) — Multi-tenant system top level.
2. **`Organizations`**: (`Id`, `TenantId`, `Name`, `FacilityLicenseNumber`, `IsActive`) — Hospital / Clinic organizations.
3. **`Users`**: (`Id`, `ExternalId` [Entra GUID `oid`], `TenantId`, `OrgId`, `Email`, `FullName`, `IsActive`) — Zero passwords, zero hashes, zero MFA keys!
4. **`DoctorPatientAssignments`**: (`Id`, `DoctorUserId`, `PatientUserId`, `AssignedAt`, `IsActive`) — ReBAC relationship mapping.
5. **`CareTeamMembers`**: (`CareTeamId`, `OrgId`, `UserId`, `RoleInTeam`) — ReBAC care team mapping.
6. **`DoctorCoverage`**: (`Id`, `CoveringDoctorUserId`, `PrimaryDoctorUserId`, `StartDate`, `EndDate`) — ReBAC coverage rule.
7. **`Patients`**: (`Id`, `TenantId`, `OrgId`, `UserId`, `MRN`, `DOB`, `Gender`) — Product clinical data.

---

## 3. Master List of 15 Azure / Microsoft Components

### 3.1 System Architecture Flow Diagram

```
[ PATIENT / DOCTOR CLIENTS ]
         │
         ▼
 1. AZURE FRONT DOOR PREMIUM (Edge WAF / DDoS / Anycast CDN)
         │
         ├──► 2. MICROSOFT ENTRA EXTERNAL ID (IdP / OIDC / MFA / SSPR)
         │
         ▼ (7. Private Link / Internal VNet)
 4. AZURE APP SERVICE / CONTAINER APPS (ASP.NET Core BFF & APIs)
         │
         ├──► 3. MANAGED IDENTITIES (Passwordless C# Auth to Azure Services)
         ├──► 5. AZURE CONTAINER REGISTRY (ACR + Defender Vulnerability Scanning)
         ├──► 6. AZURE KEY VAULT (7. Customer-Managed Keys / CMK + Soft Delete)
         ├──► 8. AZURE SQL DATABASE (TDE + Row-Level Security + CMK)
         ├──► 9. AZURE BLOB STORAGE (CMK Encrypted + User-Delegated SAS Links)
         ├──► 10. AZURE SIGNALR SERVICE (Encrypted Real-Time WebSockets)
         ├──► 11. AZURE COMMUNICATION SERVICES (HIPAA Email & SMS Notifications)
         ├──► 12. AZURE SERVICE BUS (Async Background Task Queues)
         ├──► 13. AZURE CACHE FOR REDIS (Sub-millisecond ReBAC Permission Caching)
         ├──► 14. AZURE LOG ANALYTICS & SENTINEL (WORM Immutable Audit Trail)
         └──► 15. AZURE APPLICATION INSIGHTS (APM Telemetry & Distributed Tracing)
```

### 3.2 Detailed Component Matrix (WHEN / WHERE / WHAT)

| # | Component | Category | WHEN is it used? | WHERE does it sit? | WHAT exact task does it do? |
| :-: | :--- | :--- | :--- | :--- | :--- |
| **1** | **Azure Front Door Premium** | Edge Security | Every incoming HTTP request. | Global Edge Network (in front of domain). | Layer 7 WAF, DDoS mitigation, Bot management, SSL termination, and Anycast routing. |
| **2** | **Microsoft Entra External ID** | Identity & Auth | When users log in, MFA, sign up, or reset passwords. | External Microsoft Identity Provider (IdP). | Manages user accounts, passwords, MFA, SSPR, and issues OIDC/OAuth2 tokens. |
| **3** | **Entra Managed Identities** | Service Security | Whenever C# backend calls Azure SQL, Key Vault, or Storage. | Internal Azure IAM mechanism. | Passwordless service-to-service auth. Eliminates hardcoded DB passwords/connection strings. |
| **4** | **Azure App Service / Container Apps** | Compute | When executing BFF routes, business logic, and APIs. | Private Azure Virtual Network (VNet). | Runs C# code / Docker containers with auto-scaling and OS security patching. |
| **5** | **Azure Container Registry (ACR)** | CI/CD & Security | During software deployment & container builds. | Private Azure Container Repository. | Stores Docker images and automatically scans OS/runtime for security vulnerabilities. |
| **6** | **Azure Key Vault (Premium / HSM)** | Cryptography | App startup & key fetch events. | FIPS 140-2 Level 3 Hardware Vault. | Stores DPoP private RSA keys, TLS certs, and master encryption keys. |
| **7** | **Customer-Managed Keys (CMK)** | Data Encryption | Every read/write to Azure SQL or Storage at rest. | Key Vault & Encryption Engine layer. | Master encryption key owned by you. Provides an instant cryptographic kill-switch. |
| **8** | **Azure SQL Database** | Data Layer | Querying Tenants, Orgs, Users, Care Teams. | Private Endpoint isolated database. | Stores relational data with TDE (CMK) and Row-Level Security (RLS). |
| **9** | **Azure Blob Storage** | Document Storage | Uploading/viewing lab PDFs, medical images. | Private Endpoint encrypted object storage. | Stores files encrypted with CMK; grants access via short-lived (5-min) SAS URLs. |
| **10** | **Azure SignalR Service** | Messaging | Real-time doctor chat & live alerts. | Managed WebSocket service in Azure. | Encrypted WebSocket handling without maintaining persistent sockets on web servers. |
| **11** | **Azure Communication Services (ACS)** | Notifications | Sending appointment reminders, SMS, or email. | Managed communications gateway under BAA. | Sends HIPAA-compliant email and SMS notifications directly to patients. |
| **12** | **Azure Service Bus** | Async Processing | Background PDF generation, batch processing. | Enterprise message queue. | Offloads long-running tasks into async queues so HTTP requests stay fast. |
| **13** | **Azure Cache for Redis** | Caching & Speed | On every request checking complex ReBAC rules. | In-memory RAM database in Azure VNet. | Caches relationship permissions in RAM for sub-millisecond check times. |
| **14** | **Azure Log Analytics + Sentinel** | Audit & SIEM | Continuous 24/7 background logging. | WORM Immutable Log Repository. | Maintains an immutable HIPAA audit trail (§ 164.312(b)) with AI anomaly detection. |
| **15** | **Azure Application Insights** | APM & Telemetry | Continuous background performance tracking. | Telemetry engine attached to App Service. | Distributed tracing across Front Door ➔ BFF ➔ APIs ➔ SQL to diagnose crashes/slowness. |

---

## 4. Identity & Authorization Hierarchy (Tenant ➔ Org ➔ User)

### 4.1 Multi-Tenant Hierarchy Mapping

```
🏢 TENANT (System Top-Level / Your SaaS Platform)
   │
   ├── 🏥 ORGANIZATION A ("Mayo Clinic")
   │      ├── 👨‍⚕️ User 1 (Dr. Smith - Doctor Role)
   │      └── 👩‍⚕️ User 2 (Nurse Johnson - Staff Role)
   │
   └── 🏥 ORGANIZATION B ("City Health Clinic")
          ├── 👨‍⚕️ User 3 (Dr. Davis - Doctor Role)
          └── 🤒 User 4 (John Doe - Patient Role)
```

### 4.2 Authorization Layering: RBAC + ReBAC + ABAC

To enforce strict, granular security boundaries, access decisions are layered across three distinct authorization models:

| Authorization Model | WHERE does it live in Azure / Application? | How is it managed & configured? | C# / Policy Code Evaluation Example |
| :--- | :--- | :--- | :--- |
| **RBAC** *(Role-Based)* | **Microsoft Entra ID** *(Azure App Roles)* | Defined in Azure Portal under Entra ID App Registrations ➔ App Roles (`Doctor`, `Nurse`, `Patient`, `Admin`). | Entra embeds role claims in JWT. Evaluated in C# via `[Authorize(Roles = "Doctor")]`. |
| **ReBAC** *(Relationship-Based)* | **Azure Cache for Redis + Azure SQL** *(or OpenFGA)* | Managed in App DB tables (`CareTeams`, `DoctorCoverage`) or OpenFGA relationship tuple graph. | Evaluates if Dr. Smith is assigned to Dr. Miller's patient list. Results cached in Redis (<1ms). |
| **ABAC** *(Attribute-Based)* | **ASP.NET Core Policy Engine** *(C# Handlers)* | Configured in C# Custom Authorization Policy Handlers (`IAuthorizationHandler`). | Evaluates runtime attributes: `IsOnCallHours == true`, `Department == "Oncology"`, `IsEmergencyOverride == true`. |

#### Detailed Layer Breakdown:
1. **RBAC (Role-Based Access Control)**:
   * *Source*: **Microsoft Entra ID App Roles**.
   * *Mechanism*: Entra embeds roles in ID/Access tokens. Evaluated via `[Authorize(Roles = "Doctor")]`.
2. **ReBAC (Relationship-Based Access Control)**:
   * *Source*: **App Database (`CareTeams`, `DoctorCoverage`) + Azure Cache for Redis** (or **OpenFGA engine**).
   * *Mechanism*: Evaluates relationships. *Example*: Dr. Smith can read Dr. Miller's patient records IF Dr. Smith is currently assigned to Dr. Miller's coverage list in the `DoctorCoverage` table.
   * *Optimization*: Permission decisions are cached in **Azure Cache for Redis** with a 5-minute TTL to keep queries under 1ms.
3. **ABAC (Attribute-Based Access Control)**:
   * *Source*: **ASP.NET Core Authorization Policy Handlers** evaluating dynamic runtime context.
   * *Mechanism*: Evaluates attributes such as `IsOnCallHours == true`, `Department == "Oncology"`, or `IsEmergencyBreakTheGlass == true`.

---

### 4.3 User Onboarding & Provisioning Architecture (Graph API Integration)

User creation in the new architecture replaces complex custom ASP.NET Zero user tables with three streamlined onboarding flows:

#### 1. Patient Self-Registration (Zero Custom Code Needed)
* **Flow**: Patients register directly via Microsoft Entra External ID hosted sign-up pages.
* **Security**: Entra handles email verification via One-Time Passcode (OTP), password creation, and MFA enrollment out of the box.
* **Auto-Provisioning**: Upon first successful login, the BFF auto-provisions a local lightweight user record in the App DB containing their Entra GUID (`ExternalId`), name, email, and assigned tenant.

#### 2. Admin Inviting Doctors & Clinic Staff (Microsoft Graph API)
* **Flow**: Admins use a simplified 1-step web form inside the app (`Email`, `Full Name`, `Role`). **No password field is ever collected or processed by your app.**
* **C# Backend Logic**: The ASP.NET Core backend calls **Microsoft Graph API** to create the user invitation:

```csharp
// C# Backend Endpoint: POST /api/admin/invite-user
public async Task<IResult> InviteDoctorAsync(InviteDoctorRequest request, GraphServiceClient graphClient)
{
    var invitation = new Invitation
    {
        InvitedUserEmailAddress = request.Email,
        InvitedUserDisplayName = request.FullName,
        SendInvitationMessage = true,
        InviteRedirectUrl = "https://app.mortho.com/welcome"
    };

    var result = await graphClient.Invitations.PostAsync(invitation);
    
    // Auto-create local user record in App DB with OrgId
    await _userRepository.CreateLocalUserAsync(new User {
        ExternalId = result.InvitedUser.Id,
        Email = request.Email,
        OrgId = request.OrgId
    });

    return Results.Ok(new { Message = "Invitation sent successfully." });
}
```

* **User Experience**: Microsoft emails a secure invitation link to the doctor. The doctor sets her own password securely on Microsoft's portal and completes login.

#### 3. IT Direct Management (Entra Admin Center)
* **Flow**: Internal Mortho IT administrators manage users, assign directory roles, or suspend accounts directly inside the **Microsoft Entra Admin Center** (`entra.microsoft.com`).

---

## 5. Token Management & Security: Web vs. Flutter Mobile

### 5.1 Web Architecture (ASP.NET Core BFF + DPoP)
* **Token Location**: Tokens live strictly in **BFF Server Memory / Redis**.
* **Browser Storage**: **HttpOnly, SameSite=Strict, Secure Cookies** only. Browser JavaScript never sees raw OAuth tokens, completely neutralizing Cross-Site Scripting (XSS) token theft.
* **Token Proofs**: BFF binds downstream API requests with **DPoP (RFC 9449)** headers using ECDSA (ES256) key pairs stored in Azure Key Vault.

### 5.2 Flutter Mobile Architecture (iOS Keychain / Android KeyStore + Biometrics)
* **OAuth Flow**: Authorization Code Flow + PKCE via `flutter_appauth` opening system webview (`ASWebAuthenticationSession` / `Custom Tabs`).
* **Hardware Storage**: Tokens are written to hardware-backed secure storage using `flutter_secure_storage`:
  * **iOS**: Encrypted inside the **iOS Keychain** (Apple Secure Enclave).
  * **Android**: Encrypted via **Android KeyStore** (`EncryptedSharedPreferences`).
* **Hardware DPoP Proofs**: Flutter generates a unique ECDSA key pair in the device's hardware KeyStore on first launch, signing every outgoing HTTP request header with a short-lived DPoP proof.
* **HIPAA Biometric Lock**: Uses `local_auth` package to prompt for **FaceID / TouchID / Fingerprint** whenever the app is resumed from the background before displaying patient data.

---

## 6. Implementation Sequence & Environment Strategy

To build and validate this architecture systematically, follow this three-phase development and deployment roadmap.

```
+-----------------------------------------------------------------------------------+
|                        PHASE 1: LOCAL DEVELOPMENT (PC)                            |
|  - Docker Desktop + .NET 10 SDK + .NET Aspire                                     |
|  - Azurite (Blob Emulator) + SQL Server Container + Redis Container               |
|  - Microsoft Entra ID Developer Tenant                                            |
+-----------------------------------------------------------------------------------+
                                          │
                                          ▼ CI/CD Pipeline (GitHub Actions)
+-----------------------------------------------------------------------------------+
|                        PHASE 2: QA ENVIRONMENT (Azure Cloud)                      |
|  - Azure Resource Group: rg-medical-qa                                            |
|  - Azure Front Door (qa.medical.com) + App Service + Azure SQL + Key Vault        |
|  - Cloud-native testing & security scanning                                       |
+-----------------------------------------------------------------------------------+
                                          │
                                          ▼ Approval Gate
+-----------------------------------------------------------------------------------+
|                     PHASE 3: PRODUCTION ENVIRONMENT (Azure Enterprise)            |
|  - Azure Resource Group: rg-medical-prod                                          |
|  - CMK Enabled + Private Link VNet Isolation + Sentinel SIEM                      |
+-----------------------------------------------------------------------------------+
```

---

### Phase 1: Local Development Environment (PC Setup)

#### Prerequisites on Local Machine:
1. **.NET 10 SDK** installed.
2. **Docker Desktop** (or Podman) running.
3. **IDE**: Visual Studio 2026 / VS Code / JetBrains Rider.
4. **Microsoft Entra ID Free Developer Tenant**: Created via Microsoft 365 Developer Program.

#### Step-by-Step Local Developer Sequence:
1. **Repository Setup**:
   * Clone repository `Auth0BffDpopApi`.
   * Configure `.NET Aspire` (`AppAspireHost`) to orchestrate local dependencies.
2. **Local Infrastructure Containers (via Aspire / Docker Compose)**:
   * **SQL Server**: Local container `mcr.microsoft.com/mssql/server:2022-latest`.
   * **Redis**: Local container `redis:alpine` for permission caching.
   * **Azure Storage Emulator**: **Azurite** container for local blob testing.
3. **Entra ID App Registration Setup**:
   * Register App in Entra Developer Tenant: `MedicalApp-Local-BFF`.
   * Redirect URI: `https://localhost:7001/signin-oidc`.
   * Configure API Permissions: `openid`, `profile`, `email`, `offline_access`.
4. **Local Certificate & Key Generation**:
   * Run local certificate generator (`GenerateCertificate` project) to create local DPoP testing keys (`ecdsa256-private.pem` & `ecdsa256-public.pem`).
   * Store local secrets in .NET `user-secrets` (never committed to git):
     ```bash
     dotnet user-secrets set "AzureAd:ClientId" "your-entra-client-id" --project bff/server
     dotnet user-secrets set "AzureAd:TenantId" "your-entra-tenant-id" --project bff/server
     ```
5. **Run & Debug Locally**:
   * Start `AppAspireHost`. Aspire launches BFF (`https://localhost:7001`), Core API (`https://localhost:7002`), SQL, Redis, and Azurite automatically.
   * Run Flutter app targeting `https://localhost:7001`.

---

### Phase 2: QA Environment on Azure (Cloud-Native QA)

The QA environment **must run on actual Azure cloud infrastructure** (not local PC) to validate Azure Front Door WAF rules, Entra ID tokens, Private Link networking, and managed identity permissions before going near Production.

#### Step-by-Step QA Setup Sequence:
1. **Create Azure Resource Group**:
   * Name: `rg-medical-qa-eastus`
2. **Provision QA Infrastructure (via Infrastructure as Code - Microsoft Bicep)**:
   * **Azure Front Door (Standard/Premium)**: Pointed to domain `qa.medical.com`.
   * **Azure App Service (B1 / P1v3 tier)**: Configured with Linux runtime for BFF & APIs.
   * **Azure SQL Database (S1 / Serverless tier)**: QA database populated with sanitized test data.
   * **Azure Key Vault**: Stores QA TLS certificates and DPoP keys.
   * **Azure Storage Account**: For QA document uploads.
3. **Configure Managed Identities in QA**:
   * Enable System-Assigned Managed Identity on App Service `app-medical-bff-qa`.
   * Grant `app-medical-bff-qa` the **Azure SQL DB Data Contributor** role and **Key Vault Secrets User** role.
   * Remove SQL admin passwords from App Service connection strings!
4. **Automate Deployment via CI/CD (GitHub Actions / Azure DevOps)**:
   * **On Git Push to `main` or `release/qa` branch**:
     1. Run `dotnet test` (Unit & Integration Tests).
     2. Build Docker Container images for BFF and API.
     3. Push images to **Azure Container Registry (ACR)** (`acr-medical-qa`).
     4. ACR Defender automatically scans images for vulnerabilities.
     5. Deploy images to QA App Service via Bicep template.
5. **Execute QA Verification Suite**:
   * Perform end-to-end authentication tests via Entra ID QA Tenant.
   * Test WAF rules on Azure Front Door (verify SQLi and XSS payloads are blocked at edge).
   * Test Flutter Mobile & Web builds against `https://qa.medical.com`.

---

### Phase 3: Production Environment (Azure Enterprise)

Once QA testing passes all verification gates, deploy to `rg-medical-prod-eastus` using Microsoft Bicep:

1. Enable **Customer-Managed Keys (CMK)** on Azure SQL and Azure Storage.
2. Enable **Purge Protection** and **Soft Delete** on Key Vault.
3. Attach **Azure Private Endpoints** to SQL, Storage, Key Vault, and Redis—disabling all public internet access.
4. Enable **WORM Immutability** on Log Analytics and connect **Microsoft Sentinel** for 24/7 automated HIPAA compliance monitoring.
5. Execute final penetration testing and sign off on the single **Microsoft HIPAA BAA**.

---

## 7. Bank-Grade Financial Security Standards (FAPI 2.0 & High-Assurance Controls)

To achieve a security posture equivalent to **banking-grade financial settlement systems** (SWIFT, FedWire, ISO 20022 interbank networks), the architecture incorporates the **Financial-Grade API (FAPI 2.0)** security framework:

### 7.1 Financial-Grade API (FAPI 2.0) Protocol Alignment
* **Pushed Authorization Requests (PAR - RFC 9126)**: OAuth parameters (redirect URIs, scopes, state) are pushed directly to Entra ID over authenticated back-channel HTTPS calls, preventing parameter tampering on front-channel browser URLs.
* **Sender-Constrained Token Enforcement (DPoP / mTLS)**: Every API request MUST include a cryptographic proof header bound to the client’s public key. Unbound bearer tokens are rejected automatically at the API gateway.
* **Short-Lived Access Tokens**: Maximum access token lifetime is set to **5 to 15 minutes**, requiring automated background token rotation.

### 7.2 Cryptographic Non-Repudiation for Critical Actions
For high-risk healthcare operations (e.g., electronic prescriptions, patient record transfers, or exporting ePHI):
* The client device generates a **digital signature** over the transaction payload using an asymmetric key stored in the hardware KeyStore / Key Vault.
* The digital signature is verified by the backend and saved permanently in the audit log, providing **bank-grade non-repudiation** (proving legally which user authorized the action).

### 7.3 Dedicated Hardware Security Modules (FIPS 140-2 Level 3 / 140-3 HSM)
* All master encryption keys and DPoP signing keys are generated inside **Azure Key Vault Managed HSM** (Hardware Security Module).
* Keys are protected by physical FIPS 140-2 Level 3 tamper-resistant hardware and **cannot be exported in plaintext under any operational condition**.

### 7.4 Real-Time Fraud & Anomaly Velocity Detection
* **Microsoft Sentinel SOAR Playbooks**: Automatically monitor access patterns in real time.
* If velocity anomalies occur (e.g., a doctor account attempts to download 500 patient files in 30 seconds, or logs in from an impossible travel distance), Sentinel automatically executes a SOAR playbook to **instantaneously revoke the user's Entra session and isolate the IP address in Azure Front Door in under 5 seconds**.

---

## 8. Chief Security Officer (CSO) Governance & HIPAA Safeguards Mapping

### 8.1 HIPAA § 164.312 Technical Safeguards Mapping Matrix

| HIPAA Security Rule Standard | Regulatory Requirement | Architectural Implementation |
| :--- | :--- | :--- |
| **§ 164.312(a)(1) Access Control** | Unique user identification & emergency access ("break-the-glass"). | Entra External ID GUIDs (`oid`), DPoP Sender-Constrained Tokens, and ABAC Emergency Override Policies. |
| **§ 164.312(a)(2)(iv) Encryption** | Encryption & decryption of ePHI at rest. | Azure SQL & Blob Storage encrypted with Customer-Managed Keys (CMK) in Azure Key Vault. |
| **§ 164.312(b) Audit Controls** | Record and examine activity in systems containing ePHI. | WORM Immutable Azure Log Analytics + Microsoft Sentinel SIEM logging every read/write event. |
| **§ 164.312(c)(1) Data Integrity** | Protect ePHI from improper alteration or destruction. | SHA-256 digital signatures, CMK envelope encryption, and SQL Row-Level Security (RLS). |
| **§ 164.312(d) Entity Authentication** | Verify that a person seeking access to ePHI is who they claim to be. | Risk-Based Adaptive MFA, FIDO2/WebAuthn hardware keys, and iOS/Android Biometric authentication. |
| **§ 164.312(e)(1) Transmission Security** | Protect ePHI from unauthorized access while in transit. | TLS 1.3 encryption, DPoP token binding, and Azure Private Link VNet isolation. |

---

### 8.2 CSO Emergency Cryptographic Revocation Playbook (The 30-Second Kill-Switch)

In the event of a severe security incident, active breach attempt, or legal subpoena, the **Chief Security Officer** at Mortho has the authority to execute the **30-Second Emergency Kill-Switch**:

```
[ STEP 1: KEY VAULT REVOCATION ]
  Disable Customer-Managed Key (CMK) in Azure Key Vault.
  ↳ RESULT: All database rows, blob files, and backups become instantly unreadable garbage.

[ STEP 2: ENTRA SESSION REVOCATION ]
  Execute Graph API PowerShell: Revoke-MgUserSignInSession for all active sessions.
  ↳ RESULT: Terminates all active OAuth tokens across Web, iOS, and Android clients instantly.

[ STEP 3: FRONT DOOR EDGE LOCKDOWN ]
  Enable Azure Front Door "Maintenance & Incident Response" WAF Rule.
  ↳ RESULT: Blocks 100% of incoming HTTP/S traffic at the global edge.
```

---

### 8.3 Mortho Chief Security Officer Architectural Sign-Off

This document represents the formal Security & HIPAA Compliance Architecture for Mortho.

* **Organization**: Mortho Healthcare
* **Title**: Chief Security Officer (CSO)
* **Status**: Approved & Adopted as Project Security Foundation
* **Target Security Assurance**: Bank-Grade (FAPI 2.0 / FIPS 140-2 Level 3 / Zero-Trust)
* **HIPAA BAA Provider**: Microsoft Enterprise Agreement

---

## 9. Operational Project Plan: DEV ➔ TEST ➔ QA ➔ PROD Roadmap

To guarantee a smooth, structured rollout, the project plan is divided into 4 sequential stages incorporating **Microsoft Playwright** for automated E2E web testing.

```
STAGE 1: DEV                STAGE 2: TEST              STAGE 3: QA                STAGE 4: PROD
[10 Local Steps]            [Playwright & CI Scans]     [Azure QA & Pen Testing]   [Enterprise Hardening]
  1. Workstation Setup        11. Qodana SAST Scan       17. Deploy Bicep QA Cloud  23. Deploy Private Link
  2. Entra Dev Registration   12. EF Core Multi-Tenant   18. Managed Identities     24. Key Vault Managed HSM
  3. Spin Aspire Containers   13. Playwright E2E Tests   19. CI/CD Cloud Deploy     25. Sentinel WORM SIEM
  4. Generate DPoP Keys       14. Docker Containers      20. Playwright QA Suite    26. Playwright Pre-Prod
  5. Set User-Secrets         15. Trivy CVE Scan         21. Pentest-Tools DAST     27. Sign HIPAA BAA
  6. Migration 7 DB Tables    16. GitHub Actions CI      22. QA Bug Fixes Only      28. CSO Go-Live Signoff
  7. BFF Cookie Auth
  8. Graph API Invitations
  9. Flutter Web & Mobile
 10. Local E2E Verification
```

---

### 9.1 Inexpensive Security & QA Testing Toolchain

| Testing Tier | Recommended Tool | Cost Profile | Role in Architecture |
| :--- | :--- | :--- | :--- |
| **Web UI & E2E Automated Testing** | **Microsoft Playwright** (`Microsoft.Playwright`) | **100% Free & Open Source** | Native Microsoft C# E2E test framework. Automates Chromium, Firefox, and Safari to test Flutter Web UI, BFF `HttpOnly` cookie logins, and API routes. |
| **Static Code Inspection (SAST)** | **JetBrains Qodana Ultimate Plus** | **Required for QA Deployment** ($15/dev/mo) | Deep C# .NET 10 & Flutter/Dart static code analysis, Taint Analysis (untrusted data tracking), license compliance audits, and OWASP security quality gates. |
| **Dynamic Pen Testing (DAST)** | **Pentest-Tools.com** + **OWASP ZAP** | Free (OWASP ZAP) / Cost-effective (Pentest-Tools) | Automated DAST web & API penetration scanner. Tests `https://qa.mortho.com` for OWASP Top 10, SQLi, XSS, CSRF. |
| **Container & Dependency Scan** | **Trivy** *(Open Source)* + **ACR Defender** | Free / Low-cost per container | Scans Docker container base images, C# NuGet packages, and Flutter dependencies for known CVE vulnerabilities. |
| **Infrastructure as Code (IaC)** | **Microsoft Bicep** | **100% Free** (Native Azure tool) | Defines and deploys all Azure infrastructure (Front Door, App Service, SQL, Key Vault) consistently across QA and Prod. |
| **API Integration Testing** | **Bruno / Postman** + `WebApplicationFactory` | Free / Low-cost | Automated integration tests executing C# API endpoints and verifying EF Core Global Query Filter data isolation. |

---

### 9.2 Stage 1: DEV Stage (Local Workstation Setup — Exactly 10 Steps)

The local development environment runs on the developer's PC using **.NET Aspire** to orchestrate lightweight containers.

* [ ] **Step 1 (Workstation Prerequisite Setup)**: Install .NET 10 SDK, Docker Desktop (or Podman), Flutter SDK, Microsoft Playwright CLI (`playwright install`), and IDE (Visual Studio / JetBrains Rider with JetBrains Qodana plugin).
* [ ] **Step 2 (Entra Dev Tenant Registration)**: Create a free Microsoft Entra Developer Tenant and register `Mortho-Local-BFF` with redirect URI `https://localhost:7001/signin-oidc`.
* [ ] **Step 3 (Local Container Orchestration via .NET Aspire)**: Launch `.NET Aspire` (`AppAspireHost`) to spin up local SQL Server 2022, Redis (permission cache), and Azurite (Blob Storage emulator) containers.
* [ ] **Step 4 (Local Cryptographic DPoP Keys Generation)**: Run local certificate generator (`GenerateCertificate` project) to generate local DPoP testing keys (`ecdsa256-private.pem` & `ecdsa256-public.pem`).
* [ ] **Step 5 (Configure .NET User-Secrets)**: Configure local `.NET user-secrets` for Entra `ClientId`, `TenantId`, and DPoP key paths so zero secrets exist in git.
* [ ] **Step 6 (EF Core 10 Database Migration)**: Create and apply initial EF Core migration for the **7 clean domain tables** (`Tenants`, `Organizations`, `Users`, `DoctorPatientAssignments`, `CareTeamMembers`, `DoctorCoverage`, `Patients`).
* [ ] **Step 7 (ASP.NET Core BFF Cookie Authentication Setup)**: Implement standard ASP.NET Core OpenIdConnect BFF handlers with `HttpOnly`, `SameSite=Strict`, `Secure` session cookies and server-side token storage.
* [ ] **Step 8 (Microsoft Graph API Doctor Invitation Integration)**: Implement backend C# endpoint calling `GraphServiceClient.Invitations.PostAsync` for passwordless doctor email invitations.
* [ ] **Step 9 (Flutter Cross-Platform Client Integration)**: Configure Flutter Web (served via BFF) and Flutter Mobile (iOS Keychain / Android KeyStore + Biometrics via `local_auth`).
* [ ] **Step 10 (Local End-to-End Verification)**: Run `AppAspireHost`, log in via Entra ID, query patient records through BFF, and execute local `dotnet test` suite.

---

### 9.3 Stage 2: TEST Stage (Automated Testing, Playwright E2E & CI/CD Pipeline)

The TEST stage automates code quality, static analysis, Microsoft Playwright browser tests, container builds, and integration testing on every git push before code touches the cloud.

* [ ] **Step 11 (JetBrains Qodana Ultimate Plus SAST Integration)**: Configure **JetBrains Qodana Ultimate Plus** (required prior to QA deployment) in GitHub Actions / Azure DevOps CI workflow to perform Taint Analysis, license compliance auditing, and automated SAST security scans on every Pull Request.
* [ ] **Step 12 (EF Core Multi-Tenant Integration Tests)**: Write integration tests using `WebApplicationFactory` to verify EF Core Global Query Filters and Row-Level Security (`OrgId` isolation).
* [ ] **Step 13 (Microsoft Playwright C# Automated E2E Test Suite)**: Write C# Playwright test scripts (`Microsoft.Playwright`) to automate Chromium, Firefox, and WebKit browser testing. Playwright tests:
  - Doctor & Patient login flows through Entra ID.
  - BFF `HttpOnly` session cookie issuance and renewal.
  - Patient record search, DICOM image viewing, and form submissions.
* [ ] **Step 14 (Docker Containerization & Production Manifests)**: Create production `Dockerfile` manifests for BFF and API microservices.
* [ ] **Step 15 (Trivy Container & Dependency CVE Scanning)**: Execute `trivy image` in CI pipeline to verify zero high/critical vulnerabilities in base Linux images and NuGet packages.
* [ ] **Step 16 (CI/CD Pipeline Automation)**: Configure GitHub Actions workflow to build code, run `dotnet test`, execute Playwright E2E suite, run Qodana SAST, and build Docker containers on git push to `release/qa`.

---

### 9.4 Stage 3: QA Stage (Azure Cloud QA, Playwright Validation & External Pen Testing)

The QA stage deploys the application to **actual Azure cloud infrastructure** (`rg-mortho-qa-eastus`) to validate WAF edge protection, Managed Identities, Playwright browser test suites, and execute external penetration testing. **JetBrains Qodana Ultimate Plus** license activation is required prior to QA cloud deployment to enforce Taint Analysis and license compliance quality gates. Code changes in QA are restricted exclusively to bug fixes.

* [ ] **Step 17 (Provision QA Infrastructure via Microsoft Bicep)**: Execute Microsoft Bicep templates (`main.bicep`) to provision Azure Front Door (`qa.mortho.com`), Azure App Service (Linux), Azure SQL (S1), Key Vault, and Blob Storage.
* [ ] **Step 18 (Configure Entra Managed Identities in QA)**: Enable System-Assigned Managed Identity on App Service `app-mortho-bff-qa`. Grant `Azure SQL DB Data Contributor` and `Key Vault Secrets User` roles. Remove SQL passwords from connection strings.
* [ ] **Step 19 (Deploy Containers to QA Cloud)**: Trigger CI/CD pipeline to push Docker images to ACR and deploy to QA App Service via Bicep templates.
* [ ] **Step 20 (Execute Automated Playwright QA Test Suite)**: Run Microsoft Playwright automated test suite against `https://qa.mortho.com` across Chromium, Firefox, and Safari to verify 100% of Web UI user flows pass in the live QA cloud environment.
* [ ] **Step 21 (Pentest-Tools.com DAST Penetration Scan)**: Run an automated DAST penetration scan using **Pentest-Tools.com** (or OWASP ZAP) against `https://qa.mortho.com` to test Front Door WAF protection against SQLi, XSS, CSRF, and SSL vulnerabilities.
* [ ] **Step 22 (QA Bug Fixes & Code Stabilization)**: Perform minor bug fixes based on QA, Playwright results, and penetration scan findings. Zero new features added in QA stage.

---

### 9.5 Stage 4: PROD Stage (Production Hardening, Playwright Verification & Go-Live)

The PROD stage hardens the infrastructure into an enterprise-grade, Zero-Trust environment backed by Private Link network isolation, Customer-Managed Keys (CMK), and Sentinel SIEM.

* [ ] **Step 23 (Provision Production Infrastructure via Microsoft Bicep)**: Execute Bicep templates to deploy `rg-mortho-prod-eastus` with **Azure Private Link & Private Endpoints** (disabling public internet IPs on SQL, Storage, Key Vault, and Redis).
* [ ] **Step 24 (Enable Customer-Managed Keys & Managed HSM)**: Configure Key Vault Managed HSM with CMK envelope encryption on Azure SQL and Storage, enabling **Soft-Delete** and **Purge Protection**.
* [ ] **Step 25 (Enable WORM SIEM Audit Logging & Sentinel SOAR)**: Configure Azure Log Analytics with WORM immutability and connect **Microsoft Sentinel SIEM** for 24/7 automated velocity alerting and SOAR playbooks.
* [ ] **Step 26 (Playwright Synthetic Verification & DAST Rescan)**: Run Microsoft Playwright synthetic verification tests and Pentest-Tools.com rescan against production edge endpoints (`https://app.mortho.com`).
* [ ] **Step 27 (Execute Unified Microsoft HIPAA BAA Legal Agreement)**: Confirm single HIPAA Business Associate Agreement (BAA) with Microsoft covering all 15 Azure components.
* [ ] **Step 28 (Chief Security Officer Production Go-Live Sign-Off)**: Mortho Chief Security Officer verifies all 8 HIPAA Technical Safeguards and signs off for Production Go-Live.

---

### 9.6 Critical Failure Traps & Real-World Engineering Mitigations

```
┌──────────────────────────────────────────────┬──────────────────────────────────────────────┐
│ FAILURE TRAP                                 │ REAL-WORLD ENGINEERING MITIGATION            │
├──────────────────────────────────────────────┼──────────────────────────────────────────────┤
│ 1. Configuration Hell (Over-engineering Day 1)│ Build in layers: Start with Entra ID + BFF   │
│                                              │ locally (Steps 1-10) before adding Redis/DPoP│
├──────────────────────────────────────────────┼──────────────────────────────────────────────┤
│ 2. Entra ID Claim Mismatches                 │ Inspect JWT tokens early using jwt.ms to set │
│                                              │ TokenValidationParameters.RoleClaimType      │
├──────────────────────────────────────────────┼──────────────────────────────────────────────┤
│ 3. Flutter Web Cookie CORS Blocks            │ Serve Flutter Web static files directly from │
│                                              │ BFF (https://localhost:7001) or use proxy    │
├──────────────────────────────────────────────┼──────────────────────────────────────────────┤
│ 4. DB Migration Failures over Private Link   │ Execute EF Core migrations automatically inside│
│                                              │ App Service startup code (db.Database.Migrate)│
└──────────────────────────────────────────────┴──────────────────────────────────────────────┘
```

---

## 10. High Availability, Multi-Region Failover & Incident Notification (Business Continuity)

### 10.1 Real-Time Incident Notifications & Azure Service Health
* **Azure Service Health & Action Groups**: Automatically notifies the Chief Security Officer and DevOps team via SMS, PagerDuty, Email, and Push Notifications the instant an Azure datacenter issue or service degradation affects Mortho's subscription.
* **Application Insights & Sentinel Alerts**: Sends instant automated alerts if API HTTP error rates spike above 1% or request latency exceeds 500ms.

---

### 10.2 Availability Zones (Zone-Redundant 99.99% SLA)
* Within Mortho's primary region (`East US`), Azure App Service, Azure SQL, and Azure Key Vault are deployed across **3 physically separate datacenters (Availability Zones)** with independent power, cooling, and networking.
* **Automated Failover**: If Datacenter 1 suffers a physical power loss or network outage, Azure automatically routes traffic to Datacenter 2 or 3 in **less than 1 second** with zero human intervention.

---

### 10.3 Multi-Region Automated Failover (Azure Front Door)

```
[ PATIENT / DOCTOR TRAFFIC ]
             │
             ▼
   AZURE FRONT DOOR PREMIUM (Global Edge)
   (Runs 5-Second Health Probes)
       │                        │
       ▼ (Primary: Active)      ▼ (Secondary: Standby)
 ┌───────────────┐        ┌───────────────┐
 │ REGION 1      │        │ REGION 2      │
 │ (East US)     │  ───►  │ (West US)     │
 │ App + SQL     │        │ App + SQL     │
 └───────────────┘        └───────────────┘
```

* **Continuous Health Probes**: Azure Front Door probes backend endpoints in `East US` and `West US` every 5 seconds.
* **Instant Failover (< 5 Seconds)**: If the primary region (`East US`) becomes unresponsive, Azure Front Door **automatically redirects 100% of global user traffic to the secondary region (`West US`) in under 5 seconds**, ensuring business continuity for doctors and patients.
* **Azure SQL Active Geo-Replication**: Azure SQL maintains a continuous, asynchronous secondary database replica in `West US`.
* **Geo-Redundant Storage (RA-GRS)**: Patient medical documents and lab images are automatically replicated across **6 total copies** in two distant geographic regions (3 copies in `East US`, 3 copies in `West US`).

---

### 10.4 Disaster Recovery Targets (RTO & RPO)

| Disaster Recovery Metric | Target Objective | Architectural Mechanism |
| :--- | :--- | :--- |
| **RTO (Recovery Time Objective)** | **< 5 Seconds** | Automated edge rerouting via Azure Front Door & Zone Redundancy. |
| **RPO (Recovery Point Objective)** | **< 1 Second** | Azure SQL Active Geo-Replication & Geo-Redundant Storage (RA-GRS). |
