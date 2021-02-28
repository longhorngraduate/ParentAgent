namespace ParentAgent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseModelChangedAPtoClassType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "ClassType", c => c.Int(nullable: false));
            DropColumn("dbo.Courses", "AP");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "AP", c => c.Int(nullable: false));
            DropColumn("dbo.Courses", "ClassType");
        }
    }
}
