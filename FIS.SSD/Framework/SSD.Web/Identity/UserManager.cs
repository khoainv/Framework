using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Twilio;
using SSD.Framework;
using SSD.Framework.Hashing;
using SSD.Web.Caching;
using SSD.Web.Security;
using SSD.Web.Models;
using System.Web;
using SSD.Framework.Email;
using SSD.Framework.Hashing.Config;

namespace SSD.Web.Identity
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
            var appBiz = HttpContext.Current.GetOwinContext().Get<AppConfigManager>();
            if (appBiz != null)
            {
                string mailServer = "smtp.googlemail.com";
                if (appBiz.AppSettings.ContainsKey("EmailConfirmation"))
                    mailServer = appBiz.AppSettings["EmailConfirmation"];
                SmtpData smtp = null;
                //if (AppConfigBiz.Instance.SmtpData.ContainsKey(mailServer))
                smtp = appBiz.SmtpData[mailServer];

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
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var appBiz = HttpContext.Current.GetOwinContext().Get<AppConfigManager>();
            if (appBiz != null)
            {
                string smsServer = "Twilio";
                if (appBiz.AppSettings.ContainsKey("SmsAuthentication"))
                    smsServer = appBiz.AppSettings["SmsAuthentication"];

                var sms = appBiz.SMSServerData[smsServer];

                if (sms != null)
                {
                    var Twilio = new TwilioRestClient(sms.SID, sms.Token);
                    var result = Twilio.SendMessage(sms.FromPhone, message.Destination, message.Body);

                    // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
                    System.Diagnostics.Trace.TraceInformation(result.Status);
                }
                // Twilio doesn't currently have an async API, so return success.
            }
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
    public interface IUserManager
    {
        // Property declaration:
        IQueryable<User> UGUsers
        {
            get;
        }

        bool IsExist(string userName);
    }
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class UserManager : UserManager<User>, IUserManager
    {
        public UserManager(IdentityDbContext context)
            : base(new UserStore<User>(context))
        {
            PasswordHasher = new CustomPasswordHasher();
            _dbContext = context;
        }

        public IQueryable<User> UGUsers
        {
            get
            {
                return Users;
            }
        }
        private IdentityDbContext _dbContext;
        public IdentityDbContext Context
        {
            get
            {
                return _dbContext;
            }
        }
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
        
        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var manager = new UserManager(context.Get<IdentityDbContext>());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
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
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
