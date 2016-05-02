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
    public class RoomFacilityService : BaseDao
    {
        public int Insert(RoomFacility model)
        {
            var stmtId = "RoomFacility.Insert";
            return (int)Dao.Insert(stmtId, model);
        }
        public List<RoomFacility> GetAll()
        {
            return Dao.QueryForList<RoomFacility>("RoomFacility.GetAll", null).ToList();
        }
        public RoomFacilityService(string dbContextID) : base(dbContextID)
        {
        }

        public RoomFacilityService(IDataAccess dao) : base(dao)
        {
        }
    }
}
