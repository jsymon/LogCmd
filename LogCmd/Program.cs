using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
namespace LogCmd
{
    class Program
    {
        private static string _output = null;

        private static void Main(string[] args)
        {
            DoWork(args);
        }

        private static void DoWork(string[] args)
        {
            var process = new Process() { EnableRaisingEvents = true };
            process.Exited += process_Exited;
            process.ErrorDataReceived += process_ErrorDataReceived;

            var processArguments = new ProcessStartInfo()
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + string.Join(" ", args)
            };

            process.StartInfo = processArguments;
            process.Start();
            _output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            var code = process.ExitCode;
            //TODO: write file output (EXIT /B %ERRORLEVEL%)
        }

        private static void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        private static void process_Exited(object sender, EventArgs e)
        {
            Process process = null;
            if (sender != null
                && (process = sender as Process) != null)
            {
                //TODO: Perform action if errorcode > 0, or look elsewhere?
            }
        }
    }
}
