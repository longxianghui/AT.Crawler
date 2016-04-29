using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Crawler.Model
{
    public class Hotel
    {
        public int Id { get; set; }
        public string HotelCode { get ; set ; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Address { get; set; }
        public string NameEn { get; set; }
        public string Contact { get; set; }
        public string Fax { get; set; }
        public string Information { get; set; }
        public string Description { get; set; }
        public string OpeningDate { get; set; }
        public string LastDecorationDate { get; set; }
        public string RoomCount { get; set; }
        public string HighestFloor { get; set; }
        /// <summary>
        /// 停车场信息
        /// </summary>
        public string Park { get; set; }
        public string Network { get; set; }
        public string RoomInfo { get; set; }
    }

}
