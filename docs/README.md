# Auth0 BFF Web application (Angular & ASP.NET Core) using downstream API protected with OAuth DPoP

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=damienbod_Auth0BffDpopApi&metric=bugs)](https://sonarcloud.io/summary/new_code?id=damienbod_Auth0BffDpopApi)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=damienbod_Auth0BffDpopApi&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=damienbod_Auth0BffDpopApi)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=damienbod_Auth0BffDpopApi&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=damienbod_Auth0BffDpopApi)

[![.NET and npm build](https://github.com/damienbod/Auth0BffDpopApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/damienbod/Auth0BffDpopApi/actions/workflows/dotnet.yml)

## Setup

![Authn](https://github.com/damienbod/Auth0BffDpopApi/blob/main/images/system.context.drawio.png)

## Certificates setup
- Generate the certificates using the GenerateCertiticate project
- Copy the generated certificates to the BFF server folder
- Upload the rsa public key to the Auth0 dashboard (Settings -> Advanced Settings -> Certificates)
- NOTE: use user secrets to load the certificate in the BFF server project, do not push in repo.

## Zero-Trust Clinical Architecture Documentation Suite

A comprehensive architectural and engineering blueprint has been created for the **Mortho Zero-Trust Clinical Platform**:

- [01-executive_summary.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/01-executive_summary.md): **Executive Vision, Architecture Mapping & ASP.NET Zero Replacement Rationale**
- [02a-cloudflare_work_effort.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/02a-cloudflare_work_effort.md): **Cloudflare Edge, Cloudflare Tunnel (`cloudflared`), WAF, API Shield & Origin Cloaking**
- [02b-google_cloud_work_effort.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/02b-google_cloud_work_effort.md): **GCE VM Deployment, Windows/SQL Image Setup, Cloud KMS TDE, GCS v4 Pre-signed URLs & IAP**
- [02c-fusionauth_identity_work_effort.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/02c-fusionauth_identity_work_effort.md): **FusionAuth Identity Provider, DPoP Issuance, Passkeys/WebAuthn, Hospital SSO & Lambdas**
- [02d-stream_chat_realtime_work_effort.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/02d-stream_chat_realtime_work_effort.md): **HIPAA Real-Time Communication, Care Team Messaging Channels & APNs/FCM Push Notifications**
- [02e-database_storage_work_effort.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/02e-database_storage_work_effort.md): **SQL Server on NVMe, Shared Memory LPC Transport, Cloud KMS TDE, EF Core ReBAC & Audit Trails**
- [03-application_coding_detailed.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/03-application_coding_detailed.md): **BFF YARP Pipelines, DPoP Handlers, Angular Syncfusion Workstations & Flutter Mobile Clients**
- [04-pentest_compliance_verification.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/04-pentest_compliance_verification.md): **Pentest.com Audit Checklist across Web, Mobile, Desktop (Windows, Linux, macOS, iOS, Android)**
- [05-implementservice.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/05-implementservice.md): **Service Implementation Roadmap & Phased Execution Plan (What to Build 1st, 2nd, etc.)**
- [ClientRegistration&WorkFlows.md](file:///e:/Github/Damienbod/Auth0BffDpopApi/docs/ClientRegistration&WorkFlows.md): **Client Registration Specifications & Clinician Onboarding Workflows**

---

## Blogs

[Implement BFF using Auth0, Angular and ASP.NET Core](https://damienbod.com/2026/08/10/implement-bff-using-auth0-angular-and-asp-net-core/)
[Use Aspire to implement and deploy the BFF security architecture](https://damienbod.com/2026/08/17/use-aspire-to-implement-and-deploy-the-bff-security-architecture/)


## Features
- Using OpenID Connect with client assertions (private key JWT)
- Using OAuth DPoP (Demonstrating Proof of Possession) for enhanced security
- Using OAuth PAR (Pushed Authorization Requests) for enhanced security
- Confidential client with client assertion and private key JWT
- Using YARP Reverse Proxy to forward requests to the API

- TODO Implement production YARP for proxied requests to the API with DPoP and PAR
- TODO Debug if User info endpoint is working with private key JWT, DPoP and PAR
- TODO Support mixed APIs

## Podman

https://podman-desktop.io/docs/troubleshooting/troubleshooting-podman

```
podman machine start
```

## Debugging

Start the Angular project from the ui folder

```
npm start
```

or 

```
ng serve --ssl
```

## Start the ASP.NET Core project from the server folder

```
dotnet run
```

Or just open Visual Studio and run the solution.

## Credits and used libraries
- NetEscapades.AspNetCore.SecurityHeaders
- Yarp.ReverseProxy
- ASP.NET Core
- Angular
- Auth0 NuGet packages
- Duende FOSS packages

## UI Angular setup using Angular CLI

```
npm install -g @angular/cli latest

ng update

ng update @angular/cli @angular/core
```

## Links

https://auth0.com/docs/quickstart/webapp/aspnet-core

https://auth0.com/blog/backend-for-frontend-pattern-with-auth0-and-dotnet

https://github.com/damienbod/bff-auth0-aspnetcore-angular

https://github.com/damienbod/DPOP-aspnetcore-idp

https://auth0.com/docs/secure/sender-constraining/demonstrating-proof-of-possession-dpop

https://auth0.com/blog/implementing-dpop-with-auth0

https://auth0.com/docs/quickstart/backend/aspnet-core-webapi#using-dpop-for-enhanced-security
