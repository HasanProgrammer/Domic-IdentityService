#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.DTOs;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Microsoft.Extensions.Configuration;

namespace Domic.UseCase.UserUseCase.Commands.OtpVerification;

public class OtpVerificationCommandHandler(IOtpLogCommandRepository otpLogCommandRepository, IDateTime dateTime, 
    ISerializer serializer, IUserQueryRepository userQueryRepository, IExternalDistributedCache distributedCache,
    IConfiguration configuration, IJsonWebToken jsonWebToken
) : ICommandHandler<OtpVerificationCommand, string>
{
    private readonly object _validationResult;
    
    public Task BeforeHandleAsync(OtpVerificationCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(OtpVerificationCommand command, CancellationToken cancellationToken)
    {
        var targetOtpLog = _validationResult as OtpLog;
        
        targetOtpLog.ChangeVerification(dateTime, command.PhoneNumber, true, targetOtpLog.UserId, targetOtpLog.CreatedRole);

        await otpLogCommandRepository.ChangeAsync(targetOtpLog, cancellationToken);

        var targetUser =
            await userQueryRepository.FindByIdEagerLoadingAsync(targetOtpLog.UserId, cancellationToken);

        if (targetUser.IsActive == IsActive.InActive)
            throw new UseCaseException("حساب کاربری شما مسدود شده است!");
        
        var roles       = targetUser.RoleUsers.Select(role => role.Role.Name);
        var permissions = targetUser.PermissionUsers.Select(permission => permission.Permission.Name);

        await distributedCache.SetCacheValueAsync(
            new KeyValuePair<string, string>($"{targetUser.Username}-permissions", serializer.Serialize(permissions)),
            cancellationToken: cancellationToken
        );

        var claims = new List<KeyValuePair<string, string>>();
        
        claims.Add(new KeyValuePair<string, string>("user_identity", targetUser.Id));
        claims.Add(new KeyValuePair<string, string>("username", targetUser.Username));
        
        claims.AddRange(
            roles.Select(role => new KeyValuePair<string, string>("role", role)) 
        );

        var tokenParameter = new TokenParameterDto {
            Key      = configuration.GetValue<string>("JWT:Key")      ,
            Issuer   = configuration.GetValue<string>("JWT:Issuer")   ,
            Audience = configuration.GetValue<string>("JWT:Audience") ,
            Expires  = configuration.GetValue<int>("JWT:Expire")
        };

        var token = jsonWebToken.Generate(tokenParameter, claims.ToArray());

        targetUser.Token = token;

        await userQueryRepository.ChangeAsync(targetUser, cancellationToken);

        return token;
    }

    public Task AfterHandleAsync(OtpVerificationCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}