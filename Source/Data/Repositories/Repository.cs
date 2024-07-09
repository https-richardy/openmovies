namespace OpenMovies.WebApi.Data.Repositories;

public abstract class Repository<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : Entity
    where TDbContext : DbContext
{
    protected ILogger<Repository<TEntity, TDbContext>> Logger { get; set; }
    protected TDbContext DbContext { get; private set; }

    public Repository(TDbContext dbContext, ILogger<Repository<TEntity, TDbContext>> logger)
    {
        DbContext = dbContext;
        Logger = logger;
    }

    public virtual async Task<OperationResult> DeleteAsync(TEntity entity)
    {
        try
        {
            Logger.LogInformation($"Deleting entity `{typeof(TEntity).Name}` with id `{entity.Id}` from database...");

            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation($"Entity `{typeof(TEntity).Name}` with id `{entity.Id}` deleted successfully.");
            return OperationResult.Success($"Entity {typeof(TEntity).Name} deleted successfully.");
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to delete entity `{Entity}` from database", typeof(TEntity).Name);
            return OperationResult.Failure($"Failed to delete entity {typeof(TEntity).Name}. Error: {exception.Message}");
        }
    }

    public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            Logger.LogInformation("Fetching entities `{Entity}` from database.", typeof(TEntity).Name);

            return await DbContext.Set<TEntity>()
                .Where(predicate)
                .ToListAsync();
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to fetch entities `{Entity}` from database", typeof(TEntity).Name);
            return Enumerable.Empty<TEntity>();
        }
    }

    public virtual async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            Logger.LogInformation("Fetching entity `{Entity}` from database.", typeof(TEntity).Name);
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to fetch entity `{Entity}` from database", typeof(TEntity).Name);
            return null;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            Logger.LogInformation("Fetching all entities `{Entity}` from database.", typeof(TEntity).Name);
            return await DbContext.Set<TEntity>().ToListAsync();
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to fetch all entities `{Entity}` from database", typeof(TEntity).Name);
            return Enumerable.Empty<TEntity>();
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        try
        {
            Logger.LogInformation("Fetching entity `{Entity}` with id `{Id}` from database.", typeof(TEntity).Name, id);
            return await DbContext.Set<TEntity>().FindAsync(id);
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to fetch entity `{Entity}` with id `{Id}` from database", typeof(TEntity).Name, id);
            return null;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> PagedAsync(int pageNumber, int pageSize)
    {
        try
        {
            /* Represents the adjustment applied to page numbers to align with zero-based indices in LINQ queries. */
            const int pageIndexAdjustment = 1;
            Logger.LogInformation("Fetching entities from database. Page number: {PageNumber}, Page size: {PageSize}", pageNumber, pageSize);

            return await DbContext.Set<TEntity>()
                .Skip((pageNumber - pageIndexAdjustment) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to fetch entities `{Entity}` from database", typeof(TEntity).Name);
            return Enumerable.Empty<TEntity>();
        }
    }

    public virtual async Task<IEnumerable<TEntity>> PagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize)
    {
        try
        {
            /* Represents the adjustment applied to page numbers to align with zero-based indices in LINQ queries. */
            const int pageIndexAdjustment = 1;
            Logger.LogInformation("Fetching entities from database. Page number: {PageNumber}, Page size: {PageSize}", pageNumber, pageSize);

            return await DbContext.Set<TEntity>()
                .Where(predicate)
                .Skip((pageNumber - pageIndexAdjustment) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to fetch entities `{Entity}` from database", typeof(TEntity).Name);
            return Enumerable.Empty<TEntity>();
        }
    }

    public virtual async Task<OperationResult> SaveAsync(TEntity entity)
    {
        try
        {
            Logger.LogInformation($"Saving entity {typeof(TEntity).Name}");

            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation($"Entity {typeof(TEntity).Name} saved successfully.");

            return OperationResult.Success($"Entity {typeof(TEntity).Name} saved successfully.");
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to save entity: {Entity}", typeof(TEntity).Name);
            return OperationResult.Failure($"Failed to save entity {typeof(TEntity).Name}. Error: {exception.Message}");
        }
    }

    public virtual async Task<OperationResult> UpdateAsync(TEntity entity)
    {
        try
        {
            var existingEntity = await DbContext.Set<TEntity>().FindAsync(entity.Id);

            if (existingEntity != null)
            {
                DbContext.Entry(existingEntity).State = EntityState.Detached;
                DbContext.Entry(entity).State = EntityState.Modified;

                await DbContext.SaveChangesAsync();

                Logger.LogInformation($"Entity {typeof(TEntity).Name} updated successfully.");
                return OperationResult.Success($"Entity {typeof(TEntity).Name} updated successfully.");
            }

            Logger.LogWarning($"Entity {typeof(TEntity).Name} not found.");
            return OperationResult.Failure($"Entity {typeof(TEntity).Name} not found.");
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Failed to update entity: {Entity}", typeof(TEntity).Name);
            return OperationResult.Failure($"Failed to update entity {typeof(TEntity).Name}. Error: {exception.Message}");
        }
    }
}