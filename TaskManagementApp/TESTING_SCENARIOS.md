# Task Management API - Testing Scenarios

## Setup

### 1. Register Users
First, register two test users through the Auth endpoint:

**User 1: Project Creator**
- Email: `creator@example.com`
- Password: `Password@123`

**User 2: Team Member**
- Email: `member@example.com`
- Password: `Password@123`

**User 3: Another User (non-member)**
- Email: `other@example.com`
- Password: `Password@123`

### 2. Get JWT Tokens
Obtain JWT tokens for each user using the /auth/login endpoint.

---

## Test Scenarios

### Scenario 1: Project Creation and Membership

#### ✅ Test 1.1: Creator can create a project
```
POST /api/project
Token: creator_token

{
  "name": "Mobile App Project",
  "description": "Build iOS and Android app"
}
```
**Expected**: 201 Created
**Result**: Project created with ID=1

#### ✅ Test 1.2: Creator can view their project
```
GET /api/project/1
Token: creator_token
```
**Expected**: 200 OK with project details

#### ❌ Test 1.3: Non-member cannot view project
```
GET /api/project/1
Token: other_token
```
**Expected**: 403 Forbidden

#### ✅ Test 1.4: Creator can add member to project
```
POST /api/project/1/members/2
Token: creator_token
```
**Expected**: 200 OK with "Member added successfully"

#### ✅ Test 1.5: Member can now view project
```
GET /api/project/1
Token: member_token
```
**Expected**: 200 OK with project details

---

### Scenario 2: Task Access Control

#### ✅ Test 2.1: Member can create task in project
```
POST /api/task
Token: member_token

{
  "name": "API Development",
  "description": "Build REST API endpoints",
  "dueDate": "2024-05-15T00:00:00Z",
  "status": 0,
  "projectId": 1,
  "assignedToId": null
}
```
**Expected**: 201 Created
**Result**: Task created with ID=1

#### ✅ Test 2.2: Creator can view project tasks
```
GET /api/task/1
Token: creator_token
```
**Expected**: 200 OK with task list including the created task

#### ❌ Test 2.3: Non-member cannot view project tasks
```
GET /api/task/1
Token: other_token
```
**Expected**: 403 Forbidden

#### ❌ Test 2.4: Non-member cannot create task
```
POST /api/task
Token: other_token

{
  "name": "UI Design",
  "description": "Design user interface",
  "dueDate": "2024-05-15T00:00:00Z",
  "status": 0,
  "projectId": 1
}
```
**Expected**: 403 Forbidden - "You do not have access to this project"

---

### Scenario 3: Task Assignment and Modification

#### ✅ Test 3.1: Creator can assign task to member
```
PUT /api/task/1
Token: creator_token

{
  "name": "API Development",
  "description": "Build REST API endpoints",
  "dueDate": "2024-05-15T00:00:00Z",
  "status": 0,
  "projectId": 1,
  "assignedToId": 2
}
```
**Expected**: 200 OK

#### ✅ Test 3.2: Assigned member can modify task
```
PUT /api/task/1
Token: member_token

{
  "name": "API Development",
  "description": "Build REST API endpoints - in progress",
  "dueDate": "2024-05-15T00:00:00Z",
  "status": 1,
  "projectId": 1,
  "assignedToId": 2
}
```
**Expected**: 200 OK

#### ❌ Test 3.3: Non-assigned member cannot modify task
```
PUT /api/task/1
Token: other_token

{
  "name": "Changed by other",
  ...
}
```
**Expected**: 403 Forbidden

---

### Scenario 4: Completed Task Protection

#### ✅ Test 4.1: Set task as completed
```
PUT /api/task/1
Token: member_token

{
  "name": "API Development",
  "description": "Build REST API endpoints - complete",
  "dueDate": "2024-05-15T00:00:00Z",
  "status": 2,
  "projectId": 1,
  "assignedToId": 2
}
```
**Expected**: 200 OK

#### ❌ Test 4.2: Member cannot update completed task
```
PUT /api/task/1
Token: member_token

{
  "status": 1,  // Try to reopen
  ...
}
```
**Expected**: 400 Bad Request - "Completed tasks cannot be edited"

#### ✅ Test 4.3: Admin can update completed task
```
PUT /api/task/1
Token: admin_token

{
  "status": 1,
  ...
}
```
**Expected**: 200 OK

#### ✅ Test 4.4: Member CAN add comment to completed task
```
POST /api/task/1/comments
Token: member_token

{
  "content": "Great work! Task completed successfully."
}
```
**Expected**: 201 Created

---

### Scenario 5: Task Comments

#### ✅ Test 5.1: Member adds comment to task
```
POST /api/task/1/comments
Token: member_token

{
  "content": "Started working on this task"
}
```
**Expected**: 201 Created
**Result**: Comment created with ID=1

#### ✅ Test 5.2: View all task comments
```
GET /api/task/1/comments
Token: creator_token
```
**Expected**: 200 OK with comment list

#### ❌ Test 5.3: Non-member cannot view comments
```
GET /api/task/1/comments
Token: other_token
```
**Expected**: 403 Forbidden

#### ✅ Test 5.4: Comment creator can delete own comment
```
DELETE /api/task/comments/1
Token: member_token
```
**Expected**: 204 No Content

#### ❌ Test 5.5: Different user cannot delete comment
```
DELETE /api/task/comments/1
Token: creator_token
```
**Expected**: 403 Forbidden (if creator didn't create this comment)

---

### Scenario 6: Project Member Management

#### ✅ Test 6.1: Creator removes member
```
DELETE /api/project/1/members/2
Token: creator_token
```
**Expected**: 200 OK with "Member removed successfully"

#### ❌ Test 6.2: Removed member cannot view project
```
GET /api/project/1
Token: member_token
```
**Expected**: 403 Forbidden

#### ❌ Test 6.3: Removed member cannot view project tasks
```
GET /api/task/1
Token: member_token
```
**Expected**: 403 Forbidden

#### ✅ Test 6.4: Creator can re-add member
```
POST /api/project/1/members/2
Token: creator_token
```
**Expected**: 200 OK

#### ✅ Test 6.5: Get project members
```
GET /api/project/1/members
Token: creator_token
```
**Expected**: 200 OK with member list

---

### Scenario 7: Admin Privileges

#### ✅ Test 7.1: Admin can view all projects
```
GET /api/project
Token: admin_token
```
**Expected**: 200 OK with all projects (not just memberships)

#### ✅ Test 7.2: Admin can modify any task
```
PUT /api/task/1
Token: admin_token

{
  "status": 2,
  ...
}
```
**Expected**: 200 OK

#### ✅ Test 7.3: Admin can delete any task
```
DELETE /api/task/1
Token: admin_token
```
**Expected**: 200 OK

#### ✅ Test 7.4: Admin can delete any comment
```
DELETE /api/task/comments/1
Token: admin_token
```
**Expected**: 204 No Content

---

## Authorization Summary Table

| Resource | Create | Read | Update | Delete |
|----------|--------|------|--------|--------|
| Project | Any User | Creator/Members/Admin | Creator/Admin | Creator/Admin |
| Task | Members Only | Members Only | Creator/Assignee/Admin* | Creator/Admin |
| Comment | Members Only | Members Only | - | Creator/Admin |
| Member | Creator/Admin | Creator/Admin | - | Creator/Admin |

*\*Cannot update completed tasks unless Admin*

---

## Validation Rules

### Project
- Name: Required, max 100 chars
- Description: Required, max 1000 chars

### Task
- Name: Required, max 100 chars
- Description: Required, max 1000 chars
- DueDate: Required, future date recommended
- Status: Required (0, 1, or 2)
- ProjectId: Required, must exist
- AssignedToId: Optional, user must exist if provided

### Comment
- Content: Required, max 500 chars
- TaskId: Required, task must exist

---

## Performance Notes

### Recommended Indexes
- `Users.ProjectId` ✅ Created
- `Tasks.ProjectId` ✅ Exists
- `Tasks.AssignedToId` ✅ Exists
- `TaskComments.TaskId` ✅ Created

### Query Optimization
- Comments are loaded eagerly with tasks
- Task includes assignee information
- Project includes members list

---

## Common Issues & Solutions

### Issue: Cannot add member (404)
**Cause**: User doesn't exist or wrong user ID
**Solution**: Verify the user exists before adding as member

### Issue: Cannot see project after adding as member
**Cause**: Token not refreshed or cache issue
**Solution**: Get new token, clear cache, retry

### Issue: Can modify task even though not assignee
**Cause**: User is task creator or admin
**Solution**: Check `CreatedBy` field or user role

### Issue: Cannot modify completed task
**Cause**: Status = 2 and user is not admin
**Solution**: Use admin token or ask admin to unlock

