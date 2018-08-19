
namespace CinemaBookingCore.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Promotion
    {
        public int PromotionId { get; set; }
        public int CinemaId { get; set; }
        public string UrlDocument { get; set; }
    
        public virtual Cinema Cinema { get; set; }
    }
}
