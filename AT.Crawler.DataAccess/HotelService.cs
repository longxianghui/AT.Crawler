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
    public class HotelService:BaseDao
    {
        public void Insert(Hotel model)
        {
            var stmtId = "Hotel.Insert";
             Dao.Insert(stmtId, model);
        }

        public HotelService(string dbContextID) : base(dbContextID)
        {
        }

        public HotelService(IDataAccess dao) : base(dao)
        {
        }
    }
}
