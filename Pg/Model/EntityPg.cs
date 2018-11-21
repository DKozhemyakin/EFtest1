using System.ComponentModel.DataAnnotations;

namespace EfPostgre.Pg.Model
{
    public interface IEntity
    {
        string Id { get; set; }
    }

    public class EntityPg : IEntity
    {
        [Key]
        [MaxLength(40)]
        public string Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }
    }
    
}