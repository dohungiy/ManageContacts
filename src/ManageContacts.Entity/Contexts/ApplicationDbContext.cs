using EFCore.BulkExtensions;
using ManageContacts.Entity.Abstractions.Audits;
using Microsoft.EntityFrameworkCore;

namespace ManageContacts.Entity.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public ApplicationDbContext() { }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        this.ApplyAuditFieldsToModifiedEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        this.ApplyAuditFieldsToModifiedEntities();
        return base.SaveChanges();
    }
    
    public void BulkInsert<TEntity>(IList<TEntity> listEntities) where TEntity : class
    {
        foreach (var entity in listEntities)
        {
            if (entity is ICreationAuditEntity creationAuditEntity)
            {
                creationAuditEntity.CreatedTime = DateTime.UtcNow;
            }
        }
        DbContextBulkExtensions.BulkInsert<TEntity>(this, listEntities);
    }
    
    public async Task BulkInsertAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default) where TEntity : class
    {
        foreach (var entity in listEntities)
        {
            if (entity is ICreationAuditEntity creationAuditEntity)
            {
                creationAuditEntity.CreatedTime = DateTime.UtcNow;
            }
        }
        await DbContextBulkExtensions.BulkInsertAsync<TEntity>(this, listEntities, cancellationToken: cancellationToken);
    }
    
    public void BulkUpdate<TEntity>(IList<TEntity> listEntities) where TEntity : class
    {
        foreach (var entity in listEntities)
        {
            if (entity is IModificationAuditEntity modificationAuditEntity)
            {
                modificationAuditEntity.ModifiedTime = DateTime.UtcNow;
            }
        }
        DbContextBulkExtensions.BulkUpdate<TEntity>(this, listEntities);
    }
    
    public async Task BulkUpdateAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default) where TEntity : class
    {
        foreach (var entity in listEntities)
        {
            if (entity is IModificationAuditEntity modificationAuditEntity)
            {
                modificationAuditEntity.ModifiedTime = DateTime.UtcNow;
            }
        }
        await DbContextBulkExtensions.BulkUpdateAsync<TEntity>(this, listEntities, cancellationToken: cancellationToken);
    }


    public void BulkDelete<TEntity>(IList<TEntity> listEntities) where TEntity : class
    {
        foreach (var entity in listEntities)
        {
            if (entity is IDeletionAuditEntity deletionAuditEntity)
            {
                deletionAuditEntity.Deleted = true;
            }
        }
        
        DbContextBulkExtensions.BulkDelete<TEntity>(this, listEntities);
    }
    
    public async Task BulkDeleteAsync<TEntity>(IList<TEntity> listEntities, CancellationToken cancellationToken = default) where TEntity : class
    {
        foreach (var entity in listEntities)
        {
            if (entity is IDeletionAuditEntity deletionAuditEntity)
            {
                deletionAuditEntity.Deleted = true;
            }
        }
        
        await DbContextBulkExtensions.BulkDeleteAsync<TEntity>(this, listEntities, cancellationToken : cancellationToken);
    }

    private void ApplyAuditFieldsToModifiedEntities()
    {
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added 
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted);

        foreach (var entry in modified)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is ICreationAuditEntity creationAuditEntity)
                    {
                        creationAuditEntity.CreatedTime = DateTime.UtcNow;
                        entry.State = EntityState.Added;
                    }
                    break;
                case EntityState.Modified:
                    if (entry.Entity is IModificationAuditEntity modificationAuditEntity)
                    {
                        modificationAuditEntity.ModifiedTime = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }
                    break;
                case EntityState.Deleted:
                    if (entry.Entity is IDeletionAuditEntity deletionAuditEntity)
                    {
                        deletionAuditEntity.Deleted = true;
                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }
    }
    
}