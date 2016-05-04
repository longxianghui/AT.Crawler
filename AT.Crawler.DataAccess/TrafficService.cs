using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.Crawler.Model;
using AT.Library.DataAccess;

namespace AT.Crawler.DataAccess
{
    public class TrafficService: BaseDao
    {
        public int Insert(Traffic model)
        {
            var stmtId = "Traffic.Insert";
            return (int)Dao.Insert(stmtId, model);
        }

        public TrafficService(string dbContextID) : base(dbContextID)
        {
        }

        public TrafficService(IDataAccess dao) : base(dao)
        {
        }
    }
}
