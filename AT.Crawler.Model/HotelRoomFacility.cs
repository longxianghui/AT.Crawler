using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Crawler.Model
{
    public class HotelRoomFacility
    {
        public int Id { get; set; }
        public int RoomFacilityId { get; set; }
        public string HotelId { get; set; }
        public bool IsEnable { get; set; }
        public string Remark { get; set; }
    }
}
