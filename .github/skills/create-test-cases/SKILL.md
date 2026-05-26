---
name: create-test-cases
description: Conduct an interactive session to generate comprehensive test cases for a user story or feature specification saved to .specs/. Use when asked to create test cases, generate a test plan, or mentions "/test".
---

# Tester: Interactive Test Case Generation Session

## Primary Mission
Guide users through transforming user stories into comprehensive, industry-standard test cases. Build practical test scenarios covering happy paths, edge cases, negative scenarios, and non-functional requirements with clarity and executability.

## Session Workflow

**Start Every Session:**
1. Welcome user and ask for the user story or feature specification
2. Read and analyze the requirements and acceptance criteria
3. Ask if there are existing test documentation or standards to follow
4. Present the Test Case Generation Checklist

**Interactive Development:**
Work through the checklist naturally via conversation, using testing expertise to identify gaps, edge cases, and non-obvious scenarios. Propose specific test scenarios based on acceptance criteria and ask clarifying questions about behavior under different conditions.

**Writing Style for QA Testers:**
- Use plain language accessible to any tester, regardless of technical background
- Avoid developer jargon: "UTC timezone" → "server time", "UUID" → "unique ID", "null" → "empty/not set"
- Write test steps conversationally: "Create a user named..." not "Send POST request to..."
- Focus on observable outcomes (UI behavior, messages, data) not technical implementation
- Include technical details (JSON, endpoints) only as optional reference

**Key Questions to Explore:**
- What are minimum/maximum values or limits?
- What happens with invalid or incorrect inputs?
- Any timing or sequence requirements?
- Who should/shouldn't access this feature?
- Performance expectations (speed, capacity)?
- Connections to other systems or services?

**Asking Questions:** Ask related questions by topic, wait for response. Break into manageable Q&A interactions.

**Complete When:** All checklist items satisfied and user confirms readiness

## Test Case Generation Checklist
Always show and update throughout session:

```
🧪 **Test Case Generation Checklist**

**Analysis & Understanding**
- [ ] User story requirements understood
- [ ] Acceptance criteria analyzed
- [ ] Test scope and boundaries identified

**Test Coverage**
- [ ] Positive test cases (happy paths)
- [ ] Negative test cases (error handling)
- [ ] Edge cases and boundary conditions
- [ ] Integration test scenarios (if applicable)
- [ ] Non-functional test cases (if applicable)

**Documentation**
- [ ] Test prerequisites and setup defined
- [ ] Expected results clearly documented
- [ ] Test data requirements identified

**Status: X/N items complete**
```

## Session Completion
1. Present final test case document for review
2. **Remove duplicates**: Scan and consolidate redundant test scenarios
3. Ask user to confirm readiness
4. Determine a descriptive filename: prefix with `[test] ` followed by the related story name (e.g., `[test] dark-mode-theme-toggle.md`)
5. Save alongside the related user story in `.specs/` (e.g., `.specs/[test] dark-mode-theme-toggle.md`)
6. Confirm filename and location with user
7. Write document and confirm success

## Test Case Document Structure

```markdown
# Test Cases: [FEATURE NAME]

## Test Overview
**User Story Reference:** [Link or filename of the related spec in .specs/]  
**Test Scope:** [What will be tested]  
**Out of Scope:** [What won't be covered]

## Test Prerequisites
**Test Environment:** [What needs to be set up and running]  
**Test Data:** [Sample data values]  
**Setup Steps:** [What to do before testing]  
**Requirements:** [Other systems or tools needed]

---

## [Test Category - Positive/Negative/Edge Cases/Integration/Non-Functional]

### [Test Case Name]
**Objective:** [What this validates]  
**Priority:** [Critical/High/Medium/Low]  
**Test Type:** [Functional/Integration/End-to-End/Performance/Security/Usability/Accessibility]

**Setup:**
- [What must be ready before starting]

**Test Steps:**
1. [Action in simple, clear language]
2. [Next action]
3. [Continue...]

**What You Should See:**
- [Specific, observable outcome]
- [For errors: specific error message, system response, recovery behavior]
- [Continue...]

**Test Data:**
- [Specific values: normal data, invalid data, boundary values (smallest, largest, zero, empty), etc.]

---

## Test Execution Summary

| Test Case Name | Priority | Status | Notes |
|----------------|----------|--------|-------|
| [Name] | [Priority] | [Pass/Fail/Blocked/Skipped] | [Execution notes] |

## Notes
- [Additional context, assumptions, or considerations]
- [Known limitations or areas needing further coverage]
```

## Guidelines

**Quality Standards:**
- Single, clear objective per test case
- Specific, repeatable steps in plain language describing what tester will see
- Easy-to-understand test data examples
- Clear setup without technical jargon
- Descriptive test case names
- Avoid technical implementation details (database operations, internal field names) unless necessary

**Coverage Requirements:**
- Minimum one test per acceptance criteria scenario
- Error tests for each validation rule
- Boundary tests for inputs with limits
- Integration tests for external system connections
- Non-functional tests for performance, security, usability requirements

**Priority Definitions:**
- **Critical**: Blocks core functionality or high business risk
- **High**: Important functionality affecting user experience significantly
- **Medium**: Standard functionality with moderate impact
- **Low**: Nice-to-have validations or minor edge cases
