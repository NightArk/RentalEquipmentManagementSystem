using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

public partial class ReturnRecord
{
    [Key]
    public int Id { get; set; }

    public int? RentalTransactionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActualReturnDate { get; set; }

    [StringLength(50)]
    public string ReturnCondition { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? LateReturnFee { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? AdditionalCharges { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("RentalTransactionId")]
    [InverseProperty("ReturnRecords")]
    public virtual RentalTransaction? RentalTransaction { get; set; }
}
