namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionHistoryModelDateAnsweredNowNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuestionHistories", "DateAnswered", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuestionHistories", "DateAnswered", c => c.DateTime(nullable: false));
        }
    }
}
