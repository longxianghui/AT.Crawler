using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.Crawler.Model;
using AT.Library.DataAccess;
using IBatisNet.DataAccess.Configuration;

namespace AT.Crawler.DataAccess
{
    public class ServiceFacilityService : BaseDao
    {
        public int Insert(ServiceFacility model)
        {
            var stmtId = "ServiceFacility.Insert";
            return (int)Dao.Insert(stmtId, model);
        }
        public List<ServiceFacility> GetAll()
        {
            return Dao.QueryForList<ServiceFacility>("ServiceFacility.GetAll", null).ToList();
        }
        public ServiceFacilityService(string dbContextID) : base(dbContextID)
        {
        }

        public ServiceFacilityService(IDataAccess dao) : base(dao)
        {
        }
    }
}
