using System.ComponentModel.DataAnnotations;

namespace SSD.Web.Models
{
    public enum DeviceType
    {
        Other = 0,
        Mobile = 1,
        Web = 2,
        WebAPIs = 3,
        WinForm = 4,
        Services = 5
    }
    public class IoTClient
    {
        public IoTClient()
        {
            this.ClientId = System.Guid.NewGuid().ToString("N");
            this.DeviceType = DeviceType.Other;
        }
        public IoTClient(string secret):this() {
            ClientSecret = secret;
        }
        public IoTClient(string secret, string decription) : this(secret)
        {
            Description = decription;
        }
        [Key]
        [StringLength(36)]
        public string ClientId { get; set; }

        [StringLength(100)]
        [Required]
        public string ClientSecret { get; set; }
        [StringLength(100)]
        public string ClientName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public DeviceType DeviceType { get; set; }
        public bool Enable { get; set; }
        public bool Deleted { get; set; }

    }

}