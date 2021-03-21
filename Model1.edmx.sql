
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/18/2021 11:18:40
-- Generated from EDMX file: C:\Users\noelc\Desktop\COLLEGE\OOD\project_book_info\S00199895_Mark_Curran_Book_App\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BookDB];
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

-- Creating table 'AuthorTBLs'
CREATE TABLE [dbo].[AuthorTBLs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL
);
GO

-- Creating table 'BookTBLs'
CREATE TABLE [dbo].[BookTBLs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AuthorTBLId] int  NULL,
    [Title] nvarchar(max)  NULL,
    [Author] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AuthorTBLs'
ALTER TABLE [dbo].[AuthorTBLs]
ADD CONSTRAINT [PK_AuthorTBLs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BookTBLs'
ALTER TABLE [dbo].[BookTBLs]
ADD CONSTRAINT [PK_BookTBLs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AuthorTBLId] in table 'BookTBLs'
ALTER TABLE [dbo].[BookTBLs]
ADD CONSTRAINT [FK_AuthorTBLBookTBL]
    FOREIGN KEY ([AuthorTBLId])
    REFERENCES [dbo].[AuthorTBLs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AuthorTBLBookTBL'
CREATE INDEX [IX_FK_AuthorTBLBookTBL]
ON [dbo].[BookTBLs]
    ([AuthorTBLId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------