using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;
    public class SeatViewModel
    {
        public int SeatId { get; set; }
        public string SeatRow { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; } // Đánh dấu đã đặt hay chưa
    }
