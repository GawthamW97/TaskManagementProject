const API_BASE_URL = 'https://localhost:7198/api';

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

document.getElementById('password').addEventListener('input', function() {
    const password = this.value;
    const strengthFill = document.getElementById('strengthFill');
    const strengthText = document.getElementById('strengthText');
    
    let strength = 0;
    
    if (password.length >= 8) strength++;
    if (password.match(/[a-z]/) && password.match(/[A-Z]/)) strength++;
    if (password.match(/[0-9]/)) strength++;
    if (password.match(/[^a-zA-Z0-9]/)) strength++;
    
    strengthFill.className = 'strength-fill';
    
    if (strength === 0) {
        strengthText.textContent = 'Password must be at least 8 characters';
    } else if (strength <= 2) {
        strengthFill.classList.add('weak');
        strengthText.textContent = 'Weak password';
    } else if (strength === 3) {
        strengthFill.classList.add('fair');
        strengthText.textContent = 'Fair password - Add more variety';
    } else {
        strengthFill.classList.add('strong');
        strengthText.textContent = 'Strong password';
    }
});

function validateForm() {
    const firstName = document.getElementById('firstName').value.trim();
    const lastName = document.getElementById('lastName').value.trim();
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    const agreeTerms = document.getElementById('agreeTerms').checked;

    if (!firstName || !lastName || !email || !password || !confirmPassword) {
        showAlert('Please fill in all fields', 'danger');
        return false;
    }

    if (firstName.length < 2) {
        showAlert('First name must be at least 2 characters', 'danger');
        return false;
    }

    if (lastName.length < 2) {
        showAlert('Last name must be at least 2 characters', 'danger');
        return false;
    }

    if (email.indexOf('@') === -1 || email.indexOf('.') === -1) {
        showAlert('Please enter a valid email address', 'danger');
        return false;
    }

    if (password.length < 8) {
        showAlert('Password must be at least 8 characters', 'danger');
        return false;
    }

    if (password !== confirmPassword) {
        showAlert('Passwords do not match', 'danger');
        return false;
    }

    if (!agreeTerms) {
        showAlert('You must agree to the terms and conditions', 'danger');
        return false;
    }

    return true;
}

document.getElementById('registerForm').addEventListener('submit', async function(e) {
    e.preventDefault();

    if (!validateForm()) return;

    const firstName = document.getElementById('firstName').value.trim();
    const lastName = document.getElementById('lastName').value.trim();
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value;

    const btn = this.querySelector('.btn-register');
    btn.classList.add('loading');
    btn.disabled = true;

    try {
        const response = await fetch(API_BASE_URL + '/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: email,
                password: password,
                roles: ['user']
            })
        });

        const data = await response.json();

        if (response.ok) {
            showAlert('Account created successfully! Redirecting to login...', 'success');
            
            setTimeout(function() {
                window.location.href = '/Account/Login';
            }, 1500);
        } else {
            showAlert(data.message || 'Registration failed. Please try again.', 'danger');
        }
    } catch (error) {
        console.error('Error:', error);
        showAlert('An error occurred. Please try again.', 'danger');
    } finally {
        btn.classList.remove('loading');
        btn.disabled = false;
    }
});

document.getElementById('confirmPassword').addEventListener('input', function() {
    const password = document.getElementById('password').value;
    const confirmPassword = this.value;
    const errorDiv = document.getElementById('passwordMatchError');

    if (confirmPassword && password !== confirmPassword) {
        this.classList.add('is-invalid');
        errorDiv.textContent = 'Passwords do not match';
    } else {
        this.classList.remove('is-invalid');
        errorDiv.textContent = '';
    }
});
