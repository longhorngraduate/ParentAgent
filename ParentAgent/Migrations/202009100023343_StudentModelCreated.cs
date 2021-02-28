namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentModelCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Gender = c.Int(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Active = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastActive = c.DateTime(),
                        Parent_ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Parents", t => t.Parent_ParentId)
                .Index(t => t.Parent_ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Parent_ParentId", "dbo.Parents");
            DropIndex("dbo.Students", new[] { "Parent_ParentId" });
            DropTable("dbo.Students");
        }
    }
}
