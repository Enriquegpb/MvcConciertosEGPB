using MvcConciertosEGPB.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace MvcConciertosEGPB.Services
{
    public class ServiceApiConciertos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string urlApiConciertos;

        public ServiceApiConciertos(KeyModel model)
        {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.urlApiConciertos = model.UrlApi;
        }

       private async Task<T> CallApiAsync<T>(string request)
        {
          using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.urlApiConciertos + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);

                if(response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;

                }
                else
                {
                    return default(T);
                }
            }
        }
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            string request = "api/eventos/getcategorias";
            List<Categoria> categorias = await this.CallApiAsync<List<Categoria>>(request);
            return categorias;
        }
        public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "api/eventos";
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }

        public async Task<List<Evento>> GetEventosCategoriaAsync(int idcategoria)
        {
            string request = "api/eventos/geteventoscategoria/" +idcategoria;
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }

        public async Task NewEventoAsync(Evento evento)
        {
            using(HttpClient client = new HttpClient())
            {
                string request = "api/eventos";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                string jsonEvento = JsonConvert.SerializeObject(evento);
                StringContent content =
                    new StringContent(jsonEvento, Encoding.UTF8, "application/json");
                HttpResponseMessage repsonse =
                    await client.PostAsync(this.urlApiConciertos + request, content);
                    
            }
        }
    }
}
