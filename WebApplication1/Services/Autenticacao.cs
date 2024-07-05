using CategoriasMvc.Models;
using System.Text;
using System.Text.Json;

namespace CategoriasMvc.Services
{
    public class Autenticacao : IAutenticacao
    {
        private readonly IHttpClientFactory _clientFactory;
        const string apiEndpointAutentica = "/Auth/login";

        private readonly JsonSerializerOptions _options;
        private TokenViewModel tokenUsuario;

        public Autenticacao(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<TokenViewModel> AutenticaUsuario(UsuarioViewModel usuarioVM)
        {
            try
            {
                var client = _clientFactory.CreateClient("AutenticaApi");

                var response = await client.PostAsJsonAsync(apiEndpointAutentica, usuarioVM);

                if (response.IsSuccessStatusCode)
                {
                    var contentType = response.Content.Headers.ContentType.ToString();
                    Console.WriteLine("Content-Type: " + contentType);

                    var apiResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Response: " + apiResponse);

                    var tokenUsuario = JsonSerializer.Deserialize<TokenViewModel>(apiResponse, _options);
                    return tokenUsuario;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Error Response: " + errorResponse);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao autenticar o usuário: {ex.Message}");
                throw;
            }
        }

    }
}
