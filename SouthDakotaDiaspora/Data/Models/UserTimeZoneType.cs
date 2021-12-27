using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public enum UserTimeZoneType
    {
        [Display(Name = "Pacific Standard Time")]
        PacificStandardTime,

        [Display(Name = "Mountain Standard Time")]
        MountainStandardTime,

        [Display(Name = "Central Standard Time")]
        CentralStandardTime,

        [Display(Name = "Eastern Standard Time")]
        EasternStandardTime
    }
}
