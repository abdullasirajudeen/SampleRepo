# Login and Dashboard Web Application

A simple web application with a login page and dashboard, built with HTML, CSS, and JavaScript.

## Features

- **Login Page** – Form with username and password validation
- **Dashboard Page** – Displays a welcome message and summary statistics
- **Authentication** – Client-side session management using `sessionStorage`
- **Route Protection** – Dashboard redirects to login if not authenticated; login redirects to dashboard if already authenticated

## Demo Credentials

| Username | Password   |
|----------|------------|
| admin    | admin123   |
| user     | user123    |

## Project Structure

```
├── index.html          # Login page
├── dashboard.html      # Dashboard page
├── css/
│   └── style.css       # Shared styles
├── js/
│   ├── auth.js         # Authentication module
│   ├── login.js        # Login page controller
│   └── dashboard.js    # Dashboard page controller
├── tests/
│   └── auth.test.html  # Auth module tests (open in browser)
└── ReadMe.md
```

## Getting Started

1. Open `index.html` in a web browser
2. Log in with the demo credentials above
3. You will be redirected to the dashboard

## Running Tests

Open `tests/auth.test.html` in a web browser to run the authentication module tests.
