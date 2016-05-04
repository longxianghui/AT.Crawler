using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AT.Crawler.Abot.Crawler;
using AT.Crawler.Abot.Poco;
using AT.Crawler.DataAccess;
using AT.Crawler.Model;
using CsQuery;
using CsQuery.ExtensionMethods;
using CsQuery.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AT.Crawler.Demo
{
    class Program
    {
        private static List<RoomFacility> _allRoomFacilities;
        private static List<BaseFacility> _allBaseFacilities;
        private static List<ServiceFacility> _allServiceFacilities;
        private static HotelService hotelService;
        private static RoomFacilityService roomFacilityService;
        private static HotelRoomFacilityService hotelRoomFacilityService;
        private static ServiceFacilityService serviceFacilityService;
        private static BaseFacilityService baseFacilityService;
        private static HotelServiceFacilityService hotelServiceFacilityService;
        private static HotelBaseFacilityService hotelBaseFacilityService;
        private static TrafficService trafficService;
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            hotelService = new HotelService("Crawler");
            roomFacilityService = new RoomFacilityService("Crawler");
            hotelRoomFacilityService = new HotelRoomFacilityService("Crawler");
            baseFacilityService = new BaseFacilityService("Crawler");
            serviceFacilityService = new ServiceFacilityService("Crawler");
            hotelBaseFacilityService = new HotelBaseFacilityService("Crawler");
            hotelServiceFacilityService = new HotelServiceFacilityService("Crawler");
            trafficService = new TrafficService("Crawler");
            _allRoomFacilities = roomFacilityService.GetAll();
            _allServiceFacilities = serviceFacilityService.GetAll();
            _allBaseFacilities = baseFacilityService.GetAll();
            //var url = "http://hotel.qunar.com/city/beijing_city/dt-21056";
            var urls = new List<string>
            {
                "http://hotel.qunar.com/city/singapore_city/dt-183",
                "http://hotel.qunar.com/city/singapore_city/dt-1104",
                "http://hotel.qunar.com/city/singapore_city/dt-118",
                "http://hotel.qunar.com/city/singapore_city/dt-58",
                "http://hotel.qunar.com/city/singapore_city/dt-1655",
                "http://hotel.qunar.com/city/singapore_city/dt-95",
                "http://hotel.qunar.com/city/singapore_city/dt-1105",
                "http://hotel.qunar.com/city/singapore_city/dt-1088",
                "http://hotel.qunar.com/city/singapore_city/dt-53"
            };
            foreach (var url in urls)
            {
                try
                {

                    var hotelCrawler = new PoliteWebCrawler();
                    hotelCrawler.PageCrawlStartingAsync += hotel_ProcessPageCrawlStarting;
                    hotelCrawler.PageCrawlCompletedAsync += hotel_ProcessPageCrawlCompleted;
                    hotelCrawler.PageCrawlDisallowedAsync += hotel_PageCrawlDisallowed;
                    hotelCrawler.PageLinksCrawlDisallowedAsync += hotel_PageLinksCrawlDisallowed;
                    hotelCrawler.Crawl(new Uri(url));



                }
                catch (Exception ex)
                {
                    string ssss = ex.Message;
                }

            }
        }
        #region hotel
        static void hotel_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }

        static void hotel_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {

            var hotelConfig = ConfigManager.ConfigManager.GetCreateConfig<Hotel>("hotel");
            CrawledPage crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri);
            else
                Console.WriteLine("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri);

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
                Console.WriteLine("Page had no content {0}", crawledPage.Uri.AbsoluteUri);
            var hotelCode = crawledPage.Uri.AbsoluteUri.Split('/').First(x => x.Contains("dt-")).Replace("dt-", "");
            var cityCode = crawledPage.Uri.AbsoluteUri.Split('/').First(x => x.Contains("_city")).Replace("_city", "");
            #region 基础数据
            var hotel = new Hotel();
            hotel.HotelCode = hotelCode;
            hotel.CityCode = cityCode;
            var hotelName = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Name).Text();
            if (hotelName.IndexOf("(", StringComparison.Ordinal) > 0)
            {
                hotel.NameEn = hotelName.Substring(hotelName.IndexOf("(", StringComparison.Ordinal)).Replace("(", "").Replace(")", "");
                hotel.Name = hotelName.Substring(0, hotelName.IndexOf("(", StringComparison.Ordinal)).Replace(")", "");
            }
            else
            {
                hotel.NameEn = hotelName;
                hotel.Name = hotelName;
            }
            hotel.Level = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Level).Text();
            hotel.Address = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Address).Attr("title");
            hotel.Contact = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Contact).Text().Replace("电话", "");
            hotel.Fax = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Fax).Text().Replace("传真", "");
            hotel.OpeningDate = e.CrawledPage.CsQueryDocument.Select(hotelConfig.OpeningDate).Text().Replace("年开业", "");
            hotel.LastDecorationDate =
                e.CrawledPage.CsQueryDocument.Select(hotelConfig.LastDecorationDate).Text().Replace("年最后一次装修", "");
            hotel.HighestFloor =
                e.CrawledPage.CsQueryDocument.Select(hotelConfig.HighestFloor).Text().Replace("层高", "").Replace("层", "");
            hotel.RoomCount =
                e.CrawledPage.CsQueryDocument.Select(hotelConfig.RoomCount).Text().Replace("间客房", "");
            hotel.Description = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Description).Text();
            hotel.Network = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Network).Text().Replace("", "");//图形方框
            hotel.Park = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Park).Text().Replace("", "");
            hotel.Id = hotelService.Insert(hotel);
            #endregion

            var name = string.Empty;
            var isEnable = true;
            var remark = string.Empty;
            #region 房间设施
            var roomFacilities = new List<RoomFacility>();
            var hotelRoomFacilities = new List<HotelRoomFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(5) dd .each-facility").Each((i, n) =>
            {
                GetNameAndRemark(n.InnerHTML, out name, out remark, out isEnable);
                roomFacilities.Add(new RoomFacility { Name = name });
                hotelRoomFacilities.Add(new HotelRoomFacility { HotelId = hotel.Id, Remark = remark, IsEnable = isEnable, RoomFacilityName = name });
            });
            //房间设施基础数据
            //差集 插入数据库
            var names = roomFacilities.Select(x => x.Name).Except(_allRoomFacilities.Select(x => x.Name)).ToList();
            var newRoomFacilities = new List<RoomFacility>();
            //查询出来并保存在内存里面
            if (names.Any())
            {
                foreach (var item in names)
                {
                    int id = roomFacilityService.Insert(new RoomFacility { Name = item });
                    newRoomFacilities.Add(new RoomFacility { Id = id, Name = item });
                }
            }
            _allRoomFacilities.AddRange(newRoomFacilities);
            foreach (var item in hotelRoomFacilities)
            {
                item.RoomFacilityId = _allRoomFacilities.First(x => x.Name == item.RoomFacilityName).Id;
                hotelRoomFacilityService.Insert(item);
            }
            #endregion
            #region 服务设施
            var serviceFacilities = new List<ServiceFacility>();
            var hotelServiceFacilities = new List<HotelServiceFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(6) dd .each-facility").Each((i, n) =>
            {
                GetNameAndRemark(n.InnerHTML, out name, out remark, out isEnable);
                serviceFacilities.Add(new ServiceFacility { Name = name });
                hotelServiceFacilities.Add(new HotelServiceFacility { HotelId = hotel.Id, Remark = remark, IsEnable = isEnable, ServiceFacilityName = name });
            });
            names = serviceFacilities.Select(x => x.Name).Except(_allServiceFacilities.Select(x => x.Name)).ToList();
            var newServiceFacilities = new List<ServiceFacility>();
            //查询出来并保存在内存里面
            if (names.Any())
            {
                foreach (var item in names)
                {
                    int id = serviceFacilityService.Insert(new ServiceFacility { Name = item });
                    newServiceFacilities.Add(new ServiceFacility { Id = id, Name = item });
                }
            }
            _allServiceFacilities.AddRange(newServiceFacilities);
            foreach (var item in hotelServiceFacilities)
            {
                item.ServiceFacilityId = _allServiceFacilities.First(x => x.Name == item.ServiceFacilityName).Id;
                hotelServiceFacilityService.Insert(item);
            }
            #endregion
            #region 基础设施
            var baseFacilities = new List<BaseFacility>();
            var hotelBaseFacilities = new List<HotelBaseFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(7) dd .each-facility").Each((i, n) =>
            {
                GetNameAndRemark(n.InnerHTML, out name, out remark, out isEnable);
                baseFacilities.Add(new BaseFacility { Name = name });
                hotelBaseFacilities.Add(new HotelBaseFacility { HotelId = hotel.Id, Remark = remark, IsEnable = isEnable, BaseFacilityName = name });
            });
            names = baseFacilities.Select(x => x.Name).Except(_allBaseFacilities.Select(x => x.Name)).ToList();
            var newBaseFacilities = new List<BaseFacility>();
            //查询出来并保存在内存里面
            if (names.Any())
            {
                foreach (var item in names)
                {
                    int id = baseFacilityService.Insert(new BaseFacility { Name = item });
                    newBaseFacilities.Add(new BaseFacility { Id = id, Name = item });
                }
            }
            _allBaseFacilities.AddRange(newBaseFacilities);
            foreach (var item in hotelBaseFacilities)
            {
                item.BaseFacilityId = _allBaseFacilities.First(x => x.Name == item.BaseFacilityName).Id;
                hotelBaseFacilityService.Insert(item);
            }
            #endregion

            #region 交通位置
            GetTraffic(hotel.Id, hotel.CityCode, hotel.HotelCode);
            #endregion

            //获取基本数据的urlhttp://hotel.qunar.com/detail/detailMapData.jsp?seq=beijing_city_21056&type=traffic,subway,canguan,jingdian,ent
            /// detail / detailMapData.jsp ? seq = singapore_city_183 & type = traffic,subway,canguan,jingdian,ent

        }

        static void hotel_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            Console.WriteLine("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason);
        }

        static void hotel_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
        }
        #endregion

        static void GetNameAndRemark(string html, out string name, out string remark, out bool isEnable)
        {
            var dom = (CQ)html;
            remark = string.Empty;
            isEnable = true;
            if (html.Contains("提供"))
            {
                if (html.Contains("</b>"))
                {
                    remark = dom.Select("b").Text();
                    name = dom.Document.FirstElementChild.FirstChild.NextSibling.NodeValue.Trim();
                }
                else
                {
                    name = dom.Document.FirstChild.LastChild.NodeValue;
                    if (string.IsNullOrEmpty(name))
                    {
                        name = dom.Select("span").Text().Replace("", "").Replace("", "");
                    }
                }
            }

            else
            {
                isEnable = false;
                name = dom.Select(".gray").Text();
                if (string.IsNullOrEmpty(name))
                {
                    name = dom.Select("span").Text().Replace("", "").Replace("", "");
                }
            }
        }

        static void GetTraffic(int hotelId, string cityCode, string hotelCode)
        {
            var url =
                $"http://hotel.qunar.com/detail/detailMapData.jsp?seq={cityCode}_city_{hotelCode}&type=traffic,subway,canguan,jingdian,ent";
            using (var client = new HttpClient())
            {
                var result = client.GetAsync(url);
                var json = result.Result.Content.ReadAsStringAsync().Result;
                var jo = JsonConvert.DeserializeObject(json) as JObject;
                foreach (var items in jo.Root.Last.Values())
                {
                    var category = ((JProperty)items).Name;
                    if (category != "attrs")
                    {
                        foreach (var item in items.Values())
                        {
                            var traffic = new Traffic();
                            traffic.GPoint = item["gpoint"].ToString().Replace("\r\n", "").Replace("[", "").Replace("]", "").Trim();
                            traffic.BPoint = item["bpoint"].ToString().Replace("\r\n", "").Replace("[", "").Replace("]", "").Trim();
                            traffic.Name = item["name"].ToString();
                            traffic.Distance = item["distance"].ToString();
                            traffic.HotelId = hotelId;
                            traffic.Category = category;
                            trafficService.Insert(traffic);
                        }
                    }
                }
            }
        }
    }
}
