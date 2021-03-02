using System;
using System.Net;
using System.Threading;
using MacroLeague.Types;

namespace MacroLeague.Events
{
    /// <summary>
    /// NintySecondsTillHerald Event
    /// Fires when there is exactly 90s before herald spawns
    /// </summary>
    public class NSTH
    {
        public event EventHandler<NSTHArgs> Register;

        public NSTH()
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
                        if (Math.Floor(Program.Allgamedata.gameData.gameTime) == 390)
                        {
                            Register?.Invoke(this, new NSTHArgs() {NSTH = true});
                        }
                        if (Math.Floor(Program.Allgamedata.gameData.gameTime) == Program.nextHeraldKillTimer)
                        {
                            Register?.Invoke(this, new NSTHArgs() {NSTH = true});
                        }
                    }
                }
            }
        }
    }

    public class NSTHArgs : EventArgs
    {
        public bool NSTH;
    }
}