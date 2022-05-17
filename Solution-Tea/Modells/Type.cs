using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Solution_Tea.Modells.Tea;

namespace Solution_Tea.Modells
{
    [Table("Type")]
    public class Type
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }

        public virtual ICollection<Tea> Teas { get; set; }

        public Type()
        {
        }

        public Type(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
