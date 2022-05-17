using Solution_Tea.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Tea.Modells
{
    [Table("Tea")]

    public class Tea :PropertyChangedNotification
    {
        
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        [StringLength(255)]
        [MinLength(3)]
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(() => Name, value); }
        }

        [Required]
        public string Manufacturer
        {
            get { return GetValue(() => Manufacturer); }
            set { SetValue(() => Manufacturer, value); }
        }

        [Required]
        public DateTime Debut
        {
            get { return GetValue(() => Debut); }
            set { SetValue(() => Debut, value); }
        }

        [Required]
        [ForeignKey("Type")]
        [Range(1,int.MaxValue, ErrorMessage ="Type field is required")]
        public int TypeId
        {
            get { return GetValue(() => TypeId); }
            set { SetValue(() => TypeId, value); }
        }
        public virtual Type Type { get; set; }


        public Tea()
        {
            Debut = DateTime.Now;
        }

        public Tea(int id, string name, string manufacturer, DateTime debut, int typeId)
        {
            Id = id;
            Name = name;
            Manufacturer = manufacturer;
            Debut = debut;
            TypeId = typeId;
        }

        public Tea (Tea tea)
        {
            Id = tea.Id;
            Name = tea.Name;
            Manufacturer = tea.Manufacturer;
            Debut = tea.Debut;
            TypeId = tea.TypeId;
        }
    }
}
