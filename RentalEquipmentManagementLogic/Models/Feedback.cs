using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

[Table("Feedback")]
public partial class Feedback
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? EquipmentId { get; set; }

    public int? Rating { get; set; }

    [StringLength(500)]
    public string? Comment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("EquipmentId")]
    [InverseProperty("Feedbacks")]
    public virtual Equipment? Equipment { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Feedbacks")]
    public virtual User? User { get; set; }
}
