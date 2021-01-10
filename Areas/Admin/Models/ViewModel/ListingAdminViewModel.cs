using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PinBackendSystem.Models
{
    public class ListingAdminViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Jenis Transaksi")]
        public string TransactionType { get; set; }

        [NotMapped]        
        public List<SelectListItem> TransactionTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Jual", Text = "Jual" },
            new SelectListItem { Value = "Beli", Text = "Beli" }
        };

        [Display(Name = "Id Properti")]
        public string PropertyId { get; set; }

        [Display(Name = "Judul")]
        [StringLength(350, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Deskripsi")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Listing Card")]
        public string ListingCard { get; set; }

        [Display(Name = "Gambar 1")]        
        public string ImagePath1 { get; set; }

        [Display(Name = "Gambar 2")]
        public string ImagePath2 { get; set; }

        [Display(Name = "Gambar 3")]
        public string ImagePath3 { get; set; }

        [Display(Name = "Gambar 4")]
        public string ImagePath4 { get; set; }

        [Display(Name = "Gambar 5")]
        public string ImagePath5 { get; set; }

        [Display(Name = "Sertifikat")]
        public string Certificate { get; set; }

        [Display(Name = "Alamat")]
        [RegularExpression(@"^.{10,}$", ErrorMessage = "Minimal 10 karakter")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Luas Tanah")]
        public int? LandSize { get; set; }

        [Display(Name = "Luas Bangunan")]
        public int? BuildingSize { get; set; }

        [Display(Name = "Tipe Properti")]
        public string PropertyType { get; set; }

        [NotMapped]
        public List<SelectListItem> PropertyTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Rumah", Text = "Rumah" },
            new SelectListItem { Value = "Apartemen", Text = "Apartemen" },
            new SelectListItem { Value = "Ruko", Text = "Ruko" },
            new SelectListItem { Value = "Gudang", Text = "Gudang" },
            new SelectListItem { Value = "Tanah", Text = "Tanah" }
        };

        [Display(Name = "Jumlah Kamar Tidur")]
        public int? NoOfBed { get; set; } = 0;

        [Display(Name = "Jumlah Kamar Mandi")]
        public int? NoOfBath { get; set; } = 0;

        [Display(Name = "Jumlah Lantai")]
        public int? NoOfFloor { get; set; } = 1;

        [Display(Name = "Jumlah Garasi")]
        public int? NoOfGarage { get; set; } = 0;

        [Display(Name = "Kondisi Properti")]  
        public string PropertyCondition { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "Fitur")]
        public ICollection<Feature> Features { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; } = 0;

        [Display(Name = "Tanggal posting")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd MMM yyyy}")]

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Tanggal edit")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        [Display(Name = "Nama Agen")]
        public string AgentName { get; set; }

        [Display(Name = "Email Agen")]
        public string AgentEmail { get; set; }

        [Display(Name = "Nomor HP Agen")]
        public string AgentMobileNo { get; set; }

        [Display(Name = "Nama Owner")]
        public string OwnerName { get; set; }

        [Display(Name = "Nomor HP Owner")]
        public string OwnerMobileNo { get; set; }

        [NotMapped]
        public IFormFile File1 { get; set; }

        [NotMapped]
        public IFormFile File2 { get; set; }

        [NotMapped]
        public IFormFile File3 { get; set; }

        [NotMapped]
        public IFormFile File4 { get; set; }

        [NotMapped]
        public IFormFile File5 { get; set; }

        [NotMapped]
        public IFormFile FileListingCard { get; set; }

    }
}
