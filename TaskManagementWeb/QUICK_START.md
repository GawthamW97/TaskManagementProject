# 🚀 QUICK START GUIDE

## ⚡ 30-Second Setup

### 1. Start API
```bash
cd TaskManagementApp
dotnet run
# Runs on http://localhost:5000
```

### 2. Start UI
```bash
cd TaskManagementWeb
dotnet run
# Runs on http://localhost:5001
```

### 3. Open Browser
```
http://localhost:5001
```

### 4. Get JWT Token
```javascript
// From API login endpoint, store in localStorage
localStorage.setItem('authToken', 'your-token-here');
```

### 5. Start Using
- Dashboard shows overview
- Create projects and tasks
- Manage team members
- Track progress

---

## 📱 What You Can Do

### Projects
✅ Create  
✅ Edit  
✅ Delete  
✅ Add members  
✅ Remove members  

### Tasks
✅ Create  
✅ Edit  
✅ Delete  
✅ Change status  
✅ Assign to user  
✅ Add comments  
✅ View comments  
✅ Delete comments  

### Dashboard
✅ View statistics  
✅ See recent projects  
✅ See recent tasks  
✅ Quick actions  

---

## 🎨 Key Pages

| Page | URL | Purpose |
|------|-----|---------|
| Dashboard | http://localhost:5001 | Overview |
| Projects | http://localhost:5001/Project | Project list |
| Create Project | http://localhost:5001/Project/Add | New project |
| Tasks | http://localhost:5001/Task | Task list |
| Create Task | http://localhost:5001/Task/Add | New task |

---

## 🔧 Configuration

### Update API URL
**File**: `TaskManagementWeb/appsettings.json`

```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000/api"
  }
}
```

For production, update BaseUrl to your API domain.

---

## 🔐 Authentication

### How It Works
1. Get JWT token from API
2. Store in browser localStorage
3. All API calls include token in header
4. Server validates on each request

### Example
```javascript
// Get token from login
const response = await fetch('http://localhost:5000/api/auth/login', {
    method: 'POST',
    body: JSON.stringify({ email, password })
});

const data = await response.json();
localStorage.setItem('authToken', data.token);

// It's now automatically sent with all API calls
```

---

## 📧 API Integration

### Endpoints Used

**Projects**
- `GET /api/project` - List all
- `POST /api/project` - Create
- `PUT /api/project/{id}` - Update
- `DELETE /api/project/{id}` - Delete

**Tasks**
- `GET /api/task/{projectId}` - List for project
- `POST /api/task` - Create
- `PUT /api/task/{id}` - Update
- `DELETE /api/task/{id}` - Delete

**Members**
- `GET /api/project/{id}/members` - List members
- `POST /api/project/{id}/members/{userId}` - Add
- `DELETE /api/project/{id}/members/{userId}` - Remove

**Comments**
- `GET /api/task/{taskId}/comments` - List
- `POST /api/task/{taskId}/comments` - Add
- `DELETE /api/task/comments/{id}` - Delete

---

## 🎨 Customize Colors

### Edit Layout
**File**: `TaskManagementWeb/Views/Shared/_Layout.cshtml`

Find the style section:
```html
<style>
    :root {
        --primary-color: #667eea;
        --sidebar-width: 260px;
    }
</style>
```

Change `--primary-color` to your color (hex, rgb, etc.)

---

## 🐛 Troubleshooting

### "API call fails with 401"
→ Check JWT token in localStorage

### "Projects not loading"
→ Ensure API is running on port 5000

### "404 Not Found on API"
→ Check API URL in appsettings.json

### "Styles not loading"
→ Hard refresh browser (Ctrl+Shift+R)

### "Members not showing"
→ Verify API endpoint returns members

---

## 📱 Mobile Testing

The UI is fully responsive:
- Open in Chrome DevTools (F12)
- Toggle device toolbar
- Test on mobile size
- All features work on mobile

---

## ✅ Testing Checklist

Quick test flow:
1. [ ] Dashboard loads
2. [ ] Can create project
3. [ ] Can edit project
4. [ ] Can create task
5. [ ] Can edit task
6. [ ] Can add comment
7. [ ] Can delete task
8. [ ] Mobile responsive
9. [ ] Errors handled
10. [ ] Notifications shown

---

## 🚀 Deployment

### Local
1. Ensure API running on port 5000
2. Ensure UI running on port 5001
3. Open http://localhost:5001

### Azure/Cloud
1. Update appsettings.json with API URL
2. Deploy to app service
3. Configure HTTPS
4. Set environment variables

---

## 💡 Tips & Tricks

### Speed Up Development
- Open browser console (F12)
- Check network tab for API calls
- Check console for errors
- Use localStorage in console

### Debug API Calls
```javascript
// In browser console
localStorage.getItem('authToken')  // Check token
fetch('http://localhost:5000/api/project').then(r => r.json())  // Test API
```

### Clear Data
```javascript
localStorage.clear()  // Clear all stored data
```

---

## 📞 Support

### Check Logs
1. Browser console (F12)
2. Terminal output
3. API response messages
4. Network tab errors

### Common Solutions
- Hard refresh (Ctrl+Shift+R)
- Clear localStorage
- Restart both servers
- Check port availability

---

## 🎯 Feature Summary

| Feature | Status | Level |
|---------|--------|-------|
| Dashboard | ✅ | Basic |
| Projects | ✅ | Full |
| Tasks | ✅ | Full |
| Comments | ✅ | Full |
| Members | ✅ | Full |
| Mobile | ✅ | Full |
| Auth | ✅ | JWT |

---

## 📚 More Info

For detailed documentation:
- `README.md` - Overview
- `UI_SETUP_GUIDE.md` - Setup instructions
- `UI_IMPLEMENTATION_SUMMARY.md` - Features
- `CHANGES.md` - What changed

---

## 🎉 You're Ready!

Everything is set up. Now:

1. Start the servers
2. Get a token
3. Start managing tasks!

Happy coding! 🚀

---

**Version**: 1.0  
**Status**: ✅ Ready  
**Last Updated**: 2024

