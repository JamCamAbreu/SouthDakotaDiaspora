namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        Description = c.String(maxLength: 1024),
                        Platform = c.Int(nullable: false),
                        ReleaseDate = c.DateTime(),
                        PublishDate = c.DateTime(),
                        Author = c.String(),
                        NumberPages = c.Int(),
                        DateStarted = c.DateTime(),
                        ReleaseDate1 = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ProjectOwner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ProjectOwner_Id)
                .Index(t => t.ProjectOwner_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(maxLength: 256),
                        DateCreated = c.DateTime(nullable: false),
                        UserId_Id = c.Int(),
                        Activity_Id = c.Int(),
                        TimelineEvent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId_Id)
                .ForeignKey("dbo.Activities", t => t.Activity_Id)
                .ForeignKey("dbo.TimelineEvents", t => t.TimelineEvent_Id)
                .Index(t => t.UserId_Id)
                .Index(t => t.Activity_Id)
                .Index(t => t.TimelineEvent_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 32),
                        LastName = c.String(maxLength: 32),
                        Username = c.String(maxLength: 64),
                        Password = c.String(maxLength: 32),
                        UserRole = c.Int(nullable: false),
                        Project_Id = c.Int(),
                        TimelineEvent_Id = c.Int(),
                        TimelineEvent_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.Project_Id)
                .ForeignKey("dbo.TimelineEvents", t => t.TimelineEvent_Id)
                .ForeignKey("dbo.TimelineEvents", t => t.TimelineEvent_Id1)
                .Index(t => t.Project_Id)
                .Index(t => t.TimelineEvent_Id)
                .Index(t => t.TimelineEvent_Id1);
            
            CreateTable(
                "dbo.TimelineEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 128),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ActivityId_Id = c.Int(),
                        Host_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId_Id)
                .ForeignKey("dbo.Users", t => t.Host_Id)
                .Index(t => t.ActivityId_Id)
                .Index(t => t.Host_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "TimelineEvent_Id1", "dbo.TimelineEvents");
            DropForeignKey("dbo.Users", "TimelineEvent_Id", "dbo.TimelineEvents");
            DropForeignKey("dbo.TimelineEvents", "Host_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "TimelineEvent_Id", "dbo.TimelineEvents");
            DropForeignKey("dbo.TimelineEvents", "ActivityId_Id", "dbo.Activities");
            DropForeignKey("dbo.Activities", "ProjectOwner_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Project_Id", "dbo.Activities");
            DropForeignKey("dbo.Comments", "Activity_Id", "dbo.Activities");
            DropForeignKey("dbo.Comments", "UserId_Id", "dbo.Users");
            DropIndex("dbo.TimelineEvents", new[] { "Host_Id" });
            DropIndex("dbo.TimelineEvents", new[] { "ActivityId_Id" });
            DropIndex("dbo.Users", new[] { "TimelineEvent_Id1" });
            DropIndex("dbo.Users", new[] { "TimelineEvent_Id" });
            DropIndex("dbo.Users", new[] { "Project_Id" });
            DropIndex("dbo.Comments", new[] { "TimelineEvent_Id" });
            DropIndex("dbo.Comments", new[] { "Activity_Id" });
            DropIndex("dbo.Comments", new[] { "UserId_Id" });
            DropIndex("dbo.Activities", new[] { "ProjectOwner_Id" });
            DropTable("dbo.TimelineEvents");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Activities");
        }
    }
}
