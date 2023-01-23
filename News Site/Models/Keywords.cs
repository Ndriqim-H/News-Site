using System.ComponentModel.DataAnnotations;

namespace News_Site.Models
{
    public class Keywords
    {
        
        [Display(Name = "Fjalët kyçe")]
        public string KeywordsStr { get; set; }
    }
}
