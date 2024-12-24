namespace BingWallPaper.Common.Dto
{
    public class WallPaperDetailDto
    {
        public string Id { get; set; }
        public string HDImage { get; set; }
        public string Desc { get; set; }
        public string Author { get; set; }
        public DateOnly? Date { get; set; }
    }
}
