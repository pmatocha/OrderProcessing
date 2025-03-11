using System.ComponentModel.DataAnnotations;
using JsmeNaLede.API.Model.Interfaces;

namespace OrderProcessing.Infrastructure.Entities;

public class BaseEntity : IIdentifiable<Guid>
{
    [Key]  
    public Guid Id { get; set; } = Guid.NewGuid(); 
}