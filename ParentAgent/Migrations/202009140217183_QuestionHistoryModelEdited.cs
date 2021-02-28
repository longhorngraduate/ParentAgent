namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionHistoryModelEdited : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.QuestionHistories", "Question_QuestionId", "dbo.Questions");
            DropIndex("dbo.QuestionHistories", new[] { "Course_CourseId" });
            DropIndex("dbo.QuestionHistories", new[] { "Question_QuestionId" });
            AddColumn("dbo.QuestionHistories", "QuestionTxt", c => c.String(nullable: false));
            AlterColumn("dbo.QuestionHistories", "Answer", c => c.String());
            AlterColumn("dbo.QuestionHistories", "Course_CourseId", c => c.Int());
            AlterColumn("dbo.QuestionHistories", "Question_QuestionId", c => c.Int());
            CreateIndex("dbo.QuestionHistories", "Course_CourseId");
            CreateIndex("dbo.QuestionHistories", "Question_QuestionId");
            AddForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses", "CourseId");
            AddForeignKey("dbo.QuestionHistories", "Question_QuestionId", "dbo.Questions", "QuestionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionHistories", "Question_QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.QuestionHistories", new[] { "Question_QuestionId" });
            DropIndex("dbo.QuestionHistories", new[] { "Course_CourseId" });
            AlterColumn("dbo.QuestionHistories", "Question_QuestionId", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionHistories", "Course_CourseId", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionHistories", "Answer", c => c.String(nullable: false));
            DropColumn("dbo.QuestionHistories", "QuestionTxt");
            CreateIndex("dbo.QuestionHistories", "Question_QuestionId");
            CreateIndex("dbo.QuestionHistories", "Course_CourseId");
            AddForeignKey("dbo.QuestionHistories", "Question_QuestionId", "dbo.Questions", "QuestionId", cascadeDelete: true);
            AddForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
        }
    }
}
