# ⚡ CORS FIX - QUICK REFERENCE

## 🚨 Problem
```
ERROR: 405 Method Not Allowed
Request: OPTIONS /api/auth/register
Cause: CORS not configured
```

## ✅ Solution Applied

Added CORS to `Program.cs`:

### 1. Service Registration
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("https://localhost:5001", "http://localhost:5001")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});
```

### 2. Middleware
```csharp
app.UseCors("AllowLocalhost");
```
**Place:** After `UseHttpsRedirection()`, before `UseAuthentication()`

---

## 🔄 How CORS Works

```
Browser preflight (OPTIONS)
         ↓
   API checks origin
         ↓
   ✅ Allowed → 200 OK
         ↓
   Real request (POST)
         ↓
   ✅ Success
```

---

## 🧪 Test It

```powershell
dotnet build
dotnet run

# Then test registration at:
https://localhost:5001/Account/Register
```

---

## ✨ What's Fixed

| Issue | Status |
|-------|--------|
| 405 Error | ✅ Fixed |
| CORS blocked | ✅ Fixed |
| Registration | ✅ Ready |

---

## 📊 Check Network Tab (F12)

Should see:
- ✅ OPTIONS request → 200 OK
- ✅ POST request → 201 Created
- ✅ No CORS errors in Console

---

**Status:** 🎉 CORS ENABLED

