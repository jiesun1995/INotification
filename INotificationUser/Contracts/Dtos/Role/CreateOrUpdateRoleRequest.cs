using INotificationUser.Domain.Base;

namespace INotificationUser.Contracts.Dtos.Role
{
    public class CreateOrUpdateRoleRequest:BaseRequest
    {
        public string? Name { get; set; }

        public string? NormalizedName { get; set; }
    }
}
