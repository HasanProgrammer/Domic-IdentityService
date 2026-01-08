using Domic.Core.Persistence.Configs;
using Domic.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domic.Persistence.Configs.C;

public class EmailOtpLogConfig : BaseEntityConfig<EmailOtpLog, string>
{
    public override void Configure(EntityTypeBuilder<EmailOtpLog> builder)
    {
        base.Configure(builder);
        
        /*-----------------------------------------------------------*/
        
        //Configs

        builder.ToTable("EmailOtpLogs");

        /*-----------------------------------------------------------*/
        
        //Relations
    }
}