using System.ComponentModel.DataAnnotations;

namespace Bit.Core.Models.Api
{
    public class SecurityStampRequestModel
    {
        [Required]
        public string MasterPasswordHash { get; set; }
    }
}
