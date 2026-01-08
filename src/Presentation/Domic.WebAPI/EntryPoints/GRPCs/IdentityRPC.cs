using Domic.Core.Common.ClassExtensions;
using Grpc.Core;
using Domic.Core.Identity.Grpc;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.UserUseCase.Commands.OtpGeneration;
using Domic.UseCase.UserUseCase.Commands.OtpVerification;
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

    public override Task<EmailOtpGenerationResponse> EmailOtpGeneration(EmailOtpGenerationRequest request, ServerCallContext context)
    {
        return base.EmailOtpGeneration(request, context);
    }

    public override Task<EmailOtpVerificationResponse> EmailOtpVerification(EmailOtpVerificationRequest request, ServerCallContext context)
    {
        return base.EmailOtpVerification(request, context);
    }
    
    public override Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request, ServerCallContext context)
    {
        return base.ResetPassword(request, context);
    }
}