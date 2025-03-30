using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;
public class BookingSuccessViewModel
{
    public int BookingId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string MovieName { get; set; } = string.Empty;
    public string CinemaLocation { get; set; } = string.Empty;
    public string TheatreNum { get; set; } = string.Empty;
    public DateTime ShowingDate { get; set; }  // Để lưu ngày chiếu
    public TimeSpan ShowingTime { get; set; }  // Để lưu giờ chiếu
    public List<string> Seats { get; set; } = new();
    public decimal TotalFee { get; set; }
    public DateTime BookingDate { get; set; }  // Ngày đặt vé
}

