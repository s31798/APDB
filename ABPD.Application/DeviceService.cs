using APBD.Devices;
using Microsoft.Data.SqlClient;

namespace ABPD.Application;

public class DeviceService : IDeviceService
{
    private string _connectionString;

    public DeviceService(string connectionString)
    {
        _connectionString = connectionString;
    }
    public List<ElectronicDevice> Devices()
    {
        List<ElectronicDevice> devices = [];
        const string edQuery =
            "SELECT e.Id, e.Name, e.IsOn, ed.Ip, ed.NetworkName, pc.OperatingSystem, sw.Battery FROM ElectronicDevice e LEFT JOIN EmbeddedDevice ed ON e.Id = ed.Id LEFT JOIN PersonalComputer pc ON e.Id = pc.Id LEFT JOIN SmartWatch sw ON e.Id = sw.Id;";
          
      
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(edQuery, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    bool isOn = reader.GetBoolean(2);

                    ElectronicDevice device;

                    if (!reader.IsDBNull(3))
                    {
                        string ip = reader.GetString(3);
                        string networkName = reader.GetString(4);
                        device = new EmbeddedDevice(id, name, isOn, ip, networkName);
                    }
                    else if (!reader.IsDBNull(5)) 
                    {
                        string os = reader.GetString(5);
                        device = new PersonalComputer(id, name, isOn, os);
                    }
                    else if (!reader.IsDBNull(6))
                    {
                        int battery = reader.GetInt32(6);
                        device = new SmartWatch(id, name, isOn, battery);
                    }
                    else
                    {
                        continue;
                    }

                    devices.Add(device);
                }
            }
            finally
            {
                reader.Close();
            }
        }
        return devices;  
    }

    public ElectronicDevice? GetDeviceById(string id)
    {
        const string query = @" SELECT 
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
        WHERE e.Id = @Id;";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
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
    
    public bool AddDevice(ElectronicDevice device)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
        connection.Open();
        SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            string prefix = device switch
            {
                SmartWatch => "sw",
                PersonalComputer => "pc",
                EmbeddedDevice => "ed",
                _ => "dev"
            };

            device.Id = $"{prefix}-{Guid.NewGuid().ToString("N")[..4]}";

            string insertBase = @"INSERT INTO ElectronicDevice (Id, Name, IsOn) 
                                  VALUES (@Id, @Name, @IsOn)";
            using (SqlCommand cmd = new SqlCommand(insertBase, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", device.Id);
                cmd.Parameters.AddWithValue("@Name", device.Name);
                cmd.Parameters.AddWithValue("@IsOn", device.IsOn);
                cmd.ExecuteNonQuery();
            }

            if (device is EmbeddedDevice embedded)
            {
                string insertEmbedded = @"INSERT INTO EmbeddedDevice (Id, Ip, NetworkName) 
                                          VALUES (@Id, @Ip, @NetworkName)";
                using (SqlCommand cmd = new SqlCommand(insertEmbedded, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@Id", embedded.Id);
                    cmd.Parameters.AddWithValue("@Ip", embedded.Ip);
                    cmd.Parameters.AddWithValue("@NetworkName", embedded.NetworkName);
                    cmd.ExecuteNonQuery();
                }
            }
            else if (device is PersonalComputer pc)
            {
                string insertPC = @"INSERT INTO PersonalComputer (Id, OperatingSystem) 
                                    VALUES (@Id, @OperatingSystem)";
                using (SqlCommand cmd = new SqlCommand(insertPC, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@Id", pc.Id);
                    cmd.Parameters.AddWithValue("@OperatingSystem", pc.OperatingSystem);
                    cmd.ExecuteNonQuery();
                }
            }
            else if (device is SmartWatch watch)
            {
                string insertSW = @"INSERT INTO SmartWatch (Id, Battery) 
                                    VALUES (@Id, @Battery)";
                using (SqlCommand cmd = new SqlCommand(insertSW, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@Id", watch.Id);
                    cmd.Parameters.AddWithValue("@Battery", watch.Battery);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                transaction.Rollback();
                return false;
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }
    }
    public bool EditDeviceData(string id, string newName, string? newIp = null, string? newNetworkName = null, string? newOperatingSystem = null)
    {
        var device = GetDeviceById(id);
        if (device == null) return false;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                string updateBase = @"UPDATE ElectronicDevice SET Name = @Name WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(updateBase, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", newName);
                    cmd.ExecuteNonQuery();
                }
                
                switch (device)
                {
                    case EmbeddedDevice _:
                        string updateEmbedded = @"UPDATE EmbeddedDevice SET Ip = @Ip, NetworkName = @NetworkName WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(updateEmbedded, connection, transaction))
                        {
                                cmd.Parameters.AddWithValue("@Id", id);
                                cmd.Parameters.AddWithValue("@Ip", newIp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@NetworkName", newNetworkName ?? (object)DBNull.Value);
                                cmd.ExecuteNonQuery();
                        }
                        break;
                    case PersonalComputer _:
                        string updatePC = @"UPDATE PersonalComputer SET OperatingSystem = @OS WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(updatePC, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@OS", newOperatingSystem ?? (object)DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }
                        break;

                    default:
                        transaction.Rollback();
                        return false;
                }

                    transaction.Commit();
                    return true;
            }
            finally
            {
                connection.Close();
            }
        }
    }
    
    public bool DeleteDevice(string id)
    {
        var device = GetDeviceById(id);
        if (device == null) return false;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                if (device is EmbeddedDevice)
                {
                    string deleteEmbedded = "DELETE FROM EmbeddedDevice WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(deleteEmbedded, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (device is PersonalComputer)
                {
                    string deletePC = "DELETE FROM PersonalComputer WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(deletePC, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (device is SmartWatch)
                {
                    string deleteSW = "DELETE FROM SmartWatch WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(deleteSW, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                
                string deleteBase = "DELETE FROM ElectronicDevice WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(deleteBase, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
    }


}