﻿using AdWriter_Application.DTOs;
using AdWriter_Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdWriter_AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequestDto req)
        {
            var token = await _auth.RegisterAsync(req.Email, req.Password);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto req)
        {
            var token = await _auth.LoginAsync(req.Email, req.Password);
            return Ok(new { token });
        }
    }
}
