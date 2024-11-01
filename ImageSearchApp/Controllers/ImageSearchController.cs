﻿using ImageSearchApp.Models;
using ImageSearchApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ImageSearchApp.Controllers
{
    public class ImageSearchController : Controller
    {
        private readonly PexelsService _pexelsService;

        public ImageSearchController(PexelsService pexelsService)
        {
            _pexelsService = pexelsService;
        }

        // Trang tìm kiếm
        public async Task<IActionResult> Index(string keyword, int page = 1, int perPage = 10)
        {
            var images = await _pexelsService.SearchImagesAsync(keyword, page);
            ViewBag.Keyword = keyword;
            ViewBag.Page = page;
            return View(images);
        }

        // Trang xem chi tiết ảnh
        public IActionResult Detail(string srcLarge, string photographer, string url)
        {
            var imageDetail = new PexelsImage
            {
                SrcLarge = srcLarge,
                Photographer = photographer,
                Url = url
            };

            return View("Detail", imageDetail);
        }


        // Quay lại trang tìm kiếm
        public IActionResult BackToSearch(string keyword)
        {
            return RedirectToAction("Index", new { keyword });
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreImages(string keyword, int page = 1)
        {
            var images = await _pexelsService.SearchImagesAsync(keyword, page);
            return Json(images);
        }

        // Tải ảnh về
        public async Task<IActionResult> DownloadImage(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
            var fileName = Path.GetFileName(url);

            fileName = fileName.Split('?')[0];

            return File(content, contentType, fileName);
        }
    }
}
