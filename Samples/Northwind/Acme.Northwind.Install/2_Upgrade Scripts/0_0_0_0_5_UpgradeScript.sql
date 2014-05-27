--Generated Upgrade For Version 0.0.0.0.5
--Generated on 2013-02-04 22:05:53

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__PRODUCTS_CATEGORIES' and xtype = 'F')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK__PRODUCTS_CATEGORIES]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK_CUSTOMERCUSTOMERDEMO_CUSTOMERCUSTOMERDEMO_CUSTOMERDEMOGRAPHICS' and xtype = 'F')
ALTER TABLE [dbo].[CustomerCustomerDemo] DROP CONSTRAINT [FK_CUSTOMERCUSTOMERDEMO_CUSTOMERCUSTOMERDEMO_CUSTOMERDEMOGRAPHICS]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__CUSTOMERCUSTOMERDEMO_CUSTOMERS' and xtype = 'F')
ALTER TABLE [dbo].[CustomerCustomerDemo] DROP CONSTRAINT [FK__CUSTOMERCUSTOMERDEMO_CUSTOMERS]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__ORDERS_CUSTOMERS' and xtype = 'F')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK__ORDERS_CUSTOMERS]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK_REPORTTO_EMPLOYEES_EMPLOYEES' and xtype = 'F')
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_REPORTTO_EMPLOYEES_EMPLOYEES]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__EMPLOYEETERRITORIES_EMPLOYEES' and xtype = 'F')
ALTER TABLE [dbo].[EmployeeTerritories] DROP CONSTRAINT [FK__EMPLOYEETERRITORIES_EMPLOYEES]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__ORDERS_EMPLOYEES' and xtype = 'F')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK__ORDERS_EMPLOYEES]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK_ORDER_DETAILS_ORDERS_ORDER_DETAILS_ORDERS' and xtype = 'F')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [FK_ORDER_DETAILS_ORDERS_ORDER_DETAILS_ORDERS]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK_ORDER_DETAILS_PRODUCTS_ORDER_DETAILS_PRODUCTS' and xtype = 'F')
ALTER TABLE [dbo].[Order Details] DROP CONSTRAINT [FK_ORDER_DETAILS_PRODUCTS_ORDER_DETAILS_PRODUCTS]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__TERRITORIES_REGION' and xtype = 'F')
ALTER TABLE [dbo].[Territories] DROP CONSTRAINT [FK__TERRITORIES_REGION]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__ORDERS_SHIPPERS' and xtype = 'F')
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK__ORDERS_SHIPPERS]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__PRODUCTS_SUPPLIERS' and xtype = 'F')
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK__PRODUCTS_SUPPLIERS]
GO

--REMOVE FOREIGN KEY
if exists(select * from sysobjects where name = 'FK__EMPLOYEETERRITORIES_TERRITORIES' and xtype = 'F')
ALTER TABLE [dbo].[EmployeeTerritories] DROP CONSTRAINT [FK__EMPLOYEETERRITORIES_TERRITORIES]
GO

