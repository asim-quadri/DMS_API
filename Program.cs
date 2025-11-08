using ComplianceAPI.Helpers;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;
using ComplianceAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ComplianceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MasterConnection"));
});
// Add services to the container.
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEmail, Email>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<ICountryServices, CountryService>();
builder.Services.AddScoped<IRegulationGroupService, RegulationGroupService>();
builder.Services.AddScoped<IIndustryService, IndustryService>();
builder.Services.AddScoped<IAccessServices, AccessServices>();
builder.Services.AddScoped<IParameterService, ParameterService>();
builder.Services.AddScoped<IEntityTypeService, EntityTypeService>();
builder.Services.AddScoped<IRegulationSetupService, RegulationSetupService>();
builder.Services.AddScoped<IOrganizationServices, OrganizationService>();
builder.Services.AddScoped<IEntityService, EntityService>();
builder.Services.AddScoped<IComplianceTrackerService, ComplianceTrackerService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<ITOBService, TOBService>();
builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IRegulatoryAuthorityService, RegulatoryAuthorityService>();
builder.Services.AddScoped<IConcernedMinistryService, ConcernedMinistryService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();

builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IFolderService, FolderService>();

builder.Services.AddScoped<IDmsService, DmsService>();
// Add Repositorys
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAccessRepository, AccessRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IRegulationGroupRepository, RegulationGroupRepository>();
builder.Services.AddScoped<IIndustryRepository, IndustryRepository>();
builder.Services.AddScoped<ISqlHelperRepository, SqlHelperRepository>();
builder.Services.AddScoped<IParameterRepository, ParameterRepository>();
builder.Services.AddScoped<IEntityTypeRepository, EntityTypeRepository>();
builder.Services.AddScoped<IRegulationSetupRepository, RegulationSetupRepository>();
builder.Services.AddScoped<IHelperRepository, HelperRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>();
builder.Services.AddScoped<IComplianceTrackerRepository, ComplianceTrackerRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<ITOBRepository, TOBRepository>();
builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IHelperRepository, HelperRepository>();
builder.Services.AddScoped<IGetCoordinates, GetCoordinates>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRegulatoryAuthorityRepository, RegulatoryAuthorityRepository>();
builder.Services.AddScoped<IConcernedMinistryRepository, ConcernedMinistryRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddSingleton<CountriesFromJson>();

builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
// Add this using directive at the top of the file
//// Register AutoMapper with the assembly containing Program (AutoMapper 12.0.1+)
//builder.Services.AddAutoMapper(typeof(Program).Assembly);
//// Add this at the top of the file
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();