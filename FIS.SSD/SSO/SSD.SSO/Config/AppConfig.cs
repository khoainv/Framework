using System.ComponentModel.DataAnnotations;

namespace SSD.SSO.Config
{
    public class AppConfig
    {
        public const string Table_AppConfig = "AppConfig";
        #region Column Names

        public const string FIELD_ID = "ID";
        public const string FIELD_ConfigKey = "ConfigKey";
        public const string FIELD_ConfigData = "ConfigData";
        public const string FIELD_DataType = "DataType";
        public const string FIELD_IsEncryption = "IsEncryption";
        public const string FIELD_ApplicationID = "ApplicationID";
        #endregion


        #region Members
        protected int iD;
        protected string configKey;
        protected string configData;
        protected string dataType;
        protected bool isEncryption;
        protected int applicationID;
        #endregion
        #region Public Properties

        /// <summary>
        /// Property relating to database column ID(int,not null)
        /// </summary>
        [Key]
        [Required]
        [Display(Name = "ID")]
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        /// <summary>
        /// Property relating to database column ConfigKey(nvarchar(50),not null)
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "ConfigKey")]
        public string ConfigKey
        {
            get { return configKey; }
            set { configKey = value; }
        }

        /// <summary>
        /// Property relating to database column ConfigData(nvarchar(MAX),not null)
        /// </summary>
        [Required]
        [StringLength(1073741823)]
        [Display(Name = "ConfigData")]
        public string ConfigData
        {
            get { return configData; }
            set { configData = value; }
        }

        /// <summary>
        /// Property relating to database column DataType(nvarchar(500),null)
        /// </summary>
        [StringLength(500)]
        [Display(Name = "DataType")]
        public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        /// <summary>
        /// Property relating to database column IsEncryption(bit,not null)
        /// </summary>
        [Required]
        [Display(Name = "IsEncryption")]
        public bool IsEncryption
        {
            get { return isEncryption; }
            set { isEncryption = value; }
        }

        /// <summary>
        /// Property relating to database column ApplicationID(int,null)
        /// </summary>
        [Display(Name = "ApplicationID")]
        public int ApplicationID
        {
            get { return applicationID; }
            set { applicationID = value; }
        }
        #endregion
    }
}
