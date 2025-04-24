using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

public partial class RentalTransaction
{
    [Key]
    public int Id { get; set; }

    public int? RentalRequestId { get; set; }

    public int? AssignedEquipmentId { get; set; }

    public int? CustomerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ActualRentalStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReturnDate { get; set; }

    public int RentalPeriod { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal RentalFee { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Deposit { get; set; }

    [StringLength(50)]
    public string? PaymentStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AssignedEquipmentId")]
    [InverseProperty("RentalTransactions")]
    public virtual Equipment? AssignedEquipment { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("RentalTransactions")]
    public virtual User? Customer { get; set; }

    [InverseProperty("RentalTransaction")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [ForeignKey("RentalRequestId")]
    [InverseProperty("RentalTransactions")]
    public virtual RentalRequest? RentalRequest { get; set; }

    [InverseProperty("RentalTransaction")]
    public virtual ICollection<ReturnRecord> ReturnRecords { get; set; } = new List<ReturnRecord>();
}
