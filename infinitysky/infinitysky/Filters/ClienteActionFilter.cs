using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using infinitysky.Repository;



public class ClienteActionFilter : IActionFilter
{

    //Importando A interface Icliente repositório onde estão alguns métodos 
    //Obter Cliente do id 
    private readonly IClienteRepositorio _clienteRepositorio;

    public ClienteActionFilter(IClienteRepositorio clienteRepositorio)
    {
        _clienteRepositorio = clienteRepositorio;
    }

    // Este método será chamado antes de cada ação do controller ser executada
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext; //Contexto Http Atual
        int? clienteId = httpContext.Session.GetInt32("ClienteId"); // Pega o id do Cliente da sessão, quando ele se loga

        // Se o cliente estiver logado, verificando atraves do id
        // Pega nome e coloca no Context.Items
        if (clienteId.HasValue)
        {
            var cliente = _clienteRepositorio.ObterClientePorId(clienteId.Value);
            httpContext.Items["NomeCliente"] = cliente.Nome;
        }
    }

    //Chamando o método
    // Se não estiver não funciona (da erro) 
    public void OnActionExecuted(ActionExecutedContext context) { }
}