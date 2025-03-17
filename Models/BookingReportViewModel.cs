using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;
public class BookingReportViewModel
{
    public int BookingId { get; set; }
    public string UserName { get; set; }
    public string MovieName { get; set; }
    public string CinemaLocation { get; set; }
    public int TheatreNum { get; set; }
    public DateTime? ShowingDate { get; set; }
    public TimeSpan? ShowingTime { get; set; }
    public List<string> Seats { get; set; }
    public decimal TotalFee { get; set; }
    public DateTime BookingDate { get; set; }
}
