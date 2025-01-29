using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.ValueObjects;
using Domic.Domain.User.Events;

namespace Domic.Domain.User.Entities;

public class OtpLog : Entity<string>
{
    //Fields
    
    public string UserId { get; private set; }
    public string MessageContent { get; private set; }
    public bool IsVerified { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Value Objects

    /*---------------------------------------------------------------*/
    
    //Relations
    
    /*---------------------------------------------------------------*/

    //EF Core
    private OtpLog() {}

    public OtpLog(IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, ISerializer serializer,
        string userId, string phoneNumber, string messageContent, List<string> roles
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id             = globalUniqueIdGenerator.GetRandom(6);
        UserId         = userId;
        MessageContent = messageContent;
        ExpiredAt      = DateTime.UtcNow.AddMinutes(2);
        
        //audit
        CreatedBy   = userId;
        CreatedRole = serializer.Serialize(roles);
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new OtpLogCreated {
                Id             = Id,
                UserId         = userId, 
                PhoneNumber    = phoneNumber,
                MessageContent = messageContent, 
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
    /// <param name="phoneNumber"></param>
    /// <param name="verificationStatus"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedRoles"></param>
    public void ChangeVerification(IDateTime dateTime, string phoneNumber, bool verificationStatus, 
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
            new OtpLogUpdated {
                Id             = Id,
                UserId         = UserId,
                PhoneNumber    = phoneNumber,
                MessageContent = MessageContent, 
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