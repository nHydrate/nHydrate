--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Model Version 0.0.0.0

--This SQL is generated for internal stored procedures for table [Categories]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CategoryDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CategoryDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CategoryDelete]
(
	@Original_CategoryID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Categories] 
WHERE 
	[CategoryID] = @Original_CategoryID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CategorySelectByCategoryPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CategorySelectByCategoryPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_CategorySelectByCategoryPks]
(
	@xml xml
)
AS

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

	[dbo].[Categories].[CategoryID] IN (SELECT T.c.value('./CategoryID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CategorySelectByCategorySinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CategorySelectByCategorySinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CategorySelectByCategorySinglePk]
(
	@CategoryID [Int]
)
AS

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
[dbo].[Categories].[CategoryID] = @CategoryID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CategoryInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CategoryInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CategoryInsert]
(
	@CategoryID [Int] = null,
	@CategoryName [NVarChar] (15) = null,
	@Description [NText] = null,
	@Picture [Image] = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CategoryUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CategoryUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CategoryUpdate]
(
	@CategoryName [NVarChar] (15),
	@Description [NText],
	@Picture [Image],
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_CategoryID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CategorySelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CategorySelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CategorySelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Categories].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Categories].[CreatedDate])) AND 
((([dbo].[Categories].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Categories].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CategorySelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CategorySelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CategorySelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Categories].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Categories].[ModifiedDate])) AND 
((([Categories].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Categories].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CategorySelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CategorySelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CategorySelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [CustomerCustomerDemo]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoDelete]
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
	[CustomerTypeID] = @Original_CustomerTypeID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoSelectByCustomerCustomerDemoPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoSelectByCustomerCustomerDemoPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoSelectByCustomerCustomerDemoPks]
(
	@xml xml
)
AS

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

	[dbo].[CustomerCustomerDemo].[CustomerID] IN (SELECT T.c.value('./CustomerID[1]', 'NChar (5)')
		FROM @xml.nodes('//Item') T(c))

	AND

	[dbo].[CustomerCustomerDemo].[CustomerTypeID] IN (SELECT T.c.value('./CustomerTypeID[1]', 'NChar (10)')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoSelectByCustomerCustomerDemoSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoSelectByCustomerCustomerDemoSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoSelectByCustomerCustomerDemoSinglePk]
(
	@CustomerID [NChar] (5),
	@CustomerTypeID [NChar] (10)
)
AS

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
[dbo].[CustomerCustomerDemo].[CustomerID] = @CustomerID AND [dbo].[CustomerCustomerDemo].[CustomerTypeID] = @CustomerTypeID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoInsert]
(
	@CustomerID [NChar] (5) = null,
	@CustomerTypeID [NChar] (10) = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoUpdate]
(
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_CustomerID [NChar] (5),
	@Original_CustomerTypeID [NChar] (10),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
UPDATE 
	[dbo].[CustomerCustomerDemo] 
SET
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[CustomerCustomerDemo].[CustomerID] = @Original_CustomerID AND
	[dbo].[CustomerCustomerDemo].[CustomerTypeID] = @Original_CustomerTypeID AND
	[dbo].[CustomerCustomerDemo].[Timestamp] = @Original_Timestamp


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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[CustomerCustomerDemo].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[CustomerCustomerDemo].[CreatedDate])) AND 
((([dbo].[CustomerCustomerDemo].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[CustomerCustomerDemo].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([CustomerCustomerDemo].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [CustomerCustomerDemo].[ModifiedDate])) AND 
((([CustomerCustomerDemo].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [CustomerCustomerDemo].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoSelectByCustomercustomerdemoCustomerDemographicPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoSelectByCustomercustomerdemoCustomerDemographicPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoSelectByCustomercustomerdemoCustomerDemographicPks]
(
	@xml xml
)
AS


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
	[dbo].[CustomerCustomerDemo].[CustomerTypeID] IN (SELECT T.c.value('./CustomerTypeID[1]', 'NChar (10)')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerCustomerDemoSelectByCustomerPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerCustomerDemoSelectByCustomerPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_CustomerCustomerDemoSelectByCustomerPks]
(
	@xml xml
)
AS


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
	[dbo].[CustomerCustomerDemo].[CustomerID] IN (SELECT T.c.value('./CustomerID[1]', 'NChar (5)')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [CustomerDemographics]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographicDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographicDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographicDelete]
(
	@Original_CustomerTypeID [NChar] (10)
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[CustomerDemographics] 
WHERE 
	[CustomerTypeID] = @Original_CustomerTypeID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographicSelectByCustomerDemographicPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographicSelectByCustomerDemographicPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_CustomerDemographicSelectByCustomerDemographicPks]
(
	@xml xml
)
AS

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

	[dbo].[CustomerDemographics].[CustomerTypeID] IN (SELECT T.c.value('./CustomerTypeID[1]', 'NChar (10)')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographicSelectByCustomerDemographicSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographicSelectByCustomerDemographicSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographicSelectByCustomerDemographicSinglePk]
(
	@CustomerTypeID [NChar] (10)
)
AS

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
[dbo].[CustomerDemographics].[CustomerTypeID] = @CustomerTypeID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographicInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographicInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographicInsert]
(
	@CustomerDesc [NText] = null,
	@CustomerTypeID [NChar] (10) = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographicUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographicUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographicUpdate]
(
	@CustomerDesc [NText],
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_CustomerTypeID [NChar] (10),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
UPDATE 
	[dbo].[CustomerDemographics] 
SET
	[CustomerDesc] = @CustomerDesc,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[CustomerDemographics].[CustomerTypeID] = @Original_CustomerTypeID AND
	[dbo].[CustomerDemographics].[Timestamp] = @Original_Timestamp


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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographicSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographicSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographicSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[CustomerDemographics].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[CustomerDemographics].[CreatedDate])) AND 
((([dbo].[CustomerDemographics].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[CustomerDemographics].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographicSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographicSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographicSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([CustomerDemographics].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [CustomerDemographics].[ModifiedDate])) AND 
((([CustomerDemographics].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [CustomerDemographics].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDemographicSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDemographicSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerDemographicSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Customers]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerDelete]
(
	@Original_CustomerID [NChar] (5)
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Customers] 
WHERE 
	[CustomerID] = @Original_CustomerID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerSelectByCustomerPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerSelectByCustomerPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_CustomerSelectByCustomerPks]
(
	@xml xml
)
AS

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

	[dbo].[Customers].[CustomerID] IN (SELECT T.c.value('./CustomerID[1]', 'NChar (5)')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerSelectByCustomerSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerSelectByCustomerSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerSelectByCustomerSinglePk]
(
	@CustomerID [NChar] (5)
)
AS

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
[dbo].[Customers].[CustomerID] = @CustomerID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerInsert]
(
	@Address [NVarChar] (60) = null,
	@City [NVarChar] (15) = null,
	@CompanyName [NVarChar] (40) = null,
	@ContactName [NVarChar] (30) = null,
	@ContactTitle [NVarChar] (30) = null,
	@Country [NVarChar] (15) = null,
	@CustomerID [NChar] (5) = null,
	@Fax [NVarChar] (24) = null,
	@Phone [NVarChar] (24) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerUpdate]
(
	@Address [NVarChar] (60),
	@City [NVarChar] (15),
	@CompanyName [NVarChar] (40),
	@ContactName [NVarChar] (30),
	@ContactTitle [NVarChar] (30),
	@Country [NVarChar] (15),
	@Fax [NVarChar] (24),
	@Phone [NVarChar] (24),
	@PostalCode [NVarChar] (10),
	@Region [NVarChar] (15),
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_CustomerID [NChar] (5),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Customers].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Customers].[CreatedDate])) AND 
((([dbo].[Customers].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Customers].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Customers].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Customers].[ModifiedDate])) AND 
((([Customers].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Customers].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_CustomerSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_CustomerSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_CustomerSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Employees]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeDelete]
(
	@ReportToEmployees_EmployeeID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Employees'
	@Original_EmployeeID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Employees] 
WHERE 
	[EmployeeID] = @Original_EmployeeID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeSelectByEmployeePks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeSelectByEmployeePks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_EmployeeSelectByEmployeePks]
(
	@xml xml,
	@direction char(4),
	@levels int
)
AS

DECLARE @newItemCount int
DECLARE @count int
DECLARE @index int

CREATE TABLE #TmpIds ([EmployeeID] [Int])


INSERT INTO #TmpIds([EmployeeID])
SELECT
T.c.value('./EmployeeID[1]', 'Int') [EmployeeID]
FROM @xml.nodes('//Item') T(c)

IF (@direction = 'NONE')
BEGIN
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

	[dbo].[Employees].[EmployeeID] IN (SELECT T.c.value('./EmployeeID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))


END
ELSE
BEGIN
IF (@direction = 'DOWN' OR @direction = 'BOTH')
BEGIN
	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)
	SET @count = @newItemCount
	SET @index = 1
	WHILE(@newItemCount > 0 and @index <= @levels)
	BEGIN
		INSERT INTO #TmpIds([EmployeeID])
		SELECT
			[Employee].[ReportsTo]

		FROM 
			[Employee]
			INNER JOIN #TmpIds ON 
[Employees].[ReportsTo] = #TmpIds.[EmployeeID]
		WHERE
			[dbo].[Employee].[ReportsTo] NOT IN (SELECT [EmployeeID] from #TmpIds)


		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count
		SET @count = (SELECT COUNT(*) FROM #TmpIds)
		SET @index = @index + 1
	END
END
IF (@direction = 'UP' OR @direction = 'BOTH')
BEGIN
	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)
	SET @count = @newItemCount
	SET @index = 1
	WHILE(@newItemCount > 0 and @index <= @levels)
	BEGIN
		INSERT INTO #TmpIds([ReportsTo])
		SELECT
	[Employees].[ReportsTo]
		FROM #TmpIds INNER JOIN [Employees] ON [Employees].[ReportsTo] = #TmpIds.[EmployeeID]
		WHERE
			[dbo].[Employees].[ReportsTo] NOT IN (SELECT [EmployeeID] from #TmpIds)

		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count
		SET @count = (SELECT COUNT(*) FROM #TmpIds)
		SET @index = @index + 1
	END
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
			([dbo].[Employees].[ReportsTo] IN (SELECT [EmployeeID] from #TmpIds))

END
DROP TABLE #TmpIds


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeSelectByEmployeeSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeSelectByEmployeeSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeSelectByEmployeeSinglePk]
(
	@EmployeeID [Int]
)
AS

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
[dbo].[Employees].[EmployeeID] = @EmployeeID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeInsert]
(
	@Address [NVarChar] (60) = null,
	@BirthDate [DateTime] = null,
	@City [NVarChar] (15) = null,
	@Country [NVarChar] (15) = null,
	@EmployeeID [Int] = null,
	@Extension [NVarChar] (4) = null,
	@FirstName [NVarChar] (10) = null,
	@HireDate [DateTime] = null,
	@HomePhone [NVarChar] (24) = null,
	@LastName [NVarChar] (20) = null,
	@Notes [NText] = null,
	@Photo [Image] = null,
	@PhotoPath [NVarChar] (255) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@ReportsTo [Int] = null,
	@Title [NVarChar] (30) = null,
	@TitleOfCourtesy [NVarChar] (25) = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeUpdate]
(
	@Address [NVarChar] (60),
	@BirthDate [DateTime],
	@City [NVarChar] (15),
	@Country [NVarChar] (15),
	@Extension [NVarChar] (4),
	@FirstName [NVarChar] (10),
	@HireDate [DateTime],
	@HomePhone [NVarChar] (24),
	@LastName [NVarChar] (20),
	@Notes [NText],
	@Photo [Image],
	@PhotoPath [NVarChar] (255),
	@PostalCode [NVarChar] (10),
	@Region [NVarChar] (15),
	@ReportsTo [Int],
	@Title [NVarChar] (30),
	@TitleOfCourtesy [NVarChar] (25),
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_EmployeeID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Employees].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Employees].[CreatedDate])) AND 
((([dbo].[Employees].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Employees].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Employees].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Employees].[ModifiedDate])) AND 
((([Employees].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Employees].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeSelectByReportToEmployeePks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeSelectByReportToEmployeePks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeSelectByReportToEmployeePks]
(
	@xml xml,
	@direction char(4),
	@levels int
)
AS

DECLARE @newItemCount int
DECLARE @count int
DECLARE @index int

CREATE TABLE #TmpIds ([EmployeeID] [Int])
INSERT INTO #TmpIds([EmployeeID])
SELECT
	[Employees].[ReportsTo]
FROM 
[dbo].[Employees]
WHERE
	[dbo].[Employees].[ReportsTo] IN (SELECT T.c.value('./EmployeeID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


IF (@direction = 'NONE')
BEGIN
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
	[dbo].[Employees].[ReportsTo] IN (SELECT T.c.value('./EmployeeID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))

END
ELSE
BEGIN
IF (@direction = 'DOWN' OR @direction = 'BOTH')
BEGIN
	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)
	SET @count = @newItemCount
	SET @index = 1
	WHILE(@newItemCount > 0 and @index <= @levels)
	BEGIN
		INSERT INTO #TmpIds([EmployeeID])
		SELECT
	[EmployeeID]
		FROM 
			[dbo].[Employees]
			INNER JOIN #TmpIds ON 
[Employees].[ReportsTo] = #TmpIds.[EmployeeID]
		WHERE
			([dbo].[Employees].[ReportsTo] NOT IN (SELECT [EmployeeID] from #TmpIds))

		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count
		SET @count = (SELECT COUNT(*) FROM #TmpIds)
		SET @index = @index + 1
	END
END
IF (@direction = 'UP' OR @direction = 'BOTH')
BEGIN
	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)
	SET @count = @newItemCount
	SET @index = 1
	WHILE(@newItemCount > 0 and @index <= @levels)
	BEGIN
		INSERT INTO #TmpIds([EmployeeID])
		SELECT
	[Employees].[EmployeeID]
		FROM #TmpIds INNER JOIN [Employees] ON 
			[Employees].[ReportsTo] = #TmpIds.[EmployeeID]
		WHERE
			([dbo].[Employees].[ReportsTo] NOT IN (SELECT [EmployeeID] from #TmpIds))
		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count
		SET @count = (SELECT COUNT(*) FROM #TmpIds)
		SET @index = @index + 1
	END
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
			([dbo].[Employees].[ReportsTo] IN (SELECT [EmployeeID] from #TmpIds))

END
DROP TABLE #TmpIds

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [EmployeeTerritories]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieDelete]
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
	[TerritoryID] = @Original_TerritoryID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieSelectByEmployeeTerritoriePks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieSelectByEmployeeTerritoriePks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieSelectByEmployeeTerritoriePks]
(
	@xml xml
)
AS

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

	[dbo].[EmployeeTerritories].[EmployeeID] IN (SELECT T.c.value('./EmployeeID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))

	AND

	[dbo].[EmployeeTerritories].[TerritoryID] IN (SELECT T.c.value('./TerritoryID[1]', 'NVarChar (20)')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieSelectByEmployeeTerritorieSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieSelectByEmployeeTerritorieSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieSelectByEmployeeTerritorieSinglePk]
(
	@EmployeeID [Int],
	@TerritoryID [NVarChar] (20)
)
AS

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
[dbo].[EmployeeTerritories].[EmployeeID] = @EmployeeID AND [dbo].[EmployeeTerritories].[TerritoryID] = @TerritoryID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieInsert]
(
	@EmployeeID [Int] = null,
	@TerritoryID [NVarChar] (20) = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieUpdate]
(
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_EmployeeID [Int],
	@Original_TerritoryID [NVarChar] (20),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
UPDATE 
	[dbo].[EmployeeTerritories] 
SET
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[EmployeeTerritories].[EmployeeID] = @Original_EmployeeID AND
	[dbo].[EmployeeTerritories].[TerritoryID] = @Original_TerritoryID AND
	[dbo].[EmployeeTerritories].[Timestamp] = @Original_Timestamp


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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[EmployeeTerritories].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[EmployeeTerritories].[CreatedDate])) AND 
((([dbo].[EmployeeTerritories].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[EmployeeTerritories].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([EmployeeTerritories].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [EmployeeTerritories].[ModifiedDate])) AND 
((([EmployeeTerritories].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [EmployeeTerritories].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieSelectByEmployeePks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieSelectByEmployeePks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieSelectByEmployeePks]
(
	@xml xml
)
AS


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
	[dbo].[EmployeeTerritories].[EmployeeID] IN (SELECT T.c.value('./EmployeeID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_EmployeeTerritorieSelectByTerritoryPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_EmployeeTerritorieSelectByTerritoryPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_EmployeeTerritorieSelectByTerritoryPks]
(
	@xml xml
)
AS


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
	[dbo].[EmployeeTerritories].[TerritoryID] IN (SELECT T.c.value('./TerritoryID[1]', 'NVarChar (20)')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Order Details]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderDetailDelete]
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
	[ProductID] = @Original_ProductID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailSelectByOrderDetailPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailSelectByOrderDetailPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_OrderDetailSelectByOrderDetailPks]
(
	@xml xml
)
AS

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

	[dbo].[Order Details].[OrderID] IN (SELECT T.c.value('./OrderID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))

	AND

	[dbo].[Order Details].[ProductID] IN (SELECT T.c.value('./ProductID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailSelectByOrderDetailSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailSelectByOrderDetailSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderDetailSelectByOrderDetailSinglePk]
(
	@OrderID [Int],
	@ProductID [Int]
)
AS

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
[dbo].[Order Details].[OrderID] = @OrderID AND [dbo].[Order Details].[ProductID] = @ProductID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderDetailInsert]
(
	@Discount [Real] = null,
	@OrderID [Int] = null,
	@ProductID [Int] = null,
	@Quantity [SmallInt] = null,
	@UnitPrice [Money] = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderDetailUpdate]
(
	@Discount [Real],
	@Quantity [SmallInt],
	@UnitPrice [Money],
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_OrderID [Int],
	@Original_ProductID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderDetailSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Order Details].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Order Details].[CreatedDate])) AND 
((([dbo].[Order Details].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Order Details].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderDetailSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Order Details].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Order Details].[ModifiedDate])) AND 
((([Order Details].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Order Details].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderDetailSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailSelectByOrder_details_ordersOrderPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailSelectByOrder_details_ordersOrderPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_OrderDetailSelectByOrder_details_ordersOrderPks]
(
	@xml xml
)
AS


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
	[dbo].[Order Details].[OrderID] IN (SELECT T.c.value('./OrderID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDetailSelectByOrder_details_productsProductPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDetailSelectByOrder_details_productsProductPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_OrderDetailSelectByOrder_details_productsProductPks]
(
	@xml xml
)
AS


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
	[dbo].[Order Details].[ProductID] IN (SELECT T.c.value('./ProductID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Orders]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderDelete]
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
	[OrderID] = @Original_OrderID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderSelectByOrderPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderSelectByOrderPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_OrderSelectByOrderPks]
(
	@xml xml
)
AS

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

	[dbo].[Orders].[OrderID] IN (SELECT T.c.value('./OrderID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderSelectByOrderSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderSelectByOrderSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderSelectByOrderSinglePk]
(
	@OrderID [Int]
)
AS

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
[dbo].[Orders].[OrderID] = @OrderID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderInsert]
(
	@CustomerID [NChar] (5) = null,
	@EmployeeID [Int] = null,
	@Freight [Money] = null,
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
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderUpdate]
(
	@CustomerID [NChar] (5),
	@EmployeeID [Int],
	@Freight [Money],
	@OrderDate [DateTime],
	@RequiredDate [DateTime],
	@ShipAddress [NVarChar] (60),
	@ShipCity [NVarChar] (15),
	@ShipCountry [NVarChar] (15),
	@ShipName [NVarChar] (40),
	@ShippedDate [DateTime],
	@ShipPostalCode [NVarChar] (10),
	@ShipRegion [NVarChar] (15),
	@ShipVia [Int],
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_OrderID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Orders].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Orders].[CreatedDate])) AND 
((([dbo].[Orders].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Orders].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Orders].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Orders].[ModifiedDate])) AND 
((([Orders].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Orders].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_OrderSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderSelectByCustomerPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderSelectByCustomerPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_OrderSelectByCustomerPks]
(
	@xml xml
)
AS


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
	[dbo].[Orders].[CustomerID] IN (SELECT T.c.value('./CustomerID[1]', 'NChar (5)')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderSelectByEmployeePks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderSelectByEmployeePks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_OrderSelectByEmployeePks]
(
	@xml xml
)
AS


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
	[dbo].[Orders].[EmployeeID] IN (SELECT T.c.value('./EmployeeID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_OrderSelectByShipperPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_OrderSelectByShipperPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_OrderSelectByShipperPks]
(
	@xml xml
)
AS


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
	[dbo].[Orders].[ShipVia] IN (SELECT T.c.value('./ShipperID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Products]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ProductDelete]
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
	[ProductID] = @Original_ProductID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductSelectByProductPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductSelectByProductPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_ProductSelectByProductPks]
(
	@xml xml
)
AS

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

	[dbo].[Products].[ProductID] IN (SELECT T.c.value('./ProductID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductSelectByProductSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductSelectByProductSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ProductSelectByProductSinglePk]
(
	@ProductID [Int]
)
AS

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
[dbo].[Products].[ProductID] = @ProductID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ProductInsert]
(
	@CategoryID [Int] = null,
	@Discontinued [Bit] = null,
	@ProductID [Int] = null,
	@ProductName [NVarChar] (40) = null,
	@QuantityPerUnit [NVarChar] (20) = null,
	@ReorderLevel [SmallInt] = null,
	@SupplierID [Int] = null,
	@UnitPrice [Money] = null,
	@UnitsInStock [SmallInt] = null,
	@UnitsOnOrder [SmallInt] = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ProductUpdate]
(
	@CategoryID [Int],
	@Discontinued [Bit],
	@ProductName [NVarChar] (40),
	@QuantityPerUnit [NVarChar] (20),
	@ReorderLevel [SmallInt],
	@SupplierID [Int],
	@UnitPrice [Money],
	@UnitsInStock [SmallInt],
	@UnitsOnOrder [SmallInt],
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_ProductID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ProductSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Products].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Products].[CreatedDate])) AND 
((([dbo].[Products].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Products].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ProductSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Products].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Products].[ModifiedDate])) AND 
((([Products].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Products].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ProductSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductSelectByCategoryPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductSelectByCategoryPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_ProductSelectByCategoryPks]
(
	@xml xml
)
AS


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
	[dbo].[Products].[CategoryID] IN (SELECT T.c.value('./CategoryID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ProductSelectBySupplierPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ProductSelectBySupplierPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_ProductSelectBySupplierPks]
(
	@xml xml
)
AS


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
	[dbo].[Products].[SupplierID] IN (SELECT T.c.value('./SupplierID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Region]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_RegionDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_RegionDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_RegionDelete]
(
	@Original_RegionID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Region] 
WHERE 
	[RegionID] = @Original_RegionID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_RegionSelectByRegionPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_RegionSelectByRegionPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_RegionSelectByRegionPks]
(
	@xml xml
)
AS

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

	[dbo].[Region].[RegionID] IN (SELECT T.c.value('./RegionID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_RegionSelectByRegionSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_RegionSelectByRegionSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_RegionSelectByRegionSinglePk]
(
	@RegionID [Int]
)
AS

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
[dbo].[Region].[RegionID] = @RegionID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_RegionInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_RegionInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_RegionInsert]
(
	@RegionDescription [NChar] (50) = null,
	@RegionID [Int] = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_RegionUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_RegionUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_RegionUpdate]
(
	@RegionDescription [NChar] (50),
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_RegionID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
UPDATE 
	[dbo].[Region] 
SET
	[RegionDescription] = @RegionDescription,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE
	[dbo].[Region].[RegionID] = @Original_RegionID AND
	[dbo].[Region].[Timestamp] = @Original_Timestamp


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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_RegionSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_RegionSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_RegionSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Region].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Region].[CreatedDate])) AND 
((([dbo].[Region].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Region].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_RegionSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_RegionSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_RegionSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Region].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Region].[ModifiedDate])) AND 
((([Region].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Region].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_RegionSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_RegionSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_RegionSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Shippers]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ShipperDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ShipperDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ShipperDelete]
(
	@Original_ShipperID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Shippers] 
WHERE 
	[ShipperID] = @Original_ShipperID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ShipperSelectByShipperPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ShipperSelectByShipperPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_ShipperSelectByShipperPks]
(
	@xml xml
)
AS

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

	[dbo].[Shippers].[ShipperID] IN (SELECT T.c.value('./ShipperID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ShipperSelectByShipperSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ShipperSelectByShipperSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ShipperSelectByShipperSinglePk]
(
	@ShipperID [Int]
)
AS

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
[dbo].[Shippers].[ShipperID] = @ShipperID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ShipperInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ShipperInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ShipperInsert]
(
	@CompanyName [NVarChar] (40) = null,
	@Phone [NVarChar] (24) = null,
	@ShipperID [Int] = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ShipperUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ShipperUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ShipperUpdate]
(
	@CompanyName [NVarChar] (40),
	@Phone [NVarChar] (24),
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_ShipperID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ShipperSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ShipperSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ShipperSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Shippers].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Shippers].[CreatedDate])) AND 
((([dbo].[Shippers].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Shippers].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ShipperSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ShipperSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ShipperSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Shippers].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Shippers].[ModifiedDate])) AND 
((([Shippers].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Shippers].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_ShipperSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_ShipperSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_ShipperSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Suppliers]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_SupplierDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_SupplierDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_SupplierDelete]
(
	@Original_SupplierID [Int]
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Suppliers] 
WHERE 
	[SupplierID] = @Original_SupplierID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_SupplierSelectBySupplierPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_SupplierSelectBySupplierPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_SupplierSelectBySupplierPks]
(
	@xml xml
)
AS

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

	[dbo].[Suppliers].[SupplierID] IN (SELECT T.c.value('./SupplierID[1]', 'Int')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_SupplierSelectBySupplierSinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_SupplierSelectBySupplierSinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_SupplierSelectBySupplierSinglePk]
(
	@SupplierID [Int]
)
AS

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
[dbo].[Suppliers].[SupplierID] = @SupplierID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_SupplierInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_SupplierInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_SupplierInsert]
(
	@Address [NVarChar] (60) = null,
	@City [NVarChar] (15) = null,
	@CompanyName [NVarChar] (40) = null,
	@ContactName [NVarChar] (30) = null,
	@ContactTitle [NVarChar] (30) = null,
	@Country [NVarChar] (15) = null,
	@Fax [NVarChar] (24) = null,
	@HomePage [NText] = null,
	@Phone [NVarChar] (24) = null,
	@PostalCode [NVarChar] (10) = null,
	@Region [NVarChar] (15) = null,
	@SupplierID [Int] = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_SupplierUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_SupplierUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_SupplierUpdate]
(
	@Address [NVarChar] (60),
	@City [NVarChar] (15),
	@CompanyName [NVarChar] (40),
	@ContactName [NVarChar] (30),
	@ContactTitle [NVarChar] (30),
	@Country [NVarChar] (15),
	@Fax [NVarChar] (24),
	@HomePage [NText],
	@Phone [NVarChar] (24),
	@PostalCode [NVarChar] (10),
	@Region [NVarChar] (15),
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_SupplierID [Int],
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_SupplierSelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_SupplierSelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_SupplierSelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Suppliers].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Suppliers].[CreatedDate])) AND 
((([dbo].[Suppliers].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Suppliers].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_SupplierSelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_SupplierSelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_SupplierSelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Suppliers].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Suppliers].[ModifiedDate])) AND 
((([Suppliers].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Suppliers].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_SupplierSelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_SupplierSelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_SupplierSelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--This SQL is generated for internal stored procedures for table [Territories]

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritoryDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritoryDelete]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_TerritoryDelete]
(
	@Region_RegionID [Int] = null,--Entity Framework Required Parent Keys be passed in: Table 'Region'
	@Original_TerritoryID [NVarChar] (20)
)
AS
SET NOCOUNT OFF;

DELETE FROM
	[dbo].[Territories] 
WHERE 
	[TerritoryID] = @Original_TerritoryID ;

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritorySelectByTerritoryPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritorySelectByTerritoryPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE [dbo].[gen_TerritorySelectByTerritoryPks]
(
	@xml xml
)
AS

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

	[dbo].[Territories].[TerritoryID] IN (SELECT T.c.value('./TerritoryID[1]', 'NVarChar (20)')
		FROM @xml.nodes('//Item') T(c))



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritorySelectByTerritorySinglePk]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritorySelectByTerritorySinglePk]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_TerritorySelectByTerritorySinglePk]
(
	@TerritoryID [NVarChar] (20)
)
AS

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
[dbo].[Territories].[TerritoryID] = @TerritoryID 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritoryInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritoryInsert]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_TerritoryInsert]
(
	@RegionID [Int] = null,
	@TerritoryDescription [NChar] (50) = null,
	@TerritoryID [NVarChar] (20) = null,
	@CreatedDate [DateTime],
	@CreatedBy [Varchar] (50),
	@ModifiedBy [Varchar] (50)

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

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritoryUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritoryUpdate]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_TerritoryUpdate]
(
	@RegionID [Int],
	@TerritoryDescription [NChar] (50),
	@ModifiedBy [Varchar] (50),
	@ModifiedDate [DateTime] = null,
	@Original_TerritoryID [NVarChar] (20),
	@Original_Timestamp timestamp
)
AS

IF (@ModifiedDate IS NULL)
SET @ModifiedDate = getdate();

SET NOCOUNT OFF;
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
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritorySelectByCreatedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritorySelectByCreatedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_TerritorySelectByCreatedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([dbo].[Territories].[CreatedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [dbo].[Territories].[CreatedDate])) AND 
((([dbo].[Territories].[CreatedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [dbo].[Territories].[CreatedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritorySelectByModifiedDateRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritorySelectByModifiedDateRange]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_TerritorySelectByModifiedDateRange]
(
	@start_date [DateTime],
	@end_date [DateTime]
)
AS

SET NOCOUNT ON;

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
((([Territories].[ModifiedDate] IS NULL) AND (@start_date IS NULL)) OR (@start_date <= [Territories].[ModifiedDate])) AND 
((([Territories].[ModifiedDate] IS NULL) AND (@end_date IS NULL)) OR (@end_date >= [Territories].[ModifiedDate]))
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritorySelect]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritorySelect]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[gen_TerritorySelect]
AS

SET NOCOUNT ON;

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
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[gen_TerritorySelectByRegionPks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[gen_TerritorySelectByRegionPks]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO



CREATE PROCEDURE [dbo].[gen_TerritorySelectByRegionPks]
(
	@xml xml
)
AS


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
	[dbo].[Territories].[RegionID] IN (SELECT T.c.value('./RegionID[1]', 'Int')		FROM @xml.nodes('//Item') T(c))


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

