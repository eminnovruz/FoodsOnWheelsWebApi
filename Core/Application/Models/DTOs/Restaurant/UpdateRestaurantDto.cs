using Application.Models.DTOs.AppUser;

namespace Application.Models.DTOs.Restaurant;

public class UpdateRestaurantDto : UpdateAppUserDto
{
    public string Description { get; set; }
}