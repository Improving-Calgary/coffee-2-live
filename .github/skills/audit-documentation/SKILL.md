---
name: audit-documentation
description: Analyze operational documentation against the actual codebase to identify what's out of sync, then generate a prioritized, actionable update plan. Use when asked to audit docs, verify documentation is accurate, or check if docs match the implementation.
---

# Technical Writer: Documentation Audit & Update Plan

## Mission
Analyze operational documentation against the actual codebase to identify what's out of sync, then generate a prioritized, actionable update plan.

## Scope

**Operational Documentation (In Scope):**
- README files, QUICKSTART guides, deployment guides
- Installation, prerequisites, configuration, usage documentation
- Getting started, running the project, troubleshooting guides

**Implementation Documentation (Out of Scope):**
- Architecture decisions, coding standards, technical design docs
- Code-level API documentation (docstrings, JSDoc)
- Development guidelines, internal agent instructions

## Audit Process

```
📋 **Audit Checklist**

**Discovery**
- [ ] Operational docs identified
- [ ] Config files cataloged (package.json, requirements.txt, docker-compose.yml, terraform)
- [ ] Entry points mapped (main.py, CLI scripts, etc.)

**Verification**
- [ ] Prerequisites match package dependencies
- [ ] Installation steps match setup process
- [ ] Run commands match actual entry points
- [ ] Environment/config options all documented
- [ ] Deployment steps align with IaC/scripts
- [ ] CLI commands and API endpoints exist as documented
- [ ] Port numbers, file paths, and examples accurate

**Analysis**
- [ ] Gaps identified (missing docs, undocumented features)
- [ ] Issues prioritized by impact

**Status: X/N complete**
```

**Priority Definitions:**
- **Critical**: Prevents users from running the project
- **High**: Causes confusion or errors during setup/use
- **Medium**: Incomplete or unclear but doesn't block usage
- **Low**: Minor improvements or enhancements

## Output Format

Generate this structured plan:

```markdown
# Documentation Audit & Remediation Plan

*Generated: [Date]*

## Executive Summary
[Brief overview of documentation health]

**Issue Breakdown:** Critical: [N] | High: [N] | Medium: [N] | Low: [N]

## Critical Issues (Must Fix)

### Issue 1: [Concise Title]
**File:** [path/to/file.md](path/to/file.md)
**Problem:** [What's wrong or missing]
**Evidence:** [What in code contradicts docs]
**Impact:** [Why this blocks users]
**Action:** [Specific changes needed]

## High Priority Issues

### Issue 2: [Concise Title]
**File:** [path/to/file.md](path/to/file.md)
**Problem:** [What's wrong or missing]
**Evidence:** [What in code contradicts docs]
**Impact:** [Why this causes problems]
**Action:** [Specific changes needed]

## Medium Priority Issues

### Issue 3: [Concise Title]
**File:** [path/to/file.md](path/to/file.md)
**Problem:** [What could be improved]
**Action:** [Recommended improvements]

## Low Priority Issues

### Issue 4: [Concise Title]
**File:** [path/to/file.md](path/to/file.md)
**Action:** [Optional enhancements]

## Missing Documentation

### Gap 1: [What's not documented]
**Recommended File:** [where to document]
**Content Needed:** [what to write]
**Priority:** [Critical/High/Medium/Low]

## Update Sequence

1. **Critical Fixes** (Do first)
   - [ ] Issue #[N]: [Brief title]

2. **High Priority** (Do next)
   - [ ] Issue #[N]: [Brief title]

3. **Medium Priority** (When time permits)
   - [ ] Issue #[N]: [Brief title]

4. **Low Priority** (Optional)
   - [ ] Issue #[N]: [Brief title]

## Verification Checklist

- [ ] Installation steps work from scratch
- [ ] Run commands execute successfully
- [ ] All config options documented
- [ ] All documented endpoints/commands exist
- [ ] Examples execute without errors
- [ ] Links resolve correctly
```

## Document Generation Instructions

After completing your analysis, create the output document with the following specifications:
- **File Name**: `documentation-audit-remediation-plan.md`
- **Storage Location**: Unless otherwise indicated, default to `.reports/documentation-audit-remediation-plan.md`, creating the `.reports/` folder at the root if it doesn't exist
- **Important**: If a file with this name already exists, overwrite it with your new content

### Final Review
Before creating the file, double-check your analysis and the generated document against:
1. The original requirements
2. The provided codebase

Ensure the output is complete, accurate, directly addresses the user's request, and is structurally correct.
