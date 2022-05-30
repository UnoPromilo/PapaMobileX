using System.ComponentModel.DataAnnotations;

namespace PapaMobileX.DTOs.Requests;

public class LoginDTO
{
    [Required]
    public string UserName { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;
}