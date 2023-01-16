using System.Text.RegularExpressions;

namespace News_Site.Models
{
    public class News
    {
        /*
         id,
        title,
        url,
        category,
        main_photo,
        content,
        video,
        keywords,
        comments,
        posted_at,
        author_name,
        author_photo
         */
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Category { get; set; }
        public string Main_photo { get; set; }
        public string Content { get; set; }
        public string Video { get; set; }
        public string KeyWords { get; set; }
        public string Comments { get; set; }
        public string Posted_at { get; set; }
        public string Author { get; set; }
        public string Author_photo { get; set; }
        //public double Rank { get; set; }


        public static News FromCSV(string csvLine)
        {
            string[] values = Regex.Split(csvLine, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            News news = new News();
            news.Id = Int64.Parse(values[0]);
            news.Title = values[1];
            news.URL = values[2];
            news.Content = values[3];
            news.Main_photo = values[4];
            //news.Posted_at = Convert.ToDateTime(values[9]);
            news.Author = values[10];
            return news;
        }
    }
}
