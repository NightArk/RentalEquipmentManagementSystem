using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

[Index("Email", Name = "UQ__Users__A9D10534EE022DF7", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(256)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(50)]
    public string Role { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [InverseProperty("User")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("User")]
    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("Customer")]
    public virtual ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();

    [InverseProperty("Customer")]
    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}
