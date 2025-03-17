using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS.Models;

[Table("cast_member")]
public partial class CastMember
{
    [Key]
    [Column("cast_id")]
    public int CastId { get; set; }

    [Column("cast_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string CastName { get; set; } = null!;
}
