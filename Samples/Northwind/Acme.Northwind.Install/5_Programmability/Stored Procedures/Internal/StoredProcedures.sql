--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.

--##SECTION BEGIN [INTERNAL STORED PROCS]

--This SQL is generated for internal stored procedures for table [Categories]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Category_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Category_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Category_Delete]
(
	@Original_CategoryID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Categories] 
WHERE 
	[CategoryID] = @Original_CategoryID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Category_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Category_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Category_Insert]
(
	@CategoryID [Int] = null,
	@CategoryName [NVarChar] (15),
	@Description [NText] = null,
	@Picture [Image] = null,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@CategoryID < 0) SET @CategoryID = NULL;
if ((@CategoryID IS NULL))
BEGIN
INSERT INTO [dbo].[Categories]
(
	[CategoryName],
	[Description],
	[Picture],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@CategoryName,
	@Description,
	@Picture,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[Categories] on
INSERT INTO [dbo].[Categories]
(
	[CategoryID],
	[CategoryName],
	[Description],
	[Picture],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@CategoryID,
	@CategoryName,
	@Description,
	@Picture,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[Categories] off
END


SELECT 
	[dbo].[Categories].[categoryid],
	[dbo].[Categories].[categoryname],
	[dbo].[Categories].[description],
	[dbo].[Categories].[picture],
	[dbo].[Categories].[CreatedBy],
	[dbo].[Categories].[CreatedDate],
	[dbo].[Categories].[ModifiedBy],
	[dbo].[Categories].[ModifiedDate],
	[dbo].[Categories].[Timestamp]

FROM
[dbo].[Categories]
WHERE
	[dbo].[Categories].[CategoryID] = SCOPE_IDENTITY();
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Category_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Category_Update]
GO

CREATE PROCEDURE [dbo].[gen_Category_Update]
(
	@CategoryName [NVarChar] (15),
	@Description [NText] = null,
	@Picture [Image] = null,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_CategoryID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Categories] 
SET
	[CategoryName] = @CategoryName,
	[Description] = @Description,
	[Picture] = @Picture,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Categories].[CategoryID] = @Original_CategoryID AND
	[dbo].[Categories].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Categories].[categoryid],
	[dbo].[Categories].[categoryname],
	[dbo].[Categories].[description],
	[dbo].[Categories].[picture],
	[dbo].[Categories].[CreatedBy],
	[dbo].[Categories].[CreatedDate],
	[dbo].[Categories].[ModifiedBy],
	[dbo].[Categories].[ModifiedDate],
	[dbo].[Categories].[Timestamp]
FROM 
[dbo].[Categories]
WHERE
	[dbo].[Categories].[CategoryID] = @Original_CategoryID
GO

--This SQL is generated for internal stored procedures for table [CustomerCustomerDemo]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemo_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemo_Delete]
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemo_Delete]
(
	@customercustomerdemoCustomerDemographics_CustomerTypeID [NChar] (10) = null,--Entity Framework Required Parent Keys be passed in: Table 'CustomerDemographics'
	@Customers_CustomerID [NChar] (5) = null,--Entity Framework Required Parent Keys be passed in: Table 'Customers'
	@Original_CustomerID [NChar] (5),
	@Original_CustomerTypeID [NChar] (10)
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[CustomerCustomerDemo] 
WHERE 
	[CustomerID] = @Original_CustomerID AND
	[CustomerTypeID] = @Original_CustomerTypeID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemo_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemo_Insert]
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemo_Insert]
(
	@CustomerID [NChar] (5),
	@CustomerTypeID [NChar] (10),
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
INSERT INTO [dbo].[CustomerCustomerDemo]
(
	[CustomerID],
	[CustomerTypeID],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@CustomerID,
	@CustomerTypeID,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;



SELECT 
	[dbo].[CustomerCustomerDemo].[customerid],
	[dbo].[CustomerCustomerDemo].[customertypeid],
	[dbo].[CustomerCustomerDemo].[CreatedBy],
	[dbo].[CustomerCustomerDemo].[CreatedDate],
	[dbo].[CustomerCustomerDemo].[ModifiedBy],
	[dbo].[CustomerCustomerDemo].[ModifiedDate],
	[dbo].[CustomerCustomerDemo].[Timestamp]

FROM
[dbo].[CustomerCustomerDemo]
WHERE
	[dbo].[CustomerCustomerDemo].[CustomerID] = @CustomerID AND
	[dbo].[CustomerCustomerDemo].[CustomerTypeID] = @CustomerTypeID;
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemo_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemo_Update]
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemo_Update]
(
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_CustomerID [NChar] (5),
	@Original_CustomerTypeID [NChar] (10),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[CustomerCustomerDemo] 
SET
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[CustomerCustomerDemo].[CustomerID] = @Original_CustomerID AND
	[dbo].[CustomerCustomerDemo].[CustomerTypeID] = @Original_CustomerTypeID AND
	[dbo].[CustomerCustomerDemo].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[CustomerCustomerDemo].[customerid],
	[dbo].[CustomerCustomerDemo].[customertypeid],
	[dbo].[CustomerCustomerDemo].[CreatedBy],
	[dbo].[CustomerCustomerDemo].[CreatedDate],
	[dbo].[CustomerCustomerDemo].[ModifiedBy],
	[dbo].[CustomerCustomerDemo].[ModifiedDate],
	[dbo].[CustomerCustomerDemo].[Timestamp]
FROM 
[dbo].[CustomerCustomerDemo]
WHERE
	[dbo].[CustomerCustomerDemo].[CustomerID] = @Original_CustomerID AND
	[dbo].[CustomerCustomerDemo].[CustomerTypeID] = @Original_CustomerTypeID
GO

--This SQL is generated for internal stored procedures for table [CustomerDemographics]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographic_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographic_Delete]
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographic_Delete]
(
	@Original_CustomerTypeID [NChar] (10)
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[CustomerDemographics] 
WHERE 
	[CustomerTypeID] = @Original_CustomerTypeID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographic_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographic_Insert]
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographic_Insert]
(
	@CustomerDesc [NText] = null,
	@CustomerTypeID [NChar] (10),
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
INSERT INTO [dbo].[CustomerDemographics]
(
	[CustomerDesc],
	[CustomerTypeID],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@CustomerDesc,
	@CustomerTypeID,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;



SELECT 
	[dbo].[CustomerDemographics].[customerdesc],
	[dbo].[CustomerDemographics].[customertypeid],
	[dbo].[CustomerDemographics].[CreatedBy],
	[dbo].[CustomerDemographics].[CreatedDate],
	[dbo].[CustomerDemographics].[ModifiedBy],
	[dbo].[CustomerDemographics].[ModifiedDate],
	[dbo].[CustomerDemographics].[Timestamp]

FROM
[dbo].[CustomerDemographics]
WHERE
	[dbo].[CustomerDemographics].[CustomerTypeID] = @CustomerTypeID;
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographic_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographic_Update]
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographic_Update]
(
	@CustomerDesc [NText] = null,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_CustomerTypeID [NChar] (10),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[CustomerDemographics] 
SET
	[CustomerDesc] = @CustomerDesc,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[CustomerDemographics].[CustomerTypeID] = @Original_CustomerTypeID AND
	[dbo].[CustomerDemographics].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[CustomerDemographics].[customerdesc],
	[dbo].[CustomerDemographics].[customertypeid],
	[dbo].[CustomerDemographics].[CreatedBy],
	[dbo].[CustomerDemographics].[CreatedDate],
	[dbo].[CustomerDemographics].[ModifiedBy],
	[dbo].[CustomerDemographics].[ModifiedDate],
	[dbo].[CustomerDemographics].[Timestamp]
FROM 
[dbo].[CustomerDemographics]
WHERE
	[dbo].[CustomerDemographics].[CustomerTypeID] = @Original_CustomerTypeID
GO

--This SQL is generated for internal stored procedures for table [Customers]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Customer_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Customer_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Customer_Delete]
(
	@Original_CustomerID [NChar] (5)
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Customers] 
WHERE 
	[CustomerID] = @Original_CustomerID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Customer_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Customer_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Customer_Insert]
(
	@Address [NVarChar] (60) = null,
	@City [NVarChar] (15) = null,
	@CompanyName [NVarChar] (40),
	@ContactName [NVarChar] (30) = null,
	@ContactTitle [NVarChar] (30) = null,
	@Country [NVarChar] (15) = null,
	@CustomerID [NChar] (5),
	@Fax [NVarChar] (24) = null,
	@Phone [NVarChar] (24) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
INSERT INTO [dbo].[Customers]
(
	[Address],
	[City],
	[CompanyName],
	[ContactName],
	[ContactTitle],
	[Country],
	[CustomerID],
	[Fax],
	[Phone],
	[PostalCode],
	[Region],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@Address,
	@City,
	@CompanyName,
	@ContactName,
	@ContactTitle,
	@Country,
	@CustomerID,
	@Fax,
	@Phone,
	@PostalCode,
	@Region,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;



SELECT 
	[dbo].[Customers].[address],
	[dbo].[Customers].[city],
	[dbo].[Customers].[companyname],
	[dbo].[Customers].[contactname],
	[dbo].[Customers].[contacttitle],
	[dbo].[Customers].[country],
	[dbo].[Customers].[customerid],
	[dbo].[Customers].[fax],
	[dbo].[Customers].[phone],
	[dbo].[Customers].[postalcode],
	[dbo].[Customers].[region],
	[dbo].[Customers].[CreatedBy],
	[dbo].[Customers].[CreatedDate],
	[dbo].[Customers].[ModifiedBy],
	[dbo].[Customers].[ModifiedDate],
	[dbo].[Customers].[Timestamp]

FROM
[dbo].[Customers]
WHERE
	[dbo].[Customers].[CustomerID] = @CustomerID;
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Customer_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Customer_Update]
GO

CREATE PROCEDURE [dbo].[gen_Customer_Update]
(
	@Address [NVarChar] (60) = null,
	@City [NVarChar] (15) = null,
	@CompanyName [NVarChar] (40),
	@ContactName [NVarChar] (30) = null,
	@ContactTitle [NVarChar] (30) = null,
	@Country [NVarChar] (15) = null,
	@Fax [NVarChar] (24) = null,
	@Phone [NVarChar] (24) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_CustomerID [NChar] (5),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Customers] 
SET
	[Address] = @Address,
	[City] = @City,
	[CompanyName] = @CompanyName,
	[ContactName] = @ContactName,
	[ContactTitle] = @ContactTitle,
	[Country] = @Country,
	[Fax] = @Fax,
	[Phone] = @Phone,
	[PostalCode] = @PostalCode,
	[Region] = @Region,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Customers].[CustomerID] = @Original_CustomerID AND
	[dbo].[Customers].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Customers].[address],
	[dbo].[Customers].[city],
	[dbo].[Customers].[companyname],
	[dbo].[Customers].[contactname],
	[dbo].[Customers].[contacttitle],
	[dbo].[Customers].[country],
	[dbo].[Customers].[customerid],
	[dbo].[Customers].[fax],
	[dbo].[Customers].[phone],
	[dbo].[Customers].[postalcode],
	[dbo].[Customers].[region],
	[dbo].[Customers].[CreatedBy],
	[dbo].[Customers].[CreatedDate],
	[dbo].[Customers].[ModifiedBy],
	[dbo].[Customers].[ModifiedDate],
	[dbo].[Customers].[Timestamp]
FROM 
[dbo].[Customers]
WHERE
	[dbo].[Customers].[CustomerID] = @Original_CustomerID
GO

--This SQL is generated for internal stored procedures for table [Employees]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Employee_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Employee_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Employee_Delete]
(
	@ReportToEmployees_EmployeeID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Employees'
	@Original_EmployeeID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Employees] 
WHERE 
	[EmployeeID] = @Original_EmployeeID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Employee_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Employee_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Employee_Insert]
(
	@Address [NVarChar] (60) = null,
	@BirthDate [DateTime] = null,
	@City [NVarChar] (15) = null,
	@Country [NVarChar] (15) = null,
	@EmployeeID [Int] = null,
	@Extension [NVarChar] (4) = null,
	@FirstName [NVarChar] (10),
	@HireDate [DateTime] = null,
	@HomePhone [NVarChar] (24) = null,
	@LastName [NVarChar] (20),
	@Notes [NText] = null,
	@Photo [Image] = null,
	@PhotoPath [NVarChar] (255) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@ReportsTo [Int] = null,
	@Title [NVarChar] (30) = null,
	@TitleOfCourtesy [NVarChar] (25) = null,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@EmployeeID < 0) SET @EmployeeID = NULL;
if ((@EmployeeID IS NULL))
BEGIN
INSERT INTO [dbo].[Employees]
(
	[Address],
	[BirthDate],
	[City],
	[Country],
	[Extension],
	[FirstName],
	[HireDate],
	[HomePhone],
	[LastName],
	[Notes],
	[Photo],
	[PhotoPath],
	[PostalCode],
	[Region],
	[ReportsTo],
	[Title],
	[TitleOfCourtesy],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@Address,
	@BirthDate,
	@City,
	@Country,
	@Extension,
	@FirstName,
	@HireDate,
	@HomePhone,
	@LastName,
	@Notes,
	@Photo,
	@PhotoPath,
	@PostalCode,
	@Region,
	@ReportsTo,
	@Title,
	@TitleOfCourtesy,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[Employees] on
INSERT INTO [dbo].[Employees]
(
	[EmployeeID],
	[Address],
	[BirthDate],
	[City],
	[Country],
	[Extension],
	[FirstName],
	[HireDate],
	[HomePhone],
	[LastName],
	[Notes],
	[Photo],
	[PhotoPath],
	[PostalCode],
	[Region],
	[ReportsTo],
	[Title],
	[TitleOfCourtesy],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@EmployeeID,
	@Address,
	@BirthDate,
	@City,
	@Country,
	@Extension,
	@FirstName,
	@HireDate,
	@HomePhone,
	@LastName,
	@Notes,
	@Photo,
	@PhotoPath,
	@PostalCode,
	@Region,
	@ReportsTo,
	@Title,
	@TitleOfCourtesy,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[Employees] off
END


SELECT 
	[dbo].[Employees].[address],
	[dbo].[Employees].[birthdate],
	[dbo].[Employees].[city],
	[dbo].[Employees].[country],
	[dbo].[Employees].[employeeid],
	[dbo].[Employees].[extension],
	[dbo].[Employees].[firstname],
	[dbo].[Employees].[hiredate],
	[dbo].[Employees].[homephone],
	[dbo].[Employees].[lastname],
	[dbo].[Employees].[notes],
	[dbo].[Employees].[photo],
	[dbo].[Employees].[photopath],
	[dbo].[Employees].[postalcode],
	[dbo].[Employees].[region],
	[dbo].[Employees].[reportsto],
	[dbo].[Employees].[title],
	[dbo].[Employees].[titleofcourtesy],
	[dbo].[Employees].[CreatedBy],
	[dbo].[Employees].[CreatedDate],
	[dbo].[Employees].[ModifiedBy],
	[dbo].[Employees].[ModifiedDate],
	[dbo].[Employees].[Timestamp]

FROM
[dbo].[Employees]
WHERE
	[dbo].[Employees].[EmployeeID] = SCOPE_IDENTITY();
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Employee_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Employee_Update]
GO

CREATE PROCEDURE [dbo].[gen_Employee_Update]
(
	@Address [NVarChar] (60) = null,
	@BirthDate [DateTime] = null,
	@City [NVarChar] (15) = null,
	@Country [NVarChar] (15) = null,
	@Extension [NVarChar] (4) = null,
	@FirstName [NVarChar] (10),
	@HireDate [DateTime] = null,
	@HomePhone [NVarChar] (24) = null,
	@LastName [NVarChar] (20),
	@Notes [NText] = null,
	@Photo [Image] = null,
	@PhotoPath [NVarChar] (255) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@ReportsTo [Int] = null,
	@Title [NVarChar] (30) = null,
	@TitleOfCourtesy [NVarChar] (25) = null,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_EmployeeID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Employees] 
SET
	[Address] = @Address,
	[BirthDate] = @BirthDate,
	[City] = @City,
	[Country] = @Country,
	[Extension] = @Extension,
	[FirstName] = @FirstName,
	[HireDate] = @HireDate,
	[HomePhone] = @HomePhone,
	[LastName] = @LastName,
	[Notes] = @Notes,
	[Photo] = @Photo,
	[PhotoPath] = @PhotoPath,
	[PostalCode] = @PostalCode,
	[Region] = @Region,
	[ReportsTo] = @ReportsTo,
	[Title] = @Title,
	[TitleOfCourtesy] = @TitleOfCourtesy,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Employees].[EmployeeID] = @Original_EmployeeID AND
	[dbo].[Employees].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Employees].[address],
	[dbo].[Employees].[birthdate],
	[dbo].[Employees].[city],
	[dbo].[Employees].[country],
	[dbo].[Employees].[employeeid],
	[dbo].[Employees].[extension],
	[dbo].[Employees].[firstname],
	[dbo].[Employees].[hiredate],
	[dbo].[Employees].[homephone],
	[dbo].[Employees].[lastname],
	[dbo].[Employees].[notes],
	[dbo].[Employees].[photo],
	[dbo].[Employees].[photopath],
	[dbo].[Employees].[postalcode],
	[dbo].[Employees].[region],
	[dbo].[Employees].[reportsto],
	[dbo].[Employees].[title],
	[dbo].[Employees].[titleofcourtesy],
	[dbo].[Employees].[CreatedBy],
	[dbo].[Employees].[CreatedDate],
	[dbo].[Employees].[ModifiedBy],
	[dbo].[Employees].[ModifiedDate],
	[dbo].[Employees].[Timestamp]
FROM 
[dbo].[Employees]
WHERE
	[dbo].[Employees].[EmployeeID] = @Original_EmployeeID
GO

--This SQL is generated for internal stored procedures for table [EmployeeTerritories]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorie_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorie_Delete]
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorie_Delete]
(
	@Employees_EmployeeID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Employees'
	@Territories_TerritoryID [NVarChar] (20) = null,--Entity Framework Required Parent Keys be passed in: Table 'Territories'
	@Original_EmployeeID [Int],
	@Original_TerritoryID [NVarChar] (20)
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[EmployeeTerritories] 
WHERE 
	[EmployeeID] = @Original_EmployeeID AND
	[TerritoryID] = @Original_TerritoryID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorie_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorie_Insert]
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorie_Insert]
(
	@EmployeeID [Int],
	@TerritoryID [NVarChar] (20),
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
INSERT INTO [dbo].[EmployeeTerritories]
(
	[EmployeeID],
	[TerritoryID],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@EmployeeID,
	@TerritoryID,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;



SELECT 
	[dbo].[EmployeeTerritories].[employeeid],
	[dbo].[EmployeeTerritories].[territoryid],
	[dbo].[EmployeeTerritories].[CreatedBy],
	[dbo].[EmployeeTerritories].[CreatedDate],
	[dbo].[EmployeeTerritories].[ModifiedBy],
	[dbo].[EmployeeTerritories].[ModifiedDate],
	[dbo].[EmployeeTerritories].[Timestamp]

FROM
[dbo].[EmployeeTerritories]
WHERE
	[dbo].[EmployeeTerritories].[EmployeeID] = @EmployeeID AND
	[dbo].[EmployeeTerritories].[TerritoryID] = @TerritoryID;
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorie_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorie_Update]
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorie_Update]
(
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_EmployeeID [Int],
	@Original_TerritoryID [NVarChar] (20),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[EmployeeTerritories] 
SET
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[EmployeeTerritories].[EmployeeID] = @Original_EmployeeID AND
	[dbo].[EmployeeTerritories].[TerritoryID] = @Original_TerritoryID AND
	[dbo].[EmployeeTerritories].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[EmployeeTerritories].[employeeid],
	[dbo].[EmployeeTerritories].[territoryid],
	[dbo].[EmployeeTerritories].[CreatedBy],
	[dbo].[EmployeeTerritories].[CreatedDate],
	[dbo].[EmployeeTerritories].[ModifiedBy],
	[dbo].[EmployeeTerritories].[ModifiedDate],
	[dbo].[EmployeeTerritories].[Timestamp]
FROM 
[dbo].[EmployeeTerritories]
WHERE
	[dbo].[EmployeeTerritories].[EmployeeID] = @Original_EmployeeID AND
	[dbo].[EmployeeTerritories].[TerritoryID] = @Original_TerritoryID
GO

--This SQL is generated for internal stored procedures for table [Order Details]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetail_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetail_Delete]
GO

CREATE PROCEDURE [dbo].[gen_OrderDetail_Delete]
(
	@order_details_ordersOrders_OrderID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Orders'
	@order_details_productsProducts_ProductID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Products'
	@Original_OrderID [Int],
	@Original_ProductID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Order Details] 
WHERE 
	[OrderID] = @Original_OrderID AND
	[ProductID] = @Original_ProductID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetail_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetail_Insert]
GO

CREATE PROCEDURE [dbo].[gen_OrderDetail_Insert]
(
	@Discount [Real] = 0,
	@OrderID [Int],
	@ProductID [Int],
	@Quantity [SmallInt] = 1,
	@UnitPrice [Money] = 0,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
INSERT INTO [dbo].[Order Details]
(
	[Discount],
	[OrderID],
	[ProductID],
	[Quantity],
	[UnitPrice],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@Discount,
	@OrderID,
	@ProductID,
	@Quantity,
	@UnitPrice,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;



SELECT 
	[dbo].[Order Details].[discount],
	[dbo].[Order Details].[orderid],
	[dbo].[Order Details].[productid],
	[dbo].[Order Details].[quantity],
	[dbo].[Order Details].[unitprice],
	[dbo].[Order Details].[CreatedBy],
	[dbo].[Order Details].[CreatedDate],
	[dbo].[Order Details].[ModifiedBy],
	[dbo].[Order Details].[ModifiedDate],
	[dbo].[Order Details].[Timestamp]

FROM
[dbo].[Order Details]
WHERE
	[dbo].[Order Details].[OrderID] = @OrderID AND
	[dbo].[Order Details].[ProductID] = @ProductID;
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetail_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetail_Update]
GO

CREATE PROCEDURE [dbo].[gen_OrderDetail_Update]
(
	@Discount [Real] = 0,
	@Quantity [SmallInt] = 1,
	@UnitPrice [Money] = 0,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_OrderID [Int],
	@Original_ProductID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Order Details] 
SET
	[Discount] = @Discount,
	[Quantity] = @Quantity,
	[UnitPrice] = @UnitPrice,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Order Details].[OrderID] = @Original_OrderID AND
	[dbo].[Order Details].[ProductID] = @Original_ProductID AND
	[dbo].[Order Details].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Order Details].[discount],
	[dbo].[Order Details].[orderid],
	[dbo].[Order Details].[productid],
	[dbo].[Order Details].[quantity],
	[dbo].[Order Details].[unitprice],
	[dbo].[Order Details].[CreatedBy],
	[dbo].[Order Details].[CreatedDate],
	[dbo].[Order Details].[ModifiedBy],
	[dbo].[Order Details].[ModifiedDate],
	[dbo].[Order Details].[Timestamp]
FROM 
[dbo].[Order Details]
WHERE
	[dbo].[Order Details].[OrderID] = @Original_OrderID AND
	[dbo].[Order Details].[ProductID] = @Original_ProductID
GO

--This SQL is generated for internal stored procedures for table [Orders]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Order_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Order_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Order_Delete]
(
	@Customers_CustomerID [NChar] (5) = null,--Entity Framework Required Parent Keys be passed in: Table 'Customers'
	@Employees_EmployeeID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Employees'
	@Shippers_ShipperID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Shippers'
	@Original_OrderID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Orders] 
WHERE 
	[OrderID] = @Original_OrderID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Order_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Order_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Order_Insert]
(
	@CustomerID [NChar] (5) = null,
	@EmployeeID [Int] = null,
	@Freight [Money] = 0,
	@OrderDate [DateTime] = null,
	@OrderID [Int] = null,
	@RequiredDate [DateTime] = null,
	@ShipAddress [NVarChar] (60) = null,
	@ShipCity [NVarChar] (15) = null,
	@ShipCountry [NVarChar] (15) = null,
	@ShipName [NVarChar] (40) = null,
	@ShippedDate [DateTime] = null,
	@ShipPostalCode [NVarChar] (10) = null,
	@ShipRegion [NVarChar] (15) = null,
	@ShipVia [Int] = null,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@OrderID < 0) SET @OrderID = NULL;
if ((@OrderID IS NULL))
BEGIN
INSERT INTO [dbo].[Orders]
(
	[CustomerID],
	[EmployeeID],
	[Freight],
	[OrderDate],
	[RequiredDate],
	[ShipAddress],
	[ShipCity],
	[ShipCountry],
	[ShipName],
	[ShippedDate],
	[ShipPostalCode],
	[ShipRegion],
	[ShipVia],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@CustomerID,
	@EmployeeID,
	@Freight,
	@OrderDate,
	@RequiredDate,
	@ShipAddress,
	@ShipCity,
	@ShipCountry,
	@ShipName,
	@ShippedDate,
	@ShipPostalCode,
	@ShipRegion,
	@ShipVia,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[Orders] on
INSERT INTO [dbo].[Orders]
(
	[OrderID],
	[CustomerID],
	[EmployeeID],
	[Freight],
	[OrderDate],
	[RequiredDate],
	[ShipAddress],
	[ShipCity],
	[ShipCountry],
	[ShipName],
	[ShippedDate],
	[ShipPostalCode],
	[ShipRegion],
	[ShipVia],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@OrderID,
	@CustomerID,
	@EmployeeID,
	@Freight,
	@OrderDate,
	@RequiredDate,
	@ShipAddress,
	@ShipCity,
	@ShipCountry,
	@ShipName,
	@ShippedDate,
	@ShipPostalCode,
	@ShipRegion,
	@ShipVia,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[Orders] off
END


SELECT 
	[dbo].[Orders].[customerid],
	[dbo].[Orders].[employeeid],
	[dbo].[Orders].[freight],
	[dbo].[Orders].[orderdate],
	[dbo].[Orders].[orderid],
	[dbo].[Orders].[requireddate],
	[dbo].[Orders].[shipaddress],
	[dbo].[Orders].[shipcity],
	[dbo].[Orders].[shipcountry],
	[dbo].[Orders].[shipname],
	[dbo].[Orders].[shippeddate],
	[dbo].[Orders].[shippostalcode],
	[dbo].[Orders].[shipregion],
	[dbo].[Orders].[shipvia],
	[dbo].[Orders].[CreatedBy],
	[dbo].[Orders].[CreatedDate],
	[dbo].[Orders].[ModifiedBy],
	[dbo].[Orders].[ModifiedDate],
	[dbo].[Orders].[Timestamp]

FROM
[dbo].[Orders]
WHERE
	[dbo].[Orders].[OrderID] = SCOPE_IDENTITY();
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Order_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Order_Update]
GO

CREATE PROCEDURE [dbo].[gen_Order_Update]
(
	@CustomerID [NChar] (5) = null,
	@EmployeeID [Int] = null,
	@Freight [Money] = 0,
	@OrderDate [DateTime] = null,
	@RequiredDate [DateTime] = null,
	@ShipAddress [NVarChar] (60) = null,
	@ShipCity [NVarChar] (15) = null,
	@ShipCountry [NVarChar] (15) = null,
	@ShipName [NVarChar] (40) = null,
	@ShippedDate [DateTime] = null,
	@ShipPostalCode [NVarChar] (10) = null,
	@ShipRegion [NVarChar] (15) = null,
	@ShipVia [Int] = null,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_OrderID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Orders] 
SET
	[CustomerID] = @CustomerID,
	[EmployeeID] = @EmployeeID,
	[Freight] = @Freight,
	[OrderDate] = @OrderDate,
	[RequiredDate] = @RequiredDate,
	[ShipAddress] = @ShipAddress,
	[ShipCity] = @ShipCity,
	[ShipCountry] = @ShipCountry,
	[ShipName] = @ShipName,
	[ShippedDate] = @ShippedDate,
	[ShipPostalCode] = @ShipPostalCode,
	[ShipRegion] = @ShipRegion,
	[ShipVia] = @ShipVia,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Orders].[OrderID] = @Original_OrderID AND
	[dbo].[Orders].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Orders].[customerid],
	[dbo].[Orders].[employeeid],
	[dbo].[Orders].[freight],
	[dbo].[Orders].[orderdate],
	[dbo].[Orders].[orderid],
	[dbo].[Orders].[requireddate],
	[dbo].[Orders].[shipaddress],
	[dbo].[Orders].[shipcity],
	[dbo].[Orders].[shipcountry],
	[dbo].[Orders].[shipname],
	[dbo].[Orders].[shippeddate],
	[dbo].[Orders].[shippostalcode],
	[dbo].[Orders].[shipregion],
	[dbo].[Orders].[shipvia],
	[dbo].[Orders].[CreatedBy],
	[dbo].[Orders].[CreatedDate],
	[dbo].[Orders].[ModifiedBy],
	[dbo].[Orders].[ModifiedDate],
	[dbo].[Orders].[Timestamp]
FROM 
[dbo].[Orders]
WHERE
	[dbo].[Orders].[OrderID] = @Original_OrderID
GO

--This SQL is generated for internal stored procedures for table [Products]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Product_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Product_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Product_Delete]
(
	@Categories_CategoryID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Categories'
	@Suppliers_SupplierID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Suppliers'
	@Original_ProductID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Products] 
WHERE 
	[ProductID] = @Original_ProductID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Product_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Product_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Product_Insert]
(
	@CategoryID [Int] = null,
	@Discontinued [Bit] = 0,
	@ProductID [Int] = null,
	@ProductName [NVarChar] (40),
	@QuantityPerUnit [NVarChar] (20) = null,
	@ReorderLevel [SmallInt] = 0,
	@SupplierID [Int] = null,
	@UnitPrice [Money] = 0,
	@UnitsInStock [SmallInt] = 0,
	@UnitsOnOrder [SmallInt] = 0,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@ProductID < 0) SET @ProductID = NULL;
if ((@ProductID IS NULL))
BEGIN
INSERT INTO [dbo].[Products]
(
	[CategoryID],
	[Discontinued],
	[ProductName],
	[QuantityPerUnit],
	[ReorderLevel],
	[SupplierID],
	[UnitPrice],
	[UnitsInStock],
	[UnitsOnOrder],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@CategoryID,
	@Discontinued,
	@ProductName,
	@QuantityPerUnit,
	@ReorderLevel,
	@SupplierID,
	@UnitPrice,
	@UnitsInStock,
	@UnitsOnOrder,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[Products] on
INSERT INTO [dbo].[Products]
(
	[ProductID],
	[CategoryID],
	[Discontinued],
	[ProductName],
	[QuantityPerUnit],
	[ReorderLevel],
	[SupplierID],
	[UnitPrice],
	[UnitsInStock],
	[UnitsOnOrder],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@ProductID,
	@CategoryID,
	@Discontinued,
	@ProductName,
	@QuantityPerUnit,
	@ReorderLevel,
	@SupplierID,
	@UnitPrice,
	@UnitsInStock,
	@UnitsOnOrder,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[Products] off
END


SELECT 
	[dbo].[Products].[categoryid],
	[dbo].[Products].[discontinued],
	[dbo].[Products].[productid],
	[dbo].[Products].[productname],
	[dbo].[Products].[quantityperunit],
	[dbo].[Products].[reorderlevel],
	[dbo].[Products].[supplierid],
	[dbo].[Products].[unitprice],
	[dbo].[Products].[unitsinstock],
	[dbo].[Products].[unitsonorder],
	[dbo].[Products].[CreatedBy],
	[dbo].[Products].[CreatedDate],
	[dbo].[Products].[ModifiedBy],
	[dbo].[Products].[ModifiedDate],
	[dbo].[Products].[Timestamp]

FROM
[dbo].[Products]
WHERE
	[dbo].[Products].[ProductID] = SCOPE_IDENTITY();
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Product_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Product_Update]
GO

CREATE PROCEDURE [dbo].[gen_Product_Update]
(
	@CategoryID [Int] = null,
	@Discontinued [Bit] = 0,
	@ProductName [NVarChar] (40),
	@QuantityPerUnit [NVarChar] (20) = null,
	@ReorderLevel [SmallInt] = 0,
	@SupplierID [Int] = null,
	@UnitPrice [Money] = 0,
	@UnitsInStock [SmallInt] = 0,
	@UnitsOnOrder [SmallInt] = 0,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_ProductID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Products] 
SET
	[CategoryID] = @CategoryID,
	[Discontinued] = @Discontinued,
	[ProductName] = @ProductName,
	[QuantityPerUnit] = @QuantityPerUnit,
	[ReorderLevel] = @ReorderLevel,
	[SupplierID] = @SupplierID,
	[UnitPrice] = @UnitPrice,
	[UnitsInStock] = @UnitsInStock,
	[UnitsOnOrder] = @UnitsOnOrder,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Products].[ProductID] = @Original_ProductID AND
	[dbo].[Products].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Products].[categoryid],
	[dbo].[Products].[discontinued],
	[dbo].[Products].[productid],
	[dbo].[Products].[productname],
	[dbo].[Products].[quantityperunit],
	[dbo].[Products].[reorderlevel],
	[dbo].[Products].[supplierid],
	[dbo].[Products].[unitprice],
	[dbo].[Products].[unitsinstock],
	[dbo].[Products].[unitsonorder],
	[dbo].[Products].[CreatedBy],
	[dbo].[Products].[CreatedDate],
	[dbo].[Products].[ModifiedBy],
	[dbo].[Products].[ModifiedDate],
	[dbo].[Products].[Timestamp]
FROM 
[dbo].[Products]
WHERE
	[dbo].[Products].[ProductID] = @Original_ProductID
GO

--This SQL is generated for internal stored procedures for table [Region]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Region_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Region_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Region_Delete]
(
	@Original_RegionID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Region] 
WHERE 
	[RegionID] = @Original_RegionID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Region_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Region_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Region_Insert]
(
	@RegionDescription [NChar] (50),
	@RegionID [Int],
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
INSERT INTO [dbo].[Region]
(
	[RegionDescription],
	[RegionID],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@RegionDescription,
	@RegionID,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;



SELECT 
	[dbo].[Region].[regiondescription],
	[dbo].[Region].[regionid],
	[dbo].[Region].[CreatedBy],
	[dbo].[Region].[CreatedDate],
	[dbo].[Region].[ModifiedBy],
	[dbo].[Region].[ModifiedDate],
	[dbo].[Region].[Timestamp]

FROM
[dbo].[Region]
WHERE
	[dbo].[Region].[RegionID] = @RegionID;
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Region_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Region_Update]
GO

CREATE PROCEDURE [dbo].[gen_Region_Update]
(
	@RegionDescription [NChar] (50),
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_RegionID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Region] 
SET
	[RegionDescription] = @RegionDescription,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Region].[RegionID] = @Original_RegionID AND
	[dbo].[Region].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Region].[regiondescription],
	[dbo].[Region].[regionid],
	[dbo].[Region].[CreatedBy],
	[dbo].[Region].[CreatedDate],
	[dbo].[Region].[ModifiedBy],
	[dbo].[Region].[ModifiedDate],
	[dbo].[Region].[Timestamp]
FROM 
[dbo].[Region]
WHERE
	[dbo].[Region].[RegionID] = @Original_RegionID
GO

--This SQL is generated for internal stored procedures for table [Shippers]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Shipper_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Shipper_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Shipper_Delete]
(
	@Original_ShipperID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Shippers] 
WHERE 
	[ShipperID] = @Original_ShipperID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Shipper_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Shipper_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Shipper_Insert]
(
	@CompanyName [NVarChar] (40),
	@Phone [NVarChar] (24) = null,
	@ShipperID [Int] = null,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@ShipperID < 0) SET @ShipperID = NULL;
if ((@ShipperID IS NULL))
BEGIN
INSERT INTO [dbo].[Shippers]
(
	[CompanyName],
	[Phone],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@CompanyName,
	@Phone,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[Shippers] on
INSERT INTO [dbo].[Shippers]
(
	[ShipperID],
	[CompanyName],
	[Phone],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@ShipperID,
	@CompanyName,
	@Phone,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[Shippers] off
END


SELECT 
	[dbo].[Shippers].[companyname],
	[dbo].[Shippers].[phone],
	[dbo].[Shippers].[shipperid],
	[dbo].[Shippers].[CreatedBy],
	[dbo].[Shippers].[CreatedDate],
	[dbo].[Shippers].[ModifiedBy],
	[dbo].[Shippers].[ModifiedDate],
	[dbo].[Shippers].[Timestamp]

FROM
[dbo].[Shippers]
WHERE
	[dbo].[Shippers].[ShipperID] = SCOPE_IDENTITY();
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Shipper_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Shipper_Update]
GO

CREATE PROCEDURE [dbo].[gen_Shipper_Update]
(
	@CompanyName [NVarChar] (40),
	@Phone [NVarChar] (24) = null,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_ShipperID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Shippers] 
SET
	[CompanyName] = @CompanyName,
	[Phone] = @Phone,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Shippers].[ShipperID] = @Original_ShipperID AND
	[dbo].[Shippers].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Shippers].[companyname],
	[dbo].[Shippers].[phone],
	[dbo].[Shippers].[shipperid],
	[dbo].[Shippers].[CreatedBy],
	[dbo].[Shippers].[CreatedDate],
	[dbo].[Shippers].[ModifiedBy],
	[dbo].[Shippers].[ModifiedDate],
	[dbo].[Shippers].[Timestamp]
FROM 
[dbo].[Shippers]
WHERE
	[dbo].[Shippers].[ShipperID] = @Original_ShipperID
GO

--This SQL is generated for internal stored procedures for table [Suppliers]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Supplier_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Supplier_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Supplier_Delete]
(
	@Original_SupplierID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Suppliers] 
WHERE 
	[SupplierID] = @Original_SupplierID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Supplier_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Supplier_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Supplier_Insert]
(
	@Address [NVarChar] (60) = null,
	@City [NVarChar] (15) = null,
	@CompanyName [NVarChar] (40),
	@ContactName [NVarChar] (30) = null,
	@ContactTitle [NVarChar] (30) = null,
	@Country [NVarChar] (15) = null,
	@Fax [NVarChar] (24) = null,
	@HomePage [NText] = null,
	@Phone [NVarChar] (24) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@SupplierID [Int] = null,
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
IF (@SupplierID < 0) SET @SupplierID = NULL;
if ((@SupplierID IS NULL))
BEGIN
INSERT INTO [dbo].[Suppliers]
(
	[Address],
	[City],
	[CompanyName],
	[ContactName],
	[ContactTitle],
	[Country],
	[Fax],
	[HomePage],
	[Phone],
	[PostalCode],
	[Region],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@Address,
	@City,
	@CompanyName,
	@ContactName,
	@ContactTitle,
	@Country,
	@Fax,
	@HomePage,
	@Phone,
	@PostalCode,
	@Region,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

END
ELSE
BEGIN
SET identity_insert [dbo].[Suppliers] on
INSERT INTO [dbo].[Suppliers]
(
	[SupplierID],
	[Address],
	[City],
	[CompanyName],
	[ContactName],
	[ContactTitle],
	[Country],
	[Fax],
	[HomePage],
	[Phone],
	[PostalCode],
	[Region],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@SupplierID,
	@Address,
	@City,
	@CompanyName,
	@ContactName,
	@ContactTitle,
	@Country,
	@Fax,
	@HomePage,
	@Phone,
	@PostalCode,
	@Region,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;

SET identity_insert [dbo].[Suppliers] off
END


SELECT 
	[dbo].[Suppliers].[address],
	[dbo].[Suppliers].[city],
	[dbo].[Suppliers].[companyname],
	[dbo].[Suppliers].[contactname],
	[dbo].[Suppliers].[contacttitle],
	[dbo].[Suppliers].[country],
	[dbo].[Suppliers].[fax],
	[dbo].[Suppliers].[homepage],
	[dbo].[Suppliers].[phone],
	[dbo].[Suppliers].[postalcode],
	[dbo].[Suppliers].[region],
	[dbo].[Suppliers].[supplierid],
	[dbo].[Suppliers].[CreatedBy],
	[dbo].[Suppliers].[CreatedDate],
	[dbo].[Suppliers].[ModifiedBy],
	[dbo].[Suppliers].[ModifiedDate],
	[dbo].[Suppliers].[Timestamp]

FROM
[dbo].[Suppliers]
WHERE
	[dbo].[Suppliers].[SupplierID] = SCOPE_IDENTITY();
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Supplier_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Supplier_Update]
GO

CREATE PROCEDURE [dbo].[gen_Supplier_Update]
(
	@Address [NVarChar] (60) = null,
	@City [NVarChar] (15) = null,
	@CompanyName [NVarChar] (40),
	@ContactName [NVarChar] (30) = null,
	@ContactTitle [NVarChar] (30) = null,
	@Country [NVarChar] (15) = null,
	@Fax [NVarChar] (24) = null,
	@HomePage [NText] = null,
	@Phone [NVarChar] (24) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_SupplierID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Suppliers] 
SET
	[Address] = @Address,
	[City] = @City,
	[CompanyName] = @CompanyName,
	[ContactName] = @ContactName,
	[ContactTitle] = @ContactTitle,
	[Country] = @Country,
	[Fax] = @Fax,
	[HomePage] = @HomePage,
	[Phone] = @Phone,
	[PostalCode] = @PostalCode,
	[Region] = @Region,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Suppliers].[SupplierID] = @Original_SupplierID AND
	[dbo].[Suppliers].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Suppliers].[address],
	[dbo].[Suppliers].[city],
	[dbo].[Suppliers].[companyname],
	[dbo].[Suppliers].[contactname],
	[dbo].[Suppliers].[contacttitle],
	[dbo].[Suppliers].[country],
	[dbo].[Suppliers].[fax],
	[dbo].[Suppliers].[homepage],
	[dbo].[Suppliers].[phone],
	[dbo].[Suppliers].[postalcode],
	[dbo].[Suppliers].[region],
	[dbo].[Suppliers].[supplierid],
	[dbo].[Suppliers].[CreatedBy],
	[dbo].[Suppliers].[CreatedDate],
	[dbo].[Suppliers].[ModifiedBy],
	[dbo].[Suppliers].[ModifiedDate],
	[dbo].[Suppliers].[Timestamp]
FROM 
[dbo].[Suppliers]
WHERE
	[dbo].[Suppliers].[SupplierID] = @Original_SupplierID
GO

--This SQL is generated for internal stored procedures for table [Territories]
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Territory_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Territory_Delete]
GO

CREATE PROCEDURE [dbo].[gen_Territory_Delete]
(
	@Region_RegionID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Region'
	@Original_TerritoryID [NVarChar] (20)
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Territories] 
WHERE 
	[TerritoryID] = @Original_TerritoryID;

if (@@RowCount = 0) return;


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Territory_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Territory_Insert]
GO

CREATE PROCEDURE [dbo].[gen_Territory_Insert]
(
	@RegionID [Int],
	@TerritoryDescription [NChar] (50),
	@TerritoryID [NVarChar] (20),
	@CreatedDate [DateTime] = null,
	@CreatedBy [Varchar] (50) = null,
	@ModifiedBy [Varchar] (50) = null

)
AS
SET NOCOUNT OFF;

if (@CreatedDate IS NULL)
SET @CreatedDate = getdate()
DECLARE @ModifiedDate [DateTime]
SET @ModifiedDate = getdate()
INSERT INTO [dbo].[Territories]
(
	[RegionID],
	[TerritoryDescription],
	[TerritoryID],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
VALUES
(
	@RegionID,
	@TerritoryDescription,
	@TerritoryID,
	@CreatedDate,
	@CreatedBy,
	@ModifiedDate,
	@ModifiedBy
);

if (@@RowCount = 0) return;



SELECT 
	[dbo].[Territories].[regionid],
	[dbo].[Territories].[territorydescription],
	[dbo].[Territories].[territoryid],
	[dbo].[Territories].[CreatedBy],
	[dbo].[Territories].[CreatedDate],
	[dbo].[Territories].[ModifiedBy],
	[dbo].[Territories].[ModifiedDate],
	[dbo].[Territories].[Timestamp]

FROM
[dbo].[Territories]
WHERE
	[dbo].[Territories].[TerritoryID] = @TerritoryID;
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_Territory_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_Territory_Update]
GO

CREATE PROCEDURE [dbo].[gen_Territory_Update]
(
	@RegionID [Int],
	@TerritoryDescription [NChar] (50),
	@ModifiedBy [Varchar] (50) = null,
	@ModifiedDate [DateTime] = null,
	@Original_TerritoryID [NVarChar] (20),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT ON;
UPDATE 
	[dbo].[Territories] 
SET
	[RegionID] = @RegionID,
	[TerritoryDescription] = @TerritoryDescription,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Territories].[TerritoryID] = @Original_TerritoryID AND
	[dbo].[Territories].[Timestamp] = @Original_Timestamp


if (@@RowCount = 0) return;

SELECT
	[dbo].[Territories].[regionid],
	[dbo].[Territories].[territorydescription],
	[dbo].[Territories].[territoryid],
	[dbo].[Territories].[CreatedBy],
	[dbo].[Territories].[CreatedDate],
	[dbo].[Territories].[ModifiedBy],
	[dbo].[Territories].[ModifiedDate],
	[dbo].[Territories].[Timestamp]
FROM 
[dbo].[Territories]
WHERE
	[dbo].[Territories].[TerritoryID] = @Original_TerritoryID
GO

--##SECTION END [INTERNAL STORED PROCS]

