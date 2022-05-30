using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PapaMobileX.Server.DataSource.Models;

public class Account
{
    public Account()
    {
        AccountGuid = Guid.NewGuid();
        SecurityStamp = Guid.NewGuid().ToString();
    }
    
    public Account(string userName) : this()
    {
        UserName = userName;
    }
    
    [Required]
    [Key]
    public Guid AccountGuid { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = null!;

    [MaxLength(256/8)]
    public string PasswordHash { get; set; } = null!;

    [MaxLength(128/8)]
    public string Salt { get; set; } = null!;
    public string SecurityStamp { get; set; } = null!;

    public List<Claim> Claims { get; set; } = new();
}