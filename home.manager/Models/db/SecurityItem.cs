using System;
using home.manager.Shared;

namespace home.manager.Models.db
{
    public class SecurityItem : SharedItem
    {
        public SecurityItem()
        {
            Date = DateTime.Now;
        }
        public virtual SecurityCategory Category { get; set; }
        public string Link { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class SecurityCategory : SharedCategory { }
}