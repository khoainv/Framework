using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SSD.SSO.Config;

namespace SSD.SSO.Identity
{

    // This is useful if you do not want to tear down the database each time you run the application.
    //When Testing
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    //When Live
    // public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    //When Dev
    // public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = new ApplicationUserManager(db);
            //userManager.PasswordHasher = new CustomPasswordHasher();
            var roleManager = new ApplicationRoleManager(db);
            const string name = "khoainv@fpt.com.vn";
            const string password = "abcde12345-";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, EmailConfirmed = true };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public string ConnectionStringName { get; set; }
        public ApplicationDbContext()
            : base(SSOISConstants.ConnectionStringName, throwIfV1Schema: false)
        {
            //Configuration.LazyLoadingEnabled = false;
            //Configuration.ProxyCreationEnabled = false;
            ConnectionStringName = SSOISConstants.ConnectionStringName;
        }
        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Add the ApplicationPermissions property:
        public virtual IDbSet<ApplicationPermission> ApplicationPermissions { get; set; }
        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Make sure to call the base method first:
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationPermission>().ToTable("AspNetPermissions")
                .HasMany<ApplicationPermissionRole>((ApplicationPermission g) => g.ApplicationRoles)
                .WithRequired()
                .HasForeignKey<string>((ApplicationPermissionRole ap) => ap.ApplicationPermissionId);

            

            modelBuilder.Entity<ApplicationPermissionRole>().HasKey((ApplicationPermissionRole gr) =>
                new
                {
                    ApplicationRoleId = gr.ApplicationRoleId,
                    ApplicationPermissionId = gr.ApplicationPermissionId
                }).ToTable("AspNetPermissionRoles");

            modelBuilder.Entity<AppConfig>().ToTable("AspNetAppConfig");
        }

        public System.Data.Entity.DbSet<IdentityServer3.EntityFramework.Entities.Client> Clients { get; set; }
        public System.Data.Entity.DbSet<IdentityServer3.EntityFramework.Entities.Scope> Scopes { get; set; }
    }
}