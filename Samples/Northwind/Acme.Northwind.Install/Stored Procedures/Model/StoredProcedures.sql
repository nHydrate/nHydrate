--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Model Version 0.0.0.0

--This SQL is generated for the model defined stored procedure [CustOrderHist]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrderHist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[CustOrderHist]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[CustOrderHist]
(
	@CustomerID nchar(10)  = null
)
AS
SET NOCOUNT ON;

SELECT ProductName, Total=SUM(Quantity)
FROM Products P, [Order Details] OD, Orders O, Customers C
WHERE C.CustomerID = @CustomerID
AND C.CustomerID = O.CustomerID AND O.OrderID = OD.OrderID AND OD.ProductID = P.ProductID
GROUP BY ProductName
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for the model defined stored procedure [CustOrdersDetail]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrdersDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[CustOrdersDetail]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[CustOrdersDetail]
(
	@OrderID int = null
)
AS
SET NOCOUNT ON;

SELECT ProductName,
    UnitPrice=ROUND(Od.UnitPrice, 2),
    Quantity,
    Discount=CONVERT(int, Discount * 100), 
    ExtendedPrice=ROUND(CONVERT(money, Quantity * (1 - Discount) * Od.UnitPrice), 2)
FROM Products P, [Order Details] Od
WHERE Od.ProductID = P.ProductID and Od.OrderID = @OrderID
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for the model defined stored procedure [CustOrdersOrders]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustOrdersOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[CustOrdersOrders]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[CustOrdersOrders]
(
	@CustomerID nchar(10)  = null
)
AS
SET NOCOUNT ON;

SELECT OrderID, 
	OrderDate,
	RequiredDate,
	ShippedDate
FROM Orders
WHERE CustomerID = @CustomerID
ORDER BY OrderID
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for the model defined stored procedure [Employee Sales by Country]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Employee Sales by Country]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[Employee Sales by Country]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[Employee Sales by Country]
(
	@Beginning_Date datetime = null,
	@Ending_Date datetime = null
)
AS
SET NOCOUNT ON;

SELECT Employees.Country, Employees.LastName, Employees.FirstName, Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal AS SaleAmount
FROM Employees INNER JOIN 
	(Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID) 
	ON Employees.EmployeeID = Orders.EmployeeID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for the model defined stored procedure [Sales by Year]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sales by Year]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[Sales by Year]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[Sales by Year]
(
	@Beginning_Date datetime = null,
	@Ending_Date datetime = null
)
AS
SET NOCOUNT ON;

SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal, DATENAME(yy,ShippedDate) AS Year
FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for the model defined stored procedure [SalesByCategory]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesByCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[SalesByCategory]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[SalesByCategory]
(
	@CategoryName nvarchar(30)  = null,
	@OrdYear nvarchar(8)  = null
)
AS
SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for the model defined stored procedure [Ten Most Expensive Products]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ten Most Expensive Products]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[Ten Most Expensive Products]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[Ten Most Expensive Products]
AS
SET NOCOUNT ON;

SET ROWCOUNT 10
SELECT Products.ProductName AS TenMostExpensiveProducts, Products.UnitPrice
FROM Products
ORDER BY Products.UnitPrice DESC
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

