USE [master]
GO
/****** Object:  Database [StudentDebtsDB]    Script Date: 19.12.2024 10:43:24 ******/
CREATE DATABASE [StudentDebtsDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StudentDebtsDB', FILENAME = N'C:\Users\10241421\StudentDebtsDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'StudentDebtsDB_log', FILENAME = N'C:\Users\10241421\StudentDebtsDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [StudentDebtsDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StudentDebtsDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StudentDebtsDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [StudentDebtsDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StudentDebtsDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StudentDebtsDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [StudentDebtsDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StudentDebtsDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [StudentDebtsDB] SET  MULTI_USER 
GO
ALTER DATABASE [StudentDebtsDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StudentDebtsDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StudentDebtsDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StudentDebtsDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [StudentDebtsDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [StudentDebtsDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [StudentDebtsDB] SET QUERY_STORE = OFF
GO
USE [StudentDebtsDB]
GO
/****** Object:  Table [dbo].[SubjectDebts]    Script Date: 19.12.2024 10:43:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectDebts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentName] [nvarchar](100) NOT NULL,
	[Subject] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[DueDate] [datetime] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SubjectDebts] ON 

INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (1, N'Emily Smith', N'Mathematics', N'Missed class', CAST(N'2025-01-06T06:23:00.000' AS DateTime), N'Complete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (2, N'Emily Smith', N'Physics', N'Missed exam', CAST(N'2025-01-16T06:23:00.000' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (3, N'Alex Johnson', N'Mathematics', N'Missed exam', CAST(N'2024-12-23T06:23:00.000' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (4, N'Michael Brown', N'Chemistry', N'Missed exam', CAST(N'2024-12-22T06:23:00.000' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (5, N'Sophia Williams', N'Mathematics', N'Report not submitted', CAST(N'2025-01-15T06:23:00.000' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (6, N'Sophia Williams', N'Literature', N'Incomplete assignment', CAST(N'2025-01-05T06:23:00.000' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (7, N'Emily Smith', N'Literature', N'Missed class', CAST(N'2024-12-20T06:23:00.000' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (8, N'Alex Johnson', N'Chemistry', N'Missed exam', CAST(N'2025-01-07T06:23:00.000' AS DateTime), N'Complete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (9, N'Sophia Williams', N'Chemistry', N'Incomplete assignment', CAST(N'2025-01-18T06:23:00.000' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (10, N'Alex Johnson', N'Literature', N'Incomplete assignment', CAST(N'2024-12-25T06:23:00.000' AS DateTime), N'Complete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (11, N'Sophia Williams', N'History', N'Missed class', CAST(N'2025-01-12T06:23:00.000' AS DateTime), N'Complete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (12, N'James Davis', N'Chemistry', N'Missed exam', CAST(N'2025-01-09T06:23:00.000' AS DateTime), N'Complete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (13, N'Sophia Williams', N'History', N'Missed class', CAST(N'2025-01-02T06:23:00.000' AS DateTime), N'Complete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (14, N'Emily Smith', N'Physics', N'Missed exam', CAST(N'2024-12-28T06:23:00.000' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (16, N'emily', N'Chemistry', N'Missed exam', CAST(N'2024-12-19T10:16:58.970' AS DateTime), N'Incomplete')
INSERT [dbo].[SubjectDebts] ([Id], [StudentName], [Subject], [Description], [DueDate], [Status]) VALUES (17, N'aa', N'aa', N'aa', CAST(N'2024-12-19T10:28:06.850' AS DateTime), N'Complete')
SET IDENTITY_INSERT [dbo].[SubjectDebts] OFF
GO
USE [master]
GO
ALTER DATABASE [StudentDebtsDB] SET  READ_WRITE 
GO
