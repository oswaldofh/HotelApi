using HotelApi.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelApi.Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(20)]
        [Required]
        public string Document { get; set; }

        [Required]
        public UserType UserType { get; set; }
    }
}
