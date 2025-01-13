

-- Category Table
CREATE TABLE [dbo].[Category](
	[Category_id] [int] NOT NULL,
	[Category_name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Member Table
CREATE TABLE [dbo].[Members](
	[member_id] [int] NOT NULL,
	[member_name] [varchar](50) NOT NULL,
	[group_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[member_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--Payment Hist table
CREATE TABLE [dbo].[PaymentDetails](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[group_id] [int] NOT NULL,
	[group_name] [varchar](50) NULL,
	[member_id] [int] NULL,
	[member_name] [varchar](50) NULL,
	[Category_id] [int] NULL,
	[Category_name] [varchar](50) NULL,
	[payment_date] [date] NULL,
	[payment_type] [varchar](10) NULL,
	[amount] [decimal](18, 2) NULL,
	[notes] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


-- Trip Group
CREATE TABLE [dbo].[TripGroup](
	[group_id] [int] NOT NULL,
	[group_name] [varchar](50) NOT NULL,
	[group_share] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[group_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TripGroup] ADD  DEFAULT ((0)) FOR [group_share]
GO

--- SP
CREATE OR ALTER Procedure [dbo].[get_member_amountPaid] @groupId int 
AS

Select m.member_id,m.member_name, COALESCE(Sum(Amount),0) as amount_spent from Members m
left join PaymentDetails p on p.group_id = m.group_id and p.member_id = m.member_id
where m.group_id = @groupId and m.member_name != 'TripAdvisor'
group by m.member_id, m.member_name, amount
order by member_name

--TEST SCRIPT
--EXEC get_member_amountPaid 1