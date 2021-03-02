
using System;
using System.Net;
using System.Threading;
using MacroLeague.Types;

namespace MacroLeague.Events
{
    /// <summary>
    /// NintySecondsTillDragon Event
    /// Fires when there is exactly 90s before dragon spawns
    /// </summary>
    public class NSTD
    {
        public event EventHandler<NSTDArgs> Register;

        public NSTD()
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
                        if (Math.Floor(Program.Allgamedata.gameData.gameTime) == 210)
                        {
                            Register?.Invoke(this, new NSTDArgs() {nstd = true});
                        }
                        if (Math.Floor(Program.Allgamedata.gameData.gameTime) == Program.nextDragonKillTimer)
                        {
                            Register?.Invoke(this, new NSTDArgs() {nstd = true});
                        }
                    }
                }
            }
        }
    }

    public class NSTDArgs : EventArgs
    {
        public bool nstd;
    }
}