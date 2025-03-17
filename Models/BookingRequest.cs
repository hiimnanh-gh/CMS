using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models
{ 
public class BookingRequest
    {
        public int TimeId { get; set; } // Suất chiếu
        public int UserId { get; set; } // Khách hàng
        public List<int> SeatIds { get; set; } = new List<int>(); // Danh sách ghế
        public decimal BookingFee { get; set; } // Phí tổng
        public string EmailAddress { get; set; } = string.Empty; // Email khách hàng
    }
}