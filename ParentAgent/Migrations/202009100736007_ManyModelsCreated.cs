namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyModelsCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        TimeStart = c.String(nullable: false, maxLength: 25),
                        TimeEnd = c.String(nullable: false, maxLength: 25),
                        AP = c.Int(nullable: false),
                        SchoolName = c.String(nullable: false, maxLength: 100),
                        Teacher_TeacherId = c.Int(nullable: false),
                        Student_StudentId = c.Int(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Teachers", t => t.Teacher_TeacherId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .Index(t => t.Teacher_TeacherId)
                .Index(t => t.Student_StudentId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Gender = c.Int(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 50),
                        PreferredMethodOfContact = c.Int(nullable: false),
                        Student_StudentId = c.Int(),
                    })
                .PrimaryKey(t => t.TeacherId)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .Index(t => t.Student_StudentId);
            
            CreateTable(
                "dbo.QuestionHistories",
                c => new
                    {
                        QuestionHistoryId = c.Int(nullable: false, identity: true),
                        Answer = c.String(nullable: false),
                        OtherAnswer = c.String(),
                        DateAnswered = c.DateTime(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                        Question_QuestionId = c.Int(nullable: false),
                        Student_StudentId = c.Int(),
                        Student_StudentId1 = c.Int(),
                    })
                .PrimaryKey(t => t.QuestionHistoryId)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .ForeignKey("dbo.Students", t => t.Student_StudentId1)
                .Index(t => t.Course_CourseId)
                .Index(t => t.Question_QuestionId)
                .Index(t => t.Student_StudentId)
                .Index(t => t.Student_StudentId1);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId);
            
            AlterColumn("dbo.Parents", "Username", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Students", "Username", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionHistories", "Student_StudentId1", "dbo.Students");
            DropForeignKey("dbo.Teachers", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.QuestionHistories", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.QuestionHistories", "Question_QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuestionHistories", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.Courses", "Teacher_TeacherId", "dbo.Teachers");
            DropIndex("dbo.QuestionHistories", new[] { "Student_StudentId1" });
            DropIndex("dbo.QuestionHistories", new[] { "Student_StudentId" });
            DropIndex("dbo.QuestionHistories", new[] { "Question_QuestionId" });
            DropIndex("dbo.QuestionHistories", new[] { "Course_CourseId" });
            DropIndex("dbo.Teachers", new[] { "Student_StudentId" });
            DropIndex("dbo.Courses", new[] { "Student_StudentId" });
            DropIndex("dbo.Courses", new[] { "Teacher_TeacherId" });
            AlterColumn("dbo.Students", "Username", c => c.String(nullable: false));
            AlterColumn("dbo.Parents", "Username", c => c.String(nullable: false));
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionHistories");
            DropTable("dbo.Teachers");
            DropTable("dbo.Courses");
        }
    }
}
