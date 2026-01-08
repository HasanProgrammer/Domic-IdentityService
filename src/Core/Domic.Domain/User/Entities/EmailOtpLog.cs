using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.ValueObjects;
using Domic.Domain.User.Events;

namespace Domic.Domain.User.Entities;

public class EmailOtpLog : Entity<string>
{
    //Fields
    
    public string UserId { get; private set; }
    public string Code { get; private set; }
    public bool IsVerified { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Value Objects

    /*---------------------------------------------------------------*/
    
    //Relations
    
    /*---------------------------------------------------------------*/

    //EF Core
    private EmailOtpLog() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="dateTime"></param>
    /// <param name="serializer"></param>
    /// <param name="userId"></param>
    /// <param name="emailAddress"></param>
    /// <param name="code"></param>
    /// <param name="roles"></param>
    public EmailOtpLog(IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, 
        ISerializer serializer, string userId, string emailAddress, string code, List<string> roles
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id        = globalUniqueIdGenerator.GetRandom(6);
        UserId    = userId;
        Code      = code;
        ExpiredAt = DateTime.UtcNow.AddMinutes(2);
        
        //audit
        CreatedBy   = userId;
        CreatedRole = serializer.Serialize(roles);
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new EmailOtpLogCreated {
                Id             = Id,
                UserId         = userId, 
                EmailAddress   = emailAddress,
                MessageContent = code, 
                ExpiredAt      = ExpiredAt,
                IsVerified     = false,
                CreatedBy      = CreatedBy,
                CreatedRole    = CreatedRole,
                CreatedAt_EnglishDate = nowDateTime,
                CreatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
    
    /*---------------------------------------------------------------*/
    
    //Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="emailAddress"></param>
    /// <param name="verificationStatus"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedRoles"></param>
    public void ChangeVerification(IDateTime dateTime, string emailAddress, bool verificationStatus, 
        string updatedBy, string updatedRoles
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        IsVerified = verificationStatus;
        
        //audit
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRoles;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new EmailOtpLogUpdated {
                Id             = Id,
                UserId         = UserId,
                EmailAddress   = emailAddress,
                MessageContent = Code, 
                ExpiredAt      = ExpiredAt,
                IsVerified     = true,
                UpdatedBy      = UpdatedBy,
                UpdatedRole    = UpdatedRole,
                UpdatedAt_EnglishDate = nowDateTime,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
}