using System;
using System.Net;
using System.Threading;
using MacroLeague.Types;

namespace MacroLeague.Events
{
    /// <summary>
    /// OnOnSiegeMinionSpawn Event
    /// Fires 15s before Siege Minion Spawns
    /// </summary>
    public class OnSiegeMinionSpawn
    {
        public event EventHandler<OnSiegeMinionSpawnArgs> Register;

        public OnSiegeMinionSpawn()
        {
            var task = new Thread(Update);
            task.Start();
        }

        public void Update()
        {
            var time = 0d;
            while (true)
            {
                if (Program.Allgamedata != null)
                {
                    if (Math.Truncate(Program.Allgamedata.gameData.gameTime) != time)  //
                    {                                                                  // If this event has fired withing 1 second of the last, dont fire
                        time = Math.Truncate(Program.Allgamedata.gameData.gameTime);   //
                        if (time == 110)
                        {
                            Register?.Invoke(this, new OnSiegeMinionSpawnArgs {SMS = true});
                            Program.nextSeigeSpawnTimer = time + 90;
                        }
                        else
                        {
                            if (time == Program.nextSeigeSpawnTimer)
                            {
                                Register?.Invoke(this, new OnSiegeMinionSpawnArgs {SMS = true});
                                if (time < 1200)
                                {
                                    Program.nextSeigeSpawnTimer = time + 90;
                                } else if (time > 1200 && time < 2100)
                                {
                                    Program.nextSeigeSpawnTimer = time + 60;
                                } else if (time > 2100)
                                {
                                    Program.nextSeigeSpawnTimer = time + 30;
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
    }

    public class OnSiegeMinionSpawnArgs : EventArgs
    {
        public bool SMS;
    }
}