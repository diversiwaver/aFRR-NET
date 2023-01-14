CREATE DATABASE dmai0920_1086320
GO

USE [dmai0920_1086320]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Currency](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[Abbreviation] [varchar](3) NOT NULL,
 CONSTRAINT [Currency_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [dmai0920_1086320]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Direction](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[Value] [varchar](20) NOT NULL,
 CONSTRAINT [Direction_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [dmai0920_1086320]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Signal](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[ReceivedUtc] [datetime2](3) NOT NULL,
	[SentUtc] [datetime2](3) NOT NULL,
	[QuantityMw] [decimal](38, 0) NOT NULL,
	[DirectionId] [int] NOT NULL,
 CONSTRAINT [Signal_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Signal]  WITH CHECK ADD  CONSTRAINT [Signal_Direction_FK] FOREIGN KEY([DirectionId])
REFERENCES [dbo].[Direction] ([Id])
GO

ALTER TABLE [dbo].[Signal] CHECK CONSTRAINT [Signal_Direction_FK]
GO

USE [dmai0920_1086320]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Bid](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[ExternalId] [uniqueidentifier] NOT NULL,
	[FromUtc] [datetime2](3) NOT NULL,
	[ToUtc] [datetime2](3) NOT NULL,
	[QuantityMw] [decimal](38, 0) NOT NULL,
	[Price] [decimal](38, 0) NULL,
	[CurrencyId] [int] NOT NULL,
 CONSTRAINT [Bid_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [Bid_FK] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO

ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [Bid_FK]
GO

USE [dmai0920_1086320]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BidSignalMember](
	[BidId] [int] NOT NULL,
	[SignalId] [int] NOT NULL,
 CONSTRAINT [BidSignalMember_PK] PRIMARY KEY CLUSTERED 
(
	[BidId] ASC,
	[SignalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BidSignalMember]  WITH CHECK ADD  CONSTRAINT [BidSignalMember_Bid_FK] FOREIGN KEY([BidId])
REFERENCES [dbo].[Bid] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BidSignalMember] CHECK CONSTRAINT [BidSignalMember_Bid_FK]
GO

ALTER TABLE [dbo].[BidSignalMember]  WITH CHECK ADD  CONSTRAINT [BidSignalMember_Signal_FK] FOREIGN KEY([SignalId])
REFERENCES [dbo].[Signal] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BidSignalMember] CHECK CONSTRAINT [BidSignalMember_Signal_FK]
GO

USE [dmai0920_1086320]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AssetGroup](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[BalanceArea] [varchar](50) NOT NULL,
 CONSTRAINT [AssetGroup_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [dmai0920_1086320]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Asset](
	[Id] [int] IDENTITY(0,1) NOT NULL,
	[AssetGroupId] [int] NULL,
 CONSTRAINT [Asset_PK] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [Asset_FK] FOREIGN KEY([AssetGroupId])
REFERENCES [dbo].[AssetGroup] ([Id])
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [Asset_FK]
GO

USE [dmai0920_1086320]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AssetGroupBidMember](
	[AssetGroupId] [int] NOT NULL,
	[BidId] [int] NOT NULL,
 CONSTRAINT [AssetGroupBidMember_PK] PRIMARY KEY CLUSTERED 
(
	[AssetGroupId] ASC,
	[BidId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AssetGroupBidMember]  WITH CHECK ADD  CONSTRAINT [AssetGroupBidMember_FK] FOREIGN KEY([BidId])
REFERENCES [dbo].[Bid] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AssetGroupBidMember] CHECK CONSTRAINT [AssetGroupBidMember_FK]
GO

ALTER TABLE [dbo].[AssetGroupBidMember]  WITH CHECK ADD  CONSTRAINT [AssetGroupBidMember_FK_1] FOREIGN KEY([AssetGroupId])
REFERENCES [dbo].[AssetGroup] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AssetGroupBidMember] CHECK CONSTRAINT [AssetGroupBidMember_FK_1]
GO



