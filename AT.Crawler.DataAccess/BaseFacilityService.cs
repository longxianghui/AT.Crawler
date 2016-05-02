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
    public class BaseFacilityService : BaseDao
    {
        public int Insert(BaseFacility model)
        {
            var stmtId = "BaseFacility.Insert";
            return (int)Dao.Insert(stmtId, model);
        }

        public List<BaseFacility> GetAll()
        {
            return Dao.QueryForList<BaseFacility>("BaseFacility.GetAll", null).ToList();
        } 

        public BaseFacilityService(string dbContextID) : base(dbContextID)
        {
        }

        public BaseFacilityService(IDataAccess dao) : base(dao)
        {
        }
    }
}
