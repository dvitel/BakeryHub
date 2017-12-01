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

