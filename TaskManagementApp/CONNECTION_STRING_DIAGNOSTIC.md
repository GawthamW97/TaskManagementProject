# 🔗 CONNECTION STRING & REGISTRATION DIAGNOSTIC

## Your Current Configuration

### Frontend (TaskManagementWeb)
```
appsettings.json:
├─ ApiSettings.BaseUrl: http://localhost:5000/api ✅ CORRECT
└─ Environment: Any (Development/Production)
```

### API (TaskManagementApp) - Development
```
appsettings.json:
├─ TaskManagementConnString: 
│  └─ Server=localhost (SQL Server)
│     Database=TaskManagementDb
│     Trusted_Connection=True
│     └─ ✅ Pointing to LOCAL SQL Server
├─ IdentityConnectionString:
│  └─ Server=localhost 
│     Database=NZWalksIdentityDb
│     └─ ✅ Pointing to LOCAL SQL Server
└─ Environment: Development
```

### API (TaskManagementApp) - Production
```
appsettings.Production.json:
├─ ConnectionStrings pointing to:
│  └─ sqlserver-taskmanagement-francent-dev-01.database.windows.net
│     (Azure SQL Server)
│     └─ ✅ Pointing to AZURE when deployed
└─ Environment: Production
```

---

## 🎯 Why Registration Fails - Diagnostic Chart

```
Registration Request
      │
      ▼
Frontend (localhost:5001)
      │
      ├─ Validates form
      ├─ Sends to API
      │
      ▼
Is API Running?
      │
      ├─ NO  → ❌ "Failed to fetch" error
      │        └─ Solution: dotnet run in API folder
      │
      └─ YES ─ ▼
            Is Database Connected?
                 │
                 ├─ NO  → ❌ "500 Internal Server Error"
                 │        └─ Check: SQL Server running?
                 │        └─ Check: Database created?
                 │
                 └─ YES ─ ▼
                       Can Create User?
                            │
                            ├─ NO  → ❌ Error message
                            │        └─ Check: User already exists?
                            │
                            └─ YES ─ ▼
                                  ✅ Account Created!
                                  ├─ Token generated
                                  └─ Redirect to login
```

---

## 🚨 Common Issues & Quick Fixes

### Issue #1: "Failed to fetch"
```
Symptom: Cannot reach API
Cause:   API not running on port 5000
Fix:     
  Terminal> cd TaskManagementApp
  Terminal> dotnet run
  
Expected: "Now listening on: http://localhost:5000"
```

### Issue #2: "500 Internal Server Error"
```
Symptom: API responds but registration fails
Cause:   Database connection failed
Fix:     
  Check if SQL Server is running:
  Terminal> Get-Service MSSQL$SQLEXPRESS | Select Status
  
  Apply migrations:
  Terminal> dotnet ef database update
```

### Issue #3: "CORS Error"
```
Symptom: Error mentions CORS in console
Cause:   API missing CORS configuration
Fix:     
  Add to Program.cs:
  builder.Services.AddCors(options =>
  {
      options.AddPolicy("AllowAll", builder =>
      {
          builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
      });
  });
  
  app.UseCors("AllowAll");
```

---

## 🔍 Verification Steps

### Step 1: Verify API Running
```powershell
# Open PowerShell and run:
netstat -ano | findstr :5000

# Should show:
# TCP    0.0.0.0:5000    0.0.0.0:0    LISTENING    (PID)
#
# If nothing shows, API is NOT running ❌
```

### Step 2: Verify SQL Server
```powershell
# Check service status:
Get-Service MSSQL$SQLEXPRESS | Select Status

# Should show:
# Status
# ------
# Running

# If Stopped, start it:
Start-Service MSSQL$SQLEXPRESS
```

### Step 3: Verify Database Exists
```powershell
# Connect to SQL Server:
sqlcmd -S localhost

# In sqlcmd:
SELECT name FROM sys.databases WHERE name LIKE '%Task%';
GO

# Should show:
# TaskManagementDb
# NZWalksIdentityDb
```

### Step 4: Test API Endpoint
```powershell
# Test without auth (should work):
Invoke-WebRequest -Uri "http://localhost:5000/api" -Method GET

# Test registration:
$body = @{
    email = "test@example.com"
    password = "TestPassword123!"
    roles = @("user")
} | ConvertTo-Json

$response = Invoke-WebRequest -Uri "http://localhost:5000/api/auth/register" `
    -Method POST `
    -ContentType "application/json" `
    -Body $body

$response.Content
```

---

## 🎯 Environment Check Matrix

| Component | Local (Dev) | Expected Value | Status |
|-----------|------------|-----------------|--------|
| **Frontend API URL** | TaskManagementWeb/appsettings.json | localhost:5000 | ✅ |
| **API Database** | TaskManagementApp/appsettings.json | localhost | ✅ |
| **API Auth Database** | TaskManagementApp/appsettings.json | localhost | ✅ |
| **SQL Server Service** | System Services | Running | ❓ Check |
| **API Service** | dotnet run | Listening on :5000 | ❓ Check |
| **Database Created** | SQL Server | TaskManagementDb exists | ❓ Check |
| **Identity DB Created** | SQL Server | NZWalksIdentityDb exists | ❓ Check |

---

## 🚀 Quick Fix Script

**Run this PowerShell script to check everything:**

```powershell
Write-Host "=== TASKFLOW REGISTRATION DIAGNOSTIC ===" -ForegroundColor Cyan

# Check 1: SQL Server Status
Write-Host "`n[1/5] Checking SQL Server..." -ForegroundColor Yellow
$sqlStatus = (Get-Service MSSQL$SQLEXPRESS).Status
Write-Host "SQL Server Status: $sqlStatus" -ForegroundColor $(if($sqlStatus -eq 'Running') {'Green'} else {'Red'})

# Check 2: Port 5000
Write-Host "`n[2/5] Checking if API port 5000 is listening..." -ForegroundColor Yellow
$port5000 = netstat -ano | findstr :5000
if ($port5000) {
    Write-Host "✅ Port 5000 is listening (API running)" -ForegroundColor Green
} else {
    Write-Host "❌ Port 5000 not listening (API NOT running)" -ForegroundColor Red
    Write-Host "   Run: cd TaskManagementApp && dotnet run" -ForegroundColor Yellow
}

# Check 3: Databases
Write-Host "`n[3/5] Checking databases..." -ForegroundColor Yellow
$databases = @()
try {
    $result = sqlcmd -S localhost -Q "SELECT name FROM sys.databases WHERE name LIKE '%Task%' OR name LIKE '%Walk%'" -h -1
    $databases = $result | Where-Object {$_ -match '\S'}
}
catch {
    Write-Host "❌ Cannot connect to SQL Server" -ForegroundColor Red
}

if ($databases.Count -ge 2) {
    Write-Host "✅ Both databases found:" -ForegroundColor Green
    $databases | ForEach-Object { Write-Host "   - $_" -ForegroundColor Green }
} else {
    Write-Host "⚠️  Missing databases. Run: dotnet ef database update" -ForegroundColor Yellow
}

# Check 4: Connection Strings
Write-Host "`n[4/5] Checking configuration files..." -ForegroundColor Yellow
$webConfig = Get-Content "TaskManagementWeb/appsettings.json" | ConvertFrom-Json
$apiConfig = Get-Content "TaskManagementApp/appsettings.json" | ConvertFrom-Json

Write-Host "Frontend API URL: $($webConfig.ApiSettings.BaseUrl)" -ForegroundColor Green
Write-Host "API Database: $($apiConfig.ConnectionStrings.TaskManagementConnString)" -ForegroundColor Green

# Check 5: Summary
Write-Host "`n[5/5] Summary:" -ForegroundColor Yellow
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan

if ($sqlStatus -eq 'Running' -and $port5000 -and $databases.Count -ge 2) {
    Write-Host "✅ ALL SYSTEMS GO! Registration should work!" -ForegroundColor Green
    Write-Host "Try registering at: http://localhost:5001/Account/Register" -ForegroundColor Green
} elseif ($sqlStatus -ne 'Running') {
    Write-Host "❌ SQL Server is not running" -ForegroundColor Red
    Write-Host "Start it: Start-Service MSSQL`$SQLEXPRESS" -ForegroundColor Yellow
} elseif (!$port5000) {
    Write-Host "❌ API is not running on port 5000" -ForegroundColor Red
    Write-Host "Start it: cd TaskManagementApp && dotnet run" -ForegroundColor Yellow
} else {
    Write-Host "⚠️  Databases not found" -ForegroundColor Yellow
    Write-Host "Create them: dotnet ef database update" -ForegroundColor Yellow
}

Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
```

**To run this script:**
```powershell
# Save as: check-registration.ps1
# Run:
.\check-registration.ps1
```

---

## 📋 Deployment Notes

### When Deployed to Azure
```
appsettings.Production.json is used
ConnectionStrings point to: Azure SQL Server
Environment: Production

Flow:
Frontend (Azure App Service) 
      ↓ (HTTPS)
API (Azure App Service)
      ↓ (SQL Connection String)
Azure SQL Server
```

### For Local Development (Now)
```
appsettings.json is used
ConnectionStrings point to: localhost
Environment: Development

Flow:
Frontend (localhost:5001)
      ↓ (HTTP)
API (localhost:5000)
      ↓ (Windows Auth)
Local SQL Server (localhost)
```

---

## ✅ Success Checklist

- [ ] SQL Server is running (`Get-Service MSSQL$SQLEXPRESS`)
- [ ] API is running (`dotnet run` in TaskManagementApp)
- [ ] Port 5000 is listening (`netstat -ano | findstr :5000`)
- [ ] Databases exist (check in SSMS or sqlcmd)
- [ ] Frontend API URL is `http://localhost:5000/api` ✅
- [ ] No CORS errors in browser console
- [ ] No 500 errors from API
- [ ] Registration form loads
- [ ] Can create account
- [ ] Can login with new account

---

## 🎯 Your Answer

**Q: Is connection string pointing to localhost or Azure?**

**A:** 
- ✅ **Development**: `localhost` (SQL Server) - You're using this now
- ✅ **Production**: `Azure SQL Server` - For when deployed

**Why Registration Fails:**
1. API not running (`dotnet run`)
2. SQL Server not running (`Start-Service MSSQL$SQLEXPRESS`)
3. Database not created (`dotnet ef database update`)
4. Frontend API URL misconfigured (currently correct ✅)

**Next Step:** Run the diagnostic script above to identify the exact issue!

