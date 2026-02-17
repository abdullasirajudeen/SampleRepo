/**
 * Login page controller.
 * Handles form submission and validation display.
 */
document.addEventListener("DOMContentLoaded", function () {
  if (Auth.isLoggedIn()) {
    window.location.href = "dashboard.html";
    return;
  }

  var form = document.getElementById("loginForm");
  var usernameInput = document.getElementById("username");
  var passwordInput = document.getElementById("password");
  var usernameError = document.getElementById("usernameError");
  var passwordError = document.getElementById("passwordError");
  var loginError = document.getElementById("loginError");

  form.addEventListener("submit", function (e) {
    e.preventDefault();

    usernameError.textContent = "";
    passwordError.textContent = "";
    loginError.textContent = "";
    usernameInput.classList.remove("input-error");
    passwordInput.classList.remove("input-error");

    var username = usernameInput.value;
    var password = passwordInput.value;

    var uError = Auth.validateUsername(username);
    var pError = Auth.validatePassword(password);
    var hasError = false;

    if (uError) {
      usernameError.textContent = uError;
      usernameInput.classList.add("input-error");
      hasError = true;
    }

    if (pError) {
      passwordError.textContent = pError;
      passwordInput.classList.add("input-error");
      hasError = true;
    }

    if (hasError) {
      return;
    }

    var result = Auth.login(username, password);

    if (result.success) {
      window.location.href = "dashboard.html";
    } else {
      loginError.textContent = result.message;
    }
  });
});
