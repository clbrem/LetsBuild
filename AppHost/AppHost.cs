using AppHost;
using Microsoft.Extensions.Configuration;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var configuration =
    builder.Configuration.GetSection("LetsBuild").Get<LetsBuildConfig>();
var password = 
    builder.AddParameter(
        "password",
        () => "example-password",
        secret: true
        );


var mysql =
    builder
        .AddMySql("mysql-db")
        .WithPassword(password);

if (configuration?.UseVolumes ?? true)
{
    mysql.WithDataVolume();
}
        
var mydb =
    mysql.AddDatabase("LetsBuild");


var databaseUpdate =
    builder.AddProject<LetsBuild_Database>("database")
        .WithEnvironment("ConnectionStrings__LetsBuild", mydb.Resource.ConnectionStringExpression )
        .WaitFor(mydb);

builder.Build().Run();