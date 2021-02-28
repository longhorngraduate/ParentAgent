namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionHistoryModelAddedNumOfTimesSentToTeacher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionHistories", "NumOfTimesSentToTeacher", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionHistories", "NumOfTimesSentToTeacher");
        }
    }
}
