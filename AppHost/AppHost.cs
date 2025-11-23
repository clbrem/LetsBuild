using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var password = builder.AddParameter("password", "example-password");
var connectionString  =  
    $"Server=localhost;Port=3306;Database=LetsBuild;User=root;Password={password};";


var mysql =
    builder
        .AddMySql("mysql-db")
        .WithPassword(password);
var mydb =
    mysql.AddDatabase("LetsBuild");

var databaseUpdate =
    builder.AddProject<LetsBuild_Database>("database")
        .WithEnvironment("ConnectionStrings__LetsBuild", connectionString )
        .WaitFor(mydb);


builder.Build().Run();