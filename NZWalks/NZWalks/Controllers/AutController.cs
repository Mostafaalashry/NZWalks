using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.DTO;
using System;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repository;
using NZWalks.CustomActionFilters;
using Microsoft.AspNetCore.Rewrite;

namespace NZWalks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
	public class AutController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        private ITokenRepository tokenRepository;

        public AutController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
		{
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;

        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var identityUser = new IdentityUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequest.Password);

            if (!identityResult.Succeeded)
                return BadRequest(new { Errors = identityResult.Errors });

            if (registerRequest.Roles != null && registerRequest.Roles.Any())
            {
                var roleResult = await userManager.AddToRolesAsync(identityUser, registerRequest.Roles);
                if (!roleResult.Succeeded)
                    return BadRequest(new { Errors = roleResult.Errors });
            }

            return Ok(new { Message = "User registered successfully." });
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await userManager.FindByEmailAsync(loginRequest.UserName);
            if (user != null)
            {
              var checkPasswordResult=  await userManager.CheckPasswordAsync(user,loginRequest.Password);
                if (checkPasswordResult)
                {
                    //Get Roles
                    var roles = await userManager.GetRolesAsync(user);
                    //CreateToken later
                    if (roles != null)
                      {
                        var jwtToken = tokenRepository.CreateJWTToken(user,roles.ToList());
                        var Response = new LoginResponseDTO {jwtToken=jwtToken };
                        return Ok(Response);
                    }
                   
                   

                }
            }
            return BadRequest("Username or password incorrect");

        }

    }
}

//{
//    "userName": "reader@nzwalks.com",
//  "password": "reader@123",
//  "roles": [
//    "Reader"
//  ]
//}
//eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJyZWFkZXJAbnp3YWxrcy5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJSZWFkZXIiLCJleHAiOjE3NDkxNTU1MDQsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Qvd2VicG9ydGFsIn0.Gy5ysyHSlTtFJmsbceM5IwY481CnoApaN3olgBwLHcE