 
using System.ComponentModel.DataAnnotations;

namespace ChatDomain.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int id { get; set; }

    }
}
