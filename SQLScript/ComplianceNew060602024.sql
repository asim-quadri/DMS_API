USE [master]
GO
/****** Object:  Database [ComplianceNew]    Script Date: 06-06-2024 00:48:34 ******/
CREATE DATABASE [ComplianceNew]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ComplianceNew', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ComplianceNew.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ComplianceNew_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ComplianceNew_log.ldf' , SIZE = 139264KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ComplianceNew] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ComplianceNew].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ComplianceNew] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ComplianceNew] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ComplianceNew] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ComplianceNew] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ComplianceNew] SET ARITHABORT OFF 
GO
ALTER DATABASE [ComplianceNew] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ComplianceNew] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ComplianceNew] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ComplianceNew] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ComplianceNew] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ComplianceNew] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ComplianceNew] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ComplianceNew] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ComplianceNew] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ComplianceNew] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ComplianceNew] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ComplianceNew] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ComplianceNew] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ComplianceNew] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ComplianceNew] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ComplianceNew] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ComplianceNew] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ComplianceNew] SET RECOVERY FULL 
GO
ALTER DATABASE [ComplianceNew] SET  MULTI_USER 
GO
ALTER DATABASE [ComplianceNew] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ComplianceNew] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ComplianceNew] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ComplianceNew] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ComplianceNew] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ComplianceNew] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ComplianceNew', N'ON'
GO
ALTER DATABASE [ComplianceNew] SET QUERY_STORE = ON
GO
ALTER DATABASE [ComplianceNew] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ComplianceNew]
GO
/****** Object:  Schema [product_owner]    Script Date: 06-06-2024 00:48:38 ******/
CREATE SCHEMA [product_owner]
GO
/****** Object:  UserDefinedFunction [dbo].[IsSuperAdmin]    Script Date: 06-06-2024 00:48:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[IsSuperAdmin](
@UserId INT = 0
)
RETURNS BIT
AS
BEGIN
    DECLARE @Result BIT = 0;

   
   IF EXISTS( SELECT 1 FROM [dbo].Users U 
	INNER JOIN USERROLEMAPPING RM ON RM.USERID = U.ID 
	INNER JOIN RefRoles RR ON RR.ID = RM.ROLEID
	WHERE U.ID= @UserId AND RR.ROLENAME IN('SuperAdmin','ITSupportAdmin'))
	BEGIN
		SET @Result = 1;
	END;

    RETURN @Result;
END;
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organisations]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organisations](
	[OrgId] [int] IDENTITY(1,1) NOT NULL,
	[OrgName] [nvarchar](max) NOT NULL,
	[TypeofProduct] [nvarchar](max) NOT NULL,
	[Noofentities] [int] NOT NULL,
	[Noofusers] [int] NOT NULL,
	[Billinglevel] [nvarchar](max) NOT NULL,
	[OnboardCountry] [nvarchar](max) NOT NULL,
	[Country] [int] NOT NULL,
	[State] [int] NOT NULL,
	[City] [nvarchar](max) NOT NULL,
	[address] [nvarchar](max) NOT NULL,
	[Pincode] [int] NOT NULL,
	[managerid] [bigint] NOT NULL,
	[CreatedOn] [datetime2](7) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[UID] [uniqueidentifier] NULL,
	[Status] [tinyint] NULL,
 CONSTRAINT [PK_Organisations] PRIMARY KEY CLUSTERED 
(
	[OrgId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganisationsApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganisationsApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Orgid] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalType] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OrganisationsApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefApprovalStatus]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefApprovalStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RefApprovalStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefApprovalType]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefApprovalType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApprovalType] [nvarchar](100) NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RefApprovalType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefProducts]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](50) NULL,
	[Status] [tinyint] NULL,
	[ParentProductId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RefApplications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefRoles]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ManagerId] [bigint] NULL,
	[RoleName] [nvarchar](50) NULL,
	[RoleDisplayName] [nvarchar](50) NULL,
	[Description] [nvarchar](100) NULL,
	[Status] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RefRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefRolesHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefRolesHistory](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [bigint] NULL,
	[RoleName] [nvarchar](50) NULL,
	[RoleDisplayName] [nvarchar](50) NULL,
	[Description] [nvarchar](100) NULL,
	[Status] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RefRolesHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[HistoryId] [bigint] NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[ApprovalType] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProductApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProductApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ProductMappingId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[ApprovalType] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserProductApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProductApprovalHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProductApprovalHistory](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ProductMappingId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[ApprovalType] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserProductApprovalHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProductMapping]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProductMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[ProductId] [int] NULL,
	[Status] [int] NULL,
	[Enable] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserAccessControl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProductMappingHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProductMappingHistory](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [bigint] NOT NULL,
	[UserId] [bigint] NULL,
	[ProductId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserAppMapping] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoleMapping]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[RoleId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserRoleMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoleMappingHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleMappingHistory](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [bigint] NOT NULL,
	[UserId] [bigint] NULL,
	[UserHistoryId] [bigint] NULL,
	[RoleId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserRoleMappingHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[FullName] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NULL,
	[Mobile] [nvarchar](20) NULL,
	[Password] [nvarchar](100) NULL,
	[Status] [tinyint] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ManagerId] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersHistory](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [bigint] NULL,
	[EmpId] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[FullName] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Password] [nvarchar](100) NULL,
	[Status] [tinyint] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ManagerId] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UsersHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[Country]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](100) NULL,
	[CountryCode] [nvarchar](100) NULL,
	[CountryCodeNumber] [nvarchar](100) NULL,
	[FinancialStartDate] [datetime] NULL,
	[FinancialEndDate] [datetime] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Country_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryEntityTypeMapping]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryEntityTypeMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NULL,
	[EntityTypeId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryEntityTypeMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryEntityTypeMappingApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryEntityTypeMappingApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[CountryEntityTypeMappingId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryEntityTypeMappingApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryMajorIndustryMapping]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryMajorIndustryMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NULL,
	[MajorIndustryId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryMajorMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryMajorIndustryMappingApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryMajorIndustryMappingApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[CountryMajorIndustryMappingId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryMajorIndustryMappingApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryRegulationGroupMapping]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryRegulationGroupMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NULL,
	[RegulationGroupId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryRegulationGroupMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryRegulationGroupMappingApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryRegulationGroupMappingApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[CountryRegulationGroupMappingId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryRegulationGroupMappingApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryStateMapping]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryStateMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NULL,
	[StateId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryStateMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[CountryStateMappingApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[CountryStateMappingApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[CountryStateMappingId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryStateMappingApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[EntityType]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[EntityType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EntityType] [nvarchar](50) NULL,
	[EntityTypeCode] [nvarchar](25) NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_EntityType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[EntityTypeApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[EntityTypeApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EntityTypeId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_EntityTypeApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[IndustryApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[IndustryApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MajorIndustryId] [bigint] NOT NULL,
	[CountryId] [bigint] NULL,
	[MinorIndustryId] [bigint] NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_IndustryApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[IndustryMapping]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[IndustryMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NULL,
	[MajorIndustryId] [int] NULL,
	[MinorIndustryId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_IndustryMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[IndustryMappingApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[IndustryMappingApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[IndustryMappingId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_IndustryMappingApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[MajorIndustry]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[MajorIndustry](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MajorIndustryName] [nvarchar](50) NULL,
	[MajorIndustryCode] [nvarchar](50) NULL,
	[ManagerId] [bigint] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MajorIndustry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[MajorIndustryApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[MajorIndustryApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MajorIndustryId] [bigint] NOT NULL,
	[CountryId] [bigint] NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MajorIndustryApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[MajorMinorIndustryMapping]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[MajorMinorIndustryMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MajorIndustryId] [int] NULL,
	[MinorIndustryId] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MajorMinorMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[MajorMinorIndustryMappingApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[MajorMinorIndustryMappingApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[MajorMinorIndustryMappingId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MajorMinorIndustryMappingApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[MinorIndustry]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[MinorIndustry](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MinorIndustryName] [nvarchar](50) NULL,
	[MinorIndustryCode] [nvarchar](50) NULL,
	[MajorIndustryId] [bigint] NULL,
	[ManagerId] [bigint] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MinorIndustry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[MinorIndustryApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[MinorIndustryApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MinorIndustryId] [bigint] NOT NULL,
	[MajorIndustryId] [bigint] NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MinorIndustryApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[Parameter]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[Parameter](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EmpId] [nvarchar](20) NULL,
	[ParameterName] [nvarchar](50) NULL,
	[ParameterType] [nvarchar](50) NULL,
	[ManagerId] [bigint] NULL,
	[Status] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Parameter_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[ParameterApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[ParameterApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[HistoryId] [bigint] NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[ApprovalType] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ParameterApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[ParameterHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[ParameterHistory](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [bigint] NULL,
	[EmpId] [nvarchar](20) NULL,
	[ParameterName] [nvarchar](50) NULL,
	[ParameterType] [nvarchar](50) NULL,
	[ManagerId] [bigint] NULL,
	[Status] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ParameterHistory_HistoryId] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegSetupComplianceApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegSetupComplianceApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RegSetupComplianceHistoryId] [bigint] NOT NULL,
	[RegSetupComplianceId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegSetupComplianceApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegSetupComplianceParameterHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegSetupComplianceParameterHistory](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[Id] [bigint] NULL,
	[RegulationSetupComplianceId] [bigint] NULL,
	[ParameterTypeId] [bigint] NULL,
	[ParameterTypeValue] [nvarchar](100) NULL,
	[ParameterOperator] [nvarchar](500) NULL,
	[Sequence] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegSetupComplianceParameterHistory_Id] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationGroup]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationGroup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RegulationGroupName] [nvarchar](50) NULL,
	[RegulationGroupCode] [nvarchar](5) NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegulationGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationGroupApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationGroupApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RegulationGroupId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegulationGroupApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationSetupApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationSetupApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[HistoryId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegulationSetupApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationSetupCompliance]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationSetupCompliance](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RegulationSetupId] [bigint] NOT NULL,
	[ComplianceName] [nvarchar](200) NULL,
	[Description] [nvarchar](1000) NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_product_owner.Compliance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationSetupComplianceApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationSetupComplianceApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[HistoryId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegulationSetupComplianceApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationSetupComplianceHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationSetupComplianceHistory](
	[HistoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[RegulationSetupId] [bigint] NOT NULL,
	[ComplianceName] [nvarchar](200) NULL,
	[Description] [nvarchar](1000) NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_product_owner.ComplianceHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationSetupComplianceParameter]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationSetupComplianceParameter](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RegulationSetupComplianceId] [bigint] NULL,
	[ParameterTypeId] [bigint] NULL,
	[ParameterTypeValue] [nvarchar](100) NULL,
	[ParameterOperator] [nvarchar](500) NULL,
	[Sequence] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegulationSetupComplianceParameter_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationSetupDetails]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationSetupDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[regulationType] [nvarchar](100) NULL,
	[CountryId] [bigint] NULL,
	[StateId] [bigint] NULL,
	[RegulationName] [nvarchar](200) NULL,
	[RegulationGroupId] [bigint] NULL,
	[Description] [nvarchar](500) NULL,
	[MajorIndustryId] [bigint] NULL,
	[MinorIndustryId] [bigint] NULL,
	[EntityTypeId] [bigint] NULL,
	[ParameterTypeId] [bigint] NULL,
	[ParameterType] [nvarchar](100) NULL,
	[ParameterMode] [nvarchar](500) NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegulationSetupDetails_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationSetupHistory]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationSetupHistory](
	[HistoryId] [bigint] NOT NULL,
	[RegulationName] [nvarchar](200) NULL,
	[RegulationType] [nvarchar](100) NULL,
	[Description] [nvarchar](1000) NULL,
	[CountryId] [bigint] NULL,
	[StateId] [bigint] NULL,
	[RegulationGroupId] [bigint] NULL,
	[Description1] [nvarchar](500) NULL,
	[MajorIndustryId] [bigint] NULL,
	[MinorIndustryId] [bigint] NULL,
	[EntityTypeId] [bigint] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_product_owner.RegulationSetupHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[RegulationSetupParameter]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[RegulationSetupParameter](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RegulationSetupComplianceId] [bigint] NULL,
	[ParameterTypeId] [bigint] NULL,
	[ParameterTypeValue] [nvarchar](100) NULL,
	[ParameterOperator] [nvarchar](500) NULL,
	[Sequence] [int] NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RegulationSetupParaneter_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[State]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[State](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [bigint] NULL,
	[StateName] [nvarchar](50) NULL,
	[StateCode] [nvarchar](5) NULL,
	[Status] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [product_owner].[StateApproval]    Script Date: 06-06-2024 00:48:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [product_owner].[StateApproval](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StateId] [bigint] NOT NULL,
	[ManagerId] [bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[UID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_StateApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240501133726_init', N'7.0.18')
GO
SET IDENTITY_INSERT [dbo].[RefApprovalStatus] ON 
GO
INSERT [dbo].[RefApprovalStatus] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, N'Pending', CAST(N'2023-10-27T00:00:00.000' AS DateTime), NULL, NULL, NULL, N'69fa818f-84c1-4352-ab85-f3a8963fe7c8')
GO
INSERT [dbo].[RefApprovalStatus] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (2, N'Approved', CAST(N'2023-10-27T00:00:00.000' AS DateTime), NULL, NULL, NULL, N'f0966fc4-7232-459f-b436-9f1f7bea57a6')
GO
INSERT [dbo].[RefApprovalStatus] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (3, N'Rejected', CAST(N'2023-10-27T00:00:00.000' AS DateTime), NULL, NULL, NULL, N'306e5469-be77-4ec4-a969-e3cf2e25aaf8')
GO
INSERT [dbo].[RefApprovalStatus] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (4, N'Reviewed', CAST(N'2023-10-27T00:00:00.000' AS DateTime), NULL, NULL, NULL, N'9b14e307-317f-44fc-acb5-d447434a0239')
GO
INSERT [dbo].[RefApprovalStatus] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (5, N'Forward', CAST(N'2023-10-27T00:00:00.000' AS DateTime), NULL, NULL, NULL, N'a22be11c-2ffb-4299-abdd-cebcd0535361')
GO
SET IDENTITY_INSERT [dbo].[RefApprovalStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[RefApprovalType] ON 
GO
INSERT [dbo].[RefApprovalType] ([Id], [ApprovalType], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, N'User', 1, CAST(N'2023-10-27T00:00:00.000' AS DateTime), 1, NULL, NULL, N'e57537b9-9cad-40b3-b951-80fed0996e6c')
GO
INSERT [dbo].[RefApprovalType] ([Id], [ApprovalType], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (2, N'Role', 1, CAST(N'2023-10-27T00:00:00.000' AS DateTime), 1, NULL, NULL, N'b650b6a5-28eb-4fa5-aaad-53b4a66a05e2')
GO
INSERT [dbo].[RefApprovalType] ([Id], [ApprovalType], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (3, N'Access', 1, CAST(N'2023-10-27T00:00:00.000' AS DateTime), 1, NULL, NULL, N'6dcfd865-8a0a-4b7a-8136-43cf07ce42dd')
GO
SET IDENTITY_INSERT [dbo].[RefApprovalType] OFF
GO
SET IDENTITY_INSERT [dbo].[RefProducts] ON 
GO
INSERT [dbo].[RefProducts] ([Id], [ProductName], [Status], [ParentProductId], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, N'Product Setup', 1, NULL, CAST(N'2023-10-25T00:00:00.000' AS DateTime), 1, NULL, NULL, N'e3cf3b04-b521-4bd2-b66e-89c09f822b9d')
GO
INSERT [dbo].[RefProducts] ([Id], [ProductName], [Status], [ParentProductId], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (2, N'Organization Setup', 1, NULL, CAST(N'2023-10-25T00:00:00.000' AS DateTime), 1, NULL, NULL, N'519aa7bf-f71b-4976-9ec3-469b798ee5fb')
GO
INSERT [dbo].[RefProducts] ([Id], [ProductName], [Status], [ParentProductId], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (3, N'User Management', 1, NULL, CAST(N'2023-10-25T00:00:00.000' AS DateTime), 1, NULL, NULL, N'd0acdcb7-d26a-400a-a526-309d6f3e8190')
GO
INSERT [dbo].[RefProducts] ([Id], [ProductName], [Status], [ParentProductId], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (4, N'Role Screen', 1, 3, CAST(N'2023-10-25T00:00:00.000' AS DateTime), 1, NULL, NULL, N'e6fb9cd0-97b7-46d8-a938-1b3206ada221')
GO
INSERT [dbo].[RefProducts] ([Id], [ProductName], [Status], [ParentProductId], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (5, N'User Screen', 1, 3, CAST(N'2023-10-25T00:00:00.000' AS DateTime), 1, NULL, NULL, N'212e6464-c132-4d7b-8641-f1c0b38d18a8')
GO
INSERT [dbo].[RefProducts] ([Id], [ProductName], [Status], [ParentProductId], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (6, N'Access Control Screen', 1, 3, CAST(N'2023-10-25T00:00:00.000' AS DateTime), 1, NULL, NULL, N'74e99573-77e7-41f3-88e7-99366d2893ec')
GO
SET IDENTITY_INSERT [dbo].[RefProducts] OFF
GO
SET IDENTITY_INSERT [dbo].[RefRoles] ON 
GO
INSERT [dbo].[RefRoles] ([Id], [ManagerId], [RoleName], [RoleDisplayName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 3, N'SuperAdmin', N'SuperAdmin', N'Super Admin', 1, CAST(N'2023-10-17T00:00:00.000' AS DateTime), 1, 1, CAST(N'2024-05-11T01:14:10.743' AS DateTime), N'68c41ac2-c87e-421e-bcb7-91c0b66c67d2')
GO
INSERT [dbo].[RefRoles] ([Id], [ManagerId], [RoleName], [RoleDisplayName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (2, 3, N'Admin', N'Admin', N'Admin', 1, CAST(N'2023-10-17T00:00:00.000' AS DateTime), NULL, 1, CAST(N'2024-05-04T18:06:11.783' AS DateTime), N'40d84831-9bc8-4443-aebc-c7c04a9f9df8')
GO
INSERT [dbo].[RefRoles] ([Id], [ManagerId], [RoleName], [RoleDisplayName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (3, NULL, N'Approver', N'Approver', N'Approver', 1, CAST(N'2023-10-17T00:00:00.000' AS DateTime), NULL, 1, CAST(N'2024-05-11T01:14:08.103' AS DateTime), N'6a18d6e9-ed7a-41a0-a076-15a9ee42c7c5')
GO
INSERT [dbo].[RefRoles] ([Id], [ManagerId], [RoleName], [RoleDisplayName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (4, NULL, N'Reviewer', N'Reviewer', N'Reviewer', 1, CAST(N'2023-10-17T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2024-01-02T23:04:34.530' AS DateTime), N'cacff48f-d89e-442e-b8c0-7481b4b9bb3a')
GO
INSERT [dbo].[RefRoles] ([Id], [ManagerId], [RoleName], [RoleDisplayName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (5, NULL, N'ITSupportAdmin', N'IT Support Admin', N'IT Support Admin', 1, CAST(N'2023-10-17T00:00:00.000' AS DateTime), NULL, 1, CAST(N'2024-05-04T21:22:15.077' AS DateTime), N'097adf8e-b917-4d36-9244-209d9a711ceb')
GO
INSERT [dbo].[RefRoles] ([Id], [ManagerId], [RoleName], [RoleDisplayName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (6, NULL, N'ITAdmin', N'IT Admin', N'IT Admin', 1, CAST(N'2023-10-17T00:00:00.000' AS DateTime), NULL, 1, CAST(N'2024-05-04T21:21:27.773' AS DateTime), N'8812c2b5-1488-47f3-8fa9-2463f3093eb2')
GO
INSERT [dbo].[RefRoles] ([Id], [ManagerId], [RoleName], [RoleDisplayName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (7, NULL, N'ITUser', N'IT User', N'IT User', 1, CAST(N'2023-10-17T00:00:00.000' AS DateTime), NULL, NULL, NULL, N'c4982bf1-a2e4-407a-a506-ef75d510acd8')
GO
INSERT [dbo].[RefRoles] ([Id], [ManagerId], [RoleName], [RoleDisplayName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (8, 1, N'User', N'User', N'User', 1, CAST(N'2023-10-17T00:00:00.000' AS DateTime), NULL, 1, CAST(N'2024-05-04T21:33:49.687' AS DateTime), N'9b150a0a-f730-4c74-8408-30484a9756ea')
GO
SET IDENTITY_INSERT [dbo].[RefRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoleMapping] ON 
GO
INSERT [dbo].[UserRoleMapping] ([Id], [UserId], [RoleId], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 1, 1, NULL, NULL, NULL, NULL, N'9e82010d-4d6c-4a01-aaab-c24d87376991')
GO
INSERT [dbo].[UserRoleMapping] ([Id], [UserId], [RoleId], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (38, 10070, 2, NULL, CAST(N'2024-02-05T22:03:05.227' AS DateTime), 1, 1, CAST(N'2024-04-30T14:23:14.397' AS DateTime), N'bcd26f3d-558f-40a3-b303-6ea4eaf62ade')
GO
SET IDENTITY_INSERT [dbo].[UserRoleMapping] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Id], [EmpId], [FirstName], [LastName], [FullName], [Email], [Mobile], [Password], [Status], [StartDate], [EndDate], [ManagerId], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, N'0123', NULL, NULL, N'Gulam Asif', N'gulam.asif29111@gmail.com', N'9603675400', N'123', 1, CAST(N'2020-12-30T00:00:00.000' AS DateTime), CAST(N'2030-01-23T00:00:00.000' AS DateTime), NULL, CAST(N'2021-01-01T00:00:00.000' AS DateTime), 1, NULL, CAST(N'2023-10-28T01:03:14.523' AS DateTime), N'16193229-d8dc-4b89-8209-4e2fade34930')
GO
INSERT [dbo].[Users] ([Id], [EmpId], [FirstName], [LastName], [FullName], [Email], [Mobile], [Password], [Status], [StartDate], [EndDate], [ManagerId], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (10070, N'sam123', NULL, NULL, N'Sam', N'sam@gmail.com', N'9999999999', N'123', 1, CAST(N'2024-02-01T00:00:00.000' AS DateTime), CAST(N'2024-03-01T00:00:00.000' AS DateTime), 1, CAST(N'2024-02-05T22:03:05.223' AS DateTime), 1, 1, CAST(N'2024-04-30T14:23:25.767' AS DateTime), N'7e2fba89-99e9-45fd-927a-b05eeca4af66')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [product_owner].[Country] ON 
GO
INSERT [product_owner].[Country] ([Id], [CountryName], [CountryCode], [CountryCodeNumber], [FinancialStartDate], [FinancialEndDate], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, N'India', N'IN', N'91', CAST(N'2024-05-01T00:00:00.000' AS DateTime), CAST(N'2024-05-17T00:00:00.000' AS DateTime), 1, CAST(N'2024-05-25T01:33:20.680' AS DateTime), 10070, 1, CAST(N'2024-05-25T01:34:14.510' AS DateTime), N'643ff57e-88f4-449a-95a9-cb902120b906')
GO
SET IDENTITY_INSERT [product_owner].[Country] OFF
GO
SET IDENTITY_INSERT [product_owner].[CountryApproval] ON 
GO
INSERT [product_owner].[CountryApproval] ([Id], [CountryId], [ManagerId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 1, 2, CAST(N'2024-05-25T01:33:20.720' AS DateTime), 10070, 1, CAST(N'2024-05-25T01:34:14.510' AS DateTime), N'8fdbe24e-287d-4fd4-aa78-39c39c63c048')
GO
SET IDENTITY_INSERT [product_owner].[CountryApproval] OFF
GO
SET IDENTITY_INSERT [product_owner].[CountryRegulationGroupMapping] ON 
GO
INSERT [product_owner].[CountryRegulationGroupMapping] ([Id], [CountryId], [RegulationGroupId], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 1, 1, CAST(N'2024-05-28T00:22:34.347' AS DateTime), 10070, 1, CAST(N'2024-05-28T00:22:41.500' AS DateTime), N'790ccb28-314d-4ded-aeca-861f1809b465')
GO
SET IDENTITY_INSERT [product_owner].[CountryRegulationGroupMapping] OFF
GO
SET IDENTITY_INSERT [product_owner].[CountryRegulationGroupMappingApproval] ON 
GO
INSERT [product_owner].[CountryRegulationGroupMappingApproval] ([Id], [ManagerId], [CountryRegulationGroupMappingId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 1, 2, CAST(N'2024-05-28T00:22:34.363' AS DateTime), 10070, 1, CAST(N'2024-05-28T00:22:41.500' AS DateTime), N'e85bdb22-b597-48b6-a9a1-f3c9363fd50d')
GO
SET IDENTITY_INSERT [product_owner].[CountryRegulationGroupMappingApproval] OFF
GO
SET IDENTITY_INSERT [product_owner].[CountryStateMapping] ON 
GO
INSERT [product_owner].[CountryStateMapping] ([Id], [CountryId], [StateId], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 1, 1, CAST(N'2024-05-25T01:35:00.400' AS DateTime), 10070, 1, CAST(N'2024-05-25T01:35:16.433' AS DateTime), N'2443a7b5-d0b8-4de5-b072-77080d975181')
GO
SET IDENTITY_INSERT [product_owner].[CountryStateMapping] OFF
GO
SET IDENTITY_INSERT [product_owner].[CountryStateMappingApproval] ON 
GO
INSERT [product_owner].[CountryStateMappingApproval] ([Id], [ManagerId], [CountryStateMappingId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 1, 2, CAST(N'2024-05-25T01:35:00.420' AS DateTime), 10070, 1, CAST(N'2024-05-25T01:35:16.430' AS DateTime), N'357903f2-e834-4996-acb8-f272f495b00b')
GO
SET IDENTITY_INSERT [product_owner].[CountryStateMappingApproval] OFF
GO
SET IDENTITY_INSERT [product_owner].[Parameter] ON 
GO
INSERT [product_owner].[Parameter] ([Id], [EmpId], [ParameterName], [ParameterType], [ManagerId], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, N'1', N'Parma1', N'Yes/No', 0, 0, CAST(N'2024-05-24T23:39:14.203' AS DateTime), 1, NULL, NULL, N'c64b50e5-59d8-4d33-a47c-24452820bb22')
GO
SET IDENTITY_INSERT [product_owner].[Parameter] OFF
GO
SET IDENTITY_INSERT [product_owner].[ParameterApproval] ON 
GO
INSERT [product_owner].[ParameterApproval] ([Id], [HistoryId], [ManagerId], [ApprovalStatus], [ApprovalType], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 0, 2, 1, CAST(N'2024-05-24T23:39:17.377' AS DateTime), 1, NULL, NULL, N'e3246096-da37-4a54-afb4-e3a2165154c4')
GO
SET IDENTITY_INSERT [product_owner].[ParameterApproval] OFF
GO
SET IDENTITY_INSERT [product_owner].[ParameterHistory] ON 
GO
INSERT [product_owner].[ParameterHistory] ([HistoryId], [Id], [EmpId], [ParameterName], [ParameterType], [ManagerId], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 0, N'1', N'Parma1', N'Yes/No', 0, 0, CAST(N'2024-05-24T23:39:15.060' AS DateTime), 1, NULL, NULL, N'43447596-38c8-4d40-877b-ce354a5c2ceb')
GO
SET IDENTITY_INSERT [product_owner].[ParameterHistory] OFF
GO
SET IDENTITY_INSERT [product_owner].[RegSetupComplianceParameterHistory] ON 
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (11, NULL, 0, 1, N'33', N'AND', NULL, 1, CAST(N'2024-05-27T19:15:58.287' AS DateTime), 1, NULL, NULL, N'116efa0f-e263-4c7b-8cea-6ff93965d69b')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (12, NULL, 0, 1, N'33', N'OR', NULL, 1, CAST(N'2024-05-27T19:15:58.287' AS DateTime), 1, NULL, NULL, N'0cd4c6f9-02a0-4123-b7b8-29d491bedb06')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (13, NULL, 0, 1, N'23', N'AND', NULL, 1, CAST(N'2024-05-27T19:41:14.000' AS DateTime), 1, NULL, NULL, N'36a48813-08e1-4491-88bd-86f3778d1cfc')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (14, NULL, 0, 1, N'334', N'OR', NULL, 1, CAST(N'2024-05-27T19:41:14.040' AS DateTime), 1, NULL, NULL, N'6a7633ed-361c-48f1-8dc9-d7567694f560')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (15, NULL, 0, 1, N'33', N'AND', 1, 0, CAST(N'2024-05-27T20:30:15.453' AS DateTime), 10070, NULL, NULL, N'84a02fca-2c59-493d-b49c-1b1512f2bfb6')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (16, NULL, 0, 1, N'33', N'OR', 2, 0, CAST(N'2024-05-27T20:30:15.453' AS DateTime), 10070, NULL, NULL, N'9a3d6105-416b-4734-81ec-c7638d4fe88e')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (17, NULL, 0, 1, N'22', N'OR', 3, 0, CAST(N'2024-05-27T20:30:15.453' AS DateTime), 10070, NULL, NULL, N'75bdc73e-df76-4243-b47d-187ae001493f')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (18, NULL, 0, 1, N'433', N'AND', 1, 0, CAST(N'2024-06-01T20:19:57.593' AS DateTime), 10070, NULL, NULL, N'a61b75f3-95a0-4621-b7cf-c570cfdc442d')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (19, NULL, 0, 1, N'33', N'AND', 2, 0, CAST(N'2024-06-01T20:19:57.680' AS DateTime), 10070, NULL, NULL, N'4314ff6a-aa9f-476a-b3e6-8933ab5d84cc')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (20, NULL, 0, 1, N'33', N'OR', 1, 0, CAST(N'2024-06-01T20:22:08.967' AS DateTime), 10070, NULL, NULL, N'3a437d5c-b142-417b-80ec-c12bbae92c79')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (21, NULL, 0, 1, N'33', N'AND', 2, 0, CAST(N'2024-06-01T20:22:08.967' AS DateTime), 10070, NULL, NULL, N'68be2e4f-d5f4-4c90-bdf3-4df761857f05')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (22, NULL, 0, 1, N'33', N'OR', 1, 0, CAST(N'2024-06-01T20:27:25.630' AS DateTime), 10070, NULL, NULL, N'2a86482a-a7d0-478c-a830-1644eb454445')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (23, NULL, 0, 1, N'33', N'AND', 1, 0, CAST(N'2024-06-02T02:25:05.197' AS DateTime), 10070, NULL, NULL, N'af3ba495-d214-4370-a917-2e3c440e6e7e')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (24, NULL, 0, 1, N'44', N'AND', 2, 0, CAST(N'2024-06-02T02:25:05.230' AS DateTime), 10070, NULL, NULL, N'ae6878f8-3e57-4d05-8305-f9b6810f74ce')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (25, NULL, 8, 1, N'33', N'AND', 1, 0, CAST(N'2024-06-02T02:27:44.447' AS DateTime), 10070, NULL, NULL, N'e3760baf-8bd8-4077-a35d-ae1d60d99d71')
GO
INSERT [product_owner].[RegSetupComplianceParameterHistory] ([HistoryId], [Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (26, NULL, 8, 1, N'44', N'OR', 2, 0, CAST(N'2024-06-02T02:27:44.507' AS DateTime), 10070, NULL, NULL, N'ca667686-0b2d-4d66-b7cd-4db9420b4921')
GO
SET IDENTITY_INSERT [product_owner].[RegSetupComplianceParameterHistory] OFF
GO
SET IDENTITY_INSERT [product_owner].[RegulationGroup] ON 
GO
INSERT [product_owner].[RegulationGroup] ([Id], [RegulationGroupName], [RegulationGroupCode], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, N'Test', N'001', 1, CAST(N'2024-05-28T00:22:10.863' AS DateTime), 10070, 1, CAST(N'2024-05-28T00:22:22.107' AS DateTime), N'e8202736-6aa1-4375-ad05-6085b79a744f')
GO
SET IDENTITY_INSERT [product_owner].[RegulationGroup] OFF
GO
SET IDENTITY_INSERT [product_owner].[RegulationGroupApproval] ON 
GO
INSERT [product_owner].[RegulationGroupApproval] ([Id], [RegulationGroupId], [ManagerId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 1, 2, CAST(N'2024-05-28T00:22:10.900' AS DateTime), 10070, 1, CAST(N'2024-05-28T00:22:22.107' AS DateTime), N'410849a7-572b-4b83-b1cf-dd2743579dd9')
GO
SET IDENTITY_INSERT [product_owner].[RegulationGroupApproval] OFF
GO
SET IDENTITY_INSERT [product_owner].[RegulationSetupCompliance] ON 
GO
INSERT [product_owner].[RegulationSetupCompliance] ([Id], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (2, 0, NULL, NULL, 1, CAST(N'2024-05-27T19:15:58.283' AS DateTime), NULL, NULL, NULL, N'20cc105a-5d81-405e-84c2-b86c36e1f2e0')
GO
INSERT [product_owner].[RegulationSetupCompliance] ([Id], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (3, 0, N'adad', N'adadasd', 1, CAST(N'2024-05-27T19:41:13.827' AS DateTime), 1, NULL, NULL, N'e8335a0d-18d8-4397-be6f-4d6fd70b9860')
GO
INSERT [product_owner].[RegulationSetupCompliance] ([Id], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (4, 0, N'wewer', N'werwe', 1, CAST(N'2024-06-06T00:39:49.610' AS DateTime), NULL, NULL, NULL, N'7eaa6d01-c35f-4681-8967-5ad38e251ea3')
GO
SET IDENTITY_INSERT [product_owner].[RegulationSetupCompliance] OFF
GO
SET IDENTITY_INSERT [product_owner].[RegulationSetupComplianceApproval] ON 
GO
INSERT [product_owner].[RegulationSetupComplianceApproval] ([Id], [HistoryId], [ManagerId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 5, 1, 3, CAST(N'2024-06-01T20:22:17.697' AS DateTime), 10070, 10070, CAST(N'2024-06-06T00:43:27.027' AS DateTime), N'41959014-0959-44c6-a406-832867b8638f')
GO
INSERT [product_owner].[RegulationSetupComplianceApproval] ([Id], [HistoryId], [ManagerId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (2, 6, 1, 3, CAST(N'2024-06-01T20:50:32.280' AS DateTime), 10070, 10070, CAST(N'2024-06-06T00:43:44.920' AS DateTime), N'892edbb1-0e78-457f-9265-542c72d29412')
GO
INSERT [product_owner].[RegulationSetupComplianceApproval] ([Id], [HistoryId], [ManagerId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (3, 7, 1, 3, CAST(N'2024-06-02T02:25:11.147' AS DateTime), 10070, 10070, CAST(N'2024-06-06T00:43:50.150' AS DateTime), N'fbe07c4c-2c35-4235-ac8d-6e6fe95a6c3c')
GO
INSERT [product_owner].[RegulationSetupComplianceApproval] ([Id], [HistoryId], [ManagerId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (4, 8, 1, 2, CAST(N'2024-06-02T02:27:46.477' AS DateTime), 10070, 10070, CAST(N'2024-06-06T00:40:41.540' AS DateTime), N'b247c130-0f6f-4d13-a87d-7574225518bc')
GO
SET IDENTITY_INSERT [product_owner].[RegulationSetupComplianceApproval] OFF
GO
SET IDENTITY_INSERT [product_owner].[RegulationSetupComplianceHistory] ON 
GO
INSERT [product_owner].[RegulationSetupComplianceHistory] ([HistoryId], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 0, NULL, NULL, 1, CAST(N'2024-05-27T19:15:58.287' AS DateTime), NULL, NULL, NULL, N'a4258721-8cf3-410f-b045-7d0d3b169d21')
GO
INSERT [product_owner].[RegulationSetupComplianceHistory] ([HistoryId], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (2, 0, N'adad', N'adadasd', 1, CAST(N'2024-05-27T19:41:13.963' AS DateTime), NULL, NULL, NULL, N'32ef8678-26d6-4a71-86ec-78a150818ab6')
GO
INSERT [product_owner].[RegulationSetupComplianceHistory] ([HistoryId], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (3, 0, N'sfsf', N'sfsfsf', 0, CAST(N'2024-05-27T20:30:15.447' AS DateTime), 10070, NULL, NULL, N'9a08d625-4591-402d-b79c-a35e87685f94')
GO
INSERT [product_owner].[RegulationSetupComplianceHistory] ([HistoryId], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (4, 0, N'sfsdfs', N'sdfds', 0, CAST(N'2024-06-01T20:19:57.483' AS DateTime), 10070, NULL, NULL, N'35283e75-4bc7-4dee-b051-d06b15194aff')
GO
INSERT [product_owner].[RegulationSetupComplianceHistory] ([HistoryId], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (5, 0, N'afas', N'fasfasfa', 0, CAST(N'2024-06-01T20:22:08.967' AS DateTime), 10070, NULL, NULL, N'4dfcc725-e2f1-4b30-9b32-06b8be2b2394')
GO
INSERT [product_owner].[RegulationSetupComplianceHistory] ([HistoryId], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (6, 0, N'ssdfs', N'dsd', 0, CAST(N'2024-06-01T20:27:25.630' AS DateTime), 10070, NULL, NULL, N'7b048a95-c742-48f3-9c66-98b6206c47f8')
GO
INSERT [product_owner].[RegulationSetupComplianceHistory] ([HistoryId], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (7, 0, N'asfasf', N'aasfas', 0, CAST(N'2024-06-02T02:25:05.123' AS DateTime), 10070, NULL, NULL, N'e69df652-3db5-412b-9585-1b1411f9ebd2')
GO
INSERT [product_owner].[RegulationSetupComplianceHistory] ([HistoryId], [RegulationSetupId], [ComplianceName], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (8, 0, N'wewer', N'werwe', 0, CAST(N'2024-06-02T02:27:43.860' AS DateTime), 10070, NULL, NULL, N'089ebcf8-9ca4-47f4-8767-0fb107c03d0c')
GO
SET IDENTITY_INSERT [product_owner].[RegulationSetupComplianceHistory] OFF
GO
SET IDENTITY_INSERT [product_owner].[RegulationSetupComplianceParameter] ON 
GO
INSERT [product_owner].[RegulationSetupComplianceParameter] ([Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (11, 0, 1, N'33', N'AND', NULL, 1, CAST(N'2024-05-27T19:15:58.287' AS DateTime), 1, NULL, NULL, N'26c39042-c062-4a06-b29a-515f00fe583d')
GO
INSERT [product_owner].[RegulationSetupComplianceParameter] ([Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (12, 0, 1, N'33', N'OR', NULL, 1, CAST(N'2024-05-27T19:15:58.287' AS DateTime), 1, NULL, NULL, N'83d4298f-70da-4981-a8b8-51159ac08fb0')
GO
INSERT [product_owner].[RegulationSetupComplianceParameter] ([Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (13, 0, 1, N'23', N'AND', NULL, 1, CAST(N'2024-05-27T19:41:13.927' AS DateTime), 1, NULL, NULL, N'f25118a7-fcd2-4e81-a8f4-49ba84225dfd')
GO
INSERT [product_owner].[RegulationSetupComplianceParameter] ([Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (14, 0, 1, N'334', N'OR', NULL, 1, CAST(N'2024-05-27T19:41:13.963' AS DateTime), 1, NULL, NULL, N'2b748e65-2f8f-4df5-a1bb-bb3aea6b7e49')
GO
INSERT [product_owner].[RegulationSetupComplianceParameter] ([Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (15, 4, 1, N'33', N'AND', NULL, 1, CAST(N'2024-06-06T00:40:03.190' AS DateTime), NULL, NULL, NULL, N'3f9aad64-2028-4a28-994a-9815e0e6699b')
GO
INSERT [product_owner].[RegulationSetupComplianceParameter] ([Id], [RegulationSetupComplianceId], [ParameterTypeId], [ParameterTypeValue], [ParameterOperator], [Sequence], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (16, 4, 1, N'44', N'OR', NULL, 1, CAST(N'2024-06-06T00:40:19.957' AS DateTime), NULL, NULL, NULL, N'a1fa89e9-9814-4383-8932-0b30b70efd19')
GO
SET IDENTITY_INSERT [product_owner].[RegulationSetupComplianceParameter] OFF
GO
SET IDENTITY_INSERT [product_owner].[State] ON 
GO
INSERT [product_owner].[State] ([Id], [CountryId], [StateName], [StateCode], [Status], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, N'Telangana', N'TG', 1, CAST(N'2024-05-25T01:34:39.343' AS DateTime), 10070, 1, CAST(N'2024-05-25T01:34:50.120' AS DateTime), N'23e74a46-22dd-416d-97a4-f8b5746070dc')
GO
SET IDENTITY_INSERT [product_owner].[State] OFF
GO
SET IDENTITY_INSERT [product_owner].[StateApproval] ON 
GO
INSERT [product_owner].[StateApproval] ([Id], [StateId], [ManagerId], [ApprovalStatus], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [UID]) VALUES (1, 1, 1, 2, CAST(N'2024-05-25T01:34:39.373' AS DateTime), 10070, 1, CAST(N'2024-05-25T01:34:50.117' AS DateTime), N'1aca3de4-ccd7-4f7b-a2aa-0edd0bf412e1')
GO
SET IDENTITY_INSERT [product_owner].[StateApproval] OFF
GO
/****** Object:  Index [IX_Organisations_CreatedBy]    Script Date: 06-06-2024 00:49:54 ******/
CREATE NONCLUSTERED INDEX [IX_Organisations_CreatedBy] ON [dbo].[Organisations]
(
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Organisations_managerid]    Script Date: 06-06-2024 00:49:54 ******/
CREATE NONCLUSTERED INDEX [IX_Organisations_managerid] ON [dbo].[Organisations]
(
	[managerid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RefApprovalStatus] ADD  CONSTRAINT [DF_RefApprovalStatus_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[RefApprovalType] ADD  CONSTRAINT [DF_RefApprovalType_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[RefProducts] ADD  CONSTRAINT [DF_RefApplications_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[RefRoles] ADD  CONSTRAINT [DF_RefRoles_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[RefRolesHistory] ADD  CONSTRAINT [DF_RefRoles_History_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[UserApproval] ADD  CONSTRAINT [UserApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[UserProductApproval] ADD  CONSTRAINT [DF_UserProductApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[UserProductApprovalHistory] ADD  CONSTRAINT [DF_UserProductApprovalHistory_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[UserProductMapping] ADD  CONSTRAINT [DF_UserAccessControl_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[UserProductMappingHistory] ADD  CONSTRAINT [DF_UserAppMapping_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[UserRoleMapping] ADD  CONSTRAINT [DF_UserRoleMapping_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[Country] ADD  CONSTRAINT [DF_Country_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryApproval] ADD  CONSTRAINT [DF_CountryApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryEntityTypeMapping] ADD  CONSTRAINT [DF_CountryEntityTypeMapping_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryEntityTypeMappingApproval] ADD  CONSTRAINT [DF_CountryEntityTypeMappingApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryMajorIndustryMapping] ADD  CONSTRAINT [DF_CountryMajorMapping_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryMajorIndustryMappingApproval] ADD  CONSTRAINT [DF_CountryMajorIndustryMappingApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryRegulationGroupMapping] ADD  CONSTRAINT [DF_CountryRegulationGroupMapping_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryRegulationGroupMappingApproval] ADD  CONSTRAINT [DF_CountryRegulationGroupMappingApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryStateMapping] ADD  CONSTRAINT [DF_CountryStateMapping_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[CountryStateMappingApproval] ADD  CONSTRAINT [DF_CountryStateMappingApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[EntityType] ADD  CONSTRAINT [DF_EntityType_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[EntityTypeApproval] ADD  CONSTRAINT [DF_EntityTypeApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[IndustryApproval] ADD  CONSTRAINT [DF_IndustryApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[IndustryMapping] ADD  CONSTRAINT [DF_IndustryMapping_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[IndustryMappingApproval] ADD  CONSTRAINT [DF_IndustryMappingApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[MajorIndustry] ADD  CONSTRAINT [DF_MajorIndustry_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[MajorIndustryApproval] ADD  CONSTRAINT [DF_MajorIndustryApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[MajorMinorIndustryMapping] ADD  CONSTRAINT [DF_MajorMinorMapping_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[MajorMinorIndustryMappingApproval] ADD  CONSTRAINT [DF_MajorMinorIndustryMappingApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[MinorIndustry] ADD  CONSTRAINT [DF_MinorIndustry_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[MinorIndustryApproval] ADD  CONSTRAINT [DF_MinorIndustryApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[Parameter] ADD  CONSTRAINT [DF_Parameter_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[ParameterHistory] ADD  CONSTRAINT [DF_ParameterHistory_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegSetupComplianceApproval] ADD  CONSTRAINT [DF_RegSetupComplianceApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegSetupComplianceParameterHistory] ADD  CONSTRAINT [DF_RegSetupComplianceParameterHistory_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationGroup] ADD  CONSTRAINT [DF_RegulationGroup_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationGroupApproval] ADD  CONSTRAINT [DF_RegulationGroupApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationSetupApproval] ADD  CONSTRAINT [DF_RegulationSetupApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationSetupCompliance] ADD  CONSTRAINT [DF_product_owner.Compliance_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationSetupComplianceApproval] ADD  CONSTRAINT [DF_RegulationSetupComplianceApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationSetupComplianceHistory] ADD  CONSTRAINT [DF_product_owner.ComplianceHistory_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationSetupComplianceParameter] ADD  CONSTRAINT [DF_RegulationSetupComplianceParameter_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationSetupDetails] ADD  CONSTRAINT [DF_RegulationSetupDetails_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationSetupHistory] ADD  CONSTRAINT [DF_product_owner.RegulationSetupHistory_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[RegulationSetupParameter] ADD  CONSTRAINT [DF_RegulationSetupParaneter_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[State] ADD  CONSTRAINT [DF_State_UID]  DEFAULT (newid()) FOR [UID]
GO
ALTER TABLE [product_owner].[StateApproval] ADD  CONSTRAINT [DF_StateApproval_UID]  DEFAULT (newid()) FOR [UID]
GO
/****** Object:  StoredProcedure [dbo].[USP_ADDMAJORINDUSTRY]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_ADDMAJORINDUSTRY]  
 (
	@Id						INT = 0,
	@MajorIndustryName		VARCHAR(50),
	@MajorIndustryCode		INT = 0,
	@MinorIndustryId		INT = 0,
	@ManagerId	INT,
	@CreatedBy  INT,
	@UID NVARCHAR(100)= NULL,
	@Status INT = 0
 )  
AS  
BEGIN  
	BEGIN
		INSERT INTO MajorIndustry(MajorIndustryName,MajorIndustryCode,MinorIndustryId,Status,ManagerId,CreatedOn,CreatedBy)
		VALUES(@MajorIndustryName,@MajorIndustryCode,@MinorIndustryId,@Status,@ManagerId,GETDATE(),@CreatedBy);
END;
END;
GO
/****** Object:  StoredProcedure [dbo].[USP_ADDMINORINDUSTRY]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_ADDMINORINDUSTRY]  
 (
	@Id						INT = 0,
	@MinorIndustryName		VARCHAR(50),
	@MinorIndustryCode		INT = 0,
	@MajorIndustryId		INT = 0,
	@ManagerId	INT,
	@CreatedBy  INT,
	@UID NVARCHAR(100)= NULL,
	@Status INT = 0
 )  
AS  
BEGIN  
	BEGIN
		INSERT INTO MinorIndustry(MinorIndustryName,MinorIndustryCode,MajorIndustryId,Status,ManagerId,CreatedOn,CreatedBy)
		VALUES(@MinorIndustryName,@MinorIndustryCode,@MajorIndustryId,@Status,@ManagerId,GETDATE(),@CreatedBy);
END;
END;
GO
/****** Object:  StoredProcedure [dbo].[USP_ADDPRODUCTACCESS]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_ADDPRODUCTACCESS] 
	(
	@UserId INT  = 0,
	@ProductId INT = 0,
	@Status INT =0,
	@CreatedBy INT = 0,
	@UID NVARCHAR(100) = NULL,
	@Enable INT = 0
	)
AS
BEGIN
	
	DECLARE @ISAdmin bit = 0, @StatusId INT =0;
	SELECT @StatusId = Id FROM RefApprovalStatus WHERE Status = @Status
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	 SELECT @StatusId = Id FROM RefApprovalStatus WHERE Status='Approved'
	END;


	IF(NOT EXISTS(SELECT 1 FROM UserProductMapping WHERE UserId=@UserId AND ProductId = @ProductId))
	BEGIN
		INSERT INTO UserProductMapping(UserId,ProductId,Status,CreatedBy,CreatedOn,Enable)
		VALUES (@UserId,@ProductId,@StatusId,@CreatedBy,GETDATE(),@Enable);
		SELECT Id,
				UserId,
				ProductId,
				Status,
				CreatedOn,
				CreatedBy,
				ModifiedBy,
				ModifiedOn,
				UID FROM UserProductMapping  WHERE ID= SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE UserProductMapping SET
		Enable = @Enable,
		ModifiedBy = @CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UserId=@UserId AND ProductId = @ProductId

		SELECT Id,
				UserId,
				ProductId,
				Status,
				CreatedOn,
				CreatedBy,
				ModifiedBy,
				ModifiedOn,
				UID FROM UserProductMapping  WHERE UID = @UID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_ADDUPDATEPRODUCTAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_ADDUPDATEPRODUCTAPPROVAL]
	(
		@Id INT = 0,
		@UserId INT =0,
		@ManagerId INT = 0,
		@ProductMappingId INT =0,
		@ApprovalType INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
		
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@NEWMANAGERID INT = 0,@REJECTEDAPPROVAL INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @REJECTEDAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Rejected'
	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);
	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	 SET @ApprovalStatusId = @APPROVEDSTATUS;

	END;


	IF(NOT EXISTS(SELECT * FROM UserProductApproval WHERE UID = @UID) OR @ISAdmin = 1)
	BEGIN
		INSERT INTO UserProductApproval(UserId,ManagerId,ProductMappingId,ApprovalType,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@UserId,@ManagerId,@ProductMappingId,@ApprovalType,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE UserProductApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		IF((@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId =  @REVIEWEERSTATUS) AND @NEWMANAGERID != 0)
		BEGIN
			INSERT INTO UserProductApproval(UserId,ManagerId,ProductMappingId,ApprovalType,ApprovalStatus,CreatedBy,CreatedOn)
			VALUES(@UserId,@NEWMANAGERID,@ProductMappingId,@ApprovalType,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
		END;
	END
	
	IF((@ApprovalStatusId = @APPROVEDSTATUS AND @NEWMANAGERID = 0 ) OR @ISAdmin = 1)
	BEGIN
		UPDATE UserProductMapping SET
		Status= @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE Id= @ProductMappingId
		
	END
	ELSE IF(@ApprovalStatusId = @REJECTEDAPPROVAL)
	BEGIN
		UPDATE UserProductMapping SET
		Status= @ApprovalStatusId,
		Enable = 0,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE Id= @ProductMappingId
	END;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DELETEUSERBYUID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_DELETEUSERBYUID] 
	(
		@UID NVARCHAR(100),
		@ModifiedBy INT,
		@Status INT
	)
AS
BEGIN

	DECLARE @ID INT = 0;
	SELECT @ID = ID FROM USERS WHERE UID=@UID
	IF(NOT EXISTS(SELECT U.ID FROM Users U WITH (NOLOCK)
	INNER JOIN UserRoleMapping UR WITH (NOLOCK) ON UR.UserId = U.Id
	INNER JOIN RefRoles RR WITH (NOLOCK) ON RR.Id = UR.RoleId
	WHERE U.UID = @UID AND RR.RoleName='SuperAdmin') AND (SELECT COUNT(*) FROM USERS WHERE Status = 1 AND ManagerId = @ID) <= 0)
	BEGIN

		UPDATE USERS SET 
		 Status=@Status
		,ModifiedOn =GETDATE()
		,ModifiedBy =@ModifiedBy
		WHERE UID =@UID

		SELECT  1 AS ResponseCode,'Deleted Successfully' ResponseMessage

	END
	ELSE
	BEGIN
		SELECT  0 AS ResponseCode,CONCAT_WS(' ', 'In order to delete, Please change the users role under ',FullName) as ResponseMessage from Users
		where UID=@UID
	END;
END;








GO
/****** Object:  StoredProcedure [dbo].[USP_FORGOTPASSWORD]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_FORGOTPASSWORD]
	(
	@Uid NVARCHAR(100),
	@Email nvarchar(50),
	@Password nvarchar(50)
	)
AS
BEGIN
	--update Users set Password=@Password where Email=@Email and UID = @Uid 
	print('');
END;

GO
/****** Object:  StoredProcedure [dbo].[USP_GETACCESSLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GETACCESSLIST]  --'2AAA86AB-0561-4EE2-B879-61F863BB49FA'
	(
		@UserUID NVARCHAR(100) = NULL
	)
AS
BEGIN
	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT U.FullName,U.EmpId,RR.RoleDisplayName,ISNULL(U.ManagerId,0) AS managerId,
	UPM.Status ,U.FullName AS PointofContact,
	U.UID AS UserUID,UPM.Id AS ProductMappingId,UPM.ID,UPM.Enable,RP.Id AS productId,U.ID AS userId,
	UPM.UID,UPM.CreatedBy,RP.ProductName FROM Users U 
	LEFT OUTER JOIN Users UM on U.ManagerId = UM.Id
	INNER JOIN UserRoleMapping UR ON UR.UserId = U.Id
	INNER JOIN RefRoles RR ON RR.Id = UR.RoleId
	INNER JOIN UserProductMapping UPM ON UPM.UserId =U.Id 
	INNER JOIN RefProducts RP ON RP.Id = UPM.ProductId
	WHERE U.Id=@UserId 

END
GO
/****** Object:  StoredProcedure [dbo].[USP_GETALLPRODUCTS]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GETALLPRODUCTS]
	
	
AS
BEGIN
	SELECT 
		 Id
		,ProductName
		,Status
		,CreatedOn
		,CreatedBy
		,ModifiedBy
		,ModifiedOn
		,UID
		FROM RefProducts

END
GO
/****** Object:  StoredProcedure [dbo].[USP_GETALLROLES]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GETALLROLES] 
	
AS
BEGIN
	SELECT 
	Id,
	RoleName,
	RoleDisplayName,
	Status,
	UID

	  from RefRoles WITH (NOLOCK) where Status = 1;
END;








GO
/****** Object:  StoredProcedure [dbo].[USP_GETALLUSER]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GETALLUSER] 
	
AS
BEGIN
	SELECT 
	 U.Id
	,EmpId	
	,FirstName	
	,LastName	
	,U.FullName
	,Email	
	,Mobile	
	,U.Status	
	,CAST(StartDate AS DATE) StartDate
	,CAST(EndDate AS DATE) EndDate	
	,U.ManagerId	
	,U.CreatedOn	
	,U.CreatedBy	
	,U.ModifiedBy	
	,U.ModifiedOn	
	,U.UID
	,R.RoleDisplayName
	,UM.RoleId
	FROM USERS U WITH (NOLOCK)
	INNER JOIN UserRoleMapping UM WITH (NOLOCK) ON UM.UserId = U.Id
	INNER JOIN RefRoles R WITH (NOLOCK) ON R.Id = UM.RoleId 
	WHERE U.Status = 1
	ORDER BY STATUS DESC
END;








GO
/****** Object:  StoredProcedure [dbo].[USP_GETPRODUCTAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GETPRODUCTAPPROVALLIST]  --'16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100) = NULL,
		@Unique bit = 1
	)
AS
BEGIN
	DECLARE @UserAType INT,@RoleAType INT,@AccessAType INT,@UserId INT;

	SELECT @UserAType = ID FROM RefApprovalType WHERE ApprovalType='User'
	SELECT @RoleAType = ID FROM RefApprovalType WHERE ApprovalType='Role'
	SELECT @AccessAType = ID FROM RefApprovalType WHERE ApprovalType='Access'
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID
	IF(@Unique = 1)
	BEGIN
		SELECT U.FullName,U.EmpId,RR.RoleDisplayName,ISNULL(UAM.ManagerId,0) AS ApproverManager,
		RAT.ApprovalType,RAT.Id AS ApprovalTypeId, RAS.Status,UAM.FullName AS PointofContact,
		UPA.UID AS ApproverUID,U.UID AS UserUID,UPM.Id AS ProductMappingId,RP.ID ProductID,
		RP.ParentProductId,UPA.CreatedBy,UPA.UserId FROM Users U 
		LEFT OUTER JOIN Users UM on U.ManagerId = UM.Id
		INNER JOIN UserRoleMapping UR ON UR.UserId = U.Id
		INNER JOIN RefRoles RR ON RR.Id = UR.RoleId
		INNER JOIN UserProductMapping UPM ON UPM.UserId =U.Id 
		INNER JOIN UserProductApproval UPA ON UPA.ProductMappingId = UPM.Id 
		INNER JOIN RefApprovalType RAT ON RAT.Id = UPA.ApprovalType
		INNER JOIN RefApprovalStatus RAS ON RAS.Id = UPA.ApprovalStatus
		INNER JOIN RefProducts RP ON RP.Id = UPM.ProductId
		LEFT OUTER JOIN Users UAM on UPA.ManagerId = UAM.Id
		WHERE (UPA.ManagerId=@UserId OR UPA.CreatedBy=@UserId) AND RAT.Id IN (@UserAType,@RoleAType,@AccessAType)
		AND RP.ParentProductId IS NULL
	END
	ELSE
	BEGIN
		SELECT U.FullName,U.EmpId,RR.RoleDisplayName,ISNULL(UAM.ManagerId,0) AS ApproverManager,
		RAT.ApprovalType,RAT.Id AS ApprovalTypeId, RAS.Status,UAM.FullName AS PointofContact,
		UPA.UID AS ApproverUID,U.UID AS UserUID,UPM.Id AS ProductMappingId,RP.ID ProductID,
		RP.ParentProductId,UPA.CreatedBy,UPA.UserId FROM Users U 
		LEFT OUTER JOIN Users UM on U.ManagerId = UM.Id
		INNER JOIN UserRoleMapping UR ON UR.UserId = U.Id
		INNER JOIN RefRoles RR ON RR.Id = UR.RoleId
		INNER JOIN UserProductMapping UPM ON UPM.UserId =U.Id 
		INNER JOIN UserProductApproval UPA ON UPA.ProductMappingId = UPM.Id 
		INNER JOIN RefApprovalType RAT ON RAT.Id = UPA.ApprovalType
		INNER JOIN RefApprovalStatus RAS ON RAS.Id = UPA.ApprovalStatus
		INNER JOIN RefProducts RP ON RP.Id = UPM.ProductId
		LEFT OUTER JOIN Users UAM on UPA.ManagerId = UAM.Id
		WHERE (UPA.ManagerId=@UserId OR UPA.CreatedBy=@UserId) AND RAT.Id IN (@UserAType,@RoleAType,@AccessAType)
	END;

END
GO
/****** Object:  StoredProcedure [dbo].[USP_GETPRODUCTMAPPINGBYUSERID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GETPRODUCTMAPPINGBYUSERID]
	(
		@UserId INT =0
	)
AS
BEGIN
	
	SELECT UPM.Id,UPM.UserId,UPM.ProductId,UPM.Status,UPM.UID FROM UserProductMapping UPM 
	INNER JOIN RefProducts RP ON RP.Id = UPM.ProductId
	INNER JOIN Users U ON U.Id = UPM.UserId
	WHERE U.Id= @UserId

END
GO
/****** Object:  StoredProcedure [dbo].[USP_GETROLEAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GETROLEAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930',1
	(
		@RoleUID NVARCHAR(100) ,
		@ApprovalType INT
	)
AS
BEGIN
	

	DECLARE @UserAType INT,@RoleAType INT,@AccessAType INT,@UserId INT,@RoleId INT;

	SELECT @UserAType = ID FROM RefApprovalType WHERE ApprovalType='User'
	SELECT @RoleAType = ID FROM RefApprovalType WHERE ApprovalType='Role'
	SELECT @AccessAType = ID FROM RefApprovalType WHERE ApprovalType='Access'
	
	SELECT @RoleId = ID FROM RefRoles WHERE UID=@RoleUID

	SELECT UPA.Id, U.FullName,U.EmpId,RR.RoleDisplayName,ISNULL(UM.ManagerId,0) AS ApproverManager,
	RAT.ApprovalType,RAT.Id AS ApprovalTypeId, RAS.Status,UM.FullName AS PointofContact,
	UPA.UID AS ApproverUID,U.UID AS UserUID FROM UserApproval UPA
	INNER JOIN Users U ON U.Id = UPA.UserId
	LEFT OUTER JOIN Users UM on UPA.ManagerId = UM.Id
	INNER JOIN UserRoleMapping UR ON UR.UserId = U.Id
	INNER JOIN RefRoles RR ON RR.Id = UR.RoleId
	INNER JOIN RefApprovalType RAT ON RAT.Id = UPA.ApprovalType
	INNER JOIN RefApprovalStatus RAS ON RAS.Id = UPA.ApprovalStatus
	WHERE UPA.ManagerId=U.ManagerId AND RR.Id = @RoleId AND RAT.Id IN (@UserAType,@RoleAType,@AccessAType)

END
GO
/****** Object:  StoredProcedure [dbo].[USP_GETROLEBYUID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GETROLEBYUID]  --'8cb4ff0e-9686-4f39-a427-f3cdd012a087'
	(
		@UID NVARCHAR(100) 
	)
AS
BEGIN
	SELECT 
	 U.Id	
	,U.Status		
	,U.ManagerId	
	,U.CreatedOn	
	,U.CreatedBy	
	,U.ModifiedBy	
	,U.ModifiedOn	
	,U.UID
	,U.RoleName
	,U.RoleDisplayName
	,UD.FullName AS ManagerName
	FROM RefRoles U WITH (NOLOCK)
	INNER JOIN UserRoleMapping UM WITH (NOLOCK) ON UM.UserId = U.Id
	INNER JOIN RefRoles R WITH (NOLOCK) ON R.Id = UM.RoleId 
	LEFT OUTER JOIN Users UD WITH (NOLOCK) on U.ManagerId = UD.Id
	WHERE U.UID =@UID
END;
GO
/****** Object:  StoredProcedure [dbo].[USP_GETUSERAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GETUSERAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930',1
	(
		@UserUID NVARCHAR(100) ,
		@ApprovalType INT
	)
AS
BEGIN
	

	DECLARE @UserAType INT,@RoleAType INT,@AccessAType INT,@UserId INT;

	SELECT @UserAType = ID FROM RefApprovalType WHERE ApprovalType='User'
	SELECT @RoleAType = ID FROM RefApprovalType WHERE ApprovalType='Role'
	SELECT @AccessAType = ID FROM RefApprovalType WHERE ApprovalType='Access'
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT UPA.Id, U.FullName,U.EmpId,RR.RoleDisplayName,ISNULL(UM.ManagerId,0) AS ApproverManager,
	RAT.ApprovalType,RAT.Id AS ApprovalTypeId, RAS.Status,UM.FullName AS PointofContact,
	UPA.UID AS ApproverUID,U.UID AS UserUID,UPA.CreatedBy FROM UserApproval UPA
	INNER JOIN Users U ON U.Id = UPA.UserId
	LEFT OUTER JOIN Users UM on UPA.ManagerId = UM.Id
	INNER JOIN UserRoleMapping UR ON UR.UserId = U.Id
	INNER JOIN RefRoles RR ON RR.Id = UR.RoleId
	INNER JOIN RefApprovalType RAT ON RAT.Id = UPA.ApprovalType
	INNER JOIN RefApprovalStatus RAS ON RAS.Id = UPA.ApprovalStatus
	WHERE UPA.ManagerId=@UserId OR UPA.CreatedBy = @UserId AND RAT.Id IN (@UserAType,@RoleAType,@AccessAType)

END
GO
/****** Object:  StoredProcedure [dbo].[USP_GETUSERBYUID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GETUSERBYUID]  --'8cb4ff0e-9686-4f39-a427-f3cdd012a087'
	(
		@UID NVARCHAR(100) 
	)
AS
BEGIN
	SELECT 
	 U.Id
	,U.EmpId	
	,U.FirstName	
	,U.LastName	
	,U.FullName
	,U.Email	
	,U.Mobile	
	,U.Status	
	,CAST(U.StartDate AS DATE) StartDate
	,CAST(U.EndDate AS DATE) EndDate		
	,U.ManagerId	
	,U.CreatedOn	
	,U.CreatedBy	
	,U.ModifiedBy	
	,U.ModifiedOn	
	,U.UID
	,R.RoleDisplayName
	,UM.RoleId
	,UD.FullName AS ManagerName
	FROM USERS U WITH (NOLOCK)
	INNER JOIN UserRoleMapping UM WITH (NOLOCK) ON UM.UserId = U.Id
	INNER JOIN RefRoles R WITH (NOLOCK) ON R.Id = UM.RoleId 
	LEFT OUTER JOIN Users UD WITH (NOLOCK) on U.ManagerId = UD.Id
	WHERE U.UID =@UID
END;








GO
/****** Object:  StoredProcedure [dbo].[USP_LOGIN]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_LOGIN]
	(
	@UserId nvarchar(20) = null,
	@Email nvarchar(50) = null,
	@Password nvarchar(50)
	)
AS
BEGIN
	SELECT U.Id,U.UID, U.EmpId,U.FirstName,U.LastName,U.FullName,U.Email,U.Mobile,U.Status,U.ManagerId, R.RoleName FROM 
	Users U
	INNER JOIN UserRoleMapping URM ON URM.UserId = U.Id
	INNER JOIN RefRoles R ON R.Id = URM.RoleId
	
	
	WHERE ( Email = @Email or EmpId =  @UserId ) and (Password = @Password) and u.status = 1
END;
GO
/****** Object:  StoredProcedure [dbo].[USP_POSTUSER]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_POSTUSER] 
	(
	@Id			INT = 0,
	@EmpId		VARCHAR(50),
	@FirstName	VARCHAR(50),
	@LastName	VARCHAR(50),
	@FullName   NVARCHAR(50),
	@Email		VARCHAR(50),
	@Mobile		VARCHAR(50) = NULL,
	@Password	VARCHAR(50),
	@StartDate	datetime,
	@EndDate	datetime,
	@ManagerId	INT,
	@CreatedBy  INT,
	@UID NVARCHAR(100)= NULL,
	@RoleId     INT,
	@Status INT = 0
	)
AS
BEGIN

	DECLARE @SUPERADMINCOUNT INT =0, @ROLENAME VARCHAR(50)='', @MESSAGE VARCHAR(100)='Successfully Saved';
	
	SELECT @ROLENAME = RoleName FROM RefRoles WITH (NOLOCK) WHERE ID= @RoleId ;
	SELECT @SUPERADMINCOUNT = COUNT(*) FROM Users U WITH (NOLOCK)
	INNER JOIN UserRoleMapping UR WITH (NOLOCK) ON UR.UserId = U.Id
	INNER JOIN RefRoles RR WITH (NOLOCK) ON RR.Id = UR.RoleId
	WHERE RR.RoleName='SuperAdmin'
	PRINT(@SUPERADMINCOUNT)


	IF( @ROLENAME = 'SuperAdmin' AND @SUPERADMINCOUNT > 0 AND EXISTS(SELECT U.ID FROM Users U WITH (NOLOCK)
	INNER JOIN UserRoleMapping UR WITH (NOLOCK) ON UR.UserId = U.Id
	INNER JOIN RefRoles RR WITH (NOLOCK) ON RR.Id = UR.RoleId
	WHERE U.Id != @Id AND RR.RoleName='SuperAdmin'))
	BEGIN
			SELECT 0 AS ResponseCode,'Only one Super Admin is allowed' ResponseMessage
			return;
	END;


	IF(@Id = 0 or @Id is null)
	BEGIN
		INSERT INTO Users(EmpId,FirstName,LastName,FullName,Email,Mobile,Password,Status,StartDate,EndDate,ManagerId,CreatedOn,CreatedBy)
		VALUES(@EmpId,@FirstName,@LastName,@FullName,@Email,@Mobile,@Password,@Status,@StartDate,@EndDate,@ManagerId,GETDATE(),@CreatedBy);

		 SELECT @Id = SCOPE_IDENTITY() 

		insert into UserRoleMapping(UserId,RoleId,CreatedBy,CreatedOn)
		values(@Id,@RoleId,@CreatedBy,GETDATE())

	END;
	ELSE
	BEGIN
		UPDATE Users SET
		FirstName = @FirstName,
		LastName= @LastName,
		FullName =@FullName,
		EmpId=@EmpId,
		Email= @Email,
		Mobile =@Mobile,
		Password = @Password,
		StartDate =@StartDate,
		EndDate =@EndDate,
		ManagerId=@ManagerId,
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID

		update UserRoleMapping set
		RoleId =@RoleId
		,ModifiedBy = @CreatedBy
		,ModifiedOn = GETDATE()
		where UserId= @Id;
		SET @MESSAGE = 'Updated Successfully'
	END;

	SELECT Id
	,EmpId	
	,FirstName	
	,LastName	
	,Email	
	,Mobile	
	,Status	
	,StartDate	
	,EndDate	
	,ManagerId	
	,CreatedOn	
	,CreatedBy	
	,ModifiedBy	
	,ModifiedOn	
	,UID, 1 AS ResponseCode, @MESSAGE ResponseMessage FROM Users WHERE Id =@Id
	
END;








GO
/****** Object:  StoredProcedure [dbo].[USP_UPDATACCESSCONTROLNotusing]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_UPDATACCESSCONTROLNotusing] 
(
	@UserId  INT = 0,
    @ProductId INT  = 0,
	@CreatedBy INT = 0
	)
AS
BEGIN
	DECLARE @STATUS INT = 0, @ID INT =0;

	SET @STATUS = CASE WHEN (SELECT 1 FROM Users U WITH (NOLOCK)
	INNER JOIN UserRoleMapping UR WITH (NOLOCK) ON UR.UserId = U.Id
	INNER JOIN RefRoles RR WITH (NOLOCK) ON RR.Id = UR.RoleId
	WHERE RR.RoleName='ITSupportAdmin' OR RR.RoleName='SuperAdmin'  AND U.Id= 2) = 1 THEN 1 ELSE 0 END;
	PRINT(@STATUS)

	IF(@UserId = 0 or @UserId is null)
	BEGIN
		INSERT INTO UserProductMapping (UserId,ProductId,Status,CreatedBy,CreatedOn)
		VALUES (@UserId,@ProductId,@STATUS,@CreatedBy,GETDATE());
		SET @ID = SCOPE_IDENTITY()
	END;
	ELSE
	BEGIN
		UPDATE UserProductMapping SET
		Status = @STATUS,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UserId=@UserId AND ProductId=@ProductId
		SET @ID = SCOPE_IDENTITY()
	END;

	IF(@STATUS = 0)
	BEGIN
		DECLARE @MANAGERID BIGINT = (SELECT ManagerId FROM Users WHERE ID =@UserId)
		INSERT INTO UserProductApproval (UserId,ManagerId,ProductMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@UserId,@MANAGERID,@ID,@STATUS,@CreatedBy,GETDATE())
		PRINT(@STATUS);
	END;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UPDATEROLE]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_UPDATEROLE]  
 (
  @UID NVARCHAR(100)= NULL  
 ,@RoleDisplayName VARCHAR(50) 
 ,@CreatedBy INT
 )  
AS  
BEGIN  
	
	
	update RefRoles set RoleDisplayName=@RoleDisplayName, ModifiedBy = @CreatedBy, ModifiedOn = GETDATE()  where UID = @UID
	
	
	SELECT Id
	,RoleDisplayName
	,Status	
	,ManagerId	
	,ModifiedBy	
	,ModifiedOn	
	,UID, 1 AS ResponseCode,'Successfully Updated' ResponseMessage FROM RefRoles WHERE UID =@UID
END;
GO
/****** Object:  StoredProcedure [dbo].[USP_UPDATEROLEAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_UPDATEROLEAPPROVAL]
	(
		@Id INT = 0,
		@UserId INT =0,
		@ManagerId INT = 0,
		@ApprovalTypeId INT = 0,
		@ApprovalStatus INT =0,
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);


	IF(NOT EXISTS(SELECT * FROM UserApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO UserApproval(UserId,ManagerId,ApprovalType,ApprovalStatus,CreatedBy,CreatedOn)

		VALUES(@UserId,@ManagerId,@ApprovalTypeId,@ApprovalStatus,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE UserApproval SET
		ApprovalStatus = @ApprovalStatus,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		IF((@ApprovalStatus = @APPROVEDSTATUS OR @ApprovalStatus =  @REVIEWEERSTATUS) AND @NEWMANAGERID != 0)
		BEGIN
			INSERT INTO UserApproval(UserId,ManagerId,ApprovalType,ApprovalStatus,CreatedBy,CreatedOn)
			VALUES(@UserId,@NEWMANAGERID,@ApprovalTypeId,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
			print('');
END;
	END
	
	IF(@ApprovalStatus = @APPROVEDSTATUS AND @NEWMANAGERID = 0 )
	BEGIN
		UPDATE RefRoles SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@UserId
	END;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_UPDATEUSERAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_UPDATEUSERAPPROVAL]
	(
		@Id INT = 0,
		@UserId INT =0,
		@ManagerId INT = 0,
		@ApprovalTypeId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0,@ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,
	@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'
	
	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	 IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;

	IF EXISTS(SELECT 1 FROM UserApproval WHERE UserId = @UserId AND ApprovalStatus = @PENDINGAPPROVAL AND CreatedBy = @CreatedBy)
	BEGIN
		RETURN;
	END;
	

	IF(NOT EXISTS(SELECT * FROM UserApproval WHERE UID = @UID))
	BEGIN
		
		INSERT INTO UserApproval(UserId,ManagerId,ApprovalType,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@UserId,@ManagerId,@ApprovalTypeId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE UserApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		IF((@ApprovalStatusId =  @REVIEWEERSTATUS OR @ApprovalStatusId =  @FORWARD) AND @ISAdmin != 1)
		BEGIN
			SET @CreatedBy = (SELECT CreatedBy FROM UserApproval WHERE UID = @UID);
			INSERT INTO UserApproval(UserId,ManagerId,ApprovalType,ApprovalStatus,CreatedBy,CreatedOn)
			VALUES(@UserId,@NEWMANAGERID,@ApprovalTypeId,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
			print('');
		END;
	END
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR ((@ApprovalStatusId =  @FORWARD OR @ApprovalStatusId = @APPROVEDSTATUS) AND @ISAdmin = 1))
	BEGIN
		UPDATE Users SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@UserId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_DELETECOUNTRYSTATEMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_DELETECOUNTRYSTATEMAPPING] 
	(
		@Id						INT = 0,
		@ModifiedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN
	
	UPDATE [product_owner].[CountryStateMapping] SET
	ModifiedBy = @ModifiedBy,
	ModifiedOn = GETDATE(),
	Status = 0
	WHERE UID = @UID
	

	SELECT 
          SM.Id,
		  SM.CountryId,
		  SM.StateId,
		  SM.Status,
		  SM.UID,
		  C.CountryName,
		  S.StateName,
		  C.CountryCode,
		  S.StateCode
	, 1 AS ResponseCode,'Action update and Sent for Review' AS ResponseMessage FROM
	[product_owner].[CountryStateMapping] SM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[STATE] S ON S.ID = SM.STATEID
	WHERE SM.uid =@UID
	
END;


GO
/****** Object:  StoredProcedure [product_owner].[USP_GET_REGULATION_SETUP_APPROVAL_LIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_GET_REGULATION_SETUP_APPROVAL_LIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT CSM.HistoryId, CSA.Id,UK.FullName,'Regulation Compliance' as RegulationType,
	CSA.ApprovalStatus AS StatusId,RAS.Status,CSA.UID as ApproverUID,CSA.CreatedBy,U.FullName AS ApprovedBy,
	CSM.ComplianceName AS DisplayName
	FROM [product_owner].RegulationSetupComplianceApproval CSA
	INNER JOIN [product_owner].RegulationSetupComplianceHistory CSM ON CSM.HistoryId = CSA.HistoryId
	INNER JOIN [dbo].Users U ON U.Id = CSA.ManagerId
	INNER JOIN [dbo].Users UK ON UK.Id = CSA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CSA.ApprovalStatus
	WHERE CSA.ManagerId=@UserId OR CSA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETALLCOUNTRY]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETALLCOUNTRY] 
	
AS
BEGIN

	--SELECT Id
 --         ,CountryName
 --         ,CountryCode
 --         ,CountryCodeNumber
 --         ,FinancialStartDate
 --         ,FinancialEndDate
 --         ,CreatedOn
 --         ,CreatedBy
 --         ,ModifiedBy
 --         ,ModifiedOn
 --         ,UID
	--FROM [product_owner].[Country] 
	--WHERE Status = 1

	
select     CS.CountryId AS Id
          ,C.CountryName
          ,C.CountryCode
          ,C.CountryCodeNumber
          ,C.FinancialStartDate
          ,C.FinancialEndDate
          ,C.CreatedOn
          ,C.CreatedBy
          ,C.ModifiedBy
          ,C.ModifiedOn
          ,C.UID from product_owner.CountryStateMapping CS
INNER JOIN product_owner.Country C ON C.ID = CS.COUNTRYID
GROUP BY CS.CountryId,C.CountryName, C.CountryCode
          ,C.CountryCodeNumber
          ,C.FinancialStartDate
          ,C.FinancialEndDate
          ,C.CreatedOn
          ,C.CreatedBy
          ,C.ModifiedBy
          ,C.ModifiedOn
          ,C.UID
	
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETALLCOUNTRYMASTER]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETALLCOUNTRYMASTER] 
	
AS
BEGIN

	SELECT Id
          ,CountryName
          ,CountryCode
          ,CountryCodeNumber
          ,FinancialStartDate
          ,FinancialEndDate
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	FROM [product_owner].[Country] 
	WHERE Status = 1

	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETALLENTITYTYPES]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [product_owner].[USP_GETALLENTITYTYPES] 
	
AS
BEGIN

	SELECT Id
          ,EntityType
          ,EntityTypeCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	FROM [product_owner].[EntityType] 
	WHERE Status = 1
	
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETALLMAJORINDUSTRY]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETALLMAJORINDUSTRY] 
	
AS
BEGIN

	SELECT Id
          ,MajorIndustryName
          ,MajorIndustryCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	FROM [product_owner].[MajorIndustry] 
	WHERE Status = 1
	
	
END;
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETALLREGULATIONGROUP]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_GETALLREGULATIONGROUP] 
	
AS
BEGIN

	SELECT Id
          ,RegulationGroupName
          ,RegulationGroupCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	FROM [product_owner].[RegulationGroup] 
	WHERE Status = 1
	
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT  CA.Id, C.Id as CountryId,C.CountryName,C.CountryCode,C.CountryCodeNumber,C.FinancialStartDate,
	C.FinancialEndDate,CONCAT(LEFT(FORMAT(c.FinancialStartDate, 'MMMM'),3), ' - ', LEFT(FORMAT(c.FinancialEndDate, 'MMMM'),3)) as FinancialYear ,
	U.FullName,CA.ApprovalStatus AS StatusId,RAS.Status,C.CreatedBy, 'Add Country' Type,
	CA.UID,C.UID AS CountryUID,
	U.FullName AS approvedBy FROM [product_owner].COUNTRY C
	INNER JOIN [product_owner].COUNTRYAPPROVAL CA ON CA.CountryId= c.Id
	INNER JOIN [dbo].Users U ON U.Id = CA.MANAGERID
	INNER JOIN [dbo].Users UK ON UK.Id = CA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CA.ApprovalStatus
	WHERE CA.ManagerId=@UserId OR CA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYBYUID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYBYUID] 
	(
		@UID NVARCHAR(100)
	)
AS
BEGIN

	SELECT Id
          ,CountryName
          ,CountryCode
          ,CountryCodeNumber
          ,FinancialStartDate
          ,FinancialEndDate
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	FROM [product_owner].[Country] WHERE UID = @UID;
	
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYENTITYTYPEMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYENTITYTYPEMAPPING]
	
AS
BEGIN
	SELECT  
	SM.Id,
	SM.CountryId,
	SM.EntityTypeId,
	SM.Status,
	SM.UID,
	C.CountryName,
	S.EntityType,
	C.CountryCode,
	S.EntityTypeCode
	FROM [product_owner].[CountryEntityTypeMapping] SM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[EntityType] S ON S.ID = SM.EntityTypeID
	WHERE SM.Status = 1
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYENTITYTYPEMAPPINGAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYENTITYTYPEMAPPINGAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT CSA.CountryEntityTypeMappingId, CSA.Id,C.CountryName,S.EntityType,UK.FullName,
	CSA.ApprovalStatus AS StatusId,RAS.Status,CSA.UID,CSA.CreatedBy,
	U.FullName AS approvedBy FROM [product_owner].CountryEntityTypeMappingApproval CSA
	INNER JOIN [product_owner].CountryEntityTypeMapping CSM ON CSM.Id = CSA.CountryEntityTypeMappingId
	INNER JOIN [product_owner].Country C ON C.Id = CSM.CountryId
	INNER JOIN [product_owner].EntityType S ON S.Id= CSM.EntityTypeId
	INNER JOIN [dbo].Users U ON U.Id = CSA.ManagerId
	INNER JOIN [dbo].Users UK ON UK.Id = CSA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CSA.ApprovalStatus
	WHERE CSA.ManagerId=@UserId OR CSA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYMAJORINDUSTRYMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYMAJORINDUSTRYMAPPING]
	
AS
BEGIN
	SELECT  
	SM.Id,
	SM.CountryId,
	SM.MajorIndustryId,
	SM.Status,
	SM.UID,
	C.CountryName,
	S.MajorIndustryName,
	C.CountryCode,
	S.MajorIndustryCode
	FROM [product_owner].[CountryMajorIndustryMapping] SM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[MajorIndustry] S ON S.ID = SM.MAJORINDUSTRYID
	WHERE SM.Status = 1 AND C.STATUS = 1 AND S.STATUS = 1
END;
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYMAJORINDUSTRYMAPPINGAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYMAJORINDUSTRYMAPPINGAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT CMA.CountryMajorIndustryMappingId,CMA.Id,C.CountryName,MA.MajorIndustryName,U.FullName,
	CMA.ApprovalStatus AS StatusId,RAS.Status,CMA.UID,CMM.CreatedBy FROM [product_owner].CountryMajorIndustryMappingApproval CMA
	INNER JOIN [product_owner].CountryMajorIndustryMapping CMM ON CMM.Id = CMA.CountryMajorIndustryMappingId
	INNER JOIN [product_owner].Country C ON C.Id = CMM.CountryId
	INNER JOIN [product_owner].MajorIndustry MA ON MA.Id = CMM.MajorIndustryId
	INNER JOIN [dbo].Users U ON U.Id = CMA.ManagerId
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CMA.ApprovalStatus
	WHERE CMA.ManagerId=@UserId OR CMA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYMAPPINGFORREGULATIONSETUP]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYMAPPINGFORREGULATIONSETUP]
	(
		@CountryId INT = NULL
	)
AS
BEGIN
select * from [product_owner].[CountryStateMapping] WHERE CountryId=@CountryId AND Status = 1;

select * from [product_owner].[CountryRegulationGroupMapping] WHERE CountryId=@CountryId AND Status = 1;

select * from [product_owner].[IndustryMapping] WHERE CountryId=@CountryId AND Status = 1;

select * from [product_owner].[EntityType] WHERE Status = 1;

END;
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYREGULATIONGROUPMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYREGULATIONGROUPMAPPING]
	
AS
BEGIN
	SELECT  
	SM.Id,
	SM.CountryId,
	SM.RegulationGroupId,
	SM.Status,
	SM.UID,
	C.CountryName,
	S.RegulationGroupName,
	C.CountryCode,
	S.RegulationGroupCode
	FROM [product_owner].[CountryRegulationGroupMapping] SM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[RegulationGroup] S ON S.ID = SM.RegulationGroupID
	WHERE SM.Status = 1
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYREGULATIONGROUPMAPPINGAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYREGULATIONGROUPMAPPINGAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT CSA.CountryRegulationGroupMappingId, CSA.Id,C.CountryName,S.RegulationGroupName,UK.FullName,
	CSA.ApprovalStatus AS StatusId,RAS.Status,CSA.UID,CSA.CreatedBy,U.FullName AS ApprovedBy
	FROM [product_owner].CountryRegulationGroupMappingApproval CSA
	INNER JOIN [product_owner].CountryRegulationGroupMapping CSM ON CSM.Id = CSA.CountryRegulationGroupMappingId
	INNER JOIN [product_owner].Country C ON C.Id = CSM.CountryId
	INNER JOIN [product_owner].RegulationGroup S ON S.Id= CSM.RegulationGroupId
	INNER JOIN [dbo].Users U ON U.Id = CSA.ManagerId
	INNER JOIN [dbo].Users UK ON UK.Id = CSA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CSA.ApprovalStatus
	WHERE CSA.ManagerId=@UserId OR CSA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYSTATEMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYSTATEMAPPING]
	
AS
BEGIN
	SELECT  
	SM.Id,
	SM.CountryId,
	SM.StateId,
	SM.Status,
	SM.UID,
	C.CountryName,
	S.StateName,
	C.CountryCode,
	S.StateCode
	FROM [product_owner].[CountryStateMapping] SM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[STATE] S ON S.ID = SM.STATEID
	WHERE SM.Status = 1 AND C.STATUS = 1 AND S.STATUS = 1
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETCOUNTRYSTATEMAPPINGAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [product_owner].[USP_GETCOUNTRYSTATEMAPPINGAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT  CSA.CountryStateMappingId, CSA.Id,C.CountryName,S.StateName,UK.FullName,
	CSA.ApprovalStatus AS StatusId,RAS.Status,CSA.UID,CSM.CreatedBy ,U.FullName AS ApprovedBy
	FROM [product_owner].CountryStateMappingApproval CSA
	INNER JOIN [product_owner].CountryStateMapping CSM ON CSM.Id = CSA.CountryStateMappingId
	INNER JOIN [product_owner].Country C ON C.Id = CSM.CountryId
	INNER JOIN [product_owner].STATE S ON S.Id= CSM.StateId
	INNER JOIN [dbo].Users U ON U.Id = CSA.ManagerId
	INNER JOIN [dbo].Users UK ON UK.Id = CSA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CSA.ApprovalStatus
	WHERE CSA.ManagerId=@UserId OR CSA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETENTITYTYPEAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETENTITYTYPEAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT  CA.Id, C.Id as EntityTypeId,C.EntityType,C.EntityTypeCode,
	UK.FullName,CA.ApprovalStatus AS StatusId,RAS.Status,C.CreatedBy,U.FullName AS ApprovedBy, 'Add EntityType' Type,CA.UID
	FROM [product_owner].EntityType C
	INNER JOIN [product_owner].EntityTypeApproval CA ON CA.EntityTypeId= c.Id
	INNER JOIN [dbo].Users U ON U.Id = CA.MANAGERID
	INNER JOIN [dbo].Users UK ON UK.Id = CA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CA.ApprovalStatus
	WHERE CA.ManagerId=@UserId OR CA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETINDUSTRYMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETINDUSTRYMAPPING]
	
AS
BEGIN
	SELECT  
	IM.Id,
	IM.CountryId,
	IM.MajorIndustryId,
	IM.MinorIndustryId,
	IM.Status,
	IM.UID,
	C.CountryName,
	MA.MajorIndustryName,
	MI.MinorIndustryName,
	C.CountryCode,
	MA.MajorIndustryCode,
	MI.MinorIndustryCode
	FROM [product_owner].[IndustryMapping] IM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = IM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[MajorIndustry] MA ON MA.ID = IM.MAJORINDUSTRYID
	INNER JOIN [PRODUCT_OWNER].[MinorIndustry] MI ON MI.ID = IM.MINORINDUSTRYID
	WHERE IM.Status = 1 AND C.STATUS = 1 AND MA.STATUS = 1 AND MI.STATUS = 1
END;
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETINDUSTRYMAPPINGAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [product_owner].[USP_GETINDUSTRYMAPPINGAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT  IA.IndustryMappingId, IA.Id,C.CountryName,MI.MajorIndustryName,M.MinorIndustryName,U.FullName,
	IA.ApprovalStatus AS StatusId,RAS.Status,IA.UID,IM.CreatedBy 
	FROM [product_owner].IndustryMappingApproval IA
	INNER JOIN [product_owner].IndustryMapping IM ON IM.Id = IA.IndustryMappingId
	INNER JOIN [product_owner].Country C ON C.Id = IM.CountryId
	INNER JOIN [product_owner].MajorIndustry MI ON MI.Id= IM.MajorIndustryId
	INNER JOIN [product_owner].MinorIndustry M ON M.Id= IM.MinorIndustryId
	INNER JOIN [dbo].Users U ON U.Id = IA.ManagerId
	INNER JOIN [dbo].Users UK ON UK.Id = IM.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = IA.ApprovalStatus
	WHERE IA.ManagerId=@UserId OR IA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETMAJORINDUSTRYAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_GETMAJORINDUSTRYAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

		SELECT MA.Id,M.Id as MajorIndustryId,M.MajorIndustryName,M.MajorIndustryCode,U.FullName,
	MA.ApprovalStatus AS StatusId,RAS.Status,M.CreatedBy, 'Add Major Industry' Type,MA.UID,M.UID as MajorIndustryUID
	FROM [product_owner].MajorIndustry M
	INNER JOIN [product_owner].MajorIndustryApproval MA ON MA.MajorIndustryId= M.Id
	INNER JOIN Users U ON U.Id = MA.MANAGERID
	INNER JOIN [dbo].Users UK ON UK.Id = MA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = MA.ApprovalStatus
	WHERE MA.ManagerId=@UserId OR MA.CreatedBy = @UserId
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETMAJORINDUSTRYBYID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_GETMAJORINDUSTRYBYID] 
	(
		@CountryId INT = NULL
	)
AS
BEGIN

	SELECT Id
          ,MajorIndustryName
          ,MajorIndustryCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	FROM [product_owner].[MajorIndustry]
	WHERE CountryId=@CountryId AND Status = 1
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETMAJORINDUSTRYBYUID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_GETMAJORINDUSTRYBYUID] 
	(
		@UID NVARCHAR(100)
	)
AS
BEGIN

	SELECT Id
          ,MajorIndustryName
          ,MajorIndustryCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	FROM [product_owner].[MajorIndustry] WHERE UID = @UID;
	
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_GETMAJORMINORINDUSTRYMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETMAJORMINORINDUSTRYMAPPING]
	
AS
BEGIN
	SELECT  
	SM.Id,
	SM.MajorIndustryId,
	SM.MinorIndustryId,
	SM.Status,
	SM.UID,
	C.MajorIndustryName,
	S.MinorIndustryName,
	C.MajorIndustryCode,
	S.MinorIndustryCode
	FROM [product_owner].[MajorMinorIndustryMapping] SM
	INNER JOIN [PRODUCT_OWNER].[MajorIndustry] C ON C.ID = SM.MAJORINDUSTRYID
	INNER JOIN [PRODUCT_OWNER].[MinorIndustry] S ON S.ID = SM.MINORINDUSTRYID
	WHERE SM.Status = 1 AND C.STATUS = 1 AND S.STATUS = 1
END;
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETMAJORMINORINDUSTRYMAPPINGAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [product_owner].[USP_GETMAJORMINORINDUSTRYMAPPINGAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT MMA.MajorMinorIndustryMappingId,MMA.Id,MA.MajorIndustryName,MI.MinorIndustryName,U.FullName,
	MMA.ApprovalStatus AS StatusId,RAS.Status,MMA.UID,MMA.CreatedBy FROM [product_owner].MajorMinorIndustryMappingApproval MMA
	INNER JOIN [product_owner].MajorMinorIndustryMapping MMM ON MMM.Id = MMA.MajorMinorIndustryMappingId
	INNER JOIN [product_owner].MajorIndustry MA ON MA.Id = MMM.MajorIndustryId
	INNER JOIN [product_owner].MinorIndustry MI ON MI.Id = MMM.MinorIndustryId
	INNER JOIN [dbo].Users U ON U.Id = MMA.ManagerId
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = MMA.ApprovalStatus
	WHERE MMA.ManagerId=@UserId OR MMA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETMINORINDUSTRYAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [product_owner].[USP_GETMINORINDUSTRYAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT MA.Id,MI.Id as MinorIndustryId,M.MajorIndustryName,M.MajorIndustryCode,  MA.Id, MI.Id as MinorIndustryId,MI.MinorIndustryName,MI.MinorIndustryCode,U.FullName,
	MA.ApprovalStatus AS StatusId,RAS.Status, 'Add Minor Industry' Type, MA.UID,MI.UID as MinorIndustryUID
	FROM [product_owner].MinorIndustry MI
	INNER JOIN [product_owner].MinorIndustryApproval MA ON MA.MinorIndustryId= MI.Id
	INNER JOIN [product_owner].MajorIndustry M ON M.Id = MI.MajorIndustryId
	INNER JOIN Users U ON U.Id = MA.MANAGERID
	INNER JOIN [dbo].Users UK ON UK.Id = MA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = MA.ApprovalStatus
	WHERE MA.ManagerId=@UserId OR MA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETMINORINDUSTRYBYID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_GETMINORINDUSTRYBYID] 
	(
		@MajorIndustryId INT = NULL
	)
AS
BEGIN

	SELECT Id
          ,MinorIndustryName
          ,MinorIndustryCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	FROM [product_owner].[MinorIndustry]
	WHERE MajorIndustryId=@MajorIndustryId AND Status = 1
	
END;
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETREGULATIONGROUPAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETREGULATIONGROUPAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT  CA.Id, C.Id as RegulationGroupId,C.RegulationGroupName,C.RegulationGroupCode,
	UK.FullName,CA.ApprovalStatus AS StatusId,RAS.Status,C.CreatedBy,U.FullName AS ApprovedBy, 'Add RegulationGroup' Type,CA.UID
	FROM [product_owner].RegulationGroup C
	INNER JOIN [product_owner].RegulationGroupApproval CA ON CA.RegulationGroupId= c.Id
	INNER JOIN [dbo].Users U ON U.Id = CA.MANAGERID
	INNER JOIN [dbo].Users UK ON UK.Id = CA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CA.ApprovalStatus
	WHERE CA.ManagerId=@UserId OR CA.CreatedBy = @UserId

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETSTATEAPPROVALLIST]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [product_owner].[USP_GETSTATEAPPROVALLIST] -- '16193229-D8DC-4B89-8209-4E2FADE34930'
	(
		@UserUID NVARCHAR(100)
		
	)
AS
BEGIN
	

	DECLARE @UserId INT;

	
	SELECT @UserId = ID FROM Users WHERE UID=@UserUID

	SELECT C.Id as CountryId,C.CountryName,C.CountryCode, 
	CA.Id, S.Id as StateId,S.StateName,S.StateCode,UK.FullName,
	CA.ApprovalStatus AS StatusId,RAS.Status, 'Add State' Type,CONCAT(LEFT(FORMAT(c.FinancialStartDate, 'MMMM'),3), ' - ', LEFT(FORMAT(c.FinancialEndDate, 'MMMM'),3)) as FinancialYear
	,CA.UID,U.FullName AS ApprovedBy
	FROM [product_owner].STATE S
	INNER JOIN [product_owner].STATEAPPROVAL CA ON CA.StateId= S.Id
	INNER JOIN [product_owner].Country C ON C.Id = S.CountryId
	INNER JOIN Users U ON U.Id = CA.MANAGERID
	INNER JOIN [dbo].Users UK ON UK.Id = CA.CreatedBy
	INNER JOIN [dbo].RefApprovalStatus RAS ON RAS.Id = CA.ApprovalStatus
	WHERE CA.ManagerId=@UserId OR CA.CreatedBy = @UserId
	ORDER BY CA.CreatedOn DESC

END
GO
/****** Object:  StoredProcedure [product_owner].[USP_GETSTATEBYID]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_GETSTATEBYID] 
	(
		@CountryId INT = NULL
	)
AS
BEGIN

	SELECT S.Id
		  ,StateName
		  ,StateCode
          ,S.CreatedOn
          ,S.CreatedBy
          ,S.ModifiedBy
          ,S.ModifiedOn
          ,S.UID
	FROM [product_owner].[STATE] S
	WHERE S.CountryId=@CountryId AND S.Status = 1
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTCOUNTRY]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_POSTCOUNTRY] 
	(
		@Id						INT = 0,
		@CountryName			VARCHAR(50)= NULL,
		@CountryCode			VARCHAR(50)= NULL,
		@CountryCodeNumber		VARCHAR(50)= NULL,
		@FinancialStartDate		datetime= NULL,
		@FinancialEndDate		datetime= NULL,
		@CreatedBy				INT = 0,
		@ModifiedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN
	IF EXISTS( SELECT * FROM product_owner.Country WHERE CountryName = @CountryName)
	BEGIN
		SELECT Id
          ,CountryName
          ,CountryCode
          ,CountryCodeNumber
          ,FinancialStartDate
          ,FinancialEndDate
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 0 AS ResponseCode,'Country already exists' ResponseMessage FROM [product_owner].[Country] WHERE CountryName =@CountryName
	
	END;

	IF(@Id = 0 or @Id is null)
	BEGIN
		INSERT INTO [product_owner].[Country](CountryName,CountryCode,CountryCodeNumber,FinancialStartDate,FinancialEndDate,CreatedOn,CreatedBy,Status)
		VALUES(@CountryName,@CountryCode,@CountryCodeNumber,@FinancialStartDate,@FinancialEndDate,GETDATE(),@CreatedBy,0);

		 SELECT @Id = SCOPE_IDENTITY() 

	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[Country] SET
		CountryName=@CountryName,		
		CountryCode=@CountryCode,		
		CountryCodeNumber=@CountryCodeNumber,	
		FinancialStartDate=@FinancialStartDate,	
		FinancialEndDate=@FinancialEndDate,	
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID

	END;

	SELECT Id
          ,CountryName
          ,CountryCode
          ,CountryCodeNumber
          ,FinancialStartDate
          ,FinancialEndDate
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 1 AS ResponseCode,'Country Added Successfully' ResponseMessage FROM [product_owner].[Country] WHERE Id =@Id
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTCOUNTRYENTITYTYPEMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_POSTCOUNTRYENTITYTYPEMAPPING] 
	(
		@Id						INT = 0,
		@CountryId			    INT = 0,
		@EntityTypeId			INT = 0,
		@CreatedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN


	IF(@Id = 0 or @Id is null)
	BEGIN
		IF NOT EXISTS(SELECT * FROM [product_owner].[CountryEntityTypeMapping] WHERE CountryID=@CountryId AND EntityTypeID =@EntityTypeId)
		BEGIN
			INSERT INTO [product_owner].[CountryEntityTypeMapping](CountryID,EntityTypeID,CreatedOn,CreatedBy,Status)
			VALUES(@CountryId,@EntityTypeId,GETDATE(),@CreatedBy,0);

			SELECT @Id = SCOPE_IDENTITY() 
		END;
		ELSE
		BEGIN
			SELECT E.EntityType, E.Id ,  0 AS ResponseCode,'Entity already exists' ResponseMessage FROM [product_owner].[CountryEntityTypeMapping] CP
			INNER JOIN product_owner.EntityType E ON E.Id = CP.EntityTypeID
			WHERE CountryID=@CountryId AND EntityTypeID =@EntityTypeId;
			return;
		END;
	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[CountryEntityTypeMapping] SET
		CountryID = @CountryId,
		EntityTypeID =@EntityTypeId,
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID
	END;

	SELECT 
          SM.Id,
		  SM.CountryId,
		  SM.EntityTypeId,
		  SM.Status,
		  SM.UID,
		  C.CountryName,
		  S.EntityType,
		  C.CountryCode,
		  S.EntityTypeCode
	, 1 AS ResponseCode,'Successfully saved and sent for review' AS ResponseMessage FROM
	[product_owner].[CountryEntityTypeMapping] SM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[EntityType] S ON S.ID = SM.EntityTypeID
	WHERE SM.Id =@Id
	
END;


GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTCOUNTRYREGULATIONGROUPMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_POSTCOUNTRYREGULATIONGROUPMAPPING] 
	(
		@Id						INT = 0,
		@CountryId			    INT = 0,
		@RegulationGroupId		INT = 0,
		@CreatedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN


	IF(@Id = 0 or @Id is null)
	BEGIN
		IF NOT EXISTS(SELECT * FROM [product_owner].[CountryRegulationGroupMapping] WHERE CountryID=@CountryId AND RegulationGroupID =@RegulationGroupId)
		BEGIN
			INSERT INTO [product_owner].[CountryRegulationGroupMapping](CountryID,RegulationGroupID,CreatedOn,CreatedBy,Status)
			VALUES(@CountryId,@RegulationGroupId,GETDATE(),@CreatedBy,0);

			SELECT @Id = SCOPE_IDENTITY() 
		END;
		ELSE
		BEGIN
			SELECT R.RegulationGroupName,R.Id, 0 AS ResponseCode,'Entity already exists' ResponseMessage FROM [product_owner].[CountryRegulationGroupMapping] RM
			INNER JOIN product_owner.RegulationGroup R ON R.Id= RM.RegulationGroupId
			WHERE CountryID=@CountryId AND RegulationGroupID =@RegulationGroupId
			return;
		END;
	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[CountryRegulationGroupMapping] SET
		CountryID = @CountryId,
		RegulationGroupID =@RegulationGroupId,
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID
	END;

	SELECT 
          SM.Id,
		  SM.CountryId,
		  SM.RegulationGroupId,
		  SM.Status,
		  SM.UID,
		  C.CountryName,
		  S.RegulationGroupName,
		  C.CountryCode,
		  S.RegulationGroupCode
	, 1 AS ResponseCode,'Successfully saved and sent for review' AS ResponseMessage FROM
	[product_owner].[CountryRegulationGroupMapping] SM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[RegulationGroup] S ON S.ID = SM.RegulationGroupID
	WHERE SM.Id =@Id
	
END;


GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTCOUNTRYSTATEMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_POSTCOUNTRYSTATEMAPPING] 
	(
		@Id						INT = 0,
		@CountryId			    INT = 0,
		@StateId			    INT = 0,
		@CreatedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN


	IF(@Id = 0 or @Id is null)
	BEGIN
		IF NOT EXISTS(SELECT * FROM [product_owner].[CountryStateMapping] WHERE CountryID=@CountryId AND StateID =@StateId)
		BEGIN
			INSERT INTO [product_owner].[CountryStateMapping](CountryID,StateID,CreatedOn,CreatedBy,Status)
			VALUES(@CountryId,@StateId,GETDATE(),@CreatedBy,0);

			SELECT @Id = SCOPE_IDENTITY() 
		END;
		ELSE
		BEGIN
			SELECT @Id=Id FROM [product_owner].[CountryStateMapping] WHERE CountryID=@CountryId AND StateID =@StateId
		  	SELECT 
				  SM.Id,
				  SM.CountryId,
				  SM.StateId,
				  SM.Status,
				  SM.UID,
				  C.CountryName,
				  S.StateName,
				  C.CountryCode,
				  S.StateCode
			, 0 AS ResponseCode,'Country '+c.CountryName + ' State '+ S.StateName + ' already exists' AS ResponseMessage FROM
			[product_owner].[CountryStateMapping] SM
			INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
			INNER JOIN [PRODUCT_OWNER].[STATE] S ON S.ID = SM.STATEID
			WHERE SM.Id =@Id
			return
		END;
	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[CountryStateMapping] SET
		CountryID = @CountryId,
		StateID =@StateId,
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID
	END;

	SELECT 
          SM.Id,
		  SM.CountryId,
		  SM.StateId,
		  SM.Status,
		  SM.UID,
		  C.CountryName,
		  S.StateName,
		  C.CountryCode,
		  S.StateCode
	, 1 AS ResponseCode,'Successfully saved and sent for review' AS ResponseMessage FROM
	[product_owner].[CountryStateMapping] SM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = SM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[STATE] S ON S.ID = SM.STATEID
	WHERE SM.Id =@Id
	
END;


GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTENTITYTYPE]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_POSTENTITYTYPE] 
	(
		@Id						INT = 0,
		@EntityType	VARCHAR(50)= NULL,
		@EntityTypeCode	VARCHAR(50)= NULL,
		@CreatedBy				INT = 0,
		@ModifiedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN


	IF(@Id = 0 or @Id is null)
	BEGIN
		INSERT INTO [product_owner].[EntityType](EntityType,EntityTypeCode,CreatedOn,CreatedBy,Status)
		VALUES(@EntityType,@EntityTypeCode,GETDATE(),@CreatedBy,0);

		 SELECT @Id = SCOPE_IDENTITY() 

	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[EntityType] SET
		EntityType=@EntityType,		
		EntityTypeCode=@EntityTypeCode,		
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID

	END;

	SELECT Id
          ,EntityType
          ,EntityTypeCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 1 AS ResponseCode,'EntityType Added and sent for Approval' ResponseMessage FROM [product_owner].[EntityType] WHERE Id =@Id
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTINDUSTRYMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_POSTINDUSTRYMAPPING] 
	(
		@Id						INT = 0,
		@CountryId			    INT = 0,
		@MajorIndustryId		INT = 0,
		@MinorIndustryId		INT = 0,
		@CreatedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN


	IF(@Id = 0 or @Id is null)
	BEGIN
		IF NOT EXISTS(SELECT * FROM [product_owner].[IndustryMapping] WHERE CountryId=@CountryId AND MajorIndustryId =@MajorIndustryId AND MinorIndustryId =@MinorIndustryId)
		BEGIN
			INSERT INTO [product_owner].[IndustryMapping](CountryID,MajorIndustryId,MinorIndustryId,CreatedOn,CreatedBy,Status)
			VALUES(@CountryId,@MajorIndustryId,@MinorIndustryId,GETDATE(),@CreatedBy,0);

			SELECT @Id = SCOPE_IDENTITY() 
		END;
		ELSE
		BEGIN
			SELECT @Id=Id FROM [product_owner].[IndustryMapping] WHERE CountryID=@CountryId AND MajorIndustryId =@MajorIndustryId AND MinorIndustryId =@MinorIndustryId
		END;
	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[IndustryMapping] SET
		CountryID = @CountryId,
		MajorIndustryId =@MajorIndustryId,
		MinorIndustryId =@MinorIndustryId,
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID
	END;

	SELECT 
          IM.Id,
		  IM.CountryId,
		  IM.MajorIndustryId,
		  IM.MinorIndustryId,
		  IM.Status,
		  IM.UID,
		  C.CountryName,
		  MA.MajorIndustryName,
		  MI.MinorIndustryName,
		  C.CountryCode,
		  MA.MajorIndustryCode,
		  MI.MinorIndustryCode
	, 1 AS ResponseCode,'Successfully saved and sent for review' AS ResponseMessage FROM
	[product_owner].[IndustryMapping] IM
	INNER JOIN [PRODUCT_OWNER].[COUNTRY] C ON C.ID = IM.COUNTRYID
	INNER JOIN [PRODUCT_OWNER].[MajorIndustry] MA ON MA.ID = IM.MAJORINDUSTRYID
	INNER JOIN [PRODUCT_OWNER].[MinorIndustry] MI ON MI.ID = IM.MINORINDUSTRYID
	WHERE IM.Id =@Id
	
END;


GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTMAJORINDUSTRY]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_POSTMAJORINDUSTRY] 
	(
		@Id						INT = 0,
		@MajorIndustryName			VARCHAR(50)= NULL,
		@MajorIndustryCode			VARCHAR(50)= NULL,
		@CreatedBy				INT = 0,
		@ModifiedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN
	IF EXISTS( SELECT * FROM product_owner.MajorIndustry WHERE MajorIndustryName = @MajorIndustryName)
	BEGIN
		SELECT Id
          ,MajorIndustryName
          ,MajorIndustryCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 0 AS ResponseCode,'Major Industry already exists' ResponseMessage FROM [product_owner].[MajorIndustry] WHERE MajorIndustryName =@MajorIndustryName
	
	END;

	IF(@Id = 0 or @Id is null)
	BEGIN
		INSERT INTO [product_owner].[MajorIndustry](MajorIndustryName,MajorIndustryCode,CreatedOn,CreatedBy,Status)
		VALUES(@MajorIndustryName,@MajorIndustryCode,GETDATE(),@CreatedBy,0);

		 SELECT @Id = SCOPE_IDENTITY() 

	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[MajorIndustry] SET
		MajorIndustryName=@MajorIndustryName,		
		MajorIndustryCode=@MajorIndustryCode,		
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID

	END;

	SELECT Id
          ,MajorIndustryName
          ,MajorIndustryCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 1 AS ResponseCode,'Major Industry Added and sent for Approval' ResponseMessage FROM [product_owner].[MajorIndustry] WHERE Id =@Id
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTMAJORMINORINDUSTRYMAPPING]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_POSTMAJORMINORINDUSTRYMAPPING] 
	(
		@Id						INT = 0,
		@MajorIndustryId		INT = 0,
		@MinorIndustryId		INT = 0,
		@CreatedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN


	IF(@Id = 0 or @Id is null)
	BEGIN
		IF NOT EXISTS(SELECT * FROM [product_owner].[MajorMinorIndustryMapping] WHERE MajorIndustryId=@MajorIndustryId AND MinorIndustryId =@MinorIndustryId)
		BEGIN
			INSERT INTO [product_owner].[MajorMinorIndustryMapping](MajorIndustryId,MinorIndustryId,CreatedOn,CreatedBy,Status)
			VALUES(@MajorIndustryId,@MinorIndustryId,GETDATE(),@CreatedBy,0);

			SELECT @Id = SCOPE_IDENTITY() 
		END;
		ELSE
		BEGIN
			SELECT @Id=Id FROM [product_owner].[MajorMinorIndustryMapping] WHERE MajorIndustryId=@MajorIndustryId AND MinorIndustryId =@MinorIndustryId
		END;
	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[MajorMinorIndustryMapping] SET
		MajorIndustryId=@MajorIndustryId,
		MinorIndustryId =@MinorIndustryId,
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID
	END;

	SELECT 
          SM.Id,
		  SM.MajorIndustryId,
		  SM.MinorIndustryId,
		  SM.Status,
		  SM.UID,
		  C.MajorIndustryName,
		  S.MinorIndustryName,
		  C.MajorIndustryCode,
		  S.MinorIndustryCode
	, 1 AS ResponseCode,'Successfully saved and sent for review' AS ResponseMessage FROM
	[product_owner].[MajorMinorIndustryMapping] SM
	INNER JOIN [PRODUCT_OWNER].[MajorIndustry] C ON C.ID = SM.MAJORINDUSTRYID
	INNER JOIN [PRODUCT_OWNER].[MinorIndustry] S ON S.ID = SM.MINORINDUSTRYID
	WHERE SM.Id =@Id
	
END;


GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTMINORINDUSTRY]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_POSTMINORINDUSTRY] 
	(
		@Id						INT = 0,
		@MajorIndustryId				INT = 0,
		@MinorIndustryName			VARCHAR(50)= NULL,
		@MinorIndustryCode			VARCHAR(50)= NULL,
		@CreatedBy				INT = 0,
		@ModifiedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN
	IF EXISTS( SELECT * FROM product_owner.MinorIndustry WHERE MajorIndustryId = @MajorIndustryId AND MinorIndustryName=@MinorIndustryName)
	BEGIN
		SELECT Id
		  ,MajorIndustryId
          ,MinorIndustryName
          ,MinorIndustryCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 0 AS ResponseCode,'Minor Industry already exists' ResponseMessage FROM [product_owner].[MinorIndustry] WHERE MinorIndustryName=@MinorIndustryName
	
	END;

	IF(@Id = 0 or @Id is null)
	BEGIN
		INSERT INTO [product_owner].[MinorIndustry](MajorIndustryId,MinorIndustryName,MinorIndustryCode,CreatedOn,CreatedBy,Status)
		VALUES(@MajorIndustryId,@MinorIndustryName,@MinorIndustryCode,GETDATE(),@CreatedBy,0);

		 SELECT @Id = SCOPE_IDENTITY() 

	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[MinorIndustry] SET
		MajorIndustryId=@MajorIndustryId,
		MinorIndustryName=@MinorIndustryName,		
		MinorIndustryCode=@MinorIndustryCode,		
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID

	END;

	SELECT Id
		  ,MajorIndustryId
          ,MinorIndustryName
          ,MinorIndustryCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 1 AS ResponseCode,'Successfully Saved' ResponseMessage FROM [product_owner].[MinorIndustry] WHERE Id =@Id
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTREGULATIONGROUP]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_POSTREGULATIONGROUP] 
	(
		@Id						INT = 0,
		@RegulationGroupName	VARCHAR(50)= NULL,
		@RegulationGroupCode	VARCHAR(50)= NULL,
		@CreatedBy				INT = 0,
		@ModifiedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN


	IF(@Id = 0 or @Id is null)
	BEGIN
		INSERT INTO [product_owner].[RegulationGroup](RegulationGroupName,RegulationGroupCode,CreatedOn,CreatedBy,Status)
		VALUES(@RegulationGroupName,@RegulationGroupCode,GETDATE(),@CreatedBy,0);

		 SELECT @Id = SCOPE_IDENTITY() 

	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[RegulationGroup] SET
		RegulationGroupName=@RegulationGroupName,		
		RegulationGroupCode=@RegulationGroupCode,		
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID

	END;

	SELECT Id
          ,RegulationGroupName
          ,RegulationGroupCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 1 AS ResponseCode,'RegulationGroup Added and sent for Approval' ResponseMessage FROM [product_owner].[RegulationGroup] WHERE Id =@Id
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_POSTSTATE]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_POSTSTATE] 
	(
		@Id						INT = 0,
		@CountryId				INT = 0,
		@StateName			VARCHAR(50)= NULL,
		@StateCode			VARCHAR(50)= NULL,
		@CreatedBy				INT = 0,
		@ModifiedBy				INT = 0,
		@UID					NVARCHAR(100) = NULL
	)
AS
BEGIN
	IF EXISTS( SELECT * FROM product_owner.State WHERE CountryId = @CountryId AND StateName=@StateName)
	BEGIN
		SELECT Id
		  ,CountryId
          ,StateName
          ,StateCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 0 AS ResponseCode,'State already exists' ResponseMessage FROM [product_owner].[State] WHERE StateName=@StateName
	
	END;

	IF(@Id = 0 or @Id is null)
	BEGIN
		INSERT INTO [product_owner].[State](CountryId,StateName,StateCode,CreatedOn,CreatedBy,Status)
		VALUES(@CountryId,@StateName,@StateCode,GETDATE(),@CreatedBy,0);

		 SELECT @Id = SCOPE_IDENTITY() 

	END;
	ELSE
	BEGIN
		UPDATE [product_owner].[State] SET
		CountryId=@CountryId,
		StateName=@StateName,		
		StateCode=@StateCode,		
		ModifiedBy = @CreatedBy,
		ModifiedOn = GETDATE(),
		Status = 0
		WHERE UID = @UID

	END;

	SELECT Id
		  ,CountryId
          ,StateName
          ,StateCode
          ,CreatedOn
          ,CreatedBy
          ,ModifiedBy
          ,ModifiedOn
          ,UID
	, 1 AS ResponseCode,'Successfully Saved' ResponseMessage FROM [product_owner].[State] WHERE Id =@Id
	
END;








GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATECOUNTRYAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_UPDATECOUNTRYAPPROVAL]
	(
		@Id INT = 0,
		@CountryId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
		IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;

	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);


	IF(NOT EXISTS(SELECT * FROM [product_owner].CountryApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].CountryApproval(CountryId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@CountryId,@ManagerId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].CountryApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		--IF((@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId =  @REVIEWEERSTATUS OR @ApprovalStatusId =  @FORWARD) AND @NEWMANAGERID != 0)
		--BEGIN
		--	INSERT INTO [product_owner].CountryApproval(CountryId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		--	VALUES(@CountryId,@NEWMANAGERID,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
		--	print('');
		--END;
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].Country SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@CountryId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATECOUNTRYENTITYTYPEMAPPINGAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_UPDATECOUNTRYENTITYTYPEMAPPINGAPPROVAL]
	(
		@Id INT = 0,
		@CountryEntityTypeMappingId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
		SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	  IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
	  BEGIN
		SET @ApprovalStatusId = @APPROVEDSTATUS;
	  END;
	END;

	IF(NOT EXISTS(SELECT * FROM [product_owner].CountryEntityTypeMappingApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].CountryEntityTypeMappingApproval(ManagerId,CountryEntityTypeMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@ManagerId,@CountryEntityTypeMappingId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].CountryEntityTypeMappingApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		--IF((@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId =  @REVIEWEERSTATUS OR @ApprovalStatusId =  @FORWARD) AND @NEWMANAGERID != 0)
		--BEGIN
		--	INSERT INTO [product_owner].CountryEntityTypeMappingApproval(ManagerId,CountryEntityTypeMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		--	VALUES(@NEWMANAGERID,@CountryEntityTypeMappingId,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
		--	print('');
		--END;
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].CountryEntityTypeMapping SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@CountryEntityTypeMappingId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATECOUNTRYREGULATIONGROUPMAPPINGAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_UPDATECOUNTRYREGULATIONGROUPMAPPINGAPPROVAL]
	(
		@Id INT = 0,
		@CountryRegulationGroupMappingId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
		SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	  IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
	  BEGIN
		SET @ApprovalStatusId = @APPROVEDSTATUS;
	  END;
	END;

	IF(NOT EXISTS(SELECT * FROM [product_owner].CountryRegulationGroupMappingApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].CountryRegulationGroupMappingApproval(ManagerId,CountryRegulationGroupMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@ManagerId,@CountryRegulationGroupMappingId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].CountryRegulationGroupMappingApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		--IF((@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId =  @REVIEWEERSTATUS OR @ApprovalStatusId =  @FORWARD) AND @NEWMANAGERID != 0)
		--BEGIN
		--	INSERT INTO [product_owner].CountryRegulationGroupMappingApproval(ManagerId,CountryRegulationGroupMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		--	VALUES(@NEWMANAGERID,@CountryRegulationGroupMappingId,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
		--	print('');
		--END;
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].CountryRegulationGroupMapping SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@CountryRegulationGroupMappingId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATECOUNTRYSTATEMAPPINGAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_UPDATECOUNTRYSTATEMAPPINGAPPROVAL]
	(
		@Id INT = 0,
		@CountryStateMappingId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,
	@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	 IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;

	IF(NOT EXISTS(SELECT * FROM [product_owner].CountryStateMappingApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].CountryStateMappingApproval(ManagerId,CountryStateMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@ManagerId,@CountryStateMappingId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].CountryStateMappingApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		--IF((@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId =  @REVIEWEERSTATUS OR @ApprovalStatusId =  @FORWARD) AND @NEWMANAGERID != 0)
		--BEGIN
		--	INSERT INTO [product_owner].CountryStateMappingApproval(ManagerId,CountryStateMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		--	VALUES(@NEWMANAGERID,@CountryStateMappingId,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
		--	print('');
		--END;
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].CountryStateMapping SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@CountryStateMappingId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATEENTITYTYPEAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [product_owner].[USP_UPDATEENTITYTYPEAPPROVAL]
	(
		@Id INT = 0,
		@EntityTypeId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,
	@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
		 IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END
	END;

	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);


	IF(NOT EXISTS(SELECT * FROM [product_owner].EntityTypeApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].EntityTypeApproval(EntityTypeId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@EntityTypeId,@ManagerId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].EntityTypeApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		--IF((@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId =  @REVIEWEERSTATUS OR @ApprovalStatusId =  @FORWARD) AND @NEWMANAGERID != 0 AND  @ISAdmin = 0)
		--BEGIN
		--	INSERT INTO [product_owner].EntityTypeApproval(EntityTypeId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		--	VALUES(@EntityTypeId,@NEWMANAGERID,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
		--	print('');
		--END;
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].EntityType SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@EntityTypeId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATEINDUSTRYAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_UPDATEINDUSTRYAPPROVAL]
	(
		@Id INT = 0,
		@MajorIndustryId INT = 0,
		@MinorIndustryId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
		IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;


	SET @NEWMANAGERID = ISNULL(@ManagerId,0);


	IF(NOT EXISTS(SELECT * FROM [product_owner].IndustryApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].IndustryApproval(MajorIndustryId,MinorIndustryId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@MajorIndustryId,@MinorIndustryId,@ManagerId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].IndustryApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].MajorIndustry SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@MajorIndustryId
	END;
		IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].MinorIndustry SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@MinorIndustryId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATEINDUSTRYMAPPINGAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_UPDATEINDUSTRYMAPPINGAPPROVAL]
	(
		@Id INT = 0,
		@IndustryMappingId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,
	@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	 IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;

	IF(NOT EXISTS(SELECT * FROM [product_owner].IndustryMappingApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].IndustryMappingApproval(ManagerId,IndustryMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@ManagerId,@IndustryMappingId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].IndustryMappingApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].IndustryMapping SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@IndustryMappingId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATEMAJORINDUSTRYAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_UPDATEMAJORINDUSTRYAPPROVAL]
	(
		@Id INT = 0,
		@MajorIndustryId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
		IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;


	SET @NEWMANAGERID = ISNULL(@ManagerId,0);


	IF(NOT EXISTS(SELECT * FROM [product_owner].MajorIndustryApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].MajorIndustryApproval(MajorIndustryId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@MajorIndustryId,@ManagerId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].MajorIndustryApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].MajorIndustry SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@MajorIndustryId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATEMAJORMINORMAPPINGAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_UPDATEMAJORMINORMAPPINGAPPROVAL]
	(
		@Id INT = 0,
		@MajorMinorIndustryMappingId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,
	@PENDINGAPPROVAL INT = 0, @FORWARD INT = 0, @NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	 IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;

	IF(NOT EXISTS(SELECT * FROM [product_owner].MajorMinorIndustryMappingApproval WHERE UID = @UID) OR @ISAdmin = 1)
	BEGIN
		INSERT INTO [product_owner].MajorMinorIndustryMappingApproval(ManagerId,MajorMinorIndustryMappingId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@ManagerId,@MajorMinorIndustryMappingId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].MajorMinorIndustryMappingApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].MajorMinorIndustryMapping SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@MajorMinorIndustryMappingId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATEMINORINDUSTRYAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_UPDATEMINORINDUSTRYAPPROVAL]
	(
		@Id INT = 0,
		@MinorIndustryId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus

	SET @NEWMANAGERID = ISNULL(@ManagerId,0);

	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
		IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;


	IF(NOT EXISTS(SELECT * FROM [product_owner].MinorIndustryApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].MinorIndustryApproval(MinorIndustryId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@MinorIndustryId,@ManagerId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].MinorIndustryApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1 )
	BEGIN
		UPDATE [product_owner].MinorIndustry SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@MinorIndustryId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATEREGULATIONGROUPAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [product_owner].[USP_UPDATEREGULATIONGROUPAPPROVAL]
	(
		@Id INT = 0,
		@RegulationGroupId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,
	@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
		 IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END
	END;

	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);


	IF(NOT EXISTS(SELECT * FROM [product_owner].RegulationGroupApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].RegulationGroupApproval(RegulationGroupId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@RegulationGroupId,@ManagerId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].RegulationGroupApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		--IF((@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId =  @REVIEWEERSTATUS OR @ApprovalStatusId =  @FORWARD) AND @NEWMANAGERID != 0 AND  @ISAdmin = 0)
		--BEGIN
		--	INSERT INTO [product_owner].RegulationGroupApproval(RegulationGroupId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		--	VALUES(@RegulationGroupId,@NEWMANAGERID,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
		--	print('');
		--END;
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].RegulationGroup SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@RegulationGroupId
	END;
END
GO
/****** Object:  StoredProcedure [product_owner].[USP_UPDATESTATEAPPROVAL]    Script Date: 06-06-2024 00:49:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [product_owner].[USP_UPDATESTATEAPPROVAL]
	(
		@Id INT = 0,
		@StateId INT = 0,
		@ManagerId INT = 0,
		@ApprovalStatus VARCHAR(20) = '',
		@CreatedBy INT = 0,
		@UID NVARCHAR(100) = NULL
	)
AS
BEGIN

	DECLARE @ISAdmin bit = 0, @ApprovalStatusId INT =0, @APPROVEDSTATUS INT =0,@REVIEWEERSTATUS INT = 0,@PENDINGAPPROVAL INT = 0,@FORWARD INT = 0,@NEWMANAGERID INT = 0;
	SELECT @APPROVEDSTATUS = Id FROM RefApprovalStatus WHERE Status='Approved'
	SELECT @REVIEWEERSTATUS = Id FROM RefApprovalStatus WHERE Status='Reviewed'
	SELECT @PENDINGAPPROVAL = Id FROM RefApprovalStatus WHERE Status='Pending'
	SELECT @FORWARD = Id FROM RefApprovalStatus WHERE Status='Forward'

	SELECT @ApprovalStatusId = Id FROM RefApprovalStatus WHERE Status = @ApprovalStatus
	
	SET @NEWMANAGERID = ISNULL(@ManagerId,0);

	IF ([dbo].IsSuperAdmin(@CreatedBy) = 1)
	BEGIN
	 SET @ISAdmin = 1;
	 IF (@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId = @PENDINGAPPROVAL)
		 BEGIN
			SET @ApprovalStatusId = @APPROVEDSTATUS;
		 END

	END;


	IF(NOT EXISTS(SELECT * FROM [product_owner].StateApproval WHERE UID = @UID))
	BEGIN
		INSERT INTO [product_owner].StateApproval(StateId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		VALUES(@StateId,@ManagerId,@ApprovalStatusId,@CreatedBy,GETDATE())
	END
	ELSE
	BEGIN
		UPDATE [product_owner].StateApproval SET
		ApprovalStatus = @ApprovalStatusId,
		ModifiedBy=@CreatedBy,
		ModifiedOn= GETDATE()
		WHERE UID =@UID

		
		--IF((@ApprovalStatusId = @APPROVEDSTATUS OR @ApprovalStatusId =  @REVIEWEERSTATUS OR @ApprovalStatusId = @FORWARD) AND @NEWMANAGERID != 0)
		--BEGIN
		--	INSERT INTO [product_owner].StateApproval(StateId,ManagerId,ApprovalStatus,CreatedBy,CreatedOn)
		--	VALUES(@StateId,@NEWMANAGERID,@PENDINGAPPROVAL,@CreatedBy,GETDATE())
		--	print('');
		--END;
	END 
	
	IF(@ApprovalStatusId = @APPROVEDSTATUS OR @ISAdmin = 1)
	BEGIN
		UPDATE [product_owner].State SET 
		Status = 1,
		ModifiedBy=@CreatedBy,
		ModifiedOn = GETDATE()
		WHERE ID=@StateId
	END;
END
GO
USE [master]
GO
ALTER DATABASE [ComplianceNew] SET  READ_WRITE 
GO
