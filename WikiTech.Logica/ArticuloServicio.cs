//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WikiTech.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net;

namespace WikiTech.Logica
{
    public interface IArticuloServicio
    {
        Task<List<Articulo>> ObtenerArticulos();

        Task<Articulo> BuscarArticulo(int id);

        Task<bool> GuardarArticulo(Articulo articulo, HttpContext sesion);

        Task<bool> EliminarArticulo(int id, HttpContext sesion);

        Task<List<Categoria>> ObtenerCategorias(HttpContext sesion);
    }

    public class ArticuloServicio : IArticuloServicio
    {
        public async Task<List<Articulo>> ObtenerArticulos()
        {
            List<Articulo> listaArticulos = new List<Articulo>();

            var endpoint = "http://microserviciowiki.somee.com/api/articulo";
            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true};

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json_res = await response.Content.ReadAsStringAsync();
                    var res = JsonSerializer.Deserialize<List<Articulo>>(json_res, options);
                    listaArticulos = res; 
                }
            }
            return listaArticulos;
        }


        public async Task<Articulo> BuscarArticulo(int id)
        {
            string endpoint = $"http://microserviciowiki.somee.com/api/articulo/{id}";
            Articulo buscado = null;

            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json_res = await response.Content.ReadAsStringAsync();
                    var res = JsonSerializer.Deserialize<Articulo>(json_res, options);
                    buscado = res;
                }
            }

            return buscado;

        }


        public async Task<bool> GuardarArticulo(Articulo articulo, HttpContext sesion)
        {
            string email = sesion.Session.GetString("email");
            string token = sesion.Session.GetString("token");

            articulo.IdColaborador = await BuscarColaboradorPorEmail(email, sesion);

            DateTime localDate = DateTime.Now;
            articulo.Fecha = localDate;

            bool guardado = false;
            string endpoint = "http://microserviciowiki.somee.com/api/articulo";

            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true};

            using(var HttpClient = new HttpClient())
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await HttpClient.PostAsJsonAsync(endpoint,articulo);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    guardado = true;
                }
            }
            return guardado;
        }

        public async Task<bool> EliminarArticulo(int id, HttpContext sesion)
        {
            string token = sesion.Session.GetString("token");

            bool eliminado = false;

            if(BuscarArticulo(id) != null)
            {
                string endpoint = $"http://microserviciowiki.somee.com/api/articulo/{id}";

                JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                using (var HttpClient = new HttpClient())
                {
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
                    var response = await HttpClient.DeleteAsync(endpoint);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        eliminado = true;
                    }
                }
            }
            return eliminado;
        }

        public async Task<List<Categoria>> ObtenerCategorias(HttpContext sesion)
        {
            string token = sesion.Session.GetString("token");

            List<Categoria> categorias = new List<Categoria>();

            var endpoint = "http://microserviciowiki.somee.com/api/articulo/categorias";
            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            using (var httpClient = new HttpClient())
            {

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    requestMessage.Headers.Add("Authorization", "Bearer " + token);
                    var resHeader = await httpClient.SendAsync(requestMessage);
                    var json_resHeader = await resHeader.Content.ReadAsStringAsync();
                    var responseHeader = JsonSerializer.Deserialize<List<Categoria>>(json_resHeader, options);
                    categorias = responseHeader;
                }
            }
            return categorias;
        }

        private async Task<int> BuscarColaboradorPorEmail(string email, HttpContext sesion)
        {
            string token = sesion.Session.GetString("token");

            string endpoint = $"http://microserviciowiki.somee.com/api/articulo/colaborador/{email}";
            int buscado = 0;

            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            using (var httpClient = new HttpClient())
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    requestMessage.Headers.Add("Authorization", "Bearer " + token);
                    var resHeader = await httpClient.SendAsync(requestMessage);
                    var json_resHeader = await resHeader.Content.ReadAsStringAsync();
                    var responseHeader = JsonSerializer.Deserialize<Colaborador>(json_resHeader, options);
                    buscado = responseHeader.IdColaborador;
                }
            }
            return buscado;
        }

    }
}
