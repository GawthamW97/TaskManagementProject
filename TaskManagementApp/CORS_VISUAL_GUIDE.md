# 📊 CORS - VISUAL GUIDE

## 🎯 The Problem

```
Frontend (localhost:5001)
        │
        │ POST to different origin
        │ (localhost:7198)
        │
        ▼
Browser Security Check
        │
        ├─ "Is this origin allowed?"
        │
        └─ Sends preflight OPTIONS request
        
        
API (localhost:7198)
        │
        ├─ No CORS configured
        │
        ├─ No handler for OPTIONS
        │
        └─ Returns: 405 Method Not Allowed ❌

Frontend blocked by browser ❌
Registration fails ❌
```

---

## ✅ The Solution

```
Frontend (localhost:5001)
        │
        │ POST to different origin
        │ (localhost:7198)
        │
        ▼
Browser Security Check
        │
        ├─ "Is this origin allowed?"
        │
        └─ Sends preflight OPTIONS request
        
        
API (localhost:7198) ← CORS Configured ✅
        │
        ├─ CORS middleware active
        │
        ├─ Checks origin: ✅ localhost:5001 allowed
        │
        ├─ Allows method: ✅ POST allowed
        │
        ├─ Allows headers: ✅ All headers allowed
        │
        └─ Returns: 200 OK with CORS headers ✅

Browser sees ✅ Preflight succeeded
        │
        └─ Allows real POST request
        
Real POST Request sent ✅
        │
        ▼
API processes registration ✅
        │
        └─ Returns JWT token ✅

Frontend receives token ✅
Registration succeeds ✅
```

---

## 🔀 Side-by-Side Comparison

### ❌ WITHOUT CORS
```
Step 1: OPTIONS /api/auth/register
        ↓
        API: "I don't handle OPTIONS"
        ↓
        Status: 405 Not Allowed
        ↓
        Browser: "Request blocked"
        ↓
Result: FAILURE ❌
```

### ✅ WITH CORS
```
Step 1: OPTIONS /api/auth/register
        ↓
        API CORS: "Checking origin..."
        ↓
        "localhost:5001? Yes, allowed"
        ↓
        Status: 200 OK
        ↓
        Browser: "Allowed, proceed"
        ↓
Step 2: POST /api/auth/register
        ↓
        API: "Processing..."
        ↓
        Status: 201 Created (JWT token)
        ↓
Result: SUCCESS ✅
```

---

## 📋 CORS Configuration

```csharp
// Register CORS policies
builder.Services.AddCors(options =>
{
    // Specific origins (recommended)
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder
        .WithOrigins("https://localhost:5001")
        .AllowAnyMethod()      // GET, POST, PUT, DELETE, etc.
        .AllowAnyHeader()      // Content-Type, Authorization, etc.
        .AllowCredentials();   // Allow cookies & auth headers
    });
});

// Enable CORS middleware
app.UseCors("AllowLocalhost");
```

---

## 🔧 Middleware Order (IMPORTANT)

```
app.UseHttpsRedirection();      ← 1st
         ↓
app.UseCors("AllowLocalhost");  ← 2nd (MUST BE HERE)
         ↓
app.UseAuthentication();        ← 3rd
         ↓
app.UseAuthorization();         ← 4th
         ↓
app.MapControllers();           ← 5th
```

**Why order matters:**
- CORS must run before authentication
- CORS must handle OPTIONS before auth middleware
- Otherwise: 405 error!

---

## 🌐 What Gets Allowed

### Methods
- ✅ GET
- ✅ POST
- ✅ PUT
- ✅ DELETE
- ✅ PATCH
- ✅ OPTIONS

### Headers
- ✅ Content-Type
- ✅ Authorization
- ✅ Custom headers

### Origins
```
WithOrigins(
    "https://localhost:5001",  ✅ Frontend HTTPS
    "https://localhost:5000",  ✅ Alt HTTPS
    "http://localhost:5001",   ✅ Frontend HTTP
    "http://localhost:5000"    ✅ Alt HTTP
)
```

---

## 🎯 Flow Chart

```
Browser Makes Request
        │
        ├─ Same origin? ──→ YES → Send directly ✅
        │
        └─ Different origin? ──→ YES → Preflight check
                                    │
                                    ├─ Send OPTIONS
                                    │
                                    ├─ CORS allowed?
                                    │
                                    ├─ YES → Send real request ✅
                                    │
                                    └─ NO → Block request ❌
```

---

## 📈 HTTP Status Codes

| Code | Status | Meaning |
|------|--------|---------|
| **200** | OK | Preflight allowed ✅ |
| **201** | Created | Resource created (registration) ✅ |
| **405** | Not Allowed | CORS not configured ❌ |
| **401** | Unauthorized | Token invalid ❌ |
| **403** | Forbidden | Not allowed ❌ |

---

## 🔐 CORS Policies

### Development
```csharp
.WithOrigins("https://localhost:5001")
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials()
```

### Testing (Temporary)
```csharp
.AllowAnyOrigin()        // ⚠️ Any domain
.AllowAnyMethod()
.AllowAnyHeader()
// Note: Can't use AllowCredentials()
```

### Production
```csharp
.WithOrigins("https://yourdomain.com")
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials()
```

---

## ✨ Summary

**Before:** 405 error → Registration blocked  
**After:** CORS enabled → Registration works ✅

**Key Point:** CORS preflight (OPTIONS) must succeed before actual request!

