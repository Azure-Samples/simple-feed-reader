using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;

namespace SimpleFeedReader.Models
{
    interface INewsItemRepository
    {
        List<ISyndicationItem> GetNewsItems(Uri feedUrl);
    }
}
