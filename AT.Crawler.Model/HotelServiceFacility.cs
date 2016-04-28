using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Crawler.Model
{
    public class HotelServiceFacility
    {
        public int Id { get; set; }
        public int ServiceFacilityId { get; set; }
        public int HotelId { get; set; }
        public bool IsEnable { get; set; }
        public string Remark { get; set; }
        public DateTime AddTime { get; set; }
        public string ServiceFacilityName { get; set; }
    }
}
