<!-- File: .agents/skills/run-model-testing-matrix/SKILL.md -->
<!-- LastUpdated: 2026-09-03T10:05:00-05:00 -->

---
name: run-model-testing-matrix
description: Automatically executes the full-dataset multi-tenant testing matrix across all 839 THA cases and 668 TKA cases, audits accuracy by doctor, manufacturer (Signature/DePuy/Stryker), and organization (IBJI), exports color-coded Excel spreadsheets for Dr. M, and generates executive Markdown audit reports.
---

# Run Model Testing Matrix Skill

## 🎯 Purpose & Scope

This skill orchestrates the end-to-end testing, validation, and multi-tenant audit of the trained **Mortho Total Hip Arthroplasty (THA)** and **Total Knee Arthroplasty (TKA)** deep learning sizing models.

It runs across **100% of all clinical database cases** (zero partial sampling) and evaluates four distinct analysis scenarios:
1. **Scenario A: Global Full-Cohort Benchmark** (100% of cases)
2. **Scenario B: By Doctor / Surgeon** (Surgeon technique & bias breakdown)
3. **Scenario C: By Manufacturer & Implant Model** (Signature Orthopaedics, DePuy, Stryker, Zimmer)
4. **Scenario D: By Organization / Practice Network** (Illinois Bone & Joint Institute - IBJI)

---

## ⚡ Trigger Commands

Activate this skill whenever the user asks:
- *"run the model testing"*
- *"test all cases"*
- *"run the testing matrix"*
- *"audit our accuracy for Signature Orthopaedics"*
- *"show me the doctor breakdown in Excel"*
- *"export test cases to spreadsheet"*

---

## 📋 Standard Workflow

### Phase 1: Ingest & Update Test Suites
1. Check that `deliverables/DrM_THA_Review_Package/MorthoModelTHASizing_v1.csv` and `deliverables/DrM_TKA_Review_Package/MorthoTKATrainingDataset_Approved.csv` are fresh.
2. Ingest all records into JSON master suites:
   ```bash
   python python/MorthoModelTesting/1_Data_Pipelines_And_Converters/build_master_suites_from_csv.py --joint THA
   python python/MorthoModelTesting/1_Data_Pipelines_And_Converters/build_master_suites_from_csv.py --joint TKA
   ```

### Phase 2: Execute Master Testing Matrix
Execute the master runner on the NVIDIA GeForce RTX 3090 GPU:
```bash
python python/MorthoModelTesting/run_full_testing_matrix.py --joint THA
python python/MorthoModelTesting/run_full_testing_matrix.py --joint TKA
```

### Phase 3: Verify Output Artifacts
Ensure all deliverables are generated:
- **Excel Spreadsheets with Conditional Formatting**:
  - `python/MorthoModelTesting/5_Spreadsheet_Exports_And_DrM_Packages/THA/Master_THA_Testing_Results.xlsx`
  - `python/MorthoModelTesting/5_Spreadsheet_Exports_And_DrM_Packages/THA/Doctor_Breakdown_THA.xlsx`
  - `python/MorthoModelTesting/5_Spreadsheet_Exports_And_DrM_Packages/THA/Manufacturer_Audit_THA.xlsx`
- **Executive Markdown Reports**:
  - `python/MorthoModelTesting/6_Executive_Markdown_Reports/THA/GLOBAL_THA_ACCURACY_REPORT.md`
  - `python/MorthoModelTesting/6_Executive_Markdown_Reports/THA/PARTNER_REPORT_Signature_Ortho.md`
  - `python/MorthoModelTesting/6_Executive_Markdown_Reports/THA/PRACTICE_REPORT_IBJI_Network.md`
- **Execution Log**:
  - `python/MorthoModelTesting/logs/TestRun_<joint>_<scenario>_<timestamp>.log`

### Phase 4: Two-Way Excel Synchronization (When Requested)
- Export any JSON suite to Excel:
  ```bash
  python python/MorthoModelTesting/1_Data_Pipelines_And_Converters/json_to_spreadsheet.py --input <path_to_json> --output <path_to_xlsx>
  ```
- Import Dr. M's reviewed Excel back into JSON:
  ```bash
  python python/MorthoModelTesting/1_Data_Pipelines_And_Converters/spreadsheet_to_json.py --input <path_to_xlsx> --output <path_to_json>
  ```

---

## 🔒 Compliance & Directives

1. **Zero Sugarcoating**: Reports must state plain clinical truth. Never bias, sway, or inflate metrics.
2. **Inference Acceleration & Micro-Timing**: All execution engines must support FP16 Tensor Core acceleration and log the 4-phase timing breakdown ($T_{\text{pre}}$, $T_{\text{gpu}}$, $T_{\text{post}}$, $T_{\text{total}}$) and VRAM usage.
3. **RAG Status Badges**:
   - 🟢 **Tier 1 (Approved)**: Exact match $\ge 60\%$, $\pm 1$ size compliance $\ge 90\%$, MAE $\le 0.50$.
   - 🟡 **Tier 2 (Caution)**: $\pm 1$ size compliance $75\% - 89\%$, MAE $0.51 - 0.99$.
   - 🔴 **Tier 3 (Underperforming)**: $\pm 1$ size compliance $< 75\%$, MAE $\ge 1.00$.
4. **Mandatory File Headers**: All new files must contain 2-line headers with relative path and ISO timestamp.
