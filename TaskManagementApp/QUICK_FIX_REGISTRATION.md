# ⚡ QUICK FIX - REGISTRATION NOT WORKING

## 📍 Your Current Setup

✅ **Frontend**: `localhost:5001` → API: `localhost:5000` ✓  
❓ **API**: Not sure if running  
❓ **Database**: Not sure if exists  

---

## 🚀 THE FIX (3 Steps)

### **Step 1: Start SQL Server** (5 seconds)
```powershell
Start-Service MSSQL$SQLEXPRESS
```

### **Step 2: Create Database** (10 seconds)
```powershell
cd "C:\Users\Asus\Desktop\Study\ASP.NET CORE\TaskManagementApp\TaskManagementApp"
dotnet ef database update
```

### **Step 3: Start API** (Ongoing)
```powershell
cd "C:\Users\Asus\Desktop\Study\ASP.NET CORE\TaskManagementApp\TaskManagementApp"
dotnet run
```

**Wait for:**
```
Now listening on: http://localhost:5000
Now listening on: https://localhost:5001
```

---

## ✅ Then Test

1. Open browser: `http://localhost:5001`
2. Click "Create one now"
3. Fill registration form
4. Click "Create Account"
5. ✅ Should work!

---

## 🆘 If Still Broken

**Check which step failed:**

```powershell
# 1. Is SQL Server running?
Get-Service MSSQL$SQLEXPRESS

# 2. Is database created?
sqlcmd -S localhost -Q "SELECT name FROM sys.databases"

# 3. Is API listening?
netstat -ano | findstr :5000

# 4. Can you reach API?
Invoke-WebRequest http://localhost:5000
```

**Share the output and I'll help fix!**

---

## 🎯 Quick Answers

| Question | Answer |
|----------|--------|
| Is connection string localhost? | ✅ YES (for development) |
| Is connection string Azure? | ✅ YES (for production) |
| Why can't I register? | API/Database not running |
| What do I do? | Follow 3 steps above |

