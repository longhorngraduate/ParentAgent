namespace ParentAgent.Migrations
{
	using System;
	using System.Data.Entity.Migrations;
	
	public partial class QuestionHistoryModelAddedStudent : DbMigration
	{
		public override void Up()
		{
			Sql(@"UPDATE		qh
					SET			qh.Student_StudentId = c.Student_StudentId
					FROM		dbo.QuestionHistories qh
								INNER JOIN dbo.Courses c
									ON c.CourseId = qh.Course_CourseId;");

			DropForeignKey("dbo.QuestionHistories", "Student_StudentId1", "dbo.Students");
			DropForeignKey("dbo.QuestionHistories", "Student_StudentId", "dbo.Students");
			DropIndex("dbo.QuestionHistories", new[] { "Student_StudentId" });
			DropIndex("dbo.QuestionHistories", new[] { "Student_StudentId1" });
			AlterColumn("dbo.QuestionHistories", "Student_StudentId", c => c.Int(nullable: false));
			CreateIndex("dbo.QuestionHistories", "Student_StudentId");
			AddForeignKey("dbo.QuestionHistories", "Student_StudentId", "dbo.Students", "StudentId", cascadeDelete: true);
			DropColumn("dbo.QuestionHistories", "Student_StudentId1");
		}
		
		public override void Down()
		{
			AddColumn("dbo.QuestionHistories", "Student_StudentId1", c => c.Int());
			DropForeignKey("dbo.QuestionHistories", "Student_StudentId", "dbo.Students");
			DropIndex("dbo.QuestionHistories", new[] { "Student_StudentId" });
			AlterColumn("dbo.QuestionHistories", "Student_StudentId", c => c.Int());
			CreateIndex("dbo.QuestionHistories", "Student_StudentId1");
			CreateIndex("dbo.QuestionHistories", "Student_StudentId");
			AddForeignKey("dbo.QuestionHistories", "Student_StudentId", "dbo.Students", "StudentId");
			AddForeignKey("dbo.QuestionHistories", "Student_StudentId1", "dbo.Students", "StudentId");
		}
	}
}
