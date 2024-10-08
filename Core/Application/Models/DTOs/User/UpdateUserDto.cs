﻿using Application.Models.DTOs.AppUser;

namespace Application.Models.DTOs.User;

public class UpdateUserDto : UpdateAppUserDto
{
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; }
}