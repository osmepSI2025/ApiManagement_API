using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SME_API_Apimanagement.Entities;

public partial class ApiMangeDBContext : DbContext
{
    public ApiMangeDBContext()
    {
    }

    public ApiMangeDBContext(DbContextOptions<ApiMangeDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MApiInformation> MApiInformations { get; set; }

    public virtual DbSet<MLookup> MLookups { get; set; }

    public virtual DbSet<MMenu> MMenus { get; set; }

    public virtual DbSet<MOrganization> MOrganizations { get; set; }

    public virtual DbSet<MRegister> MRegisters { get; set; }

    public virtual DbSet<MRole> MRoles { get; set; }

    public virtual DbSet<MSystem> MSystems { get; set; }

    public virtual DbSet<MSystemInfo> MSystemInfos { get; set; }

    public virtual DbSet<TApiPermisionMapping> TApiPermisionMappings { get; set; }

    public virtual DbSet<TEmployeeRole> TEmployeeRoles { get; set; }

    public virtual DbSet<TErrorApiLog> TErrorApiLogs { get; set; }

    public virtual DbSet<TRoleMenu> TRoleMenus { get; set; }

    public virtual DbSet<TRolePermission> TRolePermissions { get; set; }

    public virtual DbSet<TSystemApiMapping> TSystemApiMappings { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=192.168.9.155;Database=bluecarg_SME_API;User Id=sa;Password=Osmep@2025;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Thai_CI_AS");

        modelBuilder.Entity<MApiInformation>(entity =>
        {
            entity.ToTable("M_ApiInformation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiKey).HasMaxLength(150);
            entity.Property(e => e.AuthorizationType).HasMaxLength(50);
            entity.Property(e => e.Bearer).HasColumnType("ntext");
            entity.Property(e => e.ContentType).HasMaxLength(150);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MethodType).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(150);
            entity.Property(e => e.ServiceNameCode).HasMaxLength(250);
            entity.Property(e => e.ServiceNameTh).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Urldevelopment).HasColumnName("URLDevelopment");
            entity.Property(e => e.Urlproduction).HasColumnName("URLProduction");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<MLookup>(entity =>
        {
            entity.ToTable("M_Lookup", "SMEAPI");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FlagActive).HasMaxLength(1);
            entity.Property(e => e.FlagDelete).HasMaxLength(1);
            entity.Property(e => e.LookupCode).HasMaxLength(50);
            entity.Property(e => e.LookupDescription).HasMaxLength(50);
            entity.Property(e => e.LookupType).HasMaxLength(150);
            entity.Property(e => e.LookupValue).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId);

            entity.ToTable("M_Menu");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FlagActive).HasMaxLength(1);
            entity.Property(e => e.FlagDelete).HasMaxLength(1);
            entity.Property(e => e.MenuName).HasMaxLength(50);
            entity.Property(e => e.MenuUrl)
                .HasMaxLength(50)
                .HasColumnName("MenuURL");
            entity.Property(e => e.Style).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MOrganization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId);

            entity.ToTable("M_Organization", "SMEAPI");

            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FlagDelete).HasMaxLength(1);
            entity.Property(e => e.IndustryType).HasMaxLength(50);
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(50)
                .HasColumnName("LogoURL");
            entity.Property(e => e.OrganizationCode).HasMaxLength(50);
            entity.Property(e => e.OrganizationName).HasMaxLength(150);
            entity.Property(e => e.ParentOrganizationId)
                .HasMaxLength(50)
                .HasColumnName("ParentOrganizationID");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(50);
            entity.Property(e => e.StateOrProvince).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TaxId)
                .HasMaxLength(50)
                .HasColumnName("TaxID");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(50)
                .HasColumnName("WebsiteURL");
        });

        modelBuilder.Entity<MRegister>(entity =>
        {
            entity.ToTable("M_Register", "SMEAPI");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiKey)
                .HasMaxLength(50)
                .HasColumnName("API_Key");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.FlagDelete).HasMaxLength(1);
            entity.Property(e => e.OrganizationCode).HasMaxLength(50);
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MRole>(entity =>
        {
            entity.ToTable("M_Role", "SMEAPI");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FlagDelete).HasMaxLength(50);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(150);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MSystem>(entity =>
        {
            entity.ToTable("M_System", "SMEAPI");

            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FlagDelete).HasMaxLength(1);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.OwnerSystemCode).HasMaxLength(50);
            entity.Property(e => e.SystemCode)
                .HasMaxLength(50)
                .HasColumnName("System_Code");
            entity.Property(e => e.SystemName)
                .HasMaxLength(200)
                .HasColumnName("System_Name");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MSystemInfo>(entity =>
        {
            entity.ToTable("M_SystemInfo", "SMEAPI");

            entity.Property(e => e.ApiKey)
                .HasMaxLength(150)
                .HasColumnName("API_KEY");
            entity.Property(e => e.ApiPassword)
                .HasMaxLength(50)
                .HasColumnName("API_PASSWORD");
            entity.Property(e => e.ApiUrlProdInbound)
                .HasMaxLength(300)
                .HasColumnName("API_URL_PROD_Inbound");
            entity.Property(e => e.ApiUrlUatInbound)
                .HasMaxLength(300)
                .HasColumnName("API_URL_UAT_Inbound");
            entity.Property(e => e.ApiUser)
                .HasMaxLength(50)
                .HasColumnName("API_USER");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FlagDelete).HasMaxLength(1);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.SystemCode)
                .HasMaxLength(50)
                .HasColumnName("System_Code");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TApiPermisionMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_T_API_Mapping");

            entity.ToTable("T_API_Permision_Mapping", "SMEAPI");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiKey)
                .HasMaxLength(200)
                .HasColumnName("API_KEY");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.FlagDelete).HasMaxLength(1);
            entity.Property(e => e.OrganizationCode).HasMaxLength(50);
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.SystemApiMappingId).HasColumnName("System_API_Mapping_ID");
            entity.Property(e => e.SystemCode)
                .HasMaxLength(50)
                .HasColumnName("System_Code");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TEmployeeRole>(entity =>
        {
            entity.ToTable("T_Employee_Roles", "SMEAPI");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BusinessUnitId).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.PositionId).HasMaxLength(50);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .HasColumnName("Role_code");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TErrorApiLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_T_ErrorLog");

            entity.ToTable("T_Error_Api_Log", "SMEAPI");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorDate).HasColumnType("datetime");
            entity.Property(e => e.HttpCode).HasMaxLength(50);
            entity.Property(e => e.HttpMethod).HasMaxLength(50);
            entity.Property(e => e.InnerException).HasColumnType("ntext");
            entity.Property(e => e.Message).HasColumnType("ntext");
            entity.Property(e => e.Path).HasMaxLength(500);
            entity.Property(e => e.RequestData).HasColumnType("ntext");
            entity.Property(e => e.Source).HasMaxLength(500);
            entity.Property(e => e.StackTrace).HasColumnType("ntext");
            entity.Property(e => e.SystemCode).HasMaxLength(50);
            entity.Property(e => e.TargetSite).HasMaxLength(500);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<TRoleMenu>(entity =>
        {
            entity.ToTable("T_RoleMenu", "SMEAPI");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TRolePermission>(entity =>
        {
            entity.ToTable("T_RolePermission", "SMEAPI");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.PermissionCode).HasMaxLength(50);
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TSystemApiMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_T_System_API");

            entity.ToTable("T_System_API_Mapping", "SMEAPI");

            entity.Property(e => e.ApiKey)
                .HasMaxLength(150)
                .HasColumnName("API_KEY");
            entity.Property(e => e.ApiMethod)
                .HasMaxLength(50)
                .HasColumnName("API_Method");
            entity.Property(e => e.ApiNote).HasColumnName("API_Note");
            entity.Property(e => e.ApiPassword)
                .HasMaxLength(50)
                .HasColumnName("API_PASSWORD");
            entity.Property(e => e.ApiRequestDescription).HasColumnName("API_Request_Description");
            entity.Property(e => e.ApiRequestExample).HasColumnName("API_Request_Example");
            entity.Property(e => e.ApiRequestParamater)
                .HasMaxLength(50)
                .HasColumnName("API_Request_Paramater");
            entity.Property(e => e.ApiRequestParamaterType)
                .HasMaxLength(50)
                .HasColumnName("API_Request_Paramater_Type");
            entity.Property(e => e.ApiResponseDescription).HasColumnName("API_Response_Description");
            entity.Property(e => e.ApiResponseExample).HasColumnName("API_Response_Example");
            entity.Property(e => e.ApiResponseParamater)
                .HasMaxLength(50)
                .HasColumnName("API_Response_Paramater");
            entity.Property(e => e.ApiResponseParamaterType)
                .HasMaxLength(50)
                .HasColumnName("API_Response_Paramater_Type");
            entity.Property(e => e.ApiServiceCode)
                .HasMaxLength(50)
                .HasColumnName("API_Service_Code");
            entity.Property(e => e.ApiServiceName)
                .HasMaxLength(250)
                .HasColumnName("API_Service_Name");
            entity.Property(e => e.ApiServiceType)
                .HasMaxLength(50)
                .HasColumnName("API_Service_Type");
            entity.Property(e => e.ApiUrlProdInbound)
                .HasMaxLength(300)
                .HasColumnName("API_URL_PROD_Inbound");
            entity.Property(e => e.ApiUrlProdOutbound)
                .HasMaxLength(300)
                .HasColumnName("API_URL_PROD_Outbound");
            entity.Property(e => e.ApiUrlUatInbound)
                .HasMaxLength(300)
                .HasColumnName("API_URL_UAT_Inbound");
            entity.Property(e => e.ApiUrlUatOutbound)
                .HasMaxLength(300)
                .HasColumnName("API_URL_UAT_Outbound");
            entity.Property(e => e.ApiUser)
                .HasMaxLength(50)
                .HasColumnName("API_USER");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EndPoint)
                .HasMaxLength(300)
                .HasColumnName("End_Point");
            entity.Property(e => e.FlagDelete).HasMaxLength(1);
            entity.Property(e => e.OwnerSystemCode)
                .HasMaxLength(50)
                .HasColumnName("Owner_System_Code");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
