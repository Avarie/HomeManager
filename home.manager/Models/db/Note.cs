using System;
using home.manager.Shared;

namespace home.manager.Models.db
{
    public class Note : SharedItem
    {
        public Note()
        {
            Content = String.Empty;
            Date = DateTime.Now;
            PublicId = Guid.NewGuid().ToString();
        }

        public string PublicId { get; set; }
        public String Content { get; set; }
        public virtual NoteCategory Category { get; set; }
    }

    public class NoteCategory : SharedCategory { }
}