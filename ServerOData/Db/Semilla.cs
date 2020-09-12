using odatawebapi.Entidades;
using System.Linq;

namespace odatawebapi.Db
{
    public static class Semilla
    {
        public static void GenerarDatos(PersonasDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Persona.Any())
            {
                return;
            }

            var personas = new Persona[]
            {
                new Persona() { NombreYApellido = "Jose Perez", Edad = 34,
                    Telefono = new Telefono[] {
                        new Telefono() { TelefonoDesc = "+574447474111" },
                        new Telefono() { TelefonoDesc = "+548887777777" },
                    },
                    Direccion = new Direccion[] {
                        new Direccion () { DireccionDesc = "Avenida 323 Piso 2 Depto. 4" }
                    }
                },
                new Persona() { NombreYApellido = "Jorge Garcia", Edad = 64,
                    Telefono = new Telefono[] {
                        new Telefono() { TelefonoDesc = "+511122331122" },
                        new Telefono() { TelefonoDesc = "+525656565656" },
                        new Telefono() { TelefonoDesc = "+565656565656" }
                    },
                    Direccion = new Direccion[] {
                        new Direccion () { DireccionDesc = "Calle 23" },
                        new Direccion () { DireccionDesc = "Avenida libertad 12" },
                        new Direccion () { DireccionDesc = "Calle 28 #445" }
                    }
                }
            };

            context.Persona.AddRange(personas);
            context.SaveChanges();
        }
    }
}

