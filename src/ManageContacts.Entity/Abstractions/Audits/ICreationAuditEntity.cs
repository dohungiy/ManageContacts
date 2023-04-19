namespace ManageContacts.Entity.Abstractions.Audits;

public interface ICreationAuditEntity
{
    DateTime CreatedTime { get; set; }

    Guid? CreatorId { get; set; }
}