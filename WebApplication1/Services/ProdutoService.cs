using CategoriasMvc.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebApplication1.Models;

namespace CategoriasMvc.Services
{
    public class ProdutoService : IProdutoService
    {
        private const string apiEndpoint = "/Produtos/";

        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _clientFactory; 
        private ProdutoViewModel produtoVM;
        private IEnumerable<ProdutoViewModel> produtosVM;

        public ProdutoService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<ProdutoViewModel>> GetProdutos(string token)
        {
            var client = _clientFactory.CreateClient("ProdutosApi");
            PutTokenInHeaderAuthorization(token, client);
             
            using (var response = await client.GetAsync(apiEndpoint))
            {
                if (response.IsSuccessStatusCode) //verifica se foi realizada com sucesso
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    produtosVM = await JsonSerializer.DeserializeAsync<IEnumerable<ProdutoViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return produtosVM; 
        }

        public async Task<ProdutoViewModel> GetProdutoPorId(int id, string token)
        {
            var client = _clientFactory.CreateClient("ProdutosApi");
            PutTokenInHeaderAuthorization(token, client);

            try
            {
                using (var response = await client.GetAsync(apiEndpoint + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        var produtoVM = await JsonSerializer.DeserializeAsync<ProdutoViewModel>(apiResponse, _options);

                        if (produtoVM == null)
                        {
                            Console.WriteLine($"Falha ao desserializar o produto com ID {id}.");
                        }

                        return produtoVM;
                    }
                    else
                    {
                        Console.WriteLine($"Falha ao obter o produto com ID {id}. StatusCode: {response.StatusCode}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao obter o produto com ID {id}: {ex.Message}");
                return null;
            }
        }


        public async Task<ProdutoViewModel> CriaProduto(ProdutoViewModel produtoVM, string token)
        {
            var client = _clientFactory.CreateClient("ProdutosApi");
            PutTokenInHeaderAuthorization(token, client);

            var categoria = JsonSerializer.Serialize(produtoVM);
            StringContent content = new StringContent(categoria, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    produtoVM = await JsonSerializer.DeserializeAsync<ProdutoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return produtoVM;
        }

        public async Task<bool> AtualizarProduto(int id, ProdutoViewModel produtoVM, string token)
        {
            var client = _clientFactory.CreateClient("ProdutosApi");
            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, produtoVM))
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

        public async Task<bool> DeletaProduto(int id, string token)
        {
            var client = _clientFactory.CreateClient("ProdutosApi");
            PutTokenInHeaderAuthorization(token, client);

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

        private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
