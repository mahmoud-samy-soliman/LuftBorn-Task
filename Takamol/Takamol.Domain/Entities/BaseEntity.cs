using System;

namespace Takamol.Domain.Entities
{
    public class BaseEntity
    {
        public string CreatedById { get; set; }
        public DateTime CreationDate { get; set; }
        public string? ModifiedById { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool Deleted { get; set; }
    }
}
