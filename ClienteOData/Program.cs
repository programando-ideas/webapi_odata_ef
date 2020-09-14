using ClienteOData.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ClienteOData
{
    class Program
    {
        const string URLBase = "http://localhost:5000";

        static void Main(string[] args)
        {

            ////////////// GET ////////////// 
            Console.WriteLine("Haciendo llamada GET...");
            Console.WriteLine("http://localhost:5000/odata/personas?$expand=telefono,direccion&$filter=Id eq 1");

            var task = Task.Run(async () =>
            {
                var personas = await GetPersonas();
                Console.WriteLine(JsonConvert.SerializeObject(personas, Formatting.Indented));
            });
            task.Wait();

            ////////////// POST ////////////// 
            Console.WriteLine("Haciendo llamada POST...");
            task = Task.Run(async () =>
            {
                await GuardarPersona(new Persona()
                {
                    NombreYApellido = "Juan Perez",
                    Edad = 23,
                    Telefono = new List<Telefono>()
                    {
                        new Telefono() { TelefonoDesc = "+528885555444" },
                        new Telefono() { TelefonoDesc = "+551234567890" }
                    },
                    Direccion = new List<Direccion>()
                    {
                        new Direccion() { DireccionDesc = "Calle 23" },
                        new Direccion() { DireccionDesc = "Avenida 28 #43 depto. 4" }
                    }
                });
            });
            task.Wait();

            Console.ReadLine();
        }

        private async static Task<IEnumerable<Persona>> GetPersonas()
        {
            try
            {

                // http://localhost:5000/odata/personas?$expand=telefono,direccion&$filter=Id eq 1
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(URLBase)
                };

                var uriBuilder = new UriBuilder(URLBase)
                {
                    Path = "odata/personas"
                };
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["$expand"] = "telefono,direccion";
                query["$filter"] = "Id eq 1";
                uriBuilder.Query = query.ToString();

                HttpResponseMessage respGET = await client.GetAsync(uriBuilder.ToString());
                if (!respGET.IsSuccessStatusCode)
                {
                    string error = "ERROR! " + respGET.StatusCode;
                    throw new Exception(error);
                }

                //var respString = await respGET.Content.ReadAsStringAsync();
                //var personas = JsonConvert.DeserializeObject<PersonaResponse>(respString);

                // The lines above can be replaced with this using
                // https://www.nuget.org/packages/Microsoft.AspNet.WebApi.Client/5.2.7
                // Microsoft.AspNet.WebApi.Client
                var personas = await respGET.Content.ReadAsAsync<PersonaResponse>();

                return personas.value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static async Task GuardarPersona(Persona persona)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(URLBase)
            };
            var uriBuilder = new UriBuilder(URLBase)
            {
                Path = "odata/personas"
            };

            IEnumerable<KeyValuePair<string, string>> parameters = new Dictionary<string, string>();
            parameters = parameters.Concat(persona.ToKeyValue());
            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);

            HttpResponseMessage responsePOST = await client.PostAsync(uriBuilder.ToString(), content);
            if (!responsePOST.IsSuccessStatusCode)
            {
                string error = "ERROR! " + responsePOST.StatusCode;
                throw new Exception(error);
            }

            var resp = await responsePOST.Content.ReadAsStringAsync();

            Console.WriteLine("Respuesta " + Environment.NewLine + resp);
        }
    }
}
