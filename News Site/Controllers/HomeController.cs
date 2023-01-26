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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace News_Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsSiteContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, NewsSiteContext context, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles.Where(t=>t.Rank>0).ToListAsync();
            
            List<VM> vm = articles.Select(t => new VM
            {
                Created = t.PostedAt.Value,
                Title = $"[{t.Website}] {t.Title}",
                Main_photo = t.MainPhoto,
                Rank = t.Rank.Value,
                URL = t.Url,
            }).ToList();
            
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


        public async Task<IActionResult> Login(string Username, string Password)
        {
            JsonResult json = new JsonResult(new { success = false, message = "Invalid login attempt." });
            var result = await _signInManager.PasswordSignInAsync(Username, Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                json = new JsonResult(new { success = true, message = "Invalid login attempt." });

                _logger.LogInformation("User logged in.");
                return json;
            }
            else
                return json;
        }

        public async Task<IActionResult> ConfigKeywords()
        {
            var keywords = await _context.TargetKeywords.Select(t => t.Name).ToListAsync();
            Keywords model = new()
            {
                KeywordsStr = string.Join(", ", keywords)
            };


            return PartialView("ConfigKeywords", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfigKeywords(Keywords keywords)
        {
            var keywordsList = keywords.KeywordsStr.Split(',').Select(t => t.Trim()).ToList();

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var keywordsDb = await _context.TargetKeywords.ToListAsync();
                    foreach (var keyword in keywordsDb)
                    {
                        if (!keywordsList.Contains(keyword.Name))
                        {
                            _context.TargetKeywords.Remove(keyword);
                        }
                    }
                    foreach (var keyword in keywordsList)
                    {
                        if (!keywordsDb.Select(t => t.Name).Contains(keyword))
                        {
                            _context.TargetKeywords.Add(new TargetKeyword { Name = keyword });
                        }
                    }
                    await _context.SaveChangesAsync();

                    var articles = await _context.Articles.ToListAsync();
                    articles.ForEach(t => t.Rank = FindRank(t.Title, t.Content));
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                    throw;
                }
                
            }
            
            
        }


        public int FindRank(string title, string content)
        {
            var keywords = _context.TargetKeywords.Select(t=> t.Name).ToList();
            double rank = 0;
            double priorityCoefficient = 10;
            title = title.ToLower();
            content = content.ToLower();
            foreach (var keyword in keywords)
            {
                if (!title.Contains(keyword) || !content.Contains(keyword))
                {
                    continue;
                }
                else if (title.Contains(keyword) && !content.Contains(keyword))
                {
                    rank += Regex.Matches(title, keyword).Count / title.Length;
                    rank *= priorityCoefficient;
                }
                else if (!title.Contains(keyword) && content.Contains(keyword))
                {
                    rank += Regex.Matches(content, keyword).Count / content.Length;
                }
                else if (title.Contains(keyword) && content.Contains(keyword))
                {
                    rank += Regex.Matches(title, keyword).Count;
                    rank += Regex.Matches(content, keyword).Count;
                    rank *= priorityCoefficient;
                }
            }
            return (int)rank;
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