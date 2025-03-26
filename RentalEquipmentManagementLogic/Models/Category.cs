using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementLogic.Models;

[Index("Name", Name = "UQ__Categori__737584F6D963C015", IsUnique = true)]
public partial class Category
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
