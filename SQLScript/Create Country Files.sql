CREATE TABLE product_owner.CountryFileNames 
(
    Id INT PRIMARY KEY IDENTITY,
    CountryId INT NOT NULL,
    FileName NVARCHAR(MAX) NOT NULL,
	Status INT NULL,
	CreatedOn DATETIME NULL,
	CreatedBy BIGINT NULL,
	ModifiedBy BIGINT NULL,
	ModifiedOn DATETIME NULL,
	UID UNIQUEIDENTIFIER NULL,
    FOREIGN KEY (CountryId) REFERENCES product_owner.Country(Id)
);

ALTER TABLE [product_owner].CountryFileNames ADD  CONSTRAINT [DF_CountryFiles_UID]  DEFAULT (newid()) FOR [UID]
GO

