using DataAccess;
using IntensityServer.Data;
using System.Linq;

namespace IntensityServer.Models
{
    public class FullEventTable
    {
        private List<FullTableRowClass> fullTableRowClasses = new List<FullTableRowClass>();

        public List<FullTableRowClass> rows 
        { get { return fullTableRowClasses; } }


        public FullEventTable (List<Player> _playersU, List<Event> _eventsU, IDataAccess _dta, IConfiguration _cfg)
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
                        EventId = i+1,
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
