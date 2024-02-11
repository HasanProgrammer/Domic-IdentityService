using Domic.Core.Auth.Grpc;
using Domic.UseCase.UserUseCase.Commands.SignIn;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.UserMappers;

//Query
public static partial class RpcRequestExtension
{
    
}

//Command
public partial class RpcRequestExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this SignInRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(SignInCommand))
        {
            Request = new SignInCommand {
                Username = request.Username?.Value ,
                Password = request.Password?.Value
            };
        }

        return (T)Request;
    }
}