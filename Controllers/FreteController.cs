using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CotacaoFrete_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreteController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public FreteController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet("calcular")]
        public async Task<IActionResult> CalcularFrete([FromQuery] string cepOrigem, [FromQuery] string cepDestino, [FromQuery] double pesoKg)
        {
            var origem = await BuscarCepAsync(cepOrigem);
            var destino = await BuscarCepAsync(cepDestino);

            if (origem == null || destino == null || string.IsNullOrEmpty(origem.Uf) || string.IsNullOrEmpty(destino.Uf))
                return BadRequest("CEP inválido");

            double distanciaKm = CalcularDistanciaSimulada(origem.Uf, destino.Uf);
            double precoPorKm = 1.5; // valor fictício por km
            double precoPorKg = 2.0; // valor fictício por quilo

            double valorTotal = distanciaKm * precoPorKm + pesoKg * precoPorKg;

            return Ok(new
            {
                DistanciaKm = distanciaKm,
                PesoKg = pesoKg,
                ValorFrete = valorTotal
            });
        }

        private async Task<CepResponse?> BuscarCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CepResponse>(content);
        }

        // Simulação simples baseada no estado (UF)
        private double CalcularDistanciaSimulada(string ufOrigem, string ufDestino)
        {
            if (ufOrigem == ufDestino) return 100;
            return 500;
        }
    }

    public class CepResponse
    {
        public string? Uf { get; set; }
    }
}
