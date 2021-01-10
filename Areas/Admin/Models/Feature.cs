using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PinBackendSystem.Models
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nama Fitur")]
        public string FeatureName { get; set; }
        
        public string Icon { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; } = 1;
        public bool Selected { get; set; }

        [Display(Name = "Tanggal posting")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd MMM yyyy}")]

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Tanggal edit")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public List<Listing_feature> Listing_feature { get; set; } = new List<Listing_feature>();
    }
}
