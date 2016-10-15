using System;
using System.ComponentModel.DataAnnotations.Schema;
using home.manager.Models;

namespace home.manager.Shared
{
    public class SharedCategory
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        [NotMapped]
        public virtual bool IsEmpty { get; set; }
        public virtual string Description { get; set; }
    }

    public class SharedItem
    {
        public int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual UserProfile Owner { get; set; }
    }
}