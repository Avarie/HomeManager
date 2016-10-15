using System.Data.Entity;
using home.manager.Models.db;
using home.manager.Repositories;
using home.manager.Shared;
using StructureMap;
using StructureMap.Graph;

namespace home.manager.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IContainer>().Use(ObjectFactory.Container);
                x.For<DbContext>().Use<db>();
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(p => p.FullName.StartsWith("home"));
                    scan.Convention<DefaultConventionScanner>();
                    scan.LookForRegistries();
                });

                x.For<ISharedCategoryRepository<ContactCategory>>().Use<ContactCategoryRepository>();
                x.For<ISharedCategoryRepository<NoteCategory>>().Use<NoteCategoryRepository>();
                x.For<ISharedRepository<Note, NoteCategory>>().Use<NoteRepository>();
                x.For<ISharedCategoryRepository<SecurityCategory>>().Use<SecurityCategoryRepository>();
                x.For<ISharedCategoryRepository<Category>>().Use<ExpenseCategoryRepository>();
                x.For<ISharedCategoryRepository<SubCategory>>().Use<ExpenseSubCategoryRepository>();
            });

            return ObjectFactory.Container;
        }
    }
}