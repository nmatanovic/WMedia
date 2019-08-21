USE [WMDB]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 8/21/2019 5:16:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL
) ON [PRIMARY]

GO


