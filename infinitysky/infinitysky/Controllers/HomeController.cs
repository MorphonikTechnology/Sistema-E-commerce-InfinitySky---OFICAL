using infinitysky.CarrinhoCompra;
using infinitysky.Models;
using infinitysky.Repository;
using InfinitySky.Libraries.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace infinitysky.Controllers
{
    public class HomeController : Controller
    {

        
        private readonly ILogger<HomeController> _logger;

        // Interfaces para cliente e login 
        private IClienteRepositorio? _clienteRepositorio;
        private LoginCliente _loginCliente;

        //interface para planos
        private IPlanosRepositorio _planosRepositorio;

        //interface para pais
        private IPaisRepositorio _paisRepositorio;
        private ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IFavoritosRepositorio _favoritosRepositorio;

        //Desativar e Ativar da AreaAdm 
        private static List<long> planosDesativados = new List<long>();

        //interface para o pagamento
        private IPagamentoRepositorio _pagamentoRepositorio;

        //interface para a nota fiscal, o registro da compra em si
        private INotaFiscalRepositorio _notafiscalRepositorio;



        //declara��o para os cookies
        private CookieCarrinhoCompra _cookieCarrinhoCompra;
        private CookieFavoritos _cookieFavoritos;


        public HomeController(ILogger<HomeController> logger, IClienteRepositorio clienteRepositorio, LoginCliente loginCliente, IPlanosRepositorio planosRepositorio, IPaisRepositorio paisRepositorio, CookieCarrinhoCompra cookieCarrinhoCompra, CookieFavoritos cookieFavoritos, ICarrinhoRepositorio carrinhoRepositorio, IFavoritosRepositorio favoritosRepositorio, IPagamentoRepositorio pagamentoRepositorio, INotaFiscalRepositorio notaFiscalRepositorio)
        {
            _logger = logger;
            _clienteRepositorio = clienteRepositorio;
            _loginCliente = loginCliente;
            _planosRepositorio = planosRepositorio;
            _paisRepositorio = paisRepositorio;
            _carrinhoRepositorio = carrinhoRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _cookieFavoritos = cookieFavoritos;
            _pagamentoRepositorio = pagamentoRepositorio;
            _notafiscalRepositorio = notaFiscalRepositorio;
        }

        //�REA DE APRESENTA��O

        //TELA INICIAL - POSSUI M�TODOS DOS PLANOS E APRESENTA��O SOBRE O QUE A INFINITYSKY OFERECE
        public IActionResult Index(int IdPlano)
        {

            var tresPrimeirosPlanos = _planosRepositorio.ObterPlanosAleatorios(1);
            var restantePlanos = _planosRepositorio.ObterRestante(3);
            ViewBag.PlanosDesativados = planosDesativados;


            //Chamando classe especifica com os nomes dos pa�ses
            var viewModel = new PlanosPrimeiraParte
            {
                TresPrimeirosPlanos = tresPrimeirosPlanos,
                RestantePlanos = restantePlanos
,            };

            return View(viewModel);

        }

        //TELA PLANOS - M�TODOS DE CHAMADA DOS PLANOS E PA�SES DE ACORDO COM O ID
        public IActionResult Plano(int idPais)
        {
            ViewBag.PlanosDesativados = planosDesativados;

            //Adicionando os Id's (mudar depois) -> precisa estar de acordo com o banco de dados 
            var planosCanada = _planosRepositorio.ObterPlanosPorPaisId(1);
            var planosPortugal = _planosRepositorio.ObterPlanosPorPaisId(2);
            var planosEstadosUnidos = _planosRepositorio.ObterPlanosPorPaisId(3);
            var planosArgentina = _planosRepositorio.ObterPlanosPorPaisId(4);
            var planosItalia = _planosRepositorio.ObterPlanosPorPaisId(5);
            var planosEspanha = _planosRepositorio.ObterPlanosPorPaisId(6);
            var planosAlemanha = _planosRepositorio.ObterPlanosPorPaisId(7);
            var planosAustralia = _planosRepositorio.ObterPlanosPorPaisId(8);
            var planosInglaterra = _planosRepositorio.ObterPlanosPorPaisId(9);
            var planosFranca = _planosRepositorio.ObterPlanosPorPaisId(10);
            var planosIrlanda = _planosRepositorio.ObterPlanosPorPaisId(11);
            var planosJapao = _planosRepositorio.ObterPlanosPorPaisId(12);
            var planosCoreiadoSul = _planosRepositorio.ObterPlanosPorPaisId(13);
            var canada = _paisRepositorio.ObterPais(1);
            var portugal = _paisRepositorio.ObterPais(2);
            var eua = _paisRepositorio.ObterPais(3);
            var argentina = _paisRepositorio.ObterPais(4);
            var italia = _paisRepositorio.ObterPais(5);
            var espanha = _paisRepositorio.ObterPais(6);
            var alemanha = _paisRepositorio.ObterPais(7);
            var australia = _paisRepositorio.ObterPais(8);
            var inglaterra = _paisRepositorio.ObterPais(9);
            var franca = _paisRepositorio.ObterPais(10);
            var irlanda = _paisRepositorio.ObterPais(11);
            var japao = _paisRepositorio.ObterPais(12);
            var coreia = _paisRepositorio.ObterPais(12);

            //Chamando classe especifica com os nomes dos pa�ses
            //Primeiro o nome da Ienuberable
            //Depois chama as vari�vels criadas acima que 
            // As variaveis est�o armazenando o Metodo criado (planorepositorio) e o Id de cada pais
            var viewModel = new PlanosPorPaisViewModel
            {
                PlanosCanad� = planosCanada,
                PlanosPortugal = planosPortugal,
                PlanosEstadosUnidos = planosEstadosUnidos,
                PlanosArgentina = planosArgentina,
                PlanosItalia = planosItalia,
                PlanosEspanha = planosEspanha,
                PlanosAlemanha = planosAlemanha,
                PlanosAustr�lia = planosAustralia,
                PlanosInglaterra = planosInglaterra,
                PlanosFran�a = planosFranca,
                PlanosIrlanda = planosIrlanda,
                PlanosJap�o = planosJapao,
                PlanosCoreiadoSul = planosCoreiadoSul,
                Canada = canada,
                Portugal = portugal,
                EUA = eua,
                Argentina = argentina,
                Italia = italia,
                Espanha = espanha,
                Alemanha = alemanha,
                Australia = australia,
                Inglaterra = inglaterra,
                Franca = franca,
                Irlanda = irlanda,
                Japao = japao,
                Coreia = coreia,

            };

            return View(viewModel);

        }


        //M�TODO BUSCAR - BARRA DE PESQUISA
        public IActionResult Buscar(string searchTerm)
        {
            // Caso o usu�rio n�o tenha digitado nada, retornar todos os planos ou uma lista vazia
            var planos = string.IsNullOrEmpty(searchTerm)
                ? new List<Planos>() // Lista vazia se nada foi digitado
                : _planosRepositorio.PesquisaPlanosPorNome(searchTerm).ToList(); 

            var viewModel = new PlanosPorPaisViewModel
            {
                Planos = planos 
            };
            return View("ResultadosBusca", viewModel);
        }

        //TELA DETALHES - DETALHES DE CADA PA�S, AO CLICAR NO PA�S NA TELA PLANOS
        public IActionResult Detalhes(int Id)
        {
            var pais = _paisRepositorio.ObterPais(Id);

            if (pais == null) 
            {
                return NotFound();
            }
            var planos = _planosRepositorio.ObterPlanosPorPaisId(Id);
            var viewModel = new PaisesDetalhadosViewModel
            {
                Pais = pais,
                Planos = (List<Planos>)planos
            };

            return View(viewModel);
        }

        //TELA SOBRE N�S - INFORMA��ES SOBRE A INFINITYSKY
        public IActionResult Sobre()
        {
            return View();
        }


        //�REA CLIENTE - LOGIN, CADASTRO E RESTRI��ES
  
        //TELA LOGIN 
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha, string returnUrl = null)
        {
            var cliente = _clienteRepositorio.Login(email, senha);

            // Verifica se o cliente existe
            if (cliente == null || cliente.Email == null || cliente.Senha == null)
            {
                // se nao Retorna mensagem de erro
                ViewData["msg"] = "Dados errados";
                return View();
            }

            //verifica��o na sess�o, sess�o adm
            HttpContext.Session.SetInt32("ClienteId", cliente.Codigo);
            if (cliente.Email == "adm@gmail.com" && cliente.Senha == "adm123")
            {
                // Redireciona para a �rea do administrador
                return RedirectToAction(nameof(AreaAdm));
            }

            HttpContext.Session.SetInt32("ClienteId", cliente.Codigo);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("PainelCliente");
        }


        //TELA PAINEL CLIENTE - FUNCIONALIDADES PARA O CLIENTE + CHAMADA ATRAV�S DO ID
        // Painel Cliente 
        // Onde est�o os bot�es 
        // Nessa p�gina aparece somente o nome do cliente atraves do id 
        public IActionResult PainelCliente()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId.HasValue)
            {
                Cliente cliente = _clienteRepositorio.ObterClientePorId(clienteId.Value);
                //Utilizando uma ViewData 
                // "Chama-la" na view
                ViewData["NomeCliente"] = cliente.Nome;
                return View();
            }

            return RedirectToAction("Login");
        }

        //TELA DADOS DO CLIENTE - SUAS RESPECTIVAS INFORMA��ES CADASTRADAS
        // Tela que vai aparecer os dados do cliente
        // Precisa chamar o m�todo que faz o select de acordo com o id 
        // No ClienteRepositorio
        public IActionResult DadosCliente()
        {
            // classe Sessao para consultar o ID do cliente
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId.HasValue)
            {
                var cliente = _clienteRepositorio.DadosCliente(clienteId.Value);
                return View(cliente); // Passa o cliente para a View
            }

            // Se o cliente n�o estiver logado, redireciona para a p�gina de login
            // Apenas se for o caso, pois h� uma verifica��o no login 
            return RedirectToAction("Login");
        }

        //TELA ATUALIZAR DADOS - ATUALIZAR O QUE FOI CADASTRADO
        // Tela onde o usu�rio vai poder atualizar seus dados 
        // Passando o m�todo que obtem o cliente por meio do seu id 
        public IActionResult AtualizarDados()
        {
            //Criando a vari�vel para passar o id do cliente 
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId.HasValue)
            {
                Cliente cliente = _clienteRepositorio.ObterClientePorId(clienteId.Value);
                if (cliente == null)
                {
                    return RedirectToAction("Login");
                }
                return View(cliente); // Carregar todos os dados do cliente
            }

            return RedirectToAction("Login");
        }


        //Tela do Atualizar Dados (Envio de forms) 
        //Passando o m�todo que atualiza 
        // Colocando algumas verifica��es 
        // TempData para exibir 

        [HttpPost]
        public JsonResult AtualizarDados(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                int? clienteId = HttpContext.Session.GetInt32("ClienteId");

                if (clienteId.HasValue && cliente.Codigo == clienteId.Value)
                {
                    _clienteRepositorio.AtualizarCliente(cliente);

                    // Atualizar o nome na sess�o ap�s salvar
                    HttpContext.Session.SetString("Nome", cliente.Nome);

                    return Json(new { success = true, message = "Dados atualizados com sucesso!" });
                }
            }

            return Json(new { success = false, message = "Erro ao atualizar os dados. Por favor, verifique os campos." });
        }


        //M�TODO SAIR - SAI DA SESS�O
        // N�o � uma tela, mas chama o m�todo 
        // Essa "view" esta na "sair" da navbar e no "Sair" do painel cliente
        // Para o usu�rio se deslogar 
        public IActionResult Sair()
        {
            HttpContext.Session.Clear(); // Removendo as vari�veis de sess�o
            return RedirectToAction("Index", "Home");
            // Ap�s remover redireciona para a primeira pag

        }

        //TELA DE CADASTRO DO CLIENTE 
        public IActionResult CadastrarCliente()
        {
            return View();
        }

        //Vai vir como se fosse uma copia 
        //inicialmente com erro -> ligar com a model Cliente -> erro sai 
        // TODAS AS MODELS COM LETRAS MAIUSCULAS 
        //QUANDO FOR INSTANCIAR COMO COM LETRA minuscula


        [HttpPost]
        public IActionResult CadastrarCliente(Cliente cliente) //criando uma inst�ncia 
        {
            //M�TODO CADASTRAR

            //Para priorizar
            _clienteRepositorio.Cadastrar(cliente); //pegando a instancia de cima 

            return RedirectToAction(nameof(PainelCliente)); //nameoff-> variavel que busca alguma coisa
        }


        //�REA ADMINISTRATIVA

        //TELA ADMINISTRADOR - CONTROLA A INTERA��O DOS PLANOS
        public IActionResult AreaAdm()
        {
            return View();
        }

        //M�TODO DA LISTA QUE CHAMA OS PLANOS
        public IActionResult ListaAtualizar()
        {
            var planos = _planosRepositorio.ObterTodosPlanos();
            return View(planos);
        }

        //TELA DE ATUALIZAR PLANOS - UPDATE
        public IActionResult AtualizarPlano(long id)
        {
            // Carregar o plano existente
            var plano = _planosRepositorio.ObterPlano(id);
            if (plano == null)
            {
                return NotFound();
            }

            //  view model com o plano e lista de pa�ses
            var model = new IncluirPlanoViewModel
            {
                Planos = plano,
                Paises = _paisRepositorio.ObterTodosPaises()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AtualizarPlano(IncluirPlanoViewModel model)
        {
            try
            {
                // Atualiza o plano
                _planosRepositorio.Atualizar(model.Planos, model.Planos.image_plano);

                // Retorna sucesso como resposta JSON
                return Json(new { success = true, message = "Plano atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // Em caso de erro, retorna uma resposta de erro
                return Json(new { success = false, message = "Erro ao atualizar o plano. Tente novamente." });
            }

            // Recarrega os pa�ses para a view caso  ocorra erro
            model.Paises = _paisRepositorio.ObterTodosPaises();
            return View(model);
        }

        //TELA INCLUIR PLANOS - INCLUIR UM NOVO 
        public IActionResult IncluirPlano()
        {
            // Carregar a lista de pa�ses para o formul�rio de cadastro de planos
            var viewModel = new IncluirPlanoViewModel
            {
                Paises = _paisRepositorio.ObterTodosPaises(),
                Planos = new Planos() // Inicializa um novo plano vazio
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult IncluirPlano(IncluirPlanoViewModel viewModel)
        {
            // Tente ignorar a valida��o por enquanto
            try
            {
                _planosRepositorio.Adicionar(viewModel.Planos);
                // Retorna sucesso como resposta JSON
                return Json(new { success = true, message = "Plano incluso com sucesso!" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes (optional)
                Console.WriteLine(ex.Message); // Or use a logger

                // Em caso de erro, retorna uma resposta de erro
                return Json(new { success = false, message = "Erro ao incluir o plano. Tente novamente." });
            }


        }

        //TELA DESATIVAR/ATIVAR PLANO - PARA QUE ELE N�O FIQUE MAIS DISPON�VEL
        //View Desativar plano
        [HttpGet]
        public IActionResult DesativarPlano()
        {
            // todos os planos
            var planos = _planosRepositorio.ObterTodosPlanos();

            ViewBag.PlanosDesativados = planosDesativados;
            // Carrega os dados para a view
            return View(planos);
        }

        // M�todo para desativar o plano (adiciona o plano a lista de desativados)
        [HttpPost]
        public JsonResult DesativarPlano(long idPlano)
        {
            if (!planosDesativados.Contains(idPlano)) // verifica se o id esta presente na listaa 
            {
                planosDesativados.Add(idPlano); // Adiciona o plano a lista de desativados, pelo id 
            }

            return Json(new { success = true });
        }

        // ativar o plano (remove o plano da lista de desativados)
        [HttpPost]
        public JsonResult AtivarPlano(long idPlano)
        {
            if (planosDesativados.Contains(idPlano)) //  Constains (como uma lista) 
            {
                planosDesativados.Remove(idPlano); // Remove o plano da lista de desativados
            }

            return Json(new { success = true });
        }


        //�REA DE INTERA��O E FUNCIONALIDADES

        //TELA CARRINHO - COM VERIFICA��O DO LOGIN, M�TODOS (DIMINUIR, ADICIONAR E REMOVER ITENS)
        public IActionResult Carrinho()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId"); // id do cliente (recupera)
            // Obt�m os itens do carrinho associados ao cliente
            var itensCarrinho = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value);
            var carrinhoViewModel = itensCarrinho.Select(item =>
            {
                var plano = _planosRepositorio.ObterPlano(item.IdPlano); // Obt�m o plano pelo Id
                return new CarrinhoViewModel
                {
                    IdCarrinho = item.IdCarrinho,
                    ItensCarrinho = item.ItensCarrinho,
                    ValorTotalCarrinho = item.ValorTotalCarrinho,
                    IdPlano = item.IdPlano,
                    Nome = plano?.Nome,
                    HospedagemPlano = plano?.HospedagemPlano,
                    CursoPlano = plano?.CursoPlano,
                    InstituicaoPlano = plano?.InstituicaoPlano,
                    PeriodoPlano = plano?.PeriodoPlano,
                    ParcelaPlano = plano?.ParcelaPlano,
                    Valor = plano?.Valor ?? 0,
                    image_plano = plano?.image_plano
                };
            }).ToList();

            return View(carrinhoViewModel);
        }

        //M�TODO DE ADICIONAR AO CARRINHO
        [HttpPost]
        public IActionResult AdicionarItem(int Id)
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId"); // Recuperando Id do usu�rio

            // Verificar se o cliente est� logado
            if (clienteId == null)
            {
                return Json(new { success = false, redirectToLogin = true, redirectUrl = Url.Action("Login", "Home") });
            }

            var plano = _planosRepositorio.ObterPlano(clienteId.Value); // Vari�vel para armazenar o plano a ser adicionado
            Cliente cliente = _clienteRepositorio.ObterClientePorId(clienteId.Value); // Recupera o cliente pelo Id

            // Vari�vel para ver se o item j� existe no carrinho
            var existingItem = _carrinhoRepositorio.ObterItensCarrinho(cliente.Codigo).FirstOrDefault(i => i.IdPlano == Id);

            if (existingItem != null)
            {
                existingItem.ItensCarrinho += 1;
                existingItem.ValorTotalCarrinho = existingItem.ItensCarrinho * plano.Valor;
                _carrinhoRepositorio.AtualizarItem(existingItem);
            }
            else
            {
                var carrinho = new CarrinhoViewModel
                {
                    ItensCarrinho = 1,
                    ValorTotalCarrinho = plano.Valor,
                    IdPlano = Id,
                    IdCliente = cliente.Codigo
                };

                _carrinhoRepositorio.AdicionarItem(carrinho);
            }

            return Json(new { success = true, message = "Adicionado com sucesso ao seu Carrinho." });
        }

        //M�TODO DE REMOVER DO CARRINHO
        [HttpPost]
        // Praticamente a mesma logica do adicionar porem ao contr�rio 
        public IActionResult RemoverItem(int Id)
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");
            var item = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value).FirstOrDefault(i => i.IdPlano == Id);


            // Variavel de cima, se existir e for mair que 1 ele pode diminuir 
            // se for 1 ele nao vai diminuir, entra na quest�a do desativar no html
            // Se for mairo que 1 ele vai diminuir 
            if (item != null)
            {
                if (item.ItensCarrinho > 1)
                {
                    item.ItensCarrinho -= 1; // para diminuir 
                    item.ValorTotalCarrinho = item.ItensCarrinho * item.ValorTotalCarrinho / (item.ItensCarrinho + 1); // Corrigido o c�lculo para o novo subtotal
                    _carrinhoRepositorio.AtualizarItem(item);
                }
                else  // mas se for menor que 1 vai remover, casos mas extremos -> se por algum motivo o desabilitar n�o der certo 
                {
                    _carrinhoRepositorio.RemoverItem(item.IdCarrinho);
                }
                return Json(new { success = true, message = "Quantidade atualizada com sucesso." });
            }
            else
            {
                return Json(new { success = false, message = "O item n�o foi encontrado no carrinho." });
            }
        }

        //M�TODO DELETAR DO CARRINHO
        // m�todo pra deletar por completo o carrinho por id 
        [HttpPost]
        public IActionResult DeletarCarrinho(int idCarrinho)
        {
            _carrinhoRepositorio.DeletarCarrinhoPorId(idCarrinho);
            return Json(new { success = true }); // Retorna sucesso como json  para o AJAX no html 
        }


        //TELA FAVORITOS - COM VERIFICA��O DE LOGIN DE ACORDO COM OS DIAGRAMAS
        public IActionResult Favoritos()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId"); // Recuperando Id do usuario 

            var favoritos = _favoritosRepositorio.ObterFavoritosPorCliente(clienteId.Value); // variavel para armazenar a lista de favoritos 

            // Recupera os detalhes dos planos e cria uma vers�o ViewModel para ser exibida na tela dos "Favoritos" 

            var favoritosViewModel = favoritos.Select(favorito =>
            {
                // armazenado os planos obtidos no metodo 
                var plano = _planosRepositorio.ObterPlano(favorito.IdPlano);

                return new FavoritosViewModel  // Retorna uma  ViewModel preenchida com os dados do favorito e do plano juntos 
                {
                    IdFavorito = favorito.IdFavorito,
                    StatusFavorito = favorito.StatusFavorito,
                    IdPlano = favorito.IdPlano,
                    Nome = plano?.Nome,
                    HospedagemPlano = plano?.HospedagemPlano,
                    CursoPlano = plano?.CursoPlano,
                    InstituicaoPlano = plano?.InstituicaoPlano,
                    PeriodoPlano = plano?.PeriodoPlano,
                    ParcelaPlano = plano?.ParcelaPlano,
                    Valor = plano?.Valor ?? 0,
                    image_plano = plano?.image_plano,
                    IdCliente = clienteId.Value
                };
            }).ToList();

            return View(favoritosViewModel); //retornando -> n�o esquecer 
        }

        //M�TODO ADICIONAR AOS FAVORITOS
        // Parte do adicionar um favorito 
        [HttpPost]
        public IActionResult AdicionarFavorito(int Id)
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            // Verificar se o cliente est� logado
            if (clienteId == null)
            {
                return Json(new { success = false, redirectToLogin = true, redirectUrl = Url.Action("Login", "Home") });
            }

            // Obt�m o plano e verifica se ele existe
            var plano = _planosRepositorio.ObterPlano(Id);
            if (plano == null)
            {
                return Json(new { success = false, message = "Plano n�o encontrado" });
            }

            // Verifica se o plano j� est� nos favoritos
            var existingFavorite = _favoritosRepositorio.ObterFavorito(clienteId.Value, Id);
            if (existingFavorite == null)
            {
                var favorito = new FavoritosViewModel
                {
                    StatusFavorito = "Ativo",
                    IdPlano = Id,
                    IdCliente = clienteId.Value
                };

                _favoritosRepositorio.AdicionarFavorito(favorito);
            }

            return Json(new { success = true, message = "Plano adicionado aos seus favoritos com sucesso." });
        }



        //M�TODO DELETAR DOS FAVORITOS
        // Para deletar o Favorito 
        [HttpPost]
        public IActionResult RemoverFavorito(int idFavorito)
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            _favoritosRepositorio.RemoverFavorito(idFavorito); // executa o m�todo 
            return Json(new { success = true }); // Retorna sucesso como json para o ajaX no html 
        }


        //�REA DE PAGAMENTO - TELA PRINCIPAL + VIEWS COMO FORMA DE PAGAMENTO, INSER��O NO BANCO E SUAS ATUALIZA��ES

        //M�TODO COMPRAR - CHAMADA NO BOT�O
        [HttpPost]
        public IActionResult Comprar(string formaPagamento)
        {
            // Recupera o cliente logado pela sess�o
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            // Obt�m todos os carrinhos do cliente logado
            var carrinhos = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value).ToList();

            if (!carrinhos.Any())
            {
                return BadRequest("Nenhum carrinho encontrado para o cliente.");
            }
            foreach (var carrinho in carrinhos)
            {
                // Cria o pagamento associado ao carrinho
                var pagamento = new Pagamento
                {
                    FormaPagamento = "Indefinido",
                    StatusPagamentos = "Pendente",
                    HoraPagamento = DateTime.Now.TimeOfDay,
                    IdCarrinho = carrinho.IdCarrinho,
                    ValorPagamento = carrinho.ValorTotalCarrinho
                };

                // Salva o pagamento no reposit�rio
                _pagamentoRepositorio.Adicionar(pagamento);

            }
            return RedirectToAction("FormaCartao", "Home");
        }


        //M�TODO FINALIZAR PAGAMENTO - CHAMADA NO BOT�O TAMB�M
        [HttpPost]
        public IActionResult FinalizarPagamento(string formaPagamento)
        {
            // Recupera o cliente logado pela sess�o
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            // Obt�m todos os carrinhos do cliente logado
            var carrinhos = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value).ToList();

            if (!carrinhos.Any())
            {
                return BadRequest("Nenhum carrinho encontrado para o cliente.");
            }

            foreach (var carrinho in carrinhos)
            {
                var pagamento = _pagamentoRepositorio.ObterPagamentoPorCarrinhoId(carrinho.IdCarrinho);

                if (pagamento == null)
                {
                    continue; 
                }

                // Atualiza o pagamento para "Finalizado"
                pagamento.StatusPagamentos = "Finalizado";
                pagamento.FormaPagamento = formaPagamento;
                _pagamentoRepositorio.Atualizar(pagamento);

                // Cria uma nova nota fiscal para o pagamento
                var notaFiscal = new NotaFiscal
                {
                    ValorNF = pagamento.ValorPagamento,
                    DataEmissao = DateTime.Now.Date,
                    HoraEmissao = DateTime.Now.TimeOfDay,
                    IdPagamento = pagamento.IdPagamento // Relaciona ao pagamento finalizado
                };

                // Registra a nota fiscal no banco
                _notafiscalRepositorio.AdicionarNotaFiscal(notaFiscal);
            }

            // Redireciona para uma p�gina de confirma��o
            return RedirectToAction("TipoPagamento", "Home");
        }



        //TELA COMPRAS - AONDE AS COMPRAS ESTAR�O REGISTRADAS/ EM VIZUALIZA��O (PENDENTE OU N�O)
        public IActionResult Compras()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId");

            // Obt�m todos os carrinhos do cliente logado
            var carrinhos = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value);

            if (!carrinhos.Any())
            {
                ViewBag.Mensagem = "Nenhuma compra encontrada.";
                return View(new List<CompraViewModel>()); // Retorna uma lista vazia
            }

            // Mapeia os carrinhos e seus pagamentos associados para o ViewModel
            var compras = carrinhos.Select(carrinho =>
            {
                var pagamento = _pagamentoRepositorio.ObterPagamentoPorCarrinhoId(carrinho.IdCarrinho);
                var plano = _planosRepositorio.ObterPlano(carrinho.IdPlano);

                return new CompraViewModel
                {
                    NomePlano = plano?.Nome,
                    DescricaoPlano = plano?.DescricaoPlano,
                    image_plano = plano?.image_plano,
                    ValorPlano = plano?.Valor ?? 0,
                    StatusPagamento = pagamento?.StatusPagamentos,
                    FormaPagamento = pagamento?.FormaPagamento
                };
            }).ToList();

            return View(compras);
        }

        //TELA FORMA CART�O
        public IActionResult FormaCartao()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId"); // id do cliente (recupera)
            // Obt�m os itens do carrinho associados ao cliente
            var itensCarrinho = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value);
            var carrinhoViewModel = itensCarrinho.Select(item =>
            {
                var plano = _planosRepositorio.ObterPlano(item.IdPlano); // Obt�m o plano pelo Id
                return new CarrinhoViewModel
                {
                    IdCarrinho = item.IdCarrinho,
                    ItensCarrinho = item.ItensCarrinho,
                    ValorTotalCarrinho = item.ValorTotalCarrinho,
                    IdPlano = item.IdPlano,
                    Nome = plano?.Nome,
                    HospedagemPlano = plano?.HospedagemPlano,
                    CursoPlano = plano?.CursoPlano,
                    InstituicaoPlano = plano?.InstituicaoPlano,
                    PeriodoPlano = plano?.PeriodoPlano,
                    ParcelaPlano = plano?.ParcelaPlano,
                    Valor = plano?.Valor ?? 0,
                    image_plano = plano?.image_plano
                };
            }).ToList();

            return View(carrinhoViewModel);
        }

        //TELA FORMA PIX
        public IActionResult FormaPix()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId"); // id do cliente (recupera)
            // Obt�m os itens do carrinho associados ao cliente
            var itensCarrinho = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value);
            var carrinhoViewModel = itensCarrinho.Select(item =>
            {
                var plano = _planosRepositorio.ObterPlano(item.IdPlano); // Obt�m o plano pelo Id
                return new CarrinhoViewModel
                {
                    IdCarrinho = item.IdCarrinho,
                    ItensCarrinho = item.ItensCarrinho,
                    ValorTotalCarrinho = item.ValorTotalCarrinho,
                    IdPlano = item.IdPlano,
                    Nome = plano?.Nome,
                    HospedagemPlano = plano?.HospedagemPlano,
                    CursoPlano = plano?.CursoPlano,
                    InstituicaoPlano = plano?.InstituicaoPlano,
                    PeriodoPlano = plano?.PeriodoPlano,
                    ParcelaPlano = plano?.ParcelaPlano,
                    Valor = plano?.Valor ?? 0,
                    image_plano = plano?.image_plano
                };
            }).ToList();

            return View(carrinhoViewModel);
        }

        //TELA FORMA BOLETO
        public IActionResult FormaBoleto()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId"); // id do cliente (recupera)
            // Obt�m os itens do carrinho associados ao cliente
            var itensCarrinho = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value);
            var carrinhoViewModel = itensCarrinho.Select(item =>
            {
                var plano = _planosRepositorio.ObterPlano(item.IdPlano); // Obt�m o plano pelo Id
                return new CarrinhoViewModel
                {
                    IdCarrinho = item.IdCarrinho,
                    ItensCarrinho = item.ItensCarrinho,
                    ValorTotalCarrinho = item.ValorTotalCarrinho,
                    IdPlano = item.IdPlano,
                    Nome = plano?.Nome,
                    HospedagemPlano = plano?.HospedagemPlano,
                    CursoPlano = plano?.CursoPlano,
                    InstituicaoPlano = plano?.InstituicaoPlano,
                    PeriodoPlano = plano?.PeriodoPlano,
                    ParcelaPlano = plano?.ParcelaPlano,
                    Valor = plano?.Valor ?? 0,
                    image_plano = plano?.image_plano
                };
            }).ToList();

            return View(carrinhoViewModel);
        }

        //TELA TIPO PAGAMENTO - CONFIRMA��O DO PAGAMENTO
        public IActionResult TipoPagamento()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId"); // id do cliente (recupera)
            // Obt�m os itens do carrinho associados ao cliente
            var itensCarrinho = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value);
            var carrinhoViewModel = itensCarrinho.Select(item =>
            {
                var plano = _planosRepositorio.ObterPlano(item.IdPlano); // Obt�m o plano pelo Id
                return new CarrinhoViewModel
                {
                    IdCarrinho = item.IdCarrinho,
                    ItensCarrinho = item.ItensCarrinho,
                    ValorTotalCarrinho = item.ValorTotalCarrinho,
                    IdPlano = item.IdPlano,
                    Nome = plano?.Nome,
                    HospedagemPlano = plano?.HospedagemPlano,
                    CursoPlano = plano?.CursoPlano,
                    InstituicaoPlano = plano?.InstituicaoPlano,
                    PeriodoPlano = plano?.PeriodoPlano,
                    ParcelaPlano = plano?.ParcelaPlano,
                    Valor = plano?.Valor ?? 0,
                    image_plano = plano?.image_plano
                };
            }).ToList();

            return View(carrinhoViewModel);
        }

        //M�TODO COMPRA - CHAMADA NAS VIEWS "FORMAS"
        public IActionResult Compra()
        {
            int? clienteId = HttpContext.Session.GetInt32("ClienteId"); // id do cliente (recupera)
            // Obt�m os itens do carrinho associados ao cliente
            var itensCarrinho = _carrinhoRepositorio.ObterItensCarrinho(clienteId.Value);
            var carrinhoViewModel = itensCarrinho.Select(item =>
            {
                var plano = _planosRepositorio.ObterPlano(item.IdPlano); // Obt�m o plano pelo Id
                return new CarrinhoViewModel
                {
                    IdCarrinho = item.IdCarrinho,
                    ItensCarrinho = item.ItensCarrinho,
                    ValorTotalCarrinho = item.ValorTotalCarrinho,
                    IdPlano = item.IdPlano,
                    Nome = plano?.Nome,
                    HospedagemPlano = plano?.HospedagemPlano,
                    CursoPlano = plano?.CursoPlano,
                    InstituicaoPlano = plano?.InstituicaoPlano,
                    PeriodoPlano = plano?.PeriodoPlano,
                    ParcelaPlano = plano?.ParcelaPlano,
                    Valor = plano?.Valor ?? 0,
                    image_plano = plano?.image_plano
                };
            }).ToList();

            return View(carrinhoViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
