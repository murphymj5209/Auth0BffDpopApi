<!-- File: .agents/AGENTS.md -->
<!-- LastUpdated: 2026-09-07T17:00:00-05:00 -->

# Mortho Orthopedic Suite - Architectural & Development Rules

## SQL Query Ownership & External SQL Files Policy

> [!IMPORTANT]
> **CRITICAL DIRECTIVE: C# Direct Query Ownership**
> - The C# application (`cli/Mortho.Suite.Cli.ImplantMetrics`) owns its SQL queries directly in `SqlQueries.cs` and `queries.json`.
> - **DO NOT ACCESS OR SYNC EXTERNAL `.sql` FILES AT RUNTIME**: The C# application must NEVER read, write, parse, or synchronize with raw `.sql` files in `database/SQL/Queries/` during build or runtime execution.
> - **Do NOT add runtime sync methods** (such as `SyncSqlQueriesToQueriesJson()`) that attempt to read files from `database/SQL/Queries/`.
> - **`queriesSelectDetailedCaseList.sql` Boundary**: Raw `.sql` files under `database/SQL/Queries/` are kept for SQL reference only and must remain clean without femoral data. Femoral data metrics and C# calculations are owned by the C# application (`SqlQueries.cs`, `FemoralCalculationService.cs`, `AcetabularMetricsCalculator.cs`).

## Femoral Data & ML Model Fields in C#

- The C# query in `SqlQueries.cs` (`SelectDetailedCaseList`) includes Section A (Case/Person Data), Section B (Acetabular Data), and resolved Femoral ML Model fields (`mihs_MorthoMlFemoralStemModelId`, `mmfem_MorthoLMModels.Name`, `mmfem_MorthoLMModels.FileTypeDescription`, `mmfem.filename`, `mmfem.locationpath`, `mmfem.isactive`, `mmfem.isdeleted`, `mmfem.type`).
- Ordinal size indexing, mathematical device aggregations ($\mu, s, \text{Min}, \text{Max}, \text{normPct}, \text{normActual}$), and reporting are implemented directly in C# services (`FemoralCalculationService.cs`).

## File Header & LastUpdated Timestamp Policy

> [!IMPORTANT]
> **CRITICAL DIRECTIVE: Mandatory File Headers**
> - Every text file (`.md`, `.sql`, `.txt`, `.py`, `.cs`, `.sh`, `.ps1`, `.bat`, etc.) MUST include a 2-line header at the top of the file:
>   - **Line 1**: File name with relative path from workspace root (using appropriate comment syntax for that language).
>   - **Line 2**: `lastUpdated` ISO datetime string.
> - **Exclusions**: Binary files, generated data exports (`.csv`, `.xlsm`, `.xlsx`), `.git` objects, and build outputs (`bin/`, `obj/`).
> - **Comment Syntax Guide**:
>   - Markdown (`.md`): `<!-- File: <relative_path> -->` and `<!-- LastUpdated: <iso_timestamp> -->`
>   - C# (`.cs`): `// File: <relative_path>` and `// LastUpdated: <iso_timestamp>`
>   - Python/Shell (`.py`, `.ps1`, `.bat`): `# File: <relative_path>` and `# LastUpdated: <iso_timestamp>`
>   - SQL (`.sql`): `-- File: <relative_path>` and `-- LastUpdated: <iso_timestamp>`
>   - JSON (`.json`): Use `"_File"` and `"_LastUpdated"` fields at root level.

## Git Commit Cadence Policy

> [!IMPORTANT]
> **CRITICAL DIRECTIVE: Reduced Git Commit Frequency**
> - Do not commit after every minor tweak or file edit.
> - Batch related code changes, verifications, and output updates into cohesive, logical commits at major milestones or upon task completion (reducing commit frequency by at least 50%).

## Trigger Command Directive: "make a new spreadsheet for DrM"

> [!IMPORTANT]
> **CRITICAL DIRECTIVE: DrM Review Package Workflow**
> When the user specifies **"make a new spreadsheet for DrM"**:
> 1. Execute `dotnet run --project cli/Mortho.Suite.Cli.ImplantMetrics -- --for-drm` to extract fresh SQL metrics, update `deliverables/DrM_THA_Review_Package/` & `deliverables/DrM_TKA_Review_Package/`, and snapshot timestamped packages `DrM_THA_Review_Package_<yyyy-MM-dd_HH-mm-ss>/` and `DrM_TKA_Review_Package_<yyyy-MM-dd_HH-mm-ss>/`.
> 2. Sync and verify the review package directories.
> 3. Perform a `git add deliverables/ cli/`, `git commit`, and `git push`.

## Trigger Command Directive: "plz run the tha models"

> [!IMPORTANT]
> **CRITICAL DIRECTIVE: THA Temporal Model Pipeline (25-Case, 100-Case, 400-Case)**
> When the user specifies **"plz run the tha models"**:
> 1. Slice cases into 3 temporal cohorts: 25-Case Canary, 100-Case Benchmark, and 400-Case Modern Era.
> 2. Train/Calibrate the 3 dedicated temporal models (`MorthoMutlimodalTHASizing_25Cases`, `MorthoMutlimodalTHASizing_100Cases`, `MorthoMutlimodalTHASizing_400Cases`).
> 3. Execute GPU batch inference passing `model_horizon` (`25cases`, `100cases`, `400cases`).
> 4. Generate 3 dedicated review workbooks: `MorthoModelTHASizing_v1_25Cases_Audited.xlsx`, `MorthoModelTHASizing_v1_100Cases_Audited.xlsx`, `MorthoModelTHASizing_v1_400Cases_Audited.xlsx` with Row 2 AutoFilters.
> 5. Run the mandatory automated pre-review integrity check on each file before presentation.

## Mandatory Pre-Review Deliverables Integrity Check Policy

> [!IMPORTANT]
> **CRITICAL DIRECTIVE: Mandatory Pre-Review File Integrity Audit**
> - Before ever stating or presenting an Excel (`.xlsx`, `.xlsm`) or CSV review package as ready for user review:
>   1. **Case Name Integrity**: Verify that `Case Name` is explicitly populated for 100% of rows (not generic or empty IDs).
>   2. **Row Count Verification**: Ensure row count matches expected clinical cohorts (e.g. 839 THA cases, 668 TKA cases).
>   3. **AutoFilter on Row 2**: Confirm Excel AutoFilter is active on Row 2 across all columns (`A2:<LastCol><TotalRows+2>`).
>   4. **Automated Sanity Check**: Run an automated programmatic check that reads the output file, validates columns, and prints a 5-row sample preview to verify zero missing data.


