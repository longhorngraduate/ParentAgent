namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentModelCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parents",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Street1 = c.String(nullable: false, maxLength: 50),
                        Street2 = c.String(maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        State = c.String(nullable: false, maxLength: 50),
                        Zip = c.String(nullable: false, maxLength: 50),
                        NumOfChildren = c.String(nullable: false),
                        Active = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastConnect = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Parents");
        }
    }
}
