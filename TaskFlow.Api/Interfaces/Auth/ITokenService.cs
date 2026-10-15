using System;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Interfaces.Auth;

public interface ITokenService
{
    public string CreateToken(User user);
}
