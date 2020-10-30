using System.Data.Entity;
using SSD.Framework.Collections;

namespace SSD.SSO.Config
{
    public interface IAppConfigStore
    {
        SortableBindingList<AppConfig> AppConfigs { get; }
        void SetDbContext(DbContext context);
        int InsertConfig(AppConfig per);
        void UpdateConfig(AppConfig per);
        void DeleteConfig(AppConfig per);
        AppConfig ReadObjectByIDConfig(int id);
    }
}