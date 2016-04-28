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
    public class HotelService : BaseDao
    {
        public int Insert(Hotel model)
        {
            var stmtId = "Hotel.Insert";
            return (int)Dao.Insert(stmtId, model);
        }

        public HotelService(string dbContextID) : base(dbContextID)
        {
        }

        public HotelService(IDataAccess dao) : base(dao)
        {
        }
    }
}
