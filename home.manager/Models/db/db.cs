using System.Data.Entity;

namespace home.manager.Models.db
{
    public class db : DbContext
    {
        public db() : base("name=db") { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<ContactCategory> ContactCategories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactLine> ContactLines { get; set; }
        public virtual DbSet<SecurityItem> SecurityItems { get; set; }
        public virtual DbSet<SecurityCategory> SecurityCategories { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<NoteCategory> NoteCategories { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasMany(x => x.ContactLines)
                .WithOptional()
                .WillCascadeOnDelete(true);
        }

    }
}
