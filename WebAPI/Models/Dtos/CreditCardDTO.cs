using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos;

public class CreditCardDTO
{
    public int Id { get; set; }
    public string NameOnCard { get; set; } = null!;
    public string CardNo { get; set; } = null!;
    public int ExpireMonth { get; set; }
    public int ExpireYear { get; set; }
    public string CVV { get; set; } = null!;

}
