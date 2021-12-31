namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        ActivityType = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 64),
                        Description = c.String(maxLength: 1024),
                        Platform = c.Int(nullable: false),
                        WebsiteUrl = c.String(),
                        ReleaseDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        PublishDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Author = c.String(),
                        NumberPages = c.Int(),
                        DateStarted = c.DateTime(precision: 7, storeType: "datetime2"),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ProjectOwner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Users", t => t.ProjectOwner_UserId)
                .Index(t => t.ProjectOwner_UserId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Message = c.String(maxLength: 256),
                        DateCreated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId_UserId = c.Int(),
                        TimelineEvent_TimelineEventId = c.Int(),
                        Activity_ActivityId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Users", t => t.UserId_UserId)
                .ForeignKey("dbo.TimelineEvents", t => t.TimelineEvent_TimelineEventId)
                .ForeignKey("dbo.Activities", t => t.Activity_ActivityId)
                .Index(t => t.UserId_UserId)
                .Index(t => t.TimelineEvent_TimelineEventId)
                .Index(t => t.Activity_ActivityId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 32),
                        LastName = c.String(maxLength: 32),
                        Username = c.String(maxLength: 64),
                        Password = c.String(maxLength: 32),
                        UserRole = c.Int(nullable: false),
                        TimeZonePreference = c.Int(nullable: false),
                        LastLogin = c.DateTime(precision: 7, storeType: "datetime2"),
                        Project_ActivityId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Activities", t => t.Project_ActivityId)
                .Index(t => t.Project_ActivityId);
            
            CreateTable(
                "dbo.TimelineEvents",
                c => new
                    {
                        TimelineEventId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 128),
                        ActivityId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SentNotificationStarting = c.Boolean(nullable: false),
                        SentNotificationSoon = c.Boolean(nullable: false),
                        Host_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TimelineEventId)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Host_UserId)
                .Index(t => t.ActivityId)
                .Index(t => t.Host_UserId);
            
            CreateTable(
                "dbo.TimelineEventUser",
                c => new
                    {
                        TimelineEventRefId = c.Int(nullable: false),
                        UserRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TimelineEventRefId, t.UserRefId })
                .ForeignKey("dbo.TimelineEvents", t => t.TimelineEventRefId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserRefId, cascadeDelete: true)
                .Index(t => t.TimelineEventRefId)
                .Index(t => t.UserRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "ProjectOwner_UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Project_ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Comments", "Activity_ActivityId", "dbo.Activities");
            DropForeignKey("dbo.TimelineEventUser", "UserRefId", "dbo.Users");
            DropForeignKey("dbo.TimelineEventUser", "TimelineEventRefId", "dbo.TimelineEvents");
            DropForeignKey("dbo.TimelineEvents", "Host_UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "TimelineEvent_TimelineEventId", "dbo.TimelineEvents");
            DropForeignKey("dbo.TimelineEvents", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Comments", "UserId_UserId", "dbo.Users");
            DropIndex("dbo.TimelineEventUser", new[] { "UserRefId" });
            DropIndex("dbo.TimelineEventUser", new[] { "TimelineEventRefId" });
            DropIndex("dbo.TimelineEvents", new[] { "Host_UserId" });
            DropIndex("dbo.TimelineEvents", new[] { "ActivityId" });
            DropIndex("dbo.Users", new[] { "Project_ActivityId" });
            DropIndex("dbo.Comments", new[] { "Activity_ActivityId" });
            DropIndex("dbo.Comments", new[] { "TimelineEvent_TimelineEventId" });
            DropIndex("dbo.Comments", new[] { "UserId_UserId" });
            DropIndex("dbo.Activities", new[] { "ProjectOwner_UserId" });
            DropTable("dbo.TimelineEventUser");
            DropTable("dbo.TimelineEvents");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Activities");
        }
    }
}
