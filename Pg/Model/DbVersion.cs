using System.ComponentModel.DataAnnotations;

namespace EfPostgre.Pg.Model
{
    public class DbVersion : EntityPg
    {
        [MaxLength(20)]
        public string Version { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Version)}: {Version}";
        }
    }
}