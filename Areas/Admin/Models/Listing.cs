using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PinBackendSystem.Models
{
    public class Listing
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tipe")]
        public string TransactionType { get; set; }

        [NotMapped]        
        public List<SelectListItem> TransactionTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Dijual", Text = "Dijual" },
            new SelectListItem { Value = "Disewakan", Text = "Disewakan" }
        };

        [Display(Name = "Id Properti")]
        public string PropertyId { get; set; }

        [Display(Name = "Judul")]
        [StringLength(350, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Deskripsi")]
        public string Description { get; set; }

        [Display(Name = "Gambar Utama")]  
        public string ImagePath1 { get; set; }

        [Display(Name = "Gambar 2")]
        public string ImagePath2 { get; set; }

        [Display(Name = "Gambar 3")]
        public string ImagePath3 { get; set; }

        [Display(Name = "Gambar 4")]
        public string ImagePath4 { get; set; }

        [Display(Name = "Gambar 5")]
        public string ImagePath5 { get; set; }

        [Display(Name = "Gambar Original")]
        public string ImagePath6 { get; set; }

        [Display(Name = "Sertifikat")]
        public string Certificate { get; set; }

        [Display(Name = "Kota")]
        public string Kota { get; set; }

        [NotMapped]
        public List<SelectListItem> Kotas { get; set; }

        [Display(Name = "Kecamatan")]
        public string Kecamatan { get; set; }

        [NotMapped]
        public List<SelectListItem> Kecamatans { get; set; }

        [Display(Name = "Alamat")]
        [RegularExpression(@"^.{10,}$", ErrorMessage = "Minimal 10 karakter")]
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
            new SelectListItem { Value = "Kos", Text = "Kos" },
            new SelectListItem { Value = "Ruko", Text = "Ruko" },
            new SelectListItem { Value = "Kios", Text = "Kios" },
            new SelectListItem { Value = "Bisnis", Text = "Bisnis" },
            new SelectListItem { Value = "Gudang", Text = "Gudang" },            
            new SelectListItem { Value = "Gedung", Text = "Gedung" },            
            new SelectListItem { Value = "Tanah", Text = "Tanah" }
        };

        [DisplayFormat(NullDisplayText = "N/A")]
        public string Interior { get; set; }
        [NotMapped]
        public List<SelectListItem> Interiors { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "" },
            new SelectListItem { Value = "Furnished", Text = "Furnished" },
            new SelectListItem { Value = "Semi Furnished", Text = "Semi Furnished" },
            new SelectListItem { Value = "Not Furnished", Text = "Not Furnished" }
        };

        [Display(Name = "Hadap")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string Orientation { get; set; }

        [NotMapped]
        public List<SelectListItem> Orientations { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "" },
            new SelectListItem { Value = "Utara", Text = "Utara" },
            new SelectListItem { Value = "Barat", Text = "Barat" },
            new SelectListItem { Value = "Timur", Text = "Timur" },
            new SelectListItem { Value = "Selatan", Text = "Selatan" }
        };
        [Display(Name = "KwH Listrik")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string Electricity { get; set; }

        [Display(Name = "Air")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string Water { get; set; }        

        [Display(Name = "Jumlah Kamar Tidur")]
        public int? NoOfBed { get; set; }

        [Display(Name = "Jumlah Kamar Mandi")]
        public int? NoOfBath { get; set; }

        [Display(Name = "Jumlah Lantai")]
        public int? NoOfFloor { get; set; } = 1;

        [Display(Name = "Jumlah Garasi")]
        public int? NoOfGarage { get; set; } = 0;

        [Display(Name = "Lebar")]
        public int? Width { get; set; }

        [Display(Name = "Panjang")]
        public int? Length { get; set; }

        [Display(Name = "Kondisi Properti")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string PropertyCondition { get; set; }

        [NotMapped]
        public List<SelectListItem> PropertyConditions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "" },
            new SelectListItem { Value = "Baru", Text = "Baru" },
            new SelectListItem { Value = "Bagus", Text = "Bagus" },
            new SelectListItem { Value = "Butuh Renovasi", Text = "Butuh Renovasi" }
        };

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Harga")]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; } = 0;
        public List<Listing_feature> Listing_feature { get; set; }= new List<Listing_feature>();

        [Display(Name = "Fitur")]
        [NotMapped]
        public List<Feature> Features { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; } = 0;

        public bool IsPrimary { get; set; } = false;
        public bool IsFeatured { get; set; } = false;

        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [NotMapped]
        public List<SelectListItem> Ratings { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "" },
            new SelectListItem { Value = "1", Text = "1" },
            new SelectListItem { Value = "2", Text = "2" },
            new SelectListItem { Value = "3", Text = "3" },
            new SelectListItem { Value = "4", Text = "4" },
            new SelectListItem { Value = "5", Text = "5" }
        };

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
        [Display(Name = "Gambar Tambahan(Maksimal 4)")]
        public List<IFormFile> FileList { get; set; }

        [NotMapped]
        [Display(Name = "Gambar Utama")]
        public IFormFile File1 { get; set; }

        [NotMapped]
        [Display(Name = "Gambar 2")]
        public IFormFile File2 { get; set; }

        [NotMapped]
        [Display(Name = "Gambar 3")]
        public IFormFile File3 { get; set; }

        [NotMapped]
        [Display(Name = "Gambar 4")]
        public IFormFile File4 { get; set; }

        [NotMapped]
        [Display(Name = "Gambar 5")]
        public IFormFile File5 { get; set; }

        [NotMapped]
        [Display(Name = "Gambar Original")]
        public IFormFile File6 { get; set; }
    }
}
