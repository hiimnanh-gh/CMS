using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;
public class BookingSuccessViewModel
{
    public int BookingId { get; set; }
    public string UserName { get; set; } // Tên khách hàng
    public DateTime CreatedDate { get; set; } // Ngày đặt vé
    public string MovieName { get; set; } // Tên phim
    public string CinemaLocation { get; set; } // Rạp (Hà Nội, HCM)
    public int TheatreNum { get; set; } // Số phòng chiếu
    public string ShowingTime { get; set; } // Giờ chiếu (VD: 15/03/2025 - 19:30)
    public List<string> Seats { get; set; } // Danh sách ghế (VD: A1, A2, B3)
    public decimal TotalFee { get; set; } // Tổng tiền
}

