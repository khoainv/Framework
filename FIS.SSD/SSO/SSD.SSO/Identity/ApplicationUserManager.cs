using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Twilio;
using SSD.Framework.Email;
using SSD.Framework.Hashing.Config;
using SSD.SSO.Config;

namespace SSD.SSO.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            string mailServer = "smtp.googlemail.com";
            if (AppConfigBiz.Instance.AppSettings.ContainsKey("EmailConfirmation"))
                mailServer = AppConfigBiz.Instance.AppSettings["EmailConfirmation"];
            SmtpData smtp = null;
            //if (AppConfigBiz.Instance.SmtpData.ContainsKey(mailServer))
            smtp = AppConfigBiz.Instance.SmtpData[mailServer];

            if (smtp != null)
            {
                MailMessage msg = new MailMessage();

                msg.To.Add(message.Destination);
                msg.From = new System.Net.Mail.MailAddress(
                                    smtp.UserName, "UG-Trad.");
                msg.Subject = message.Subject;
                msg.Body = message.Body;
                msg.IsBodyHtml = true;
                await Email.Instance.SendMailAsync(msg, smtp);
            }
            else
            {
                System.Diagnostics.Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }

        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string smsServer = "Twilio";
            if (AppConfigBiz.Instance.AppSettings.ContainsKey("SmsAuthentication"))
                smsServer = AppConfigBiz.Instance.AppSettings["SmsAuthentication"];

            var sms = AppConfigBiz.Instance.SMSServerData[smsServer];
            if (sms != null)
            {
                var Twilio = new TwilioRestClient(sms.SID, sms.Token);
                var result = Twilio.SendMessage(sms.FromPhone, message.Destination, message.Body);
                // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
                System.Diagnostics.Trace.TraceInformation(result.Status);
            }
            else throw new Exception("Config Twilio SMS Service");
            // Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);
        }
    }
    public class CustomPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
                return HashingConfig.HashPassword(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (HashingConfig.VerifyPassword(providedPassword, hashedPassword))
                return PasswordVerificationResult.Success;
            else return PasswordVerificationResult.Failed;
        }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public ApplicationUserStore(ApplicationDbContext ctx)
            : base(ctx)
        {
        }
    }
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public bool IsExist(string userID)
        {
            var u = this.FindByName(userID);
            return u != null;
        }
        public bool ValidateUser(string userID, string password)
        {
            var u = this.Find(userID, password);
            return u != null;
        }
        public ApplicationUserManager(ApplicationDbContext context)
            : base(new UserStore<ApplicationUser>(context))
        {
            PasswordHasher = new CustomPasswordHasher();
            //Using with IdentityServer3
            CustomObjectValue(this);
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(context.Get<ApplicationDbContext>());
            //manager = CustomObjectValue(manager);
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
        private static ApplicationUserManager CustomObjectValue(ApplicationUserManager manager)
        {
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
                //OLD
                //RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };
            //manager.PasswordHasher = new CustomPasswordHasher();
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            return manager;
        }
    }

    //public class UserManagerStore:Singleton<UserManagerStore>
    //{
    //    private static ApplicationDbContext _db;
    //    static ApplicationDbContext DbContext
    //    {
    //        get
    //        {
    //            if (_db == null)
    //                _db = new ApplicationDbContext();
    //            return _db;
    //        }
    //    }
    //    private static ApplicationUserManager userManager;
    //    static ApplicationUserManager UserManager
    //    {
    //        get
    //        {
    //            if (userManager == null)
    //                userManager = new ApplicationUserManager(DbContext);
    //            return userManager;
    //        }
    //    }
    //    private static List<ApplicationUser> lstAppUsers;
    //    public void CleanCacheApplicationUser()
    //    {
    //        lstAppUsers = null;
    //    }
    //    public List<ApplicationUser> ApplicationUsers
    //    {
    //        get
    //        {
    //            if (lstAppUsers == null)
    //            {
    //                ApplicationUserManager manager = new ApplicationUserManager(DbContext);
    //                lstAppUsers = manager.Users.ToList();
    //            }
    //            return lstAppUsers;
    //        }
    //    }

    //    public ApplicationUser Find(string UserId)
    //    {
    //        var user = from u in ApplicationUsers where u.UserName == UserId select u;
    //        if (user.Count() > 0)
    //            return user.First();
    //        return null;
    //    }
    //    public ApplicationUser Find(UserAuthen user)
    //    {
    //        var userDB = UserManager.Find(user.UserId, user.Password);
    //        return userDB;
    //    }
    //    public IList<string> GetRoles(string userID)
    //    {
    //        return UserManager.GetRoles(userID);
    //    }
    //    public IList<ApplicationPermission> GetPermissions(string userID)
    //    {
    //        ApplicationPermissionManager perManager = new ApplicationPermissionManager(DbContext);
    //        var lstP = perManager.GetUserPermissions(userID);
    //        return lstP.ToList();
    //    }
    //}
}
