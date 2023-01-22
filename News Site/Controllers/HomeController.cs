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
using News_Site.Data;
using Microsoft.EntityFrameworkCore;

namespace News_Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsSiteContext _context;


        public HomeController(ILogger<HomeController> logger, NewsSiteContext context)
        {
            _logger = logger;
            _context = context;
        }
        

        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles.ToListAsync();
            List<string> keywords = await _context.TargetKeywords.Select(t=>t.Name).ToListAsync();
            
            List<News> news = new();
            using (StreamReader r = new StreamReader("Kallxo.json"))
            {


                string json = r.ReadToEnd();
                news = JsonSerializer.Deserialize<List<News>>(json);
            }
            var x = DateTime.Parse(news[0].Posted_at);

            List<VM> vm = articles.Select(t => new VM
            {
                Created = t.PostedAt.Value,
                Title = $"[{t.Website}] {t.Title}",
                Main_photo = t.MainPhoto,
                Rank = t.Rank.Value,
                URL = t.Url,
            }).ToList();
            //List<News> news = System.IO.File.ReadAllLines("Kallxo.csv")
            //.Skip(1)
            //.Select(News.FromCSV)
            //.ToList();

            //for (int i = 0; i < news.Count; i++)
            //{
            //    MatchCollection matches = Regex.Matches(news[i].Content, @"\b[\w'\.]*\b");
            //    vm[i].Rank += (matches.Where(match => keywords.Contains(match.Value))).Count();
            //    vm[i].Rank = (vm[i].Rank / matches.Count) * Math.Exp(-1 * ((DateTime.Now - vm[i].Created).TotalDays));
            //}
            vm.ForEach(t => t.Rank *= Math.Exp(-1 * (DateTime.Now - t.Created).TotalDays));
            //
            //news.ForEach(t=>t.Rank = )
            vm = vm.OrderByDescending(t => t.Rank).ToList();
            return View(vm);
        }


        public IActionResult _LoginPartial1()
        {
            return PartialView();
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