using PedidosMvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// IMPORTANTE: os serviços devem ser adicionados na seguinte ordem
builder.Services.AdicionarConfiguracao();
builder.Services.AdicionarDadosEscopo();
builder.Services.AdicionarUnidadeDeTrabalho();
builder.Services.AdicionarRepositorio();
builder.Services.AdicionarServico();
string aspnetcoreUrls = builder.Configuration["ASPNETCORE_URLS"];
builder.WebHost.UseKestrel()
    .UseUrls(aspnetcoreUrls)
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseIISIntegration();

var app = builder.Build();

app.UseExceptionHandler("/Home/Erro");
// Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();