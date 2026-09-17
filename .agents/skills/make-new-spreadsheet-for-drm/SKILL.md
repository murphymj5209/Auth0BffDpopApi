<!-- File: .agents/skills/make-new-spreadsheet-for-drm/SKILL.md -->
<!-- LastUpdated: 2026-09-02T10:19:00-05:00 -->

---
name: make-new-spreadsheet-for-drm
description: Automatically builds the ImplantMetrics CLI, generates fresh Excel/CSV report spreadsheets, syncs DrM_THA_Review_Package and DrM_TKA_Review_Package, creates timestamped review package folders, and performs git commit & push.
---

# Skill Instructions: Make New Spreadsheet for DrM

When the user requests **"make a new spreadsheet for DrM"** or asks to refresh/generate a new spreadsheet package for Dr. M, execute the following workflow step-by-step:

## Step 1: Build & Execute Implant Metrics Pipeline
1. Run `dotnet build cli/Mortho.Suite.Cli.ImplantMetrics`.
2. Run `dotnet run --project cli/Mortho.Suite.Cli.ImplantMetrics -- --for-drm`.
   - *Note*: The pipeline will automatically update `Outputs/MorthoModelTHASizing_v1.xlsm` & `Outputs/MorthoModelTKASizing_v1.xlsm`, sync `deliverables/DrM_THA_Review_Package` & `deliverables/DrM_TKA_Review_Package`, and create timestamped folders `deliverables/DrM_THA_Review_Package_<yyyy-MM-dd_HH-mm-ss>` & `deliverables/DrM_TKA_Review_Package_<yyyy-MM-dd_HH-mm-ss>`.

## Step 2: Verify Deliverables Packages
1. Verify that `deliverables/DrM_THA_Review_Package` and `deliverables/DrM_TKA_Review_Package` contain updated `.xlsm` and `.csv` files.
2. Verify that new timestamped directories were created:
   - `deliverables/DrM_THA_Review_Package_<yyyy-MM-dd_HH-mm-ss>` (containing `MorthoModelTHASizing_v1.xlsm`, `MorthoModelTHASizing_v1.csv`, `ExcelUserGuide.md`, `README.md`, `BlacklistShortcuts.bas`)
   - `deliverables/DrM_TKA_Review_Package_<yyyy-MM-dd_HH-mm-ss>` (containing `MorthoModelTKASizing_v1.xlsm`, `MorthoModelTKASizing_v1.csv`, `ExcelUserGuide.md`, `TKASizing5_Architecture_And_Specification.md`, `README.md`, `BlacklistShortcuts.bas`, `image_manifest_tka.json`)

## Step 3: Git Commit & Push
1. Run `git add deliverables/ cli/Mortho.Suite.Cli.ImplantMetrics/`.
2. Run `git commit -m "feat(deliverables): regenerate spreadsheet report and create DrM_THA_Review_Package timestamped package"`.
3. Run `git push`.
