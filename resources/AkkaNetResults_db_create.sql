SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE DATABASE AkkaNetResults
GO

CREATE DATABASE AkkaPersistence
GO

USE [AkkaNetResults]
GO

CREATE TABLE [dbo].[HourlyConsumption](
	[DeviceId] [uniqueidentifier] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Consumption] [numeric](18, 4) NOT NULL,
	[Unit] [nvarchar](10) NOT NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[HighConsumptionAlerts](
	[DeviceId] [uniqueidentifier] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Consumption] [numeric](18, 4) NOT NULL,
	[Duration] [int] NOT NULL
) ON [PRIMARY]
GO