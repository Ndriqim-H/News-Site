using Microsoft.AspNetCore.Mvc;
using News_Site.Models;
using System.IO;
using System.Text.RegularExpressions;
using CsvHelper;
using System.Globalization;
using System.Diagnostics;
using Elfie.Serialization;
using System;
using System.Text.Json;
using System.Linq;

namespace News_Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            string[] keyWords = { "siguri", "telefon" , "teknologji" };
            List<News> news = new();
            using (StreamReader r = new StreamReader("Kallxo.json"))
            {

                
                string json = r.ReadToEnd();
                news = JsonSerializer.Deserialize<List<News>>(json);
            }
            var x = DateTime.Parse(news[0].Posted_at);

            List<VM> vm = news.Select(t => new VM
            {
                Created = DateTime.ParseExact(t.Posted_at, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                Title = t.Title,
                Main_photo = t.Main_photo,
                URL = t.URL,
            }).ToList();
            //List<News> news = System.IO.File.ReadAllLines("Kallxo.csv")
            //.Skip(1)
            //.Select(News.FromCSV)
            //.ToList();

            for (int i = 0; i < news.Count; i++)
            {
                MatchCollection matches = Regex.Matches(news[i].Content, @"\b[\w'\.]*\b");
                vm[i].Rank += (matches.Where(match => keyWords.Contains(match.Value))).Count();
                vm[i].Rank = (vm[i].Rank / matches.Count) * Math.Exp(-1 * ((DateTime.Now - vm[i].Created).TotalDays));
            }
            //
            //news.ForEach(t=>t.Rank = )
            vm = vm.OrderByDescending(t => t.Rank).ToList();
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}