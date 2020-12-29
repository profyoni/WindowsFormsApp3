using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // avoid tight loops that use CPU...
        // for example when polling a service, sleep between repeated polls
        private void LongTaskLikeGettingWebPageOrWebService(string s)
        {
            toolStripStatusLabel1.Text = @"Starting Processing";
            Thread.Sleep(4000); // 100ms
            toolStripStatusLabel1.Text = @"Finished Processing";
        }

        
        private void button1_Clickv1(object sender, EventArgs e)
        {
           Task.Run( () => LongTaskLikeGettingWebPageOrWebService(e.ToString()));
        }

        // Downsides of new Threads
        // 1. resource intensive for OS
        // 2. may introduce thread contention issues (synchronization) = race conditions


        // async and await
        // uses same thread as invoked
        // hides the complexity of multi threading

        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/task-asynchronous-programming-model





        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "" + GetUrlContentLengthAsync();
        }

        public async Task<int> GetUrlContentLengthAsync()
        {
            toolStripStatusLabel1.Text = @"Starting Processing";
            var client = new HttpClient();

            Task<string> getStringTask =
                client.GetStringAsync("https://docs.microsoft.com/dotnet");

            DoIndependentWork();

            string contents = await getStringTask;
            Trace.WriteLine(contents);
            toolStripStatusLabel1.Text = @"Ending Processing";
            return contents.Length;
        }

        void DoIndependentWork()
        {
            Console.WriteLine("Working...");
        }
    }
}
