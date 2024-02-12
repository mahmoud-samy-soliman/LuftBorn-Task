using Microsoft.AspNetCore.Identity;

namespace Takamol.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string IdentityNo { get; set; }
        public string FingerPrintId { get; set; }
        public bool IsMale { get; set; }
        public int? SuspensionStatusId { get; set; }
        public bool Deleted { get; set; }
        public bool Archived { get; set; }
    }
}
