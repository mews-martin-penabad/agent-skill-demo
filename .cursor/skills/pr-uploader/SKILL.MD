---
name: create-pr
description: Creates a well-structured pull request using the appropriate PR template for the repository, with automated field population and validation. Use when PR should be created.
---

## Usage
```
/pr [--ticket <<ticket-name>>] [optional context]
```

## Examples
```
/pr "Aligned Temporal port to be consistent with monolith"
/pr --ticket FINTECH-12345
```

## Steps

### 1. Prepare branch
- Ensure all changes are committed
- Push branch to remote if it's not yet on remote
- Verify branch is up to date with main

### 2. Write PR description
Follow this structure. Make sure the language is written in natural language and doesn't sound like AI. Keep it concise so that reviewers can figure out what the PR is about quickly.

**PR Description Template:**
Read the template from `.github/PULL_REQUEST_TEMPLATE.md` in the repository root.

**Field Generation Instructions:**

#### 2.1. Title
Keep the title concise (1 sentence). Focus on WHAT changed, not HOW

#### 2.2. Release note
- Replace XXXXXX placeholder with actual JIRA ticket (PAY-XXXX pattern). Take this value from the branch name
- User can optionally provide the ticket name in the command using --ticket argument
- If none is applicable, state "No ticket"
- Use same title of the PR to release note after the URL link

**Examples:**
```markdown
✅ CORRECT:
https://mews.atlassian.net/browse/FINTECH-12345: Added support for partial refunds via PayPal gateway.

✅ CORRECT:
No ticket: Update NuGet dependencies to latest versions.

❌ WRONG:
Added partial refunds. (Missing Jira link and context)

❌ WRONG:
https://mews.atlassian.net/browse/FINTECH-12345: Implemented PayPalPartialRefundUsecase with Result pattern, added new controller endpoint, updated DI registration, and created mapper methods. (Too detailed - this belongs in Summary)
```

#### 2.3. Summary
Provide concisely what changed. Explanation of what changed and why. Include:
- What was changed
- Why it was changed
- Any important technical decisions

Also:
- Highlight any important patterns or conventions used
- Mention any breaking changes
- Reference related PRs or issues

#### 2.4. Testing Steps
If test files detected, use "Unit tested. Integration tests: [specify if needed]". Otherwise, prompt for manual testing instructions.
- Mention if certain scenarios are not testable and why

#### 2.5. Follow-up Issues
Scan commit messages for TODO/FIXME keywords. Default to "None" if clean

#### 2.6. Performance & Security & Data Privacy
- Quick scan changes for migrations, auth code, data processing
- Identify security risks and mitigations
- If found, prompt for assessment. If in doubt (needs testing locally to properly assess performance), prompt user with findings and recommended actions. Otherwise default to "None"


### 3. Set default configuration
- **Assignee**: Always set to PR author (`@me`)

### 4. Create the pull request
- Display the PR URL upon successful creation