using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Data;
using HotelBooking.API.Models;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Controllers
{
    /// <summary>
    /// Controller for managing users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">Database context</param>
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns the list of users</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// Gets a specific user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="registerDto">User registration data</param>
        /// <returns>Created user</returns>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Invalid input data or email already exists</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email already exists
            var emailExists = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
            if (emailExists)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            // In production, use a proper password hashing library like BCrypt
            var user = new User
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                PasswordHash = HashPassword(registerDto.Password), // Simple hash for demo
                PhoneNumber = registerDto.PhoneNumber,
                Role = "Customer",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>User information</returns>
        /// <response code="200">Login successful</response>
        /// <response code="401">Invalid credentials</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hashedPassword = HashPassword(loginDto.Password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == hashedPassword);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Updates user profile
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="userDto">Updated user data</param>
        /// <returns>No content</returns>
        /// <response code="204">User updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">User not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            user.FullName = userDto.FullName;
            user.PhoneNumber = userDto.PhoneNumber;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Simple password hashing (for demo purposes only)
        /// In production, use BCrypt, Argon2, or similar
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Hashed password</returns>
        private string HashPassword(string password)
        {
            // This is a simple hash for demonstration purposes
            // In production, use a proper password hashing library like BCrypt.Net
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
