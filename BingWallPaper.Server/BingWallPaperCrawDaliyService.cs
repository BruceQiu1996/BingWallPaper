using BingWallPaper.Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BingWallPaper.Server
{
    public class BingWallPaperCrawDaliyService : BackgroundService
    {
        private readonly DateTime? _LastCrawTime = null;
        private readonly BingwallpaperContext _bingwallpaperContext;
        private readonly ILogger<BingWallPaperCrawDaliyService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _cnUrl = "https://global.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&setmkt=zh-CN";
        private readonly string _usUrl = "https://global.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&setmkt=en-US";
        private readonly string _hostPrefix = "https://cn.bing.com";
        public BingWallPaperCrawDaliyService(BingwallpaperContext bingwallpaperContext, ILogger<BingWallPaperCrawDaliyService> logger)
        {
            _logger = logger;
            _bingwallpaperContext = bingwallpaperContext;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (DateTime.Now.Hour == 11) //每天下午13点更新数据
                    {
                        int i = 0;
                        while (i < 3)
                        {
                            try
                            {
                                var cnDate = await (await _httpClient.GetAsync(_cnUrl)).Content.ReadAsStringAsync();
                                var json = JsonSerializer.Deserialize<BingJson>(cnDate);
                                if (json != null && json.Images != null)
                                {
                                    if (json.Images.Count >= 1)
                                    {
                                        var image = json.Images[0];
                                        DateOnly date = DateOnly.ParseExact(image.StartDate, "yyyyMMdd");
                                        if (await _bingwallpaperContext.WallpaperCns.FirstOrDefaultAsync(x => x.Date.Value == date) == null)
                                        {
                                            await _bingwallpaperContext.WallpaperCns.AddAsync(new WallpaperCn()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                UrlBase = $"{_hostPrefix}{image.UrlBase}",
                                                Desc = image.Copyright,
                                                Date = date
                                            });
                                        }
                                    }
                                }

                                var usDate = await (await _httpClient.GetAsync(_usUrl)).Content.ReadAsStringAsync();
                                var json1 = JsonSerializer.Deserialize<BingJson>(usDate);
                                if (json1 != null && json1.Images != null)
                                {
                                    if (json1.Images.Count >= 1)
                                    {
                                        var image = json1.Images[0];
                                        DateOnly date = DateOnly.ParseExact(image.StartDate, "yyyyMMdd");
                                        if (await _bingwallpaperContext.Wallpapers.FirstOrDefaultAsync(x => x.Date.Value == date) == null)
                                        {
                                            await _bingwallpaperContext.Wallpapers.AddAsync(new Wallpaper()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                UrlBase = $"{_hostPrefix}{image.UrlBase}",
                                                Desc = image.Copyright,
                                                Date = date
                                            });
                                        }
                                    }
                                }

                                await _bingwallpaperContext.SaveChangesAsync();

                                break;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.ToString());
                                i++;
                                continue;
                            }
                        }
                    }

                    await Task.Delay(1000 * 60 * 60);
                }
            });
        }

        public class BingJson
        {
            [JsonPropertyName("images")]
            public List<BingImage> Images { get; set; }
        }

        public class BingImage
        {
            [JsonPropertyName("startdate")]
            public string StartDate { get; set; }
            [JsonPropertyName("urlbase")]
            public string UrlBase { get; set; }
            [JsonPropertyName("copyright")]
            public string Copyright { get; set; }
        }
    }
}
