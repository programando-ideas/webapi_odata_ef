using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace odatawebapi.Entidades
{
    public class Direccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string DireccionDesc { get; set; }
        public int? IdPersona { get; set; }
        [JsonIgnore]
        public Persona IdPersonaNavigation { get; set; }
    }
}
