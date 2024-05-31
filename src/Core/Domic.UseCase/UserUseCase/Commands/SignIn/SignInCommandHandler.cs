using System.Security.Claims;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Extensions;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.DTOs;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Domic.UseCase.UserUseCase.Commands.SignIn;

public class SignInCommandHandler : ICommandHandler<SignInCommand, string>
{
    private readonly IConfiguration            _configuration;
    private readonly IJsonWebToken             _jsonWebToken;
    private readonly IUserQueryRepository      _userQueryRepository;
    private readonly ISerializer               _serializer;
    private readonly IExternalDistributedCache _distributedCache;

    public SignInCommandHandler(
        IUserQueryRepository userQueryRepository, 
        IConfiguration configuration,
        IJsonWebToken jsonWebToken,
        ISerializer serializer,
        IExternalDistributedCache distributedCache
    )
    {
        _configuration       = configuration;
        _jsonWebToken        = jsonWebToken;
        _userQueryRepository = userQueryRepository;
        _distributedCache    = distributedCache;
        _serializer          = serializer;
    }

    public async Task<string> HandleAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        var targetUser =
            await _userQueryRepository.FindByUsernameEagerLoadingAsync(command.Username, cancellationToken);
        
        var hashedPassword = await command.Password.HashAsync(cancellationToken);
        
        if (targetUser == null || !targetUser.Password.Equals(hashedPassword))
            throw new UseCaseException("نام کاربری یا رمز عبور عبور صحیح نمی باشد !");
        
        var roles       = targetUser.RoleUsers.Select(role => role.Role.Name);
        var permissions = targetUser.PermissionUsers.Select(permission => permission.Permission.Name);

        await _distributedCache.SetCacheValueAsync(
            new KeyValuePair<string, string>($"{command.Username}-permissions", _serializer.Serialize(permissions)),
            cancellationToken: cancellationToken
        );

        var claims = new List<KeyValuePair<string, string>>();
        
        claims.Add(new KeyValuePair<string, string>("UserId", targetUser.Id));
        claims.Add(new KeyValuePair<string, string>(ClaimTypes.Name, targetUser.Username));
        
        claims.AddRange(
            roles.Select(role => new KeyValuePair<string, string>(ClaimTypes.Role, role)) 
        );

        var tokenParameter = new TokenParameterDto {
            Key      = _configuration.GetValue<string>("JWT:Key")      ,
            Issuer   = _configuration.GetValue<string>("JWT:Issuer")   ,
            Audience = _configuration.GetValue<string>("JWT:Audience") ,
            Expires  = _configuration.GetValue<int>("JWT:Expire")
        };

        return _jsonWebToken.Generate(tokenParameter, claims.ToArray());
    }
}