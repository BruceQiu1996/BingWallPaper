using BingWallPaper.Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BingWallPaper.Server
{
    public class BingWallPaperCrawDaliyService : BackgroundService
    {
        private readonly DateTime? _LastCrawTime = null;
        private readonly BingwallpaperContext _bingwallpaperContext;
        private readonly HttpClient _httpClient;
        private readonly string _cnUrl = "https://global.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&setmkt=zh-CN";
        private readonly string _usUrl = "https://global.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&setmkt=en-US";
        private readonly string _hostPrefix = "https://cn.bing.com";
        public BingWallPaperCrawDaliyService(BingwallpaperContext bingwallpaperContext)
        {
            _bingwallpaperContext = bingwallpaperContext;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (DateTime.Now.Hour == 17)

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
                                    DateOnly date = DateOnly.ParseExact(image.StartDate,"yyyyMMdd");
                                    if (_bingwallpaperContext.WallpaperCns.FirstOrDefaultAsync(x => x.Date.Value == date) == null)
                                    {
                                        await _bingwallpaperContext.WallpaperCns.AddAsync(new WallpaperCn()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            UrlBase = Path.Combine(_hostPrefix, image.UrlBase),
                                            Desc = image.Copyright,
                                            Date = DateOnly.Parse(image.StartDate)
                                        });
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

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
