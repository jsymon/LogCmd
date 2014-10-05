using System;
using System.Configuration;
using System.Diagnostics;
namespace LogCmd
{
    class Program
    {
        /// <summary>
        /// Console standard output
        /// </summary>
        private static string _output = null;

        /// <summary>
        /// Console error output
        /// </summary>
        private static string _error = null;

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
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + string.Join(" ", args)
            };

            //for testing
            //processArguments.Arguments = "/C test.bat";

            process.StartInfo = processArguments;
            process.Start();
            _output = process.StandardOutput.ReadToEnd();
            _error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            //TODO: write file output (EXIT /B %ERRORLEVEL%)
        }

        private static void process_ErrorDataReceived(object sender, DataReceivedEventArgs e) { }

        private static void process_Exited(object sender, EventArgs e)
        {
            Process process = null;
            if (sender != null
                && (process = sender as Process) != null)
            {
                OnExit(process);
            }
        }

        private static void OnExit(Process process)
        {
            //JS:If exitcode is equal to 0 then assume everything completed OK
            if (process.ExitCode == 0)
                return;
            //JS:Else log error (usually exitCode = 0)
            EmailError(process);
        }

        private static void EmailError(Process process)
        {
            string subject = string.Format("An error occurred whilst running {0} via {1}.", process.StartInfo.Arguments, process.StartInfo.FileName);
            string body =
                "Standard output:"
                + Environment.NewLine
                + "{0}"
                + Environment.NewLine + Environment.NewLine
                + "Error output:"
                + Environment.NewLine
                + "{1}"
                + _error;

            var sm = new SendMail(ConfigurationManager.AppSettings["SendMail.From"], ConfigurationManager.AppSettings["SendMail.To"]);
            sm.Subject = subject;
            sm.Body = body;
            sm.Send();
        }
    }
}