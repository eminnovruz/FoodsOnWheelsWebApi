using Application.Models.DTOs.Comment;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.User;

namespace Application.Services.IUserServices;

public interface ICourierService
{
    Task<bool> AcceptOrder(AcceptOrderFromCourierDto request);
    
    Task<bool> RejectOrder(RejectOrderDto orderDto);
    
    List<InfoOrderDto> GetNewOrder();
    
    Task<GetProfileInfoDto> GetProfileInfo(string CourierId);
    
    Task<bool> UpdateProfile(UpdateCourierDto dto);

    Task<bool> UpdateProfilePasssword(UpdateCourierPasswordDto dto);
    
    Task<bool> RemoveProfile(string courierId);
    
    Task<IEnumerable<InfoOrderDto>> GetOrderHistory(string CourierId);
    
    Task<InfoOrderDto> GetActiveOrderInfo(string OrderId);
    
    Task<IEnumerable<GetCommentDto>> GetAllComments(string CourierId);
    
    Task<InfoOrderDto> GetPastOrderInfoById(string PastOrderId);
}