using System;
using System.Net;
using System.Threading;
using MacroLeague.Types;
using Newtonsoft.Json;

namespace MacroLeague.Events
{
     /// <summary>
     /// Heartbeat event
     /// Connects and retives information from the game client
     /// Resets data when connection is lost
     /// </summary>
    public class OnHeartbeat
    {
        public event EventHandler<OnHeartbeatArgs> Register;

        public OnHeartbeat()
        {
            var task = new Thread(Update);
            task.Start();
        }

        public void Update()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    var uri = "https://127.0.0.1:2999/liveclientdata/allgamedata";

                    var web = new WebClient();
                    var responseString = web.DownloadString(uri);
                    var dataObject = JsonConvert.DeserializeObject<allgamedata>(responseString);

                    if (dataObject != null)
                    {
                        Program.Allgamedata = dataObject;
                        Register?.Invoke(this, new OnHeartbeatArgs() {Heartbeat = true});
                    }
                }
                catch (WebException)
                {
                }
            }
        }
    }

    public class OnHeartbeatArgs : EventArgs
    {
        public bool Heartbeat;
    }
}