using Microsoft.AspNetCore.Mvc.Rendering;
using PinBackendSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PinBackendSystem.Models
{
    public class ListingPropertyTypeViewModel
    {
        public PaginatedList<Listing> Listings { get; set; }
        public SelectList PropertyType { get; set; }

        public SelectList Kota { get; set; }
        public SelectList Kecamatan { get; set; }
        public List<SelectListItem> TransactionType { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Dijual", Text = "Dijual"},
            new SelectListItem { Value = "Disewa", Text = "Disewa" }
        };
        public string ListingPropertyType { get; set; }
        public string SearchString { get; set; }
        public string SearchIdString { get; set; }
        public int? pageIndex { get; set; }
    }
    public class DetailPropertyViewModel
    {
        public PaginatedList<Listing> Listings{ get; set; }
        public SelectList PropertyType { get; set; }
        public string ListingPropertyType { get; set; }
        public string SearchString { get; set; }
        public int? pageIndex { get; set; }
    }

}
