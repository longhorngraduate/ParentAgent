namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionHistoryModelAddedParent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionHistories", "Parent_ParentId", c => c.Int());
            CreateIndex("dbo.QuestionHistories", "Parent_ParentId");
            AddForeignKey("dbo.QuestionHistories", "Parent_ParentId", "dbo.Parents", "ParentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionHistories", "Parent_ParentId", "dbo.Parents");
            DropIndex("dbo.QuestionHistories", new[] { "Parent_ParentId" });
            DropColumn("dbo.QuestionHistories", "Parent_ParentId");
        }
    }
}
