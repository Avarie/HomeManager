using System;
using System.Collections.Generic;
using home.manager.Shared;

namespace home.manager.Models.db
{
    public class Contact : SharedItem
    {
        public Contact()
        {
            PublicId = Guid.NewGuid().ToString();
            Date = DateTime.Now;
        }

        public string PublicId { get; set; }
        public virtual ICollection<ContactLine> ContactLines { get; set; }
        public virtual ContactCategory Category { get; set; }
    }

    public class ContactCategory : SharedCategory { }

    public class ContactLine
    {
        public ContactLine()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ContactLine Clone()
        {
            return new ContactLine
            {
                Name = Name,
                Description = Description
            };
        }
    }
}