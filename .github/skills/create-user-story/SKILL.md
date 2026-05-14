---
name: create-user-story
description: Conduct an interactive session to specify feature requirements and generate a crisp user story saved to .specs/. Use when asked to create a user story, define a feature spec, or mentions "/story".
---

# Spec Lead: Interactive Specification Session

## Primary Mission
Guide users through transforming vague feature ideas into crisp, actionable user stories that can be implemented effectively. Focus on essential details while avoiding information overload.

## Session Workflow

**Start Every Session:**
1. Welcome user and ask for a description of the feature
2. If not provided, ask user if there are any user story guide docs to consider
3. Present the specification checklist

**Interactive Session Flow:**
Work through the checklist areas naturally through conversation, starting with discovery questions to clarify vague requests. Lead with intelligence by providing specific recommendations and asking clarifying questions as needed. Document the user story incrementally as the conversation progresses, updating the checklist to show progress.

**Asking Questions:**
Ask related questions together by topic, then wait for response. Do not ask all questions at once, break them into manageable parts entering into a Q&A style interaction.

**Complete When:** All checklist items are satisfied and user confirms readiness

## Specification Checklist
Always show this checklist and update progress throughout the session:

```
📋 **Specification Checklist**

**Core Story**
- [ ] User Story (As a/I want/So that)
- [ ] Context (Optional relevant background information)

**Key Scenarios**
- [ ] Happy path (primary success flow)
- [ ] Error handling (main failure cases)
- [ ] Edge cases (if critical to implementation)

**Constraints**
- [ ] Critical constraints identified

**Status: X/N items complete**
```

## Session Completion
When all checklist items are complete:
1. Present the final specification document for review
2. **Review for duplicate content**: Scan the document to identify and remove redundant information across sections (e.g., combining similar scenarios, condensing repetitive constraints, removing redundant out-of-scope items)
3. Ask user to confirm the document is ready
4. Once confirmed, triage the story for agent readiness
5. Determine a descriptive filename based on the specification content (e.g., "user_authentication_enhancement.md")
6. If there is an existing `.specs/` or similar folder, determine the best location to save the file based on content and folder structure
7. Confirm the filename and location with the user
8. Write the specification to the specified location
9. Confirm the file has been successfully created

## Final Specification Document Structure

Generate this document when the session is complete. **Avoid repeating information across sections - each section should add new information, not restate what's already covered.**

**Formatting:** Use two trailing spaces after each **As a**, **I want to**, **So that**, **Given**, **When**, **Then**, and **And** line to create line breaks between these thoughts.

```markdown
# [FEATURE NAME]

## User Story
**As a** [specific user role],
**I want to** [specific capability],
**So that** [clear business value].

## Context
[Why this feature is needed and high-level background. Avoid implementation details - those belong in acceptance criteria.]

## Acceptance Criteria

### Scenario 1: Happy Path
**Given** [specific initial conditions]
**When** [specific user action]
**Then** [specific measurable outcome]

### Scenario 2: Alternative Path
**Given** [different initial conditions]
**When** [alternative user action]
**Then** [alternative expected outcome]

### Scenario 3: Error Handling
**Given** [specific error condition]
**When** [specific trigger]
**Then** [specific error response]

### Scenario 4: Edge Case - [Boundary Condition]
**Given** [boundary condition]
**When** [edge case trigger]
**Then** [expected behavior]

## Critical Constraints (Optional)
[Only include non-negotiable requirements that apply across all scenarios and aren't already clear from acceptance criteria. Skip this section if constraints are already covered in scenarios.]
- [e.g., Must maintain backward compatibility]

## Out of Scope (Optional)
- [What is explicitly NOT included]

## Assumptions (Optional)
- [Only assumptions not already implied by acceptance criteria]
```
