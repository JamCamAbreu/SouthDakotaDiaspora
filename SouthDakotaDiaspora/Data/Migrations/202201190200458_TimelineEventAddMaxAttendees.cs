namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimelineEventAddMaxAttendees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimelineEvents", "MaxAttendees", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimelineEvents", "MaxAttendees");
        }
    }
}
