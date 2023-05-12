using BankCustomerSystem.Services.Finance.Api.Models.Enums;
using BankCustomerSystem.Services.Finance.Api.Models.Responses;
using BankCustomerSystem.Services.Finance.Api.Resources.Messages;
using BankCustomerSystem.Services.Finance.Services.Models.Custom;
using BankCustomerSystem.Services.Finance.Services.Models.Enums;
using BankCustomerSystem.Services.Finance.Services.Models.Responses;

namespace BankCustomerSystem.Services.Finance.Api.Helpers
{
    public class ResponseHelper
    {
        #region prop
        private readonly string exceptionUserMessageKey = "ExceptionUserMessage";
        private List<(string key, string value)> messages = new List<(string, string)>();
        #endregion

        #region GenerateResponse
        public ApiResponse GenerateResponse(BaseServiceResponse serviceResponse,string language) {
            FillSystemMessages(language);
            var userMessages = serviceResponse.SendDirectly ? serviceResponse.UserMessages : GetApiMessagesFromServiceMessages(serviceResponse.UserMessages);
            var techMessages = GetApiMessagesFromServiceMessages(serviceResponse.TechMessages);
            return new ApiResponse
            {
                Data = serviceResponse.Data,
                ApiStatus = serviceResponse.Status,
                UserMessages = userMessages,
                TechMessages = techMessages
            };
        }

        private List<MessageDTO> GetApiMessagesFromServiceMessages(List<MessageDTO> servicMessages)
        {
            var apiMessages = servicMessages.Select(m => new MessageDTO
            {
                Message = GetMessageValueFromResources(m.Message, m.Parameters),
                Type = m.Type
            }).ToList();
            return apiMessages;
        }

        private string GetMessageValueFromResources(string messageKey,List<string> parameters) 
        {
            string message = messages.Where(m => m.key == messageKey).Any() ?
                             messages.Where(m => m.key == messageKey).FirstOrDefault().value :
                             messageKey;
            for(int i = 0; i < parameters.Count; i++) 
            {
                message.Replace("{" + i + "}", parameters[i]);
            }
            return message;
        }
        #endregion

        #region GenerateExceptionResponse

        public ApiResponse GenerateExceptionResponse(Exception ex, string language) 
        {
            FillSystemMessages(language);
            return new ApiResponse
            {
                Data = null,
                ApiStatus = (int)ApiStatusEnum.Exception,
                UserMessages = GetUserMessagesForException(),
                TechMessages = GetTechMessagesForException(ex)
            };
        }

        private List<MessageDTO> GetUserMessagesForException() 
        {
            var userMessages = new List<MessageDTO>
            {
                new MessageDTO
                {
                    Message = messages.Where(m=>m.key == exceptionUserMessageKey).FirstOrDefault().value,
                    Type = (int)MessageTypeEnum.Error
                }
            };
            return userMessages;
        }

        private List<MessageDTO> GetTechMessagesForException(Exception ex) 
        {
            var techMessages = new List<MessageDTO>
            {
                new MessageDTO
                {
                    Message = ex.Message,
                    Type = (int)MessageTypeEnum.Exception,
                },
                new MessageDTO
                {
                    Message = ex.ToString(),
                    Type = (int)MessageTypeEnum.Exception
                }
            };
            return techMessages;
        }

        #endregion

        private void FillSystemMessages(string language) 
        {
            switch (language)
            {
                case "ar":
                    messages = Messages_ar.Messages;
                    break;
                case "en":
                    messages = Messages_en.Messages;
                    break;
                default:
                    messages = Messages_en.Messages;
                    break;
            }
        }
    }
}
