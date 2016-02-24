
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/24/2016 15:41:05
-- Generated from EDMX file: D:\WIP\CustomerIdentityManagementPOC\CIM\CIM\CIM.Model\CimDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [testDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [CompanyId] int IDENTITY(1,1) NOT NULL,
    [CompanyName] nvarchar(max)  NOT NULL,
    [CompanyRegistrationNumber] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Telephone] nvarchar(max)  NOT NULL,
    [AreasOfOperation] nvarchar(max)  NOT NULL,
    [Telephones_TelephoneId] int  NOT NULL
);
GO

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [AddressId] int IDENTITY(1,1) NOT NULL,
    [AddressLine1] nvarchar(max)  NOT NULL,
    [AddressLine2] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NOT NULL,
    [Town] nvarchar(max)  NOT NULL,
    [PostCode] nvarchar(max)  NOT NULL,
    [CompanyId] nvarchar(max)  NOT NULL,
    [Companies_CompanyId] int  NOT NULL
);
GO

-- Creating table 'Telephones'
CREATE TABLE [dbo].[Telephones] (
    [TelephoneId] int IDENTITY(1,1) NOT NULL,
    [CountryCode] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [CompanyId] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CompanyId] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([CompanyId] ASC);
GO

-- Creating primary key on [AddressId] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([AddressId] ASC);
GO

-- Creating primary key on [TelephoneId] in table 'Telephones'
ALTER TABLE [dbo].[Telephones]
ADD CONSTRAINT [PK_Telephones]
    PRIMARY KEY CLUSTERED ([TelephoneId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Companies_CompanyId] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [FK_AddressCompany]
    FOREIGN KEY ([Companies_CompanyId])
    REFERENCES [dbo].[Companies]
        ([CompanyId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressCompany'
CREATE INDEX [IX_FK_AddressCompany]
ON [dbo].[Addresses]
    ([Companies_CompanyId]);
GO

-- Creating foreign key on [Telephones_TelephoneId] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_CompanyTelephone]
    FOREIGN KEY ([Telephones_TelephoneId])
    REFERENCES [dbo].[Telephones]
        ([TelephoneId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyTelephone'
CREATE INDEX [IX_FK_CompanyTelephone]
ON [dbo].[Companies]
    ([Telephones_TelephoneId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------