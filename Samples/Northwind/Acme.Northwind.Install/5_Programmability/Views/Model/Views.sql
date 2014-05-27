--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.

--This SQL is generated for the model defined view [Alphabetical list of products]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Alphabetical list of products]') and [xtype] = 'V')
drop view [dbo].[Alphabetical list of products]
--MODELID: 95ff1824-aee7-459f-9a30-40b1fc0a7bf4
GO

CREATE VIEW [dbo].[Alphabetical list of products]
AS

SELECT Products.*, Categories.CategoryName
FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID
WHERE (((Products.Discontinued)=0))
--MODELID,BODY: 95ff1824-aee7-459f-9a30-40b1fc0a7bf4
GO
exec sp_refreshview N'[dbo].[Alphabetical list of products]';
--MODELID: 95ff1824-aee7-459f-9a30-40b1fc0a7bf4
GO

--This SQL is generated for the model defined view [Category Sales for 1997]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Category Sales for 1997]') and [xtype] = 'V')
drop view [dbo].[Category Sales for 1997]
--MODELID: fc435ce3-4422-4c18-9f0c-381eece9b9b7
GO

CREATE VIEW [dbo].[Category Sales for 1997]
AS

SELECT [Product Sales for 1997].CategoryName, Sum([Product Sales for 1997].ProductSales) AS CategorySales
FROM [Product Sales for 1997]
GROUP BY [Product Sales for 1997].CategoryName
--MODELID,BODY: fc435ce3-4422-4c18-9f0c-381eece9b9b7
GO
exec sp_refreshview N'[dbo].[Category Sales for 1997]';
--MODELID: fc435ce3-4422-4c18-9f0c-381eece9b9b7
GO

--This SQL is generated for the model defined view [Current Product List]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Current Product List]') and [xtype] = 'V')
drop view [dbo].[Current Product List]
--MODELID: 21060a50-ee0f-441f-9796-5827ed4f9ff4
GO

CREATE VIEW [dbo].[Current Product List]
AS

SELECT Product_List.ProductID, Product_List.ProductName
FROM Products AS Product_List
WHERE (((Product_List.Discontinued)=0))
--ORDER BY Product_List.ProductName
--MODELID,BODY: 21060a50-ee0f-441f-9796-5827ed4f9ff4
GO
exec sp_refreshview N'[dbo].[Current Product List]';
--MODELID: 21060a50-ee0f-441f-9796-5827ed4f9ff4
GO

--This SQL is generated for the model defined view [Customer and Suppliers by City]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Customer and Suppliers by City]') and [xtype] = 'V')
drop view [dbo].[Customer and Suppliers by City]
--MODELID: 96bcefa8-7727-4a78-a27a-54da7e72b694
GO

CREATE VIEW [dbo].[Customer and Suppliers by City]
AS

SELECT City, CompanyName, ContactName, 'Customers' AS Relationship 
FROM Customers
UNION SELECT City, CompanyName, ContactName, 'Suppliers'
FROM Suppliers
--ORDER BY City, CompanyName
--MODELID,BODY: 96bcefa8-7727-4a78-a27a-54da7e72b694
GO
exec sp_refreshview N'[dbo].[Customer and Suppliers by City]';
--MODELID: 96bcefa8-7727-4a78-a27a-54da7e72b694
GO

--This SQL is generated for the model defined view [Invoices]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Invoices]') and [xtype] = 'V')
drop view [dbo].[Invoices]
--MODELID: 98199790-baad-4bd0-98b0-f5d63d2690ed
GO

CREATE VIEW [dbo].[Invoices]
AS

SELECT Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, 
	Orders.ShipCountry, Orders.CustomerID, Customers.CompanyName AS CustomerName, Customers.Address, Customers.City, 
	Customers.Region, Customers.PostalCode, Customers.Country, 
	(FirstName + ' ' + LastName) AS Salesperson, 
	Orders.OrderID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Shippers.CompanyName As ShipperName, 
	"Order Details".ProductID, Products.ProductName, "Order Details".UnitPrice, "Order Details".Quantity, 
	"Order Details".Discount, 
	(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice, Orders.Freight
FROM 	Shippers INNER JOIN 
		(Products INNER JOIN 
			(
				(Employees INNER JOIN 
					(Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID) 
				ON Employees.EmployeeID = Orders.EmployeeID) 
			INNER JOIN "Order Details" ON Orders.OrderID = "Order Details".OrderID) 
		ON Products.ProductID = "Order Details".ProductID) 
	ON Shippers.ShipperID = Orders.ShipVia
--MODELID,BODY: 98199790-baad-4bd0-98b0-f5d63d2690ed
GO
exec sp_refreshview N'[dbo].[Invoices]';
--MODELID: 98199790-baad-4bd0-98b0-f5d63d2690ed
GO

--This SQL is generated for the model defined view [Order Details Extended]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Order Details Extended]') and [xtype] = 'V')
drop view [dbo].[Order Details Extended]
--MODELID: d3449e44-7228-47ce-b5d7-a1ea68c40745
GO

CREATE VIEW [dbo].[Order Details Extended]
AS

SELECT "Order Details".OrderID, "Order Details".ProductID, Products.ProductName, 
	"Order Details".UnitPrice, "Order Details".Quantity, "Order Details".Discount, 
	(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice
FROM Products INNER JOIN "Order Details" ON Products.ProductID = "Order Details".ProductID
--ORDER BY "Order Details".OrderID
--MODELID,BODY: d3449e44-7228-47ce-b5d7-a1ea68c40745
GO
exec sp_refreshview N'[dbo].[Order Details Extended]';
--MODELID: d3449e44-7228-47ce-b5d7-a1ea68c40745
GO

--This SQL is generated for the model defined view [Order Subtotals]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Order Subtotals]') and [xtype] = 'V')
drop view [dbo].[Order Subtotals]
--MODELID: 6a7e7cfc-6bb2-48d6-a9a3-e6a31592133a
GO

CREATE VIEW [dbo].[Order Subtotals]
AS

SELECT "Order Details".OrderID, Sum(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS Subtotal
FROM "Order Details"
GROUP BY "Order Details".OrderID
--MODELID,BODY: 6a7e7cfc-6bb2-48d6-a9a3-e6a31592133a
GO
exec sp_refreshview N'[dbo].[Order Subtotals]';
--MODELID: 6a7e7cfc-6bb2-48d6-a9a3-e6a31592133a
GO

--This SQL is generated for the model defined view [Orders Qry]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Orders Qry]') and [xtype] = 'V')
drop view [dbo].[Orders Qry]
--MODELID: ef85b49d-75f5-4160-802b-b70633373bba
GO

CREATE VIEW [dbo].[Orders Qry]
AS

SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, 
	Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, 
	Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, 
	Customers.CompanyName, Customers.Address, Customers.City, Customers.Region, Customers.PostalCode, Customers.Country
FROM Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
--MODELID,BODY: ef85b49d-75f5-4160-802b-b70633373bba
GO
exec sp_refreshview N'[dbo].[Orders Qry]';
--MODELID: ef85b49d-75f5-4160-802b-b70633373bba
GO

--This SQL is generated for the model defined view [Product Sales for 1997]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Product Sales for 1997]') and [xtype] = 'V')
drop view [dbo].[Product Sales for 1997]
--MODELID: 536cc4c3-2cd4-4b8a-b0e7-eaa88b65e810
GO

CREATE VIEW [dbo].[Product Sales for 1997]
AS

SELECT Categories.CategoryName, Products.ProductName, 
Sum(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS ProductSales
FROM (Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID) 
	INNER JOIN (Orders 
		INNER JOIN "Order Details" ON Orders.OrderID = "Order Details".OrderID) 
	ON Products.ProductID = "Order Details".ProductID
WHERE (((Orders.ShippedDate) Between '19970101' And '19971231'))
GROUP BY Categories.CategoryName, Products.ProductName
--MODELID,BODY: 536cc4c3-2cd4-4b8a-b0e7-eaa88b65e810
GO
exec sp_refreshview N'[dbo].[Product Sales for 1997]';
--MODELID: 536cc4c3-2cd4-4b8a-b0e7-eaa88b65e810
GO

--This SQL is generated for the model defined view [Products Above Average Price]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Products Above Average Price]') and [xtype] = 'V')
drop view [dbo].[Products Above Average Price]
--MODELID: 17727839-d339-409f-99d4-a1fe78875b59
GO

CREATE VIEW [dbo].[Products Above Average Price]
AS

SELECT Products.ProductName, Products.UnitPrice
FROM Products
WHERE Products.UnitPrice>(SELECT AVG(UnitPrice) From Products)
--ORDER BY Products.UnitPrice DESC
--MODELID,BODY: 17727839-d339-409f-99d4-a1fe78875b59
GO
exec sp_refreshview N'[dbo].[Products Above Average Price]';
--MODELID: 17727839-d339-409f-99d4-a1fe78875b59
GO

--This SQL is generated for the model defined view [Products by Category]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Products by Category]') and [xtype] = 'V')
drop view [dbo].[Products by Category]
--MODELID: 34ae20b1-0efe-472f-b814-3ff5832a715c
GO

CREATE VIEW [dbo].[Products by Category]
AS

SELECT Categories.CategoryName, Products.ProductName, Products.QuantityPerUnit, Products.UnitsInStock, Products.Discontinued
FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID
WHERE Products.Discontinued <> 1
--ORDER BY Categories.CategoryName, Products.ProductName
--MODELID,BODY: 34ae20b1-0efe-472f-b814-3ff5832a715c
GO
exec sp_refreshview N'[dbo].[Products by Category]';
--MODELID: 34ae20b1-0efe-472f-b814-3ff5832a715c
GO

--This SQL is generated for the model defined view [Quarterly Orders]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Quarterly Orders]') and [xtype] = 'V')
drop view [dbo].[Quarterly Orders]
--MODELID: 610e0597-c137-42a5-acfc-bcf734e3b8a1
GO

CREATE VIEW [dbo].[Quarterly Orders]
AS

SELECT DISTINCT Customers.CustomerID, Customers.CompanyName, Customers.City, Customers.Country
FROM Customers RIGHT JOIN Orders ON Customers.CustomerID = Orders.CustomerID
WHERE Orders.OrderDate BETWEEN '19970101' And '19971231'
--MODELID,BODY: 610e0597-c137-42a5-acfc-bcf734e3b8a1
GO
exec sp_refreshview N'[dbo].[Quarterly Orders]';
--MODELID: 610e0597-c137-42a5-acfc-bcf734e3b8a1
GO

--This SQL is generated for the model defined view [Sales by Category]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sales by Category]') and [xtype] = 'V')
drop view [dbo].[Sales by Category]
--MODELID: 75f78407-1931-46b1-8d7a-9105f5e478cf
GO

CREATE VIEW [dbo].[Sales by Category]
AS

SELECT Categories.CategoryID, Categories.CategoryName, Products.ProductName, 
	Sum("Order Details Extended".ExtendedPrice) AS ProductSales
FROM 	Categories INNER JOIN 
		(Products INNER JOIN 
			(Orders INNER JOIN "Order Details Extended" ON Orders.OrderID = "Order Details Extended".OrderID) 
		ON Products.ProductID = "Order Details Extended".ProductID) 
	ON Categories.CategoryID = Products.CategoryID
WHERE Orders.OrderDate BETWEEN '19970101' And '19971231'
GROUP BY Categories.CategoryID, Categories.CategoryName, Products.ProductName
--ORDER BY Products.ProductName
--MODELID,BODY: 75f78407-1931-46b1-8d7a-9105f5e478cf
GO
exec sp_refreshview N'[dbo].[Sales by Category]';
--MODELID: 75f78407-1931-46b1-8d7a-9105f5e478cf
GO

--This SQL is generated for the model defined view [Sales Totals by Amount]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sales Totals by Amount]') and [xtype] = 'V')
drop view [dbo].[Sales Totals by Amount]
--MODELID: 804d3294-db46-4616-9d4a-2cc513160ee6
GO

CREATE VIEW [dbo].[Sales Totals by Amount]
AS

SELECT "Order Subtotals".Subtotal AS SaleAmount, Orders.OrderID, Customers.CompanyName, Orders.ShippedDate
FROM 	Customers INNER JOIN 
		(Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID) 
	ON Customers.CustomerID = Orders.CustomerID
WHERE ("Order Subtotals".Subtotal >2500) AND (Orders.ShippedDate BETWEEN '19970101' And '19971231')
--MODELID,BODY: 804d3294-db46-4616-9d4a-2cc513160ee6
GO
exec sp_refreshview N'[dbo].[Sales Totals by Amount]';
--MODELID: 804d3294-db46-4616-9d4a-2cc513160ee6
GO

--This SQL is generated for the model defined view [Summary of Sales by Quarter]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Summary of Sales by Quarter]') and [xtype] = 'V')
drop view [dbo].[Summary of Sales by Quarter]
--MODELID: 72d5c826-8be3-429a-ae5a-362697ca550b
GO

CREATE VIEW [dbo].[Summary of Sales by Quarter]
AS

SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal
FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
WHERE Orders.ShippedDate IS NOT NULL
--ORDER BY Orders.ShippedDate
--MODELID,BODY: 72d5c826-8be3-429a-ae5a-362697ca550b
GO
exec sp_refreshview N'[dbo].[Summary of Sales by Quarter]';
--MODELID: 72d5c826-8be3-429a-ae5a-362697ca550b
GO

--This SQL is generated for the model defined view [Summary of Sales by Year]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Summary of Sales by Year]') and [xtype] = 'V')
drop view [dbo].[Summary of Sales by Year]
--MODELID: d15f2672-15e9-4e7a-a587-867b215f9a9c
GO

CREATE VIEW [dbo].[Summary of Sales by Year]
AS

SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal
FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
WHERE Orders.ShippedDate IS NOT NULL
--ORDER BY Orders.ShippedDate
--MODELID,BODY: d15f2672-15e9-4e7a-a587-867b215f9a9c
GO
exec sp_refreshview N'[dbo].[Summary of Sales by Year]';
--MODELID: d15f2672-15e9-4e7a-a587-867b215f9a9c
GO

