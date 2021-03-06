﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BakeryHub.Domain.Migrations
{
    public partial class NLog_partitioning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("NotificationLog");
            migrationBuilder.Sql(
                @"
CREATE TABLE [dbo].[NotificationLog](
	[Date] [datetime2](0) NOT NULL,
    [Id] [uniqueidentifier] NOT NULL,
	[ContactId] [uniqueidentifier] NOT NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[Subject] [nvarchar](255) NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_NotificationLog] PRIMARY KEY CLUSTERED 
(
	[Date] ASC, [Id] ASC
)
) ON bucketsPerDayScheme([Date])
GO

ALTER TABLE [dbo].[NotificationLog]  WITH CHECK ADD  CONSTRAINT [FK_NotificationLog_Contacts_ContactId] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([ContactId])
GO
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
