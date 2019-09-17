using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyBB.Models.ViewModels
{
    public class ThemeViewModel
    {

    

        [StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "{0}长度不对")]
        [Display(Name = "标题")]
        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "内容")]
        [StringLength(maximumLength: 10000, MinimumLength = 6, ErrorMessage = "{0}长度不对")]
        public string Content { get; set; }
        public int ID { get; internal set; }
    }
}