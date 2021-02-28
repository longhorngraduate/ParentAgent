namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParentModelChangedIdToParentId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Parents");
            AddColumn("dbo.Parents", "ParentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Parents", "ParentId");
            DropColumn("dbo.Parents", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Parents", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Parents");
            DropColumn("dbo.Parents", "ParentId");
            AddPrimaryKey("dbo.Parents", "Id");
        }
    }
}
