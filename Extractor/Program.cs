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
if (string.IsNullOrEmpty(cascPath))
{
    throw new Exception("A valid CASC path in the config is required to run the extractor.");
}

Console.WriteLine("Starting extractor using CASC path: " + cascPath);
Console.WriteLine("Writing to output path: " + outputPath);

//var fileDataProvider = new TACTSharpFileDataProvider(config, new HttpClient
var fallBackProvider = new CASCFileDataProvider(config);
var arctProvider = new ArctiumOverrideFileDataProvider(fallBackProvider, cascPath);
var dbcProvider = new FileDataDBCProvider(arctProvider);
var dbdProvider = new GithubDBDProvider();
var dbcd = new DBCD.DBCD(dbcProvider, dbdProvider);
var dbcdStorageProvider = new DBCDStorageProvider(dbcd);

var messageWriter = new ConsoleMessageWriter();
var extractComponent = new ExtractComponent(arctProvider, dbcdStorageProvider, outputPath, messageWriter);
extractComponent.Initialize();

// TODO: Make M2, WMO extraction automatic somehow
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
                extractComponent.ExtractTextureVariations(todo.FileId);
                break;
            }
        case "wmo":
            {
                extractComponent.ExtractWmo(todo.FileId);
                break;
            }
    }
}

extractComponent.ExtractLiquidTypes();
extractComponent.ExtractCharacterModelMetadata();
extractComponent.ExtractItemVisualMetadata();
extractComponent.ExtractItemMetadata();
extractComponent.ExtractItemDisplayInfosMetadata();
