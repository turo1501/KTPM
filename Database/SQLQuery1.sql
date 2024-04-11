
-- Bang AdminUser
CREATE TABLE [dbo].[AdminUser] (
    [ID]           INT            NOT NULL,
    [NameUser]     NVARCHAR (50) NULL,
    [RoleUser]     NVARCHAR (50) NULL,
    [PasswordUser] NCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

--Bang Category
CREATE TABLE [dbo].[Category] (
    [IDCate]   INT IDENTITY (1, 1) NOT NULL,
    [NameCate] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([IDCate] ASC)
);
--Bang Customer
CREATE TABLE [dbo].[Customer] (
    [IDCus]    INT  IDENTITY (1, 1) NOT NULL,
    [NameCus]  NVARCHAR (50) NULL,
    [PhoneCus] NVARCHAR (15)  NULL,
    [EmailCus] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([IDCus] ASC)
);

--Bang Products
CREATE TABLE [dbo].[Product] (
    [ProductID]     INT             IDENTITY (1, 1) NOT NULL,
    [NamePro]       NVARCHAR (50)  NULL,
    [DecriptionPro] NVARCHAR (50)  NULL,
    [CateID]        INT      NULL,
    [Price]         DECIMAL (18, 2) NULL,
    [ImagePro]      NVARCHAR (50)  NULL,
	[Quantity]		int		NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC),
    CONSTRAINT [FK_Pro_Category] FOREIGN KEY ([CateID]) REFERENCES [dbo].[Category] ([IDCate])
);
--Bang OrderPro
CREATE TABLE [dbo].[OrderPro] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [DateOrder]        DATE           NULL,
    [IDCus]            INT            NULL,
    [AddressDeliverry] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDCus]) REFERENCES [dbo].[Customer] ([IDCus])
);

--Bang OrderDetail
CREATE TABLE [dbo].[OrderDetail] (
    [ID]        INT        IDENTITY (1, 1) NOT NULL,
    [IDProduct] INT        NULL,
    [IDOrder]   INT        NULL,
    [Quantity]  INT        NULL,
    [UnitPrice] FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDProduct]) REFERENCES [dbo].[Product] ([ProductID]),
    FOREIGN KEY ([IDOrder]) REFERENCES [dbo].[OrderPro] ([ID])
);

-- Insert dữ liệu


------Category
Insert into Category (NameCate)
    Values(N'ADIDAS')
Insert into Category (NameCate)
    Values(N'CONVERSE')
Insert into Category (NameCate)
    Values(N'NIKE')
Insert into Category (NameCate)
    Values(N'PUMA')
Insert into Category (NameCate)
    Values(N'VANS')

--------Products
SET IDENTITY_INSERT [dbo].[Product] ON
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (3, N'Nike React Infinity Run Flyknit 3', N'Giày Sneaker hãng Nike nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',3, CAST(3600000.00 AS Decimal(18, 2)), N'~/Content/images/nike_RIRFlyknit3.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (4, N'Nike React Miler 3', N'Giày Sneaker hãng Nike nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 3, CAST(3500000.00 AS Decimal(18, 2)), N'~/Content/images/nike_RMiler3.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (5, N'Nike Metcon 7', N'Giày Sneaker hãng Nike nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',3, CAST(3829000.00 AS Decimal(18, 2)), N'~/Content/images/nike_Metcon7.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (6, N'Nike SB Zoom Blazer Mid EK', N'Giày Sneaker hãng Nike nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',3, CAST(2649000.00 AS Decimal(18, 2)), N'~/Content/images/nike_SBZBMidEK.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (7, N'Adidas Stan Smith ', N'Giày Sneaker hãng Adidas nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',1, CAST(2500000.00 AS Decimal(18, 2)), N'~/Content/images/stanSmith.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (8, N'Adidas Yeezy 350 V2 Beluga', N'Giày Sneaker hãng Adidas nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',1, CAST(6000000.00 AS Decimal(18, 2)), N'~/Content/images/y350V2_B.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (9, N'Adidas Harden Stepback 3 ', N'Giày Sneaker hãng Adidas nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',1, CAST(23000000.00 AS Decimal(18, 2)), N'~/Content/images/hardenS3.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (10, N'Adidas Dame 8', N'Giày Sneaker hãng Adidas nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',1, CAST(3200000.00 AS Decimal(18, 2)), N'~/Content/images/dame8.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (11, N'Adidas Harden Vol 6', N'Giày Sneaker hãng Adidas nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',1, CAST(4000000.00 AS Decimal(18, 2)), N'~/Content/images/hardenV6.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (12, N'Adidas Duramo 10', N'Giày Sneaker hãng Adidas nổi tiếng, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 1, CAST(1300000.00 AS Decimal(18, 2)), N'~/Content/images/duramo10.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (13, N'Vans Authentic Classic ', N'Giày Sneaker hãng Vans nổi tiếng với giới trẻ. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 5, CAST(1450000.00 AS Decimal(18, 2)), N'~/Content/images/vans1.png')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (14, N'Vans Vault OG Classic Slip On', N'Giày Sneaker hãng Vans nổi tiếng với giới trẻ. Mang phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 5, CAST(2600000.00 AS Decimal(18, 2)), N'~/Content/images/vans3.png')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (15, N'Vans Canvas Old Skool Classic True White', N'Giày Sneaker hãng Vans nổi tiếng với giới trẻ. Mang phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 5, CAST(1750000.00 AS Decimal(18, 2)), N'~/Content/images/vans5.png')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (16, N'Vans Classic SK8-HI', N'Giày Sneaker hãng Vans nổi tiếng với giới trẻ. Mang phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 5, CAST(1600000.00 AS Decimal(18, 2)), N'~/Content/images/vans6.png')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (17, N'Vans Ouroboros Slip-On', N'Giày Sneaker hãng Vans nổi tiếng với giới trẻ. Mang phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 5, CAST(1600000.00 AS Decimal(18, 2)), N'~/Content/images/vans7.png')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (18, N'Vans Ouroboros SK8-HI', N'Giày Sneaker hãng Vans nổi tiếng với giới trẻ. Mang phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 5, CAST(2100000.00 AS Decimal(18, 2)), N'~/Content/images/vans8.png')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (19, N'Vans x National Geographic SK8-HI Reissue 138', N'Giày Sneaker hãng Vans nổi tiếng với giới trẻ. Mang phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 5, CAST(3100000.00 AS Decimal(18, 2)), N'~/Content/images/vans9.png')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (20, N'Vans X Moca Judy Baca Authentic', N'Giày Sneaker hãng Vans nổi tiếng với giới trẻ. Mang phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 5, CAST(2850000.00 AS Decimal(18, 2)), N'~/Content/images/vans10.png')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (21, N'Converse Chuck Taylor All Star 70 Plus', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(2500000.00 AS Decimal(18, 2)), N'~/Content/images/Converse1.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (22, N'Converse Chuck Taylor All Star Dainty', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ',2, CAST(1500000.00 AS Decimal(18, 2)), N'~/Content/images/Converse2.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (23, N'Converse Chuck Taylor All Star Lift Platform Canvas', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(1800000.00 AS Decimal(18, 2)), N'~/Content/images/Converse3.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (24, N'Converse Chuck Taylor All Star 1970s Archive Paint Splatter', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(2500000.00 AS Decimal(18, 2)), N'~/Content/images/Converse4.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (25, N'Converse Chuck Taylor All Star 1970s My Story', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(2000000.00 AS Decimal(18, 2)), N'~/Content/images/Converse5.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (26, N'Converse Chuck Taylor All Star Double Stack Lift My Story', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(1800000.00 AS Decimal(18, 2)), N'~/Content/images/Converse6.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (27, N'Converse Chuck Taylor All Star Wordmark', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(1600000.00 AS Decimal(18, 2)), N'~/Content/images/Converse7.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (28, N'Converse Chuck Taylor All Star Gamer Low-Top', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(1000000.00 AS Decimal(18, 2)), N'~/Content/images/Converse8.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (29, N'Converse Chuck Taylor All Star Valentine''s Day', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(1600000.00 AS Decimal(18, 2)), N'~/Content/images/Converse9.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (30, N'Converse Chuck Taylor All Star 1970s Mi Gente', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(2000000.00 AS Decimal(18, 2)), N'~/Content/images/Converse10.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (31, N'Converse Chuck Taylor All Star Create Future', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(1600000.00 AS Decimal(18, 2)), N'~/Content/images/Converse12.jpg')
INSERT INTO [dbo].[Product] ([ProductID], [NamePro], [DecriptionPro], [CateID], [Price], [ImagePro]) VALUES (32, N'Converse Chuck Taylor All Star Twisted Classic Logo Play', N'Giày Sneaker hãng Converse đình đá. Phong cách Basic, đường phố nhưng không kém phần hiện đại, giày được hoàn thiện một cách tỉ mỉ, không chi tiết thừa, đủ size, đủ màu. Cam kết hàng chính hãng, phát hiện hàng giả hàng nhái đền bù gấp 10. ', 2, CAST(1600000.00 AS Decimal(18, 2)), N'~/Content/images/Converse11.jpg')
SET IDENTITY_INSERT [dbo].[Product] OFF


