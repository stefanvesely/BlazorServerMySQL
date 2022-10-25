using DataAccess;
using IntensityServer.Data;

namespace IntensityServer.Models
{
    public class FullEventTable
    {
        private List<FullTableRowClass> fullTableRowClasses = new List<FullTableRowClass>();

        public List<FullTableRowClass> rows
        { get { return fullTableRowClasses; } }

        private FullEventTable (List<FullTableRowClass> fullTableRowClasses)
        {
            this.fullTableRowClasses = fullTableRowClasses;
        }

        public FullEventTable(List<Player> _playersU, List<Event> _eventsU, IDataAccess _dta, IConfiguration _cfg)
        {
            foreach (Player _pl in _playersU)
            {
                if (_eventsU.Where(c => c.UserId == _pl.PlayerId).Count() > 0)
                {
                    foreach (Event _eventy in _eventsU.Where(c => c.UserId == _pl.PlayerId))
                    {
                        UpdateTable(_pl, _eventy);
                    }
                    continue;
                }
                List<Event> blanks = new List<Event>();
                for (int i = 0; i < 5; i++)
                {
                    blanks.Add(new Event()
                    {
                        UserId = _pl.PlayerId,
                        EventId = i + 1,
                        Seconds = 0,
                    });
                    EventData _evie = new EventData(_dta, _cfg);
                    int eid = i + 1;
                    _evie.UploadEvent(_pl.PlayerId.ToString(), "0", eid.ToString());
                }
                foreach (Event b in blanks)
                {
                    UpdateTable(_pl, b);
                }
            }
        }

        public void UpdateTable(Player _playerU, Models.Event _event)
        {
            int one = 0;
            int two = 0;
            int three = 0;
            int four = 0;
            switch (_event.EventId)
            {
                case 1:
                    one = _event.Seconds;
                    break;
                case 2:
                    two = _event.Seconds;
                    break;
                case 3:
                    three = _event.Seconds;
                    break;
                case 4:
                    four = _event.Seconds;
                    break;
            }
            if (fullTableRowClasses.Where(c => c.player.PlayerId == _playerU.PlayerId).Count() == 0)
            {
                fullTableRowClasses.Add(new FullTableRowClass()
                {
                    player = _playerU,
                    eSeconds1 = one,
                    eSeconds2 = two,
                    eSeconds3 = three,
                    eSeconds4 = four
                });
                return;
            }
            switch (_event.EventId)
            {
                case 1:
                    fullTableRowClasses.First(c => c.player.PlayerId == _playerU.PlayerId).eSeconds1 = one;
                    break;
                case 2:
                    fullTableRowClasses.First(c => c.player.PlayerId == _playerU.PlayerId).eSeconds2 = two;
                    break;
                case 3:
                    fullTableRowClasses.First(c => c.player.PlayerId == _playerU.PlayerId).eSeconds3 = three;
                    break;
                case 4:
                    fullTableRowClasses.First(c => c.player.PlayerId == _playerU.PlayerId).eSeconds4 = four;
                    break;
            }
        }
        public void RemovePlayer(Player _playDead)
        {

            fullTableRowClasses.Remove(fullTableRowClasses.First(c => c.player == _playDead));

        }

        public async Task<FullEventTable> UpdateScores()
        {
            List<EventPlayerSecondsClass> event1 = new List<EventPlayerSecondsClass>();
            List<EventPlayerSecondsClass> event2 = new List<EventPlayerSecondsClass>();
            List<EventPlayerSecondsClass> event3 = new List<EventPlayerSecondsClass>();
            List<EventPlayerSecondsClass> event4 = new List<EventPlayerSecondsClass>();
            foreach(FullTableRowClass fullTableRowClass in fullTableRowClasses)
            {
                if (fullTableRowClass.eSeconds1 != 0)
                {
                    event1.Add(new EventPlayerSecondsClass()
                    {
                        playerId = fullTableRowClass.player.PlayerId,
                        eventId = 1,
                        seconds = fullTableRowClass.eSeconds1
                    });
                }
                if (fullTableRowClass.eSeconds2 != 0)
                {
                    event2.Add(new EventPlayerSecondsClass()
                    {
                        playerId = fullTableRowClass.player.PlayerId,
                        eventId = 2,
                        seconds = fullTableRowClass.eSeconds2
                    });
                }
                if (fullTableRowClass.eSeconds3 != 0)
                {
                    event3.Add(new EventPlayerSecondsClass()
                    {
                        playerId = fullTableRowClass.player.PlayerId,
                        eventId = 3,
                        seconds = fullTableRowClass.eSeconds3
                    });
                }
                if (fullTableRowClass.eSeconds4 != 0)
                {
                    event4.Add(new EventPlayerSecondsClass()
                    {
                        playerId = fullTableRowClass.player.PlayerId,
                        eventId = 4,
                        seconds = fullTableRowClass.eSeconds4
                    });
                }
            }
            event1.OrderBy(c => c.seconds);
            event2.OrderBy(c => c.seconds);
            event3.OrderBy(c => c.seconds);
            event4.OrderBy(c => c.seconds);
            int icount = 1;
            foreach (EventPlayerSecondsClass epv in event1)
            {
                epv.score = icount;
                icount++;
            }
            icount = 1;
            foreach (EventPlayerSecondsClass epv in event2)
            {
                epv.score = icount;
                icount++;
            }
            icount = 1;

            foreach (EventPlayerSecondsClass epv in event3)
            {
                epv.score = icount;
                icount++;
            }
            icount = 1;
            foreach (EventPlayerSecondsClass epv in event4)
            {
                epv.score = icount;
                icount++;
            }
            foreach (FullTableRowClass ftrc in fullTableRowClasses)
            {
                int iplayertotal = 0;
                foreach (EventPlayerSecondsClass epsc in event1.Where(x =>x.playerId == ftrc.player.PlayerId))
                {
                    iplayertotal += epsc.score;
                }
                foreach (EventPlayerSecondsClass epsc in event2.Where(x => x.playerId == ftrc.player.PlayerId))
                {
                    iplayertotal += epsc.score;
                }
                foreach (EventPlayerSecondsClass epsc in event3.Where(x => x.playerId == ftrc.player.PlayerId))
                {
                    iplayertotal += epsc.score;
                }
                foreach (EventPlayerSecondsClass epsc in event4.Where(x => x.playerId == ftrc.player.PlayerId))
                {
                    iplayertotal += epsc.score;
                }
                ftrc.eScore = iplayertotal;
            }
            List<FullTableRowClass> ftrcret = new List<FullTableRowClass>();
            foreach (FullTableRowClass frit in fullTableRowClasses)
            {
                if (frit.eScore != 0)
                {
                    ftrcret.Add(frit);
                }
            }
            FullEventTable fet = new FullEventTable(ftrcret);
            return fet; 
        }
    }
    
    public class EventPlayerSecondsClass
    {
        public int playerId;
        public int eventId;
        public int seconds;
        public int score;
    }

    public class FullTableRowClass
    {
        public Player player;
        public int eSeconds1;
        public int eSeconds2;
        public int eSeconds3;
        public int eSeconds4;
        public int eScore;

    }
}
