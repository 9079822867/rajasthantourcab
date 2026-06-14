using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RajasthanTourCabN.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        // ✅ SEO
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
    }
    public class AdminMenu
    {
        public int Id { get; set; }

        public string Title { get; set; }     // Menu name (Dashboard, Pages)
        public string Url { get; set; }       // लिंक (/Admin/Pages)
        public string Icon { get; set; }      // Bootstrap icon (bi-file, etc.)

        public string Role { get; set; }      // Admin / Editor

        public int DisplayOrder { get; set; } // Menu order
                                              // 🔥 For submenu
        public int? ParentId { get; set; }
    }

    public class CabPricing
    {
        public int Id { get; set; }
        [Required]
        public string VehicleName { get; set; }
        public decimal Fare4hr { get; set; }
        public decimal Fare8hr { get; set; }
        public decimal ExtraKm { get; set; }
        public decimal ExtraHour { get; set; }
        public int DisplayOrder { get; set; }
        public string City { get; set; }
    }
    public class Booking
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string VehicleName { get; set; }
        public int KM { get; set; }
        public int Hours { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class BookingInquiryModel
    {
        public int BookingId { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public string PickupLocation { get; set; }
        public string DropLocation { get; set; }
        public DateTime? PickupDate { get; set; }
        public string PickupTime { get; set; }
        public string VehicleType { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DriverBooking
    {
        public int Id { get; set; }
        public string TripType { get; set; }        // OneWay / RoundTrip
        public string PickupDate { get; set; }
        public string PickupTime { get; set; }
        public string PickupLocation { get; set; }
        public string DropLocation { get; set; }
        public string PickupLat { get; set; }
        public string PickupLng { get; set; }
        public string DropLat { get; set; }
        public string DropLng { get; set; }
        public string DropoffDate { get; set; }
        public string DropoffTime { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CarType { get; set; }        // Hatchback / Sedan / SUV / Luxury
        public string Transmission { get; set; }    // Manual / Automatic
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string DriverName { get; set; }      // assigned driver (set by admin)
        public string DriverMobile { get; set; }    // assigned driver mobile (set by admin)
        public string BookingStatus { get; set; } = "Pending";
        public DateTime CreatedDate { get; set; }
    }

    public class Feedback
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; } // 1 to 5
        public DateTime SubmittedOn { get; set; }
    }
}