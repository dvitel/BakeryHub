IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [DeliverySites] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(400) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [isCompany] bit NOT NULL,
    CONSTRAINT [PK_DeliverySites] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [ProductCategories] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(400) NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_ProductCategories] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [States] (
    [Code] nvarchar(2) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_States] PRIMARY KEY ([Code])
);

GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Login] nvarchar(100) NOT NULL,
    [Password] nvarchar(100) NOT NULL,
    [PasswordEncryptionAlgorithm] int NOT NULL,
    [Salt] nvarchar(max) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Customers] (
    [Id] int NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Customers_Users_Id] FOREIGN KEY ([Id]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Suppliers] (
    [Id] int NOT NULL,
    [Description] nvarchar(400) NOT NULL,
    [IsCompany] bit NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Suppliers_Users_Id] FOREIGN KEY ([Id]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [CustomerAddress] (
    [CustomerId] int NOT NULL,
    [AddressId] int NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [StateId] nvarchar(2) NULL,
    [Street] nvarchar(400) NOT NULL,
    [Zip] nvarchar(30) NULL,
    CONSTRAINT [PK_CustomerAddress] PRIMARY KEY ([CustomerId], [AddressId]),
    CONSTRAINT [FK_CustomerAddress_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CustomerAddress_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Code]) ON DELETE NO ACTION
);

GO

CREATE TABLE [CustomerContacts] (
    [CustomerId] int NOT NULL,
    [ContactId] int NOT NULL,
    [Address] nvarchar(max) NULL,
    [IsConfirmed] bit NOT NULL,
    [Name] nvarchar(max) NULL,
    [Type] int NOT NULL,
    CONSTRAINT [PK_CustomerContacts] PRIMARY KEY ([CustomerId], [ContactId]),
    CONSTRAINT [FK_CustomerContacts_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Orders] (
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [DatePlaced] datetime2 NOT NULL,
    [Price] decimal(18, 2) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([CustomerId], [OrderId]),
    CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [PaymentMethods] (
    [CustomerId] int NOT NULL,
    [PaymentMethodId] int NOT NULL,
    [CardNumber] nvarchar(16) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [Country] nvarchar(100) NULL,
    [NameOnCard] nvarchar(100) NOT NULL,
    [StateId] nvarchar(2) NULL,
    [Street] nvarchar(400) NOT NULL,
    [Zip] nvarchar(30) NULL,
    CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([CustomerId], [PaymentMethodId]),
    CONSTRAINT [FK_PaymentMethods_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PaymentMethods_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Code]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Products] (
    [SupplierId] int NOT NULL,
    [ProductId] int NOT NULL,
    [AvailableNow] int NOT NULL,
    [CategoryId] int NOT NULL,
    [Description] nvarchar(400) NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    [Price] decimal(18, 2) NOT NULL,
    [SupplierId1] int NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([SupplierId], [ProductId]),
    CONSTRAINT [FK_Products_ProductCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Products_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Products_Suppliers_SupplierId1] FOREIGN KEY ([SupplierId1]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [SupplierAddress] (
    [SupplierId] int NOT NULL,
    [AddressId] int NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [StateId] nvarchar(2) NULL,
    [Street] nvarchar(400) NOT NULL,
    [Zip] nvarchar(30) NULL,
    [isUIVisible] bit NOT NULL,
    CONSTRAINT [PK_SupplierAddress] PRIMARY KEY ([SupplierId], [AddressId]),
    CONSTRAINT [FK_SupplierAddress_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [States] ([Code]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SupplierAddress_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [SupplierContacts] (
    [SupplierId] int NOT NULL,
    [ContactId] int NOT NULL,
    [Address] nvarchar(255) NOT NULL,
    [IsConfirmed] bit NOT NULL,
    [IsUIVisible] bit NOT NULL,
    [Name] nvarchar(max) NULL,
    [Type] int NOT NULL,
    CONSTRAINT [PK_SupplierContacts] PRIMARY KEY ([SupplierId], [ContactId]),
    CONSTRAINT [FK_SupplierContacts_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [SupplierReviews] (
    [SupplierId] int NOT NULL,
    [Date] datetime2 NOT NULL,
    [CustomerId] int NOT NULL,
    [Feedback] nvarchar(400) NOT NULL,
    [Rating] int NOT NULL,
    CONSTRAINT [PK_SupplierReviews] PRIMARY KEY ([SupplierId], [Date], [CustomerId]),
    CONSTRAINT [FK_SupplierReviews_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SupplierReviews_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [CustomerContactSubscription] (
    [CustomerId] int NOT NULL,
    [ContactId] int NOT NULL,
    [Purpose] int NOT NULL,
    CONSTRAINT [PK_CustomerContactSubscription] PRIMARY KEY ([CustomerId], [ContactId], [Purpose]),
    CONSTRAINT [FK_CustomerContactSubscription_CustomerContacts_CustomerId_ContactId] FOREIGN KEY ([CustomerId], [ContactId]) REFERENCES [CustomerContacts] ([CustomerId], [ContactId]) ON DELETE CASCADE
);

GO

CREATE TABLE [Delivery] (
    [OrderId] int NOT NULL,
    [SupplierId] int NOT NULL,
    [CustomerId] int NOT NULL,
    [CustomerAddressId] int NOT NULL,
    [DeliverySiteId] int NULL,
    [ExpectedDeliveryDate] datetime2 NOT NULL,
    [SupplierAddressId] int NOT NULL,
    CONSTRAINT [PK_Delivery] PRIMARY KEY ([OrderId], [SupplierId], [CustomerId]),
    CONSTRAINT [FK_Delivery_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Delivery_DeliverySites_DeliverySiteId] FOREIGN KEY ([DeliverySiteId]) REFERENCES [DeliverySites] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delivery_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Delivery_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OrderItems] (
    [CustomerId] int NOT NULL,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [PricePerItem] decimal(18, 2) NOT NULL,
    [ProductCount] int NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([CustomerId], [OrderId], [ProductId]),
    CONSTRAINT [FK_OrderItems_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE CASCADE
);

GO

CREATE TABLE [Payments] (
    [CustomerId] int NOT NULL,
    [PaymentId] int NOT NULL,
    [Amount] decimal(18, 2) NOT NULL,
    [CardPaymentMethodId] int NOT NULL,
    [OrderId] int NOT NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([CustomerId], [PaymentId]),
    CONSTRAINT [FK_Payments_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payments_Orders_CustomerId_OrderId] FOREIGN KEY ([CustomerId], [OrderId]) REFERENCES [Orders] ([CustomerId], [OrderId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Carts] (
    [CustomerId] int NOT NULL,
    [ItemId] int NOT NULL,
    [DatePlaced] datetime2 NOT NULL,
    [ProductCount] int NOT NULL,
    [ProductId] int NOT NULL,
    [SupplierId] int NOT NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY ([CustomerId], [ItemId]),
    CONSTRAINT [FK_Carts_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Carts_Products_SupplierId_ProductId] FOREIGN KEY ([SupplierId], [ProductId]) REFERENCES [Products] ([SupplierId], [ProductId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [ProductImages] (
    [SupplierId] int NOT NULL,
    [ProductId] int NOT NULL,
    [ImageId] int NOT NULL,
    [Height] int NOT NULL,
    [IsMain] bit NOT NULL,
    [Mime] nvarchar(max) NULL,
    [Path] nvarchar(max) NULL,
    [Width] int NOT NULL,
    CONSTRAINT [PK_ProductImages] PRIMARY KEY ([SupplierId], [ProductId], [ImageId]),
    CONSTRAINT [FK_ProductImages_Products_SupplierId_ProductId] FOREIGN KEY ([SupplierId], [ProductId]) REFERENCES [Products] ([SupplierId], [ProductId]) ON DELETE CASCADE
);

GO

CREATE TABLE [ProductReviews] (
    [ProductId] int NOT NULL,
    [Date] datetime2 NOT NULL,
    [CustomerId] int NOT NULL,
    [CustomerId1] int NULL,
    [Feedback] nvarchar(400) NOT NULL,
    [Rating] int NOT NULL,
    CONSTRAINT [PK_ProductReviews] PRIMARY KEY ([ProductId], [Date], [CustomerId]),
    CONSTRAINT [FK_ProductReviews_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductReviews_Customers_CustomerId1] FOREIGN KEY ([CustomerId1]) REFERENCES [Customers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ProductReviews_Products_CustomerId_ProductId] FOREIGN KEY ([CustomerId], [ProductId]) REFERENCES [Products] ([SupplierId], [ProductId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [SupplierContactSubscription] (
    [SupplierId] int NOT NULL,
    [ContactId] int NOT NULL,
    [Purpose] int NOT NULL,
    CONSTRAINT [PK_SupplierContactSubscription] PRIMARY KEY ([SupplierId], [ContactId], [Purpose]),
    CONSTRAINT [FK_SupplierContactSubscription_SupplierContacts_SupplierId_ContactId] FOREIGN KEY ([SupplierId], [ContactId]) REFERENCES [SupplierContacts] ([SupplierId], [ContactId]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Carts_SupplierId_ProductId] ON [Carts] ([SupplierId], [ProductId]);

GO

CREATE UNIQUE INDEX [IX_CustomerAddress_StateId] ON [CustomerAddress] ([StateId]) WHERE [StateId] IS NOT NULL;

GO

CREATE INDEX [IX_Delivery_DeliverySiteId] ON [Delivery] ([DeliverySiteId]);

GO

CREATE INDEX [IX_Delivery_SupplierId] ON [Delivery] ([SupplierId]);

GO

CREATE INDEX [IX_Delivery_CustomerId_OrderId] ON [Delivery] ([CustomerId], [OrderId]);

GO

CREATE INDEX [IX_PaymentMethods_StateId] ON [PaymentMethods] ([StateId]);

GO

CREATE UNIQUE INDEX [IX_Payments_CustomerId_OrderId] ON [Payments] ([CustomerId], [OrderId]);

GO

CREATE INDEX [IX_ProductReviews_CustomerId1] ON [ProductReviews] ([CustomerId1]);

GO

CREATE INDEX [IX_ProductReviews_CustomerId_ProductId] ON [ProductReviews] ([CustomerId], [ProductId]);

GO

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

GO

CREATE INDEX [IX_Products_SupplierId1] ON [Products] ([SupplierId1]);

GO

CREATE UNIQUE INDEX [IX_SupplierAddress_StateId] ON [SupplierAddress] ([StateId]) WHERE [StateId] IS NOT NULL;

GO

CREATE INDEX [IX_SupplierContactSubscription_SupplierId_ContactId] ON [SupplierContactSubscription] ([SupplierId], [ContactId]);

GO

CREATE INDEX [IX_SupplierReviews_CustomerId] ON [SupplierReviews] ([CustomerId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171127214539_Schema_v0', N'2.0.0-rtm-26452');

GO

