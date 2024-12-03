using infinitysky.CarrinhoCompra;
using infinitysky.Repository;
using InfinitySky.Libraries.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adicionar HttpContextAccessor para acesso a informações da sessão
builder.Services.AddHttpContextAccessor();

// Registrar interfaces e repositórios como serviços
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IPlanosRepositorio, PlanosRepositorio>();
builder.Services.AddScoped<IPaisRepositorio, PaisRepositorio>();
builder.Services.AddScoped<ICarrinhoRepositorio, CarrinhoRepositorio>();
builder.Services.AddScoped<IFavoritosRepositorio, FavoritosRepositorio>();
builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();
builder.Services.AddScoped<INotaFiscalRepositorio, NotaFiscalRepositorio>(); // Adicionado repositório de Nota Fiscal

// Adicionar bibliotecas auxiliares de sessão e login
builder.Services.AddScoped<InfinitySky.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LoginCliente>();

// Adicionar CookieCarrinhoCompra e serviços relacionados a carrinho e favoritos
builder.Services.AddScoped<CookieCarrinhoCompra>();
builder.Services.AddScoped<CookieFavoritos>();
builder.Services.AddScoped<infinitysky.Cookie.Cookie>();

// Configurar sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true; // Segurança do cookie
    options.Cookie.IsEssential = true; // Necessário para o funcionamento essencial do site
});

// Adicionar filtro global para autenticação do cliente
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ClienteActionFilter>();
});

var app = builder.Build();

// Configurar o pipeline de tratamento de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Força o uso de HSTS em ambientes não de desenvolvimento
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Adicionar middleware de sessão
app.UseSession();

app.UseAuthorization();

// Configuração das rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
