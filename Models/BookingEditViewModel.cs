using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

public class BookingEditViewModel
{
    public int BookingId { get; set; }
    public int ShowingTimeId { get; set; }
    public List<int> SelectedSeatIds { get; set; } // Danh sách ghế chọn mới
    public List<Seat> AvailableSeats { get; set; } // Tất cả ghế có thể chọn (load từ phòng chiếu)
    public decimal TotalFee { get; set; } // Tổng phí (cập nhật lại theo số ghế)
}
