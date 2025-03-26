using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

public partial class Equipment
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal RentalPrice { get; set; }

    [StringLength(50)]
    public string AvailabilityStatus { get; set; } = null!;

    [StringLength(50)]
    public string ConditionStatus { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Equipment")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Equipment")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Equipment")]
    public virtual ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();

    [InverseProperty("AssignedEquipment")]
    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}
