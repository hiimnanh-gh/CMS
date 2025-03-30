using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;
public class MovieListViewModel
{
    // Danh sách phim trả về
    public List<Movie> Movies { get; set; }

    // Danh sách thể loại để khách chọn lọc
    public List<Genre> Genres { get; set; }

    // Danh sách trạng thái phim
    public List<MovieStatus> Statuses { get; set; }

    // Biến để biết đang lọc theo thể loại nào
    public int? SelectedGenreId { get; set; }

    // Biến để biết đang lọc theo trạng thái nào
    public int? SelectedStatusId { get; set; }

    // Biến lưu từ khóa tìm kiếm
    public string SearchKeyword { get; set; }
}
