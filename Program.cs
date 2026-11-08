using v45MiniProject.Models;
using v45MiniProject.Presentation;
using v45MiniProject.Services;

string programFolder = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory, @"..\..\..\Data"));

FileHandler _fileHandlerAssets = new FileHandler(programFolder, "assets.json");
FileHandler _fileHandlerOffices = new FileHandler(programFolder, "offices.json");
_fileHandlerAssets.EnsureFileExists();
_fileHandlerOffices.EnsureFileExists();
string _officesFile = _fileHandlerOffices.ReadAllText();
string _assetsFile = _fileHandlerAssets.ReadAllText();

try
{
    // Read JSON files
    List<Device> devices =
        _fileHandlerAssets.ReadJson<Device>();

    List<Office> offices =
        _fileHandlerOffices.ReadJson<Office>();

    // Create DeviceManager
    DeviceManager deviceManager =
        new DeviceManager(devices, offices);

    // Display devices
    UI.ShowDevicesByOffice(deviceManager);
    Console.ReadLine();
}
catch (Exception ex)
{
    UI.ShowMessage(
        $"Something went wrong: {ex.Message}",
        "error");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
  
