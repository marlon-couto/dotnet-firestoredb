using ApiWeb.Dtos;
using ApiWeb.Helpers;
using ApiWeb.Models;
using ApiWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestaoController(IQuestaoService questaoService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarQuestao([FromBody] CriarQuestaoDto dto)
    {
        var novaQuestao = await questaoService.CriarQuestao(dto);
        var res = new RespostaDaApi<Questao>
        {
            Dados = novaQuestao, Mensagem = "Questão criada com sucesso.", Sucesso = true
        };

        return Created($"api/questao/{novaQuestao.Id}", res);
    }

    [HttpGet("buscar-texto/{termoDeBusca}")]
    public async Task<IActionResult> BuscarTextoEmAlternativas([FromRoute] string termoDeBusca
        , [FromQuery] int pagina = 1
        , [FromQuery(Name = "quantidade")] int quantidadePorPagina = 10)
    {
        var (questoesEncontradas, contagem)
            = await questaoService.BuscarTextoEmAlternativas(termoDeBusca, pagina, quantidadePorPagina);

        var res = new RespostaDaApiPaginada<List<Questao>>
        {
            Dados = questoesEncontradas
            , Mensagem = "Questões encontradas com sucesso."
            , Paginacao = new Paginacao
            {
                Pagina = pagina, QuantidadePorPagina = quantidadePorPagina, Total = contagem
            }
            , Sucesso = true
        };

        return Ok(res);
    }

    [HttpPut("{questaoId:guid}")]
    public async Task<IActionResult> AtualizarQuestao([FromRoute] Guid questaoId, [FromBody] AtualizarQuestaoDto dto)
    {
        var questaoAtualizada = await questaoService.AtualizarQuestao(questaoId, dto);
        var res = new RespostaDaApi<Questao>
        {
            Dados = questaoAtualizada, Mensagem = "Questão atualizada com sucesso.", Sucesso = true
        };

        return Ok(res);
    }
}