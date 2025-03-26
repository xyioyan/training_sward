CREATE TABLE [dbo].[customer_list] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [customer_code] VARCHAR (MAX) NOT NULL,
    [customer_name] VARCHAR (MAX) NOT NULL,
    [contact_no]    VARCHAR (15)  NOT NULL,
    [email]         VARCHAR (MAX) NOT NULL,
    [area]          VARCHAR (MAX) NOT NULL,
    [address]       VARCHAR (MAX) NOT NULL,
    [branch]        VARCHAR (MAX) NOT NULL,
    [status]        INT           NOT NULL,
    CONSTRAINT [PK_customer_list] PRIMARY KEY CLUSTERED ([Id] ASC)
);

