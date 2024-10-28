namespace ImageSearchApp.Models
{
    public class PexelsImage
    {
        public int Id { get; set; }
        public string Photographer { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string SrcMedium { get; set; } = string.Empty;
        public string SrcLarge { get; set; } = string.Empty;
    }
}
