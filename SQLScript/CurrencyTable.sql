CREATE TABLE [product_owner].[CurrencyCodes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](100) NULL,
	[CurrencyCode] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



-- 1. Temporary staging table
CREATE TABLE #TempCurrency (
  CountryName NVARCHAR(100),
  CurrencyCode NVARCHAR(10)
);

-- 2. Insert your extracted data
INSERT INTO #TempCurrency (CountryName, CurrencyCode)
VALUES
  ('India', 'INR'),
  ('United States', 'USD'),
  ('United Kingdom', 'GBP'),
  ('Japan', 'JPY'),
  ('Canada', 'CAD');
-- Add more countries as needed

-- 3. Insert into main table only if not already present
INSERT INTO [product_owner].[CurrencyCode] (CountryName, CurrencyCode)
SELECT t.CountryName, t.CurrencyCode
FROM #TempCurrency AS t
LEFT JOIN [product_owner].[CurrencyCode] AS c
  ON t.CountryName = c.CountryName AND t.CurrencyCode = c.CurrencyCode
WHERE c.Id IS NULL;

-- 4. Drop the temp table
DROP TABLE #TempCurrency;
