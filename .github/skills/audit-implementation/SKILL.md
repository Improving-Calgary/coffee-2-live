---
name: audit-implementation
description: Analyze the codebase against documented architectural patterns, design principles, and coding standards to identify implementation gaps, then generate a prioritized, actionable remediation plan. Use when asked to audit the implementation, check code against architecture standards, or verify coding standards compliance.
---

# Solution Architect: Implementation Audit & Remediation Plan

## Mission
Analyze the codebase against documented architectural patterns, design principles, and coding standards to identify implementation gaps, then generate a prioritized, actionable remediation plan.

## Scope

**In Scope:**
- Architecture and design principles (API, database, security, performance)
- Coding standards and conventions
- Error handling and logging
- Testing strategies and coverage
- Dependency management and module structure

**Out of Scope:**
- README files, setup guides, deployment instructions
- User-facing usage documentation

## Discovery Phase

**Step 1: Locate Implementation Guidelines**

Search for implementation documentation in common locations:
- `.docs/` folder (technical design, architecture docs)
- `docs/` folder (look for technical/development docs)
- `CONTRIBUTING.md` (coding standards section)
- Root-level `ARCHITECTURE.md`, `DESIGN.md`, `STANDARDS.md`
- Code comments with architectural decisions (ADRs)

**Step 2: Confirm with User**

If implementation docs are found:
- Present the discovered locations to the user
- Ask: "I found implementation guidelines in [locations]. Should I use these, or is there a different location I should reference?"

If no clear implementation docs are found:
- Ask: "I couldn't locate clear implementation guidelines. Please specify the folder or files that contain your architecture patterns, coding standards, and design principles."

**Step 3: Load and Analyze Standards**

Once confirmed, extract documented standards for:
- File/folder structure and naming conventions
- Architectural layers and design patterns
- Testing, security, and error handling requirements
- Logging and monitoring standards

## Audit Process

```
📋 **Implementation Audit Checklist**

**Discovery & Analysis**
- [ ] Implementation guidelines confirmed
- [ ] Code structure mapped against standards

**Compliance Verification**
- [ ] File/folder structure and naming follow conventions
- [ ] Architectural layers properly separated
- [ ] Design patterns correctly implemented
- [ ] Error handling, logging, and testing meet requirements
- [ ] Security and API design follow documented principles

**Results**
- [ ] Violations categorized by severity
- [ ] Missing implementations identified
- [ ] Remediation plan prioritized

**Status: X/N complete**
```

**Priority Definitions:**
- **Critical**: Violates security requirements or breaks architectural integrity
- **High**: Violates core design patterns, creates technical debt, or hampers maintainability
- **Medium**: Inconsistent with standards but doesn't create immediate problems
- **Low**: Minor style or organizational improvements

## Output Format

Generate this structured plan:

```markdown
# Implementation Audit & Remediation Plan

*Generated: [Date]*

## Implementation Guidelines Used
[List the documents/locations analyzed as the baseline]

## Executive Summary
[Brief overview of implementation health and adherence to standards]

**Violation Breakdown:** Critical: [N] | High: [N] | Medium: [N] | Low: [N]

## Critical Violations (Must Fix)

### Violation 1: [Concise Title]
**Location:** [path/to/file.py](path/to/file.py#L10-L25)
**Documented Standard:** [What the guidelines say should be done]
**Actual Implementation:** [What the code actually does]
**Impact:** [Why this violates architectural integrity or creates security risk]
**Required Action:** [Specific refactoring needed with examples]

## High Priority Violations

[Use same format as Critical, emphasizing technical debt/maintainability impact]

## Medium Priority Violations

[Use same format, focus on inconsistencies that don't create immediate problems]

## Low Priority Violations

[Simplified format: Location, Suggested Action]

## Missing Implementations

### Gap 1: [What's not implemented per standards]
**Documented Requirement:** [What the guidelines require]
**Current State:** [What exists or is missing]
**Recommended Location:** [where to implement]
**Implementation Guidance:** [how to implement per standards]
**Priority:** [Critical/High/Medium/Low]

## Pattern Inconsistencies

### Inconsistency 1: [Where multiple approaches exist]
**Standard Pattern:** [What the guidelines prescribe]
**Found Implementations:**
- Approach A: [location references]
- Approach B: [location references]
**Impact:** [Why inconsistency is problematic]
**Recommended Resolution:** [Which approach to standardize on and why]
**Files to Update:** [List of files needing changes]

## Remediation Sequence

1. **Critical Fixes** (Do first - breaks architectural integrity)
   - [ ] Violation #[N]: [Brief title + file reference]

2. **High Priority** (Do next - technical debt/maintainability)
   - [ ] Violation #[N]: [Brief title + file reference]

3. **Pattern Standardization** (Resolve inconsistencies)
   - [ ] Inconsistency #[N]: [Brief title]

4. **Medium Priority** (When time permits)
   - [ ] Violation #[N]: [Brief title + file reference]

5. **Low Priority** (Optional improvements)
   - [ ] Violation #[N]: [Brief title + file reference]

## Recommended Documentation Updates

[If gaps in the implementation guidelines were discovered, suggest additions]
```

## Key Principles

- Wait for user confirmation on documentation locations before proceeding
- Reference specific file paths and line numbers in findings
- Explain *why* violations matter (architectural integrity, maintainability, security)
- Provide concrete, actionable remediation steps
- Group related violations to maintain readability

## Document Generation Instructions

After completing your analysis, create the output document with the following specifications:
- **File Name**: `implementation-audit-remediation-plan.md`
- **Storage Location**: Unless otherwise indicated, default to `.reports/implementation-audit-remediation-plan.md`, creating the `.reports/` folder at the root if it doesn't exist
- **Important**: If a file with this name already exists, overwrite it with your new content

### Final Review
Before creating the file, double-check your analysis and the generated document against:
1. The original requirements
2. The provided codebase

Ensure the output is complete, accurate, directly addresses the user's request, and is structurally correct.
