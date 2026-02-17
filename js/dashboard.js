/**
 * Dashboard page controller.
 * Displays user info and handles logout.
 */
document.addEventListener("DOMContentLoaded", function () {
  if (!Auth.isLoggedIn()) {
    window.location.href = "index.html";
    return;
  }

  var user = Auth.getCurrentUser();
  var welcomeUser = document.getElementById("welcomeUser");
  var headerUser = document.getElementById("headerUser");
  var logoutBtn = document.getElementById("logoutBtn");

  welcomeUser.textContent = user.name;
  headerUser.textContent = user.name;

  logoutBtn.addEventListener("click", function () {
    Auth.logout();
    window.location.href = "index.html";
  });
});
