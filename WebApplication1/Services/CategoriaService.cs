﻿using System.Text;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class CategoriaService : ICategoriaService
    {
        private const string apiEndpoint = "/Categorias/";

        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        private CategoriaViewModel categoriaVM;
        private IEnumerable<CategoriaViewModel> categoriasVM;

        public CategoriaService(JsonSerializerOptions options, IHttpClientFactory clientFactory)
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _clientFactory = clientFactory;
        }
        public async Task<IEnumerable<CategoriaViewModel>> GetCategorias()
        {
            var client = _clientFactory.CreateClient("CategoriasApi"); // criar instancia do http client
            using (var response = await client.GetAsync(apiEndpoint))
            {
                if (response.IsSuccessStatusCode) //verifica se foi realizada com sucesso
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoriasVM = await JsonSerializer.DeserializeAsync<IEnumerable<CategoriaViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return categoriasVM;
        }

        public async Task<CategoriaViewModel> GetCategoriaPorId(int id)
        {
            var client = _clientFactory.CreateClient("CategoriasApi");
            using (var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoriaVM = await JsonSerializer.DeserializeAsync<CategoriaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return categoriaVM;
        }

        public async Task<CategoriaViewModel> CriaCategoria(CategoriaViewModel categoriaVM)
        {
            var client = _clientFactory.CreateClient("CategoriasApi");
            var categoria = JsonSerializer.Serialize(categoriaVM);
            StringContent content = new StringContent(categoria, Encoding.UTF8, "application/json");
            using (var response = await client.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoriaVM = await JsonSerializer.DeserializeAsync<CategoriaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return categoriaVM;

        }

        public async Task<bool> AtualizarCategoria(int id, CategoriaViewModel categoriaVM)
        {
            var client = _clientFactory.CreateClient("CategoriasApi");
            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, categoriaVM))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } 
        }

        public async Task<bool> DeletaCategoria(int id)
        {
            var client = _clientFactory.CreateClient("CategoriasApi");
            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
