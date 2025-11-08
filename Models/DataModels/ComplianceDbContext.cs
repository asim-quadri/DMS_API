using Microsoft.EntityFrameworkCore;

namespace ComplianceAPI.Models.DataModels
{
    public class ComplianceDbContext : DbContext
    {
        public ComplianceDbContext(DbContextOptions<ComplianceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UsersHistory> UsersHistory { get; set; }

        public DbSet<RefRole> RefRoles { get; set; }

        public DbSet<UserApproval> UserApproval { get; set; }

        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }

        public DbSet<UserRoleMappingHistory> UserRoleMappingHistory { get; set; }

        public DbSet<RefApprovalStatus> RefApprovalStatus { get; set; }

        public DbSet<RefApprovalType> RefApprovalType { get; set; }

        public DbSet<UserProductMapping> UserProductMapping { get; set; }

        public DbSet<UserProductApproval> UserProductApproval { get; set; }

        public DbSet<RefProduct> RefProducts { get; set; }

        public DbSet<RefRolesHistory> RefRolesHistory { get; set; }

        public DbSet<RegulationSetupCompliance> RegulationSetupCompliance { get; set; }

        public DbSet<RegulationSetupComplianceParameter> RegulationSetupComplianceParameter { get; set; }

        public DbSet<RegulationSetupComplianceApproval> RegulationSetupComplianceApproval { get; set; }

        public DbSet<RegulationSetupComplianceHistory> RegulationSetupComplianceHistory { get; set; }

        public DbSet<RegSetupComplianceParameterHistory> RegSetupComplianceParameterHistory { get; set; }

        public DbSet<CountryStateMapping> CountryStateMapping { get; set; }

        public DbSet<IndustryMappings> IndustryrMapping { get; set; }

        public DbSet<RegulationGroupMapping> GetCountryRegulationGroupMapping { get; set; }

        public DbSet<Countries> Country { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<MajorIndustry> MajorIndustries { get; set; }

        public DbSet<MinorIndustry> MinorIndustries { get; set; }

        public DbSet<RegulationGrouping> RegulationGroups { get; set; }

        public DbSet<CountryEntityMappings> EntityTypeMapping { get; set; }

        public DbSet<EntityTypes> EntityType { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<OrganizationHistory> OrganizationHistory { get; set; }

        public DbSet<OrganizationApproval> OrganizationApproval { get; set; }

        public DbSet<TOCRegistration> TOCRegistration { get; set; }

        public DbSet<RegulationSetupCAAMappingHistory> RegulationSetupCAAMappingHistory { get; set; }
        public DbSet<TOCDocuments> TOCDocuments { get; set; }

        public DbSet<TOCDueDates> TOCDueDates { get; set; }

        public DbSet<TOCDues> TOCDues { get; set; }

        public DbSet<TOCImprisonment> TOCImprisonment { get; set; }

        public DbSet<TOCParameter> TOCParameter { get; set; }

        public DbSet<TOCIntrestPenality> TOCIntrestPenality { get; set; }

        public DbSet<TOCHistory> TOCHistory { get; set; }

        public DbSet<TOCApproval> TOCApproval { get; set; }

        public DbSet<Entity> Entity { get; set; }

        public DbSet<EntityHistory> EntityHistory { get; set; }

        public DbSet<EntityApproval> EntityApproval { get; set; }

        public DbSet<BillingDetails> BillingDetails { get; set; }

        public DbSet<BillingLevel> BillingLevel { get; set; }

        public DbSet<BillingFrequency> BillingFrequency { get; set; }

        public DbSet<ServiceProvider> ServiceProvider { get; set; }

        public DbSet<BillStatus> BillStatus { get; set; }

        public DbSet<DeliveryStatus> DeliveryStatus { get; set; }

        public DbSet<CurrencyCodes> CurrencyCodes { get; set; }

        public DbSet<CountryApproval> CountryApprovals { get; set; }

        public DbSet<CountryFileNames> countryFileNames { get; set; }

        public DbSet<CountryFileNamesApproval> CountryFileNamesApproval { get; set; }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        public DbSet<MajorModule> MajorModules { get; set; }

        public DbSet<MinorModule> MinorModules { get; set; }

        public DbSet<LevelMaster> LevelMasters { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<ClientUser> ClientUsers { get; set; }

        public DbSet<UserOrganization> UserOrganizations { get; set; }

        public DbSet<EntityTypeApproval> EntityTypeApproval { get; set; }
        public DbSet<Announcements> Announcements { get; set; }
        public DbSet<AnnouncementHistory> AnnouncementHistory { get; set; }
        public DbSet<AnnouncementApproval> AnnouncementApproval { get; set; }

        public DbSet<RegulationSetupLegalEntityType> RegulationSetupLegalEntityType { get; set; }

        public DbSet<RegulationSetupLegalEntityTypeHistory> RegulationSetupLegalEntityTypeHistory { get; set; }

        public DbSet<RegulatoryAuthorities> RegulatoryAuthority { get; set; }

        public DbSet<RegulatoryAuthorityApproval> RegulatoryAuthorityApproval { get; set; }

        public DbSet<CountryRegulatoryAuthorityMapping> CountryRegulatoryAuthorityMapping { get; set; }

        public DbSet<CountryRegulatoryAuthorityMappingApproval> CountryRegulatoryAuthorityMappingApproval { get; set; }

        public DbSet<ConcernedMinistry> ConcernedMinistries { get; set; }

        public DbSet<ConcernedMinistryApproval> ConcernedMinistryApproval { get; set; }

        public DbSet<CountryConcernedMinistryMapping> CountryConcernedMinistryMapping { get; set; }

        public DbSet<RegulationSetupCAAMapping> RegulationSetupCAAMapping { get; set; }
        public DbSet<CountryConcernedMinistryMappingApproval> CountryConcernedMinistryMappingApproval { get; set; }

        public DbSet<ReportMaster> ReportMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<UserApproval>()
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Parameters>().ToTable("Parameter", schema: "product_owner")
                 .Property(u => u.UID)
                 .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<ParameterApproval>().ToTable("ParameterApproval", schema: "product_owner")
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<ParameterHistory>().ToTable("ParameterHistory", schema: "product_owner")
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RegulationSetupDetails>().ToTable("RegulationSetupDetails", schema: "product_owner")
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RegulationSetupHistory>().ToTable("RegulationSetupHistory", schema: "product_owner")
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RegulationSetupApproval>().ToTable("RegulationSetupApproval", schema: "product_owner")
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RegulationSetupParameter>().ToTable("RegulationSetupParameter", schema: "product_owner")
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RegulationSetupParameterHistory>().ToTable("RegulationSetupParameterHistory", schema: "product_owner")
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RegulationSetupCompliance>().ToTable("RegulationSetupCompliance", schema: "product_owner")
                .Property(u => u.UID)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RegulationSetupComplianceParameter>().ToTable("RegulationSetupComplianceParameter", schema: "product_owner")
             .Property(u => u.UID)
             .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RegulationSetupComplianceApproval>().ToTable("RegulationSetupComplianceApproval", schema: "product_owner")
            .Property(u => u.UID)
            .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<RegulationSetupComplianceHistory>().ToTable("RegulationSetupComplianceHistory", schema: "product_owner")
           .Property(u => u.UID)
           .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<RegSetupComplianceParameterHistory>().ToTable("RegSetupComplianceParameterHistory", schema: "product_owner")
           .Property(u => u.UID)
           .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<CountryStateMapping>().ToTable("CountryStateMapping", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); 

            modelBuilder.Entity<IndustryMappings>().ToTable("IndustryMapping", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<RegulationGroupMapping>().ToTable("CountryRegulationGroupMapping", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<Countries>().ToTable("Country", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<State>().ToTable("State", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<MajorIndustry>().ToTable("MajorIndustry", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<MinorIndustry>().ToTable("MinorIndustry", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<RegulationGrouping>().ToTable("RegulationGroup", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<CountryEntityMappings>().ToTable("CountryEntityTypeMapping", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<EntityTypes>().ToTable("EntityType", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<EntityTypeApproval>().ToTable("EntityTypeApproval", schema: "product_owner").Property(u => u.UID)
              .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<TOCRegistration>().ToTable("TOCRegistration", schema: "product_owner")
              .Property(u => u.UID)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<TOCDocuments>().ToTable("TOCDocuments", schema: "product_owner")
                .Property(u => u.UID);

            modelBuilder.Entity<TOCDueDates>()
                .ToTable("TOCDueDates", schema: "product_owner")
              .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOCDues>()
                .ToTable("TOCDues", schema: "product_owner")
              .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOCImprisonment>()
                .ToTable("TOCImprisonment", schema: "product_owner")
              .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOCParameter>()
                .ToTable("TOCParameter", schema: "product_owner")
              .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOCIntrestPenality>()
                .ToTable("TOCIntrestPenality", schema: "product_owner")
              .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOCHistory>()
               .ToTable("TOCHistory", schema: "product_owner")
             .Property(u => u.UID);

            modelBuilder.Entity<TOCApproval>()
               .ToTable("TOCApproval", schema: "product_owner")
             .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOBHistory>()
                .ToTable("TOBHistory", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOBApproval>()
               .ToTable("TOBApproval", schema: "product_owner")
               .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOBDetails>()
                .ToTable("TOB", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOBMapping>()
                .ToTable("TOBMapping", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<TOBMappingApproval>()
                .ToTable("TOBMappingApproval", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql("NEWID()"); ;

            modelBuilder.Entity<Organization>().ToTable("Organizations", schema: "organizations");

            modelBuilder.Entity<OrganizationHistory>().ToTable("OrganizationHistory", schema: "organizations");

            modelBuilder.Entity<OrganizationApproval>().ToTable("OrganizationApproval", schema: "organizations");

            modelBuilder.Entity<Entity>().ToTable("Entity", schema: "organizations");

            modelBuilder.Entity<EntityHistory>().ToTable("EntityHistory", schema: "organizations");

            modelBuilder.Entity<EntityApproval>().ToTable("EntityApproval", schema: "organizations");

            modelBuilder.Entity<BillingDetails>().ToTable("BillingDetails", schema: "organizations");

            modelBuilder.Entity<ServiceProvider>().ToTable("ServiceProvider", schema: "organizations");

            modelBuilder.Entity<DeliveryStatus>().ToTable("DeliveryStatus", schema: "organizations");

            modelBuilder.Entity<BillStatus>().ToTable("BillStatus", schema: "organizations");

            modelBuilder.Entity<BillingFrequency>().ToTable("BillingFrequency", schema: "organizations");

            modelBuilder.Entity<BillingLevel>().ToTable("BillingLevel", schema: "organizations");

            modelBuilder.Entity<RegulationSetupIndustryTOB>()
                .ToTable("RegulationSetupIndustryTOB", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<RegulationSetupIndustryTOBHistory>()
                .ToTable("RegulationSetupIndustryTOBHistory", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql(sql: "NEWID()");

            modelBuilder.Entity<CurrencyCodes>().ToTable("CurrencyCodes", schema: "product_owner");

            modelBuilder.Entity<CountryFileNames>().ToTable("CountryFileNames", schema: "product_owner");

            modelBuilder.Entity<CountryFileNamesApproval>().ToTable("CountryFileNamesApproval", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<ServiceRequest>()
                .ToTable("ServiceRequest", "client");
            modelBuilder.Entity<Notification>().ToTable("Notifications", schema: "dbo");

            modelBuilder.Entity<UserOrganization>().ToTable("UserOrganization");

            modelBuilder.Entity<RegulationSetupLegalEntityType>().ToTable("RegulationSetupLegalEntityType", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<RegulationSetupLegalEntityTypeHistory>().ToTable("RegulationSetupLegalEntityTypeHistory", schema: "product_owner")
                .Property(u => u.UID).HasDefaultValueSql("NEWID()");

            //modelBuilder.Entity<Country>()
            //    .Ignore(c => c.ResultSet);

            //modelBuilder.Entity<States>()
            //    .Ignore(c => c.ResultSet);

            //modelBuilder.Entity<EntityTypeModel>()
            //    .Ignore(c => c.ResultSet);

            //modelBuilder.Entity<IndustryrMapping>()
            //    .Ignore(c => c.ResultSet);

            //modelBuilder.Entity<MajorIndustry>()
            //    .Ignore(c => c.ResultSet);

            //modelBuilder.Entity<MinorIndustry>()
            //    .Ignore(c => c.ResultSet);

            //modelBuilder.Entity<RegulationGroupModel>()
            //    .Ignore(c => c.ResultSet);
        }

        public DbSet<Parameters> Parameter { get; set; }

        public DbSet<ParameterApproval> ParameterApproval { get; set; }

        public DbSet<ParameterHistory> ParameterHistory { get; set; }

        public DbSet<RegulationSetupDetails> RegulationSetupDetails { get; set; }

        public DbSet<RegulationSetupParameter> RegulationSetupParameter { get; set; }

        public DbSet<RegulationSetupParameterHistory> RegulationSetupParameterHistory { get; set; }

        public DbSet<RegulationSetupHistory> RegulationSetupHistory { get; set; }

        public DbSet<RegulationSetupApproval> RegulationSetupApproval { get; set; }

        public DbSet<TOBDetails> TOBDetails { get; set; }

        public DbSet<TOBApproval> TOBApproval { get; set; }

        public DbSet<TOBHistory> TOBHistory { get; set; }

        public DbSet<TOBMapping> TOBMapping { get; set; }

        public DbSet<TOBMappingApproval> TOBMappingApproval { get; set; }

        public DbSet<RegulationSetupIndustryTOB> RegulationSetupIndustryTOB { get; set; }

        public DbSet<RegulationSetupIndustryTOBHistory> RegulationSetupIndustryTOBHistory { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Define schema for Product entity
        //    modelBuilder.Entity<Parameters>().ToTable("Parameter", schema: "product_owner");
        //    modelBuilder.Entity<Parameters>()
        //                .Property(u => u.UID)
        //                .HasDefaultValueSql("NEWID()");
        //}
    }
}