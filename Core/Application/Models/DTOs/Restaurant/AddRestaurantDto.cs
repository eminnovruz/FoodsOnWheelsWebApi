using Application.Models.DTOs.AppUser;
using Microsoft.AspNetCore.Http;

namespace Application.Models.DTOs.Restaurant;

public class AddRestaurantDto  : AddAppUserDto
{
    public string Description { get; set; }
    public IFormFile File { get; set; }
}