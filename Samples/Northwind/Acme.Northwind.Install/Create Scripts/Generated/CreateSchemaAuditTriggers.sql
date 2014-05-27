--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.
--Audit Triggers For Version 0.0.0.0
--Generated on 2012-02-25 15:25:22

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Categories]
if exists(select * from sysobjects where name = '__TR_Categories__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Categories__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Categories__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Categories__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Categories__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Categories__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[CustomerCustomerDemo]
if exists(select * from sysobjects where name = '__TR_CustomerCustomerDemo__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_CustomerCustomerDemo__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_CustomerCustomerDemo__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_CustomerCustomerDemo__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_CustomerCustomerDemo__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_CustomerCustomerDemo__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[CustomerDemographics]
if exists(select * from sysobjects where name = '__TR_CustomerDemographics__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_CustomerDemographics__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_CustomerDemographics__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_CustomerDemographics__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_CustomerDemographics__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_CustomerDemographics__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Customers]
if exists(select * from sysobjects where name = '__TR_Customers__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Customers__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Customers__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Customers__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Customers__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Customers__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Employees]
if exists(select * from sysobjects where name = '__TR_Employees__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Employees__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Employees__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Employees__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Employees__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Employees__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[EmployeeTerritories]
if exists(select * from sysobjects where name = '__TR_EmployeeTerritories__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_EmployeeTerritories__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_EmployeeTerritories__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_EmployeeTerritories__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_EmployeeTerritories__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_EmployeeTerritories__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Order Details]
if exists(select * from sysobjects where name = '__TR_Order Details__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Order Details__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Order Details__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Order Details__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Order Details__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Order Details__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Orders]
if exists(select * from sysobjects where name = '__TR_Orders__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Orders__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Orders__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Orders__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Orders__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Orders__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Products]
if exists(select * from sysobjects where name = '__TR_Products__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Products__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Products__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Products__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Products__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Products__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Region]
if exists(select * from sysobjects where name = '__TR_Region__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Region__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Region__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Region__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Region__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Region__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Shippers]
if exists(select * from sysobjects where name = '__TR_Shippers__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Shippers__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Shippers__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Shippers__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Shippers__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Shippers__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Suppliers]
if exists(select * from sysobjects where name = '__TR_Suppliers__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Suppliers__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Suppliers__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Suppliers__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Suppliers__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Suppliers__DELETE]
GO

--DROP ANY AUDIT TRIGGERS FOR [dbo].[Territories]
if exists(select * from sysobjects where name = '__TR_Territories__INSERT' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Territories__INSERT]
GO
if exists(select * from sysobjects where name = '__TR_Territories__UPDATE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Territories__UPDATE]
GO
if exists(select * from sysobjects where name = '__TR_Territories__DELETE' AND xtype = 'TR')
DROP TRIGGER [dbo].[__TR_Territories__DELETE]
GO

