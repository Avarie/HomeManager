using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using home.manager.Shared;


namespace home.manager.Models.db
{
    [Table("Expenses")]
    public class Expense : SharedItem
    {
        public virtual Category Category { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public double SpentMoney { get; set; }
    }

    public class Category : SharedCategory { }
    public class SubCategory : SharedCategory
    {
        [JsonIgnore]
        virtual public Category Category { get; set; }
    }
}
