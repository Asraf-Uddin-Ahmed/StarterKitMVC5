namespace $safeprojectname$.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordVerification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordVerifications",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        VerificationCode = c.String(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PasswordVerifications", "UserID", "dbo.Users");
            DropIndex("dbo.PasswordVerifications", new[] { "UserID" });
            DropTable("dbo.PasswordVerifications");
        }
    }
}
