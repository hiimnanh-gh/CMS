using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;
    public class ShowTimeViewModel
    {
        public int TimeId { get; set; }
        public TimeSpan ShowingDatetime { get; set; }
    }
