namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionHistoryModelAddedParent1 : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                UPDATE  dbo.QuestionHistories
                SET     Parent_ParentId = 2
                WHERE   Parent_ParentId IS null;
            ");

            DropForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.QuestionHistories", "Parent_ParentId", "dbo.Parents");
            DropIndex("dbo.QuestionHistories", new[] { "Course_CourseId" });
            DropIndex("dbo.QuestionHistories", new[] { "Parent_ParentId" });
            AlterColumn("dbo.QuestionHistories", "Course_CourseId", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionHistories", "Parent_ParentId", c => c.Int(nullable: false));
            CreateIndex("dbo.QuestionHistories", "Course_CourseId");
            CreateIndex("dbo.QuestionHistories", "Parent_ParentId");
            AddForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses", "CourseId", cascadeDelete: true);
            AddForeignKey("dbo.QuestionHistories", "Parent_ParentId", "dbo.Parents", "ParentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionHistories", "Parent_ParentId", "dbo.Parents");
            DropForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.QuestionHistories", new[] { "Parent_ParentId" });
            DropIndex("dbo.QuestionHistories", new[] { "Course_CourseId" });
            AlterColumn("dbo.QuestionHistories", "Parent_ParentId", c => c.Int());
            AlterColumn("dbo.QuestionHistories", "Course_CourseId", c => c.Int());
            CreateIndex("dbo.QuestionHistories", "Parent_ParentId");
            CreateIndex("dbo.QuestionHistories", "Course_CourseId");
            AddForeignKey("dbo.QuestionHistories", "Parent_ParentId", "dbo.Parents", "ParentId");
            AddForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses", "CourseId");
        }
    }
}
