using Domic.Core.Common.ClassExtensions;
using Grpc.Core;
using Domic.Core.Identity.Grpc;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.UserUseCase.Commands.EmailOtpGeneration;
using Domic.UseCase.UserUseCase.Commands.OtpGeneration;
using Domic.UseCase.UserUseCase.Commands.OtpVerification;
using Domic.UseCase.UserUseCase.Commands.ResetPassword;
using Domic.UseCase.UserUseCase.Commands.SignIn;
using Domic.WebAPI.Frameworks.Extensions.Mappers.UserMappers;

namespace Domic.WebAPI.EntryPoints.GRPCs;

public class AuthRPC : IdentityService.IdentityServiceBase
{
    private readonly IMediator      _mediator;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="configuration"></param>
    public AuthRPC(IMediator mediator, IConfiguration configuration)
    {
        _mediator      = mediator;
        _configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<SignInCommand>();
        
        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);

        return result.ToRpcResponse<SignInResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<OtpGenerationResponse> OtpGeneration(OtpGenerationRequest request, 
        ServerCallContext context
    )
    {
        var command = new OtpGenerationCommand { PhoneNumber = request.PhoneNumber.Value };
        
        var result = await _mediator.DispatchAsync<bool>(command, context.CancellationToken);

        return new() {
            Code    = _configuration.GetSuccessCreateStatusCode(),
            Message = _configuration.GetSuccessCreateMessage(),
            Body    = new OtpGenerationResponseBody { Result = result }
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<OtpVerificationResponse> OtpVerification(OtpVerificationRequest request,
        ServerCallContext context
    )
    {
        var command = new OtpVerificationCommand { PhoneNumber = request.PhoneNumber.Value, Code = request.Code.Value };
        
        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);

        return new() {
            Code    = _configuration.GetSuccessCreateStatusCode(),
            Message = _configuration.GetSuccessCreateMessage(),
            Body    = new OtpVerificationResponseBody { Token = result }
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<EmailOtpGenerationResponse> EmailOtpGeneration(EmailOtpGenerationRequest request, 
        ServerCallContext context
    )
    {
        var command = new EmailOtpGenerationCommand { EmailAddress = request.EmailAddress.Value };
        
        var result = await _mediator.DispatchAsync(command, context.CancellationToken);

        return new() {
            Code    = _configuration.GetSuccessCreateStatusCode(),
            Message = _configuration.GetSuccessCreateMessage(),
            Body    = new EmailOtpGenerationResponseBody { Result = result }
        };
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request, ServerCallContext context)
    {
        var command = new ResetPasswordCommand {
            NewPassword  = request.NewPassword.Value  , 
            EmailAddress = request.EmailAddress.Value ,
            EmailCode    = request.EmailCode.Value 
        };
        
        var result = await _mediator.DispatchAsync(command, context.CancellationToken);

        return new() {
            Code    = _configuration.GetSuccessCreateStatusCode(),
            Message = _configuration.GetSuccessCreateMessage(),
            Body    = new ResetPasswordResponseBody { Result = result }
        };
    }
}