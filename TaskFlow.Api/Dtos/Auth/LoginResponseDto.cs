using System;

namespace TaskFlow.Api.Dtos.Auth;

public class LoginResponseDto
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
}
