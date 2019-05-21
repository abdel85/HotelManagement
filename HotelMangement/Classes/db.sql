 CREATE TABLE [dbo].[Customer] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [firstName] VARCHAR (50)  NOT NULL,
    [lastName]  VARCHAR (50)  NOT NULL,
    [phone]     VARCHAR (50)  NOT NULL,
    [address]   VARCHAR (MAX) NOT NULL,
    [hotelId]   INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Booking] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [Code]            VARCHAR (50) NOT NULL,
    [CustomerId]      INT          NOT NULL,
    [RoomId]          INT          NOT NULL,
    [OccupatedNumber] INT          NOT NULL,
    [status]          INT          NOT NULL,
    [invoiceStatus]   INT          DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Hotel] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    [RoomsNumber] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[invoice] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CustomerId] INT          NOT NULL,
    [Price]      DECIMAL (18) NOT NULL,
    [Status]     INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Room] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Number]       INT          NOT NULL,
    [HotelId]      INT          NOT NULL,
    [OccupatedMax] INT          NOT NULL,
    [price]        DECIMAL (18) NOT NULL,
    [Status]       INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
