using Karami.WebAPI.Frameworks.Middlewares;

namespace Karami.WebAPI.Frameworks.Extensions;

public static class IApplicationBuilderExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Builder"></param>
    /// <param name="Configuration"></param>
    public static void UseSecretKeyHandler(this IApplicationBuilder Builder, IConfiguration Configuration) => 
        Builder.UseMiddleware<SecretKeyHandler>(Configuration);
}