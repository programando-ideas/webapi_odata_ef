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
            
            Console.WriteLine("Haciendo llamada...");
            Console.WriteLine("http://localhost:5000/odata/personas?$expand=telefono,direccion&$filter=Id eq 1");

            var task = Task.Run(async () =>
            {
                var personas = await GetPersonas();
                Console.WriteLine(JsonConvert.SerializeObject(personas, Formatting.Indented));
            });

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

                var respString = await respGET.Content.ReadAsStringAsync();
                var personas = JsonConvert.DeserializeObject<PersonaResponse>(respString);

                return personas.value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
