﻿namespace AuthSerrviceAPI.Interface
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);

    }
}
