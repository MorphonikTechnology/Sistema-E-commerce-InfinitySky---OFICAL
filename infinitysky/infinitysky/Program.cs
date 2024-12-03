using infinitysky.CarrinhoCompra;
using infinitysky.Repository;
using InfinitySky.Libraries.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adicionar HttpContextAccessor para acesso a informa��es da sess�o
builder.Services.AddHttpContextAccessor();

// Registrar interfaces e reposit�rios como servi�os
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IPlanosRepositorio, PlanosRepositorio>();
builder.Services.AddScoped<IPaisRepositorio, PaisRepositorio>();
builder.Services.AddScoped<ICarrinhoRepositorio, CarrinhoRepositorio>();
builder.Services.AddScoped<IFavoritosRepositorio, FavoritosRepositorio>();
builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();
builder.Services.AddScoped<INotaFiscalRepositorio, NotaFiscalRepositorio>(); // Adicionado reposit�rio de Nota Fiscal

// Adicionar bibliotecas auxiliares de sess�o e login
builder.Services.AddScoped<InfinitySky.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LoginCliente>();

// Adicionar CookieCarrinhoCompra e servi�os relacionados a carrinho e favoritos
builder.Services.AddScoped<CookieCarrinhoCompra>();
builder.Services.AddScoped<CookieFavoritos>();
builder.Services.AddScoped<infinitysky.Cookie.Cookie>();

// Configurar sess�o
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true; // Seguran�a do cookie
    options.Cookie.IsEssential = true; // Necess�rio para o funcionamento essencial do site
});

// Adicionar filtro global para autentica��o do cliente
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ClienteActionFilter>();
});

var app = builder.Build();

// Configurar o pipeline de tratamento de requisi��es HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // For�a o uso de HSTS em ambientes n�o de desenvolvimento
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Adicionar middleware de sess�o
app.UseSession();

app.UseAuthorization();

// Configura��o das rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
