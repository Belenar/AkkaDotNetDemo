USE [AkkaNetResults]
GO

/****** Object:  Table [dbo].[HourlyConsumption]    Script Date: 14/02/2018 0:00:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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


