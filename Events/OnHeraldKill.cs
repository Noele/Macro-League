using System;
using System.Threading;
using MacroLeague.Types;

namespace MacroLeague.Events
{

    public class OnHeraldKill
    {
        public event EventHandler<OnHeraldKillArgs> Register;

        public OnHeraldKill()
        {
            var task = new Thread(Update);
            task.Start();
        }

        private void Update()
        {
            var time = 0d;
            while (true)
            {
                if (Program.Allgamedata != null && Program.Allgamedata.events.Events.Count > 0 && Math.Truncate(Program.Allgamedata.gameData.gameTime) != time)
                {
                    time = Math.Truncate(Program.Allgamedata.gameData.gameTime);
                    var aEvent = Program.Allgamedata.events.Events[Program.Allgamedata.events.Events.Count - 1];
                    
                    if (Program.Allgamedata.gameData.gameTime - aEvent.EventTime < 1 && aEvent.EventName == "HeraldKill")
                    {
                        Register?.Invoke(this, new OnHeraldKillArgs {Allgamedata = Program.Allgamedata});
                    }
                }
            }
        }
    }

    public class OnHeraldKillArgs : EventArgs
    {
        public allgamedata Allgamedata;
    }
}