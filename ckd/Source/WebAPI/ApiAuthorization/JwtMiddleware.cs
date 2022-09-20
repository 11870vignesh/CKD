//Commmented by rishi
// using Models.model;

// namespace WebAPI.apiauthorization;

// // public class JwtMiddleware
// // {
// //     private readonly RequestDelegate _next;

// //     public JwtMiddleware(RequestDelegate next)
// //     {
// //         _next = next;
// //     }

// //     public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
// //     {
// //         var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//         if (!string.IsNullOrEmpty(token))
//         {
//             var userId = jwtUtils.ValidateToken(token);
//             if (userId != null)
//             {
//                 // attach user to context on successful jwt validation
//                 UserModel user = new UserModel();
//                 user.Name = "Shakeel";
//                 context.Items["User"] = user;// userService.GetById(userId.Value);
//             }
//         }
//         await _next(context);
//     }
// }
