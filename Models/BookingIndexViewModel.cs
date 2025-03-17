using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

public class BookingIndexViewModel
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public List<Theatre> Theatres { get; set; }


    public int BookingId { get; set; }
    public string CustomerName { get; set; }
    public string TheatreLocation { get; set; }
    public int TheatreNum { get; set; }
    public DateTime ShowDate { get; set; }
    public TimeSpan ShowingDateTime { get; set; }
    public List<string> Seats { get; set; }
    public decimal TotalFee { get; set; }
    public DateTime CreatedDate { get; set; }
}
