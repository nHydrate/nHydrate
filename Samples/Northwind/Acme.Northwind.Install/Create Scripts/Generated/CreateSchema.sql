--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Data Schema For Version 0.0.0.0
--Generated on 2012-02-25 15:25:22

--CREATE TABLE [Categories]
if not exists(select * from sysobjects where name = 'Categories' and xtype = 'U')
CREATE TABLE [dbo].[Categories] (
[CategoryID] [Int] IDENTITY (1, 1) NOT NULL ,
[CategoryName] [NVarChar] (15) NOT NULL ,
[Description] [NText] NULL ,
[Picture] [Image] NULL ,
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__CATEGORIES_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__CATEGORIES_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [CustomerCustomerDemo]
if not exists(select * from sysobjects where name = 'CustomerCustomerDemo' and xtype = 'U')
CREATE TABLE [dbo].[CustomerCustomerDemo] (
[CustomerID] [NChar] (5) NOT NULL ,
[CustomerTypeID] [NChar] (10) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [CustomerDemographics]
if not exists(select * from sysobjects where name = 'CustomerDemographics' and xtype = 'U')
CREATE TABLE [dbo].[CustomerDemographics] (
[CustomerDesc] [NText] NULL ,
[CustomerTypeID] [NChar] (10) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

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
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERS_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERS_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

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
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__EMPLOYEES_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__EMPLOYEES_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [EmployeeTerritories]
if not exists(select * from sysobjects where name = 'EmployeeTerritories' and xtype = 'U')
CREATE TABLE [dbo].[EmployeeTerritories] (
[EmployeeID] [Int] NOT NULL ,
[TerritoryID] [NVarChar] (20) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__EMPLOYEETERRITORIES_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__EMPLOYEETERRITORIES_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [Order Details]
if not exists(select * from sysobjects where name = 'Order Details' and xtype = 'U')
CREATE TABLE [dbo].[Order Details] (
[Discount] [Real] NOT NULL  CONSTRAINT [DF__ORDER DETAILS_DISCOUNT] DEFAULT (0),
[OrderID] [Int] NOT NULL ,
[ProductID] [Int] NOT NULL ,
[Quantity] [SmallInt] NOT NULL  CONSTRAINT [DF__ORDER DETAILS_QUANTITY] DEFAULT (1),
[UnitPrice] [Money] NOT NULL  CONSTRAINT [DF__ORDER DETAILS_UNITPRICE] DEFAULT (0),
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__ORDER DETAILS_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__ORDER DETAILS_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [Orders]
if not exists(select * from sysobjects where name = 'Orders' and xtype = 'U')
CREATE TABLE [dbo].[Orders] (
[CustomerID] [NChar] (5) NULL ,
[EmployeeID] [Int] NULL ,
[Freight] [Money] NULL  CONSTRAINT [DF__ORDERS_FREIGHT] DEFAULT (0),
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
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__ORDERS_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__ORDERS_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [Products]
if not exists(select * from sysobjects where name = 'Products' and xtype = 'U')
CREATE TABLE [dbo].[Products] (
[CategoryID] [Int] NULL ,
[Discontinued] [Bit] NOT NULL  CONSTRAINT [DF__PRODUCTS_DISCONTINUED] DEFAULT (0),
[ProductID] [Int] IDENTITY (1, 1) NOT NULL ,
[ProductName] [NVarChar] (40) NOT NULL ,
[QuantityPerUnit] [NVarChar] (20) NULL ,
[ReorderLevel] [SmallInt] NULL  CONSTRAINT [DF__PRODUCTS_REORDERLEVEL] DEFAULT (0),
[SupplierID] [Int] NULL ,
[UnitPrice] [Money] NULL  CONSTRAINT [DF__PRODUCTS_UNITPRICE] DEFAULT (0),
[UnitsInStock] [SmallInt] NULL  CONSTRAINT [DF__PRODUCTS_UNITSINSTOCK] DEFAULT (0),
[UnitsOnOrder] [SmallInt] NULL  CONSTRAINT [DF__PRODUCTS_UNITSONORDER] DEFAULT (0),
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__PRODUCTS_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__PRODUCTS_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [Region]
if not exists(select * from sysobjects where name = 'Region' and xtype = 'U')
CREATE TABLE [dbo].[Region] (
[RegionDescription] [NChar] (50) NOT NULL ,
[RegionID] [Int] NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__REGION_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__REGION_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [Shippers]
if not exists(select * from sysobjects where name = 'Shippers' and xtype = 'U')
CREATE TABLE [dbo].[Shippers] (
[CompanyName] [NVarChar] (40) NOT NULL ,
[Phone] [NVarChar] (24) NULL ,
[ShipperID] [Int] IDENTITY (1, 1) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__SHIPPERS_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__SHIPPERS_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

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
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__SUPPLIERS_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__SUPPLIERS_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--CREATE TABLE [Territories]
if not exists(select * from sysobjects where name = 'Territories' and xtype = 'U')
CREATE TABLE [dbo].[Territories] (
[RegionID] [Int] NOT NULL ,
[TerritoryDescription] [NChar] (50) NOT NULL ,
[TerritoryID] [NVarChar] (20) NOT NULL ,
[ModifiedBy] [Varchar] (50) NULL ,
[ModifiedDate] [DateTime] CONSTRAINT [DF__TERRITORIES_MODIFIEDDATE] DEFAULT getdate() NULL ,
[CreatedBy] [Varchar] (50) NULL ,
[CreatedDate] [DateTime] CONSTRAINT [DF__TERRITORIES_CREATEDDATE] DEFAULT getdate() NULL ,
[Timestamp] [timestamp] NOT NULL 
) ON [PRIMARY]

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Categories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CATEGORIES_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Categories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CATEGORIES_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Categories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Categories')
ALTER TABLE [dbo].[Categories] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [CustomerCustomerDemo]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [CustomerCustomerDemo]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [CustomerCustomerDemo]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'CustomerCustomerDemo')
ALTER TABLE [dbo].[CustomerCustomerDemo] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [CustomerDemographics]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [CustomerDemographics]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [CustomerDemographics]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'CustomerDemographics')
ALTER TABLE [dbo].[CustomerDemographics] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Customers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__CUSTOMERS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Customers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__CUSTOMERS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Customers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Customers')
ALTER TABLE [dbo].[Customers] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Employees]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__EMPLOYEES_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Employees]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__EMPLOYEES_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Employees]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Employees')
ALTER TABLE [dbo].[Employees] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [EmployeeTerritories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__EMPLOYEETERRITORIES_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [EmployeeTerritories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__EMPLOYEETERRITORIES_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [EmployeeTerritories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'EmployeeTerritories')
ALTER TABLE [dbo].[EmployeeTerritories] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Order Details]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__ORDER DETAILS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Order Details]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__ORDER DETAILS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Order Details]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Order Details')
ALTER TABLE [dbo].[Order Details] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Orders]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__ORDERS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Orders]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__ORDERS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Orders]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Orders')
ALTER TABLE [dbo].[Orders] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Products]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__PRODUCTS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Products]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__PRODUCTS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Products]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Products')
ALTER TABLE [dbo].[Products] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Region]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__REGION_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Region]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__REGION_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Region]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Region')
ALTER TABLE [dbo].[Region] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Shippers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__SHIPPERS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Shippers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__SHIPPERS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Shippers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Shippers')
ALTER TABLE [dbo].[Shippers] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Suppliers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__SUPPLIERS_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Suppliers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__SUPPLIERS_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Suppliers]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Suppliers')
ALTER TABLE [dbo].[Suppliers] ADD [Timestamp] [timestamp] NOT NULL

GO

--APPEND AUDIT TRAIL CREATE FOR TABLE [Territories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedBy' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [CreatedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'CreatedDate' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [CreatedDate] [DateTime] CONSTRAINT [DF__TERRITORIES_CREATEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL MODIFY FOR TABLE [Territories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedBy' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [ModifiedBy] [Varchar] (50) NULL
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'ModifiedDate' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [ModifiedDate] [DateTime] CONSTRAINT [DF__TERRITORIES_MODIFIEDDATE] DEFAULT getdate() NULL

--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [Territories]
if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = 'Timestamp' and o.name = 'Territories')
ALTER TABLE [dbo].[Territories] ADD [Timestamp] [timestamp] NOT NULL

GO

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

--PRIMARY KEY FOR TABLE [Categories]
if not exists(select * from sysobjects where name = 'PK_CATEGORIES' and xtype = 'PK')
ALTER TABLE [dbo].[Categories] WITH NOCHECK ADD 
CONSTRAINT [PK_CATEGORIES] PRIMARY KEY CLUSTERED 
(
	[CategoryID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Categories]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__CATEGORIES'))
ALTER TABLE [dbo].[__AUDIT__Categories] DROP CONSTRAINT [PK___AUDIT__CATEGORIES]
GO

--PRIMARY KEY FOR TABLE [CustomerCustomerDemo]
if not exists(select * from sysobjects where name = 'PK_CUSTOMERCUSTOMERDEMO' and xtype = 'PK')
ALTER TABLE [dbo].[CustomerCustomerDemo] WITH NOCHECK ADD 
CONSTRAINT [PK_CUSTOMERCUSTOMERDEMO] PRIMARY KEY CLUSTERED 
(
	[CustomerID],
	[CustomerTypeID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__CustomerCustomerDemo]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__CUSTOMERCUSTOMERDEMO'))
ALTER TABLE [dbo].[__AUDIT__CustomerCustomerDemo] DROP CONSTRAINT [PK___AUDIT__CUSTOMERCUSTOMERDEMO]
GO

--PRIMARY KEY FOR TABLE [CustomerDemographics]
if not exists(select * from sysobjects where name = 'PK_CUSTOMERDEMOGRAPHICS' and xtype = 'PK')
ALTER TABLE [dbo].[CustomerDemographics] WITH NOCHECK ADD 
CONSTRAINT [PK_CUSTOMERDEMOGRAPHICS] PRIMARY KEY CLUSTERED 
(
	[CustomerTypeID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__CustomerDemographics]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__CUSTOMERDEMOGRAPHICS'))
ALTER TABLE [dbo].[__AUDIT__CustomerDemographics] DROP CONSTRAINT [PK___AUDIT__CUSTOMERDEMOGRAPHICS]
GO

--PRIMARY KEY FOR TABLE [Customers]
if not exists(select * from sysobjects where name = 'PK_CUSTOMERS' and xtype = 'PK')
ALTER TABLE [dbo].[Customers] WITH NOCHECK ADD 
CONSTRAINT [PK_CUSTOMERS] PRIMARY KEY CLUSTERED 
(
	[CustomerID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Customers]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__CUSTOMERS'))
ALTER TABLE [dbo].[__AUDIT__Customers] DROP CONSTRAINT [PK___AUDIT__CUSTOMERS]
GO

--PRIMARY KEY FOR TABLE [Employees]
if not exists(select * from sysobjects where name = 'PK_EMPLOYEES' and xtype = 'PK')
ALTER TABLE [dbo].[Employees] WITH NOCHECK ADD 
CONSTRAINT [PK_EMPLOYEES] PRIMARY KEY CLUSTERED 
(
	[EmployeeID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Employees]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__EMPLOYEES'))
ALTER TABLE [dbo].[__AUDIT__Employees] DROP CONSTRAINT [PK___AUDIT__EMPLOYEES]
GO

--PRIMARY KEY FOR TABLE [EmployeeTerritories]
if not exists(select * from sysobjects where name = 'PK_EMPLOYEETERRITORIES' and xtype = 'PK')
ALTER TABLE [dbo].[EmployeeTerritories] WITH NOCHECK ADD 
CONSTRAINT [PK_EMPLOYEETERRITORIES] PRIMARY KEY CLUSTERED 
(
	[EmployeeID],
	[TerritoryID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__EmployeeTerritories]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__EMPLOYEETERRITORIES'))
ALTER TABLE [dbo].[__AUDIT__EmployeeTerritories] DROP CONSTRAINT [PK___AUDIT__EMPLOYEETERRITORIES]
GO

--PRIMARY KEY FOR TABLE [Order Details]
if not exists(select * from sysobjects where name = 'PK_ORDER DETAILS' and xtype = 'PK')
ALTER TABLE [dbo].[Order Details] WITH NOCHECK ADD 
CONSTRAINT [PK_ORDER DETAILS] PRIMARY KEY CLUSTERED 
(
	[OrderID],
	[ProductID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Order Details]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__ORDER DETAILS'))
ALTER TABLE [dbo].[__AUDIT__Order Details] DROP CONSTRAINT [PK___AUDIT__ORDER DETAILS]
GO

--PRIMARY KEY FOR TABLE [Orders]
if not exists(select * from sysobjects where name = 'PK_ORDERS' and xtype = 'PK')
ALTER TABLE [dbo].[Orders] WITH NOCHECK ADD 
CONSTRAINT [PK_ORDERS] PRIMARY KEY CLUSTERED 
(
	[OrderID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Orders]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__ORDERS'))
ALTER TABLE [dbo].[__AUDIT__Orders] DROP CONSTRAINT [PK___AUDIT__ORDERS]
GO

--PRIMARY KEY FOR TABLE [Products]
if not exists(select * from sysobjects where name = 'PK_PRODUCTS' and xtype = 'PK')
ALTER TABLE [dbo].[Products] WITH NOCHECK ADD 
CONSTRAINT [PK_PRODUCTS] PRIMARY KEY CLUSTERED 
(
	[ProductID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Products]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__PRODUCTS'))
ALTER TABLE [dbo].[__AUDIT__Products] DROP CONSTRAINT [PK___AUDIT__PRODUCTS]
GO

--PRIMARY KEY FOR TABLE [Region]
if not exists(select * from sysobjects where name = 'PK_REGION' and xtype = 'PK')
ALTER TABLE [dbo].[Region] WITH NOCHECK ADD 
CONSTRAINT [PK_REGION] PRIMARY KEY CLUSTERED 
(
	[RegionID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Region]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__REGION'))
ALTER TABLE [dbo].[__AUDIT__Region] DROP CONSTRAINT [PK___AUDIT__REGION]
GO

--PRIMARY KEY FOR TABLE [Shippers]
if not exists(select * from sysobjects where name = 'PK_SHIPPERS' and xtype = 'PK')
ALTER TABLE [dbo].[Shippers] WITH NOCHECK ADD 
CONSTRAINT [PK_SHIPPERS] PRIMARY KEY CLUSTERED 
(
	[ShipperID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Shippers]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__SHIPPERS'))
ALTER TABLE [dbo].[__AUDIT__Shippers] DROP CONSTRAINT [PK___AUDIT__SHIPPERS]
GO

--PRIMARY KEY FOR TABLE [Suppliers]
if not exists(select * from sysobjects where name = 'PK_SUPPLIERS' and xtype = 'PK')
ALTER TABLE [dbo].[Suppliers] WITH NOCHECK ADD 
CONSTRAINT [PK_SUPPLIERS] PRIMARY KEY CLUSTERED 
(
	[SupplierID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Suppliers]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__SUPPLIERS'))
ALTER TABLE [dbo].[__AUDIT__Suppliers] DROP CONSTRAINT [PK___AUDIT__SUPPLIERS]
GO

--PRIMARY KEY FOR TABLE [Territories]
if not exists(select * from sysobjects where name = 'PK_TERRITORIES' and xtype = 'PK')
ALTER TABLE [dbo].[Territories] WITH NOCHECK ADD 
CONSTRAINT [PK_TERRITORIES] PRIMARY KEY CLUSTERED 
(
	[TerritoryID]
) ON [PRIMARY]
GO

--DROP PRIMARY KEY FOR TABLE [__AUDIT__Territories]
if exists (select * from dbo.sysobjects where id = object_id(N'PK___AUDIT__TERRITORIES'))
ALTER TABLE [dbo].[__AUDIT__Territories] DROP CONSTRAINT [PK___AUDIT__TERRITORIES]
GO

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
if not exists(select * from sysobjects where name = 'FK_ORDER_DETAILS_ORDERS_ORDER DETAILS_ORDERS' and xtype = 'F')
ALTER TABLE [dbo].[Order Details] ADD 
CONSTRAINT [FK_ORDER_DETAILS_ORDERS_ORDER DETAILS_ORDERS] FOREIGN KEY 
(
	[OrderID]
) REFERENCES [dbo].[Orders] (
	[OrderID]
)
GO

--FOREIGN KEY RELATIONSHIP [Products] -> [Order Details] ([Products].[ProductID] -> [Order Details].[ProductID])
if not exists(select * from sysobjects where name = 'FK_ORDER_DETAILS_PRODUCTS_ORDER DETAILS_PRODUCTS' and xtype = 'F')
ALTER TABLE [dbo].[Order Details] ADD 
CONSTRAINT [FK_ORDER_DETAILS_PRODUCTS_ORDER DETAILS_PRODUCTS] FOREIGN KEY 
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

--INDEX FOR TABLE [Categories] COLUMN [CategoryName]
if not exists(select * from sys.indexes where name = 'IDX_CATEGORIES_CATEGORYNAME')
CREATE INDEX [IDX_CATEGORIES_CATEGORYNAME] ON [dbo].[Categories]([CategoryName]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Customers] COLUMN [City]
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_CITY')
CREATE INDEX [IDX_CUSTOMERS_CITY] ON [dbo].[Customers]([City]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Customers] COLUMN [CompanyName]
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_COMPANYNAME')
CREATE INDEX [IDX_CUSTOMERS_COMPANYNAME] ON [dbo].[Customers]([CompanyName]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Customers] COLUMN [PostalCode]
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_POSTALCODE')
CREATE INDEX [IDX_CUSTOMERS_POSTALCODE] ON [dbo].[Customers]([PostalCode]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Customers] COLUMN [Region]
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_REGION')
CREATE INDEX [IDX_CUSTOMERS_REGION] ON [dbo].[Customers]([Region]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Employees] COLUMN [LastName]
if not exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_LASTNAME')
CREATE INDEX [IDX_EMPLOYEES_LASTNAME] ON [dbo].[Employees]([LastName]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Employees] COLUMN [PostalCode]
if not exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_POSTALCODE')
CREATE INDEX [IDX_EMPLOYEES_POSTALCODE] ON [dbo].[Employees]([PostalCode]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Orders] COLUMN [CustomerID]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_CUSTOMERID')
CREATE INDEX [IDX_ORDERS_CUSTOMERID] ON [dbo].[Orders]([CustomerID]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Orders] COLUMN [EmployeeID]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_EMPLOYEEID')
CREATE INDEX [IDX_ORDERS_EMPLOYEEID] ON [dbo].[Orders]([EmployeeID]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Orders] COLUMN [OrderDate]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_ORDERDATE')
CREATE INDEX [IDX_ORDERS_ORDERDATE] ON [dbo].[Orders]([OrderDate]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Orders] COLUMN [ShippedDate]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPEDDATE')
CREATE INDEX [IDX_ORDERS_SHIPPEDDATE] ON [dbo].[Orders]([ShippedDate]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Orders] COLUMN [ShipPostalCode]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPOSTALCODE')
CREATE INDEX [IDX_ORDERS_SHIPPOSTALCODE] ON [dbo].[Orders]([ShipPostalCode]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Orders] COLUMN [ShipVia]
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPVIA')
CREATE INDEX [IDX_ORDERS_SHIPVIA] ON [dbo].[Orders]([ShipVia]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Products] COLUMN [CategoryID]
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_CATEGORYID')
CREATE INDEX [IDX_PRODUCTS_CATEGORYID] ON [dbo].[Products]([CategoryID]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Products] COLUMN [ProductName]
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_PRODUCTNAME')
CREATE INDEX [IDX_PRODUCTS_PRODUCTNAME] ON [dbo].[Products]([ProductName]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Products] COLUMN [SupplierID]
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_SUPPLIERID')
CREATE INDEX [IDX_PRODUCTS_SUPPLIERID] ON [dbo].[Products]([SupplierID]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Suppliers] COLUMN [CompanyName]
if not exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_COMPANYNAME')
CREATE INDEX [IDX_SUPPLIERS_COMPANYNAME] ON [dbo].[Suppliers]([CompanyName]) ON [PRIMARY]
GO

--INDEX FOR TABLE [Suppliers] COLUMN [PostalCode]
if not exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_POSTALCODE')
CREATE INDEX [IDX_SUPPLIERS_POSTALCODE] ON [dbo].[Suppliers]([PostalCode]) ON [PRIMARY]
GO

--BEGIN DEFAULTS FOR TABLE [Categories]
--DROP UNKNOWN CONSTRAINT FOR '[Categories].[CategoryID]' if one exists
declare @CategoryID varchar(500)
set @CategoryID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Categories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Categories') and name = 'CategoryID'))
if (@CategoryID IS NOT NULL) exec ('ALTER TABLE [Categories] DROP CONSTRAINT [' + @CategoryID + ']')
--DROP KNOWN CONSTRAINT FOR '[Categories].[CategoryID]'
if exists(select * from sysobjects where name = 'DF__CATEGORIES_CATEGORYID' and xtype = 'D')
ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [DF__CATEGORIES_CATEGORYID] 
--DROP UNKNOWN CONSTRAINT FOR '[Categories].[CategoryName]' if one exists
declare @CategoryName varchar(500)
set @CategoryName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Categories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Categories') and name = 'CategoryName'))
if (@CategoryName IS NOT NULL) exec ('ALTER TABLE [Categories] DROP CONSTRAINT [' + @CategoryName + ']')
--DROP KNOWN CONSTRAINT FOR '[Categories].[CategoryName]'
if exists(select * from sysobjects where name = 'DF__CATEGORIES_CATEGORYNAME' and xtype = 'D')
ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [DF__CATEGORIES_CATEGORYNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Categories].[Description]' if one exists
declare @Description varchar(500)
set @Description = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Categories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Categories') and name = 'Description'))
if (@Description IS NOT NULL) exec ('ALTER TABLE [Categories] DROP CONSTRAINT [' + @Description + ']')
--DROP KNOWN CONSTRAINT FOR '[Categories].[Description]'
if exists(select * from sysobjects where name = 'DF__CATEGORIES_DESCRIPTION' and xtype = 'D')
ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [DF__CATEGORIES_DESCRIPTION] 
--DROP UNKNOWN CONSTRAINT FOR '[Categories].[Picture]' if one exists
declare @Picture varchar(500)
set @Picture = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Categories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Categories') and name = 'Picture'))
if (@Picture IS NOT NULL) exec ('ALTER TABLE [Categories] DROP CONSTRAINT [' + @Picture + ']')
--DROP KNOWN CONSTRAINT FOR '[Categories].[Picture]'
if exists(select * from sysobjects where name = 'DF__CATEGORIES_PICTURE' and xtype = 'D')
ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [DF__CATEGORIES_PICTURE] 
--END DEFAULTS FOR TABLE [Categories]
GO

--BEGIN DEFAULTS FOR TABLE [CustomerCustomerDemo]
--DROP UNKNOWN CONSTRAINT FOR '[CustomerCustomerDemo].[CustomerID]' if one exists
declare @CustomerID varchar(500)
set @CustomerID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'CustomerCustomerDemo') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'CustomerCustomerDemo') and name = 'CustomerID'))
if (@CustomerID IS NOT NULL) exec ('ALTER TABLE [CustomerCustomerDemo] DROP CONSTRAINT [' + @CustomerID + ']')
--DROP KNOWN CONSTRAINT FOR '[CustomerCustomerDemo].[CustomerID]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERCUSTOMERDEMO_CUSTOMERID' and xtype = 'D')
ALTER TABLE [dbo].[CustomerCustomerDemo] DROP CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_CUSTOMERID] 
--DROP UNKNOWN CONSTRAINT FOR '[CustomerCustomerDemo].[CustomerTypeID]' if one exists
declare @CustomerTypeID varchar(500)
set @CustomerTypeID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'CustomerCustomerDemo') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'CustomerCustomerDemo') and name = 'CustomerTypeID'))
if (@CustomerTypeID IS NOT NULL) exec ('ALTER TABLE [CustomerCustomerDemo] DROP CONSTRAINT [' + @CustomerTypeID + ']')
--DROP KNOWN CONSTRAINT FOR '[CustomerCustomerDemo].[CustomerTypeID]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERCUSTOMERDEMO_CUSTOMERTYPEID' and xtype = 'D')
ALTER TABLE [dbo].[CustomerCustomerDemo] DROP CONSTRAINT [DF__CUSTOMERCUSTOMERDEMO_CUSTOMERTYPEID] 
--END DEFAULTS FOR TABLE [CustomerCustomerDemo]
GO

--BEGIN DEFAULTS FOR TABLE [CustomerDemographics]
--DROP UNKNOWN CONSTRAINT FOR '[CustomerDemographics].[CustomerDesc]' if one exists
declare @CustomerDesc varchar(500)
set @CustomerDesc = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'CustomerDemographics') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'CustomerDemographics') and name = 'CustomerDesc'))
if (@CustomerDesc IS NOT NULL) exec ('ALTER TABLE [CustomerDemographics] DROP CONSTRAINT [' + @CustomerDesc + ']')
--DROP KNOWN CONSTRAINT FOR '[CustomerDemographics].[CustomerDesc]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERDEMOGRAPHICS_CUSTOMERDESC' and xtype = 'D')
ALTER TABLE [dbo].[CustomerDemographics] DROP CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_CUSTOMERDESC] 
--DROP UNKNOWN CONSTRAINT FOR '[CustomerDemographics].[CustomerTypeID]' if one exists
declare @CustomerTypeID varchar(500)
set @CustomerTypeID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'CustomerDemographics') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'CustomerDemographics') and name = 'CustomerTypeID'))
if (@CustomerTypeID IS NOT NULL) exec ('ALTER TABLE [CustomerDemographics] DROP CONSTRAINT [' + @CustomerTypeID + ']')
--DROP KNOWN CONSTRAINT FOR '[CustomerDemographics].[CustomerTypeID]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERDEMOGRAPHICS_CUSTOMERTYPEID' and xtype = 'D')
ALTER TABLE [dbo].[CustomerDemographics] DROP CONSTRAINT [DF__CUSTOMERDEMOGRAPHICS_CUSTOMERTYPEID] 
--END DEFAULTS FOR TABLE [CustomerDemographics]
GO

--BEGIN DEFAULTS FOR TABLE [Customers]
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[Address]' if one exists
declare @Address varchar(500)
set @Address = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'Address'))
if (@Address IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Address + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[Address]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_ADDRESS' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_ADDRESS] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[City]' if one exists
declare @City varchar(500)
set @City = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'City'))
if (@City IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @City + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[City]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_CITY' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_CITY] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[CompanyName]' if one exists
declare @CompanyName varchar(500)
set @CompanyName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'CompanyName'))
if (@CompanyName IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @CompanyName + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[CompanyName]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_COMPANYNAME' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_COMPANYNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[ContactName]' if one exists
declare @ContactName varchar(500)
set @ContactName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'ContactName'))
if (@ContactName IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @ContactName + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[ContactName]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_CONTACTNAME' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_CONTACTNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[ContactTitle]' if one exists
declare @ContactTitle varchar(500)
set @ContactTitle = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'ContactTitle'))
if (@ContactTitle IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @ContactTitle + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[ContactTitle]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_CONTACTTITLE' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_CONTACTTITLE] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[Country]' if one exists
declare @Country varchar(500)
set @Country = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'Country'))
if (@Country IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Country + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[Country]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_COUNTRY' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_COUNTRY] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[CustomerID]' if one exists
declare @CustomerID varchar(500)
set @CustomerID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'CustomerID'))
if (@CustomerID IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @CustomerID + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[CustomerID]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_CUSTOMERID' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_CUSTOMERID] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[Fax]' if one exists
declare @Fax varchar(500)
set @Fax = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'Fax'))
if (@Fax IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Fax + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[Fax]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_FAX' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_FAX] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[Phone]' if one exists
declare @Phone varchar(500)
set @Phone = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'Phone'))
if (@Phone IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Phone + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[Phone]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_PHONE' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_PHONE] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[PostalCode]' if one exists
declare @PostalCode varchar(500)
set @PostalCode = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'PostalCode'))
if (@PostalCode IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @PostalCode + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[PostalCode]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_POSTALCODE' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_POSTALCODE] 
--DROP UNKNOWN CONSTRAINT FOR '[Customers].[Region]' if one exists
declare @Region varchar(500)
set @Region = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Customers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Customers') and name = 'Region'))
if (@Region IS NOT NULL) exec ('ALTER TABLE [Customers] DROP CONSTRAINT [' + @Region + ']')
--DROP KNOWN CONSTRAINT FOR '[Customers].[Region]'
if exists(select * from sysobjects where name = 'DF__CUSTOMERS_REGION' and xtype = 'D')
ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [DF__CUSTOMERS_REGION] 
--END DEFAULTS FOR TABLE [Customers]
GO

--BEGIN DEFAULTS FOR TABLE [Employees]
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[Address]' if one exists
declare @Address varchar(500)
set @Address = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'Address'))
if (@Address IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Address + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[Address]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_ADDRESS' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_ADDRESS] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[BirthDate]' if one exists
declare @BirthDate varchar(500)
set @BirthDate = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'BirthDate'))
if (@BirthDate IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @BirthDate + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[BirthDate]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_BIRTHDATE' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_BIRTHDATE] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[City]' if one exists
declare @City varchar(500)
set @City = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'City'))
if (@City IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @City + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[City]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_CITY' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_CITY] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[Country]' if one exists
declare @Country varchar(500)
set @Country = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'Country'))
if (@Country IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Country + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[Country]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_COUNTRY' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_COUNTRY] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[EmployeeID]' if one exists
declare @EmployeeID varchar(500)
set @EmployeeID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'EmployeeID'))
if (@EmployeeID IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @EmployeeID + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[EmployeeID]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_EMPLOYEEID' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_EMPLOYEEID] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[Extension]' if one exists
declare @Extension varchar(500)
set @Extension = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'Extension'))
if (@Extension IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Extension + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[Extension]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_EXTENSION' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_EXTENSION] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[FirstName]' if one exists
declare @FirstName varchar(500)
set @FirstName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'FirstName'))
if (@FirstName IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @FirstName + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[FirstName]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_FIRSTNAME' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_FIRSTNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[HireDate]' if one exists
declare @HireDate varchar(500)
set @HireDate = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'HireDate'))
if (@HireDate IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @HireDate + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[HireDate]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_HIREDATE' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_HIREDATE] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[HomePhone]' if one exists
declare @HomePhone varchar(500)
set @HomePhone = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'HomePhone'))
if (@HomePhone IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @HomePhone + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[HomePhone]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_HOMEPHONE' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_HOMEPHONE] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[LastName]' if one exists
declare @LastName varchar(500)
set @LastName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'LastName'))
if (@LastName IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @LastName + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[LastName]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_LASTNAME' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_LASTNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[Notes]' if one exists
declare @Notes varchar(500)
set @Notes = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'Notes'))
if (@Notes IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Notes + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[Notes]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_NOTES' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_NOTES] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[Photo]' if one exists
declare @Photo varchar(500)
set @Photo = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'Photo'))
if (@Photo IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Photo + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[Photo]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_PHOTO' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_PHOTO] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[PhotoPath]' if one exists
declare @PhotoPath varchar(500)
set @PhotoPath = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'PhotoPath'))
if (@PhotoPath IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @PhotoPath + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[PhotoPath]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_PHOTOPATH' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_PHOTOPATH] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[PostalCode]' if one exists
declare @PostalCode varchar(500)
set @PostalCode = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'PostalCode'))
if (@PostalCode IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @PostalCode + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[PostalCode]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_POSTALCODE' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_POSTALCODE] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[Region]' if one exists
declare @Region varchar(500)
set @Region = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'Region'))
if (@Region IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Region + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[Region]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_REGION' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_REGION] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[ReportsTo]' if one exists
declare @ReportsTo varchar(500)
set @ReportsTo = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'ReportsTo'))
if (@ReportsTo IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @ReportsTo + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[ReportsTo]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_REPORTSTO' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_REPORTSTO] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[Title]' if one exists
declare @Title varchar(500)
set @Title = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'Title'))
if (@Title IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @Title + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[Title]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_TITLE' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_TITLE] 
--DROP UNKNOWN CONSTRAINT FOR '[Employees].[TitleOfCourtesy]' if one exists
declare @TitleOfCourtesy varchar(500)
set @TitleOfCourtesy = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Employees') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Employees') and name = 'TitleOfCourtesy'))
if (@TitleOfCourtesy IS NOT NULL) exec ('ALTER TABLE [Employees] DROP CONSTRAINT [' + @TitleOfCourtesy + ']')
--DROP KNOWN CONSTRAINT FOR '[Employees].[TitleOfCourtesy]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEES_TITLEOFCOURTESY' and xtype = 'D')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [DF__EMPLOYEES_TITLEOFCOURTESY] 
--END DEFAULTS FOR TABLE [Employees]
GO

--BEGIN DEFAULTS FOR TABLE [EmployeeTerritories]
--DROP UNKNOWN CONSTRAINT FOR '[EmployeeTerritories].[EmployeeID]' if one exists
declare @EmployeeID varchar(500)
set @EmployeeID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'EmployeeTerritories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'EmployeeTerritories') and name = 'EmployeeID'))
if (@EmployeeID IS NOT NULL) exec ('ALTER TABLE [EmployeeTerritories] DROP CONSTRAINT [' + @EmployeeID + ']')
--DROP KNOWN CONSTRAINT FOR '[EmployeeTerritories].[EmployeeID]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEETERRITORIES_EMPLOYEEID' and xtype = 'D')
ALTER TABLE [dbo].[EmployeeTerritories] DROP CONSTRAINT [DF__EMPLOYEETERRITORIES_EMPLOYEEID] 
--DROP UNKNOWN CONSTRAINT FOR '[EmployeeTerritories].[TerritoryID]' if one exists
declare @TerritoryID varchar(500)
set @TerritoryID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'EmployeeTerritories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'EmployeeTerritories') and name = 'TerritoryID'))
if (@TerritoryID IS NOT NULL) exec ('ALTER TABLE [EmployeeTerritories] DROP CONSTRAINT [' + @TerritoryID + ']')
--DROP KNOWN CONSTRAINT FOR '[EmployeeTerritories].[TerritoryID]'
if exists(select * from sysobjects where name = 'DF__EMPLOYEETERRITORIES_TERRITORYID' and xtype = 'D')
ALTER TABLE [dbo].[EmployeeTerritories] DROP CONSTRAINT [DF__EMPLOYEETERRITORIES_TERRITORYID] 
--END DEFAULTS FOR TABLE [EmployeeTerritories]
GO

--BEGIN DEFAULTS FOR TABLE [Order Details]
--DROP UNKNOWN CONSTRAINT FOR '[Order Details].[Discount]' if one exists
declare @Discount varchar(500)
set @Discount = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and name = 'Discount'))
if (@Discount IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @Discount + ']')
--DROP KNOWN CONSTRAINT FOR '[Order Details].[Discount]'
if exists(select * from sysobjects where name = 'DF__ORDER DETAILS_DISCOUNT' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [DF__ORDER DETAILS_DISCOUNT] 
--ADD KNOWN CONSTRAINT FOR '[Order Details].[Discount]'
if not exists(select * from sysobjects where name = 'DF__ORDER DETAILS_DISCOUNT' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] ADD  CONSTRAINT [DF__ORDER DETAILS_DISCOUNT] DEFAULT (0) FOR [Discount]
--DROP UNKNOWN CONSTRAINT FOR '[Order Details].[OrderID]' if one exists
declare @OrderID varchar(500)
set @OrderID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and name = 'OrderID'))
if (@OrderID IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @OrderID + ']')
--DROP KNOWN CONSTRAINT FOR '[Order Details].[OrderID]'
if exists(select * from sysobjects where name = 'DF__ORDER DETAILS_ORDERID' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [DF__ORDER DETAILS_ORDERID] 
--DROP UNKNOWN CONSTRAINT FOR '[Order Details].[ProductID]' if one exists
declare @ProductID varchar(500)
set @ProductID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and name = 'ProductID'))
if (@ProductID IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @ProductID + ']')
--DROP KNOWN CONSTRAINT FOR '[Order Details].[ProductID]'
if exists(select * from sysobjects where name = 'DF__ORDER DETAILS_PRODUCTID' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [DF__ORDER DETAILS_PRODUCTID] 
--DROP UNKNOWN CONSTRAINT FOR '[Order Details].[Quantity]' if one exists
declare @Quantity varchar(500)
set @Quantity = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and name = 'Quantity'))
if (@Quantity IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @Quantity + ']')
--DROP KNOWN CONSTRAINT FOR '[Order Details].[Quantity]'
if exists(select * from sysobjects where name = 'DF__ORDER DETAILS_QUANTITY' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [DF__ORDER DETAILS_QUANTITY] 
--ADD KNOWN CONSTRAINT FOR '[Order Details].[Quantity]'
if not exists(select * from sysobjects where name = 'DF__ORDER DETAILS_QUANTITY' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] ADD  CONSTRAINT [DF__ORDER DETAILS_QUANTITY] DEFAULT (1) FOR [Quantity]
--DROP UNKNOWN CONSTRAINT FOR '[Order Details].[UnitPrice]' if one exists
declare @UnitPrice varchar(500)
set @UnitPrice = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Order Details') and name = 'UnitPrice'))
if (@UnitPrice IS NOT NULL) exec ('ALTER TABLE [Order Details] DROP CONSTRAINT [' + @UnitPrice + ']')
--DROP KNOWN CONSTRAINT FOR '[Order Details].[UnitPrice]'
if exists(select * from sysobjects where name = 'DF__ORDER DETAILS_UNITPRICE' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [DF__ORDER DETAILS_UNITPRICE] 
--ADD KNOWN CONSTRAINT FOR '[Order Details].[UnitPrice]'
if not exists(select * from sysobjects where name = 'DF__ORDER DETAILS_UNITPRICE' and xtype = 'D')
ALTER TABLE [dbo].[Order Details] ADD  CONSTRAINT [DF__ORDER DETAILS_UNITPRICE] DEFAULT (0) FOR [UnitPrice]
--END DEFAULTS FOR TABLE [Order Details]
GO

--BEGIN DEFAULTS FOR TABLE [Orders]
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[CustomerID]' if one exists
declare @CustomerID varchar(500)
set @CustomerID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'CustomerID'))
if (@CustomerID IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @CustomerID + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[CustomerID]'
if exists(select * from sysobjects where name = 'DF__ORDERS_CUSTOMERID' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_CUSTOMERID] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[EmployeeID]' if one exists
declare @EmployeeID varchar(500)
set @EmployeeID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'EmployeeID'))
if (@EmployeeID IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @EmployeeID + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[EmployeeID]'
if exists(select * from sysobjects where name = 'DF__ORDERS_EMPLOYEEID' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_EMPLOYEEID] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[Freight]' if one exists
declare @Freight varchar(500)
set @Freight = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'Freight'))
if (@Freight IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @Freight + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[Freight]'
if exists(select * from sysobjects where name = 'DF__ORDERS_FREIGHT' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_FREIGHT] 
--ADD KNOWN CONSTRAINT FOR '[Orders].[Freight]'
if not exists(select * from sysobjects where name = 'DF__ORDERS_FREIGHT' and xtype = 'D')
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF__ORDERS_FREIGHT] DEFAULT (0) FOR [Freight]
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[OrderDate]' if one exists
declare @OrderDate varchar(500)
set @OrderDate = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'OrderDate'))
if (@OrderDate IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @OrderDate + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[OrderDate]'
if exists(select * from sysobjects where name = 'DF__ORDERS_ORDERDATE' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_ORDERDATE] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[OrderID]' if one exists
declare @OrderID varchar(500)
set @OrderID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'OrderID'))
if (@OrderID IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @OrderID + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[OrderID]'
if exists(select * from sysobjects where name = 'DF__ORDERS_ORDERID' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_ORDERID] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[RequiredDate]' if one exists
declare @RequiredDate varchar(500)
set @RequiredDate = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'RequiredDate'))
if (@RequiredDate IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @RequiredDate + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[RequiredDate]'
if exists(select * from sysobjects where name = 'DF__ORDERS_REQUIREDDATE' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_REQUIREDDATE] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[ShipAddress]' if one exists
declare @ShipAddress varchar(500)
set @ShipAddress = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'ShipAddress'))
if (@ShipAddress IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @ShipAddress + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[ShipAddress]'
if exists(select * from sysobjects where name = 'DF__ORDERS_SHIPADDRESS' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_SHIPADDRESS] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[ShipCity]' if one exists
declare @ShipCity varchar(500)
set @ShipCity = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'ShipCity'))
if (@ShipCity IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @ShipCity + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[ShipCity]'
if exists(select * from sysobjects where name = 'DF__ORDERS_SHIPCITY' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_SHIPCITY] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[ShipCountry]' if one exists
declare @ShipCountry varchar(500)
set @ShipCountry = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'ShipCountry'))
if (@ShipCountry IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @ShipCountry + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[ShipCountry]'
if exists(select * from sysobjects where name = 'DF__ORDERS_SHIPCOUNTRY' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_SHIPCOUNTRY] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[ShipName]' if one exists
declare @ShipName varchar(500)
set @ShipName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'ShipName'))
if (@ShipName IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @ShipName + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[ShipName]'
if exists(select * from sysobjects where name = 'DF__ORDERS_SHIPNAME' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_SHIPNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[ShippedDate]' if one exists
declare @ShippedDate varchar(500)
set @ShippedDate = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'ShippedDate'))
if (@ShippedDate IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @ShippedDate + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[ShippedDate]'
if exists(select * from sysobjects where name = 'DF__ORDERS_SHIPPEDDATE' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_SHIPPEDDATE] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[ShipPostalCode]' if one exists
declare @ShipPostalCode varchar(500)
set @ShipPostalCode = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'ShipPostalCode'))
if (@ShipPostalCode IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @ShipPostalCode + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[ShipPostalCode]'
if exists(select * from sysobjects where name = 'DF__ORDERS_SHIPPOSTALCODE' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_SHIPPOSTALCODE] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[ShipRegion]' if one exists
declare @ShipRegion varchar(500)
set @ShipRegion = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'ShipRegion'))
if (@ShipRegion IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @ShipRegion + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[ShipRegion]'
if exists(select * from sysobjects where name = 'DF__ORDERS_SHIPREGION' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_SHIPREGION] 
--DROP UNKNOWN CONSTRAINT FOR '[Orders].[ShipVia]' if one exists
declare @ShipVia varchar(500)
set @ShipVia = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Orders') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Orders') and name = 'ShipVia'))
if (@ShipVia IS NOT NULL) exec ('ALTER TABLE [Orders] DROP CONSTRAINT [' + @ShipVia + ']')
--DROP KNOWN CONSTRAINT FOR '[Orders].[ShipVia]'
if exists(select * from sysobjects where name = 'DF__ORDERS_SHIPVIA' and xtype = 'D')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF__ORDERS_SHIPVIA] 
--END DEFAULTS FOR TABLE [Orders]
GO

--BEGIN DEFAULTS FOR TABLE [Products]
--DROP UNKNOWN CONSTRAINT FOR '[Products].[CategoryID]' if one exists
declare @CategoryID varchar(500)
set @CategoryID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'CategoryID'))
if (@CategoryID IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @CategoryID + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[CategoryID]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_CATEGORYID' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_CATEGORYID] 
--DROP UNKNOWN CONSTRAINT FOR '[Products].[Discontinued]' if one exists
declare @Discontinued varchar(500)
set @Discontinued = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'Discontinued'))
if (@Discontinued IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @Discontinued + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[Discontinued]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_DISCONTINUED' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_DISCONTINUED] 
--ADD KNOWN CONSTRAINT FOR '[Products].[Discontinued]'
if not exists(select * from sysobjects where name = 'DF__PRODUCTS_DISCONTINUED' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__PRODUCTS_DISCONTINUED] DEFAULT (0) FOR [Discontinued]
--DROP UNKNOWN CONSTRAINT FOR '[Products].[ProductID]' if one exists
declare @ProductID varchar(500)
set @ProductID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'ProductID'))
if (@ProductID IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @ProductID + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[ProductID]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_PRODUCTID' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_PRODUCTID] 
--DROP UNKNOWN CONSTRAINT FOR '[Products].[ProductName]' if one exists
declare @ProductName varchar(500)
set @ProductName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'ProductName'))
if (@ProductName IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @ProductName + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[ProductName]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_PRODUCTNAME' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_PRODUCTNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Products].[QuantityPerUnit]' if one exists
declare @QuantityPerUnit varchar(500)
set @QuantityPerUnit = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'QuantityPerUnit'))
if (@QuantityPerUnit IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @QuantityPerUnit + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[QuantityPerUnit]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_QUANTITYPERUNIT' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_QUANTITYPERUNIT] 
--DROP UNKNOWN CONSTRAINT FOR '[Products].[ReorderLevel]' if one exists
declare @ReorderLevel varchar(500)
set @ReorderLevel = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'ReorderLevel'))
if (@ReorderLevel IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @ReorderLevel + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[ReorderLevel]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_REORDERLEVEL' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_REORDERLEVEL] 
--ADD KNOWN CONSTRAINT FOR '[Products].[ReorderLevel]'
if not exists(select * from sysobjects where name = 'DF__PRODUCTS_REORDERLEVEL' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__PRODUCTS_REORDERLEVEL] DEFAULT (0) FOR [ReorderLevel]
--DROP UNKNOWN CONSTRAINT FOR '[Products].[SupplierID]' if one exists
declare @SupplierID varchar(500)
set @SupplierID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'SupplierID'))
if (@SupplierID IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @SupplierID + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[SupplierID]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_SUPPLIERID' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_SUPPLIERID] 
--DROP UNKNOWN CONSTRAINT FOR '[Products].[UnitPrice]' if one exists
declare @UnitPrice varchar(500)
set @UnitPrice = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'UnitPrice'))
if (@UnitPrice IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @UnitPrice + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[UnitPrice]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITPRICE' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_UNITPRICE] 
--ADD KNOWN CONSTRAINT FOR '[Products].[UnitPrice]'
if not exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITPRICE' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__PRODUCTS_UNITPRICE] DEFAULT (0) FOR [UnitPrice]
--DROP UNKNOWN CONSTRAINT FOR '[Products].[UnitsInStock]' if one exists
declare @UnitsInStock varchar(500)
set @UnitsInStock = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'UnitsInStock'))
if (@UnitsInStock IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @UnitsInStock + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[UnitsInStock]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITSINSTOCK' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_UNITSINSTOCK] 
--ADD KNOWN CONSTRAINT FOR '[Products].[UnitsInStock]'
if not exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITSINSTOCK' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__PRODUCTS_UNITSINSTOCK] DEFAULT (0) FOR [UnitsInStock]
--DROP UNKNOWN CONSTRAINT FOR '[Products].[UnitsOnOrder]' if one exists
declare @UnitsOnOrder varchar(500)
set @UnitsOnOrder = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Products') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Products') and name = 'UnitsOnOrder'))
if (@UnitsOnOrder IS NOT NULL) exec ('ALTER TABLE [Products] DROP CONSTRAINT [' + @UnitsOnOrder + ']')
--DROP KNOWN CONSTRAINT FOR '[Products].[UnitsOnOrder]'
if exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITSONORDER' and xtype = 'D')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__PRODUCTS_UNITSONORDER] 
--ADD KNOWN CONSTRAINT FOR '[Products].[UnitsOnOrder]'
if not exists(select * from sysobjects where name = 'DF__PRODUCTS_UNITSONORDER' and xtype = 'D')
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__PRODUCTS_UNITSONORDER] DEFAULT (0) FOR [UnitsOnOrder]
--END DEFAULTS FOR TABLE [Products]
GO

--BEGIN DEFAULTS FOR TABLE [Region]
--DROP UNKNOWN CONSTRAINT FOR '[Region].[RegionDescription]' if one exists
declare @RegionDescription varchar(500)
set @RegionDescription = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Region') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Region') and name = 'RegionDescription'))
if (@RegionDescription IS NOT NULL) exec ('ALTER TABLE [Region] DROP CONSTRAINT [' + @RegionDescription + ']')
--DROP KNOWN CONSTRAINT FOR '[Region].[RegionDescription]'
if exists(select * from sysobjects where name = 'DF__REGION_REGIONDESCRIPTION' and xtype = 'D')
ALTER TABLE [dbo].[Region] DROP CONSTRAINT [DF__REGION_REGIONDESCRIPTION] 
--DROP UNKNOWN CONSTRAINT FOR '[Region].[RegionID]' if one exists
declare @RegionID varchar(500)
set @RegionID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Region') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Region') and name = 'RegionID'))
if (@RegionID IS NOT NULL) exec ('ALTER TABLE [Region] DROP CONSTRAINT [' + @RegionID + ']')
--DROP KNOWN CONSTRAINT FOR '[Region].[RegionID]'
if exists(select * from sysobjects where name = 'DF__REGION_REGIONID' and xtype = 'D')
ALTER TABLE [dbo].[Region] DROP CONSTRAINT [DF__REGION_REGIONID] 
--END DEFAULTS FOR TABLE [Region]
GO

--BEGIN DEFAULTS FOR TABLE [Shippers]
--DROP UNKNOWN CONSTRAINT FOR '[Shippers].[CompanyName]' if one exists
declare @CompanyName varchar(500)
set @CompanyName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Shippers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Shippers') and name = 'CompanyName'))
if (@CompanyName IS NOT NULL) exec ('ALTER TABLE [Shippers] DROP CONSTRAINT [' + @CompanyName + ']')
--DROP KNOWN CONSTRAINT FOR '[Shippers].[CompanyName]'
if exists(select * from sysobjects where name = 'DF__SHIPPERS_COMPANYNAME' and xtype = 'D')
ALTER TABLE [dbo].[Shippers] DROP CONSTRAINT [DF__SHIPPERS_COMPANYNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Shippers].[Phone]' if one exists
declare @Phone varchar(500)
set @Phone = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Shippers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Shippers') and name = 'Phone'))
if (@Phone IS NOT NULL) exec ('ALTER TABLE [Shippers] DROP CONSTRAINT [' + @Phone + ']')
--DROP KNOWN CONSTRAINT FOR '[Shippers].[Phone]'
if exists(select * from sysobjects where name = 'DF__SHIPPERS_PHONE' and xtype = 'D')
ALTER TABLE [dbo].[Shippers] DROP CONSTRAINT [DF__SHIPPERS_PHONE] 
--DROP UNKNOWN CONSTRAINT FOR '[Shippers].[ShipperID]' if one exists
declare @ShipperID varchar(500)
set @ShipperID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Shippers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Shippers') and name = 'ShipperID'))
if (@ShipperID IS NOT NULL) exec ('ALTER TABLE [Shippers] DROP CONSTRAINT [' + @ShipperID + ']')
--DROP KNOWN CONSTRAINT FOR '[Shippers].[ShipperID]'
if exists(select * from sysobjects where name = 'DF__SHIPPERS_SHIPPERID' and xtype = 'D')
ALTER TABLE [dbo].[Shippers] DROP CONSTRAINT [DF__SHIPPERS_SHIPPERID] 
--END DEFAULTS FOR TABLE [Shippers]
GO

--BEGIN DEFAULTS FOR TABLE [Suppliers]
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[Address]' if one exists
declare @Address varchar(500)
set @Address = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'Address'))
if (@Address IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Address + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[Address]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_ADDRESS' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_ADDRESS] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[City]' if one exists
declare @City varchar(500)
set @City = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'City'))
if (@City IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @City + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[City]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_CITY' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_CITY] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[CompanyName]' if one exists
declare @CompanyName varchar(500)
set @CompanyName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'CompanyName'))
if (@CompanyName IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @CompanyName + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[CompanyName]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_COMPANYNAME' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_COMPANYNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[ContactName]' if one exists
declare @ContactName varchar(500)
set @ContactName = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'ContactName'))
if (@ContactName IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @ContactName + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[ContactName]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_CONTACTNAME' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_CONTACTNAME] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[ContactTitle]' if one exists
declare @ContactTitle varchar(500)
set @ContactTitle = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'ContactTitle'))
if (@ContactTitle IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @ContactTitle + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[ContactTitle]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_CONTACTTITLE' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_CONTACTTITLE] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[Country]' if one exists
declare @Country varchar(500)
set @Country = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'Country'))
if (@Country IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Country + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[Country]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_COUNTRY' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_COUNTRY] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[Fax]' if one exists
declare @Fax varchar(500)
set @Fax = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'Fax'))
if (@Fax IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Fax + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[Fax]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_FAX' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_FAX] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[HomePage]' if one exists
declare @HomePage varchar(500)
set @HomePage = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'HomePage'))
if (@HomePage IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @HomePage + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[HomePage]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_HOMEPAGE' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_HOMEPAGE] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[Phone]' if one exists
declare @Phone varchar(500)
set @Phone = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'Phone'))
if (@Phone IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Phone + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[Phone]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_PHONE' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_PHONE] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[PostalCode]' if one exists
declare @PostalCode varchar(500)
set @PostalCode = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'PostalCode'))
if (@PostalCode IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @PostalCode + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[PostalCode]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_POSTALCODE' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_POSTALCODE] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[Region]' if one exists
declare @Region varchar(500)
set @Region = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'Region'))
if (@Region IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @Region + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[Region]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_REGION' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_REGION] 
--DROP UNKNOWN CONSTRAINT FOR '[Suppliers].[SupplierID]' if one exists
declare @SupplierID varchar(500)
set @SupplierID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Suppliers') and name = 'SupplierID'))
if (@SupplierID IS NOT NULL) exec ('ALTER TABLE [Suppliers] DROP CONSTRAINT [' + @SupplierID + ']')
--DROP KNOWN CONSTRAINT FOR '[Suppliers].[SupplierID]'
if exists(select * from sysobjects where name = 'DF__SUPPLIERS_SUPPLIERID' and xtype = 'D')
ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [DF__SUPPLIERS_SUPPLIERID] 
--END DEFAULTS FOR TABLE [Suppliers]
GO

--BEGIN DEFAULTS FOR TABLE [Territories]
--DROP UNKNOWN CONSTRAINT FOR '[Territories].[RegionID]' if one exists
declare @RegionID varchar(500)
set @RegionID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Territories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Territories') and name = 'RegionID'))
if (@RegionID IS NOT NULL) exec ('ALTER TABLE [Territories] DROP CONSTRAINT [' + @RegionID + ']')
--DROP KNOWN CONSTRAINT FOR '[Territories].[RegionID]'
if exists(select * from sysobjects where name = 'DF__TERRITORIES_REGIONID' and xtype = 'D')
ALTER TABLE [dbo].[Territories] DROP CONSTRAINT [DF__TERRITORIES_REGIONID] 
--DROP UNKNOWN CONSTRAINT FOR '[Territories].[TerritoryDescription]' if one exists
declare @TerritoryDescription varchar(500)
set @TerritoryDescription = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Territories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Territories') and name = 'TerritoryDescription'))
if (@TerritoryDescription IS NOT NULL) exec ('ALTER TABLE [Territories] DROP CONSTRAINT [' + @TerritoryDescription + ']')
--DROP KNOWN CONSTRAINT FOR '[Territories].[TerritoryDescription]'
if exists(select * from sysobjects where name = 'DF__TERRITORIES_TERRITORYDESCRIPTION' and xtype = 'D')
ALTER TABLE [dbo].[Territories] DROP CONSTRAINT [DF__TERRITORIES_TERRITORYDESCRIPTION] 
--DROP UNKNOWN CONSTRAINT FOR '[Territories].[TerritoryID]' if one exists
declare @TerritoryID varchar(500)
set @TerritoryID = (SELECT top 1 name FROM sys.default_constraints WHERE parent_object_id = (SELECT OBJECT_ID FROM sys.tables WHERE name = 'Territories') and parent_column_id = (select top 1 column_id from sys.columns where object_id = (SELECT top 1 OBJECT_ID FROM sys.tables WHERE name = 'Territories') and name = 'TerritoryID'))
if (@TerritoryID IS NOT NULL) exec ('ALTER TABLE [Territories] DROP CONSTRAINT [' + @TerritoryID + ']')
--DROP KNOWN CONSTRAINT FOR '[Territories].[TerritoryID]'
if exists(select * from sysobjects where name = 'DF__TERRITORIES_TERRITORYID' and xtype = 'D')
ALTER TABLE [dbo].[Territories] DROP CONSTRAINT [DF__TERRITORIES_TERRITORYID] 
--END DEFAULTS FOR TABLE [Territories]
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Categories]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Categories' and c.name = 'CategoryID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Categories] ALTER COLUMN [CategoryID] [Int] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Categories' and c.name = 'CategoryName' and o.type = 'U' and c.is_nullable = 1)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_CATEGORIES_CATEGORYNAME')
	DROP INDEX [IDX_CATEGORIES_CATEGORYNAME] ON [dbo].[Categories]
ALTER TABLE [dbo].[Categories] ALTER COLUMN [CategoryName] [NVarChar] (15) NOT NULL
if not exists(select * from sys.indexes where name = 'IDX_CATEGORIES_CATEGORYNAME')
CREATE INDEX [IDX_CATEGORIES_CATEGORYNAME] ON [dbo].[Categories]([CategoryName])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Categories' and c.name = 'Description' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Categories] ALTER COLUMN [Description] [NText] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Categories' and c.name = 'Picture' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Categories] ALTER COLUMN [Picture] [Image] NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [CustomerCustomerDemo]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'CustomerCustomerDemo' and c.name = 'CustomerID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[CustomerCustomerDemo] ALTER COLUMN [CustomerID] [NChar] (5) NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'CustomerCustomerDemo' and c.name = 'CustomerTypeID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[CustomerCustomerDemo] ALTER COLUMN [CustomerTypeID] [NChar] (10) NOT NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [CustomerDemographics]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'CustomerDemographics' and c.name = 'CustomerDesc' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[CustomerDemographics] ALTER COLUMN [CustomerDesc] [NText] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'CustomerDemographics' and c.name = 'CustomerTypeID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[CustomerDemographics] ALTER COLUMN [CustomerTypeID] [NChar] (10) NOT NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Customers]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'Address' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Customers] ALTER COLUMN [Address] [NVarChar] (60) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'City' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_CITY')
	DROP INDEX [IDX_CUSTOMERS_CITY] ON [dbo].[Customers]
ALTER TABLE [dbo].[Customers] ALTER COLUMN [City] [NVarChar] (15) NULL
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_CITY')
CREATE INDEX [IDX_CUSTOMERS_CITY] ON [dbo].[Customers]([City])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'CompanyName' and o.type = 'U' and c.is_nullable = 1)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_COMPANYNAME')
	DROP INDEX [IDX_CUSTOMERS_COMPANYNAME] ON [dbo].[Customers]
ALTER TABLE [dbo].[Customers] ALTER COLUMN [CompanyName] [NVarChar] (40) NOT NULL
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_COMPANYNAME')
CREATE INDEX [IDX_CUSTOMERS_COMPANYNAME] ON [dbo].[Customers]([CompanyName])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'ContactName' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Customers] ALTER COLUMN [ContactName] [NVarChar] (30) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'ContactTitle' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Customers] ALTER COLUMN [ContactTitle] [NVarChar] (30) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'Country' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Customers] ALTER COLUMN [Country] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'CustomerID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Customers] ALTER COLUMN [CustomerID] [NChar] (5) NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'Fax' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Customers] ALTER COLUMN [Fax] [NVarChar] (24) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'Phone' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Customers] ALTER COLUMN [Phone] [NVarChar] (24) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'PostalCode' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_POSTALCODE')
	DROP INDEX [IDX_CUSTOMERS_POSTALCODE] ON [dbo].[Customers]
ALTER TABLE [dbo].[Customers] ALTER COLUMN [PostalCode] [NVarChar] (10) NULL
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_POSTALCODE')
CREATE INDEX [IDX_CUSTOMERS_POSTALCODE] ON [dbo].[Customers]([PostalCode])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Customers' and c.name = 'Region' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_REGION')
	DROP INDEX [IDX_CUSTOMERS_REGION] ON [dbo].[Customers]
ALTER TABLE [dbo].[Customers] ALTER COLUMN [Region] [NVarChar] (15) NULL
if not exists(select * from sys.indexes where name = 'IDX_CUSTOMERS_REGION')
CREATE INDEX [IDX_CUSTOMERS_REGION] ON [dbo].[Customers]([Region])END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Employees]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'Address' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [Address] [NVarChar] (60) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'BirthDate' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [BirthDate] [DateTime] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'City' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [City] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'Country' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [Country] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'EmployeeID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [EmployeeID] [Int] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'Extension' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [Extension] [NVarChar] (4) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'FirstName' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [FirstName] [NVarChar] (10) NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'HireDate' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [HireDate] [DateTime] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'HomePhone' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [HomePhone] [NVarChar] (24) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'LastName' and o.type = 'U' and c.is_nullable = 1)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_LASTNAME')
	DROP INDEX [IDX_EMPLOYEES_LASTNAME] ON [dbo].[Employees]
ALTER TABLE [dbo].[Employees] ALTER COLUMN [LastName] [NVarChar] (20) NOT NULL
if not exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_LASTNAME')
CREATE INDEX [IDX_EMPLOYEES_LASTNAME] ON [dbo].[Employees]([LastName])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'Notes' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [Notes] [NText] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'Photo' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [Photo] [Image] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'PhotoPath' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [PhotoPath] [NVarChar] (255) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'PostalCode' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_POSTALCODE')
	DROP INDEX [IDX_EMPLOYEES_POSTALCODE] ON [dbo].[Employees]
ALTER TABLE [dbo].[Employees] ALTER COLUMN [PostalCode] [NVarChar] (10) NULL
if not exists(select * from sys.indexes where name = 'IDX_EMPLOYEES_POSTALCODE')
CREATE INDEX [IDX_EMPLOYEES_POSTALCODE] ON [dbo].[Employees]([PostalCode])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'Region' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [Region] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'ReportsTo' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [ReportsTo] [Int] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'Title' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [Title] [NVarChar] (30) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Employees' and c.name = 'TitleOfCourtesy' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Employees] ALTER COLUMN [TitleOfCourtesy] [NVarChar] (25) NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [EmployeeTerritories]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'EmployeeTerritories' and c.name = 'EmployeeID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[EmployeeTerritories] ALTER COLUMN [EmployeeID] [Int] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'EmployeeTerritories' and c.name = 'TerritoryID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[EmployeeTerritories] ALTER COLUMN [TerritoryID] [NVarChar] (20) NOT NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Order Details]
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__ORDER DETAILS_DISCOUNT')
ALTER TABLE [dbo].[Order Details] ADD CONSTRAINT [DF__ORDER DETAILS_DISCOUNT] DEFAULT (0) FOR [Discount]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Order Details' and c.name = 'Discount' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Order Details] ALTER COLUMN [Discount] [Real] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Order Details' and c.name = 'OrderID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Order Details] ALTER COLUMN [OrderID] [Int] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Order Details' and c.name = 'ProductID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Order Details] ALTER COLUMN [ProductID] [Int] NOT NULL
END
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__ORDER DETAILS_QUANTITY')
ALTER TABLE [dbo].[Order Details] ADD CONSTRAINT [DF__ORDER DETAILS_QUANTITY] DEFAULT (1) FOR [Quantity]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Order Details' and c.name = 'Quantity' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Order Details] ALTER COLUMN [Quantity] [SmallInt] NOT NULL
END
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__ORDER DETAILS_UNITPRICE')
ALTER TABLE [dbo].[Order Details] ADD CONSTRAINT [DF__ORDER DETAILS_UNITPRICE] DEFAULT (0) FOR [UnitPrice]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Order Details' and c.name = 'UnitPrice' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Order Details] ALTER COLUMN [UnitPrice] [Money] NOT NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Orders]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'CustomerID' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_ORDERS_CUSTOMERID')
	DROP INDEX [IDX_ORDERS_CUSTOMERID] ON [dbo].[Orders]
ALTER TABLE [dbo].[Orders] ALTER COLUMN [CustomerID] [NChar] (5) NULL
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_CUSTOMERID')
CREATE INDEX [IDX_ORDERS_CUSTOMERID] ON [dbo].[Orders]([CustomerID])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'EmployeeID' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_ORDERS_EMPLOYEEID')
	DROP INDEX [IDX_ORDERS_EMPLOYEEID] ON [dbo].[Orders]
ALTER TABLE [dbo].[Orders] ALTER COLUMN [EmployeeID] [Int] NULL
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_EMPLOYEEID')
CREATE INDEX [IDX_ORDERS_EMPLOYEEID] ON [dbo].[Orders]([EmployeeID])END
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__ORDERS_FREIGHT')
ALTER TABLE [dbo].[Orders] ADD CONSTRAINT [DF__ORDERS_FREIGHT] DEFAULT (0) FOR [Freight]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'Freight' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Orders] ALTER COLUMN [Freight] [Money] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'OrderDate' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_ORDERS_ORDERDATE')
	DROP INDEX [IDX_ORDERS_ORDERDATE] ON [dbo].[Orders]
ALTER TABLE [dbo].[Orders] ALTER COLUMN [OrderDate] [DateTime] NULL
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_ORDERDATE')
CREATE INDEX [IDX_ORDERS_ORDERDATE] ON [dbo].[Orders]([OrderDate])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'OrderID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Orders] ALTER COLUMN [OrderID] [Int] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'RequiredDate' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Orders] ALTER COLUMN [RequiredDate] [DateTime] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'ShipAddress' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Orders] ALTER COLUMN [ShipAddress] [NVarChar] (60) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'ShipCity' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Orders] ALTER COLUMN [ShipCity] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'ShipCountry' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Orders] ALTER COLUMN [ShipCountry] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'ShipName' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Orders] ALTER COLUMN [ShipName] [NVarChar] (40) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'ShippedDate' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPEDDATE')
	DROP INDEX [IDX_ORDERS_SHIPPEDDATE] ON [dbo].[Orders]
ALTER TABLE [dbo].[Orders] ALTER COLUMN [ShippedDate] [DateTime] NULL
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPEDDATE')
CREATE INDEX [IDX_ORDERS_SHIPPEDDATE] ON [dbo].[Orders]([ShippedDate])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'ShipPostalCode' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPOSTALCODE')
	DROP INDEX [IDX_ORDERS_SHIPPOSTALCODE] ON [dbo].[Orders]
ALTER TABLE [dbo].[Orders] ALTER COLUMN [ShipPostalCode] [NVarChar] (10) NULL
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPPOSTALCODE')
CREATE INDEX [IDX_ORDERS_SHIPPOSTALCODE] ON [dbo].[Orders]([ShipPostalCode])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'ShipRegion' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Orders] ALTER COLUMN [ShipRegion] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Orders' and c.name = 'ShipVia' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPVIA')
	DROP INDEX [IDX_ORDERS_SHIPVIA] ON [dbo].[Orders]
ALTER TABLE [dbo].[Orders] ALTER COLUMN [ShipVia] [Int] NULL
if not exists(select * from sys.indexes where name = 'IDX_ORDERS_SHIPVIA')
CREATE INDEX [IDX_ORDERS_SHIPVIA] ON [dbo].[Orders]([ShipVia])END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Products]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'CategoryID' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_PRODUCTS_CATEGORYID')
	DROP INDEX [IDX_PRODUCTS_CATEGORYID] ON [dbo].[Products]
ALTER TABLE [dbo].[Products] ALTER COLUMN [CategoryID] [Int] NULL
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_CATEGORYID')
CREATE INDEX [IDX_PRODUCTS_CATEGORYID] ON [dbo].[Products]([CategoryID])END
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__PRODUCTS_DISCONTINUED')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_DISCONTINUED] DEFAULT (0) FOR [Discontinued]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'Discontinued' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Products] ALTER COLUMN [Discontinued] [Bit] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'ProductID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Products] ALTER COLUMN [ProductID] [Int] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'ProductName' and o.type = 'U' and c.is_nullable = 1)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_PRODUCTS_PRODUCTNAME')
	DROP INDEX [IDX_PRODUCTS_PRODUCTNAME] ON [dbo].[Products]
ALTER TABLE [dbo].[Products] ALTER COLUMN [ProductName] [NVarChar] (40) NOT NULL
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_PRODUCTNAME')
CREATE INDEX [IDX_PRODUCTS_PRODUCTNAME] ON [dbo].[Products]([ProductName])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'QuantityPerUnit' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Products] ALTER COLUMN [QuantityPerUnit] [NVarChar] (20) NULL
END
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__PRODUCTS_REORDERLEVEL')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_REORDERLEVEL] DEFAULT (0) FOR [ReorderLevel]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'ReorderLevel' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Products] ALTER COLUMN [ReorderLevel] [SmallInt] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'SupplierID' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_PRODUCTS_SUPPLIERID')
	DROP INDEX [IDX_PRODUCTS_SUPPLIERID] ON [dbo].[Products]
ALTER TABLE [dbo].[Products] ALTER COLUMN [SupplierID] [Int] NULL
if not exists(select * from sys.indexes where name = 'IDX_PRODUCTS_SUPPLIERID')
CREATE INDEX [IDX_PRODUCTS_SUPPLIERID] ON [dbo].[Products]([SupplierID])END
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__PRODUCTS_UNITPRICE')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_UNITPRICE] DEFAULT (0) FOR [UnitPrice]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'UnitPrice' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Products] ALTER COLUMN [UnitPrice] [Money] NULL
END
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__PRODUCTS_UNITSINSTOCK')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_UNITSINSTOCK] DEFAULT (0) FOR [UnitsInStock]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'UnitsInStock' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Products] ALTER COLUMN [UnitsInStock] [SmallInt] NULL
END
if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DF__PRODUCTS_UNITSONORDER')
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [DF__PRODUCTS_UNITSONORDER] DEFAULT (0) FOR [UnitsOnOrder]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Products' and c.name = 'UnitsOnOrder' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Products] ALTER COLUMN [UnitsOnOrder] [SmallInt] NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Region]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Region' and c.name = 'RegionDescription' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Region] ALTER COLUMN [RegionDescription] [NChar] (50) NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Region' and c.name = 'RegionID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Region] ALTER COLUMN [RegionID] [Int] NOT NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Shippers]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Shippers' and c.name = 'CompanyName' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Shippers] ALTER COLUMN [CompanyName] [NVarChar] (40) NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Shippers' and c.name = 'Phone' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Shippers] ALTER COLUMN [Phone] [NVarChar] (24) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Shippers' and c.name = 'ShipperID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Shippers] ALTER COLUMN [ShipperID] [Int] NOT NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Suppliers]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'Address' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [Address] [NVarChar] (60) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'City' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [City] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'CompanyName' and o.type = 'U' and c.is_nullable = 1)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_COMPANYNAME')
	DROP INDEX [IDX_SUPPLIERS_COMPANYNAME] ON [dbo].[Suppliers]
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [CompanyName] [NVarChar] (40) NOT NULL
if not exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_COMPANYNAME')
CREATE INDEX [IDX_SUPPLIERS_COMPANYNAME] ON [dbo].[Suppliers]([CompanyName])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'ContactName' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [ContactName] [NVarChar] (30) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'ContactTitle' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [ContactTitle] [NVarChar] (30) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'Country' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [Country] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'Fax' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [Fax] [NVarChar] (24) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'HomePage' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [HomePage] [NText] NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'Phone' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [Phone] [NVarChar] (24) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'PostalCode' and o.type = 'U' and c.is_nullable = 0)
BEGIN
if exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_POSTALCODE')
	DROP INDEX [IDX_SUPPLIERS_POSTALCODE] ON [dbo].[Suppliers]
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [PostalCode] [NVarChar] (10) NULL
if not exists(select * from sys.indexes where name = 'IDX_SUPPLIERS_POSTALCODE')
CREATE INDEX [IDX_SUPPLIERS_POSTALCODE] ON [dbo].[Suppliers]([PostalCode])END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'Region' and o.type = 'U' and c.is_nullable = 0)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [Region] [NVarChar] (15) NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Suppliers' and c.name = 'SupplierID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Suppliers] ALTER COLUMN [SupplierID] [Int] NOT NULL
END
GO

--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [Territories]
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Territories' and c.name = 'RegionID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Territories] ALTER COLUMN [RegionID] [Int] NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Territories' and c.name = 'TerritoryDescription' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Territories] ALTER COLUMN [TerritoryDescription] [NChar] (50) NOT NULL
END
if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = 'dbo' AND o.name = 'Territories' and c.name = 'TerritoryID' and o.type = 'U' and c.is_nullable = 1)
BEGIN
ALTER TABLE [dbo].[Territories] ALTER COLUMN [TerritoryID] [NVarChar] (20) NOT NULL
END
GO

--CLEAR ALL EXISTING GENERATED STORED PROCEDURES
select '[' + s.name + '].[' + o.name + ']' as [text] 
into #tmpDropSP
from sys.objects o inner join sys.schemas s on o.schema_id = s.schema_id 
where o.name like 'gen_%' and type = 'P'

--LOOP AND REMOVE THESE CONSTRAINTS
DECLARE @mycur CURSOR
DECLARE @test VARCHAR(1000)
SET @mycur = CURSOR
FOR
SELECT [text] FROM #tmpDropSP
OPEN @mycur
FETCH NEXT FROM @mycur INTO @test
WHILE @@FETCH_STATUS = 0
BEGIN
exec(
'
if exists (select * from dbo.sysobjects where id = object_id(N''' + @test + ''') and OBJECTPROPERTY(id, N''IsProcedure'') = 1)
drop procedure ' + @test + '
')
FETCH NEXT FROM @mycur INTO @test
END
DEALLOCATE @mycur

DROP TABLE #tmpDropSP
GO

