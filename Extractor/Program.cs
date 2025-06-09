using CharacterViewer.Core.Providers;
using DBCD.Providers;
using Extractor;
using Microsoft.Extensions.Configuration;
using ModelViewer.Core.Providers;


var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("config.json");
var config = configBuilder.Build();

var outputPath = config["OutputPath"] ?? "";
var cascPath = config["CASC:BasePath"];

Console.WriteLine("Starting extractor using CASC path: " + cascPath);
Console.WriteLine("Writing to output path: " + outputPath);

var fileDataProvider = new CASCFileDataProvider(config);
var dbcProvider = new FileDataDBCProvider(fileDataProvider);
var dbdProvider = new GithubDBDProvider();
var dbcd = new DBCD.DBCD(dbcProvider, dbdProvider);
var dbcdStorageProvider = new DBCDStorageProvider(dbcd);

var messageWriter = new ConsoleMessageWriter();
var extractComponent = new ExtractComponent(fileDataProvider, outputPath, messageWriter);
extractComponent.Initialize();

var toExtract = File.ReadAllLines("extract.txt").Select(x =>
{
    var split = x.Split("-");
    return new ExtractData()
    {
        FileId = uint.Parse(split[0]),
        Type = split[1]
    };
});

foreach (var todo in toExtract)
{
    messageWriter.WriteLine($"Processing file {todo.FileId} with filetype {todo.Type}");
    switch (todo.Type)
    {
        case "m2":
            {
                extractComponent.ExtractM2(todo.FileId);
                break;
            }
        case "wmo":
            {
                extractComponent.ExtractWmo(todo.FileId);
                break;
            }
    }
}

//builder.Services.AddSingleton<IFileDataProvider, CASCFileDataProvider>();
//builder.Services.AddSingleton<IDBCDStorageProvider, DBCDStorageProvider>();

////Add Transients;
//builder.Services.AddTransient<IDBCProvider, FileDataDBCProvider>();
//builder.Services.AddTransient<IDBDProvider, GithubDBDProvider>();
//builder.Services.AddTransient<DBCD.DBCD>(