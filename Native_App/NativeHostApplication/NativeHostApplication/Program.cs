using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

class NativeMessagingHost
{
    public static void Main(string[] args){
        JObject data;
        String currentDirectory = Directory.GetCurrentDirectory();
        DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentDirectory);
        String NativeApplicationRoot = currentDirectoryInfo.Parent.Parent.Parent.Parent.FullName;
        while ((data = ReadFromExtension()) != null){
            var message = extractMessage(data);
            sendToStandardOutput(message);
            switch(message)
            {
                case "restart":{
                        System.Diagnostics.ProcessStartInfo restartAndStartChrome= new System.Diagnostics.ProcessStartInfo();
                        restartAndStartChrome.FileName = NativeApplicationRoot + "\\BatchCommands\\reboot_and_startChrome.bat";
                        restartAndStartChrome.Verb = "runas";
                        restartAndStartChrome.CreateNoWindow = true;
                        restartAndStartChrome.UseShellExecute = false;
                        restartAndStartChrome.WindowStyle= System.Diagnostics.ProcessWindowStyle.Hidden;
                        System.Diagnostics.Process.Start(restartAndStartChrome);
                    }
                    break;
                case "startChrome":{
                        Console.WriteLine("Start called");
                        System.Diagnostics.Process.Start(NativeApplicationRoot+"\\BatchCommands\\startChrome.bat");    
                    }
                    break;
                case "enableButton":{
                        System.Diagnostics.ProcessStartInfo enableWindowsButton = new System.Diagnostics.ProcessStartInfo();
                        enableWindowsButton.FileName = NativeApplicationRoot+"\\BatchCommands\\enableWindowsKey.bat";
                        enableWindowsButton.Verb = "runas";
                        //enableWindowsButton.CreateNoWindow = true;
                        //enableWindowsButton.UseShellExecute = false;
                        //enableWindowsButton.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        System.Diagnostics.Process.Start(enableWindowsButton);
                    }
                    break;
                case "disableButton":
                    {
                        System.Diagnostics.ProcessStartInfo disableWindowsButton = new System.Diagnostics.ProcessStartInfo();
                        disableWindowsButton.Verb = "runas";
                        disableWindowsButton.FileName = NativeApplicationRoot+"\\BatchCommands\\disableWindowsKey.bat";
                        //disableWindowsButton.CreateNoWindow = true;
                        //disableWindowsButton.UseShellExecute = false;
                        //disableWindowsButton.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        System.Diagnostics.Process.Start(disableWindowsButton);
                    }
                    break;
                case "deletePrinters":
                    {
                        System.Diagnostics.ProcessStartInfo deletePrinters = new System.Diagnostics.ProcessStartInfo();
                        deletePrinters.Verb = "runas";
                        deletePrinters.FileName = NativeApplicationRoot + "\\BatchCommands\\deleteLocalPrinters.bat";
                        deletePrinters.CreateNoWindow = true;
                        deletePrinters.UseShellExecute = false;
                        deletePrinters.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        System.Diagnostics.Process.Start(deletePrinters);
                    }
                    break;
                default:
                    return;
            }
        }
    }
    //Function that interprets the data in the message.
    public static string extractMessage(JObject data){
        var message = data["text"].Value<string>();
        switch (message){
            case "restart":
                return "restart";
            case "start chrome":
                return "startChrome";
            case "enable button":
                return "enableButton";
            case "disable button":
                return "disableButton";
            case "delete printers":
                return "deletePrinters";
            default:
                return "echo: " + message;
        }
    }

    //Function that reads the message passed by the extension.
    public static JObject ReadFromExtension(){
        var stdin = Console.OpenStandardInput();
        var length = 0;
        var lengthBytes = new byte[4];
        stdin.Read(lengthBytes, 0, 4);
        length = BitConverter.ToInt32(lengthBytes, 0);
        var buffer = new char[length];
        using (var reader = new StreamReader(stdin)){
            while (reader.Peek() >= 0){
                reader.Read(buffer, 0, buffer.Length);
            }
        }
        return (JObject)JsonConvert.DeserializeObject<JObject>(new string(buffer));
    }

    //Function that sends data to standard outpout
    public static void sendToStandardOutput(JToken data){
        var json = new JObject();
        json["data"] = data;
        var bytes = System.Text.Encoding.UTF8.GetBytes(json.ToString(Formatting.None));
        var stdout = Console.OpenStandardOutput();
        stdout.WriteByte((byte)((bytes.Length >> 0) & 0xFF));
        stdout.WriteByte((byte)((bytes.Length >> 8) & 0xFF));
        stdout.WriteByte((byte)((bytes.Length >> 16) & 0xFF));
        stdout.WriteByte((byte)((bytes.Length >> 24) & 0xFF));
        stdout.Write(bytes, 0, bytes.Length);
        stdout.Flush();
    }
}
