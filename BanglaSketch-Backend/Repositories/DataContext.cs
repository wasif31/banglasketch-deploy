using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Model;
using User = Model.Users.User;

namespace Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<People> Peoples { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<TranslatedData> TranslatedData { get; set; }
    }

    // // // Migration Instructions ------>
    //
    // Go to Solution Directory in Terminal (Ex - C:\Users\BanglaSketch-CodeBase\BanglaSketch--Backend) ->
    //
    // dotnet ef migrations add InitialMigration --startup-project WebService --project Migrations
    // dotnet ef database update --startup-project WebService --project Migrations
    //
    // // // If Migrations is not successful then follow this (N.B. This will delete database) ->
    // 
    // Remove all .cs files from migrations folder 
    // From Solution Directory 
    // dotnet ef database drop -f -v --startup-project WebService --project Migrations
    // dotnet ef migrations add InitialMigration --startup-project WebService --project Migrations
    // dotnet ef database update --startup-project WebService --project Migrations
}
