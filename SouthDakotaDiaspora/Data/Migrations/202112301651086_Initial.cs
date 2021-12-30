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
                        Activity_ActivityId = c.Int(),
                        TimelineEvent_TimelineEventId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Users", t => t.UserId_UserId)
                .ForeignKey("dbo.Activities", t => t.Activity_ActivityId)
                .ForeignKey("dbo.TimelineEvents", t => t.TimelineEvent_TimelineEventId)
                .Index(t => t.UserId_UserId)
                .Index(t => t.Activity_ActivityId)
                .Index(t => t.TimelineEvent_TimelineEventId);
            
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
                        TimelineEvent_TimelineEventId = c.Int(),
                        TimelineEvent_TimelineEventId1 = c.Int(),
                        Project_ActivityId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.TimelineEvents", t => t.TimelineEvent_TimelineEventId)
                .ForeignKey("dbo.TimelineEvents", t => t.TimelineEvent_TimelineEventId1)
                .ForeignKey("dbo.Activities", t => t.Project_ActivityId)
                .Index(t => t.TimelineEvent_TimelineEventId)
                .Index(t => t.TimelineEvent_TimelineEventId1)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "ProjectOwner_UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Project_ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Users", "TimelineEvent_TimelineEventId1", "dbo.TimelineEvents");
            DropForeignKey("dbo.Users", "TimelineEvent_TimelineEventId", "dbo.TimelineEvents");
            DropForeignKey("dbo.TimelineEvents", "Host_UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "TimelineEvent_TimelineEventId", "dbo.TimelineEvents");
            DropForeignKey("dbo.TimelineEvents", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Comments", "Activity_ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Comments", "UserId_UserId", "dbo.Users");
            DropIndex("dbo.TimelineEvents", new[] { "Host_UserId" });
            DropIndex("dbo.TimelineEvents", new[] { "ActivityId" });
            DropIndex("dbo.Users", new[] { "Project_ActivityId" });
            DropIndex("dbo.Users", new[] { "TimelineEvent_TimelineEventId1" });
            DropIndex("dbo.Users", new[] { "TimelineEvent_TimelineEventId" });
            DropIndex("dbo.Comments", new[] { "TimelineEvent_TimelineEventId" });
            DropIndex("dbo.Comments", new[] { "Activity_ActivityId" });
            DropIndex("dbo.Comments", new[] { "UserId_UserId" });
            DropIndex("dbo.Activities", new[] { "ProjectOwner_UserId" });
            DropTable("dbo.TimelineEvents");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Activities");
        }
    }
}
