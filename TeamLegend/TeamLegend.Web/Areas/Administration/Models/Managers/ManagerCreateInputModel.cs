﻿namespace TeamLegend.Web.Areas.Administration.Models.Managers
{
    using Microsoft.AspNetCore.Http;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class ManagerCreateInputModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(int.MaxValue, MinimumLength = 3, ErrorMessage = "Manager's name has to contain at least {2} characters.")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Nationality")]
        [StringLength(int.MaxValue, MinimumLength = 3, ErrorMessage = "Manager's nationality name has to contain at least {2} characters.")]
        public string Nationality { get; set; }

        [Display(Name = "Manager Picture")]
        public IFormFile ManagerPicture { get; set; }
    }
}