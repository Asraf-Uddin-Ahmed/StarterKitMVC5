namespace $safeprojectname$.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        DisplayName = c.String(nullable: false),
                        Value = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        TypeOfUser = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        LastLogin = c.DateTime(),
                        WrongPasswordAttempt = c.Int(nullable: false),
                        LastWrongPasswordAttempt = c.DateTime(),
                        CreationTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserVerifications",
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
            DropForeignKey("dbo.UserVerifications", "UserID", "dbo.Users");
            DropIndex("dbo.UserVerifications", new[] { "UserID" });
            DropTable("dbo.UserVerifications");
            DropTable("dbo.Users");
            DropTable("dbo.Settings");
        }
    }
}
