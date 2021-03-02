using System;
using System.Threading;
using MacroLeague.Types;

namespace MacroLeague.Events
{
    /// <summary>
    /// OnGameStart Event
    /// Fires when a new game starts, restarting a game in practice tool does not fire this event
    /// </summary>
    public class OnGameStart
    {
        public event EventHandler<OnGameStartArgs> Register;

        public OnGameStart()
        {
            var task = new Thread(Update);
            task.Start();
        }

        public void Update()
        {
            var starttime = 0d;
            var time = 0d;
            while (true)
            {
                if (Program.Allgamedata != null)
                {
                    if (Math.Truncate(Program.Allgamedata.gameData.gameTime) != time)
                    {
                        time = Math.Truncate(Program.Allgamedata.gameData.gameTime);
                        if (Program.Allgamedata.events.Events.Count == 1)
                        {
                            if (starttime != Program.Allgamedata.events.Events[0].EventTime)
                            {
                                starttime = Program.Allgamedata.events.Events[0].EventTime;
                                Register?.Invoke(this, new OnGameStartArgs {Allgamedata = Program.Allgamedata});
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
    }

    public class OnGameStartArgs : EventArgs
    {
        public allgamedata Allgamedata;
    }
}