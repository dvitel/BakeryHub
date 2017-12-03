IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Countries] (
    [Code] varchar(2) NOT NULL,
    [Name] varchar(100) NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY ([Code])
);

GO

CREATE TABLE [Handshake] (
    [Id] uniqueidentifier NOT NULL,
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [SeqNum] int NOT NULL,
    [SupplierId] int NOT NULL,
    [TimeStamp] datetime2(0) NOT NULL,
    [Turn] int NOT NULL,
    CONSTRAINT [PK_Handshake] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [ProductCategories] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(400) NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_ProductCategories] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Session] (
    [Id] uniqueidentifier NOT NULL,
    [FirstVisit] datetime2(0) NOT NULL,
    [IP] nvarchar(39) NOT NULL,
    [LastVisit] datetime2(0) NOT NULL,
    [UserAgent] nvarchar(400) NOT NULL,
    CONSTRAINT [PK_Session] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [States] (
    [Code] varchar(2) NOT NULL,
    [Name] varchar(100) NOT NULL,
    CONSTRAINT [PK_States] PRIMARY KEY ([Code])
);

GO

CREATE TABLE [HandshakeComments] (
    [HandshakeId] uniqueidentifier NOT NULL,
    [ProductId] int NOT NULL,
    [Comment] nvarchar(max) NULL,
    [IsCanceled] bit NOT NULL,
    [NewPrice] decimal(18, 2) NULL,
    CONSTRAINT [PK_HandshakeComments] PRIMARY KEY ([HandshakeId], [ProductId]),
    CONSTRAINT [FK_HandshakeComments_Handshake_HandshakeId] FOREIGN KEY ([HandshakeId]) REFERENCES [Handshake] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Login] nvarchar(100) NOT NULL,
    [Password] nvarchar(100) NOT NULL,
    [PasswordEncryptionAlgorithm] int NOT NULL,
    [Salt] nvarchar(max) NULL,
    [SessionId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Session_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Session] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Address] (
    [UserId] int NOT NULL,
    [AddressId] int NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [CountryId] varchar(2) NULL,
    [IsBilling] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [StateId] varchar(2) NULL,
    [Street] nvarchar(400) NOT NULL,
    [Zip] nvarchar(30) NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY ([UserId], [AddressId]),
    CONSTRAINT [FK_Address_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Code]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Address_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Code]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Address_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Contacts] (
    [UserId] int NOT NULL,
    [ContactId] int NOT NULL,
    [Address] nvarchar(255) NOT NULL,
    [IsConfirmed] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsPrivate] bit NOT NULL,
    [Name] nvarchar(max) NULL,
    [Type] int NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([UserId], [ContactId]),
    CONSTRAINT [FK_Contacts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Customers] (
    [Id] int NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Customers_Users_Id] FOREIGN KEY ([Id]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [DeliverySites] (
    [Id] int NOT NULL,
    [Description] nvarchar(400) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [isCompany] bit NOT NULL,
    CONSTRAINT [PK_DeliverySites] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DeliverySites_Users_Id] FOREIGN KEY ([Id]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [PaymentMethods] (
    [UserId] int NOT NULL,
    [PaymentMethodId] int NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Type] int NOT NULL,
    [UIDesc] nvarchar(max) NULL,
    CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([UserId], [PaymentMethodId]),
    CONSTRAINT [FK_PaymentMethods_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Reviews] (
    [Id] uniqueidentifier NOT NULL,
    [Date] datetime2(0) NOT NULL,
    [Feedback] nvarchar(400) NOT NULL,
    [IsAboutProduct] bit NOT NULL,
    [Rating] int NOT NULL,
    [TargetUserId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reviews_Users_TargetUserId] FOREIGN KEY ([TargetUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reviews_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Suppliers] (
    [Id] int NOT NULL,
    [Description] nvarchar(400) NOT NULL,
    [HasLogo] bit NOT NULL DEFAULT 0,
    [IsCompany] bit NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Suppliers_Users_Id] FOREIGN KEY ([Id]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [FeedbackSubscription] (
    [UserId] int NOT NULL,
    [ContactId] int NOT NULL,
    CONSTRAINT [PK_FeedbackSubscription] PRIMARY KEY ([UserId], [ContactId]),
    CONSTRAINT [FK_FeedbackSubscription_Contacts_UserId_ContactId] FOREIGN KEY ([UserId], [ContactId]) REFERENCES [Contacts] ([UserId], [ContactId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [NotificationLog] (
    [UserId] int NOT NULL,
    [ContactId] int NOT NULL,
    [MessageId] bigint NOT NULL,
    [Date] datetime2(0) NOT NULL,
    [ErrorMessage] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [Subject] nvarchar(max) NULL,
    [Text] nvarchar(max) NULL,
    CONSTRAINT [PK_NotificationLog] PRIMARY KEY ([UserId], [ContactId], [MessageId]),
    CONSTRAINT [FK_NotificationLog_Contacts_UserId_ContactId] FOREIGN KEY ([UserId], [ContactId]) REFERENCES [Contacts] ([UserId], [ContactId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [ReportSubscription] (
    [UserId] int NOT NULL,
    [ContactId] int NOT NULL,
    [Type] int NOT NULL,
    CONSTRAINT [PK_ReportSubscription] PRIMARY KEY ([UserId], [ContactId]),
    CONSTRAINT [FK_ReportSubscription_Contacts_UserId_ContactId] FOREIGN KEY ([UserId], [ContactId]) REFERENCES [Contacts] ([UserId], [ContactId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [CardPaymentMethods] (
    [UserId] int NOT NULL,
    [PaymentMethodId] int NOT NULL,
    [BillingAddressId] int NOT NULL,
    [CardNumber] nvarchar(16) NOT NULL,
    [NameOnCard] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_CardPaymentMethods] PRIMARY KEY ([UserId], [PaymentMethodId]),
    CONSTRAINT [FK_CardPaymentMethods_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CardPaymentMethods_Address_UserId_BillingAddressId] FOREIGN KEY ([UserId], [BillingAddressId]) REFERENCES [Address] ([UserId], [AddressId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CardPaymentMethods_PaymentMethods_UserId_PaymentMethodId] FOREIGN KEY ([UserId], [PaymentMethodId]) REFERENCES [PaymentMethods] ([UserId], [PaymentMethodId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [PayPalPaymentMethods] (
    [UserId] int NOT NULL,
    [PaymentMethodId] int NOT NULL,
    [PayPalAddress] nvarchar(max) NULL,
    CONSTRAINT [PK_PayPalPaymentMethods] PRIMARY KEY ([UserId], [PaymentMethodId]),
    CONSTRAINT [FK_PayPalPaymentMethods_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PayPalPaymentMethods_PaymentMethods_UserId_PaymentMethodId] FOREIGN KEY ([UserId], [PaymentMethodId]) REFERENCES [PaymentMethods] ([UserId], [PaymentMethodId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Orders] (
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [DatePlaced] datetime2(0) NOT NULL,
    [LastUpdated] datetime2 NOT NULL,
    [PlannedDeliveryDate] datetime2(0) NOT NULL,
    [Price] decimal(18, 2) NOT NULL,
    [Status] int NOT NULL,
    [SupplierId] int NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([CustomerId], [OrderId]),
    CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Orders_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Products] (
    [SupplierId] int NOT NULL,
    [ProductId] int NOT NULL,
    [AvailableNow] int NOT NULL,
    [CategoryId] int NOT NULL,
    [Description] nvarchar(400) NOT NULL,
    [LastUpdated] datetime2 NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    [Price] decimal(18, 2) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([SupplierId], [ProductId]),
    CONSTRAINT [FK_Products_ProductCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Products_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Delivery] (
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [CustomerAddressId] int NOT NULL,
    [DeliverySiteId] int NOT NULL,
    [LastUpdated] datetime2 NOT NULL,
    [Price] decimal(18, 2) NULL,
    [Status] int NOT NULL,
    [SupplierAddressId] int NOT NULL,
    [SupplierId] int NOT NULL,
    CONSTRAINT [PK_Delivery] PRIMARY KEY ([CustomerId], [OrderId]),
    CONSTRAINT [FK_Delivery_DeliverySites_DeliverySiteId] FOREIGN KEY ([DeliverySiteId]) REFERENCES [DeliverySites] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delivery_Address_CustomerId_CustomerAddressId] FOREIGN KEY ([CustomerId], [CustomerAddressId]) REFERENCES [Address] ([UserId], [AddressId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delivery_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delivery_Address_SupplierId_SupplierAddressId] FOREIGN KEY ([SupplierId], [SupplierAddressId]) REFERENCES [Address] ([UserId], [AddressId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrderItems] (
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [IsCanceled] bit NOT NULL,
    [ProductCount] int NOT NULL,
    [TotalPrice] decimal(18, 2) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([CustomerId], [OrderId], [ProductId]),
    CONSTRAINT [FK_OrderItems_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrderPaymentSensitiveInfo] (
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [CVV] nvarchar(max) NULL,
    [ExpirationDate] nvarchar(max) NULL,
    [PaymentMethodId] int NOT NULL,
    CONSTRAINT [PK_OrderPaymentSensitiveInfo] PRIMARY KEY ([CustomerId], [OrderId]),
    CONSTRAINT [FK_OrderPaymentSensitiveInfo_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrderPaymentSensitiveInfo_PaymentMethods_CustomerId_PaymentMethodId] FOREIGN KEY ([CustomerId], [PaymentMethodId]) REFERENCES [PaymentMethods] ([UserId], [PaymentMethodId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrderSubscription] (
    [UserId] int NOT NULL,
    [ContactId] int NOT NULL,
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    CONSTRAINT [PK_OrderSubscription] PRIMARY KEY ([UserId], [ContactId], [CustomerId], [OrderId]),
    CONSTRAINT [FK_OrderSubscription_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrderSubscription_Contacts_UserId_ContactId] FOREIGN KEY ([UserId], [ContactId]) REFERENCES [Contacts] ([UserId], [ContactId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Payments] (
    [UserId] int NOT NULL,
    [PaymentId] int NOT NULL,
    [Amount] decimal(18, 2) NOT NULL,
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [PaymentMethodId] int NOT NULL,
    [Status] int NOT NULL,
    [Target] int NOT NULL,
    [TargetUserId] int NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([UserId], [PaymentId]),
    CONSTRAINT [FK_Payments_Users_TargetUserId] FOREIGN KEY ([TargetUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_PaymentMethods_UserId_PaymentMethodId] FOREIGN KEY ([UserId], [PaymentMethodId]) REFERENCES [PaymentMethods] ([UserId], [PaymentMethodId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [CartItems] (
    [SessionId] uniqueidentifier NOT NULL,
    [ItemId] int NOT NULL,
    [DatePlaced] datetime2(0) NOT NULL,
    [ProductCount] int NOT NULL,
    [ProductId] int NOT NULL,
    [SupplierId] int NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([SessionId], [ItemId]),
    CONSTRAINT [FK_CartItems_Session_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Session] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CartItems_Products_SupplierId_ProductId] FOREIGN KEY ([SupplierId], [ProductId]) REFERENCES [Products] ([SupplierId], [ProductId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [ProductImages] (
    [SupplierId] int NOT NULL,
    [ProductId] int NOT NULL,
    [ImageId] int NOT NULL,
    [LogicalPath] nvarchar(max) NULL,
    [Mime] nvarchar(max) NULL,
    [Path] nvarchar(max) NULL,
    CONSTRAINT [PK_ProductImages] PRIMARY KEY ([SupplierId], [ProductId], [ImageId]),
    CONSTRAINT [FK_ProductImages_Products_SupplierId_ProductId] FOREIGN KEY ([SupplierId], [ProductId]) REFERENCES [Products] ([SupplierId], [ProductId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [ProductReview] (
    [ReviewId] uniqueidentifier NOT NULL,
    [ProductId] int NOT NULL,
    [SupplierId] int NOT NULL,
    CONSTRAINT [PK_ProductReview] PRIMARY KEY ([ReviewId]),
    CONSTRAINT [FK_ProductReview_Reviews_ReviewId] FOREIGN KEY ([ReviewId]) REFERENCES [Reviews] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ProductReview_Products_SupplierId_ProductId] FOREIGN KEY ([SupplierId], [ProductId]) REFERENCES [Products] ([SupplierId], [ProductId]) ON DELETE NO ACTION
);

GO

CREATE UNIQUE INDEX [IX_Address_CountryId] ON [Address] ([CountryId]) WHERE [CountryId] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_Address_StateId] ON [Address] ([StateId]) WHERE [StateId] IS NOT NULL;

GO

CREATE INDEX [IX_CardPaymentMethods_UserId_BillingAddressId] ON [CardPaymentMethods] ([UserId], [BillingAddressId]);

GO

CREATE INDEX [IX_CartItems_SupplierId_ProductId] ON [CartItems] ([SupplierId], [ProductId]);

GO

CREATE INDEX [IX_Delivery_DeliverySiteId] ON [Delivery] ([DeliverySiteId]);

GO

CREATE INDEX [IX_Delivery_CustomerId_CustomerAddressId] ON [Delivery] ([CustomerId], [CustomerAddressId]);

GO

CREATE INDEX [IX_Delivery_SupplierId_SupplierAddressId] ON [Delivery] ([SupplierId], [SupplierAddressId]);

GO

CREATE INDEX [IX_Handshake_CustomerId_OrderId] ON [Handshake] ([CustomerId], [OrderId]);

GO

CREATE INDEX [IX_OrderPaymentSensitiveInfo_CustomerId_PaymentMethodId] ON [OrderPaymentSensitiveInfo] ([CustomerId], [PaymentMethodId]);

GO

CREATE INDEX [IX_Orders_SupplierId] ON [Orders] ([SupplierId]);

GO

CREATE INDEX [IX_OrderSubscription_CustomerId_OrderId] ON [OrderSubscription] ([CustomerId], [OrderId]);

GO

CREATE INDEX [IX_Payments_TargetUserId] ON [Payments] ([TargetUserId]);

GO

CREATE UNIQUE INDEX [IX_Payments_CustomerId_OrderId] ON [Payments] ([CustomerId], [OrderId]);

GO

CREATE INDEX [IX_Payments_UserId_PaymentMethodId] ON [Payments] ([UserId], [PaymentMethodId]);

GO

CREATE INDEX [IX_ProductReview_SupplierId_ProductId] ON [ProductReview] ([SupplierId], [ProductId]);

GO

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

GO

CREATE INDEX [IX_Reviews_UserId] ON [Reviews] ([UserId]);

GO

CREATE INDEX [IX_Reviews_TargetUserId_Date] ON [Reviews] ([TargetUserId], [Date]);

GO

CREATE INDEX [IX_Users_Login] ON [Users] ([Login]);

GO

CREATE INDEX [IX_Users_SessionId] ON [Users] ([SessionId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171201052841_Schema_v1', N'2.0.0-rtm-26452');

GO

CREATE INDEX [IX_Handshake_SupplierId] ON [Handshake] ([SupplierId]);

GO

ALTER TABLE [Handshake] ADD CONSTRAINT [FK_Handshake_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Handshake] ADD CONSTRAINT [FK_Handshake_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Handshake] ADD CONSTRAINT [FK_Handshake_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171201055802_Handshake_fix', N'2.0.0-rtm-26452');

GO

ALTER TABLE [CardPaymentMethods] DROP CONSTRAINT [FK_CardPaymentMethods_Users_UserId];

GO

ALTER TABLE [PayPalPaymentMethods] DROP CONSTRAINT [FK_PayPalPaymentMethods_Users_UserId];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171201060829_remove_fk_Card_PayPal_User', N'2.0.0-rtm-26452');

GO

DELETE Countries

GO

DELETE States

GO

DELETE ProductCategories

GO

--DELETE ProductCategories

INSERT ProductCategories (Id, Description, Name) VALUES 
	(1, 'Different kind of cakes: with cream, chocolate, etc...', 'Cakes'),
	(2, 'Wheat, Rye, Pita breads, etc...', 'Bread')

INSERT ProductCategories (Id, Description, Name) VALUES 
	(3, 'Cookies, etc...', 'Pastry')

GO

USE [BakeryHub]

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AD', 'Andorra')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AE', 'United Arab Emirates')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AF', 'Afghanistan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AG', 'Antigua and Barbuda')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AI', 'Anguilla')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AL', 'Albania')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AM', 'Armenia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AO', 'Angola')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AQ', 'Antarctica')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AR', 'Argentina')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AS', 'American Samoa')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AT', 'Austria')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AU', 'Australia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AW', 'Aruba')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AX', 'Ã…land Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('AZ', 'Azerbaijan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BA', 'Bosnia and Herzegovina')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BB', 'Barbados')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BD', 'Bangladesh')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BE', 'Belgium')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BF', 'Burkina Faso')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BG', 'Bulgaria')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BH', 'Bahrain')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BI', 'Burundi')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BJ', 'Benin')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BL', 'Saint BarthÃ©lemy')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BM', 'Bermuda')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BN', 'Brunei Darussalam')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BO', 'Bolivia (Plurinational State of)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BQ', 'Bonaire, Sint Eustatius and Saba')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BR', 'Brazil')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BS', 'Bahamas')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BT', 'Bhutan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BV', 'Bouvet Island')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BW', 'Botswana')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BY', 'Belarus')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('BZ', 'Belize')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CA', 'Canada')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CC', 'Cocos (Keeling) Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CD', 'Congo (Democratic Republic of the)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CF', 'Central African Republic')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CG', 'Congo')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CH', 'Switzerland')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CI', 'CÃ´te d''Ivoire')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CK', 'Cook Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CL', 'Chile')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CM', 'Cameroon')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CN', 'China')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CO', 'Colombia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CR', 'Costa Rica')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CU', 'Cuba')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CV', 'Cabo Verde')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CW', 'CuraÃ§ao')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CX', 'Christmas Island')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CY', 'Cyprus')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('CZ', 'Czech Republic')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('DE', 'Germany')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('DJ', 'Djibouti')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('DK', 'Denmark')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('DM', 'Dominica')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('DO', 'Dominican Republic')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('DZ', 'Algeria')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('EC', 'Ecuador')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('EE', 'Estonia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('EG', 'Egypt')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('EH', 'Western Sahara')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ER', 'Eritrea')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ES', 'Spain')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ET', 'Ethiopia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('FI', 'Finland')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('FJ', 'Fiji')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('FK', 'Falkland Islands (Malvinas)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('FM', 'Micronesia (Federated States of)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('FO', 'Faroe Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('FR', 'France')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GA', 'Gabon')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GB', 'United Kingdom of Great Britain')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GD', 'Grenada')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GE', 'Georgia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GF', 'French Guiana')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GG', 'Guernsey')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GH', 'Ghana')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GI', 'Gibraltar')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GL', 'Greenland')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GM', 'Gambia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GN', 'Guinea')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GP', 'Guadeloupe')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GQ', 'Equatorial Guinea')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GR', 'Greece')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GS', 'South Georgia and the South Sandwich Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GT', 'Guatemala')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GU', 'Guam')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GW', 'Guinea-Bissau')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('GY', 'Guyana')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('HK', 'Hong Kong')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('HM', 'Heard Island and McDonald Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('HN', 'Honduras')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('HR', 'Croatia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('HT', 'Haiti')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('HU', 'Hungary')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ID', 'Indonesia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IE', 'Ireland')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IL', 'Israel')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IM', 'Isle of Man')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IN', 'India')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IO', 'British Indian Ocean Territory')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IQ', 'Iraq')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IR', 'Iran (Islamic Republic of)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IS', 'Iceland')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('IT', 'Italy')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('JE', 'Jersey')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('JM', 'Jamaica')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('JO', 'Jordan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('JP', 'Japan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KE', 'Kenya')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KG', 'Kyrgyzstan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KH', 'Cambodia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KI', 'Kiribati')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KM', 'Comoros')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KN', 'Saint Kitts and Nevis')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KP', 'Korea (Democratic People''s Republic of)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KR', 'Korea (Republic of)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KW', 'Kuwait')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KY', 'Cayman Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('KZ', 'Kazakhstan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LA', 'Lao People''s Democratic Republic')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LB', 'Lebanon')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LC', 'Saint Lucia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LI', 'Liechtenstein')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LK', 'Sri Lanka')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LR', 'Liberia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LS', 'Lesotho')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LT', 'Lithuania')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LU', 'Luxembourg')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LV', 'Latvia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('LY', 'Libya')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MA', 'Morocco')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MC', 'Monaco')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MD', 'Moldova (Republic of)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ME', 'Montenegro')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MF', 'Saint Martin (French part)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MG', 'Madagascar')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MH', 'Marshall Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MK', 'Macedonia (the former Yugoslav Republic of)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ML', 'Mali')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MM', 'Myanmar')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MN', 'Mongolia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MO', 'Macao')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MP', 'Northern Mariana Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MQ', 'Martinique')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MR', 'Mauritania')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MS', 'Montserrat')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MT', 'Malta')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MU', 'Mauritius')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MV', 'Maldives')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MW', 'Malawi')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MX', 'Mexico')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MY', 'Malaysia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('MZ', 'Mozambique')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NA', 'Namibia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NC', 'New Caledonia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NE', 'Niger')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NF', 'Norfolk Island')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NG', 'Nigeria')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NI', 'Nicaragua')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NL', 'Netherlands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NO', 'Norway')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NP', 'Nepal')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NR', 'Nauru')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NU', 'Niue')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('NZ', 'New Zealand')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('OM', 'Oman')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PA', 'Panama')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PE', 'Peru')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PF', 'French Polynesia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PG', 'Papua New Guinea')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PH', 'Philippines')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PK', 'Pakistan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PL', 'Poland')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PM', 'Saint Pierre and Miquelon')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PN', 'Pitcairn')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PR', 'Puerto Rico')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PS', 'Palestine, State of')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PT', 'Portugal')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PW', 'Palau')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('PY', 'Paraguay')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('QA', 'Qatar')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('RE', 'RÃ©union')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('RO', 'Romania')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('RS', 'Serbia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('RU', 'Russian Federation')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('RW', 'Rwanda')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SA', 'Saudi Arabia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SB', 'Solomon Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SC', 'Seychelles')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SD', 'Sudan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SE', 'Sweden')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SG', 'Singapore')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SH', 'Saint Helena, Ascension and Tristan da Cunha')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SI', 'Slovenia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SJ', 'Svalbard and Jan Mayen')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SK', 'Slovakia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SL', 'Sierra Leone')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SM', 'San Marino')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SN', 'Senegal')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SO', 'Somalia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SR', 'Suriname')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SS', 'South Sudan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ST', 'Sao Tome and Principe')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SV', 'El Salvador')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SX', 'Sint Maarten (Dutch part)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SY', 'Syrian Arab Republic')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('SZ', 'Swaziland')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TC', 'Turks and Caicos Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TD', 'Chad')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TF', 'French Southern Territories')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TG', 'Togo')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TH', 'Thailand')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TJ', 'Tajikistan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TK', 'Tokelau')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TL', 'Timor-Leste')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TM', 'Turkmenistan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TN', 'Tunisia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TO', 'Tonga')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TR', 'Turkey')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TT', 'Trinidad and Tobago')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TV', 'Tuvalu')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TW', 'Taiwan, Province of China')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('TZ', 'Tanzania, United Republic of')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('UA', 'Ukraine')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('UG', 'Uganda')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('UM', 'United States Minor Outlying Islands')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('US', 'United States of America')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('UY', 'Uruguay')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('UZ', 'Uzbekistan')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('VA', 'Holy See')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('VC', 'Saint Vincent and the Grenadines')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('VE', 'Venezuela (Bolivarian Republic of)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('VG', 'Virgin Islands (British)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('VI', 'Virgin Islands (U.S.)')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('VN', 'Viet Nam')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('VU', 'Vanuatu')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('WF', 'Wallis and Futuna')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('WS', 'Samoa')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('YE', 'Yemen')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('YT', 'Mayotte')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ZA', 'South Africa')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ZM', 'Zambia')

GO

INSERT [dbo].[Countries] ([Code], [Name]) VALUES ('ZW', 'Zimbabwe')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('AK', 'Alaska')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('AL', 'Alabama')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('AR', 'Arkansas')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('AZ', 'Arizona')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('CA', 'California')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('CO', 'Colorado')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('CT', 'Connecticut')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('DC', 'District of Columbia')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('DE', 'Delaware')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('FL', 'Florida')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('GA', 'Georgia')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('HI', 'Hawaii')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('IA', 'Iowa')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('ID', 'Idaho')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('IL', 'Illinois')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('IN', 'Indiana')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('KS', 'Kansas')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('KY', 'Kentucky')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('LA', 'Louisiana')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('MA', 'Massachusetts')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('MD', 'Maryland')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('ME', 'Maine')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('MI', 'Michigan')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('MN', 'Minnesota')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('MO', 'Missouri')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('MS', 'Mississippi')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('MT', 'Montana')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('NC', 'North Carolina')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('ND', 'North Dakota')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('NE', 'Nebraska')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('NH', 'New Hampshire')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('NJ', 'New Jersey')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('NM', 'New Mexico')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('NV', 'Nevada')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('NY', 'New York')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('OH', 'Ohio')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('OK', 'Oklahoma')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('OR', 'Oregon')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('PA', 'Pennsylvania')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('RI', 'Rhode Island')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('SC', 'South Carolina')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('SD', 'South Dakota')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('TN', 'Tennessee')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('TX', 'Texas')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('UT', 'Utah')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('VA', 'Virginia')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('VT', 'Vermont')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('WA', 'Washington')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('WI', 'Wisconsin')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('WV', 'West Virginia')

GO

INSERT [dbo].[States] ([Code], [Name]) VALUES ('WY', 'Wyoming')

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171201063324_INIT_DATA', N'2.0.0-rtm-26452');

GO

DROP INDEX [IX_Users_Login] ON [Users];

GO

CREATE UNIQUE INDEX [IX_Users_Login] ON [Users] ([Login]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171201200809_Simple_Migration', N'2.0.0-rtm-26452');

GO

ALTER TABLE [Handshake] DROP CONSTRAINT [FK_Handshake_Customers_CustomerId];

GO

ALTER TABLE [Handshake] DROP CONSTRAINT [FK_Handshake_Suppliers_SupplierId];

GO

DROP INDEX [IX_Handshake_SupplierId] ON [Handshake];

GO

DROP INDEX [IX_Address_CountryId] ON [Address];

GO

DROP INDEX [IX_Address_StateId] ON [Address];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Users') AND [c].[name] = N'Salt');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] ALTER COLUMN [Salt] nvarchar(100) NOT NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Users') AND [c].[name] = N'Password');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [Password] nvarchar(200) NOT NULL;

GO

ALTER TABLE [Suppliers] ADD [IsApproved] bit NOT NULL DEFAULT 0;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'ProductImages') AND [c].[name] = N'Path');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ProductImages] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [ProductImages] ALTER COLUMN [Path] nvarchar(255) NOT NULL;

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'ProductImages') AND [c].[name] = N'Mime');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [ProductImages] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [ProductImages] ALTER COLUMN [Mime] nvarchar(40) NOT NULL;

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'ProductImages') AND [c].[name] = N'LogicalPath');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [ProductImages] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [ProductImages] ALTER COLUMN [LogicalPath] nvarchar(100) NOT NULL;

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'PayPalPaymentMethods') AND [c].[name] = N'PayPalAddress');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [PayPalPaymentMethods] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [PayPalPaymentMethods] ALTER COLUMN [PayPalAddress] nvarchar(255) NOT NULL;

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'PaymentMethods') AND [c].[name] = N'UIDesc');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [PaymentMethods] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [PaymentMethods] ALTER COLUMN [UIDesc] nvarchar(100) NOT NULL;

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'OrderPaymentSensitiveInfo') AND [c].[name] = N'ExpirationDate');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [OrderPaymentSensitiveInfo] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [OrderPaymentSensitiveInfo] ALTER COLUMN [ExpirationDate] nvarchar(100) NOT NULL;

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'OrderPaymentSensitiveInfo') AND [c].[name] = N'CVV');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [OrderPaymentSensitiveInfo] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [OrderPaymentSensitiveInfo] ALTER COLUMN [CVV] nvarchar(100) NOT NULL;

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'NotificationLog') AND [c].[name] = N'Text');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [NotificationLog] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [NotificationLog] ALTER COLUMN [Text] nvarchar(max) NOT NULL;

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'NotificationLog') AND [c].[name] = N'Subject');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [NotificationLog] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [NotificationLog] ALTER COLUMN [Subject] nvarchar(255) NULL;

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'HandshakeComments') AND [c].[name] = N'Comment');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [HandshakeComments] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [HandshakeComments] ALTER COLUMN [Comment] nvarchar(400) NOT NULL;

GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Customers') AND [c].[name] = N'Name');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Customers] ALTER COLUMN [Name] nvarchar(100) NOT NULL;

GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Contacts') AND [c].[name] = N'Name');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Contacts] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Contacts] ALTER COLUMN [Name] nvarchar(100) NOT NULL;

GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Address') AND [c].[name] = N'Zip');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Address] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Address] ALTER COLUMN [Zip] varchar(30) NULL;

GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Address') AND [c].[name] = N'CountryId');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Address] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Address] ALTER COLUMN [CountryId] varchar(2) NOT NULL;

GO

CREATE INDEX [IX_Address_CountryId] ON [Address] ([CountryId]);

GO

CREATE INDEX [IX_Address_StateId] ON [Address] ([StateId]);

GO

ALTER TABLE [Products] DROP CONSTRAINT [FK_Products_ProductCategories_CategoryId];

GO

DROP TABLE [ProductCategories];

GO

CREATE TABLE [ProductCategories] (
    [Id] int NOT NULL,
    [Description] nvarchar(400) NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_ProductCategories] PRIMARY KEY ([Id])
);

GO

--DELETE ProductCategories

INSERT ProductCategories (Id, Description, Name) VALUES 
	(1, 'Different kind of cakes: with cream, chocolate, etc...', 'Cakes'),
	(2, 'Wheat, Rye, Pita breads, etc...', 'Bread')

INSERT ProductCategories (Id, Description, Name) VALUES 
	(3, 'Cookies, etc...', 'Pastry')

GO

ALTER TABLE [Products] ADD CONSTRAINT [FK_Products_ProductCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171202153546_fix_NULL_constraints', N'2.0.0-rtm-26452');

GO

DROP TABLE [FeedbackSubscription];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171202192252_FeedbackSubscription_Removed', N'2.0.0-rtm-26452');

GO

ALTER TABLE [ReportSubscription] DROP CONSTRAINT [PK_ReportSubscription];

GO

ALTER TABLE [ReportSubscription] ADD CONSTRAINT [PK_ReportSubscription] PRIMARY KEY ([UserId], [ContactId], [Type]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171202195100_ReportSubscription_KeyFix', N'2.0.0-rtm-26452');

GO

ALTER TABLE [ProductReview] DROP CONSTRAINT [FK_ProductReview_Reviews_ReviewId];

GO

ALTER TABLE [ProductReview] DROP CONSTRAINT [FK_ProductReview_Products_SupplierId_ProductId];

GO

ALTER TABLE [ProductReview] DROP CONSTRAINT [PK_ProductReview];

GO

EXEC sp_rename N'ProductReview', N'ProductReviews';

GO

EXEC sp_rename N'ProductReviews.IX_ProductReview_SupplierId_ProductId', N'IX_ProductReviews_SupplierId_ProductId', N'INDEX';

GO

ALTER TABLE [ProductReviews] ADD CONSTRAINT [PK_ProductReviews] PRIMARY KEY ([ReviewId]);

GO

ALTER TABLE [ProductReviews] ADD CONSTRAINT [FK_ProductReviews_Reviews_ReviewId] FOREIGN KEY ([ReviewId]) REFERENCES [Reviews] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [ProductReviews] ADD CONSTRAINT [FK_ProductReviews_Products_SupplierId_ProductId] FOREIGN KEY ([SupplierId], [ProductId]) REFERENCES [Products] ([SupplierId], [ProductId]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171202213201_ProductReviewsFix', N'2.0.0-rtm-26452');

GO

ALTER TABLE [NotificationLog] DROP CONSTRAINT [PK_NotificationLog];

GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'NotificationLog') AND [c].[name] = N'MessageId');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [NotificationLog] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [NotificationLog] DROP COLUMN [MessageId];

GO

ALTER TABLE [NotificationLog] ADD [Id] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

GO

ALTER TABLE [NotificationLog] ADD CONSTRAINT [PK_NotificationLog] PRIMARY KEY ([Id]);

GO

CREATE INDEX [IX_NotificationLog_UserId_ContactId] ON [NotificationLog] ([UserId], [ContactId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171202224407_NotificationLogKey', N'2.0.0-rtm-26452');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171203004449_NotificationLogPartitioning', N'2.0.0-rtm-26452');

GO

