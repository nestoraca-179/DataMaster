using System.Data.Entity.Core.EntityClient;

namespace DataMaster.Controllers
{
    public class EntityController
    {
        public static EntityConnectionStringBuilder GetEntity(string conn)
        {
            EntityConnectionStringBuilder entity = new EntityConnectionStringBuilder();

            entity.Provider = "System.Data.SqlClient";
            entity.ProviderConnectionString = conn + ";MultipleActiveResultSets=True;App=EntityFramework;";
            entity.Metadata = @"res://*/Models.ProfitAdm.csdl|res://*/Models.ProfitAdm.ssdl|res://*/Models.ProfitAdm.msl";

            return entity;
        }
    }
}