namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'd1394df9-260c-401c-99d2-f47ba35ceebc', N'admin@parentagent.com', 0, N'AECmPnNwe8cnY563K5fGQ37l96zWMZgjL2Dr0Onq41dyjPXvXwNBBFQFZGEfFhoZ3A==', N'f4afd6f7-b6b8-4e9b-8bca-b22a3ab447c0', NULL, 0, 0, NULL, 1, 0, N'admin@parentagent.com');
            
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'0b7f74ba-8b1a-4351-93dc-8a302c31171e', N'omniscient');
                
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'd1394df9-260c-401c-99d2-f47ba35ceebc', N'0b7f74ba-8b1a-4351-93dc-8a302c31171e');
            ");
        }
        
        public override void Down()
        {
        }
    }
}
