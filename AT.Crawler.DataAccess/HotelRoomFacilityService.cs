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
    public class HotelRoomFacilityService : BaseDao
    {
        public int Insert(HotelRoomFacility model)
        {
            var stmtId = "HotelRoomFacility.Insert";
            return (int)Dao.Insert(stmtId, model);
        }
      
        public HotelRoomFacilityService(string dbContextID) : base(dbContextID)
        {
        }

        public HotelRoomFacilityService(IDataAccess dao) : base(dao)
        {
        }
    }
}
