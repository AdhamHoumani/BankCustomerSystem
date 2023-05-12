using BankCustomerSystem.Services.Core.Services.Models.Custom;

namespace BankCustomerSystem.Services.Core.Api.Models.Responses
{
    public class ApiResponse
    {
        public dynamic? Data { get; set; }
        public int ApiStatus { get; set; }
        public List<MessageDTO> UserMessages { get; set; } = new List<MessageDTO>();
        public List<MessageDTO> TechMessages { get; set; } = new List<MessageDTO>();
    }
}
