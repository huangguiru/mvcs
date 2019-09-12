using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyBB.Models
{
    public class UserLoginDTO
    {
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "{0}长度不对")]
        [Display(Name = "用户名")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "密码")]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = "{0}长度不对")]
        public string Pwd { get; set; }
    }
}