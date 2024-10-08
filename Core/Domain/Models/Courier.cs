﻿namespace Domain.Models;


public class Courier : AppUser
{
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string ActiveOrderId {  get; set; }
    public List<string> OrderIds { get; set; }
    public List<string> CourierCommentIds { get; set; }
    public float Rating { get; set; }
}
