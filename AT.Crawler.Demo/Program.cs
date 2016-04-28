using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AT.Crawler.Abot.Crawler;
using AT.Crawler.Abot.Poco;
using AT.Crawler.DataAccess;
using AT.Crawler.Model;
using CsQuery.ExtensionMethods;

namespace AT.Crawler.Demo
{
    class Program
    {
        private static List<RoomFacility> _allRoomFacilities;
        private static List<BaseFacility> _allBaseFacilities;
        private static List<ServiceFacility> _allServiceFacilities;
        private List<ServiceFacility> _serviceFacilities = new List<ServiceFacility>();
        private List<BaseFacility> _baseFacilities = new List<BaseFacility>();
        private static HotelService hotelService;
        private static RoomFacilityService roomFacilityService;
        private static HotelRoomFacilityService hotelRoomFacilityService;
        private static ServiceFacilityService serviceFacilityService;
        private static BaseFacilityService baseFacilityService;
        private static HotelServiceFacilityService hotelServiceFacilityService;
        private static HotelBaseFacilityService hotelBaseFacilityService;
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
            _allRoomFacilities = roomFacilityService.GetAll();
            _allServiceFacilities = serviceFacilityService.GetAll();
            _allBaseFacilities = baseFacilityService.GetAll();
            var url = "http://hotel.qunar.com/city/beijing_city/dt-21056";
            var crawler = new PoliteWebCrawler();
            //Register for events and create processing methods(both synchronous and asynchronous versions available)
            crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
            crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;
            //for (int i = 0; i < 1; i++)
            //{
            //    int n = 21056;
            //    var result = crawler.Crawl(new Uri(url + (n + i)));
            //}
            var result = crawler.Crawl(new Uri(url));
            Console.ReadLine();
        }
        static void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }

        static void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {

            var hotelConfig = ConfigManager.ConfigManager.GetCreateConfig<Hotel>("hotel");
            CrawledPage crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri);
            else
                Console.WriteLine("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri);

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
                Console.WriteLine("Page had no content {0}", crawledPage.Uri.AbsoluteUri);
            #region 基础数据
            var hotel = new Hotel();
            hotel.HotelCode = crawledPage.Uri.AbsoluteUri
                .Split('/')
                .First(x => x.Contains("dt-"))
                .Replace("dt-", "");
            var hotelName = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Name).Text();
            if (hotelName.IndexOf("(", StringComparison.Ordinal) > 0)
            {
                hotel.NameEn = hotelName.Substring(hotelName.IndexOf("(", StringComparison.Ordinal), hotelName.IndexOf(")", StringComparison.Ordinal));
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
                e.CrawledPage.CsQueryDocument.Select(hotelConfig.HighestFloor).Text().Replace("间客房", "");
            hotel.Description = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Description).Text();
            hotel.Network = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Network).Text();
            hotel.Id = hotelService.Insert(hotel);
            #endregion
            #region 房间设施
            var roomFacilities = new List<RoomFacility>();
            var hotelRoomFacilities = new List<HotelRoomFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(5) dd .each-facility").Each((i, n) =>
            {
                if (string.IsNullOrEmpty(n.FirstElementChild.LastChild.NodeValue))
                {
                    roomFacilities.Add(new RoomFacility { Name = n.FirstElementChild.LastChild.FirstChild.NodeValue });
                    hotelRoomFacilities.Add(new HotelRoomFacility { HotelId = hotel.Id, IsEnable = false, RoomFacilityName = n.FirstElementChild.LastChild.FirstChild.NodeValue });
                }
                else
                {
                    roomFacilities.Add(new RoomFacility { Name = n.FirstElementChild.LastChild.NodeValue });
                    hotelRoomFacilities.Add(new HotelRoomFacility { HotelId = hotel.Id, IsEnable = true, RoomFacilityName = n.FirstElementChild.LastChild.NodeValue });
                }
            });
            //房间设施基础数据
            //差集 插入数据库
            var newRoomFacilities = _allRoomFacilities.Except(roomFacilities).ToList();
            //查询出来并保存在内存里面
            if (newRoomFacilities.Any())
            {
                foreach (var item in newRoomFacilities)
                {
                    int id = roomFacilityService.Insert(item);
                    item.Id = id;
                }
            }
            _allRoomFacilities.AddRange(newRoomFacilities);
            foreach (var item in hotelRoomFacilities)
            {
                item.RoomFacilityId = newRoomFacilities.First(x => x.Name == item.RoomFacilityName).Id;
                hotelRoomFacilityService.Insert(item);
            }
            #endregion
            #region 服务设施
            var serviceFacilities = new List<ServiceFacility>();
            var hotelServiceFacilities = new List<HotelServiceFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(6) dd .each-facility").Each((i, n) =>
            {
                if (string.IsNullOrEmpty(n.FirstElementChild.LastChild.NodeValue))
                {
                    serviceFacilities.Add(new ServiceFacility { Name = n.FirstElementChild.LastChild.FirstChild.NodeValue });
                    hotelServiceFacilities.Add(new HotelServiceFacility { HotelId = hotel.Id, IsEnable = true, ServiceFacilityName = n.FirstElementChild.LastChild.FirstChild.NodeValue });
                }
                else
                {
                    serviceFacilities.Add(new ServiceFacility { Name = n.FirstElementChild.LastChild.NodeValue });
                    hotelServiceFacilities.Add(new HotelServiceFacility { HotelId = hotel.Id, IsEnable = true, ServiceFacilityName = n.FirstElementChild.LastChild.NodeValue });

                }
            });
            var newServiceFacilities = _allServiceFacilities.Except(serviceFacilities).ToList();
            //查询出来并保存在内存里面
            if (newServiceFacilities.Any())
            {
                foreach (var item in newServiceFacilities)
                {
                    int id = serviceFacilityService.Insert(item);
                    item.Id = id;
                }
            }
            _allServiceFacilities.AddRange(newServiceFacilities);
            foreach (var item in hotelServiceFacilities)
            {
                item.ServiceFacilityId = newServiceFacilities.First(x => x.Name == item.ServiceFacilityName).Id;
                hotelServiceFacilityService.Insert(item);
            }
            #endregion
            #region 基础设施
            var baseFacilities = new List<BaseFacility>();
            var hotelBaseFacilities =new List<HotelBaseFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(7) dd .each-facility").Each((i, n) =>
            {
                if (string.IsNullOrEmpty(n.FirstElementChild.LastChild.NodeValue))
                {
                    baseFacilities.Add(new BaseFacility { Name = n.FirstElementChild.LastChild.FirstChild.NodeValue });
                }
                else
                {
                    baseFacilities.Add(new BaseFacility { Name = n.FirstElementChild.LastChild.NodeValue });
                }
            });
            var newBaseFacilities = _allRoomFacilities.Except(roomFacilities).ToList();
            //查询出来并保存在内存里面
            if (newRoomFacilities.Any())
            {
                foreach (var item in newRoomFacilities)
                {
                    int id = roomFacilityService.Insert(item);
                    item.Id = id;
                }
            }
            _allRoomFacilities.AddRange(newBaseFacilities);
            foreach (var item in hotelBaseFacilities)
            {
                item.BaseFacilityId = newRoomFacilities.First(x => x.Name == item.BaseFacilityName).Id;
                hotelBaseFacilityService.Insert(item);
            }
            #endregion
            //获取基本数据的urlhttp://hotel.qunar.com/detail/detailMapData.jsp?seq=beijing_city_21056&type=traffic,subway,canguan,jingdian,ent
            /// detail / detailMapData.jsp ? seq = singapore_city_183 & type = traffic,subway,canguan,jingdian,ent

        }

        static void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            Console.WriteLine("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason);
        }

        static void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
        }
    }
}
