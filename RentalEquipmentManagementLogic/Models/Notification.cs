using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

public partial class Notification
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [StringLength(500)]
    public string MessageContent { get; set; } = null!;

    [StringLength(50)]
    public string NotificationType { get; set; } = null!;

    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User? User { get; set; }
}
