using System.Collections.Generic;

namespace EfPostgre.Pg.Model
{
    public class Category
    {
        // эти свойства соотносятся со столбцами в БД
        public string CategoryID { get; set; }
        
        public string CategoryName { get; set; }
        
        //[Column(TypeName = "ntext")]
        public string Description { get; set; }
        
        // определяет свойство navigation для связанных строк
        public virtual ICollection<Product> Products { get; } = new List<Product>();
        public Category()
        {
        // чтобы позволить разработчикам добавлять товары в Category,
        // мы должны инициализировать свойства navigation пустым списком
            //2.1 Core EF - перенесено в конструктор
        //this.Products = new List<Product>();
        }    
    }
}