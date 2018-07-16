IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [AdminAccount] (
        [adminId] nvarchar(255) NOT NULL,
        [adminPassword] nvarchar(255) NULL,
        [email] nvarchar(255) NULL,
        [phone] nvarchar(255) NULL,
        CONSTRAINT [PK_AdminAccount] PRIMARY KEY ([adminId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [BankAccount] (
        [cardNumber] nvarchar(50) NOT NULL,
        [BankId] int NULL,
        [email] nvarchar(255) NULL,
        [expDate] date NULL,
        [ownerName] nvarchar(255) NULL,
        CONSTRAINT [PK_BankAccount] PRIMARY KEY ([cardNumber])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [BankList] (
        [bankId] int NOT NULL IDENTITY,
        [bankName] nvarchar(255) NULL,
        [imgLogo] nvarchar(255) NULL,
        CONSTRAINT [PK_BankList] PRIMARY KEY ([bankId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [Customer] (
        [customerId] int NOT NULL IDENTITY,
        [email] nvarchar(255) NULL,
        [phone] nvarchar(255) NULL,
        [userId] nvarchar(255) NULL,
        CONSTRAINT [PK_Customer] PRIMARY KEY ([customerId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [DigitalType] (
        [digTypeId] int NOT NULL IDENTITY,
        [name] nvarchar(255) NULL,
        CONSTRAINT [PK_DigitalType] PRIMARY KEY ([digTypeId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [Film] (
        [filmId] int NOT NULL IDENTITY,
        [actorList] nvarchar(255) NULL,
        [additionPicture] nvarchar(1000) NULL,
        [author] nvarchar(255) NULL,
        [countries] nvarchar(255) NULL,
        [dateRelease] datetime NOT NULL,
        [digTypeId] nvarchar(50) NULL,
        [filmContent] nvarchar(1000) NULL,
        [filmLength] int NULL,
        [filmStatus] int NULL,
        [imdb] float NULL,
        [movieGenre] nvarchar(50) NULL,
        [name] nvarchar(255) NULL,
        [posterPicture] nvarchar(255) NULL,
        [restricted] int NULL,
        [trailerLink] nvarchar(500) NULL,
        CONSTRAINT [PK_Film] PRIMARY KEY ([filmId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [GroupCinema] (
        [GroupId] int NOT NULL IDENTITY,
        [logoImg] nvarchar(50) NULL,
        [name] nvarchar(255) NULL,
        CONSTRAINT [PK_GroupCinema] PRIMARY KEY ([GroupId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [ShowTime] (
        [timeId] int NOT NULL IDENTITY,
        [endTime] datetime NULL,
        [startTime] datetime NULL,
        CONSTRAINT [PK_ShowTime] PRIMARY KEY ([timeId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [UserAccount] (
        [userId] nvarchar(255) NOT NULL,
        [email] nvarchar(255) NULL,
        [phone] nvarchar(255) NULL,
        [userPassword] nvarchar(255) NULL,
        CONSTRAINT [PK_UserAccount] PRIMARY KEY ([userId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [BookingTicket] (
        [bookingId] int NOT NULL IDENTITY,
        [bookingDate] datetime NULL,
        [customerId] int NULL,
        [paymentMethodId] int NULL,
        [quantity] int NULL,
        CONSTRAINT [PK_BookingTicket] PRIMARY KEY ([bookingId]),
        CONSTRAINT [FKBookingTicketCustomer001] FOREIGN KEY ([customerId]) REFERENCES [Customer] ([customerId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [News] (
        [newsId] int NOT NULL IDENTITY,
        [filmId] int NULL,
        [urlDocument] nvarchar(255) NULL,
        CONSTRAINT [PK_News] PRIMARY KEY ([newsId]),
        CONSTRAINT [FKNewsFilm001] FOREIGN KEY ([filmId]) REFERENCES [Film] ([filmId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [Cinema] (
        [cinemaId] int NOT NULL IDENTITY,
        [cinemaAddress] nvarchar(255) NULL,
        [email] nvarchar(200) NULL,
        [groupId] int NULL,
        [introduction] nvarchar(1000) NULL,
        [openTime] nvarchar(200) NULL,
        [phone] nvarchar(200) NULL,
        [profilePicture] nvarchar(255) NULL,
        CONSTRAINT [PK_Cinema] PRIMARY KEY ([cinemaId]),
        CONSTRAINT [FKCinemaGroupCinema001] FOREIGN KEY ([groupId]) REFERENCES [GroupCinema] ([GroupId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [PartnerAccount] (
        [partnerId] nvarchar(255) NOT NULL,
        [email] nvarchar(255) NULL,
        [groupOfCinemaId] int NULL,
        [partnerPassword] nvarchar(255) NULL,
        [phone] nvarchar(255) NULL,
        CONSTRAINT [PK_PartnerAccount] PRIMARY KEY ([partnerId]),
        CONSTRAINT [FKPartnerAccountGroupCinema001] FOREIGN KEY ([groupOfCinemaId]) REFERENCES [GroupCinema] ([GroupId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [TypeOfSeat] (
        [typeSeatId] int NOT NULL IDENTITY,
        [capacity] int NULL,
        [groupId] int NULL,
        [price] float NULL,
        [typeName] int NULL,
        CONSTRAINT [PK_TypeOfSeat] PRIMARY KEY ([typeSeatId]),
        CONSTRAINT [FKTypeOfSeatGroupCinema001] FOREIGN KEY ([groupId]) REFERENCES [GroupCinema] ([GroupId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [CinemaManager] (
        [managerId] nvarchar(255) NOT NULL,
        [cinemaId] int NULL,
        [email] nvarchar(255) NULL,
        [managerPassword] nvarchar(255) NULL,
        [phone] nvarchar(255) NULL,
        CONSTRAINT [PK_CinemaManager] PRIMARY KEY ([managerId]),
        CONSTRAINT [FKCinemaManagerCinema001] FOREIGN KEY ([cinemaId]) REFERENCES [Cinema] ([cinemaId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [Promotion] (
        [promotionId] int NOT NULL IDENTITY,
        [cinemaId] int NULL,
        [urlDocument] nvarchar(255) NULL,
        CONSTRAINT [PK_Promotion] PRIMARY KEY ([promotionId]),
        CONSTRAINT [FKPromotionCinema001] FOREIGN KEY ([cinemaId]) REFERENCES [Cinema] ([cinemaId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [Room] (
        [roomId] int NOT NULL IDENTITY,
        [capacity] int NULL,
        [cinemaId] int NULL,
        [digTypeId] int NULL,
        [name] nvarchar(15) NULL,
        CONSTRAINT [PK_Room] PRIMARY KEY ([roomId]),
        CONSTRAINT [FKRoomCinema001] FOREIGN KEY ([cinemaId]) REFERENCES [Cinema] ([cinemaId]) ON DELETE NO ACTION,
        CONSTRAINT [FKRoomDigitalType001] FOREIGN KEY ([digTypeId]) REFERENCES [DigitalType] ([digTypeId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [MovieSchedule] (
        [scheduleId] int NOT NULL IDENTITY,
        [filmId] int NULL,
        [roomId] int NULL,
        [timeId] int NULL,
        CONSTRAINT [PK_MovieSchedule] PRIMARY KEY ([scheduleId]),
        CONSTRAINT [FKMovieScheduleFilm001] FOREIGN KEY ([filmId]) REFERENCES [Film] ([filmId]) ON DELETE NO ACTION,
        CONSTRAINT [FKMovieScheduleRoom001] FOREIGN KEY ([roomId]) REFERENCES [Room] ([roomId]) ON DELETE NO ACTION,
        CONSTRAINT [FKMovieScheduleShowTime001] FOREIGN KEY ([timeId]) REFERENCES [ShowTime] ([timeId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [Seat] (
        [seatId] int NOT NULL IDENTITY,
        [px] int NULL,
        [py] int NULL,
        [roomId] int NULL,
        [typeSeatId] int NULL,
        CONSTRAINT [PK_Seat] PRIMARY KEY ([seatId]),
        CONSTRAINT [FKSeatRoom001] FOREIGN KEY ([roomId]) REFERENCES [Room] ([roomId]) ON DELETE NO ACTION,
        CONSTRAINT [FKSeatTypeOfSeat001] FOREIGN KEY ([typeSeatId]) REFERENCES [TypeOfSeat] ([typeSeatId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [Ticket] (
        [ticketId] int NOT NULL IDENTITY,
        [paymentCode] nvarchar(50) NULL,
        [price] float NULL,
        [qrCode] int NULL,
        [scheduleId] int NULL,
        [seatId] int NULL,
        [ticketStatus] nvarchar(20) NULL,
        CONSTRAINT [PK_Ticket] PRIMARY KEY ([ticketId]),
        CONSTRAINT [FKTicketMovieSchedule001] FOREIGN KEY ([scheduleId]) REFERENCES [MovieSchedule] ([scheduleId]) ON DELETE NO ACTION,
        CONSTRAINT [FKTicketSeat001] FOREIGN KEY ([seatId]) REFERENCES [Seat] ([seatId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE TABLE [BookingDetail] (
        [bookingDetailId] int NOT NULL IDENTITY,
        [bookingId] int NULL,
        [ticketId] int NULL,
        CONSTRAINT [PK_BookingDetail] PRIMARY KEY ([bookingDetailId]),
        CONSTRAINT [FKBookingDetailBookingTicket001] FOREIGN KEY ([bookingId]) REFERENCES [BookingTicket] ([bookingId]) ON DELETE NO ACTION,
        CONSTRAINT [FKBookingDetailTicket001] FOREIGN KEY ([ticketId]) REFERENCES [Ticket] ([ticketId]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_BookingDetail_bookingId] ON [BookingDetail] ([bookingId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_BookingDetail_ticketId] ON [BookingDetail] ([ticketId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_BookingTicket_customerId] ON [BookingTicket] ([customerId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_Cinema_groupId] ON [Cinema] ([groupId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_CinemaManager_cinemaId] ON [CinemaManager] ([cinemaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_MovieSchedule_filmId] ON [MovieSchedule] ([filmId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_MovieSchedule_roomId] ON [MovieSchedule] ([roomId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_MovieSchedule_timeId] ON [MovieSchedule] ([timeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_News_filmId] ON [News] ([filmId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_PartnerAccount_groupOfCinemaId] ON [PartnerAccount] ([groupOfCinemaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_Promotion_cinemaId] ON [Promotion] ([cinemaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_Room_cinemaId] ON [Room] ([cinemaId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_Room_digTypeId] ON [Room] ([digTypeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_Seat_roomId] ON [Seat] ([roomId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_Seat_typeSeatId] ON [Seat] ([typeSeatId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_Ticket_scheduleId] ON [Ticket] ([scheduleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_Ticket_seatId] ON [Ticket] ([seatId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    CREATE INDEX [IX_TypeOfSeat_groupId] ON [TypeOfSeat] ([groupId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180604222810_InitialDb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20180604222810_InitialDb', N'2.0.3-rtm-10026');
END;

GO

