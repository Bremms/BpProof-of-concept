namespace ProjectLeague.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postmortem : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Client", newName: "Clients");
            RenameTable(name: "dbo.group", newName: "Groups");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Groups", newName: "group");
            RenameTable(name: "dbo.Clients", newName: "Client");
        }
    }
}
