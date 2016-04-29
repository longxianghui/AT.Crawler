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
        public int HotelId { get; set; }
        public bool IsEnable { get; set; }
        public string Remark { get; set; }
        public string RoomFacilityName { get; set; }
    }
}
