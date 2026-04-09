# Task Management UI - Setup & Usage Guide

## 📱 UI Features

The new standardized UI includes:

### Dashboard
- Overview of projects and tasks
- Quick statistics (total projects, active tasks, completed tasks, comments)
- Recent projects list
- Recent tasks table

### Projects
- **List View**: Card-based project display
- **Create**: Form to create new projects
- **Edit**: Manage project details and members
- **Delete**: Remove projects
- **Members**: Add/remove project members

### Tasks
- **List View**: Table display of tasks with filtering
- **Create**: Comprehensive task creation form
- **Edit**: Full task management with comments
- **Delete**: Remove tasks
- **Status Tracking**: Not Started, In Progress, Completed
- **Comments**: Add and view task comments

### Design Features
- Modern gradient sidebar navigation
- Responsive Bootstrap 5 design
- Color-coded status badges
- Clean card-based layouts
- Loading states and error handling
- Modal dialogs for confirmations
- Real-time API integration

---

## 🚀 Quick Start

### 1. Update Configuration
Edit `TaskManagementWeb/appsettings.json`:
```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000/api"
  }
}
```

### 2. Update Layout (_Layout.cshtml)
The layout has been updated with:
- Sidebar navigation
- Bootstrap 5 styling
- Custom CSS for branding
- Alert notifications

### 3. Create Required Views
All views have been created:
- ✅ Dashboard (Home/Index)
- ✅ Projects (Project/Index, Add, Edit)
- ✅ Tasks (Task/Index, Add, Edit)
- ✅ Layout (_Layout.cshtml)

### 4. Run the Application
```bash
cd TaskManagementWeb
dotnet run
```

Access at: `http://localhost:5001`

---

## 📊 Page Structure

### Dashboard
```
[Navigation Sidebar]
┌─────────────────────────────────────┐
│ Dashboard                           │
├─────────────────────────────────────┤
│ [Stat Cards]                        │
│ ┌──────────┬──────────┬──────────┐  │
│ │Projects  │Tasks     │Completed │  │
│ └──────────┴──────────┴──────────┘  │
│                                     │
│ [Recent Tasks] [Recent Projects]    │
│                                     │
└─────────────────────────────────────┘
```

### Projects
```
[Navigation Sidebar]
┌─────────────────────────────────────┐
│ Projects             [+ New Project] │
├─────────────────────────────────────┤
│ ┌─────────────┐ ┌─────────────┐     │
│ │ Project 1   │ │ Project 2   │     │
│ │ [View][Edit] │ │ [View][Edit] │    │
│ └─────────────┘ └─────────────┘     │
│                                     │
│ ┌─────────────┐                     │
│ │ Project 3   │                     │
│ │ [View][Edit] │                    │
│ └─────────────┘                     │
└─────────────────────────────────────┘
```

### Tasks
```
[Navigation Sidebar]
┌─────────────────────────────────────┐
│ Tasks      [Project ▼] [+ New Task]  │
├─────────────────────────────────────┤
│ Task Name    Due Date  Status Action │
├─────────────────────────────────────┤
│ Task 1       05/15/24  In Progress.. │
│ Task 2       05/20/24  Not Started.. │
│ Task 3       05/10/24  Completed  .. │
└─────────────────────────────────────┘
```

---

## 🎨 UI Components

### Color Scheme
- **Primary**: Purple Gradient (#667eea → #764ba2)
- **Success**: Green (#28a745)
- **Warning**: Yellow (#ffc107)
- **Danger**: Red (#dc3545)
- **Background**: Light Gray (#f8f9fa)

### Status Badges
- **Not Started** (0): Gray
- **In Progress** (1): Yellow/Orange
- **Completed** (2): Green

### Buttons
- **Primary**: Create, Save, Submit actions
- **Secondary**: Cancel, Back navigation
- **Danger**: Delete actions
- **Outline**: Secondary actions

---

## 🔗 Navigation Map

```
Home (Dashboard)
├── Projects
│   ├── View Project Details
│   ├── Add New Project
│   ├── Edit Project
│   │   ├── Manage Members
│   │   └── View Tasks
│   └── Delete Project
│
└── Tasks
    ├── View All Tasks
    ├── Add New Task
    ├── Edit Task
    │   ├── View Comments
    │   ├── Add Comment
    │   └── Delete Comment
    └── Delete Task
```

---

## 📝 API Integration

Each view communicates with the API:

### Projects
- `GET /api/project` - List projects
- `GET /api/project/{id}` - Get project details
- `POST /api/project` - Create project
- `PUT /api/project/{id}` - Update project
- `DELETE /api/project/{id}` - Delete project
- `GET /api/project/{id}/members` - List members
- `POST /api/project/{id}/members/{userId}` - Add member
- `DELETE /api/project/{id}/members/{userId}` - Remove member

### Tasks
- `GET /api/task/{projectId}` - List project tasks
- `GET /api/task/task/{id}` - Get task details
- `POST /api/task` - Create task
- `PUT /api/task/{id}` - Update task
- `DELETE /api/task/{id}` - Delete task

### Comments
- `GET /api/task/{taskId}/comments` - List comments
- `POST /api/task/{taskId}/comments` - Add comment
- `DELETE /api/task/comments/{commentId}` - Delete comment

---

## 🔐 Authentication

The UI expects JWT tokens stored in localStorage:
```javascript
localStorage.setItem('authToken', 'your-jwt-token');
```

All API requests include:
```
Authorization: Bearer {authToken}
```

---

## 📱 Responsive Design

The UI is fully responsive:
- **Desktop**: Full sidebar + main content
- **Tablet**: Sidebar collapses, adjusted layouts
- **Mobile**: Stack sidebar at top, optimized for touch

---

## ⚙️ Configuration

### appsettings.json
```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000/api"
  }
}
```

### appsettings.Production.json
```json
{
  "ApiSettings": {
    "BaseUrl": "https://your-api-url/api"
  }
}
```

---

## 🐛 Troubleshooting

### Issue: API calls fail with 401
**Solution**: Ensure JWT token is in localStorage

### Issue: Projects not loading
**Solution**: Check API is running on correct port (5000)

### Issue: 404 on API endpoints
**Solution**: Verify API URL in appsettings.json

### Issue: CORS errors
**Solution**: Ensure API allows requests from your frontend URL

---

## 📦 Files Modified/Created

### Modified
- `Program.cs` - Added API client configuration
- `Views/Shared/_Layout.cshtml` - New responsive layout
- `Views/Home/Index.cshtml` - Dashboard page
- `appsettings.json` - API settings

### Created
- `Views/Task/Index.cshtml` - Task list view
- `Views/Task/Add.cshtml` - Create task form
- `Views/Task/Edit.cshtml` - Edit task form
- `Views/Project/Edit.cshtml` - Enhanced project editor

---

## 🎯 Next Steps

1. **Start the API**: Run TaskManagementApp on port 5000
2. **Start the Web UI**: Run TaskManagementWeb on port 5001
3. **Authenticate**: Use login to get JWT token
4. **Test Features**: Create projects, tasks, and comments
5. **Customize**: Modify colors/layout in _Layout.cshtml

---

## 📚 Customization

### Change Colors
Edit the `:root` variables in `_Layout.cshtml`:
```css
:root {
    --primary-color: #YOUR_COLOR;
    --sidebar-width: 260px;
}
```

### Change Sidebar Width
```css
--sidebar-width: 300px; /* adjust as needed */
```

### Add Custom CSS
Create `wwwroot/css/custom.css` and link in layout

---

## ✅ Checklist

- [ ] API running on http://localhost:5000
- [ ] Web UI running on http://localhost:5001
- [ ] JWT token in localStorage
- [ ] Can create projects
- [ ] Can create tasks
- [ ] Can add comments
- [ ] Can manage members
- [ ] Responsive design works on mobile

---

**Status**: ✅ UI Complete & Ready
**Last Updated**: 2024
**Version**: 1.0

