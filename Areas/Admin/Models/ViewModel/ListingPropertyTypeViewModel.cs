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
        public PaginatedList<Listing> Listings{ get; set; }
        public SelectList PropertyType { get; set; }
        public string ListingPropertyType { get; set; }
        public string SearchString { get; set; }
        public int? pageIndex { get; set; }
    }
}
