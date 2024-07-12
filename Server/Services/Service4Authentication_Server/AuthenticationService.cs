using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Electronics_Store.Server.Services.Service4Authentication_Server;

public class AuthenticationService: IAuthenticationService
{
    private readonly ElectronicsStoreDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ElectronicsStoreDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        _dbContext = context;
        _httpContextAccessor = httpContextAccessor;
        _configuration = config;
    }

    public string GetUserEmail()
        => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)??"HttpContext=Null@gmail.com";

    public async Task<User?> GetUserByEmail(string email)
        => await _dbContext.Users.FirstOrDefaultAsync(user => user.userEmail.Equals(email));
    
    public int GetUserId()
        => int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)??"0123456789");
    public async Task<ServiceResponder<string>> SignIn(string enteredEmail, string enteredPassword)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.userEmail.ToUpper().Equals(enteredEmail.ToUpper()));
        return (user == null) ?
                    new ServiceResponder<string> {IsSuccess = false, Message = "There's no account match email you entered"}
                :   
                    ( !VerifyPassword(enteredPassword, user.userPasswordHashed, user.userPasswordSalt) ?
                            new ServiceResponder<string>(){IsSuccess = false, Message = "Wrong Password"}
                        :
                            new ServiceResponder<string>(){Data = GenerateToken(user)}
                    )
        ;
    }
    
    public async Task<ServiceResponder<int>> SignUp(User user, string password)
    {
        if (!(await IsExistingUser(user.userEmail)))
        {
                HashPassword(password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.userPasswordHashed = passwordHash;
                    user.userPasswordSalt = passwordSalt;
                    _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            return new ServiceResponder<int> { Data = user.userId, Message = "Successfully Registered!\nYou'll be redirect to login in 5 seconds\nIf Not, You Can Log In Manually"};
        } else
        {
            return new ServiceResponder<int>
            {
                IsSuccess = false,
                Message = "Existing User! Whether You Want to Login or Create a New Account?"
            };
        }
    }

    public async Task<bool> IsExistingUser(string email) => await _dbContext.Users.AnyAsync(user => user.userEmail.ToUpper().Equals(email.ToUpper()));
    
    

    
    
    private void HashPassword(string password, out byte[] hashedPassword, out byte[] passwordSalt)
    {
        HMACSHA512 hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        hashedPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        // using (HMACSHA512? hmac = new HMACSHA512())
        // {
        //     passwordSalt = hmac.Key;
        //     passwordHash = hmac
        //         .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        // }
    }
    private bool VerifyPassword(string pwd, byte[] hashedPwd, byte[] pwdSalt)
        => (new HMACSHA512(pwdSalt))// HMAC
            .ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd))// hashed pwd
            .SequenceEqual(hashedPwd);// compare just hashed password with the already stored hashed password
    
    private string GenerateToken(User user)
        =>  new JwtSecurityTokenHandler().WriteToken
        (
            new JwtSecurityToken (
                claims: new List<Claim>() {
                            new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
                            new Claim(ClaimTypes.Name, user.userEmail),
                            new Claim(ClaimTypes.Role, user.Role)
                        },
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials (
                                        new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value/*??"Value not found"*/)),/*key*/
                                        SecurityAlgorithms.HmacSha512Signature/*algorithm*/
                                    )
            )
        );
        
    public async Task<ServiceResponder<bool>> ChangePassword(int userId, string oldPassword, string newPassword)
    {
        User? user = await _dbContext.Users.FindAsync(userId);
        if (user != null)
        {
            byte[] oldPasswordHashed = HMACSHA512.HashData(user.userPasswordSalt, System.Text.Encoding.UTF8.GetBytes(oldPassword));
            if (!user.userPasswordHashed.SequenceEqual(oldPasswordHashed))
            {
                Console.WriteLine(BitConverter.ToString(user.userPasswordHashed) + "\n" + BitConverter.ToString(oldPasswordHashed));
                return new ServiceResponder<bool>()
                {
                    IsSuccess = false,
                    Message = "Your entered Current password doesn't match current password"
                };
            }

            HashPassword(newPassword, out byte[] newPasswordHashed, out byte[] newPasswordSalt);
            user.userPasswordHashed = newPasswordHashed;
            user.userPasswordSalt = newPasswordSalt;

            await _dbContext.SaveChangesAsync();
            return new ServiceResponder<bool>()
            {
                Data = true,
                Message = "Change Password Successfully! You'll be logged out in 5 seconds, please log in with your new password!"
            };
        }
        return new ServiceResponder<bool>()
        {
            IsSuccess = false,
            Message = "Can't Find Your User Account"
        };
    }
}