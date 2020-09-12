using System.Collections.Generic;

namespace ClienteOData.Entidades
{
    public class Persona
    {
        public int Id { get; set; }
        public string NombreYApellido { get; set; }
        public int Edad { get; set; }

        public IEnumerable<Telefono> Telefono { get; set; }
        public IEnumerable<Direccion> Direccion { get; set; }
    }
}
