using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public enum PlatformType
    {
        [Display(Name = "Website")]
        Website,

        [Display(Name = "Steam Game")]
        Steam,

        [Display(Name = "Epic")]
        Epic,

        [Display(Name = "Blizzard")]
        Blizzard,

        [Display(Name = "Desktop Application")]
        DesktopApp,

        [Display(Name = "Nintendo Switch")]
        NintendoSwitch,

        [Display(Name = "Xbox")]
        Xbox,

        [Display(Name = "Playstation")]
        Playstation,

        [Display(Name = "Mobile Application")]
        PhoneApp,

        [Display(Name = "Cross-Platform")]
        CrossPlatform,

        [Display(Name = "Streaming from Discord")]
        StreamFromDiscord,

        [Display(Name = "Physical Location")]
        PhysicalLocation,

        [Display(Name = "Other")]
        Other
    }
}
