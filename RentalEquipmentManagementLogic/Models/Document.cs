using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

public partial class Document
{
    [Key]
    public int Id { get; set; }

    public int? RentalTransactionId { get; set; }

    public int? UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UploadedAt { get; set; }

    public byte[]? FileData { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? FileName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? FileType { get; set; }

    [ForeignKey("RentalTransactionId")]
    [InverseProperty("Documents")]
    public virtual RentalTransaction? RentalTransaction { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Documents")]
    public virtual User? User { get; set; }
}
