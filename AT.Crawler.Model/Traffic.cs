using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT.Crawler.Model
{
    public class Traffic
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string GPoint { get; set; }
        public string BPoint { get; set; }
        public string Category { get; set; }
        public string Distance { get; set; }
    }
}
