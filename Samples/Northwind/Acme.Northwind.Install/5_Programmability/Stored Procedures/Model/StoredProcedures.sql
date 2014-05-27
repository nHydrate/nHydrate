--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.

--This SQL is generated for the model defined stored procedure [CustOrderHist]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrderHist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CustOrderHist]
--MODELID: 3e92e856-7eb1-49db-9836-f84880f63c5f
GO

CREATE PROCEDURE [dbo].[CustOrderHist]
(
	@CustomerID nchar(10)  = null
)
AS

SELECT ProductName, Total=SUM(Quantity)
FROM Products P, [Order Details] OD, Orders O, Customers C
WHERE C.CustomerID = @CustomerID
AND C.CustomerID = O.CustomerID AND O.OrderID = OD.OrderID AND OD.ProductID = P.ProductID
GROUP BY ProductName
--MODELID,BODY: 3e92e856-7eb1-49db-9836-f84880f63c5f
GO

--This SQL is generated for the model defined stored procedure [CustOrdersDetail]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrdersDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CustOrdersDetail]
--MODELID: fb92431e-3328-4f72-a9a8-344877295169
GO

CREATE PROCEDURE [dbo].[CustOrdersDetail]
(
	@OrderID int = null
)
AS

SELECT ProductName,
    UnitPrice=ROUND(Od.UnitPrice, 2),
    Quantity,
    Discount=CONVERT(int, Discount * 100), 
    ExtendedPrice=ROUND(CONVERT(money, Quantity * (1 - Discount) * Od.UnitPrice), 2)
FROM Products P, [Order Details] Od
WHERE Od.ProductID = P.ProductID and Od.OrderID = @OrderID
--MODELID,BODY: fb92431e-3328-4f72-a9a8-344877295169
GO

--This SQL is generated for the model defined stored procedure [CustOrdersOrders]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrdersOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CustOrdersOrders]
--MODELID: c48495af-4f13-497a-af45-36833c1865c0
GO

CREATE PROCEDURE [dbo].[CustOrdersOrders]
(
	@CustomerID nchar(10)  = null
)
AS

SELECT OrderID, 
	OrderDate,
	RequiredDate,
	ShippedDate
FROM Orders
WHERE CustomerID = @CustomerID
ORDER BY OrderID
--MODELID,BODY: c48495af-4f13-497a-af45-36833c1865c0
GO

--This SQL is generated for the model defined stored procedure [Employee Sales by Country]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Employee Sales by Country]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Employee Sales by Country]
--MODELID: fdd89537-cdbc-41f3-88b7-7f173488aaa7
GO

CREATE PROCEDURE [dbo].[Employee Sales by Country]
(
	@Beginning_Date datetime = null,
	@Ending_Date datetime = null
)
AS

SELECT Employees.Country, Employees.LastName, Employees.FirstName, Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal AS SaleAmount
FROM Employees INNER JOIN 
	(Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID) 
	ON Employees.EmployeeID = Orders.EmployeeID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
--MODELID,BODY: fdd89537-cdbc-41f3-88b7-7f173488aaa7
GO

--This SQL is generated for the model defined stored procedure [Sales by Year]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sales by Year]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sales by Year]
--MODELID: 9cbc747a-7952-446c-aaa0-076927c95ee4
GO

CREATE PROCEDURE [dbo].[Sales by Year]
(
	@Beginning_Date datetime = null,
	@Ending_Date datetime = null
)
AS

SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal, DATENAME(yy,ShippedDate) AS Year
FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
--MODELID,BODY: 9cbc747a-7952-446c-aaa0-076927c95ee4
GO

--This SQL is generated for the model defined stored procedure [SalesByCategory]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesByCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SalesByCategory]
--MODELID: 4a9f019f-a149-4931-adb2-e03377f9076e
GO

CREATE PROCEDURE [dbo].[SalesByCategory]
(
	@CategoryName nvarchar(30)  = null,
	@OrdYear nvarchar(8)  = null
)
AS

IF @OrdYear != '1996' AND @OrdYear != '1997' AND @OrdYear != '1998' 
BEGIN
	SELECT @OrdYear = '1998'
END

SELECT ProductName,
	TotalPurchase=ROUND(SUM(CONVERT(decimal(14,2), OD.Quantity * (1-OD.Discount) * OD.UnitPrice)), 0)
FROM [Order Details] OD, Orders O, Products P, Categories C
WHERE OD.OrderID = O.OrderID 
	AND OD.ProductID = P.ProductID 
	AND P.CategoryID = C.CategoryID
	AND C.CategoryName = @CategoryName
	AND SUBSTRING(CONVERT(nvarchar(22), O.OrderDate, 111), 1, 4) = @OrdYear
GROUP BY ProductName
ORDER BY ProductName
--MODELID,BODY: 4a9f019f-a149-4931-adb2-e03377f9076e
GO

--This SQL is generated for the model defined stored procedure [Ten Most Expensive Products]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ten Most Expensive Products]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ten Most Expensive Products]
--MODELID: c42e7851-fe0f-4fd0-9e52-4e4d819156dd
GO

CREATE PROCEDURE [dbo].[Ten Most Expensive Products]
AS

SET ROWCOUNT 10
SELECT Products.ProductName AS TenMostExpensiveProducts, Products.UnitPrice
FROM Products
ORDER BY Products.UnitPrice DESC
--MODELID,BODY: c42e7851-fe0f-4fd0-9e52-4e4d819156dd
GO

