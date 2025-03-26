using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

public partial class RentalRequest
{
    [Key]
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? EquipmentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RentalStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReturnDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalCost { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("RentalRequests")]
    public virtual User? Customer { get; set; }

    [ForeignKey("EquipmentId")]
    [InverseProperty("RentalRequests")]
    public virtual Equipment? Equipment { get; set; }

    [InverseProperty("RentalRequest")]
    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}
