namespace DevicesManager;

/// <summary>
/// Factory class for creating instances of <see cref="DeviceManager{T}"/>.
/// </summary>
public class DeviceManagerFactory
{
    /// <summary>
    /// Creates and returns a new instance of <see cref="DeviceManager{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of device managed by the <see cref="DeviceManager{T}"/>.</typeparam>
    /// <param name="devices">The initial list of devices.</param>
    /// <returns>A new instance of <see cref="DeviceManager{T}"/>.</returns>
    public static DeviceManager<T> CreateDeviceManager<T>(List<T> devices) where T : Device
    {
        return new DeviceManager<T>(devices);
    }
}