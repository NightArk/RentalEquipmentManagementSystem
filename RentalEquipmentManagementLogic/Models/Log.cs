using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

public partial class Log
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [StringLength(255)]
    public string Action { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Timestamp { get; set; }

    [StringLength(255)]
    public string? AffectedData { get; set; }

    [StringLength(50)]
    public string? Source { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Logs")]
    public virtual User? User { get; set; }
}
