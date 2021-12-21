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
        [Display(Name = "Physical Location")]
        PhysicalLocation,

        [Display(Name = "Discord Meeting")]
        DiscordMeeting,

        [Display(Name = "Streaming")]
        Streaming,

        [Display(Name = "Browser Site")]
        BrowserSite,

        [Display(Name = "Steam")]
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

        [Display(Name = "Other")]
        Other
    }
}
