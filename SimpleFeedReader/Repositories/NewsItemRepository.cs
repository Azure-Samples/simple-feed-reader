using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleFeedReader.Repositories
{
    public class NewsItemRepository
    {
        public async Task<List<ISyndicationItem>> GetNewsItems(Uri feedUrl)
        {
            var returnList = new List<ISyndicationItem>();

            using (var xmlReader = XmlReader.Create(feedUrl.ToString(), 
                   new XmlReaderSettings { Async = true }))
            {
                try
                {
                    var feedReader = new RssFeedReader(xmlReader);

                    while (await feedReader.Read())
                    {
                        switch (feedReader.ElementType)
                        {
                            // RSS Item
                            case SyndicationElementType.Item:
                                ISyndicationItem item = await feedReader.ReadItem();
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