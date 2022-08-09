
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/15/2021 09:42:55
-- Generated from EDMX file: D:\Freelancer\POS Printing Server\POS_PrintingServer\POS_PrintingServer\POS_PrintingServer_API\PrintingServerConfigDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PrintingServerConfigDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ClientDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientDetails];
GO
IF OBJECT_ID(N'[dbo].[ApplicationLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationLogs];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ClientDetails'
CREATE TABLE [dbo].[ClientDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] int  NOT NULL,
    [UUID] nvarchar(max)  NOT NULL,
    [OS] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [Architecture] nvarchar(max)  NULL,
    [PosType] nvarchar(max)  NULL,
    [PosDestination] nvarchar(max)  NULL,
    [PosHost] nvarchar(max)  NOT NULL,
    [PosPort] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL
);
GO

-- Creating table 'ApplicationLogs'
CREATE TABLE [dbo].[ApplicationLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ActionName] nvarchar(max)  NOT NULL,
    [JsonObject] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ClientDetails'
ALTER TABLE [dbo].[ClientDetails]
ADD CONSTRAINT [PK_ClientDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApplicationLogs'
ALTER TABLE [dbo].[ApplicationLogs]
ADD CONSTRAINT [PK_ApplicationLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------