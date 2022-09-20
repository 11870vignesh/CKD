//Commmented by rishi
// <<<<<<< HEAD
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.Extensions.Options;
// using Microsoft.IdentityModel.Tokens;
// using Models.common;
// using Models.entity;
// using Models.entity;

// namespace WebAPI.apiauthorization;
// =======
// // using System;
// // using System.IdentityModel.Tokens.Jwt;
// // using System.Security.Claims;
// // using System.Text;
// // using Microsoft.Extensions.Options;
// // using Microsoft.IdentityModel.Tokens;
// // using Models.common;
// // using Models.entity;
// // using Models.entity;

// // namespace WebAPI.apiauthorization;
// >>>>>>> 230039a1d35c24e211939ee1b099f0cfed42e374


// // public class AppSettings
// // {
// //     public string? Secret { get; set; }
// // }

// // public interface IJwtUtils
// // {
// //     public string GenerateToken(User user);
// //     public int? ValidateToken(string token);
// // }

// <<<<<<< HEAD
// public class JwtUtils : IJwtUtils
// {
//     private readonly string hashSecret;

//     public JwtUtils(IOptions<AppSettings> appSettings)
//     {
//         if (appSettings == null)
//             throw new Exception("AppSettings was null/Unable to get AppSettings object!");
// =======
// // public class JwtUtils : IJwtUtils
// // {
// //     private readonly string hashSecret;
// >>>>>>> 230039a1d35c24e211939ee1b099f0cfed42e374

// //     public JwtUtils(IOptions<AppSettings> appSettings)
// //     {
// //         if (appSettings == null)
// //             throw new Exception("AppSettings was null/Unable to get AppSettings object!");

// <<<<<<< HEAD
//         if (string.IsNullOrEmpty(setting.Secret))
//             throw new Exception("Missing or empty Hash Secret in AppSettings!");
// =======
// //         var setting = appSettings.Value;
// //         if (setting == null)
// //             throw new Exception("AppSettings is empty or not configured!");
// >>>>>>> 230039a1d35c24e211939ee1b099f0cfed42e374

// //         if (string.IsNullOrEmpty(setting.Secret))        
// //             throw new Exception("Missing or empty Hash Secret in AppSettings!");

// //         hashSecret = setting.Secret;
// //     }

// <<<<<<< HEAD
//     public int? ValidateToken(string token)
//     {
//         if (token == null)
//             return null;
// =======
// //     public string GenerateToken(User user)
// //     {
// //         // generate token that is valid for 7 days
// //         var tokenHandler = new JwtSecurityTokenHandler();
// //         var key = Encoding.ASCII.GetBytes(hashSecret);
// //         var tokenDescriptor = new SecurityTokenDescriptor
// //         {
// //             Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
// //             Expires = DateTime.UtcNow.AddDays(7),
// //             SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
// //         };
// //         var token = tokenHandler.CreateToken(tokenDescriptor);
// //         return tokenHandler.WriteToken(token);
// //     }
// >>>>>>> 230039a1d35c24e211939ee1b099f0cfed42e374

// //     public int? ValidateToken(string token)
// //     {
// //         if (token == null) 
// //             return null;

// //         var tokenHandler = new JwtSecurityTokenHandler();
// //         var key = Encoding.ASCII.GetBytes(hashSecret);
// //         try
// //         {
// //             tokenHandler.ValidateToken(token, new TokenValidationParameters
// //             {
// //                 ValidateIssuerSigningKey = true,
// //                 IssuerSigningKey = new SymmetricSecurityKey(key),
// //                 ValidateIssuer = false,
// //                 ValidateAudience = false,
// //                 // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
// //                 ClockSkew = TimeSpan.Zero
// //             }, out SecurityToken validatedToken);

// //             var jwtToken = (JwtSecurityToken)validatedToken;
// //             var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

// //             // return user id from JWT token if validation successful
// //             return userId;
// //         }
// //         catch
// //         {
// //             // return null if validation fails
// //             return null;
// //         }
// //     }
// // }
