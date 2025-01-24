using Domic.Core.Persistence.Configs;
using Domic.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domic.Persistence.Configs.C;

public class OtpLogConfig : BaseEntityConfig<OtpLog, string>
{
    public override void Configure(EntityTypeBuilder<OtpLog> builder)
    {
        base.Configure(builder);
        
        /*-----------------------------------------------------------*/
        
        //Configs

        builder.ToTable("OtpLogs");

        /*-----------------------------------------------------------*/
        
        //Relations
    }
}