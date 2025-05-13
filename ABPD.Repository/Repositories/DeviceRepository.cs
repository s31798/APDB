using System.Data;
using APBD.Devices;
using Microsoft.Data.SqlClient;

namespace ABPD.Repository;

public class DeviceRepository : IDeviceRepository
{
    private readonly string _connectionString;

    public DeviceRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<ElectronicDevice>> GetDevicesAsync()
    {
        List<ElectronicDevice> devices = new();
        const string edQuery = @"
        SELECT e.Id, e.Name, e.IsOn, 
               ed.Ip, ed.NetworkName, 
               pc.OperatingSystem, 
               sw.Battery,
               e.RowVersion
        FROM ElectronicDevice e
        LEFT JOIN EmbeddedDevice ed ON e.Id = ed.Id
        LEFT JOIN PersonalComputer pc ON e.Id = pc.Id
        LEFT JOIN SmartWatch sw ON e.Id = sw.Id;
    ";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(edQuery, connection))
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    bool isOn = reader.GetBoolean(2);

                    byte[] rowVersion = reader["RowVersion"] as byte[];

                    ElectronicDevice device;

                    if (!reader.IsDBNull(3))
                    {
                        string ip = reader.GetString(3);
                        string networkName = reader.GetString(4);
                        device = new EmbeddedDevice(id, name, isOn, ip, networkName) { RowVersion = rowVersion };
                    }
                    else if (!reader.IsDBNull(5))
                    {
                        string os = reader.GetString(5);
                        device = new PersonalComputer(id, name, isOn, os) { RowVersion = rowVersion };
                    }
                    else if (!reader.IsDBNull(6))
                    {
                        int battery = reader.GetInt32(6);
                        device = new SmartWatch(id, name, isOn, battery) { RowVersion = rowVersion };
                    }
                    else
                    {
                        continue;
                    }

                    devices.Add(device);
                }
            }
        }

        return devices;
    }


    public async Task<ElectronicDevice?> GetDeviceByIdAsync(string id)
    {
        const string query = @"
            SELECT 
                e.Id, 
                e.Name, 
                e.IsOn,
                ed.Ip, 
                ed.NetworkName,
                pc.OperatingSystem,
                sw.Battery
            FROM ElectronicDevice e
            LEFT JOIN EmbeddedDevice ed ON e.Id = ed.Id
            LEFT JOIN PersonalComputer pc ON e.Id = pc.Id
            LEFT JOIN SmartWatch sw ON e.Id = sw.Id
            WHERE e.Id = @Id;
        ";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    string name = reader.GetString(1);
                    bool isOn = reader.GetBoolean(2);

                    if (!reader.IsDBNull(3))
                    {
                        string ip = reader.GetString(3);
                        string networkName = reader.GetString(4);
                        return new EmbeddedDevice(id, name, isOn, ip, networkName);
                    }
                    else if (!reader.IsDBNull(5))
                    {
                        string os = reader.GetString(5);
                        return new PersonalComputer(id, name, isOn, os);
                    }
                    else if (!reader.IsDBNull(6))
                    {
                        int battery = reader.GetInt32(6);
                        return new SmartWatch(id, name, isOn, battery);
                    }
                }
            }
        }

        return null;
    }


    public async Task<bool> AddEmbeddedDeviceAsync(string id, string name, bool isOn, string ipAddress,
        string? netWorkName)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlTransaction transaction = connection.BeginTransaction())
            using (SqlCommand command = new SqlCommand("AddEmbedded", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@DeviceId", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@IsEnabled", isOn);
                command.Parameters.AddWithValue("@IpAddress", ipAddress);
                command.Parameters.AddWithValue("@NetworkName", netWorkName);

                try
                {
                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Transaction failed: {ex.Message}");
                    return false;
                }
            }
        }

        return true;
    }

    public async Task<bool> AddSmartWatchAsync(string id, string name, bool isOn, int batteryPercentage)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlTransaction transaction = connection.BeginTransaction())
            using (SqlCommand command = new SqlCommand("AddSmartwatch", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@DeviceId", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@IsEnabled", isOn);
                command.Parameters.AddWithValue("@BatteryPercentage", batteryPercentage);

                try
                {
                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Failed to add smartwatch: {ex.Message}");
                    return false;
                }
            }
        }

        return true;
    }

    public async Task<bool> AddPersonalComputerAsync(string id, string name, bool isOn, string operatingSystem)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlTransaction transaction = connection.BeginTransaction())
            using (SqlCommand command = new SqlCommand("AddPersonalComputer", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@DeviceId", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@IsEnabled", isOn);
                command.Parameters.AddWithValue("@OperationSystem", operatingSystem);

                try
                {
                    await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error adding PC: {ex.Message}");
                    return false;
                }
            }
        }

        return true;
    }

    public async Task<bool> EditDeviceDataAsync(
        string id,
        string newName,
        string? newIp,
        string? newNetworkName,
        string? newOperatingSystem
      )
    {
        var device = await GetDeviceByIdAsync(id);
        if (device == null)
            return false;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int affectedRows = 0;

                    string updateBase = @"
                    UPDATE ElectronicDevice
                    SET Name = @Name
                    WHERE Id = @Id AND RowVersion = @RowVersion;";

                    using (SqlCommand cmd = new SqlCommand(updateBase, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Name", newName);
                        cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp).Value = device.RowVersion;

                        affectedRows = await cmd.ExecuteNonQueryAsync();
                    }

                    if (affectedRows == 0)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    switch (device)
                    {
                        case EmbeddedDevice:
                            string updateEmbedded = @"
                            UPDATE EmbeddedDevice
                            SET Ip = @Ip, NetworkName = @NetworkName
                            WHERE Id = @Id;";

                            using (SqlCommand cmd = new SqlCommand(updateEmbedded, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Id", id);
                                cmd.Parameters.AddWithValue("@Ip", newIp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@NetworkName", newNetworkName ?? (object)DBNull.Value);
                                await cmd.ExecuteNonQueryAsync();
                            }

                            break;

                        case PersonalComputer:
                            string updatePC = @"
                            UPDATE PersonalComputer
                            SET OperatingSystem = @OS
                            WHERE Id = @Id;";

                            using (SqlCommand cmd = new SqlCommand(updatePC, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Id", id);
                                cmd.Parameters.AddWithValue("@OS", newOperatingSystem ?? (object)DBNull.Value);
                                await cmd.ExecuteNonQueryAsync();
                            }

                            break;

                        default:
                            await transaction.RollbackAsync();
                            return false;
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }


    public async Task<bool> DeleteDeviceAsync(string id)
    {
        var device = await GetDeviceByIdAsync(id);
        if (device == null)
            return false;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    if (device is EmbeddedDevice)
                    {
                        string deleteEmbedded = "DELETE FROM EmbeddedDevice WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(deleteEmbedded, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    else if (device is PersonalComputer)
                    {
                        string deletePC = "DELETE FROM PersonalComputer WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(deletePC, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    else if (device is SmartWatch)
                    {
                        string deleteSW = "DELETE FROM SmartWatch WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(deleteSW, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }


                    string deleteBase = @"
                    DELETE FROM ElectronicDevice 
                    WHERE Id = @Id AND RowVersion = @RowVersion";

                    using (SqlCommand cmd = new SqlCommand(deleteBase, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp).Value =  device.RowVersion;

                        int affectedRows = await cmd.ExecuteNonQueryAsync();
                        if (affectedRows == 0)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}