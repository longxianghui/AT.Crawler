﻿using AT.Crawler.Abot.Poco;
using System;

namespace AT.Crawler.Abot.Crawler
{
    [Serializable]
    public class PageCrawlCompletedArgs : CrawlArgs
    {
        public CrawledPage CrawledPage { get; private set; }

        public PageCrawlCompletedArgs(CrawlContext crawlContext, CrawledPage crawledPage)
            : base(crawlContext)
        {
            if (crawledPage == null)
                throw new ArgumentNullException("crawledPage");

            CrawledPage = crawledPage;
        }
    }
}
