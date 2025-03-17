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
}
