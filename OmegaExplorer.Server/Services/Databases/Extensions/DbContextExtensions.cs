using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;

namespace OmegaExplorer.Server.Services.Databases.Extensions;

public static class DbContextExtensions
{
    /// <summary>
    ///     <!> No need to savechanges after calling this method
    ///     <remarks>NO NEED TO SAVE CHANGES</remarks>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<T> CreateOrUpdateAsync<T>(this DbContext context, T entity) where T : class
    {
        try
        {
            var dbSet = context.Set<T>();

            // Assuming 'ID' is the name of your primary key property
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("No property 'Id' found on the entity");
            }

            var entityId = idProperty.GetValue(entity);
            var existingEntity = await dbSet.FindAsync(entityId);

            if (existingEntity == null)
            {
                dbSet.Add(entity);
            }
            else
            {
                context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            await context.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Log.Error(e, $"Error while creating or updating entity");
            throw;
        }
    }

    public static IQueryable<SpatialObject> WithinDistance(this IQueryable<SpatialObject> query, Vector2 position, float distance)
    {
        var squaredDistance = distance * distance;
        var positionX = position.X;
        var positionY = position.Y;

        // Using squared distance to avoid computing square root
        // (x1 - x2)^2 + (y1 - y2)^2 < d^2
        return query.Where(o => (o.SpatialLocation.Position.X - positionX) * (o.SpatialLocation.Position.X - positionX) + (o.SpatialLocation.Position.Y - positionY) * (o.SpatialLocation.Position.Y - positionY) < squaredDistance);
    }
}