using MVCProject.Attributes;

namespace MVCProject.ViewModels
{
    public class EditGameFormViewModel : BaseGameFormViewModel
    {
       
        public int Id { get; set; }
        [AllowedExtensionsAttribute(FileSettings.AllowedExtensions), MaxFileSizeAttribute(FileSettings.MaxFileSizeInBytes)]
        public IFormFile? Cover { get; set; } = default!;
        public string? CoverUrl { get; set; } 
    }
}
