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

CREATE TABLE [ProductCategories] (
    [Id] int NOT NULL,
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

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Login] nvarchar(100) NOT NULL,
    [Name] nvarchar(100) NULL,
    [Password] nvarchar(200) NOT NULL,
    [PasswordEncryptionAlgorithm] int NOT NULL,
    [Salt] nvarchar(100) NOT NULL,
    [SessionId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Session_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Session] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Address] (
    [AddressId] uniqueidentifier NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [CountryId] varchar(2) NOT NULL,
    [IsBilling] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [StateId] varchar(2) NULL,
    [Street] nvarchar(400) NOT NULL,
    [UserId] int NOT NULL,
    [Zip] varchar(30) NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY ([AddressId]),
    CONSTRAINT [FK_Address_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Code]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Address_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Code]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Address_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Contacts] (
    [ContactId] uniqueidentifier NOT NULL,
    [Address] nvarchar(255) NOT NULL,
    [IsConfirmed] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsPrivate] bit NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Type] int NOT NULL,
    [UserId] int NOT NULL,
    [monthlyDeliveriesReport] bit NOT NULL,
    [monthlySalesReport] bit NOT NULL,
    [notifyAboutFeedback] bit NOT NULL,
    [notifyAboutNewOrder] bit NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([ContactId]),
    CONSTRAINT [FK_Contacts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [DeliverySites] (
    [Id] int NOT NULL,
    [Description] nvarchar(400) NOT NULL,
    [isCompany] bit NOT NULL,
    CONSTRAINT [PK_DeliverySites] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DeliverySites_Users_Id] FOREIGN KEY ([Id]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [PaymentMethods] (
    [PaymentMethodId] uniqueidentifier NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Type] int NOT NULL,
    [UIDesc] nvarchar(100) NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([PaymentMethodId]),
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
    [IsApproved] bit NOT NULL,
    [IsCompany] bit NOT NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Suppliers_Users_Id] FOREIGN KEY ([Id]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [NotificationLog] (
    [Id] uniqueidentifier NOT NULL,
    [ContactId] uniqueidentifier NOT NULL,
    [Date] datetime2(0) NOT NULL,
    [ErrorMessage] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [Subject] nvarchar(255) NULL,
    [Text] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_NotificationLog] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_NotificationLog_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [CardPaymentMethods] (
    [PaymentMethodId] uniqueidentifier NOT NULL,
    [BillingAddressId] uniqueidentifier NOT NULL,
    [CardNumber] nvarchar(16) NOT NULL,
    [NameOnCard] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_CardPaymentMethods] PRIMARY KEY ([PaymentMethodId]),
    CONSTRAINT [FK_CardPaymentMethods_Address_BillingAddressId] FOREIGN KEY ([BillingAddressId]) REFERENCES [Address] ([AddressId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CardPaymentMethods_PaymentMethods_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethods] ([PaymentMethodId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [PayPalPaymentMethods] (
    [PaymentMethodId] uniqueidentifier NOT NULL,
    [PayPalAddress] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_PayPalPaymentMethods] PRIMARY KEY ([PaymentMethodId]),
    CONSTRAINT [FK_PayPalPaymentMethods_PaymentMethods_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethods] ([PaymentMethodId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Orders] (
    [OrderId] uniqueidentifier NOT NULL,
    [DatePlaced] datetime2(0) NOT NULL,
    [LastUpdated] datetime2 NOT NULL,
    [PlannedDeliveryDate] datetime2(0) NOT NULL,
    [Price] decimal(18, 2) NOT NULL,
    [Status] int NOT NULL,
    [SupplierId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK_Orders_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Delivery] (
    [DeliveryId] uniqueidentifier NOT NULL,
    [CustomerAddressId] uniqueidentifier NOT NULL,
    [DeliverySiteId] int NOT NULL,
    [LastUpdated] datetime2 NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [Price] decimal(18, 2) NULL,
    [Status] int NOT NULL,
    [SupplierAddressId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Delivery] PRIMARY KEY ([DeliveryId]),
    CONSTRAINT [FK_Delivery_Address_CustomerAddressId] FOREIGN KEY ([CustomerAddressId]) REFERENCES [Address] ([AddressId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delivery_DeliverySites_DeliverySiteId] FOREIGN KEY ([DeliverySiteId]) REFERENCES [DeliverySites] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delivery_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delivery_Address_SupplierAddressId] FOREIGN KEY ([SupplierAddressId]) REFERENCES [Address] ([AddressId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Handshake] (
    [Id] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [TimeStamp] datetime2(0) NOT NULL,
    [Turn] int NOT NULL,
    CONSTRAINT [PK_Handshake] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Handshake_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrderItems] (
    [OrderId] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [IsCanceled] bit NOT NULL,
    [ProductCount] int NOT NULL,
    [SeqNumInOrer] int NOT NULL,
    [TotalPrice] decimal(18, 2) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([OrderId], [ProductId]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrderPaymentSensitiveInfo] (
    [OrderId] uniqueidentifier NOT NULL,
    [CVV] nvarchar(100) NOT NULL,
    [CardPaymentMethodId] uniqueidentifier NOT NULL,
    [ExpirationDate] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_OrderPaymentSensitiveInfo] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK_OrderPaymentSensitiveInfo_CardPaymentMethods_CardPaymentMethodId] FOREIGN KEY ([CardPaymentMethodId]) REFERENCES [CardPaymentMethods] ([PaymentMethodId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrderPaymentSensitiveInfo_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrderSubscription] (
    [ContactId] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_OrderSubscription] PRIMARY KEY ([ContactId], [OrderId]),
    CONSTRAINT [FK_OrderSubscription_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrderSubscription_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Payments] (
    [PaymentId] uniqueidentifier NOT NULL,
    [Amount] decimal(18, 2) NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [PaymentMethodId] uniqueidentifier NOT NULL,
    [Status] int NOT NULL,
    [Target] int NOT NULL,
    [TargetUserId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([PaymentId]),
    CONSTRAINT [FK_Payments_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_PaymentMethods_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethods] ([PaymentMethodId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_Users_TargetUserId] FOREIGN KEY ([TargetUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [HandshakeComments] (
    [HandshakeId] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [Comment] nvarchar(400) NOT NULL,
    [IsCanceled] bit NOT NULL,
    [NewPrice] decimal(18, 2) NULL,
    CONSTRAINT [PK_HandshakeComments] PRIMARY KEY ([HandshakeId], [ProductId]),
    CONSTRAINT [FK_HandshakeComments_Handshake_HandshakeId] FOREIGN KEY ([HandshakeId]) REFERENCES [Handshake] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Products] (
    [ProductId] uniqueidentifier NOT NULL,
    [AvailableNow] int NOT NULL,
    [Description] nvarchar(400) NOT NULL,
    [LastUpdated] datetime2 NOT NULL,
    [MainImageId] uniqueidentifier NULL,
    [Name] nvarchar(255) NOT NULL,
    [Price] decimal(18, 2) NOT NULL,
    [ProductCategoryId] int NOT NULL,
    [SupplierId] int NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK_Products_ProductCategories_ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Products_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [CartItems] (
    [Id] uniqueidentifier NOT NULL,
    [DatePlaced] datetime2(0) NOT NULL,
    [ProductCount] int NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [SessionId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CartItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CartItems_Session_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Session] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [ProductImages] (
    [Id] uniqueidentifier NOT NULL,
    [LogicalPath] nvarchar(100) NOT NULL,
    [Mime] nvarchar(40) NOT NULL,
    [Path] nvarchar(255) NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_ProductImages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [ProductReviews] (
    [ReviewId] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_ProductReviews] PRIMARY KEY ([ReviewId], [ProductId]),
    CONSTRAINT [FK_ProductReviews_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ProductReviews_Reviews_ReviewId] FOREIGN KEY ([ReviewId]) REFERENCES [Reviews] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Address_CountryId] ON [Address] ([CountryId]);

GO

CREATE INDEX [IX_Address_StateId] ON [Address] ([StateId]);

GO

CREATE INDEX [IX_Address_UserId] ON [Address] ([UserId]);

GO

CREATE INDEX [IX_CardPaymentMethods_BillingAddressId] ON [CardPaymentMethods] ([BillingAddressId]);

GO

CREATE INDEX [IX_CartItems_ProductId] ON [CartItems] ([ProductId]);

GO

CREATE INDEX [IX_CartItems_SessionId] ON [CartItems] ([SessionId]);

GO

CREATE INDEX [IX_Contacts_UserId] ON [Contacts] ([UserId]);

GO

CREATE INDEX [IX_Delivery_CustomerAddressId] ON [Delivery] ([CustomerAddressId]);

GO

CREATE INDEX [IX_Delivery_DeliverySiteId] ON [Delivery] ([DeliverySiteId]);

GO

CREATE INDEX [IX_Delivery_OrderId] ON [Delivery] ([OrderId]);

GO

CREATE INDEX [IX_Delivery_SupplierAddressId] ON [Delivery] ([SupplierAddressId]);

GO

CREATE INDEX [IX_Handshake_OrderId] ON [Handshake] ([OrderId]);

GO

CREATE INDEX [IX_NotificationLog_ContactId] ON [NotificationLog] ([ContactId]);

GO

CREATE INDEX [IX_OrderPaymentSensitiveInfo_CardPaymentMethodId] ON [OrderPaymentSensitiveInfo] ([CardPaymentMethodId]);

GO

CREATE INDEX [IX_Orders_SupplierId] ON [Orders] ([SupplierId]);

GO

CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);

GO

CREATE INDEX [IX_OrderSubscription_OrderId] ON [OrderSubscription] ([OrderId]);

GO

CREATE INDEX [IX_PaymentMethods_UserId] ON [PaymentMethods] ([UserId]);

GO

CREATE INDEX [IX_Payments_OrderId] ON [Payments] ([OrderId]);

GO

CREATE INDEX [IX_Payments_PaymentMethodId] ON [Payments] ([PaymentMethodId]);

GO

CREATE INDEX [IX_Payments_TargetUserId] ON [Payments] ([TargetUserId]);

GO

CREATE INDEX [IX_Payments_UserId] ON [Payments] ([UserId]);

GO

CREATE INDEX [IX_ProductImages_ProductId] ON [ProductImages] ([ProductId]);

GO

CREATE INDEX [IX_ProductReviews_ProductId] ON [ProductReviews] ([ProductId]);

GO

CREATE UNIQUE INDEX [IX_Products_MainImageId] ON [Products] ([MainImageId]) WHERE [MainImageId] IS NOT NULL;

GO

CREATE INDEX [IX_Products_ProductCategoryId] ON [Products] ([ProductCategoryId]);

GO

CREATE INDEX [IX_Products_SupplierId] ON [Products] ([SupplierId]);

GO

CREATE INDEX [IX_Reviews_UserId] ON [Reviews] ([UserId]);

GO

CREATE INDEX [IX_Reviews_TargetUserId_Date] ON [Reviews] ([TargetUserId], [Date]);

GO

CREATE UNIQUE INDEX [IX_Users_Login] ON [Users] ([Login]);

GO

CREATE INDEX [IX_Users_SessionId] ON [Users] ([SessionId]);

GO

ALTER TABLE [Products] ADD CONSTRAINT [FK_Products_ProductImages_MainImageId] FOREIGN KEY ([MainImageId]) REFERENCES [ProductImages] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171203163332_Schema_v2', N'2.0.0-rtm-26452');

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
VALUES (N'20171203163433_Init_Data', N'2.0.0-rtm-26452');

GO

DROP TABLE [NotificationLog];

GO

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

ALTER TABLE [dbo].[NotificationLog]  WITH CHECK ADD  CONSTRAINT [FK_NotificationLog_Contacts_UserId_ContactId] FOREIGN KEY([UserId], [ContactId])
REFERENCES [dbo].[Contacts] ([UserId], [ContactId])

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171203163535_NLog_partitioning', N'2.0.0-rtm-26452');

GO

