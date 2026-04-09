const API_BASE_URL = 'https://localhost:7198/api';

function togglePassword() {
    const passwordInput = document.getElementById('password');
    const toggleIcon = document.getElementById('toggleIcon');
    
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        toggleIcon.classList.remove('bi-eye');
        toggleIcon.classList.add('bi-eye-slash');
    } else {
        passwordInput.type = 'password';
        toggleIcon.classList.remove('bi-eye-slash');
        toggleIcon.classList.add('bi-eye');
    }
}

function showAlert(message, type) {
    const alertContainer = document.getElementById('alertContainer');
    const icon = type === 'danger' ? 'exclamation-circle' : 'check-circle';
    const alertHTML = '<div class="alert alert-' + type + ' alert-dismissible fade show" role="alert">'
        + '<i class="bi bi-' + icon + '"></i> ' + message
        + '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>'
        + '</div>';
    alertContainer.innerHTML = alertHTML;
    
    setTimeout(function() {
        const alert = alertContainer.querySelector('.alert');
        if (alert) {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }
    }, 5000);
}

document.getElementById('loginForm').addEventListener('submit', async function(e) {
    e.preventDefault();

    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value;
    const rememberMe = document.getElementById('rememberMe').checked;

    if (!email || !password) {
        showAlert('Please fill in all fields', 'danger');
        return;
    }

    if (email.indexOf('@') === -1) {
        showAlert('Please enter a valid email address', 'danger');
        return;
    }

    if (password.length < 6) {
        showAlert('Password must be at least 6 characters', 'danger');
        return;
    }

    const btn = this.querySelector('.btn-login');
    btn.classList.add('loading');
    btn.disabled = true;

    try {
        const response = await fetch(API_BASE_URL + '/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: email,
                password: password
            })
        });

        const data = await response.json();

        if (response.ok && data.token) {
            localStorage.setItem('authToken', data.token);
            
            if (rememberMe) {
                localStorage.setItem('rememberMe', 'true');
                localStorage.setItem('userEmail', email);
            }

            showAlert('Login successful! Redirecting...', 'success');
            
            setTimeout(function() {
                window.location.href = '/';
            }, 1500);
        } else {
            showAlert(data.message || 'Login failed. Please check your credentials.', 'danger');
        }
    } catch (error) {
        console.error('Error:', error);
        showAlert('An error occurred. Please try again.', 'danger');
    } finally {
        btn.classList.remove('loading');
        btn.disabled = false;
    }
});

window.addEventListener('DOMContentLoaded', function() {
    if (localStorage.getItem('rememberMe') === 'true') {
        const savedEmail = localStorage.getItem('userEmail');
        if (savedEmail) {
            document.getElementById('email').value = savedEmail;
            document.getElementById('rememberMe').checked = true;
        }
    }
});

document.getElementById('password').addEventListener('keypress', function(e) {
    if (e.key === 'Enter') {
        document.getElementById('loginForm').dispatchEvent(new Event('submit'));
    }
});
