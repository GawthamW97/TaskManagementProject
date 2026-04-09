# 🔍 HOW TO DEBUG API INTEGRATION ISSUES

## The Lesson From This Bug

When API works in Swagger but not in UI:

### **Step 1: Check What Swagger Sends** ✅
```
Open Swagger UI
Click "Try it out"
Look at the "Request body" section
Note the exact property names
```

### **Step 2: Check What UI Sends** ⚠️
```
Open DevTools (F12)
Go to Network tab
Make a request
Click on the request
Look at "Request payload"
Compare property names
```

### **Step 3: Match Them** 🎯
```
If they don't match:
  ❌ Swagger: { "Username": "...", "Password": "..." }
  ❌ UI: { "email": "...", "password": "..." }
  
Fix the JavaScript:
  ✅ UI: { "username": "...", "password": "..." }
```

---

## Your Specific Bug

### **What Swagger Used (Correct)**
```json
Login:
{
  "username": "user@example.com",
  "password": "password123"
}

Register:
{
  "username": "user@example.com",
  "password": "password123",
  "roles": ["user"]
}
```

### **What UI Used (Wrong)**
```json
Login:
{
  "email": "user@example.com",           ← ❌ Should be "username"
  "password": "password123"
}

Register:
{
  "firstName": "John",                   ← ❌ Not in API
  "lastName": "Doe",                     ← ❌ Not in API
  "email": "user@example.com",           ← ❌ Should be "username"
  "password": "password123"
}
```

---

## How to Spot This Quickly

### **In Browser DevTools (F12):**

**1. Go to Network tab**
```
Click to start recording
Try to login/register
Look at the POST request
Click on it
Go to "Request" or "Payload" tab
```

**2. Compare with Swagger**
```
Open Swagger UI at https://localhost:7198/swagger
Try the same request
Look at "Request body"
Compare property names
```

**3. If Different:**
```
You found the bug! Update JavaScript to match Swagger.
```

---

## The Three Questions to Ask

### **Q1: Does Swagger work?**
- ✅ Yes → API is correct
- ❌ No → API code needs fixing

### **Q2: Does UI work?**
- ✅ Yes → Done!
- ❌ No → Check payload

### **Q3: Are property names the same?**
- ✅ Yes → Problem is elsewhere
- ❌ No → Fix property names! ← **Your issue**

---

## Common API Integration Mistakes

| Mistake | Example | How to Spot |
|---------|---------|-----------|
| **Wrong property name** | `email` vs `username` | Compare with Swagger |
| **Wrong endpoint** | `/login` vs `/auth/login` | Check Network tab |
| **Wrong HTTP method** | GET instead of POST | Check Network tab |
| **Wrong port** | 5000 vs 7198 | Check Network tab URL |
| **Missing CORS** | OPTIONS returns 405 | Check Console for CORS error |
| **Wrong JSON format** | `form-data` vs `application/json` | Check Request headers |

---

## Debugging Checklist

```
□ Can access API in browser? (https://localhost:7198/swagger)
□ Does request work in Swagger?
□ Does request fail in UI form?
□ Check Console tab for errors
□ Check Network tab for request details
□ Compare property names: Swagger vs UI
□ Match them exactly
□ Rebuild
□ Test again
```

---

## Quick Network Tab Inspection

```
Press F12 → Network → Fill form → Submit

Look for: POST https://localhost:7198/api/auth/login

Status codes:
✅ 200 OK        → Success
✅ 201 Created   → Created
❌ 400 Bad Req   → Validation failed (payload issue)
❌ 405 Not Found → Wrong endpoint
❌ 500 Server    → API error
```

---

## The Root Cause

**Your UI was built without checking what the API actually expects.**

Solution:
1. Check Swagger first
2. Build UI to match
3. Test both together

---

## Files to Check

```
Frontend:
  TaskManagementWeb/wwwroot/js/auth-login.js ✅ Fixed
  TaskManagementWeb/wwwroot/js/auth-register.js ✅ Fixed

Backend:
  TaskManagementApp/Controllers/AuthController.cs
  TaskManagementApp/Models/DTOs/LoginRequestDTO.cs
  TaskManagementApp/Models/DTOs/RegisterRequestDTO.cs
```

---

## Prevention

**Always start with backend DTOs:**

```csharp
// What the API expects
public class LoginRequestDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
}
```

**Then build frontend to match:**

```javascript
// Send exactly what API expects
body: JSON.stringify({
    username: email,
    password: password
})
```

---

## Tools That Help

1. **Swagger/OpenAPI** - See exact API contract
2. **Postman** - Test API before UI
3. **DevTools Network** - Inspect actual requests
4. **Browser Console** - See JavaScript errors
5. **API Logs** - See what API received

---

## Your Bug Summary

**Problem:** UI sending `{ email, password }` instead of `{ username, password }`  
**Root Cause:** Didn't check Swagger before building UI  
**Fix:** Rename properties to match API contract  
**Time to Fix:** 2 minutes once identified  

---

## Lesson Learned

**Backend drives the contract. Frontend must follow.**

Not the other way around!

