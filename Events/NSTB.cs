using System;
using System.Net;
using System.Threading;
using MacroLeague.Types;

namespace MacroLeague.Events
{
    /// <summary>
    /// NintySecondsTillBaron Event
    /// Fires when there is exactly 90s before baron spawns
    /// </summary>
    public class NSTB
    {
        public event EventHandler<NSTBArgs> Register;

        public NSTB()
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
                        if (Math.Floor(Program.Allgamedata.gameData.gameTime) == 1110)
                        {
                            Register?.Invoke(this, new NSTBArgs() {NSTB = true});
                        }
                        if (Math.Floor(Program.Allgamedata.gameData.gameTime) == Program.nextBaronKillTimer)
                        {
                            Register?.Invoke(this, new NSTBArgs() {NSTB = true});
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
 
    public class NSTBArgs : EventArgs
    {
        public bool NSTB;
    }
}