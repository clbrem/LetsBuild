open DbUp
open DbUp.MySql
open Microsoft.Extensions.Configuration

type Config() =
    
     let builder =
            ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build()
     member this.LetsBuild() =
        builder.GetConnectionString("LetsBuild")
                    

[<EntryPoint>]
let main argv =
    let config = Config()
    let connectionString = config.LetsBuild()
    let upgrader =
        DeployChanges.To
            .MySqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build()
    
    match upgrader.TryConnect()
    with
    | true, _ ->
        printfn "Connected!"
        0
    | _ ->
        printfn "Can't connect"
        -1
    
        
