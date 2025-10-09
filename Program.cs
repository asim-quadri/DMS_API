using Dms_Api.Repository;
using Dms_Api.Services;
using DmsApi.Repository;
using DmsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<ICountryServices, CountryService>();
builder.Services.AddScoped<IRegulationGroupService, RegulationGroupService>();
builder.Services.AddScoped<IIndustryService, IndustryService>();
builder.Services.AddScoped<IAccessServices, AccessServices>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<IOrganizationService, OrganisationService>();



// Add Repositorys
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAccessRepository, AccessRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IRegulationGroupRepository, RegulationGroupRepository>();
builder.Services.AddScoped<IIndustryRepository, IndustryRepository>();
builder.Services.AddScoped<ISqlHelperRepository, SqlHelperRepository>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganisationRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>();





var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                           .WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:57683/")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                      });

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();



app.Run();
