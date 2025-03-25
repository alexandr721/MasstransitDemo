using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Shared.Interfaces
{
    public interface INotificationMessage
    {
        string UserId { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        DateTime CreatedAt { get; set; }
        bool IsActive { get; set; }
        string RoleId { get; set; }
        string RoleName { get; set; }
        string Message { get; set; }
    }

}
