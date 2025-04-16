namespace DevicesManager.RestAPI.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/devices")]
[ApiController]
public class DevicesController : ControllerBase
{
    private static List<SmartWatch> _smartWatches = new List<SmartWatch>
    {
        new SmartWatch("SW-1", "Apple Watch", true, 90),
        new SmartWatch("SW-2", "Galaxy Watch", false, 78)
    };

    private static List<PersonalComputer> _personalComputers = new List<PersonalComputer>
    {
        new PersonalComputer("P-1", "Dell XPS", true, "Windows 11"),
        new PersonalComputer("P-2", "MacBook Pro", true, "macOS Ventura")
    };

    private static List<EmbeddedDevice> _embeddedDevices = new List<EmbeddedDevice>
    {
        new EmbeddedDevice("E-1", "EmbeddedDevice1", true, "192.198.1.6", "Network1"),
        new EmbeddedDevice("E-2", "EmbeddedDevice2", false, "192.198.2.1", "Network2")
    };

    private readonly DeviceManager<SmartWatch> _smartWatchManager = new DeviceManager<SmartWatch>(_smartWatches);
    private readonly DeviceManager<PersonalComputer> _pcManager = new DeviceManager<PersonalComputer>(_personalComputers);
    private readonly DeviceManager<EmbeddedDevice> _embeddedManager = new DeviceManager<EmbeddedDevice>(_embeddedDevices);

    [HttpGet]
    public IResult GetAllDevices([FromQuery] string type)
    {
        return type.ToLower() switch
        {
            "smartwatch" => Results.Ok(_smartWatchManager.GetAllDevices()),
            "personalcomputer" => Results.Ok(_pcManager.GetAllDevices()),
            "embeddeddevice" => Results.Ok(_embeddedManager.GetAllDevices()),
            _ => Results.BadRequest("Invalid device type")
        };
    }

    [HttpGet("{id}")]
    public IResult GetDeviceById(string id, [FromQuery] string type)
    {
        return type.ToLower() switch
        {
            "smartwatch" => Results.Ok(_smartWatchManager.GetDeviceById(id)) ?? Results.NotFound(),
            "personalcomputer" => Results.Ok(_pcManager.GetDeviceById(id)) ?? Results.NotFound(),
            "embeddeddevice" => Results.Ok(_embeddedManager.GetDeviceById(id)) ?? Results.NotFound(),
            _ => Results.BadRequest("Invalid device type")
        };
    }

    [HttpPost]
    public IResult PostDevice([FromQuery] string type, [FromBody] Device device)
    {
        switch (type.ToLower())
        {
            case "smartwatch":
                _smartWatchManager.Post((SmartWatch)device);
                break;
            case "personalcomputer":
                _pcManager.Post((PersonalComputer)device);
                break;
            case "embeddeddevice":
                _embeddedManager.Post((EmbeddedDevice)device);
                break;
            default:
                return Results.BadRequest("Invalid device type");
        }

        return Results.Created($"/api/devices/{device.Id}?type={type}", device);
    }

    [HttpPut("{id}")]
    public IResult EditDevice(string id, [FromQuery] string type, [FromBody] Device updatedDevice)
    {
        return type.ToLower() switch
        {
            "smartwatch" => _smartWatchManager.EditDevice(id, (SmartWatch)updatedDevice) ? Results.NoContent() : Results.NotFound(),
            "personalcomputer" => _pcManager.EditDevice(id, (PersonalComputer)updatedDevice) ? Results.NoContent() : Results.NotFound(),
            "embeddeddevice" => _embeddedManager.EditDevice(id, (EmbeddedDevice)updatedDevice) ? Results.NoContent() : Results.NotFound(),
            _ => Results.BadRequest("Invalid device type")
        };
    }

    [HttpDelete("{id}")]
    public IResult DeleteDevice(string id, [FromQuery] string type)
    {
        return type.ToLower() switch
        {
            "smartwatch" => _smartWatchManager.DeleteDevice(id) ? Results.NoContent() : Results.NotFound(),
            "personalcomputer" => _pcManager.DeleteDevice(id) ? Results.NoContent() : Results.NotFound(),
            "embeddeddevice" => _embeddedManager.DeleteDevice(id) ? Results.NoContent() : Results.NotFound(),
            _ => Results.BadRequest("Invalid device type")
        };
    }
}