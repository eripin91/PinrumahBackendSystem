using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PinBackendSystem.Models
{
    public class Kecamatan
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nama Kecamatan")]
        public string KecamatanName { get; set; }


        public int? KotaId { get; set; }

        [ForeignKey("KotaId")]
        public Kota Kota { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; } = 1;

    }
}
