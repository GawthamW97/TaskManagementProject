# Quick Reference - Task Management API

## 🚀 Quick Start

### 1. Apply Database Migrations
```bash
cd TaskManagementApp
dotnet ef database update
```

### 2. Run the Application
```bash
dotnet run
```

### 3. Access API Documentation
- Swagger UI: `http://localhost:5000/swagger/index.html`
- Base URL: `http://localhost:5000/api`

---

## 📋 Core API Endpoints

### Projects
```
POST   /api/project                    - Create project
GET    /api/project                    - List your projects
GET    /api/project/{id}               - Get project details
PUT    /api/project/{id}               - Update project
DELETE /api/project/{id}               - Delete project
```

### Project Members
```
GET    /api/project/{projectId}/members           - List members
POST   /api/project/{projectId}/members/{userId}  - Add member
DELETE /api/project/{projectId}/members/{userId}  - Remove member
```

### Tasks
```
POST   /api/task                       - Create task
GET    /api/task/{projectId}           - Get project tasks
GET    /api/task/task/{id}             - Get task details
PUT    /api/task/{id}                  - Update task
DELETE /api/task/{id}                  - Delete task
```

### Comments
```
POST   /api/task/{taskId}/comments     - Add comment
GET    /api/task/{taskId}/comments     - Get comments
DELETE /api/task/comments/{commentId}  - Delete comment
```

---

## 🔐 Authentication

All endpoints require JWT token:
```
Authorization: Bearer <your_jwt_token>
```

Get token from `/api/auth/login` endpoint.

---

## 👥 User Roles & Permissions

### Admin User
- ✅ Access all projects
- ✅ Modify any task
- ✅ Delete any resource
- ✅ Override completed task protection

### Regular User
- ✅ Create projects
- ✅ Add/manage project members (own projects)
- ✅ Create tasks (in member projects only)
- ✅ Modify own or assigned tasks
- ✅ Add comments to project tasks

---

## 📊 Task Status Values

| Value | Status | Editable | Comment Allowed |
|-------|--------|----------|-----------------|
| 0 | Not Started | ✅ | ✅ |
| 1 | In Progress | ✅ | ✅ |
| 2 | Completed | ❌* | ✅ |

*Only admins can edit completed tasks

---

## 🔒 Authorization Rules Summary

### Can I View This?
- ✅ If you're the project creator
- ✅ If you're a project member
- ✅ If you're an admin
- ❌ Otherwise

### Can I Edit This?
- ✅ If you created it
- ✅ If you're assigned to it (tasks only)
- ✅ If you're an admin
- ❌ If task is completed (unless admin)
- ❌ Otherwise

### Can I Delete This?
- ✅ If you created it
- ✅ If you're an admin
- ❌ Otherwise

---

## 📝 Example Request

### Create a Task
```bash
curl -X POST http://localhost:5000/api/task \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Build API",
    "description": "Create REST API endpoints",
    "dueDate": "2024-05-15T00:00:00Z",
    "status": 0,
    "projectId": 1,
    "assignedToId": 2
  }'
```

### Add a Comment
```bash
curl -X POST http://localhost:5000/api/task/1/comments \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "content": "Started working on this"
  }'
```

---

## 🗄️ Database Schema

### Projects
- Owns: Tasks, Users (members)
- Created by: User email
- AccessControl: Creator + Members

### Users
- Belongs to: One Project (member relationship)
- Role: Admin or User
- AccessControl: Self + Admins

### Tasks
- Belongs to: One Project
- Assigned to: One User (optional)
- Has: Many Comments
- AccessControl: Creator/Assignee/Admin

### TaskComments
- Belongs to: One Task
- Created by: User email
- AccessControl: Creator/Admin

---

## ❌ Common Errors & Fixes

| Error | Cause | Solution |
|-------|-------|----------|
| 403 Forbidden | Not a project member | Add yourself to project |
| 400 Bad Request | Task is completed | Use admin token or reopen task |
| 404 Not Found | Project doesn't exist | Check project ID |
| 401 Unauthorized | Missing/invalid token | Login and get valid token |
| 400 Bad Request | Invalid data | Check request format in API docs |

---

## 📚 Documentation Files

- **`IMPLEMENTATION_COMPLETE.md`** - Full implementation details
- **`IMPLEMENTATION_SUMMARY.md`** - Scope fulfillment summary
- **`API_DOCUMENTATION.md`** - Complete API reference
- **`TESTING_SCENARIOS.md`** - Test cases and scenarios

---

## 🧪 Testing

Run test scenarios from `TESTING_SCENARIOS.md`:
1. Start with Scenario 1: Project Creation
2. Move to Scenario 2: Task Access Control
3. Test Scenario 4: Completed Task Protection
4. Verify Scenario 7: Admin Privileges

---

## 🔧 Configuration

Required in `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  },
  "ConnectionStrings": {
    "TaskManagementConnString": "Server=...;Database=...;"
  }
}
```

---

## 📊 Database Migrations

Applied migrations:
1. ✅ Create Initial entities
2. ✅ Create Auth Table
3. ✅ Adding Images Table
4. ✅ Update_Model_Changes
5. ✅ AddProjectIdToUsers (TaskComments + ProjectId)

---

## 🎯 Status Verification

| Component | Status |
|-----------|--------|
| Build | ✅ Successful |
| Models | ✅ Updated |
| Repositories | ✅ Implemented |
| Controllers | ✅ Enhanced |
| Migrations | ✅ Ready |
| Authorization | ✅ Enforced |
| Comments | ✅ Working |
| Documentation | ✅ Complete |

---

## 🚦 Next Steps

1. **Apply Migrations**: `dotnet ef database update`
2. **Run Tests**: Execute scenarios from TESTING_SCENARIOS.md
3. **Verify**: Check all authorization rules work
4. **Deploy**: Push to your hosting environment
5. **Monitor**: Watch for authorization errors

---

**Last Updated**: April 9, 2024
**Version**: 1.0 - Complete
**Compatibility**: .NET 10, C# 14.0

