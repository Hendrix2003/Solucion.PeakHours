using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolucionPeakHours.Entities;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Shared.UserAccount;

namespace SolucionPeakHours.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IAccountService _authService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserIdentityEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserRepositoy _userRepositoy;

        public UserAccountController(IAccountService authService,
                                     ITokenService tokenService,
                                     UserManager<UserIdentityEntity> userManager,
                                     IUserRepositoy userRepositoy,
                                     IMapper mapper)
        {
            _userRepositoy = userRepositoy;
            _authService = authService;
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet("getRoles")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<string>>> GetRoles()
        {
            var roles = await _authService.GetRoles();

            return Ok(roles);
        }

        [HttpGet("getUserById")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<UserDTO>> GetUserById(string userId)
        {
            var user = await _authService.GetUserById(userId);

            var userDTO = _mapper.Map<UserDTO>(user);

            return Ok(userDTO);
        }

        [HttpGet("checkEmergencyTimePermission")]
        [Authorize(Roles = "Administrator, GerenteArea")]
        public async Task<ActionResult<bool>> CheckEmergencyTimePermission(string userId)
        {
            var result = await _userRepositoy.CheckEmergencyTimePermissionAsync(userId);

            if (!result)
            {
                return BadRequest("Error al actualizar permiso de tiempo de emergencia");
            }

            return Ok(result);
        }

        [HttpGet("getCurrentUserState")]
        [Authorize]
        public async Task<UserStateAuthResponse> GetCurrentUserState()
        {
            var userIdLogged = User
                .Claims
                .FirstOrDefault(x => x.Type == "Id")!
                .Value;

            var user = await _authService.GetUserById(userIdLogged);

            var userRole = await _userManager.GetRolesAsync(user);

            return new UserStateAuthResponse
            {
                IsAuthenticated = User.Identity!.IsAuthenticated,
                FullName = user.FactoryStaffEntity!.FullName!,
                Id = userIdLogged,
                Area = user.FactoryStaffEntity!.Area!,
                Role = userRole.FirstOrDefault() ?? ""
            };
        }

        [HttpGet("getUsers")]
        [Authorize(Roles = "Administrator, GerenteArea, Supervisor")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var users = await _authService.GetUsers();

            var usersDTO = _mapper.Map<List<UserDTO>>(users);

            return Ok(usersDTO);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDTO login)
        {
            var authResponse = await _authService.Login(login.UserName!, login.Pwd!);

            if (authResponse == null)
            {
                return BadRequest("Usuario o contraseña incorrectos");
            }

            var token = await _tokenService.CreateToken(authResponse);

            var authResponseDTO = _mapper.Map<AuthResponse>(authResponse);

            authResponseDTO.Token = token;

            return Ok(authResponseDTO);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Administrator")]

        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDTO register)
        {
            var user = _mapper.Map<UserIdentityEntity>(register);

            user.UserName = register.Email;

            var registerResponse = await _authService.Register(user, register.Pwd!);

            if (registerResponse == null)
            {
                return BadRequest("Error al registrar usuario");
            }

            if(!string.IsNullOrEmpty(register.Role))
            {
                var result = await _authService.AddRoleToUser(registerResponse.Id, register.Role);

                if (!result)
                {
                    return BadRequest("Error al agregar rol al usuario");
                }
            }

            var authResponseDTO = _mapper.Map<AuthResponse>(registerResponse);

            return Ok(authResponseDTO);
        }

        [HttpPost("updateUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<AuthResponse>> UpdateUser([FromBody] RegisterDTO updateUser)
        {
            var user = _mapper.Map<UserIdentityEntity>(updateUser);

            user.UserName = updateUser.Email;

            var result = await _authService.UpdateUser(user, updateUser.Pwd!);

            if (result == null)
            {
                return BadRequest("Error al actualizar usuario");
            }

            if (!string.IsNullOrEmpty(updateUser.Role))
            {
                var currentRole = await _userManager.GetRolesAsync(result);

                if (currentRole.FirstOrDefault() != updateUser.Role)
                {
                    await _userManager.RemoveFromRoleAsync(result, currentRole.FirstOrDefault()!);

                    var resultAddRole = await _authService.AddRoleToUser(result.Id, updateUser.Role);

                    if (!resultAddRole)
                    {
                        return BadRequest("Error al agregar rol al usuario");
                    }
                }
            }

            var authResponseDTO = _mapper.Map<AuthResponse>(result);

            return Ok(authResponseDTO);
        }

        [HttpDelete("deleteUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<bool>> DeleteUser(string userId)
        {
            var result = await _authService.DeleteUser(userId);

            if (!result)
            {
                return BadRequest("Error al eliminar usuario");
            }

            return Ok(result);
        }

        [HttpPost("addRoleToUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<bool>> AddRoleToUser([FromBody] AddRoleToUserDTO addRoleToUser)
        {
            var result = await _authService.AddRoleToUser(addRoleToUser.UserId!, addRoleToUser.Role!);

            if (!result)
            {
                return BadRequest("Error al agregar rol al usuario");
            }

            return Ok(result);
        }

        [HttpPost("createRole")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<bool>> CreateRole([FromBody] string roleName)
        {
            var result = await _authService.CreateRole(roleName);

            if (!result)
            {
                return BadRequest("Error al crear rol");
            }

            return Ok(result);
        }
    }
}
