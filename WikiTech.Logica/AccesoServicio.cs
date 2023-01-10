using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WikiTech.Entidades;

namespace WikiTech.Logica
{

    public interface IAccesoServicio
    {
        Task<string> LoginAsync(Usuario usuario);

        void Registrarse(Usuario usuario);
    }

    public class AccesoServicio : IAccesoServicio
    {   
        public async Task<string> LoginAsync(Usuario usuario)
        {   
            //url donde tenemos la api con el endpoint
            string urlApi = $"http://microserviciologin.somee.com/login/loginsesion";

            // httpClient para hacer peticiones
            using (var httpClient = new HttpClient())
            {
                // creo body para mandar a la api con metodo post
                var body = new
                {
                    user = usuario.email,
                    password = usuario.contrasenia
                };
                
                //obtengo la respuesta de la api
                var respuesta = await httpClient.PostAsJsonAsync(urlApi, body);
                // si no explota con un 500 prosigo
                if(respuesta.IsSuccessStatusCode)
                {   
                    // trato de "leer" la respuesta con promesa
                    var result = await respuesta.Content.ReadAsStringAsync();
                    // luego parseamos la respuesta q viene en formato
                    // json a una clase creada para manejar mejor
                    // los datos de la respuesta esta clase la llame "Respuesta" por que se me hizo bonito
                    var postResult = JsonSerializer.Deserialize<Respuesta>(result);
                    return postResult.result;
                } 
                else
                {
                    return "error fatal";
                }
            }
            
        }

        public async void Registrarse(Usuario usuario)
        {
            int statusCode = 0;

            string urlApi = $"http://microserviciologin.somee.com/login/registeruser";

            using (var httpClient = new HttpClient())
            {
                var body = new
                {
                    user= usuario.email,
                    fullname= usuario.nombre+" "+usuario.apellido,
                    password= usuario.contrasenia
                };
                var respuesta = await httpClient.PostAsJsonAsync(urlApi, body);
                //aca no me importa mucho la respuesta pero si quisiera usaria esa variable

                statusCode = (int)respuesta.StatusCode;

            }

            if(statusCode == 200)
            {
                await GuardarColaborador(usuario);
            }
            
        }

        public async Task<bool> GuardarColaborador(Usuario usuario)
        {
            bool guardado = false;

            Colaborador colaborador = new Colaborador();
            colaborador.Nombre = usuario.nombre;
            colaborador.Apellido = usuario.apellido;
            colaborador.Email = usuario.email;

            string endpoint = "http://microserviciowiki.somee.com/api/articulo/colaborador";

            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            using (var HttpClient = new HttpClient())
            {
                var response = await HttpClient.PostAsJsonAsync(endpoint, colaborador);
                if (response.IsSuccessStatusCode)
                {
                    guardado = true;
                }
            }

            return guardado;
        } 
    }
}
