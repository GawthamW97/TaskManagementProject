# Task Management API - Documentation

## Base URL
```
http://localhost:5000/api
or
https://app-taskmanagement-cenfran-dev-01.azurewebsites.net/api (Production)
```

## Authentication
All endpoints require JWT Bearer token in the Authorization header:
```
Authorization: Bearer <your_jwt_token>
```

---

## Project Endpoints

### Get All User Projects
```http
GET /api/project
```
**Description**: Retrieve all projects where the user is a member or creator
**Authorization**: Required (Admin sees all projects)
**Response**:
```json
[
  {
    "id": 1,
    "name": "Project Name",
    "description": "Project description",
    "createdDate": "2024-04-09T10:30:00Z",
    "createdBy": "user@example.com",
    "updatedDate": "2024-04-09T10:30:00Z",
    "updatedBy": "user@example.com",
    "users": []
  }
]
```

### Get Project Details
```http
GET /api/project/{id}
```
**Parameters**:
- `id` (integer, required): Project ID

**Authorization**: Project Creator, Project Members, or Admins
**Response**: Single project object

### Create Project
```http
POST /api/project
Content-Type: application/json

{
  "name": "New Project",
  "description": "Project description"
}
```
**Authorization**: Required (Any authenticated user)
**Response**: Created project object (201 Created)

### Update Project
```http
PUT /api/project/{id}
Content-Type: application/json

{
  "name": "Updated Project Name",
  "description": "Updated description"
}
```
**Authorization**: Project Creator or Admins
**Response**: Updated project object

### Delete Project
```http
DELETE /api/project/{id}
```
**Authorization**: Project Creator or Admins
**Response**: Deleted project object

### Get Project Members
```http
GET /api/project/{projectId}/members
```
**Authorization**: Project Members, Project Creator, or Admins
**Response**:
```json
[
  {
    "id": 1,
    "fullName": "John Doe",
    "email": "john@example.com",
    "username": "johndoe",
    "role": "User"
  }
]
```

### Add Project Member
```http
POST /api/project/{projectId}/members/{userId}
```
**Parameters**:
- `projectId` (integer, required): Project ID
- `userId` (integer, required): User ID to add

**Authorization**: Project Creator or Admins
**Response**: 
```json
{
  "message": "Member added successfully"
}
```

### Remove Project Member
```http
DELETE /api/project/{projectId}/members/{userId}
```
**Authorization**: Project Creator or Admins
**Response**: 
```json
{
  "message": "Member removed successfully"
}
```

---

## Task Endpoints

### Get Project Tasks
```http
GET /api/task/{projectId}
```
**Parameters**:
- `projectId` (integer, required): Project ID

**Authorization**: Project Members, Project Creator, or Admins
**Response**:
```json
[
  {
    "id": 1,
    "name": "Task Name",
    "description": "Task description",
    "dueDate": "2024-05-01T00:00:00Z",
    "status": 1,
    "projectId": 1,
    "assignedToId": 2,
    "createdDate": "2024-04-09T10:30:00Z",
    "createdBy": "user@example.com",
    "updatedDate": "2024-04-09T10:30:00Z",
    "updatedBy": "user@example.com",
    "comments": []
  }
]
```

### Get Task Details
```http
GET /api/task/task/{id}
```
**Parameters**:
- `id` (integer, required): Task ID

**Authorization**: Project Members or Admins
**Response**: Single task object with comments

### Create Task
```http
POST /api/task
Content-Type: application/json

{
  "name": "New Task",
  "description": "Task description",
  "dueDate": "2024-05-01T00:00:00Z",
  "status": 0,
  "projectId": 1,
  "assignedToId": 2
}
```
**Authorization**: Project Members
**Request Fields**:
- `name` (string, required, max 100): Task name
- `description` (string, required, max 1000): Task description
- `dueDate` (datetime, required): Due date
- `status` (integer, required): 0=Not Started, 1=In Progress, 2=Completed
- `projectId` (integer, required): Project ID
- `assignedToId` (integer, optional): User ID to assign to

**Response**: Created task object (201 Created)

### Update Task
```http
PUT /api/task/{id}
Content-Type: application/json

{
  "name": "Updated Task",
  "description": "Updated description",
  "dueDate": "2024-05-10T00:00:00Z",
  "status": 1,
  "projectId": 1,
  "assignedToId": 2
}
```
**Authorization**: Task Creator, Assigned User, or Admins
**Restrictions**: 
- Cannot update completed tasks (status=2) unless Admin
- Only assignee or creator can modify (unless admin)

**Response**: Updated task object

### Delete Task
```http
DELETE /api/task/{id}
```
**Authorization**: Task Creator or Admins
**Response**: Deleted task object

---

## Task Comment Endpoints

### Get Task Comments
```http
GET /api/task/{taskId}/comments
```
**Parameters**:
- `taskId` (integer, required): Task ID

**Authorization**: Project Members
**Response**:
```json
[
  {
    "id": 1,
    "content": "Comment text",
    "createdDate": "2024-04-09T10:30:00Z",
    "createdBy": "user@example.com",
    "taskId": 1
  }
]
```

### Add Comment to Task
```http
POST /api/task/{taskId}/comments
Content-Type: application/json

{
  "content": "Comment text here"
}
```
**Parameters**:
- `taskId` (integer, required): Task ID

**Request Fields**:
- `content` (string, required, max 500): Comment text

**Authorization**: Project Members
**Restrictions**: 
- Allowed even on completed tasks
- Only project members can comment

**Response**: Created comment object (201 Created)

### Delete Comment
```http
DELETE /api/task/comments/{commentId}
```
**Parameters**:
- `commentId` (integer, required): Comment ID

**Authorization**: Comment Creator or Admins
**Response**: 204 No Content

---

## Status Codes

| Code | Description |
|------|-------------|
| 200 | OK - Successful GET, PUT, DELETE |
| 201 | Created - Successful POST |
| 204 | No Content - Successful DELETE (no response body) |
| 400 | Bad Request - Invalid input or business logic error |
| 401 | Unauthorized - Missing or invalid token |
| 403 | Forbidden - Insufficient permissions |
| 404 | Not Found - Resource not found |
| 500 | Internal Server Error |

---

## Error Response Format
```json
{
  "error": "Error message description",
  "detail": "Additional error details if available"
}
```

---

## Common Business Rules

### Project Access
- **Create**: Any authenticated user
- **View**: Project Creator, Members, or Admins
- **Modify**: Project Creator or Admins only
- **Delete**: Project Creator or Admins only
- **Manage Members**: Project Creator or Admins only

### Task Access
- **Create**: Project Members only
- **View**: Project Members only
- **Modify**: Task Creator, Assigned User, or Admins
  - ❌ Cannot modify if status=2 (Completed) unless Admin
- **Delete**: Task Creator or Admins only

### Comment Access
- **Create**: Project Members (always allowed, even on completed tasks)
- **View**: Project Members only
- **Delete**: Comment Creator or Admins only

---

## Task Status Values

| Value | Status | Description |
|-------|--------|-------------|
| 0 | Not Started | Task has not begun |
| 1 | In Progress | Task is currently being worked on |
| 2 | Completed | Task is finished (cannot be edited unless by admin) |

---

## Examples

### Example 1: Create a Project
```bash
curl -X POST http://localhost:5000/api/project \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Website Redesign",
    "description": "Redesign company website"
  }'
```

### Example 2: Create a Task in Project
```bash
curl -X POST http://localhost:5000/api/task \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Design Home Page",
    "description": "Create mockups and designs",
    "dueDate": "2024-05-15T00:00:00Z",
    "status": 0,
    "projectId": 1,
    "assignedToId": 2
  }'
```

### Example 3: Add Comment to Task
```bash
curl -X POST http://localhost:5000/api/task/1/comments \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Added the first draft of the design"
  }'
```

### Example 4: Add Member to Project
```bash
curl -X POST http://localhost:5000/api/project/1/members/3 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## Rate Limiting
Currently not implemented. Recommended for future deployment.

## CORS
Configured for development. Review before production deployment.

## API Versioning
Current version: v1 (via header or query parameter if configured)

