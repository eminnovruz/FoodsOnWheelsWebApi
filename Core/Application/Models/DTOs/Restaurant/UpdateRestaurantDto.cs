using Application.Models.DTOs.AppUser;

namespace Application.Models.DTOs.Restaurant;

public class UpdateRestaurantDto : UpdateAppUserDto
{
    public string Id { get; set; }
    public string Description { get; set; }
}