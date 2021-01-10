using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PinBackendSystem.Models
{
    public class Kota
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nama Kota")]
        public string KotaName { get; set; }        

        [Display(Name = "Status")]
        public int Status { get; set; } = 1;

        public List<Kecamatan> Kecamatan { get; set; } = new List<Kecamatan>();
        
    }
}
