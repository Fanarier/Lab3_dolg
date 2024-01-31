using Lab3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthOptions.AUDIENCE,
        ValidateLifetime = true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };
});

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
IServiceCollection serviceCollection = builder.Services.AddDbContext<ModelDB>(options => options.UseSqlServer(connection));
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapPost("/login", async (User loginData, ModelDB db) =>
{
    User? person = await db.Users!.FirstOrDefaultAsync(p => p.EMail == loginData.EMail &&
p.Password == loginData.Password);
    if (person is null) return Results.Unauthorized();
    var claims = new List<Claim> { new Claim(ClaimTypes.Email, person.EMail!) };
    var jwt = new JwtSecurityToken(issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.Now.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
    var encoderJWT = new JwtSecurityTokenHandler().WriteToken(jwt);
    var response = new
    {
        access_token = encoderJWT,
        username = person.EMail
    };
    return Results.Json(response);
});

app.MapGet("/api/percent", [Authorize] async (ModelDB db) => await db.Percents!.ToListAsync());
app.MapGet("/api/percent/{name}", [Authorize] async (ModelDB db, string name) => await db.Percents!.Where(u => u.DepositName == name).ToListAsync());
app.MapPost("/api/percent", [Authorize] async (Percent percent, ModelDB db) =>
{
    await db.Percents!.AddAsync(percent);
    await db.SaveChangesAsync();
    return percent;
});
app.MapPost("/api/investor", [Authorize] async (Investor investor, ModelDB db) =>
{
    await db.Investors!.AddAsync(investor);
    await db.SaveChangesAsync();
    return investor;
});
app.MapDelete("/api/percent/{id:int}", [Authorize] async (int id, ModelDB db) =>
{
    Percent? percent = await db.Percents!.FirstOrDefaultAsync(u => u.Id == id);
    if (percent == null) return Results.NotFound(new { message = "Percent не найден" });
    db.Percents!.Remove(percent);
    await db.SaveChangesAsync();
    return Results.Json(percent);
});
app.MapDelete("/api/investor/{id:int}", [Authorize] async (int id, ModelDB db) =>
{
    Investor? investor = await db.Investors!.FirstOrDefaultAsync(u => u.Id == id);
    if (investor == null) return Results.NotFound(new { message = "Investor не найден" });
    db.Investors!.Remove(investor);
    await db.SaveChangesAsync();
    return Results.Json(investor);
});
app.MapPut("/api/percent", [Authorize] async (Percent percent, ModelDB db) =>
{
    Percent? a = await db.Percents!.FirstOrDefaultAsync(u => u.Id == percent.Id);
    if (a == null) return Results.NotFound(new { message = "Ассортимент не найден" });
    percent.DepositNumber = a.DepositNumber;
    percent.DepositName = a.DepositName;
    percent.InterestRate = a.InterestRate;
    await db.SaveChangesAsync();
    return Results.Json(a);
});
app.MapPut("/api/investor", [Authorize] async (Investor investor, ModelDB db) =>
{
    Investor? reg = await db.Investors!.FirstOrDefaultAsync(u => u.Id == investor.Id);
    if (reg == null) return Results.NotFound(new { message = "Investor не найден" });
    reg.Id = investor.Id;
    reg.DepositNumber= investor.DepositNumber;
    reg.DepositName = investor.DepositName;
    reg.FullName = investor.FullName;
    reg.DepositAmount = investor.DepositAmount;
    reg.DepositDate = investor.DepositDate;
    reg.InterestRate = investor.InterestRate;
    reg.TotalAmount = investor.TotalAmount;
    await db.SaveChangesAsync();
    return Results.Json(reg);
});
app.Run();