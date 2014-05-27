--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Model Version 0.0.0.0

--This SQL is generated for the model defined view [Alphabetical list of products]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Alphabetical list of products]') and [xtype] = 'V')
	drop view [dbo].[Alphabetical list of products]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Alphabetical list of products]
AS

SELECT Products.*, Categories.CategoryName
FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID
WHERE (((Products.Discontinued)=0))
GO
exec sp_refreshview N'[dbo].[Alphabetical list of products]';
GO

--This SQL is generated for the model defined view [Category Sales for 1997]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Category Sales for 1997]') and [xtype] = 'V')
	drop view [dbo].[Category Sales for 1997]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Category Sales for 1997]
AS

SELECT [Product Sales for 1997].CategoryName, Sum([Product Sales for 1997].ProductSales) AS CategorySales
FROM [Product Sales for 1997]
GROUP BY [Product Sales for 1997].CategoryName
GO
exec sp_refreshview N'[dbo].[Category Sales for 1997]';
GO

--This SQL is generated for the model defined view [Current Product List]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Current Product List]') and [xtype] = 'V')
	drop view [dbo].[Current Product List]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Current Product List]
AS

SELECT Product_List.ProductID, Product_List.ProductName
FROM Products AS Product_List
WHERE (((Product_List.Discontinued)=0))
--ORDER BY Product_List.ProductName
GO
exec sp_refreshview N'[dbo].[Current Product List]';
GO

--This SQL is generated for the model defined view [Customer and Suppliers by City]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Customer and Suppliers by City]') and [xtype] = 'V')
	drop view [dbo].[Customer and Suppliers by City]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Customer and Suppliers by City]
AS

SELECT City, CompanyName, ContactName, 'Customers' AS Relationship 
FROM Customers
UNION SELECT City, CompanyName, ContactName, 'Suppliers'
FROM Suppliers
--ORDER BY City, CompanyName
GO
exec sp_refreshview N'[dbo].[Customer and Suppliers by City]';
GO

--This SQL is generated for the model defined view [Invoices]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Invoices]') and [xtype] = 'V')
	drop view [dbo].[Invoices]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
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
GO
exec sp_refreshview N'[dbo].[Invoices]';
GO

--This SQL is generated for the model defined view [Order Details Extended]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Order Details Extended]') and [xtype] = 'V')
	drop view [dbo].[Order Details Extended]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Order Details Extended]
AS

SELECT "Order Details".OrderID, "Order Details".ProductID, Products.ProductName, 
	"Order Details".UnitPrice, "Order Details".Quantity, "Order Details".Discount, 
	(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice
FROM Products INNER JOIN "Order Details" ON Products.ProductID = "Order Details".ProductID
--ORDER BY "Order Details".OrderID
GO
exec sp_refreshview N'[dbo].[Order Details Extended]';
GO

--This SQL is generated for the model defined view [Order Subtotals]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Order Subtotals]') and [xtype] = 'V')
	drop view [dbo].[Order Subtotals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Order Subtotals]
AS

SELECT "Order Details".OrderID, Sum(CONVERT(money,("Order Details".UnitPrice*Quantity*(1-Discount)/100))*100) AS Subtotal
FROM "Order Details"
GROUP BY "Order Details".OrderID
GO
exec sp_refreshview N'[dbo].[Order Subtotals]';
GO

--This SQL is generated for the model defined view [Orders Qry]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Orders Qry]') and [xtype] = 'V')
	drop view [dbo].[Orders Qry]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Orders Qry]
AS

SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, 
	Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, 
	Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, 
	Customers.CompanyName, Customers.Address, Customers.City, Customers.Region, Customers.PostalCode, Customers.Country
FROM Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GO
exec sp_refreshview N'[dbo].[Orders Qry]';
GO

--This SQL is generated for the model defined view [Product Sales for 1997]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Product Sales for 1997]') and [xtype] = 'V')
	drop view [dbo].[Product Sales for 1997]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
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
GO
exec sp_refreshview N'[dbo].[Product Sales for 1997]';
GO

--This SQL is generated for the model defined view [Products Above Average Price]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Products Above Average Price]') and [xtype] = 'V')
	drop view [dbo].[Products Above Average Price]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Products Above Average Price]
AS

SELECT Products.ProductName, Products.UnitPrice
FROM Products
WHERE Products.UnitPrice>(SELECT AVG(UnitPrice) From Products)
--ORDER BY Products.UnitPrice DESC
GO
exec sp_refreshview N'[dbo].[Products Above Average Price]';
GO

--This SQL is generated for the model defined view [Products by Category]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Products by Category]') and [xtype] = 'V')
	drop view [dbo].[Products by Category]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Products by Category]
AS

SELECT Categories.CategoryName, Products.ProductName, Products.QuantityPerUnit, Products.UnitsInStock, Products.Discontinued
FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID
WHERE Products.Discontinued <> 1
--ORDER BY Categories.CategoryName, Products.ProductName
GO
exec sp_refreshview N'[dbo].[Products by Category]';
GO

--This SQL is generated for the model defined view [Quarterly Orders]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Quarterly Orders]') and [xtype] = 'V')
	drop view [dbo].[Quarterly Orders]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Quarterly Orders]
AS

SELECT DISTINCT Customers.CustomerID, Customers.CompanyName, Customers.City, Customers.Country
FROM Customers RIGHT JOIN Orders ON Customers.CustomerID = Orders.CustomerID
WHERE Orders.OrderDate BETWEEN '19970101' And '19971231'
GO
exec sp_refreshview N'[dbo].[Quarterly Orders]';
GO

--This SQL is generated for the model defined view [Sales by Category]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sales by Category]') and [xtype] = 'V')
	drop view [dbo].[Sales by Category]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
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
GO
exec sp_refreshview N'[dbo].[Sales by Category]';
GO

--This SQL is generated for the model defined view [Sales Totals by Amount]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sales Totals by Amount]') and [xtype] = 'V')
	drop view [dbo].[Sales Totals by Amount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Sales Totals by Amount]
AS

SELECT "Order Subtotals".Subtotal AS SaleAmount, Orders.OrderID, Customers.CompanyName, Orders.ShippedDate
FROM 	Customers INNER JOIN 
		(Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID) 
	ON Customers.CustomerID = Orders.CustomerID
WHERE ("Order Subtotals".Subtotal >2500) AND (Orders.ShippedDate BETWEEN '19970101' And '19971231')
GO
exec sp_refreshview N'[dbo].[Sales Totals by Amount]';
GO

--This SQL is generated for the model defined view [Summary of Sales by Quarter]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Summary of Sales by Quarter]') and [xtype] = 'V')
	drop view [dbo].[Summary of Sales by Quarter]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Summary of Sales by Quarter]
AS

SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal
FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
WHERE Orders.ShippedDate IS NOT NULL
--ORDER BY Orders.ShippedDate
GO
exec sp_refreshview N'[dbo].[Summary of Sales by Quarter]';
GO

--This SQL is generated for the model defined view [Summary of Sales by Year]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Summary of Sales by Year]') and [xtype] = 'V')
	drop view [dbo].[Summary of Sales by Year]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[Summary of Sales by Year]
AS

SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal
FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
WHERE Orders.ShippedDate IS NOT NULL
--ORDER BY Orders.ShippedDate
GO
exec sp_refreshview N'[dbo].[Summary of Sales by Year]';
GO

