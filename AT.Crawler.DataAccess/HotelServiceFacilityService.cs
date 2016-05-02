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
    public class HotelServiceFacilityService : BaseDao
    {
        public int Insert(HotelServiceFacility model)
        {
            var stmtId = "HotelServiceFacility.Insert";
            return (int)Dao.Insert(stmtId, model);
        }
        
        public HotelServiceFacilityService(string dbContextID) : base(dbContextID)
        {
        }

        public HotelServiceFacilityService(IDataAccess dao) : base(dao)
        {
        }
    }
}

