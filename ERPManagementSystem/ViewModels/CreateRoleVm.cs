using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.ViewModels
{
    public class CreateRoleVm
    {
        public CreateRoleVm()
        {
            Users=new List<UserInRoleVm>();
        }
        public string Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public List<UserInRoleVm> Users { get; set; }
    }
}
