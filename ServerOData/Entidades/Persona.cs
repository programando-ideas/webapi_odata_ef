using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odatawebapi.Entidades
{
    public class Persona
    {
        public Persona()
        {
            Direccion = new HashSet<Direccion>();
            Telefono = new HashSet<Telefono>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string NombreYApellido { get; set; }
        [Required]
        public int Edad { get; set; }

        public ICollection<Telefono> Telefono { get; set; }
        public ICollection<Direccion> Direccion { get; set; }
    }
}
