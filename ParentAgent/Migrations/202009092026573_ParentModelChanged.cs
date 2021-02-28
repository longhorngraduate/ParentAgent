namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentModelChanged : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Parents");
            AddColumn("dbo.Parents", "Gender", c => c.Int(nullable: false));
            AlterColumn("dbo.Parents", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Parents", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Parents");
            AlterColumn("dbo.Parents", "Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Parents", "Gender");
            AddPrimaryKey("dbo.Parents", "Id");
        }
    }
}
