using Domic.Core.Identity.Grpc;
using Domic.Core.Common.ClassExtensions;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.UserMappers;

//Query
public static partial class RpcResponseExtension
{
    
}

//Command
public partial class RpcResponseExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="response"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this string response, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(SignInResponse))
        {
            Response = new SignInResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessSignInMessage() ,
                Body    = new SignInResponseBody { Token = response }
            };
        }

        return (T)Response;
    }
}