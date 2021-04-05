/****** Object:  Table [dbo].[ServiceRequest]    Script Date: 4/5/2021 12:35:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceRequest](
	[ServiceRequestId] [uniqueidentifier] NOT NULL,
	[ServiceRequestNumber] [nvarchar](10) NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CompletedOn] [datetime] NULL,
	[DueOn] [datetime] NOT NULL,
	[Severity] [int] NULL,
 CONSTRAINT [PK_ServiceRequest] PRIMARY KEY CLUSTERED 
(
	[ServiceRequestId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ServiceRequest] ADD  CONSTRAINT [DF_ServiceRequest_ServiceRequestId]  DEFAULT (newid()) FOR [ServiceRequestId]
GO


