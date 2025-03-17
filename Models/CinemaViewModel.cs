
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

    public class CinemaViewModel
    {
        public int CinemaId { get; set; }
        public string CinemaName { get; set; } = string.Empty;
    }
