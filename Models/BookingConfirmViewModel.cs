using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;
public class BookingConfirmViewModel
{
    public string UserName { get; set; }
    public string TheatreName { get; set; }
    public string ShowingTime { get; set; }
    public List<string> SeatNumbers { get; set; }
    public List<int> SeatIds { get; set; }
    public int TimeId { get; set; }
    public decimal TotalFee { get; set; }
}
