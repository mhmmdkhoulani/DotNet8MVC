using Microsoft.AspNetCore.Mvc.Rendering;
using MVCProject.Attributes;
using System.Runtime.CompilerServices;

namespace MVCProject.ViewModels
{
    public class CreateGameFormViewModel : BaseGameFormViewModel
    {

        [AllowedExtensionsAttribute(FileSettings.AllowedExtensions), MaxFileSizeAttribute(FileSettings.MaxFileSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;

    }
}
