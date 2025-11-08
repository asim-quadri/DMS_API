--Script to create CountryFileNamesApproval table
  CREATE TABLE [product_owner].CountryFileNamesApproval(
	[Id]			[bigint] IDENTITY(1,1) NOT NULL,
	[CountryFileId] [int] NOT NULL,
	[ManagerId]		[bigint] NOT NULL,
	[ApprovalStatus] [int] NOT NULL,
	[CreatedOn]		[datetime] NULL,
	[CreatedBy]		[bigint] NULL,
	[ModifiedBy]	[bigint] NULL,
	[ModifiedOn]	[datetime] NULL,
	[UID]			[uniqueidentifier] NULL,
 CONSTRAINT [PK_CountryFileApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [product_owner].CountryFileNamesApproval ADD  CONSTRAINT [DF_CountryFileApproval_UID]  DEFAULT (newid()) FOR [UID]
GO


