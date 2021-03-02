using System;
using System.Threading;
using MacroLeague.Types;

namespace MacroLeague.Events
{

    public class OnBaronKill
    {
        public event EventHandler<OnBaronKillArgs> Register;

        public OnBaronKill()
        {
            var task = new Thread(Update);
            task.Start();
        }

        public void Update()
        {
            var time = 0d;
            while (true)
            {
                if (Program.Allgamedata != null && Program.Allgamedata.events.Events.Count > 0 && Math.Truncate(Program.Allgamedata.gameData.gameTime) != time)
                {
                    time = Math.Truncate(Program.Allgamedata.gameData.gameTime);

                    var aEvent = Program.Allgamedata.events.Events[Program.Allgamedata.events.Events.Count - 1];
                    if (Program.Allgamedata.gameData.gameTime - aEvent.EventTime < 1 &&
                        aEvent.EventName == "BaronKill")
                    {
                        Register?.Invoke(this, new OnBaronKillArgs {Allgamedata = Program.Allgamedata});
                    }
                }
                Thread.Sleep(100);
            }
        }
    }

    public class OnBaronKillArgs : EventArgs
    {
        public allgamedata Allgamedata;
    }
}