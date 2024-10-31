namespace KhyberYouth.ViewModel
{
    public class MediaGalleryCreateViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile Photo { get; set; }
    }
}