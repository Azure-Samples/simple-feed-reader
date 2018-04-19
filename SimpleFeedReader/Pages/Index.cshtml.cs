using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SyndicationFeed;
using SimpleFeedReader.Models;

namespace SimpleFeedReader.Pages
{
    public class IndexModel : PageModel
    {
        NewsItemRepository _repository = new NewsItemRepository();
        public string ErrorText { get; set; }
        public List<ISyndicationItem> NewsItems { get; set; }
        public void OnGet()
        {
            string urlString = Request.Query["feedurl"];
            Uri feedUrl = null;

            if (!string.IsNullOrEmpty(urlString))
            {
                try
                {
                    feedUrl = new Uri(urlString);
                    NewsItems = _repository.GetNewsItems(feedUrl);
                }
                catch(UriFormatException)
                {
                    ErrorText = "There was a problem parsing the URL.";
                    return;
                }
                catch(AggregateException ae)
                {
                    ae.Handle((x) =>
                    {
                        if (x is XmlException)
                        {
                            ErrorText = "There was a problem parsing the feed. Are you sure that URL is a syndication feed?";
                            return true;
                        }
                        return false;
                    });
                }
            }

           
            
        }
    }
}
