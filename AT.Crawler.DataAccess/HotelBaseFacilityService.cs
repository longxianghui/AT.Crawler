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
    public class HotelBaseFacilityService : BaseDao
    {
        public int Insert(HotelBaseFacility model)
        {
            var stmtId = "HotelBaseFacility.Insert";
            return (int)Dao.Insert(stmtId, model);
        }

        public HotelBaseFacilityService(string dbContextID) : base(dbContextID)
        {
        }

        public HotelBaseFacilityService(IDataAccess dao) : base(dao)
        {
        }
    }
}
