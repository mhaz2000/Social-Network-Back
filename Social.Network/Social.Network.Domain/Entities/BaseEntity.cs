using System;
using System.ComponentModel.DataAnnotations;

namespace Social.Network.Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
