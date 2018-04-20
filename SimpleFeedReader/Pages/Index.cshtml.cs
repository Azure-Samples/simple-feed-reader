using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SyndicationFeed;
using SimpleFeedReader.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleFeedReader.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NewsItemRepository _newsItemRepository;

        public IndexModel(NewsItemRepository newsItemRepository)
        {
            _newsItemRepository = newsItemRepository;
        }

        public string ErrorText { get; set; }

        public List<ISyndicationItem> NewsItems { get; set; }

        public async Task OnGet()
        {
            string urlString = Request.Query["feedurl"];
            Uri feedUrl = null;

            if (!string.IsNullOrEmpty(urlString))
            {
                try
                {
                    feedUrl = new Uri(urlString);
                    NewsItems = await _newsItemRepository.GetNewsItems(feedUrl);
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
