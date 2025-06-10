using Microsoft.AspNetCore.Mvc;

namespace CotacaoFreteApi.Controllers;

[ApiController]
[Route("controller")]

public class CotacaoController : ControllerBase
{
    [HttpPost]

    public IActionResult CalcularFrete([FromBody] CotacaoRequest req)
    {
        double precoPorKm = 1.5;

        double distancia = CalcularDistancia(
            req.OrigemLat, req.OrigemLon,
            req.DestinoLat, req.DestinoLon
        );

        double valor = distancia * precoPorKm;


        return Ok(new
        {
            distancia = distancia.ToString("0.00"),
            valor = valor.ToString("0.00")

        });

    }

    //abaixo usei a formula de Haversine
    private double CalcularDistancia(double lat1, double lon1, double lat2, double lon2) //aqui 1 se refere ao remetente e 2 ao destinatario
    {
        const double R = 6371; //raio medio da Terra em km

        var dLat = (lat2 - lat1) * Math.PI / 180; //convertendo as diferenças de latitude e longitude de graus radianos
        var dLon = (lon2 - lon1) * Math.PI / 180;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * //medindo a curvatura entre dois pontos
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);


        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)); //calcula o angulo central da Terra

        return R * c; //multiplica o angulo pelo raio da Terra, retornando a distancia em km
    }
}

