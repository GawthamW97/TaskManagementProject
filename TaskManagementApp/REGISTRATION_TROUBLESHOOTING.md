# 🔍 REGISTRATION TROUBLESHOOTING GUIDE

## Current Configuration Status

### ✅ What's Correctly Configured

**TaskManagementWeb (Frontend):**
- API Base URL: `http://localhost:5000/api` ✓
- Correctly pointing to localhost

**TaskManagementApp (API):**
- Development DB: `localhost` (SQL Server) ✓
- Production DB: Azure SQL Server ✓
- Proper environment-based configuration ✓

---

## 🚨 Why Registration Might Be Failing

### Checklist - Run Through These in Order

#### 1. **Is the API Running?**
```powershell
# Check if API is running on port 5000/5001
netstat -ano | findstr :5000
netstat -ano | findstr :5001
```

**Expected Output:** Should show TaskManagementApp.exe listening

**If Not Running:**
```powershell
# Navigate to API project
cd C:\Users\Asus\Desktop\Study\ASP.NET CORE\TaskManagementApp\TaskManagementApp

# Run the API
dotnet run

# Or in Visual Studio: Press F5 to start debugging
```

---

#### 2. **Is SQL Server Running?**

**Check SQL Server Status:**
```powershell
# Check if SQL Server services are running
Get-Service *SQL* | Select Name, Status

# Look for:
# MSSQL$SQLEXPRESS (if using SQL Express)
# Or your named instance
```

**Expected Output:**
```
Status Name
------ ----
Running MSSQL$SQLEXPRESS
```

**If Not Running:**
```powershell
# Start SQL Server
Start-Service MSSQL$SQLEXPRESS

# Or open SQL Server Configuration Manager
# Computer Management > Services and Applications > SQL Server
```

---

#### 3. **Is the Database Created?**

**Option A: Using SQL Server Management Studio (SSMS)**
1. Open SSMS
2. Connect to: `localhost` (or `.\SQLEXPRESS`)
3. Look for databases:
   - `TaskManagementDb`
   - `NZWalksIdentityDb`

**Option B: Using Command Line**
```powershell
# Run migrations
cd TaskManagementApp
dotnet ef database update --context TaskManagementDbContext

# This will create/update the database
```

---

#### 4. **Check the Registration Endpoint**

**Test the API endpoint directly:**

**Using PowerShell:**
```powershell
$body = @{
    email = "test@example.com"
    password = "TestPassword123!"
    roles = @("user")
} | ConvertTo-Json

Invoke-WebRequest -Uri "http://localhost:5000/api/auth/register" `
    -Method POST `
    -ContentType "application/json" `
    -Body $body
```

**Using curl (if installed):**
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"TestPassword123!","roles":["user"]}'
```

---

#### 5. **Check Browser Console for Errors**

1. Open browser (where TaskFlow is running)
2. Press `F12` to open DevTools
3. Go to **Console** tab
4. Try to register
5. Look for error messages

**Common Errors & Solutions:**

| Error | Cause | Solution |
|-------|-------|----------|
| `Failed to fetch` | API not running | Start API with `dotnet run` |
| `CORS error` | CORS not configured | Add CORS to Program.cs |
| `404 Not Found` | Wrong endpoint | Check API route is `/api/auth/register` |
| `500 Internal Server Error` | Database issue | Check SQL Server is running |
| `Invalid credentials` | Database error | Run migrations |

---

#### 6. **Check API Logs**

**If running in Terminal:**
```
Look for error messages starting with [ERR] or red text
```

**If running in Visual Studio:**
1. Go to **Debug > Windows > Output**
2. Look for exception messages
3. Check the detailed stack trace

---

## 🔧 Step-by-Step Fix Guide

### **Scenario 1: API Not Running**

```powershell
# Terminal 1: Start the API
cd "C:\Users\Asus\Desktop\Study\ASP.NET CORE\TaskManagementApp\TaskManagementApp"
dotnet run

# Should output:
# ...
# Now listening on: http://localhost:5000
# Now listening on: https://localhost:5001
```

### **Scenario 2: Database Not Created**

```powershell
# Terminal: Run migrations
cd "C:\Users\Asus\Desktop\Study\ASP.NET CORE\TaskManagementApp\TaskManagementApp"

# Create initial migration (if needed)
dotnet ef migrations add InitialCreate --context TaskManagementDbContext

# Apply migrations
dotnet ef database update --context TaskManagementDbContext

# Output should show:
# Build started...
# Build succeeded.
# Applying migration...
# Done.
```

### **Scenario 3: CORS Issues**

**Check Program.cs for CORS configuration:**

```csharp
// Should have something like this:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// And in the pipeline:
app.UseCors("AllowAll");
```

If missing, add it to Program.cs (before `app.MapControllers()`).

---

## 📊 Complete Verification Checklist

Use this checklist to verify everything:

- [ ] **SQL Server is running**
  ```powershell
  Get-Service MSSQL$SQLEXPRESS | Select Status
  ```

- [ ] **Database exists**
  ```powershell
  # In SSMS, check Object Explorer
  # Look for TaskManagementDb
  ```

- [ ] **Migrations are applied**
  ```powershell
  cd TaskManagementApp
  dotnet ef database update
  ```

- [ ] **API is running**
  ```powershell
  # In another terminal:
  netstat -ano | findstr :5000
  # Should show: LISTENING
  ```

- [ ] **API responds to ping**
  ```powershell
  Invoke-WebRequest -Uri "http://localhost:5000/api" -Method GET
  # Should return 404 or success, not connection refused
  ```

- [ ] **Frontend can reach API**
  ```
  Open browser DevTools (F12)
  Check Network tab when trying to register
  Should see POST to http://localhost:5000/api/auth/register
  ```

- [ ] **No CORS errors**
  ```
  Console tab should have no CORS errors
  If it does, CORS needs to be added to API
  ```

---

## 🎯 Quick Start - Everything at Once

**Terminal 1 - Start API:**
```powershell
cd "C:\Users\Asus\Desktop\Study\ASP.NET CORE\TaskManagementApp\TaskManagementApp"
dotnet run
```

**Terminal 2 - Check/Create Database:**
```powershell
cd "C:\Users\Asus\Desktop\Study\ASP.NET CORE\TaskManagementApp\TaskManagementApp"
dotnet ef database update --context TaskManagementDbContext
```

**Then in Visual Studio:**
1. Open TaskManagementWeb project
2. Press `F5` to run
3. Click "Create one now" from login
4. Try to register

---

## 📍 Connection String Summary

### **Development (What You're Using)**
```
Server=localhost;
Database=TaskManagementDb;
Trusted_Connection=True;
TrustServerCertificate=True;
```
- ✅ Points to **localhost** SQL Server
- ✅ Uses **Windows Authentication**
- ✅ For local development only

### **Production (Azure)**
```
Server=tcp:sqlserver-taskmanagement-francent-dev-01.database.windows.net;
Initial Catalog=sqdb-taskmanagement-francent-dev-01;
Encrypt=True;
TrustServerCertificate=False;
Connection Timeout=30;
```
- Points to **Azure SQL Server**
- Uses **SQL Authentication** (username/password)
- For production deployment

---

## ✅ Expected Success Indicators

When everything is working:

1. ✅ **API Terminal shows:**
   ```
   Now listening on: http://localhost:5000
   Now listening on: https://localhost:5001
   ```

2. ✅ **Browser loads login page**
   - No errors in console
   - Can see the login form

3. ✅ **Registration form loads**
   - All fields visible
   - "Create Account" button enabled

4. ✅ **Registration succeeds**
   - Submit form
   - See "Account created successfully" message
   - Redirects to login page

5. ✅ **Can login**
   - Enter registered credentials
   - See "Login successful" message
   - Redirects to dashboard
   - User menu shows in sidebar

---

## 🆘 If Still Not Working

**Collect Debug Information:**

```powershell
# 1. Get API startup logs
# (Copy everything from terminal when dotnet run starts)

# 2. Get database info
sqlcmd -S localhost -Q "SELECT name FROM sys.databases WHERE name LIKE '%Task%'"

# 3. Get service status
Get-Service | Where-Object {$_.Name -like '*SQL*' -or $_.Name -like '*SSMS*'}

# 4. Check port availability
netstat -ano | findstr :5000
netstat -ano | findstr :5001
```

**Share the output of above and I can help diagnose!**

---

## 🎓 Summary

**Your Setup:**
- ✅ Frontend correctly configured for **localhost**
- ✅ API has proper **environment-based configuration**
- ✅ Development uses **local SQL Server**
- ✅ Production uses **Azure SQL Server**

**Most Common Issues:**
1. API not running
2. SQL Server not running
3. Database not migrated
4. CORS not configured

**Next Step:** Follow the **Quick Start** section above and check each item in the checklist.

---

**Need More Help?**
1. Run the **Verification Checklist** above
2. Collect the **Debug Information**
3. Check the **Common Errors** table
4. Share your findings!

