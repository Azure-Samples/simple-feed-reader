using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Xml;

namespace SimpleFeedReader.Models
{
    public class NewsItemRepository : INewsItemRepository
    {
        public List<ISyndicationItem> GetNewsItems(Uri feedUrl)
        {
            var returnList = new List<ISyndicationItem>();

            using (var xmlReader = XmlReader.Create(feedUrl.ToString(), new XmlReaderSettings() { Async = true }))
            {
                try
                {
                    var feedReader = new RssFeedReader(xmlReader);
                    while (feedReader.Read().Result)
                    {
                        switch (feedReader.ElementType)
                        {
                            // RSS Item
                            case SyndicationElementType.Item:
                                ISyndicationItem item = feedReader.ReadItem().Result;
                                returnList.Add(item);
                                break;

                            // Something else
                            default:
                                break;
                        }
                    }
                }
                catch(AggregateException ae)
                {
                    throw ae.Flatten();
                }
            }
            return returnList;
        }
    }
}
