#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace OmegaExplorer.Server.Services._Game._core.Models.Classes;

/// <summary>
///     Resource identifiable into the database
/// </summary>
public class Identifiable
{
    /// <summary>
    ///     Id of the element into the database
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     Name of the element
    /// </summary>
    [MaxLength(255)]
    public string? Name { get; set; }

    public T CopyEntity<T>() where T : Identifiable
    {
        var res = MemberwiseClone();

        if (res is T entity)
        {
            entity.Id = Guid.NewGuid();
            return entity;
        }

        throw new Exception(
            $"Error while copying entity with CopyEntity() method, object is not a {typeof(T)} compatible cast element ! Source is : {res.GetType().Name}");
    }
}