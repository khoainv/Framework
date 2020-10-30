using System.Data.Entity;
using Microsoft.AspNet.Identity;
using SSD.Web.Models;
using SSD.Web.Extensions;
using System;
using SSD.Framework;

namespace SSD.Web.SSO
{

    // This is useful if you do not want to tear down the database each time you run the application.
    //When Testing
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    //When Live
    // public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    //When Dev
    // public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class SSODbInitializer : CreateDatabaseIfNotExists<SSODbContext>
    {
        protected override void Seed(SSODbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(SSODbContext db)
        {
            var grpManager = new SSOGroupManager(db);
            string grpName = UGConstants.GroupAdmin;

            //Create Role Admin if it does not exist
            var grp = grpManager.FindByName(grpName);
            if (grp == null)
            {
                grp = new Group(grpName);
                var roleresult = grpManager.Create(grp);
            }

            //Add Anonymous Group
            var anonymous = grpManager.FindByName(UGConstants.GroupAnonymous);
            if (anonymous == null)
            {
                anonymous = new Group(UGConstants.GroupAnonymous);
                var result = grpManager.Create(anonymous);
            }

            string userName = System.Web.HttpContext.Current.User.GetUserName();
            if (string.IsNullOrWhiteSpace(userName))
                userName = UGConstants.AccountAdmin;// "admin@7i.com.vn";

            var ugrp = grpManager.FindUserGroupById(userName, grp.Id);
            if (ugrp == null)
            {
                var result = grpManager.AddUserToGroup(userName, grp.Id);
            }
        }
    }
  
    public class SSODbContext : DbContext
    {
        public SSODbContext()
            : base(UGConstants.ConnectionStringNameIdentity)
        {
            //Configuration.LazyLoadingEnabled = false;
            //Configuration.ProxyCreationEnabled = false;
        }
        public SSODbContext(string connectionStrName)
            : base(connectionStrName)
        {
            //Configuration.LazyLoadingEnabled = false;
            //Configuration.ProxyCreationEnabled = false;
        }
        static SSODbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<SSODbContext>(new SSODbInitializer());
        }
        public static SSODbContext Create()
        {
            return new SSODbContext();
        }

        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Make sure to call the base method first:
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Group>().ToTable("UGGroups").HasMany<PermissionGroup>((Group g) => g.Permissions)
                .WithRequired().HasForeignKey<string>((PermissionGroup ap) => ap.GroupId);

            modelBuilder.Entity<Permission>().ToTable("UGPermissions")
                .HasMany<PermissionGroup>((Permission g) => g.Groups)
                .WithRequired()
                .HasForeignKey<string>((PermissionGroup ap) => ap.PermissionId);

            modelBuilder.Entity<PermissionGroup>().HasKey((PermissionGroup gr) =>
                new
                {
                    GroupId = gr.GroupId,
                    PermissionId = gr.PermissionId
                }).ToTable("UGPermissionGroups");


            modelBuilder.Entity<UserGroup>().HasKey((UserGroup gr) =>
                new
                {
                    GroupId = gr.GroupId,
                    UserName = gr.UserName
                }).ToTable("UGUserGroups");

            modelBuilder.Entity<AppConfig>().ToTable("UGAppConfig");
            modelBuilder.Entity<IoTClient>().ToTable("UGIoTClients");
        }

        public System.Data.Entity.DbSet<SSD.Web.Models.IoTClient> IoTClients { get; set; }
    }
}