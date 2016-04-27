using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private static readonly List<RoomFacility> _roomFacilities = new List<RoomFacility>();
        private List<ServiceFacility> _serviceFacilities = new List<ServiceFacility>();
        private List<BaseFacility> _baseFacilities = new List<BaseFacility>();
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
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
            //Process data
            var hotel = new Hotel();
            hotel.HotelId = crawledPage.Uri.AbsoluteUri
                .Split('/')
                .First(x => x.Contains("dt-"))
                .Replace("dt-", "");
            hotel.Name = e.CrawledPage.CsQueryDocument.Select(hotelConfig.Name).Text();
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
            var roomFacility = new List<RoomFacility>();
            var hotelRoomFacility = new List<HotelRoomFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(5) dd .each-facility").Each((i, n) =>
            {
                if (string.IsNullOrEmpty(n.FirstElementChild.LastChild.NodeValue))
                {
                    roomFacility.Add(new RoomFacility { Name = n.FirstElementChild.LastChild.FirstChild.NodeValue });
                    hotelRoomFacility.Add(new HotelRoomFacility { HotelId = hotel.HotelId, IsEnable = false, });
                }
                else
                {
                    roomFacility.Add(new RoomFacility { Name = n.FirstElementChild.LastChild.NodeValue });
                }
            });
            //差集 插入数据库
            var newRoomFacilities = _roomFacilities.Except(roomFacility);
            //查询出来并保存在内存里面
            //todo
            _roomFacilities.AddRange(newRoomFacilities);

            var serviceFacility = new List<ServiceFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(6) dd .each-facility").Each((i, n) =>
            {
                if (string.IsNullOrEmpty(n.FirstElementChild.LastChild.NodeValue))
                {
                    serviceFacility.Add(new ServiceFacility { Name = n.FirstElementChild.LastChild.FirstChild.NodeValue });
                }
                else
                {
                    serviceFacility.Add(new ServiceFacility { Name = n.FirstElementChild.LastChild.NodeValue });
                }
            });
            var baseFacility = new List<BaseFacility>();
            e.CrawledPage.CsQueryDocument.Select("#descContent dl:eq(7) dd .each-facility").Each((i, n) =>
            {
                if (string.IsNullOrEmpty(n.FirstElementChild.LastChild.NodeValue))
                {
                    baseFacility.Add(new BaseFacility { Name = n.FirstElementChild.LastChild.FirstChild.NodeValue });
                }
                else
                {
                    baseFacility.Add(new BaseFacility { Name = n.FirstElementChild.LastChild.NodeValue });
                }
            });
            var hotelService = new HotelService("Crawler");
            hotelService.Insert(hotel);
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
