﻿/*
Deployment script for MSA_Auth_DB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "MSA_Auth_DB"
:setvar DefaultFilePrefix "MSA_Auth_DB"
:setvar DefaultDataPath "C:\Users\jackm\AppData\Local\Microsoft\VisualStudio\SSDT\MSA_Auth"
:setvar DefaultLogPath "C:\Users\jackm\AppData\Local\Microsoft\VisualStudio\SSDT\MSA_Auth"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Dropping Default Constraint <unnamed>...';


GO
ALTER TABLE [dbo].[Account] DROP CONSTRAINT [DF__Account__Account__398D8EEE];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Account]...';


GO
ALTER TABLE [dbo].[Account]
    ADD DEFAULT USER FOR [AccountType];


GO
PRINT N'Update complete.';


GO
