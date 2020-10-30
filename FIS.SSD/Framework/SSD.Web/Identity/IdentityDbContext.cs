using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SSD.Web.Models;
using SSD.Framework;

namespace SSD.Web.Identity
{

    // This is useful if you do not want to tear down the database each time you run the application.
    //When Testing
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    //When Live
    // public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    //When Dev
    // public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<IdentityDbContext>
    {
        protected override void Seed(IdentityDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(IdentityDbContext db)
        {
            var grpManager = new GroupManager(db);
            string grpName = UGConstants.GroupAdmin;

            //Create Role Admin if it does not exist
            var grp = grpManager.FindByName(grpName);
            if (grp == null)
            {
                grp = new Group(grpName);
                var roleresult = grpManager.Create(grp);
            }

            var userManager = new UserManager(db);
            //var roleManager = new RoleManager(db);
            string name = UGConstants.AccountAdmin;// "admin@7i.com.vn";
            const string password = "abcde12345-";
            //const string roleName = "Admin";

            //Create Role Admin if it does not exist
            //var role = roleManager.FindByName(roleName);
            //if (role == null)
            //{
            //    role = new Role(roleName);
            //    var roleresult = roleManager.Create(role);
            //}

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new User { UserName = name, Email = name, EmailConfirmed = true };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            //var rolesForUser = userManager.GetRoles(user.Id);
            //if (!rolesForUser.Contains(role.Name))
            //{
            //    var result = userManager.AddToRole(user.Id, role.Name);
            //}

            var ugrp = grpManager.FindUserGroupById(user.UserName, grp.Id);
            if (ugrp == null)
            {
                var result = grpManager.AddUserToGroup(user.UserName, grp.Id);
            }

        }
    }
  
    public class IdentityDbContext : IdentityDbContext<User>
    {
        public IdentityDbContext()
            : base(UGConstants.ConnectionStringNameIdentity, throwIfV1Schema: false)
        {
            //Configuration.LazyLoadingEnabled = false;
            //Configuration.ProxyCreationEnabled = false;
        }
        public IdentityDbContext(string connectionStrName)
            : base(connectionStrName, throwIfV1Schema: false)
        {
            //Configuration.LazyLoadingEnabled = false;
            //Configuration.ProxyCreationEnabled = false;
        }
        static IdentityDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<IdentityDbContext>(new ApplicationDbInitializer());
        }
        public static IdentityDbContext Create()
        {
            return new IdentityDbContext();
        }

        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Make sure to call the base method first:
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Group>().ToTable("AspNetGroups").HasMany<PermissionGroup>((Group g) => g.Permissions)
                .WithRequired().HasForeignKey<string>((PermissionGroup ap) => ap.GroupId);

            modelBuilder.Entity<Permission>().ToTable("AspNetPermissions")
                .HasMany<PermissionGroup>((Permission g) => g.Groups)
                .WithRequired()
                .HasForeignKey<string>((PermissionGroup ap) => ap.PermissionId);

            modelBuilder.Entity<PermissionGroup>().HasKey((PermissionGroup gr) =>
                new
                {
                    GroupId = gr.GroupId,
                    PermissionId = gr.PermissionId
                }).ToTable("AspNetPermissionGroups");


            modelBuilder.Entity<UserGroup>().HasKey((UserGroup gr) =>
                new
                {
                    GroupId = gr.GroupId,
                    UserName = gr.UserName
                }).ToTable("AspNetUserGroups");

            modelBuilder.Entity<AppConfig>().ToTable("AspNetAppConfig");
            modelBuilder.Entity<IoTClient>().ToTable("AspNetIoTClients");
        }
    }
}