
using STUDENTU_1._06.Model.DBModelClasses;
using System.Data.Entity;

namespace STUDENTU_1._06.Model
{
    class StudentuConteiner : DbContext
    {
        public StudentuConteiner() : base("name=StudPersons")
        {

            // DbMigrationsConfiguration.AutomaticMigrationsEnabled=true;
            Database.SetInitializer<StudentuConteiner>
             (new DropCreateDatabaseIfModelChanges<StudentuConteiner>());
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<AuthorStatus> AuthorStatuses { get; set; }
        public virtual DbSet<AfterDoneDescription> AfterDoneDescriptions { get; set; }        
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<Dates> Dates { get; set; }
        public virtual DbSet<Direction> Directions { get; set; }
        public virtual DbSet<Evaluation> Evaluations { get; set; }
        public virtual DbSet<Money> Moneys { get; set; }
        public virtual DbSet<OrderLine> Orderlines { get; set; }
        public virtual DbSet<Persone> Persones { get; set; }
        public virtual DbSet<PersoneDescription> PersoneDescriptions { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WorkType> WorkTypes { get; set; }
        

    }
}
