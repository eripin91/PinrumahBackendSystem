using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PinBackendSystem.Areas.Identity
{
    public class PinrumahUser : IdentityUser
    {
        [PersonalData]
        public int parent_id { get; set; } = 0;
        [PersonalData]
        public string full_name { get; set; }
        [PersonalData]
        public string slug { get; set; }
        [PersonalData]
        public string agent_status { get; set; }
        [PersonalData]
        public string agent_level { get; set; }
        [PersonalData]
        public string home_address { get; set; }
        [PersonalData]
        public string office_name { get; set; }
        [PersonalData]
        public string office_address { get; set; }
        [PersonalData]
        public string avatar_link { get; set; }
    }
}
