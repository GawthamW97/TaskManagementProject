# 🔐 Authentication - Login & Register Pages

## Overview

Complete authentication UI with modern Login and Register pages that integrate with your API.

---

## ✨ Features

### Login Page
✅ Email and password input  
✅ Remember me functionality  
✅ Forgot password link  
✅ Social login buttons (UI ready)  
✅ Password visibility toggle  
✅ Form validation  
✅ Error/success alerts  
✅ Loading states  

### Register Page
✅ First name & last name fields  
✅ Email with validation  
✅ Password with strength meter  
✅ Confirm password validation  
✅ Terms & conditions checkbox  
✅ Real-time password strength feedback  
✅ Form validation  
✅ Error/success alerts  
✅ Loading states  

### Features
✅ Modern gradient design  
✅ Fully responsive  
✅ Bootstrap 5 integration  
✅ JWT token storage  
✅ Auto-redirect after login  
✅ Persistent "Remember Me"  
✅ Token-based authentication  
✅ User info display in sidebar  
✅ Logout functionality  

---

## 📁 Files Created

### Controllers
```
Controllers/AccountController.cs
├── Login() - Display login page
├── Register() - Display register page
└── Logout() - Clear authentication
```

### Views
```
Views/Account/
├── Login.cshtml - Login form
└── Register.cshtml - Registration form
```

### Styles
```
wwwroot/css/auth.css - Authentication styles
```

### Scripts
```
wwwroot/js/
├── auth-login.js - Login page logic
└── auth-register.js - Register page logic
```

### Layout Update
```
Views/Shared/_Layout.cshtml
├── User menu in sidebar
├── Authentication check
└── Auto-redirect to login
```

---

## 🎯 How It Works

### Login Flow
```
1. User visits /Account/Login
2. Enters email and password
3. Form validates locally
4. Sends POST to /api/auth/login
5. Receives JWT token
6. Stores in localStorage
7. Redirects to dashboard
8. Token sent with all API requests
```

### Register Flow
```
1. User visits /Account/Register
2. Fills out registration form
3. Validates password strength
4. Confirms password match
5. Sends POST to /api/auth/register
6. Account created on API
7. Redirects to login
8. User can now login
```

### Protected Pages
```
1. All pages check for authToken in localStorage
2. If no token, redirect to login
3. Token included in API headers
4. Auto-logout on 401 response
```

---

## 🚀 Usage

### Accessing Login
```
URL: http://localhost:5001/Account/Login
```

### Accessing Register
```
URL: http://localhost:5001/Account/Register
```

### Logout
```
URL: http://localhost:5001/Account/Logout
Click "Logout" in sidebar user menu
```

---

## 🔧 Configuration

### API Base URL
Edit `wwwroot/js/auth-login.js` and `wwwroot/js/auth-register.js`:
```javascript
const API_BASE_URL = 'http://localhost:5000/api';
```

### Expected API Endpoints
```
POST /api/auth/login
POST /api/auth/register
```

### Request Format
```javascript
// Login
{
  "email": "user@example.com",
  "password": "password123"
}

// Register
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "user@example.com",
  "password": "password123"
}
```

### Response Format
```javascript
// Success
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "message": "Login successful"
}

// Error
{
  "message": "Invalid credentials"
}
```

---

## 🎨 Design

### Colors
- **Background Gradient**: Purple (#667eea → #764ba2)
- **Primary**: Purple (#667eea)
- **Success**: Green (#28a745)
- **Danger**: Red (#dc3545)
- **Background**: White & Light Gray

### Components
- Card-based layout
- Gradient buttons
- Input fields with focus states
- Password strength meter
- Alert notifications
- Loading spinners

### Responsive
- Full-width on mobile
- Optimized for tablet
- Desktop with sidebar
- Touch-friendly buttons

---

## 🔐 Security Features

### Frontend
- ✅ Form validation
- ✅ Password strength checks
- ✅ XSS protection
- ✅ HTTPS ready
- ✅ Token in localStorage
- ✅ Auto-logout on 401

### Best Practices
```javascript
// Token storage
localStorage.setItem('authToken', token);

// Token usage
headers: {
    'Authorization': 'Bearer ' + token
}

// Token removal
localStorage.removeItem('authToken');
```

---

## 📝 Form Validation

### Login
- ✅ Email required
- ✅ Valid email format
- ✅ Password required (min 6 chars)

### Register
- ✅ First name required (min 2 chars)
- ✅ Last name required (min 2 chars)
- ✅ Valid email format
- ✅ Password min 8 characters
- ✅ Password strength check
- ✅ Confirm password match
- ✅ Terms agreement required

### Password Strength
```
0 chars:    'Password must be at least 8 characters'
1-2 items:  'Weak password'
3 items:    'Fair password - Add more variety'
4 items:    'Strong password'

Items:
- 8+ characters
- Upper & lowercase
- Numbers
- Special characters
```

---

## 🧪 Testing

### Login Test
```
1. Go to http://localhost:5001/Account/Login
2. Enter valid email and password
3. Click "Sign In"
4. Should redirect to dashboard
5. User menu should show in sidebar
```

### Register Test
```
1. Go to http://localhost:5001/Account/Register
2. Fill out all fields
3. Check password strength meter
4. Click "Create Account"
5. Should redirect to login
6. Login with new credentials
```

### Remember Me Test
```
1. Check "Remember me"
2. Login
3. Close browser
4. Return to login page
5. Email should be pre-filled
```

### Logout Test
```
1. Login first
2. Look in sidebar for user menu
3. Click "Logout"
4. Should redirect to login
5. Token should be cleared
```

---

## 🔄 Integration with API

### Expected Endpoints

The login/register pages expect these endpoints on your API:

```csharp
// POST /api/auth/login
public async Task<IActionResult> Login(LoginRequest request)
{
    // Validate credentials
    // Generate JWT token
    // Return { token, message }
}

// POST /api/auth/register
public async Task<IActionResult> Register(RegisterRequest request)
{
    // Validate input
    // Create user
    // Return success message
}
```

---

## 🛡️ JWT Token Handling

### Storing Token
```javascript
const response = await fetch('/api/auth/login', {...});
const data = await response.json();
localStorage.setItem('authToken', data.token);
```

### Using Token
```javascript
const token = localStorage.getItem('authToken');
const headers = {
    'Authorization': 'Bearer ' + token
};
```

### Decoding Token (Optional)
```javascript
const token = localStorage.getItem('authToken');
const base64Url = token.split('.')[1];
const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
const jsonPayload = decodeURIComponent(atob(base64).split('').map(c =>
    '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
).join(''));
const data = JSON.parse(jsonPayload);
```

---

## 📱 Mobile Experience

### Optimized For
- ✅ Small screens (320px+)
- ✅ Touch input
- ✅ Portrait mode
- ✅ Slow connections
- ✅ Low bandwidth

### Features
- Full-width cards
- Larger buttons
- Clear error messages
- Auto-focus first field
- Enter key submission

---

## 🎓 Customization

### Change Login Form Title
Edit `Views/Account/Login.cshtml`:
```html
<h1><i class="bi bi-clipboard-check"></i> Your Title</h1>
```

### Change Colors
Edit `wwwroot/css/auth.css`:
```css
.auth-header {
    background: linear-gradient(135deg, #your-color1 0%, #your-color2 100%);
}
```

### Add More Fields
Edit form and add validation:
```html
<input type="text" id="phone" class="form-control" />
```

---

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| Login fails | Check API is running on port 5000 |
| Token not stored | Check browser localStorage |
| Redirect not working | Verify token in response |
| Form not submitting | Check console for errors |
| Styles not loading | Hard refresh browser |
| Remember me not working | Check localStorage settings |

---

## 📊 Files Summary

| File | Lines | Purpose |
|------|-------|---------|
| AccountController.cs | 20 | Handle auth routes |
| Login.cshtml | 80 | Login form |
| Register.cshtml | 110 | Registration form |
| auth.css | 350 | Styling |
| auth-login.js | 110 | Login logic |
| auth-register.js | 130 | Register logic |

---

## ✅ Checklist

- [x] Login page created
- [x] Register page created
- [x] Modern design
- [x] Form validation
- [x] API integration
- [x] JWT token handling
- [x] Auto-redirect
- [x] Remember me
- [x] User menu
- [x] Logout
- [x] Responsive design
- [x] Error handling
- [x] Documentation

---

## 🚀 Next Steps

1. **Implement API Endpoints**
   - Create /api/auth/login
   - Create /api/auth/register
   - Return JWT tokens

2. **Test Authentication**
   - Test login flow
   - Test register flow
   - Test token storage
   - Test protected pages

3. **Customize**
   - Update colors
   - Add logo
   - Adjust fields
   - Update messages

4. **Deploy**
   - Update API URL
   - Configure CORS
   - Enable HTTPS
   - Monitor logs

---

**Status**: ✅ Complete  
**Version**: 1.0  
**Last Updated**: 2024

