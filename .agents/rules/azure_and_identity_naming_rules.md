# Azure & Identity Resource Naming Rules

## Mandatory Naming Standard

All Azure Cloud resources, Microsoft Entra ID App Registrations, API scopes, app roles, user accounts, service principals, and database identifiers associated with this project MUST strictly follow this naming convention to ensure total isolation from existing infrastructure:

### 1. 4-Letter Prefix
* **Prefix:** `dpop` (maximum 4 letters).
* Every item MUST begin with `dpop` (or the resource abbreviation followed by `dpop`).

### 2. Single-Letter Environment Suffix
Every item MUST end with the designated single-letter environment code:
* **`d`** $\rightarrow$ **DEV** (Local development & developer sandbox)
* **`t`** $\rightarrow$ **TEST** (Automated testing & CI/CD builds)
* **`q`** $\rightarrow$ **QA** (Staging & penetration testing)
* **`p`** $\rightarrow$ **PROD** (Live production system)

---

## Standardized Resource Name Mapping

| Resource / Item | Pattern | DEV (`d`) | TEST (`t`) | QA (`q`) | PROD (`p`) |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **BFF App Registration** | `dpop-bff-{env}` | `dpop-bff-d` | `dpop-bff-t` | `dpop-bff-q` | `dpop-bff-p` |
| **API App Registration** | `dpop-api-{env}` | `dpop-api-d` | `dpop-api-t` | `dpop-api-q` | `dpop-api-p` |
| **Application ID URI** | `api://dpop-api-{env}` | `api://dpop-api-d` | `api://dpop-api-t` | `api://dpop-api-q` | `api://dpop-api-p` |
| **API Permission Scope** | `api://dpop-api-{env}/{scope}` | `api://dpop-api-d/access_as_user` | `api://dpop-api-t/access_as_user` | `api://dpop-api-q/access_as_user` | `api://dpop-api-p/access_as_user` |
| **App Roles** | `dpop.{role}.{env}` | `dpop.doc.d`, `dpop.pat.d` | `dpop.doc.t`, `dpop.pat.t` | `dpop.doc.q`, `dpop.pat.q` | `dpop.doc.p`, `dpop.pat.p` |
| **Resource Group** | `rg-dpop-{env}-{region}` | `rg-dpop-d-eastus` | `rg-dpop-t-eastus` | `rg-dpop-q-eastus` | `rg-dpop-p-eastus` |
| **App Service (BFF)** | `app-dpop-bff-{env}` | `app-dpop-bff-d` | `app-dpop-bff-t` | `app-dpop-bff-q` | `app-dpop-bff-p` |
| **App Service (API)** | `app-dpop-api-{env}` | `app-dpop-api-d` | `app-dpop-api-t` | `app-dpop-api-q` | `app-dpop-api-p` |
| **Key Vault** | `kv-dpop-{env}` | `kv-dpop-d` | `kv-dpop-t` | `kv-dpop-q` | `kv-dpop-p` |
| **Azure SQL Server** | `sql-dpop-{env}` | `sql-dpop-d` | `sql-dpop-t` | `sql-dpop-q` | `sql-dpop-p` |
| **Storage Account** | `st` + `dpop` + `{env}` | `stdpopd` | `stdpopt` | `stdpopq` | `stdpopp` |
| **Front Door Profile** | `afd-dpop-{env}` | `afd-dpop-d` | `afd-dpop-t` | `afd-dpop-q` | `afd-dpop-p` |
| **Managed Identity** | `id-dpop-{role}-{env}` | `id-dpop-bff-d` | `id-dpop-bff-t` | `id-dpop-bff-q` | `id-dpop-bff-p` |
| **SQL User / Schema** | `dpop_app_{env}` | `dpop_app_d` | `dpop_app_t` | `dpop_app_q` | `dpop_app_p` |

---

## User & Identity Naming Rules

1. **Test Accounts:**
   * DEV: `doc-smith-d@yourdomain.com`, `pat-jones-d@yourdomain.com`
   * TEST: `doc-smith-t@yourdomain.com`, `pat-jones-t@yourdomain.com`
   * QA: `doc-smith-q@yourdomain.com`, `pat-jones-q@yourdomain.com`
   * PROD: Real provider accounts / `dpop-doc-p`

2. **Strict Prohibition:**
   * DO NOT use or overwrite existing `mortho` or legacy resource names.
   * All Bicep templates, Terraform scripts, and User-Secrets MUST strictly use the `dpop-...-[d|t|q|p]` schema.
