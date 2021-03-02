using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using MacroLeague.Events;
using MacroLeague.Types;


namespace MacroLeague
{
    internal class Program
    {
        private SpeechSynthesizer _speechSynthesizer = new SpeechSynthesizer();
        
        public static double nextDragonKillTimer = 100000000000;
        public static double nextHeraldKillTimer = 100000000000;
        public static double nextBaronKillTimer = 100000000000;
        public static double nextSeigeSpawnTimer = 100000000000;
        
        public static allgamedata Allgamedata = null;

        public static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        private async Task Start()
        {
            // Select a voice from the SpeechSynthesizer#GetInstalledVoices list
            _speechSynthesizer.SelectVoice("Microsoft Zira Desktop");
            
            // Instantiate events 
            var onSiegeMinionSpawn = new OnSiegeMinionSpawn();
            var onGameStart = new OnGameStart();
            var onHeartbeat = new OnHeartbeat();
            var onDragonKill = new OnDragonKill();
            var onHeraldKill = new OnHeraldKill();
            var onBaronKill = new OnBaronKill();
            var nsth = new NSTH();
            var nstd = new NSTD();
            var nstb = new NSTB();
            
            // Register Events
            onSiegeMinionSpawn.Register += siegeMinionSpawn;
            onGameStart.Register += gameStart;
            onHeartbeat.Register += heartbeat;
            onBaronKill.Register += baronKill;
            onDragonKill.Register += dragonKill;
            onHeraldKill.Register += heraldKill;
            nstd.Register += nintySecondsTillDragon;
            nsth.Register += nintySecondsTillHerald;
            nstb.Register += nintySecondsTillBaron;
            
            // Delay the task forever
            await Task.Delay(-1);
        }
        
        /// <summary>
        /// Called from OnGameStart.cs
        /// Fires when a game starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void gameStart(object sender, OnGameStartArgs response)
        {
            Console.WriteLine("New game started.");
        }
        
        /// <summary>
        /// Called from OnHeartbeat.cs
        /// Fires when we update Program.allgamedata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void heartbeat(object sender, OnHeartbeatArgs response) {}
        
        /// <summary>
        /// Called from OnDragonKill.cs
        /// Fires when a dragon has been killed by either team
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void dragonKill(object sender, OnDragonKillArgs response)
        {
            Console.WriteLine("Dragon killed");
            var chaosDragonKills = 0;
            var orderDragonKills = 0;
            foreach (var dkevent in Allgamedata.events.Events)       // For each even that exists  
            {                                                              
                if (dkevent.EventName == "DragonKill")                   // If the event type is of "DragonKill"
                {                                                        
                    foreach (var player in Allgamedata.allPlayers)       // 
                    {                                                    // Check who killed it
                        if (player.summonerName == dkevent.KillerName)   // 
                        {
                            if (player.team == "ORDER")                  // If they are on team "ORDER"
                            {
                                orderDragonKills += 1;                   // Grant team order a point
                            }
                            else                                         // If they are on team "CHAOS"
                            {
                                chaosDragonKills += 1;                   // Grant team chaos a point
                            }
                        }
                    }
                }
            }

            if (chaosDragonKills >= 4 || orderDragonKills >= 4)      // If either order or chaos has killed 4 dragons, the next will be elder
            {
                nextDragonKillTimer = Math.Floor(Allgamedata.gameData.gameTime) + 270; // if elder, set timer accordingly
            }
            else
            {
                nextDragonKillTimer = Math.Floor(Allgamedata.gameData.gameTime) + 210; // if normal, set timer accordingly 
            }
        }
        
        /// <summary>
        /// Called from OnHeraldKill.cs
        /// Fires when herald is killed by either team
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void heraldKill(object sender, OnHeraldKillArgs response)
        {
            Console.WriteLine("Herald Killed");
            if (Math.Truncate(Allgamedata.gameData.gameTime) < 825)
            {
                nextHeraldKillTimer = Math.Truncate(Allgamedata.gameData.gameTime) + 270;
            }
            else
            {
                nextHeraldKillTimer = 999999999999999;
            }
        }
        
        /// <summary>
        /// Called from OnBaronKill.cs
        /// Fires when baron is killed by either team
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void baronKill(object sender, OnBaronKillArgs response)
        {
            Console.WriteLine("Baron Killed");
            nextBaronKillTimer = Math.Truncate(Allgamedata.gameData.gameTime) + 270;
        }
        
        /// <summary>
        /// Called from NSTD.cs
        /// Fires when there is exactly 90s till dragon spawns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void nintySecondsTillDragon(object sender, NSTDArgs response)
        {
            Console.WriteLine("90s till dragon spawns.");
            _speechSynthesizer.SpeakAsync("Dragon 90");
        }
        
        /// <summary>
        /// Called from NSTH.cs
        /// Fires when there is exactly 90s till herald spawns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void nintySecondsTillHerald(object sender, NSTHArgs response)
        {
            Console.WriteLine("90s till herald spawns.");
            _speechSynthesizer.SpeakAsync("Herald 90");
        }
        
        /// <summary>
        /// Called from NSTB.cs
        /// Fires when there is exactly 90s till baron spawns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void nintySecondsTillBaron(object sender, NSTBArgs response)
        {
            Console.WriteLine("90s till baron spawns.");
            _speechSynthesizer.SpeakAsync("Baron 90");
        }
        
        private void siegeMinionSpawn(object sender, OnSiegeMinionSpawnArgs response)
        {
            Console.WriteLine("Siege 15");
            _speechSynthesizer.SpeakAsync("Siege 15");
        }
    }
} 