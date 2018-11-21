using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfPostgre.Pg.Model
{
    public class Product
    {

        public string ProductID { get; set; }
        [Required] [MaxLength(40)] 
        public string ProductName { get; set; }

        //[Column("UnitPrice")]
        public decimal? Cost { get; set; }

        //[Column("UnitsInStock")]
        public short? Stock { get; set; }

        public bool Discontinued { get; set; }

        // эти два свойства определяют отношение вторичного ключа к таблице Categories
        public string CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}