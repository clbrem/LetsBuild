open System.Reflection
open DbUp
open DbUp.Engine
open DbUp.Support
open Microsoft.Extensions.Configuration
open System.IO

type Config() =    
     let builder =
            ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build()
     member this.LetsBuild() =
        builder.GetConnectionString("LetsBuild")
module Sql =
    let (|Upgrade|_|) (s: string) =
        s.EndsWith ".sql" && s.Split(".")[2] = "sql"
    let (|Script|_|) (s: string) =
        s.EndsWith ".sql" && s.Split(".")[2] = "scripts"
    let chain onError next : DatabaseUpgradeResult -> DatabaseUpgradeResult*int =
        function
        | result when result.Successful -> next(), 0
        | result -> onError result
    let handleError (result: DatabaseUpgradeResult) =
        printfn $"Error: %s{result.Error.Message}"
        result, 1
    let succeed (_,code) =
        printfn "Database upgrade successful."
        code 

[<EntryPoint>]
let main argv =
    let config = Config()
    let connectionString = config.LetsBuild()
    let assembly = Assembly.GetExecutingAssembly()
    let upgrader =
        DeployChanges.To            
            .MySqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                assembly,
                function | Sql.Upgrade -> true | _ -> false
                )
            .LogToConsole()
            .Build()
    let migrationOptions = SqlScriptOptions(ScriptType = ScriptType.RunAlways)
    let path = Path.GetFullPath("data/users.csv") 
    let dataMigration =
        DeployChanges.To
            .MySqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                (function | Sql.Script -> true | _ -> false),
                SqlScriptOptions(ScriptType = ScriptType.RunAlways)
                )
            .WithVariable("UserFile", path)
            .LogToConsole()
            .Build()
    
    upgrader.PerformUpgrade()
    |> Sql.chain Sql.handleError dataMigration.PerformUpgrade
    |> Sql.succeed
    
    
        
