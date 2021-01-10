using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PinBackendSystem.Models
{
    public class Listing_feature
    {
        [Key]
        public int Id { get; set; }
        public int FeaturesId { get; set; }

        [ForeignKey("FeaturesId")]
        public Feature Features { get; set; }
        public Listing Listings { get; set; }
    }
}
