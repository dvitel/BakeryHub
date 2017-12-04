CREATE PROCEDURE partitionCleanup 
AS 
BEGIN 

CREATE TABLE [dbo].[TempNotificationLog](
	[Date] [datetime2](0) NOT NULL,
    [Id] [uniqueidentifier] NOT NULL,
	[ContactId] uniqueidentifier NOT NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[Subject] [nvarchar](255) NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TempNotificationLog] PRIMARY KEY CLUSTERED 
(
	[Date] ASC, [Id] ASC
)
);

ALTER TABLE NotificationLog SWITCH PARTITION 1 TO [dbo].[TempNotificationLog];
DROP TABLE [TempNotificationLog];
	
END
GO