using System.Security.Claims;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.DTOs;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.User.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Karami.UseCase.UserUseCase.Commands.SignIn;

public class SignInCommandHandler : ICommandHandler<SignInCommand, string>
{
    private readonly IConfiguration       _configuration;
    private readonly IJsonWebToken        _jsonWebToken;
    private readonly ISerializer          _serializer;
    private readonly IUserQueryRepository _userQueryRepository;

    public SignInCommandHandler(IUserQueryRepository userQueryRepository, 
        IConfiguration configuration ,
        IJsonWebToken jsonWebToken   ,
        ISerializer serializer
    )
    {
        _configuration       = configuration;
        _jsonWebToken        = jsonWebToken;
        _serializer          = serializer;
        _userQueryRepository = userQueryRepository;
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

        var claims = new List<KeyValuePair<string, string>>();
        
        claims.Add(new KeyValuePair<string, string>("UserId", targetUser.Id));
        claims.Add(new KeyValuePair<string, string>(ClaimTypes.Name, targetUser.Username));
        
        claims.AddRange(
            roles.Select(role => new KeyValuePair<string, string>(ClaimTypes.Role, role)) 
        );
        
        claims.AddRange(
            permissions.Select(permission => new KeyValuePair<string, string>("Permission", permission)) 
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