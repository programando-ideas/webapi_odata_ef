using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using odatawebapi.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace odatawebapi.Controllers
{
    public class PersonasController : ODataController
    {
        private readonly PersonasDbContext _contex;
        public PersonasController(PersonasDbContext contex)
        {
            _contex = contex;
        }

        [EnableQuery]
        public IEnumerable<Persona> Get()
        {
            return _contex.Persona;
        }

        /// <summary>
        /// Permite crear una nueva persona
        /// </summary>
        /// <param name="parameters">Parametros enviados en formato JSON con los datos de la persona</param>
        /// <example>
        /// POST http://localhost:5000/odata/personas(1)/CrearPersona
        /// Body --> raw --> JSON
        /// {
        ///     "NombreYApellido": "Luciano",
        ///     "Edad": 23,
        ///     "Telefonos": [
        /// 	    { "TelefonoDesc": "+528885555444" },
        ///         { "TelefonoDesc": "+551234567890" }],
        ///     "Direcciones": [
        /// 	    { "DireccionDesc": "Calle 23"},
        /// 	    { "DireccionDesc": "Avenida 28 #43 depto. 4"}]
        ///  }
        /// </example>
        [HttpPost]
        public async Task<IActionResult> CrearPersona([FromODataUri] int key, ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pNombreYApellido = parameters["NombreYApellido"].ToString();
            var pEdad = Convert.ToInt32(parameters["Edad"].ToString());
            var telefonos = (IEnumerable<Telefono>)parameters["Telefonos"];
            var direcciones = (IEnumerable<Direccion>)parameters["Direcciones"];

            Persona persona = new Persona()
            {
                NombreYApellido = pNombreYApellido,
                Edad = pEdad,
                Telefono = new HashSet<Telefono>(telefonos),
                Direccion = new HashSet<Direccion>(direcciones)
            };

            _contex.Persona.Add(persona);
            await _contex.SaveChangesAsync();

            return Created(persona);
        }

        /// <summary>
        /// Otra forma de utilizar OData para crear una Persona
        /// </summary>
        /// <param name="persona">Datos de la persona a crear</param>
        /// <example>
        /// POST http://localhost:5000/odata/Personas
        /// Body --> x-www-form-urlencoded (key: value)
        ///     NombreYApellido: Nueva Persona
        ///     Edad: 23
        ///     Telefono[0].TelefonoDesc: +545555444477
        ///     Telefono[1].TelefonoDesc: +559879879879
        ///     Direccion[0].DireccionDesc: Avenida 293 #445
        /// </example>
        [HttpPost]
        [ODataRoute("Personas")]
        public async Task<IActionResult> CrearPersona2(Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _contex.Persona.Add(persona);
            await _contex.SaveChangesAsync();

            return Created(persona);
        }
    }
}