--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Data Schema For Version 0.0.0.0

--CREATE TABLE [Categories]
if not exists(select * from sysobjects where name = 'Categories' and xtype = 'U')
CREATE TABLE [dbo].[Categories] (
[CategoryID] [Int] IDENTITY (1, 1) NOT NULL ,
[CategoryName] [NVarChar] (15) NOT NULL ,
[Description] [NText] NULL ,
[Picture] [Image] NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__CATEGORIES_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__CATEGORIES_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [CustomerCustomerDemo]
if not exists(select * from sysobjects where name = 'CustomerCustomerDemo' and xtype = 'U')
CREATE TABLE [dbo].[CustomerCustomerDemo] (
[CustomerID] [NChar] (5) NOT NULL ,
[CustomerTypeID] [NChar] (10) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [CustomerDemographics]
if not exists(select * from sysobjects where name = 'CustomerDemographics' and xtype = 'U')
CREATE TABLE [dbo].[CustomerDemographics] (
[CustomerDesc] [NText] NULL ,
[CustomerTypeID] [NChar] (10) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Customers]
if not exists(select * from sysobjects where name = 'Customers' and xtype = 'U')
CREATE TABLE [dbo].[Customers] (
[Address] [NVarChar] (60) NULL ,
[City] [NVarChar] (15) NULL ,
[CompanyName] [NVarChar] (40) NOT NULL ,
[ContactName] [NVarChar] (30) NULL ,
[ContactTitle] [NVarChar] (30) NULL ,
[Country] [NVarChar] (15) NULL ,
[CustomerID] [NChar] (5) NOT NULL ,
[Fax] [NVarChar] (24) NULL ,
[Phone] [NVarChar] (24) NULL ,
[PostalCode] [NVarChar] (10) NULL ,
[Region] [NVarChar] (15) NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERS_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERS_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Employees]
if not exists(select * from sysobjects where name = 'Employees' and xtype = 'U')
CREATE TABLE [dbo].[Employees] (
[Address] [NVarChar] (60) NULL ,
[BirthDate] [DateTime] NULL ,
[City] [NVarChar] (15) NULL ,
[Country] [NVarChar] (15) NULL ,
[EmployeeID] [Int] IDENTITY (1, 1) NOT NULL ,
[Extension] [NVarChar] (4) NULL ,
[FirstName] [NVarChar] (10) NOT NULL ,
[HireDate] [DateTime] NULL ,
[HomePhone] [NVarChar] (24) NULL ,
[LastName] [NVarChar] (20) NOT NULL ,
[Notes] [NText] NULL ,
[Photo] [Image] NULL ,
[PhotoPath] [NVarChar] (255) NULL ,
[PostalCode] [NVarChar] (10) NULL ,
[Region] [NVarChar] (15) NULL ,
[ReportsTo] [Int] NULL ,
[Title] [NVarChar] (30) NULL ,
[TitleOfCourtesy] [NVarChar] (25) NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__EMPLOYEES_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__EMPLOYEES_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [EmployeeTerritories]
if not exists(select * from sysobjects where name = 'EmployeeTerritories' and xtype = 'U')
CREATE TABLE [dbo].[EmployeeTerritories] (
[EmployeeID] [Int] NOT NULL ,
[TerritoryID] [NVarChar] (20) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__EMPLOYEETERRITORIES_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__EMPLOYEETERRITORIES_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Order Details]
if not exists(select * from sysobjects where name = 'Order Details' and xtype = 'U')
CREATE TABLE [dbo].[Order Details] (
[Discount] [Real] NOT NULL CONSTRAINT [DF__ORDER DETAILS_DISCOUNT] DEFAULT (0),
[OrderID] [Int] NOT NULL ,
[ProductID] [Int] NOT NULL ,
[Quantity] [SmallInt] NOT NULL CONSTRAINT [DF__ORDER DETAILS_QUANTITY] DEFAULT (1),
[UnitPrice] [Money] NOT NULL CONSTRAINT [DF__ORDER DETAILS_UNITPRICE] DEFAULT (0),
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__ORDER DETAILS_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__ORDER DETAILS_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Orders]
if not exists(select * from sysobjects where name = 'Orders' and xtype = 'U')
CREATE TABLE [dbo].[Orders] (
[CustomerID] [NChar] (5) NULL ,
[EmployeeID] [Int] NULL ,
[Freight] [Money] NULL CONSTRAINT [DF__ORDERS_FREIGHT] DEFAULT (0),
[OrderDate] [DateTime] NULL ,
[OrderID] [Int] IDENTITY (1, 1) NOT NULL ,
[RequiredDate] [DateTime] NULL ,
[ShipAddress] [NVarChar] (60) NULL ,
[ShipCity] [NVarChar] (15) NULL ,
[ShipCountry] [NVarChar] (15) NULL ,
[ShipName] [NVarChar] (40) NULL ,
[ShippedDate] [DateTime] NULL ,
[ShipPostalCode] [NVarChar] (10) NULL ,
[ShipRegion] [NVarChar] (15) NULL ,
[ShipVia] [Int] NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__ORDERS_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__ORDERS_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Products]
if not exists(select * from sysobjects where name = 'Products' and xtype = 'U')
CREATE TABLE [dbo].[Products] (
[CategoryID] [Int] NULL ,
[Discontinued] [Bit] NOT NULL CONSTRAINT [DF__PRODUCTS_DISCONTINUED] DEFAULT (0),
[ProductID] [Int] IDENTITY (1, 1) NOT NULL ,
[ProductName] [NVarChar] (40) NOT NULL ,
[QuantityPerUnit] [NVarChar] (20) NULL ,
[ReorderLevel] [SmallInt] NULL CONSTRAINT [DF__PRODUCTS_REORDERLEVEL] DEFAULT (0),
[SupplierID] [Int] NULL ,
[UnitPrice] [Money] NULL CONSTRAINT [DF__PRODUCTS_UNITPRICE] DEFAULT (0),
[UnitsInStock] [SmallInt] NULL CONSTRAINT [DF__PRODUCTS_UNITSINSTOCK] DEFAULT (0),
[UnitsOnOrder] [SmallInt] NULL CONSTRAINT [DF__PRODUCTS_UNITSONORDER] DEFAULT (0),
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__PRODUCTS_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__PRODUCTS_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Region]
if not exists(select * from sysobjects where name = 'Region' and xtype = 'U')
CREATE TABLE [dbo].[Region] (
[RegionDescription] [NChar] (50) NOT NULL ,
[RegionID] [Int] NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__REGION_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__REGION_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Shippers]
if not exists(select * from sysobjects where name = 'Shippers' and xtype = 'U')
CREATE TABLE [dbo].[Shippers] (
[CompanyName] [NVarChar] (40) NOT NULL ,
[Phone] [NVarChar] (24) NULL ,
[ShipperID] [Int] IDENTITY (1, 1) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__SHIPPERS_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__SHIPPERS_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Suppliers]
if not exists(select * from sysobjects where name = 'Suppliers' and xtype = 'U')
CREATE TABLE [dbo].[Suppliers] (
[Address] [NVarChar] (60) NULL ,
[City] [NVarChar] (15) NULL ,
[CompanyName] [NVarChar] (40) NOT NULL ,
[ContactName] [NVarChar] (30) NULL ,
[ContactTitle] [NVarChar] (30) NULL ,
[Country] [NVarChar] (15) NULL ,
[Fax] [NVarChar] (24) NULL ,
[HomePage] [NText] NULL ,
[Phone] [NVarChar] (24) NULL ,
[PostalCode] [NVarChar] (10) NULL ,
[Region] [NVarChar] (15) NULL ,
[SupplierID] [Int] IDENTITY (1, 1) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__SUPPLIERS_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__SUPPLIERS_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--CREATE TABLE [Territories]
if not exists(select * from sysobjects where name = 'Territories' and xtype = 'U')
CREATE TABLE [dbo].[Territories] (
[RegionID] [Int] NOT NULL ,
[TerritoryDescription] [NChar] (50) NOT NULL ,
[TerritoryID] [NVarChar] (20) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL,
[ModifiedDate] [DateTime] CONSTRAINT [DF__TERRITORIES_MODIFIEDDATE] DEFAULT getdate() NULL,
[CreatedBy] [Varchar] (50) NULL,
[CreatedDate] [DateTime] CONSTRAINT [DF__TERRITORIES_CREATEDDATE] DEFAULT getdate() NULL,
[Timestamp] [timestamp] NOT NULL
)

GO

--##SECTION BEGIN [FIELD CREATE]
if exists(select * from sys.objects where name = 'Categories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CategoryID' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [CategoryID] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'Categories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CategoryName' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [CategoryName] [NVarChar] (15) NOT NULL 
if exists(select * from sys.objects where name = 'Categories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Description' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [Description] [NText] NULL 
if exists(select * from sys.objects where name = 'Categories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Picture' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [Picture] [Image] NULL 
if exists(select * from sys.objects where name = 'CustomerCustomerDemo' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CustomerID' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [CustomerID] [NChar] (5) NOT NULL 
if exists(select * from sys.objects where name = 'CustomerCustomerDemo' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CustomerTypeID' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [CustomerTypeID] [NChar] (10) NOT NULL 
if exists(select * from sys.objects where name = 'CustomerDemographics' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CustomerDesc' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [CustomerDesc] [NText] NULL 
if exists(select * from sys.objects where name = 'CustomerDemographics' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CustomerTypeID' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [CustomerTypeID] [NChar] (10) NOT NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Address' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [Address] [NVarChar] (60) NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'City' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [City] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CompanyName' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [CompanyName] [NVarChar] (40) NOT NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ContactName' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [ContactName] [NVarChar] (30) NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ContactTitle' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [ContactTitle] [NVarChar] (30) NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Country' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [Country] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CustomerID' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [CustomerID] [NChar] (5) NOT NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Fax' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [Fax] [NVarChar] (24) NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Phone' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [Phone] [NVarChar] (24) NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'PostalCode' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [PostalCode] [NVarChar] (10) NULL 
if exists(select * from sys.objects where name = 'Customers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Region' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [Region] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Address' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Address] [NVarChar] (60) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'BirthDate' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [BirthDate] [DateTime] NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'City' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [City] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Country' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Country] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'EmployeeID' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [EmployeeID] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Extension' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Extension] [NVarChar] (4) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'FirstName' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [FirstName] [NVarChar] (10) NOT NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'HireDate' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [HireDate] [DateTime] NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'HomePhone' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [HomePhone] [NVarChar] (24) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'LastName' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [LastName] [NVarChar] (20) NOT NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Notes' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Notes] [NText] NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Photo' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Photo] [Image] NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'PhotoPath' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [PhotoPath] [NVarChar] (255) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'PostalCode' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [PostalCode] [NVarChar] (10) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Region' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Region] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ReportsTo' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [ReportsTo] [Int] NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Title' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Title] [NVarChar] (30) NULL 
if exists(select * from sys.objects where name = 'Employees' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'TitleOfCourtesy' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [TitleOfCourtesy] [NVarChar] (25) NULL 
if exists(select * from sys.objects where name = 'EmployeeTerritories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'EmployeeID' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [EmployeeID] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'EmployeeTerritories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'TerritoryID' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [TerritoryID] [NVarChar] (20) NOT NULL 
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Discount' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [Discount] [Real] NOT NULL CONSTRAINT [DF__ORDER DETAILS_DISCOUNT] DEFAULT (0)
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'OrderID' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [OrderID] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ProductID' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [ProductID] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Quantity' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [Quantity] [SmallInt] NOT NULL CONSTRAINT [DF__ORDER DETAILS_QUANTITY] DEFAULT (1)
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UnitPrice' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [UnitPrice] [Money] NOT NULL CONSTRAINT [DF__ORDER DETAILS_UNITPRICE] DEFAULT (0)
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CustomerID' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [CustomerID] [NChar] (5) NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'EmployeeID' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [EmployeeID] [Int] NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Freight' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [Freight] [Money] NULL CONSTRAINT [DF__ORDERS_FREIGHT] DEFAULT (0)
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'OrderDate' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [OrderDate] [DateTime] NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'OrderID' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [OrderID] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RequiredDate' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [RequiredDate] [DateTime] NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShipAddress' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ShipAddress] [NVarChar] (60) NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShipCity' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ShipCity] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShipCountry' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ShipCountry] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShipName' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ShipName] [NVarChar] (40) NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShippedDate' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ShippedDate] [DateTime] NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShipPostalCode' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ShipPostalCode] [NVarChar] (10) NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShipRegion' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ShipRegion] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Orders' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShipVia' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ShipVia] [Int] NULL 
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CategoryID' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [CategoryID] [Int] NULL 
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Discontinued' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [Discontinued] [Bit] NOT NULL CONSTRAINT [DF__PRODUCTS_DISCONTINUED] DEFAULT (0)
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ProductID' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [ProductID] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ProductName' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [ProductName] [NVarChar] (40) NOT NULL 
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'QuantityPerUnit' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [QuantityPerUnit] [NVarChar] (20) NULL 
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ReorderLevel' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [ReorderLevel] [SmallInt] NULL CONSTRAINT [DF__PRODUCTS_REORDERLEVEL] DEFAULT (0)
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'SupplierID' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [SupplierID] [Int] NULL 
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UnitPrice' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [UnitPrice] [Money] NULL CONSTRAINT [DF__PRODUCTS_UNITPRICE] DEFAULT (0)
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UnitsInStock' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [UnitsInStock] [SmallInt] NULL CONSTRAINT [DF__PRODUCTS_UNITSINSTOCK] DEFAULT (0)
if exists(select * from sys.objects where name = 'Products' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'UnitsOnOrder' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [UnitsOnOrder] [SmallInt] NULL CONSTRAINT [DF__PRODUCTS_UNITSONORDER] DEFAULT (0)
if exists(select * from sys.objects where name = 'Region' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RegionDescription' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [RegionDescription] [NChar] (50) NOT NULL 
if exists(select * from sys.objects where name = 'Region' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RegionID' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [RegionID] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'Shippers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CompanyName' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [CompanyName] [NVarChar] (40) NOT NULL 
if exists(select * from sys.objects where name = 'Shippers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Phone' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [Phone] [NVarChar] (24) NULL 
if exists(select * from sys.objects where name = 'Shippers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ShipperID' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [ShipperID] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Address' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [Address] [NVarChar] (60) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'City' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [City] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CompanyName' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [CompanyName] [NVarChar] (40) NOT NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ContactName' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [ContactName] [NVarChar] (30) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ContactTitle' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [ContactTitle] [NVarChar] (30) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Country' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [Country] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Fax' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [Fax] [NVarChar] (24) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'HomePage' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [HomePage] [NText] NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Phone' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [Phone] [NVarChar] (24) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'PostalCode' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [PostalCode] [NVarChar] (10) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Region' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [Region] [NVarChar] (15) NULL 
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'SupplierID' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [SupplierID] [Int] IDENTITY (1, 1) NOT NULL 
if exists(select * from sys.objects where name = 'Territories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'RegionID' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [RegionID] [Int] NOT NULL 
if exists(select * from sys.objects where name = 'Territories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'TerritoryDescription' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [TerritoryDescription] [NChar] (50) NOT NULL 
if exists(select * from sys.objects where name = 'Territories' and type = 'U') AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'TerritoryID' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [TerritoryID] [NVarChar] (20) NOT NULL 
GO
--##SECTION END [FIELD CREATE]

--##SECTION BEGIN [AUDIT TRAIL CREATE]

--APPEND AUDIT TRAIL CREATE FOR TABLE [Categories]
if exists(select * from sys.objects where name = 'Categories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Categories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CATEGORIES_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Categories]
if exists(select * from sys.objects where name = 'Categories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Categories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CATEGORIES_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Categories]
if exists(select * from sys.objects where name = 'Categories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [CustomerCustomerDemo]
if exists(select * from sys.objects where name = 'CustomerCustomerDemo' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'CustomerCustomerDemo' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [CustomerCustomerDemo]
if exists(select * from sys.objects where name = 'CustomerCustomerDemo' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'CustomerCustomerDemo' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [CustomerCustomerDemo]
if exists(select * from sys.objects where name = 'CustomerCustomerDemo' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [CustomerDemographics]
if exists(select * from sys.objects where name = 'CustomerDemographics' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'CustomerDemographics' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [CustomerDemographics]
if exists(select * from sys.objects where name = 'CustomerDemographics' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'CustomerDemographics' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [CustomerDemographics]
if exists(select * from sys.objects where name = 'CustomerDemographics' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Customers]
if exists(select * from sys.objects where name = 'Customers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Customers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Customers]
if exists(select * from sys.objects where name = 'Customers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Customers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Customers]
if exists(select * from sys.objects where name = 'Customers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Employees]
if exists(select * from sys.objects where name = 'Employees' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Employees' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__EMPLOYEES_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Employees]
if exists(select * from sys.objects where name = 'Employees' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Employees' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__EMPLOYEES_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Employees]
if exists(select * from sys.objects where name = 'Employees' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [EmployeeTerritories]
if exists(select * from sys.objects where name = 'EmployeeTerritories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'EmployeeTerritories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__EMPLOYEETERRITORIES_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [EmployeeTerritories]
if exists(select * from sys.objects where name = 'EmployeeTerritories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'EmployeeTerritories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__EMPLOYEETERRITORIES_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [EmployeeTerritories]
if exists(select * from sys.objects where name = 'EmployeeTerritories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Order Details]
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__ORDER DETAILS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Order Details]
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__ORDER DETAILS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Order Details]
if exists(select * from sys.objects where name = 'Order Details' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Orders]
if exists(select * from sys.objects where name = 'Orders' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Orders' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__ORDERS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Orders]
if exists(select * from sys.objects where name = 'Orders' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Orders' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__ORDERS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Orders]
if exists(select * from sys.objects where name = 'Orders' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Products]
if exists(select * from sys.objects where name = 'Products' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Products' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__PRODUCTS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Products]
if exists(select * from sys.objects where name = 'Products' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Products' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__PRODUCTS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Products]
if exists(select * from sys.objects where name = 'Products' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Region]
if exists(select * from sys.objects where name = 'Region' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Region' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__REGION_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Region]
if exists(select * from sys.objects where name = 'Region' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Region' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__REGION_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Region]
if exists(select * from sys.objects where name = 'Region' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Shippers]
if exists(select * from sys.objects where name = 'Shippers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Shippers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__SHIPPERS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Shippers]
if exists(select * from sys.objects where name = 'Shippers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Shippers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__SHIPPERS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Shippers]
if exists(select * from sys.objects where name = 'Shippers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Suppliers]
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__SUPPLIERS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Suppliers]
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__SUPPLIERS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Suppliers]
if exists(select * from sys.objects where name = 'Suppliers' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Territories]
if exists(select * from sys.objects where name = 'Territories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [CreatedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Territories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__TERRITORIES_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Territories]
if exists(select * from sys.objects where name = 'Territories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [ModifiedBy] [Varchar] (50) NULL
if exists(select * from sys.objects where name = 'Territories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__TERRITORIES_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Territories]
if exists(select * from sys.objects where name = 'Territories' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [Timestamp] [timestamp] NOT NULL

GO

--##SECTION END [AUDIT TRAIL CREATE]

--##SECTION BEGIN [AUDIT TRAIL REMOVE]

--##SECTION END [AUDIT TRAIL REMOVE]

--##SECTION BEGIN [RENAME PK]

--RENAME EXISTING PRIMARY KEYS IF NECESSARY
DECLARE @pkfixCategory varchar(500)
SET @pkfixCategory = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Categories')
if @pkfixCategory <> '' and (BINARY_CHECKSUM(@pkfixCategory) <> BINARY_CHECKSUM('PK_CATEGORIES')) exec('sp_rename '''+@pkfixCategory+''', ''PK_CATEGORIES''')
DECLARE @pkfixCustomerCustomerDemo varchar(500)
SET @pkfixCustomerCustomerDemo = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'CustomerCustomerDemo')
if @pkfixCustomerCustomerDemo <> '' and (BINARY_CHECKSUM(@pkfixCustomerCustomerDemo) <> BINARY_CHECKSUM('PK_CUSTOMERCUSTOMERDEMO')) exec('sp_rename '''+@pkfixCustomerCustomerDemo+''', ''PK_CUSTOMERCUSTOMERDEMO''')
DECLARE @pkfixCustomerDemographic varchar(500)
SET @pkfixCustomerDemographic = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'CustomerDemographics')
if @pkfixCustomerDemographic <> '' and (BINARY_CHECKSUM(@pkfixCustomerDemographic) <> BINARY_CHECKSUM('PK_CUSTOMERDEMOGRAPHICS')) exec('sp_rename '''+@pkfixCustomerDemographic+''', ''PK_CUSTOMERDEMOGRAPHICS''')
DECLARE @pkfixCustomer varchar(500)
SET @pkfixCustomer = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Customers')
if @pkfixCustomer <> '' and (BINARY_CHECKSUM(@pkfixCustomer) <> BINARY_CHECKSUM('PK_CUSTOMERS')) exec('sp_rename '''+@pkfixCustomer+''', ''PK_CUSTOMERS''')
DECLARE @pkfixEmployee varchar(500)
SET @pkfixEmployee = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Employees')
if @pkfixEmployee <> '' and (BINARY_CHECKSUM(@pkfixEmployee) <> BINARY_CHECKSUM('PK_EMPLOYEES')) exec('sp_rename '''+@pkfixEmployee+''', ''PK_EMPLOYEES''')
DECLARE @pkfixEmployeeTerritorie varchar(500)
SET @pkfixEmployeeTerritorie = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'EmployeeTerritories')
if @pkfixEmployeeTerritorie <> '' and (BINARY_CHECKSUM(@pkfixEmployeeTerritorie) <> BINARY_CHECKSUM('PK_EMPLOYEETERRITORIES')) exec('sp_rename '''+@pkfixEmployeeTerritorie+''', ''PK_EMPLOYEETERRITORIES''')
DECLARE @pkfixOrderDetail varchar(500)
SET @pkfixOrderDetail = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Order Details')
if @pkfixOrderDetail <> '' and (BINARY_CHECKSUM(@pkfixOrderDetail) <> BINARY_CHECKSUM('PK_ORDER DETAILS')) exec('sp_rename '''+@pkfixOrderDetail+''', ''PK_ORDER DETAILS''')
DECLARE @pkfixOrder varchar(500)
SET @pkfixOrder = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Orders')
if @pkfixOrder <> '' and (BINARY_CHECKSUM(@pkfixOrder) <> BINARY_CHECKSUM('PK_ORDERS')) exec('sp_rename '''+@pkfixOrder+''', ''PK_ORDERS''')
DECLARE @pkfixProduct varchar(500)
SET @pkfixProduct = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Products')
if @pkfixProduct <> '' and (BINARY_CHECKSUM(@pkfixProduct) <> BINARY_CHECKSUM('PK_PRODUCTS')) exec('sp_rename '''+@pkfixProduct+''', ''PK_PRODUCTS''')
DECLARE @pkfixRegion varchar(500)
SET @pkfixRegion = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Region')
if @pkfixRegion <> '' and (BINARY_CHECKSUM(@pkfixRegion) <> BINARY_CHECKSUM('PK_REGION')) exec('sp_rename '''+@pkfixRegion+''', ''PK_REGION''')
DECLARE @pkfixShipper varchar(500)
SET @pkfixShipper = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Shippers')
if @pkfixShipper <> '' and (BINARY_CHECKSUM(@pkfixShipper) <> BINARY_CHECKSUM('PK_SHIPPERS')) exec('sp_rename '''+@pkfixShipper+''', ''PK_SHIPPERS''')
DECLARE @pkfixSupplier varchar(500)
SET @pkfixSupplier = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Suppliers')
if @pkfixSupplier <> '' and (BINARY_CHECKSUM(@pkfixSupplier) <> BINARY_CHECKSUM('PK_SUPPLIERS')) exec('sp_rename '''+@pkfixSupplier+''', ''PK_SUPPLIERS''')
DECLARE @pkfixTerritory varchar(500)
SET @pkfixTerritory = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = 'Territories')
if @pkfixTerritory <> '' and (BINARY_CHECKSUM(@pkfixTerritory) <> BINARY_CHECKSUM('PK_TERRITORIES')) exec('sp_rename '''+@pkfixTerritory+''', ''PK_TERRITORIES''')
GO

--##SECTION END [RENAME PK]

--##SECTION BEGIN [DROP PK]

--##SECTION END [DROP PK]

--##SECTION BEGIN [CREATE PK]

--PRIMARY KEY FOR TABLE [Categories]
if not exists(select * from sysobjects where name = 'PK_CATEGORIES' and xtype = 'PK')
ALTER TABLE [dbo].[Categories] WITH NOCHECK ADD 
CONSTRAINT [PK_CATEGORIES] PRIMARY KEY CLUSTERED
(
	[CategoryID]
)
GO

--PRIMARY KEY FOR TABLE [CustomerCustomerDemo]
if not exists(select * from sysobjects where name = 'PK_CUSTOMERCUSTOMERDEMO' and xtype = 'PK')
ALTER TABLE [dbo].[CustomerCustomerDemo] WITH NOCHECK ADD 
CONSTRAINT [PK_CUSTOMERCUSTOMERDEMO] PRIMARY KEY CLUSTERED
(
	[CustomerID],
	[CustomerTypeID]
)
GO

--PRIMARY KEY FOR TABLE [CustomerDemographics]
if not exists(select * from sysobjects where name = 'PK_CUSTOMERDEMOGRAPHICS' and xtype = 'PK')
ALTER TABLE [dbo].[CustomerDemographics] WITH NOCHECK ADD 
CONSTRAINT [PK_CUSTOMERDEMOGRAPHICS] PRIMARY KEY CLUSTERED
(
	[CustomerTypeID]
)
GO

--PRIMARY KEY FOR TABLE [Customers]
if not exists(select * from sysobjects where name = 'PK_CUSTOMERS' and xtype = 'PK')
ALTER TABLE [dbo].[Customers] WITH NOCHECK ADD 
CONSTRAINT [PK_CUSTOMERS] PRIMARY KEY CLUSTERED
(
	[CustomerID]
)
GO

--PRIMARY KEY FOR TABLE [Employees]
if not exists(select * from sysobjects where name = 'PK_EMPLOYEES' and xtype = 'PK')
ALTER TABLE [dbo].[Employees] WITH NOCHECK ADD 
CONSTRAINT [PK_EMPLOYEES] PRIMARY KEY CLUSTERED
(
	[EmployeeID]
)
GO

--PRIMARY KEY FOR TABLE [EmployeeTerritories]
if not exists(select * from sysobjects where name = 'PK_EMPLOYEETERRITORIES' and xtype = 'PK')
ALTER TABLE [dbo].[EmployeeTerritories] WITH NOCHECK ADD 
CONSTRAINT [PK_EMPLOYEETERRITORIES] PRIMARY KEY CLUSTERED
(
	[EmployeeID],
	[TerritoryID]
)
GO

--PRIMARY KEY FOR TABLE [Order Details]
if not exists(select * from sysobjects where name = 'PK_ORDER DETAILS' and xtype = 'PK')
ALTER TABLE [dbo].[Order Details] WITH NOCHECK ADD 
CONSTRAINT [PK_ORDER DETAILS] PRIMARY KEY CLUSTERED
(
	[OrderID],
	[ProductID]
)
GO

--PRIMARY KEY FOR TABLE [Orders]
if not exists(select * from sysobjects where name = 'PK_ORDERS' and xtype = 'PK')
ALTER TABLE [dbo].[Orders] WITH NOCHECK ADD 
CONSTRAINT [PK_ORDERS] PRIMARY KEY CLUSTERED
(
	[OrderID]
)
GO

--PRIMARY KEY FOR TABLE [Products]
if not exists(select * from sysobjects where name = 'PK_PRODUCTS' and xtype = 'PK')
ALTER TABLE [dbo].[Products] WITH NOCHECK ADD 
CONSTRAINT [PK_PRODUCTS] PRIMARY KEY CLUSTERED
(
	[ProductID]
)
GO

--PRIMARY KEY FOR TABLE [Region]
if not exists(select * from sysobjects where name = 'PK_REGION' and xtype = 'PK')
ALTER TABLE [dbo].[Region] WITH NOCHECK ADD 
CONSTRAINT [PK_REGION] PRIMARY KEY CLUSTERED
(
	[RegionID]
)
GO

--PRIMARY KEY FOR TABLE [Shippers]
if not exists(select * from sysobjects where name = 'PK_SHIPPERS' and xtype = 'PK')
ALTER TABLE [dbo].[Shippers] WITH NOCHECK ADD 
CONSTRAINT [PK_SHIPPERS] PRIMARY KEY CLUSTERED
(
	[ShipperID]
)
GO

--PRIMARY KEY FOR TABLE [Suppliers]
if not exists(select * from sysobjects where name = 'PK_SUPPLIERS' and xtype = 'PK')
ALTER TABLE [dbo].[Suppliers] WITH NOCHECK ADD 
CONSTRAINT [PK_SUPPLIERS] PRIMARY KEY CLUSTERED
(
	[SupplierID]
)
GO

--PRIMARY KEY FOR TABLE [Territories]
if not exists(select * from sysobjects where name = 'PK_TERRITORIES' and xtype = 'PK')
ALTER TABLE [dbo].[Territories] WITH NOCHECK ADD 
CONSTRAINT [PK_TERRITORIES] PRIMARY KEY CLUSTERED
(
	[TerritoryID]
)
GO

--##SECTION END [CREATE PK]

--##SECTION BEGIN [AUDIT TABLES PK]

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Categories]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__CATEGORIES'))
ALTER TABLE [dbo].[__AUDIT__Categories] DROP CONSTRAINT [PK___AUDIT__CATEGORIES]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__CustomerCustomerDemo]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__CUSTOMERCUSTOMERDEMO'))
ALTER TABLE [dbo].[__AUDIT__CustomerCustomerDemo] DROP CONSTRAINT [PK___AUDIT__CUSTOMERCUSTOMERDEMO]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__CustomerDemographics]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__CUSTOMERDEMOGRAPHICS'))
ALTER TABLE [dbo].[__AUDIT__CustomerDemographics] DROP CONSTRAINT [PK___AUDIT__CUSTOMERDEMOGRAPHICS]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Customers]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__CUSTOMERS'))
ALTER TABLE [dbo].[__AUDIT__Customers] DROP CONSTRAINT [PK___AUDIT__CUSTOMERS]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Employees]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__EMPLOYEES'))
ALTER TABLE [dbo].[__AUDIT__Employees] DROP CONSTRAINT [PK___AUDIT__EMPLOYEES]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__EmployeeTerritories]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__EMPLOYEETERRITORIES'))
ALTER TABLE [dbo].[__AUDIT__EmployeeTerritories] DROP CONSTRAINT [PK___AUDIT__EMPLOYEETERRITORIES]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Order Details]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__ORDER DETAILS'))
ALTER TABLE [dbo].[__AUDIT__Order Details] DROP CONSTRAINT [PK___AUDIT__ORDER DETAILS]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Orders]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__ORDERS'))
ALTER TABLE [dbo].[__AUDIT__Orders] DROP CONSTRAINT [PK___AUDIT__ORDERS]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Products]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__PRODUCTS'))
ALTER TABLE [dbo].[__AUDIT__Products] DROP CONSTRAINT [PK___AUDIT__PRODUCTS]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Region]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__REGION'))
ALTER TABLE [dbo].[__AUDIT__Region] DROP CONSTRAINT [PK___AUDIT__REGION]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Shippers]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__SHIPPERS'))
ALTER TABLE [dbo].[__AUDIT__Shippers] DROP CONSTRAINT [PK___AUDIT__SHIPPERS]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Suppliers]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__SUPPLIERS'))
ALTER TABLE [dbo].[__AUDIT__Suppliers] DROP CONSTRAINT [PK___AUDIT__SUPPLIERS]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Territories]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__TERRITORIES'))
ALTER TABLE [dbo].[__AUDIT__Territories] DROP CONSTRAINT [PK___AUDIT__TERRITORIES]
GO

--##SECTION END [AUDIT TABLES PK]

--FOREIGN KEY RELATIONSHIP [CustomerDemographics] -> [CustomerCustomerDemo] ([CustomerDemographics].[CustomerTypeID] -> [CustomerCustomerDemo].[CustomerTypeID])
if not exists(select * from sysobjects where name = 'FK_CUSTOMERCUSTOMERDEMO_CUSTOMERCUSTOMERDEMO_CUSTOMERDEMOGRAPHICS' and xtype = 'F')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD 
CONSTRAINT [FK_CUSTOMERCUSTOMERDEMO_CUSTOMERCUSTOMERDEMO_CUSTOMERDEMOGRAPHICS] FOREIGN KEY 
(
	[CustomerTypeID]
) REFERENCES [dbo].[CustomerDemographics] (
	[CustomerTypeID]
)
GO

--FOREIGN KEY RELATIONSHIP [Customers] -> [CustomerCustomerDemo] ([Customers].[CustomerID] -> [CustomerCustomerDemo].[CustomerID])
if not exists(select * from sysobjects where name = 'FK__CUSTOMERCUSTOMERDEMO_CUSTOMERS' and xtype = 'F')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD 
CONSTRAINT [FK__CUSTOMERCUSTOMERDEMO_CUSTOMERS] FOREIGN KEY 
(
	[CustomerID]
) REFERENCES [dbo].[Customers] (
	[CustomerID]
)
GO

--FOREIGN KEY RELATIONSHIP [Employees] -> [Employees] ([Employees].[EmployeeID] -> [Employees].[ReportsTo])
if not exists(select * from sysobjects where name = 'FK_REPORTTO_EMPLOYEES_EMPLOYEES' and xtype = 'F')
ALTER TABLE [dbo].[Employees] ADD 
CONSTRAINT [FK_REPORTTO_EMPLOYEES_EMPLOYEES] FOREIGN KEY 
(
	[ReportsTo]
) REFERENCES [dbo].[Employees] (
	[EmployeeID]
)
GO

--FOREIGN KEY RELATIONSHIP [Employees] -> [EmployeeTerritories] ([Employees].[EmployeeID] -> [EmployeeTerritories].[EmployeeID])
if not exists(select * from sysobjects where name = 'FK__EMPLOYEETERRITORIES_EMPLOYEES' and xtype = 'F')
ALTER TABLE [dbo].[EmployeeTerritories] ADD 
CONSTRAINT [FK__EMPLOYEETERRITORIES_EMPLOYEES] FOREIGN KEY 
(
	[EmployeeID]
) REFERENCES [dbo].[Employees] (
	[EmployeeID]
)
GO

--FOREIGN KEY RELATIONSHIP [Territories] -> [EmployeeTerritories] ([Territories].[TerritoryID] -> [EmployeeTerritories].[TerritoryID])
if not exists(select * from sysobjects where name = 'FK__EMPLOYEETERRITORIES_TERRITORIES' and xtype = 'F')
ALTER TABLE [dbo].[EmployeeTerritories] ADD 
CONSTRAINT [FK__EMPLOYEETERRITORIES_TERRITORIES] FOREIGN KEY 
(
	[TerritoryID]
) REFERENCES [dbo].[Territories] (
	[TerritoryID]
)
GO

--FOREIGN KEY RELATIONSHIP [Orders] -> [Order Details] ([Orders].[OrderID] -> [Order Details].[OrderID])
if not exists(select * from sysobjects where name = 'FK_ORDER_DETAILS_ORDERS_ORDER_DETAILS_ORDERS' and xtype = 'F')
ALTER TABLE [dbo].[Order Details] ADD 
CONSTRAINT [FK_ORDER_DETAILS_ORDERS_ORDER_DETAILS_ORDERS] FOREIGN KEY 
(
	[OrderID]
) REFERENCES [dbo].[Orders] (
	[OrderID]
)
GO

--FOREIGN KEY RELATIONSHIP [Products] -> [Order Details] ([Products].[ProductID] -> [Order Details].[ProductID])
if not exists(select * from sysobjects where name = 'FK_ORDER_DETAILS_PRODUCTS_ORDER_DETAILS_PRODUCTS' and xtype = 'F')
ALTER TABLE [dbo].[Order Details] ADD 
CONSTRAINT [FK_ORDER_DETAILS_PRODUCTS_ORDER_DETAILS_PRODUCTS] FOREIGN KEY 
(
	[ProductID]
) REFERENCES [dbo].[Products] (
	[ProductID]
)
GO

--FOREIGN KEY RELATIONSHIP [Customers] -> [Orders] ([Customers].[CustomerID] -> [Orders].[CustomerID])
if not exists(select * from sysobjects where name = 'FK__ORDERS_CUSTOMERS' and xtype = 'F')
ALTER TABLE [dbo].[Orders] ADD 
CONSTRAINT [FK__ORDERS_CUSTOMERS] FOREIGN KEY 
(
	[CustomerID]
) REFERENCES [dbo].[Customers] (
	[CustomerID]
)
GO

--FOREIGN KEY RELATIONSHIP [Employees] -> [Orders] ([Employees].[EmployeeID] -> [Orders].[EmployeeID])
if not exists(select * from sysobjects where name = 'FK__ORDERS_EMPLOYEES' and xtype = 'F')
ALTER TABLE [dbo].[Orders] ADD 
CONSTRAINT [FK__ORDERS_EMPLOYEES] FOREIGN KEY 
(
	[EmployeeID]
) REFERENCES [dbo].[Employees] (
	[EmployeeID]
)
GO

--FOREIGN KEY RELATIONSHIP [Shippers] -> [Orders] ([Shippers].[ShipperID] -> [Orders].[ShipVia])
if not exists(select * from sysobjects where name = 'FK__ORDERS_SHIPPERS' and xtype = 'F')
ALTER TABLE [dbo].[Orders] ADD 
CONSTRAINT [FK__ORDERS_SHIPPERS] FOREIGN KEY 
(
	[ShipVia]
) REFERENCES [dbo].[Shippers] (
	[ShipperID]
)
GO

--FOREIGN KEY RELATIONSHIP [Categories] -> [Products] ([Categories].[CategoryID] -> [Products].[CategoryID])
if not exists(select * from sysobjects where name = 'FK__PRODUCTS_CATEGORIES' and xtype = 'F')
ALTER TABLE [dbo].[Products] ADD 
CONSTRAINT [FK__PRODUCTS_CATEGORIES] FOREIGN KEY 
(
	[CategoryID]
) REFERENCES [dbo].[Categories] (
	[CategoryID]
)
GO

--FOREIGN KEY RELATIONSHIP [Suppliers] -> [Products] ([Suppliers].[SupplierID] -> [Products].[SupplierID])
if not exists(select * from sysobjects where name = 'FK__PRODUCTS_SUPPLIERS' and xtype = 'F')
ALTER TABLE [dbo].[Products] ADD 
CONSTRAINT [FK__PRODUCTS_SUPPLIERS] FOREIGN KEY 
(
	[SupplierID]
) REFERENCES [dbo].[Suppliers] (
	[SupplierID]
)
GO

--FOREIGN KEY RELATIONSHIP [Region] -> [Territories] ([Region].[RegionID] -> [Territories].[RegionID])
if not exists(select * from sysobjects where name = 'FK__TERRITORIES_REGION' and xtype = 'F')
ALTER TABLE [dbo].[Territories] ADD 
CONSTRAINT [FK__TERRITORIES_REGION] FOREIGN KEY 
(
	[RegionID]
) REFERENCES [dbo].[Region] (
	[RegionID]
)
GO

--##SECTION BEGIN [CREATE INDEXES]

if exists(select * from sys.indexes where name = 'IDX_CATEGORIES_CATEGORYNAME' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_CATEGORIES_CATEGORYNAME] ON [dbo].[Categories]

--INDEX FOR TABLE [Categories] COLUMNS: [CategoryName]
if not exists(select * from sys.indexes where name = 'IDX_CATEGORIES_CATEGORYNAME')
CREATE NONCLUSTERED INDEX [IDX_CATEGORIES_CATEGORYNAME] ON [dbo].[Categories] ([CategoryName] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_CITY' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_CUSTOMERS_CITY] ON [dbo].[Customers]

--INDEX FOR TABLE [Customers] COLUMNS: [City]
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_CITY')
CREATE NONCLUSTERED INDEX [IDX_CUSTOMERS_CITY] ON [dbo].[Customers] ([City] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_COMPANYNAME' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_CUSTOMERS_COMPANYNAME] ON [dbo].[Customers]

--INDEX FOR TABLE [Customers] COLUMNS: [CompanyName]
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_COMPANYNAME')
CREATE NONCLUSTERED INDEX [IDX_CUSTOMERS_COMPANYNAME] ON [dbo].[Customers] ([CompanyName] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_POSTALCODE' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_CUSTOMERS_POSTALCODE] ON [dbo].[Customers]

--INDEX FOR TABLE [Customers] COLUMNS: [PostalCode]
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_POSTALCODE')
CREATE NONCLUSTERED INDEX [IDX_CUSTOMERS_POSTALCODE] ON [dbo].[Customers] ([PostalCode] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_REGION' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_CUSTOMERS_REGION] ON [dbo].[Customers]

--INDEX FOR TABLE [Customers] COLUMNS: [Region]
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_REGION')
CREATE NONCLUSTERED INDEX [IDX_CUSTOMERS_REGION] ON [dbo].[Customers] ([Region] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_LASTNAME' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_EMPLOYEES_LASTNAME] ON [dbo].[Employees]

--INDEX FOR TABLE [Employees] COLUMNS: [LastName]
if not exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_LASTNAME')
CREATE NONCLUSTERED INDEX [IDX_EMPLOYEES_LASTNAME] ON [dbo].[Employees] ([LastName] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_POSTALCODE' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_EMPLOYEES_POSTALCODE] ON [dbo].[Employees]

--INDEX FOR TABLE [Employees] COLUMNS: [PostalCode]
if not exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_POSTALCODE')
CREATE NONCLUSTERED INDEX [IDX_EMPLOYEES_POSTALCODE] ON [dbo].[Employees] ([PostalCode] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_ORDERS_CUSTOMERID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_ORDERS_CUSTOMERID] ON [dbo].[Orders]

--INDEX FOR TABLE [Orders] COLUMNS: [CustomerID]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_CUSTOMERID')
CREATE NONCLUSTERED INDEX [IDX_ORDERS_CUSTOMERID] ON [dbo].[Orders] ([CustomerID] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_ORDERS_EMPLOYEEID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_ORDERS_EMPLOYEEID] ON [dbo].[Orders]

--INDEX FOR TABLE [Orders] COLUMNS: [EmployeeID]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_EMPLOYEEID')
CREATE NONCLUSTERED INDEX [IDX_ORDERS_EMPLOYEEID] ON [dbo].[Orders] ([EmployeeID] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_ORDERS_ORDERDATE' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_ORDERS_ORDERDATE] ON [dbo].[Orders]

--INDEX FOR TABLE [Orders] COLUMNS: [OrderDate]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_ORDERDATE')
CREATE NONCLUSTERED INDEX [IDX_ORDERS_ORDERDATE] ON [dbo].[Orders] ([OrderDate] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPEDDATE' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_ORDERS_SHIPPEDDATE] ON [dbo].[Orders]

--INDEX FOR TABLE [Orders] COLUMNS: [ShippedDate]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPEDDATE')
CREATE NONCLUSTERED INDEX [IDX_ORDERS_SHIPPEDDATE] ON [dbo].[Orders] ([ShippedDate] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPOSTALCODE' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_ORDERS_SHIPPOSTALCODE] ON [dbo].[Orders]

--INDEX FOR TABLE [Orders] COLUMNS: [ShipPostalCode]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPOSTALCODE')
CREATE NONCLUSTERED INDEX [IDX_ORDERS_SHIPPOSTALCODE] ON [dbo].[Orders] ([ShipPostalCode] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPVIA' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_ORDERS_SHIPVIA] ON [dbo].[Orders]

--INDEX FOR TABLE [Orders] COLUMNS: [ShipVia]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPVIA')
CREATE NONCLUSTERED INDEX [IDX_ORDERS_SHIPVIA] ON [dbo].[Orders] ([ShipVia] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_PRODUCTS_CATEGORYID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_PRODUCTS_CATEGORYID] ON [dbo].[Products]

--INDEX FOR TABLE [Products] COLUMNS: [CategoryID]
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_CATEGORYID')
CREATE NONCLUSTERED INDEX [IDX_PRODUCTS_CATEGORYID] ON [dbo].[Products] ([CategoryID] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_PRODUCTS_PRODUCTNAME' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_PRODUCTS_PRODUCTNAME] ON [dbo].[Products]

--INDEX FOR TABLE [Products] COLUMNS: [ProductName]
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_PRODUCTNAME')
CREATE NONCLUSTERED INDEX [IDX_PRODUCTS_PRODUCTNAME] ON [dbo].[Products] ([ProductName] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_PRODUCTS_SUPPLIERID' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_PRODUCTS_SUPPLIERID] ON [dbo].[Products]

--INDEX FOR TABLE [Products] COLUMNS: [SupplierID]
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_SUPPLIERID')
CREATE NONCLUSTERED INDEX [IDX_PRODUCTS_SUPPLIERID] ON [dbo].[Products] ([SupplierID] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_COMPANYNAME' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_SUPPLIERS_COMPANYNAME] ON [dbo].[Suppliers]

--INDEX FOR TABLE [Suppliers] COLUMNS: [CompanyName]
if not exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_COMPANYNAME')
CREATE NONCLUSTERED INDEX [IDX_SUPPLIERS_COMPANYNAME] ON [dbo].[Suppliers] ([CompanyName] ASC)

GO

if exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_POSTALCODE' and type_desc = 'CLUSTERED')
DROP INDEX [IDX_SUPPLIERS_POSTALCODE] ON [dbo].[Suppliers]

--INDEX FOR TABLE [Suppliers] COLUMNS: [PostalCode]
if not exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_POSTALCODE')
CREATE NONCLUSTERED INDEX [IDX_SUPPLIERS_POSTALCODE] ON [dbo].[Suppliers] ([PostalCode] ASC)

GO

--##SECTION END [CREATE INDEXES]

--##SECTION BEGIN [TENANT INDEXES]

--##SECTION END [TENANT INDEXES]

--##SECTION BEGIN [REMOVE DEFAULTS]

--BEGIN DEFAULTS FOR TABLE [Categories]
--DROP CONSTRAINT FOR '[Categories].[CategoryID]' if one exists
declare @Category_CategoryID varchar(500)
set @Category_CategoryID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Categories' and a.name = 'CategoryID')
if (@Category_CategoryID IS NOT NULL) exec ('ALTER TABLE [Categories] DROP CONSTRAINT [' + @Category_CategoryID + ']')

--DROP CONSTRAINT FOR '[Categories].[CategoryName]' if one exists
declare @Category_CategoryName varchar(500)
set @Category_CategoryName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Categories' and a.name = 'CategoryName')
if (@Category_CategoryName IS NOT NULL) exec ('ALTER TABLE [Categories] DROP CONSTRAINT [' + @Category_CategoryName + ']')

--DROP CONSTRAINT FOR '[Categories].[Description]' if one exists
declare @Category_Description varchar(500)
set @Category_Description = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Categories' and a.name = 'Description')
if (@Category_Description IS NOT NULL) exec ('ALTER TABLE [Categories] DROP CONSTRAINT [' + @Category_Description + ']')

--DROP CONSTRAINT FOR '[Categories].[Picture]' if one exists
declare @Category_Picture varchar(500)
set @Category_Picture = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Categories' and a.name = 'Picture')
if (@Category_Picture IS NOT NULL) exec ('ALTER TABLE [Categories] DROP CONSTRAINT [' + @Category_Picture + ']')

--END DEFAULTS FOR TABLE [Categories]
GO

--BEGIN DEFAULTS FOR TABLE [CustomerCustomerDemo]
--DROP CONSTRAINT FOR '[CustomerCustomerDemo].[CustomerID]' if one exists
declare @CustomerCustomerDemo_CustomerID varchar(500)
set @CustomerCustomerDemo_CustomerID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='CustomerCustomerDemo' and a.name = 'CustomerID')
if (@CustomerCustomerDemo_CustomerID IS NOT NULL) exec ('ALTER TABLE [CustomerCustomerDemo] DROP CONSTRAINT [' + @CustomerCustomerDemo_CustomerID + ']')

--DROP CONSTRAINT FOR '[CustomerCustomerDemo].[CustomerTypeID]' if one exists
declare @CustomerCustomerDemo_CustomerTypeID varchar(500)
set @CustomerCustomerDemo_CustomerTypeID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='CustomerCustomerDemo' and a.name = 'CustomerTypeID')
if (@CustomerCustomerDemo_CustomerTypeID IS NOT NULL) exec ('ALTER TABLE [CustomerCustomerDemo] DROP CONSTRAINT [' + @CustomerCustomerDemo_CustomerTypeID + ']')

--END DEFAULTS FOR TABLE [CustomerCustomerDemo]
GO

--BEGIN DEFAULTS FOR TABLE [CustomerDemographics]
--DROP CONSTRAINT FOR '[CustomerDemographics].[CustomerDesc]' if one exists
declare @CustomerDemographic_CustomerDesc varchar(500)
set @CustomerDemographic_CustomerDesc = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='CustomerDemographics' and a.name = 'CustomerDesc')
if (@CustomerDemographic_CustomerDesc IS NOT NULL) exec ('ALTER TABLE [CustomerDemographics] DROP CONSTRAINT [' + @CustomerDemographic_CustomerDesc + ']')

--DROP CONSTRAINT FOR '[CustomerDemographics].[CustomerTypeID]' if one exists
declare @CustomerDemographic_CustomerTypeID varchar(500)
set @CustomerDemographic_CustomerTypeID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='CustomerDemographics' and a.name = 'CustomerTypeID')
if (@CustomerDemographic_CustomerTypeID IS NOT NULL) exec ('ALTER TABLE [CustomerDemographics] DROP CONSTRAINT [' + @CustomerDemographic_CustomerTypeID + ']')

--END DEFAULTS FOR TABLE [CustomerDemographics]
GO

--BEGIN DEFAULTS FOR TABLE [Customers]
--DROP CONSTRAINT FOR '[Customers].[Address]' if one exists
declare @Customer_Address varchar(500)
set @Customer_Address = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'Address')
if (@Customer_Address IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_Address + ']')

--DROP CONSTRAINT FOR '[Customers].[City]' if one exists
declare @Customer_City varchar(500)
set @Customer_City = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'City')
if (@Customer_City IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_City + ']')

--DROP CONSTRAINT FOR '[Customers].[CompanyName]' if one exists
declare @Customer_CompanyName varchar(500)
set @Customer_CompanyName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'CompanyName')
if (@Customer_CompanyName IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_CompanyName + ']')

--DROP CONSTRAINT FOR '[Customers].[ContactName]' if one exists
declare @Customer_ContactName varchar(500)
set @Customer_ContactName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'ContactName')
if (@Customer_ContactName IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_ContactName + ']')

--DROP CONSTRAINT FOR '[Customers].[ContactTitle]' if one exists
declare @Customer_ContactTitle varchar(500)
set @Customer_ContactTitle = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'ContactTitle')
if (@Customer_ContactTitle IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_ContactTitle + ']')

--DROP CONSTRAINT FOR '[Customers].[Country]' if one exists
declare @Customer_Country varchar(500)
set @Customer_Country = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'Country')
if (@Customer_Country IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_Country + ']')

--DROP CONSTRAINT FOR '[Customers].[CustomerID]' if one exists
declare @Customer_CustomerID varchar(500)
set @Customer_CustomerID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'CustomerID')
if (@Customer_CustomerID IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_CustomerID + ']')

--DROP CONSTRAINT FOR '[Customers].[Fax]' if one exists
declare @Customer_Fax varchar(500)
set @Customer_Fax = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'Fax')
if (@Customer_Fax IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_Fax + ']')

--DROP CONSTRAINT FOR '[Customers].[Phone]' if one exists
declare @Customer_Phone varchar(500)
set @Customer_Phone = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'Phone')
if (@Customer_Phone IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_Phone + ']')

--DROP CONSTRAINT FOR '[Customers].[PostalCode]' if one exists
declare @Customer_PostalCode varchar(500)
set @Customer_PostalCode = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'PostalCode')
if (@Customer_PostalCode IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_PostalCode + ']')

--DROP CONSTRAINT FOR '[Customers].[Region]' if one exists
declare @Customer_Region varchar(500)
set @Customer_Region = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Customers' and a.name = 'Region')
if (@Customer_Region IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Customer_Region + ']')

--END DEFAULTS FOR TABLE [Customers]
GO

--BEGIN DEFAULTS FOR TABLE [Employees]
--DROP CONSTRAINT FOR '[Employees].[Address]' if one exists
declare @Employee_Address varchar(500)
set @Employee_Address = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'Address')
if (@Employee_Address IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_Address + ']')

--DROP CONSTRAINT FOR '[Employees].[BirthDate]' if one exists
declare @Employee_BirthDate varchar(500)
set @Employee_BirthDate = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'BirthDate')
if (@Employee_BirthDate IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_BirthDate + ']')

--DROP CONSTRAINT FOR '[Employees].[City]' if one exists
declare @Employee_City varchar(500)
set @Employee_City = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'City')
if (@Employee_City IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_City + ']')

--DROP CONSTRAINT FOR '[Employees].[Country]' if one exists
declare @Employee_Country varchar(500)
set @Employee_Country = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'Country')
if (@Employee_Country IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_Country + ']')

--DROP CONSTRAINT FOR '[Employees].[EmployeeID]' if one exists
declare @Employee_EmployeeID varchar(500)
set @Employee_EmployeeID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'EmployeeID')
if (@Employee_EmployeeID IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_EmployeeID + ']')

--DROP CONSTRAINT FOR '[Employees].[Extension]' if one exists
declare @Employee_Extension varchar(500)
set @Employee_Extension = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'Extension')
if (@Employee_Extension IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_Extension + ']')

--DROP CONSTRAINT FOR '[Employees].[FirstName]' if one exists
declare @Employee_FirstName varchar(500)
set @Employee_FirstName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'FirstName')
if (@Employee_FirstName IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_FirstName + ']')

--DROP CONSTRAINT FOR '[Employees].[HireDate]' if one exists
declare @Employee_HireDate varchar(500)
set @Employee_HireDate = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'HireDate')
if (@Employee_HireDate IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_HireDate + ']')

--DROP CONSTRAINT FOR '[Employees].[HomePhone]' if one exists
declare @Employee_HomePhone varchar(500)
set @Employee_HomePhone = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'HomePhone')
if (@Employee_HomePhone IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_HomePhone + ']')

--DROP CONSTRAINT FOR '[Employees].[LastName]' if one exists
declare @Employee_LastName varchar(500)
set @Employee_LastName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'LastName')
if (@Employee_LastName IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_LastName + ']')

--DROP CONSTRAINT FOR '[Employees].[Notes]' if one exists
declare @Employee_Notes varchar(500)
set @Employee_Notes = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'Notes')
if (@Employee_Notes IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_Notes + ']')

--DROP CONSTRAINT FOR '[Employees].[Photo]' if one exists
declare @Employee_Photo varchar(500)
set @Employee_Photo = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'Photo')
if (@Employee_Photo IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_Photo + ']')

--DROP CONSTRAINT FOR '[Employees].[PhotoPath]' if one exists
declare @Employee_PhotoPath varchar(500)
set @Employee_PhotoPath = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'PhotoPath')
if (@Employee_PhotoPath IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_PhotoPath + ']')

--DROP CONSTRAINT FOR '[Employees].[PostalCode]' if one exists
declare @Employee_PostalCode varchar(500)
set @Employee_PostalCode = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'PostalCode')
if (@Employee_PostalCode IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_PostalCode + ']')

--DROP CONSTRAINT FOR '[Employees].[Region]' if one exists
declare @Employee_Region varchar(500)
set @Employee_Region = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'Region')
if (@Employee_Region IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_Region + ']')

--DROP CONSTRAINT FOR '[Employees].[ReportsTo]' if one exists
declare @Employee_ReportsTo varchar(500)
set @Employee_ReportsTo = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'ReportsTo')
if (@Employee_ReportsTo IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_ReportsTo + ']')

--DROP CONSTRAINT FOR '[Employees].[Title]' if one exists
declare @Employee_Title varchar(500)
set @Employee_Title = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'Title')
if (@Employee_Title IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_Title + ']')

--DROP CONSTRAINT FOR '[Employees].[TitleOfCourtesy]' if one exists
declare @Employee_TitleOfCourtesy varchar(500)
set @Employee_TitleOfCourtesy = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Employees' and a.name = 'TitleOfCourtesy')
if (@Employee_TitleOfCourtesy IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Employee_TitleOfCourtesy + ']')

--END DEFAULTS FOR TABLE [Employees]
GO

--BEGIN DEFAULTS FOR TABLE [EmployeeTerritories]
--DROP CONSTRAINT FOR '[EmployeeTerritories].[EmployeeID]' if one exists
declare @EmployeeTerritorie_EmployeeID varchar(500)
set @EmployeeTerritorie_EmployeeID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='EmployeeTerritories' and a.name = 'EmployeeID')
if (@EmployeeTerritorie_EmployeeID IS NOT NULL) exec ('ALTER TABLE [EmployeeTerritories] DROP CONSTRAINT [' + @EmployeeTerritorie_EmployeeID + ']')

--DROP CONSTRAINT FOR '[EmployeeTerritories].[TerritoryID]' if one exists
declare @EmployeeTerritorie_TerritoryID varchar(500)
set @EmployeeTerritorie_TerritoryID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='EmployeeTerritories' and a.name = 'TerritoryID')
if (@EmployeeTerritorie_TerritoryID IS NOT NULL) exec ('ALTER TABLE [EmployeeTerritories] DROP CONSTRAINT [' + @EmployeeTerritorie_TerritoryID + ']')

--END DEFAULTS FOR TABLE [EmployeeTerritories]
GO

--BEGIN DEFAULTS FOR TABLE [Order Details]
--DROP CONSTRAINT FOR '[Order Details].[Discount]' if one exists
declare @OrderDetail_Discount varchar(500)
set @OrderDetail_Discount = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Order Details' and a.name = 'Discount')
if (@OrderDetail_Discount IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @OrderDetail_Discount + ']')

--DROP CONSTRAINT FOR '[Order Details].[OrderID]' if one exists
declare @OrderDetail_OrderID varchar(500)
set @OrderDetail_OrderID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Order Details' and a.name = 'OrderID')
if (@OrderDetail_OrderID IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @OrderDetail_OrderID + ']')

--DROP CONSTRAINT FOR '[Order Details].[ProductID]' if one exists
declare @OrderDetail_ProductID varchar(500)
set @OrderDetail_ProductID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Order Details' and a.name = 'ProductID')
if (@OrderDetail_ProductID IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @OrderDetail_ProductID + ']')

--DROP CONSTRAINT FOR '[Order Details].[Quantity]' if one exists
declare @OrderDetail_Quantity varchar(500)
set @OrderDetail_Quantity = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Order Details' and a.name = 'Quantity')
if (@OrderDetail_Quantity IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @OrderDetail_Quantity + ']')

--DROP CONSTRAINT FOR '[Order Details].[UnitPrice]' if one exists
declare @OrderDetail_UnitPrice varchar(500)
set @OrderDetail_UnitPrice = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Order Details' and a.name = 'UnitPrice')
if (@OrderDetail_UnitPrice IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @OrderDetail_UnitPrice + ']')

--END DEFAULTS FOR TABLE [Order Details]
GO

--BEGIN DEFAULTS FOR TABLE [Orders]
--DROP CONSTRAINT FOR '[Orders].[CustomerID]' if one exists
declare @Order_CustomerID varchar(500)
set @Order_CustomerID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'CustomerID')
if (@Order_CustomerID IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_CustomerID + ']')

--DROP CONSTRAINT FOR '[Orders].[EmployeeID]' if one exists
declare @Order_EmployeeID varchar(500)
set @Order_EmployeeID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'EmployeeID')
if (@Order_EmployeeID IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_EmployeeID + ']')

--DROP CONSTRAINT FOR '[Orders].[Freight]' if one exists
declare @Order_Freight varchar(500)
set @Order_Freight = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'Freight')
if (@Order_Freight IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_Freight + ']')

--DROP CONSTRAINT FOR '[Orders].[OrderDate]' if one exists
declare @Order_OrderDate varchar(500)
set @Order_OrderDate = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'OrderDate')
if (@Order_OrderDate IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_OrderDate + ']')

--DROP CONSTRAINT FOR '[Orders].[OrderID]' if one exists
declare @Order_OrderID varchar(500)
set @Order_OrderID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'OrderID')
if (@Order_OrderID IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_OrderID + ']')

--DROP CONSTRAINT FOR '[Orders].[RequiredDate]' if one exists
declare @Order_RequiredDate varchar(500)
set @Order_RequiredDate = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'RequiredDate')
if (@Order_RequiredDate IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_RequiredDate + ']')

--DROP CONSTRAINT FOR '[Orders].[ShipAddress]' if one exists
declare @Order_ShipAddress varchar(500)
set @Order_ShipAddress = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'ShipAddress')
if (@Order_ShipAddress IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_ShipAddress + ']')

--DROP CONSTRAINT FOR '[Orders].[ShipCity]' if one exists
declare @Order_ShipCity varchar(500)
set @Order_ShipCity = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'ShipCity')
if (@Order_ShipCity IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_ShipCity + ']')

--DROP CONSTRAINT FOR '[Orders].[ShipCountry]' if one exists
declare @Order_ShipCountry varchar(500)
set @Order_ShipCountry = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'ShipCountry')
if (@Order_ShipCountry IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_ShipCountry + ']')

--DROP CONSTRAINT FOR '[Orders].[ShipName]' if one exists
declare @Order_ShipName varchar(500)
set @Order_ShipName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'ShipName')
if (@Order_ShipName IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_ShipName + ']')

--DROP CONSTRAINT FOR '[Orders].[ShippedDate]' if one exists
declare @Order_ShippedDate varchar(500)
set @Order_ShippedDate = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'ShippedDate')
if (@Order_ShippedDate IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_ShippedDate + ']')

--DROP CONSTRAINT FOR '[Orders].[ShipPostalCode]' if one exists
declare @Order_ShipPostalCode varchar(500)
set @Order_ShipPostalCode = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'ShipPostalCode')
if (@Order_ShipPostalCode IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_ShipPostalCode + ']')

--DROP CONSTRAINT FOR '[Orders].[ShipRegion]' if one exists
declare @Order_ShipRegion varchar(500)
set @Order_ShipRegion = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'ShipRegion')
if (@Order_ShipRegion IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_ShipRegion + ']')

--DROP CONSTRAINT FOR '[Orders].[ShipVia]' if one exists
declare @Order_ShipVia varchar(500)
set @Order_ShipVia = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Orders' and a.name = 'ShipVia')
if (@Order_ShipVia IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Order_ShipVia + ']')

--END DEFAULTS FOR TABLE [Orders]
GO

--BEGIN DEFAULTS FOR TABLE [Products]
--DROP CONSTRAINT FOR '[Products].[CategoryID]' if one exists
declare @Product_CategoryID varchar(500)
set @Product_CategoryID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'CategoryID')
if (@Product_CategoryID IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_CategoryID + ']')

--DROP CONSTRAINT FOR '[Products].[Discontinued]' if one exists
declare @Product_Discontinued varchar(500)
set @Product_Discontinued = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'Discontinued')
if (@Product_Discontinued IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_Discontinued + ']')

--DROP CONSTRAINT FOR '[Products].[ProductID]' if one exists
declare @Product_ProductID varchar(500)
set @Product_ProductID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'ProductID')
if (@Product_ProductID IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_ProductID + ']')

--DROP CONSTRAINT FOR '[Products].[ProductName]' if one exists
declare @Product_ProductName varchar(500)
set @Product_ProductName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'ProductName')
if (@Product_ProductName IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_ProductName + ']')

--DROP CONSTRAINT FOR '[Products].[QuantityPerUnit]' if one exists
declare @Product_QuantityPerUnit varchar(500)
set @Product_QuantityPerUnit = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'QuantityPerUnit')
if (@Product_QuantityPerUnit IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_QuantityPerUnit + ']')

--DROP CONSTRAINT FOR '[Products].[ReorderLevel]' if one exists
declare @Product_ReorderLevel varchar(500)
set @Product_ReorderLevel = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'ReorderLevel')
if (@Product_ReorderLevel IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_ReorderLevel + ']')

--DROP CONSTRAINT FOR '[Products].[SupplierID]' if one exists
declare @Product_SupplierID varchar(500)
set @Product_SupplierID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'SupplierID')
if (@Product_SupplierID IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_SupplierID + ']')

--DROP CONSTRAINT FOR '[Products].[UnitPrice]' if one exists
declare @Product_UnitPrice varchar(500)
set @Product_UnitPrice = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'UnitPrice')
if (@Product_UnitPrice IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_UnitPrice + ']')

--DROP CONSTRAINT FOR '[Products].[UnitsInStock]' if one exists
declare @Product_UnitsInStock varchar(500)
set @Product_UnitsInStock = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'UnitsInStock')
if (@Product_UnitsInStock IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_UnitsInStock + ']')

--DROP CONSTRAINT FOR '[Products].[UnitsOnOrder]' if one exists
declare @Product_UnitsOnOrder varchar(500)
set @Product_UnitsOnOrder = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Products' and a.name = 'UnitsOnOrder')
if (@Product_UnitsOnOrder IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Product_UnitsOnOrder + ']')

--END DEFAULTS FOR TABLE [Products]
GO

--BEGIN DEFAULTS FOR TABLE [Region]
--DROP CONSTRAINT FOR '[Region].[RegionDescription]' if one exists
declare @Region_RegionDescription varchar(500)
set @Region_RegionDescription = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Region' and a.name = 'RegionDescription')
if (@Region_RegionDescription IS NOT NULL) exec ('ALTER TABLE [Region] DROP CONSTRAINT [' + @Region_RegionDescription + ']')

--DROP CONSTRAINT FOR '[Region].[RegionID]' if one exists
declare @Region_RegionID varchar(500)
set @Region_RegionID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Region' and a.name = 'RegionID')
if (@Region_RegionID IS NOT NULL) exec ('ALTER TABLE [Region] DROP CONSTRAINT [' + @Region_RegionID + ']')

--END DEFAULTS FOR TABLE [Region]
GO

--BEGIN DEFAULTS FOR TABLE [Shippers]
--DROP CONSTRAINT FOR '[Shippers].[CompanyName]' if one exists
declare @Shipper_CompanyName varchar(500)
set @Shipper_CompanyName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Shippers' and a.name = 'CompanyName')
if (@Shipper_CompanyName IS NOT NULL) exec ('ALTER TABLE [Shippers] DROP CONSTRAINT [' + @Shipper_CompanyName + ']')

--DROP CONSTRAINT FOR '[Shippers].[Phone]' if one exists
declare @Shipper_Phone varchar(500)
set @Shipper_Phone = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Shippers' and a.name = 'Phone')
if (@Shipper_Phone IS NOT NULL) exec ('ALTER TABLE [Shippers] DROP CONSTRAINT [' + @Shipper_Phone + ']')

--DROP CONSTRAINT FOR '[Shippers].[ShipperID]' if one exists
declare @Shipper_ShipperID varchar(500)
set @Shipper_ShipperID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Shippers' and a.name = 'ShipperID')
if (@Shipper_ShipperID IS NOT NULL) exec ('ALTER TABLE [Shippers] DROP CONSTRAINT [' + @Shipper_ShipperID + ']')

--END DEFAULTS FOR TABLE [Shippers]
GO

--BEGIN DEFAULTS FOR TABLE [Suppliers]
--DROP CONSTRAINT FOR '[Suppliers].[Address]' if one exists
declare @Supplier_Address varchar(500)
set @Supplier_Address = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'Address')
if (@Supplier_Address IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_Address + ']')

--DROP CONSTRAINT FOR '[Suppliers].[City]' if one exists
declare @Supplier_City varchar(500)
set @Supplier_City = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'City')
if (@Supplier_City IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_City + ']')

--DROP CONSTRAINT FOR '[Suppliers].[CompanyName]' if one exists
declare @Supplier_CompanyName varchar(500)
set @Supplier_CompanyName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'CompanyName')
if (@Supplier_CompanyName IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_CompanyName + ']')

--DROP CONSTRAINT FOR '[Suppliers].[ContactName]' if one exists
declare @Supplier_ContactName varchar(500)
set @Supplier_ContactName = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'ContactName')
if (@Supplier_ContactName IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_ContactName + ']')

--DROP CONSTRAINT FOR '[Suppliers].[ContactTitle]' if one exists
declare @Supplier_ContactTitle varchar(500)
set @Supplier_ContactTitle = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'ContactTitle')
if (@Supplier_ContactTitle IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_ContactTitle + ']')

--DROP CONSTRAINT FOR '[Suppliers].[Country]' if one exists
declare @Supplier_Country varchar(500)
set @Supplier_Country = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'Country')
if (@Supplier_Country IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_Country + ']')

--DROP CONSTRAINT FOR '[Suppliers].[Fax]' if one exists
declare @Supplier_Fax varchar(500)
set @Supplier_Fax = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'Fax')
if (@Supplier_Fax IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_Fax + ']')

--DROP CONSTRAINT FOR '[Suppliers].[HomePage]' if one exists
declare @Supplier_HomePage varchar(500)
set @Supplier_HomePage = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'HomePage')
if (@Supplier_HomePage IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_HomePage + ']')

--DROP CONSTRAINT FOR '[Suppliers].[Phone]' if one exists
declare @Supplier_Phone varchar(500)
set @Supplier_Phone = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'Phone')
if (@Supplier_Phone IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_Phone + ']')

--DROP CONSTRAINT FOR '[Suppliers].[PostalCode]' if one exists
declare @Supplier_PostalCode varchar(500)
set @Supplier_PostalCode = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'PostalCode')
if (@Supplier_PostalCode IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_PostalCode + ']')

--DROP CONSTRAINT FOR '[Suppliers].[Region]' if one exists
declare @Supplier_Region varchar(500)
set @Supplier_Region = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'Region')
if (@Supplier_Region IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_Region + ']')

--DROP CONSTRAINT FOR '[Suppliers].[SupplierID]' if one exists
declare @Supplier_SupplierID varchar(500)
set @Supplier_SupplierID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Suppliers' and a.name = 'SupplierID')
if (@Supplier_SupplierID IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Supplier_SupplierID + ']')

--END DEFAULTS FOR TABLE [Suppliers]
GO

--BEGIN DEFAULTS FOR TABLE [Territories]
--DROP CONSTRAINT FOR '[Territories].[RegionID]' if one exists
declare @Territory_RegionID varchar(500)
set @Territory_RegionID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Territories' and a.name = 'RegionID')
if (@Territory_RegionID IS NOT NULL) exec ('ALTER TABLE [Territories] DROP CONSTRAINT [' + @Territory_RegionID + ']')

--DROP CONSTRAINT FOR '[Territories].[TerritoryDescription]' if one exists
declare @Territory_TerritoryDescription varchar(500)
set @Territory_TerritoryDescription = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Territories' and a.name = 'TerritoryDescription')
if (@Territory_TerritoryDescription IS NOT NULL) exec ('ALTER TABLE [Territories] DROP CONSTRAINT [' + @Territory_TerritoryDescription + ']')

--DROP CONSTRAINT FOR '[Territories].[TerritoryID]' if one exists
declare @Territory_TerritoryID varchar(500)
set @Territory_TerritoryID = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='Territories' and a.name = 'TerritoryID')
if (@Territory_TerritoryID IS NOT NULL) exec ('ALTER TABLE [Territories] DROP CONSTRAINT [' + @Territory_TerritoryID + ']')

--END DEFAULTS FOR TABLE [Territories]
GO

--##SECTION END [REMOVE DEFAULTS]

--##SECTION BEGIN [CREATE DEFAULTS]

--BEGIN DEFAULTS FOR TABLE [Order Details]
--ADD CONSTRAINT FOR '[Order Details].[Discount]'
if exists(select * from sysobjects where name = 'DF__ORDER DETAILS_DISCOUNT' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [DF__ORDER DETAILS_DISCOUNT]

if not exists(select * from sysobjects where name = 'DF__ORDER DETAILS_DISCOUNT' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] ADD CONSTRAINT [DF__ORDER DETAILS_DISCOUNT] DEFAULT (0) FOR [Discount]

--ADD CONSTRAINT FOR '[Order Details].[Quantity]'
if exists(select * from sysobjects where name = 'DF__ORDER DETAILS_QUANTITY' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [DF__ORDER DETAILS_QUANTITY]

if not exists(select * from sysobjects where name = 'DF__ORDER DETAILS_QUANTITY' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] ADD CONSTRAINT [DF__ORDER DETAILS_QUANTITY] DEFAULT (1) FOR [Quantity]

--ADD CONSTRAINT FOR '[Order Details].[UnitPrice]'
if exists(select * from sysobjects where name = 'DF__ORDER DETAILS_UNITPRICE' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [DF__ORDER DETAILS_UNITPRICE]

if not exists(select * from sysobjects where name = 'DF__ORDER DETAILS_UNITPRICE' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] ADD CONSTRAINT [DF__ORDER DETAILS_UNITPRICE] DEFAULT (0) FOR [UnitPrice]

--END DEFAULTS FOR TABLE [Order Details]
GO

--BEGIN DEFAULTS FOR TABLE [Orders]
--ADD CONSTRAINT FOR '[Orders].[Freight]'
if exists(select * from sysobjects where name = 'DF__ORDERS_FREIGHT' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_FREIGHT]

if not exists(select * from sysobjects where name = 'DF__ORDERS_FREIGHT' and xtype = 'D')
ALTER TABLE [dbo].[Orders] ADD CONSTRAINT [DF__ORDERS_FREIGHT] DEFAULT (0) FOR [Freight]

--END DEFAULTS FOR TABLE [Orders]
GO

--BEGIN DEFAULTS FOR TABLE [Products]
--ADD CONSTRAINT FOR '[Products].[Discontinued]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_DISCONTINUED' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_DISCONTINUED]

if not exists(select * from sysobjects where name = 'DF__PRODUCTS_DISCONTINUED' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_DISCONTINUED] DEFAULT (0) FOR [Discontinued]

--ADD CONSTRAINT FOR '[Products].[ReorderLevel]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_REORDERLEVEL' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_REORDERLEVEL]

if not exists(select * from sysobjects where name = 'DF__PRODUCTS_REORDERLEVEL' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_REORDERLEVEL] DEFAULT (0) FOR [ReorderLevel]

--ADD CONSTRAINT FOR '[Products].[UnitPrice]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITPRICE' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_UNITPRICE]

if not exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITPRICE' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_UNITPRICE] DEFAULT (0) FOR [UnitPrice]

--ADD CONSTRAINT FOR '[Products].[UnitsInStock]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITSINSTOCK' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_UNITSINSTOCK]

if not exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITSINSTOCK' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_UNITSINSTOCK] DEFAULT (0) FOR [UnitsInStock]

--ADD CONSTRAINT FOR '[Products].[UnitsOnOrder]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITSONORDER' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_UNITSONORDER]

if not exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITSONORDER' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_UNITSONORDER] DEFAULT (0) FOR [UnitsOnOrder]

--END DEFAULTS FOR TABLE [Products]
GO

--##SECTION END [CREATE DEFAULTS]

if not exists(select * from sys.objects where [name] = '__nhydrateschema' and [type] = 'U')
BEGIN
CREATE TABLE [__nhydrateschema] (
[dbVersion] [varchar] (50) NOT NULL,
[LastUpdate] [datetime] NOT NULL,
[ModelKey] [uniqueidentifier] NOT NULL,
[History] [nvarchar](max) NOT NULL
)
if not exists(select * from sys.objects where [name] = '__pk__nhydrateschema' and [type] = 'PK')
ALTER TABLE [__nhydrateschema] WITH NOCHECK ADD CONSTRAINT [__pk__nhydrateschema] PRIMARY KEY CLUSTERED ([ModelKey])
END
GO

if not exists(select * from sys.objects where name = '__nhydrateobjects' and [type] = 'U')
CREATE TABLE [dbo].[__nhydrateobjects]
(
	[rowid] [bigint] IDENTITY(1,1) NOT NULL,
	[id] [uniqueidentifier] NULL,
	[name] [varchar](500) NOT NULL,
	[type] [varchar](10) NOT NULL,
	[schema] [varchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Hash] [varchar](32) NULL,
	[ModelKey] [uniqueidentifier] NOT NULL,
)

if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_name')
CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_name] ON [dbo].[__nhydrateobjects]
(
	[name] ASC
)

if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_schema')
CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_schema] ON [dbo].[__nhydrateobjects] 
(
	[schema] ASC
)

if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_type')
CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_type] ON [dbo].[__nhydrateobjects] 
(
	[type] ASC
)

if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_modelkey')
CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_modelkey] ON [dbo].[__nhydrateobjects] 
(
	[ModelKey] ASC
)

if not exists(select * from sys.indexes where name = '__pk__nhydrateobjects')
ALTER TABLE [dbo].[__nhydrateobjects] ADD CONSTRAINT [__pk__nhydrateobjects] PRIMARY KEY CLUSTERED 
(
	[rowid] ASC
)

