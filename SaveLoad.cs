using System;
using System.IO;
using System.Text.Json;
public class SaveLoad
{
    public static void SaveData()
    {
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        string json = JsonSerializer.Serialize(Program.accounts, options);
        File.WriteAllText("Accounts.json", json);
    }
    public static void LoadData()
    {
        if (!File.Exists("Accounts.json"))
        {
            Program.accounts = new List<Account>();
            return;
        }

        string json = File.ReadAllText("Accounts.json");

        Program.accounts = JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
    }
}