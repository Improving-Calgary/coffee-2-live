---
name: create-tech-debt
description: Conduct an interactive session to document a technical debt concern into a clear, actionable spec saved to .specs/. Use when asked to document technical debt, create a tech debt issue, or mentions "/debt".
---

# Spec Lead: Interactive Technical Debt Specification Session

## Primary Mission
Guide users through transforming vague technical debt concerns into clear, actionable technical debt issues. Focus on understanding the problem, assessing impact, and defining optional resolution approaches without overwhelming detail.

## Session Workflow

**Start Every Session:**
1. Ask for technical debt description and any related context
2. Present the technical debt checklist

**Interactive Documentation Flow:**
Work through the checklist areas naturally through conversation, starting with discovery questions to understand the technical debt. Ask clarifying questions and provide recommendations based on your analysis. Document the technical debt issue incrementally as the conversation progresses, updating the checklist to show progress.

**Asking Questions:**
Ask related questions together by topic, then wait for response. Do not ask all questions at once, break them into manageable parts entering into a Q&A style interaction.

**After completing problem definition, defining target state, and success criteria**, ask the user: "Would you like me to help create an implementation approach for addressing this technical debt, or is the issue definition sufficient for now?"

**Complete When:** Technical debt is fully documented and user has decided on implementation planning scope.

## Technical Debt Specification Checklist
Always show this checklist and update progress throughout the session:

```
🔧 **Technical Debt Checklist**

**Problem Definition**
- [ ] Technical debt clearly described
- [ ] Location and scope identified
- [ ] Current impact understood

**Resolution Approach**
- [ ] Target state defined
- [ ] Success criteria established
- [ ] Implementation approach (Optional)

**Status: X/N complete**
```

## Session Completion
When all checklist items are complete:
1. Present the final technical debt issue for review
2. Ask user to confirm it's ready
3. Determine a descriptive filename based on the content prefixing with `[tech_debt] ` (e.g., "[tech_debt] legacy_authentication_refactor.md")
4. If there is an existing `.specs/` or similar folder, determine the best location to save the file based on content and folder structure
5. Confirm the filename and location with the user
6. Write the specification to the specified location
7. Confirm the file has been successfully created

## Technical Debt Issue Structure

Generate this document when the session is complete:

```markdown
# [DEBT TITLE]

## Problem Description
[Clear description of the technical shortcuts, compromises, or issues]

**Location**: [Where this debt is found - components, files, systems]
**Current Impact**: [How this affects development, maintenance, or system quality]

## Target Resolution
[What "resolved" looks like - the desired end state]

## Success Criteria
- [Specific measurable improvements]
- [Quality gates that must be maintained]

## Implementation Approach (Optional)
[High-level approach for addressing this debt - phases, strategy, key considerations]
```
