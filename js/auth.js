/**
 * Authentication module for the Login and Dashboard application.
 * Uses sessionStorage for session management.
 */

var Auth = (function () {
  var VALID_USERS = [
    { username: "admin", password: "admin123", name: "Admin User" },
    { username: "user", password: "user123", name: "Regular User" },
  ];

  var SESSION_KEY = "loggedInUser";

  /**
   * Validates that the username is non-empty and at least 3 characters.
   * Returns an error message string, or empty string if valid.
   */
  function validateUsername(username) {
    if (!username || username.trim() === "") {
      return "Username is required";
    }
    if (username.trim().length < 3) {
      return "Username must be at least 3 characters";
    }
    return "";
  }

  /**
   * Validates that the password is non-empty and at least 6 characters.
   * Returns an error message string, or empty string if valid.
   */
  function validatePassword(password) {
    if (!password || password === "") {
      return "Password is required";
    }
    if (password.length < 6) {
      return "Password must be at least 6 characters";
    }
    return "";
  }

  /**
   * Attempts to log in with the given credentials.
   * Returns { success: true, user: { username, name } } on success,
   * or { success: false, message: "..." } on failure.
   */
  function login(username, password) {
    var usernameError = validateUsername(username);
    if (usernameError) {
      return { success: false, message: usernameError };
    }

    var passwordError = validatePassword(password);
    if (passwordError) {
      return { success: false, message: passwordError };
    }

    var trimmedUsername = username.trim().toLowerCase();
    var found = null;
    for (var i = 0; i < VALID_USERS.length; i++) {
      if (VALID_USERS[i].username === trimmedUsername && VALID_USERS[i].password === password) {
        found = VALID_USERS[i];
        break;
      }
    }

    if (!found) {
      return { success: false, message: "Invalid username or password" };
    }

    var userData = { username: found.username, name: found.name };
    sessionStorage.setItem(SESSION_KEY, JSON.stringify(userData));
    return { success: true, user: userData };
  }

  /**
   * Logs out the current user by clearing the session.
   */
  function logout() {
    sessionStorage.removeItem(SESSION_KEY);
  }

  /**
   * Returns the currently logged-in user object, or null if not logged in.
   */
  function getCurrentUser() {
    var data = sessionStorage.getItem(SESSION_KEY);
    if (!data) {
      return null;
    }
    try {
      return JSON.parse(data);
    } catch (e) {
      return null;
    }
  }

  /**
   * Returns true if a user is currently logged in.
   */
  function isLoggedIn() {
    return getCurrentUser() !== null;
  }

  return {
    validateUsername: validateUsername,
    validatePassword: validatePassword,
    login: login,
    logout: logout,
    getCurrentUser: getCurrentUser,
    isLoggedIn: isLoggedIn,
  };
})();

if (typeof module !== "undefined" && module.exports) {
  module.exports = Auth;
}
