using DataAccess;
using IntensityServer.Models;

namespace IntensityServer.Data
{
    public class EventData
    {
        private Event _event;
        private IDataAccess _data;
        private IConfiguration _config;

        public EventData(Event eventy, IDataAccess data, IConfiguration configuration)
        {
            _event = eventy;
            _data = data;
            _config = configuration;
        }
        public EventData(IDataAccess data, IConfiguration configuration)
        {
            _event = new Event();
            _data = data;
            _config = configuration;
        }

        public async Task UploadEvent(string userid, string seconds, string eventid)
        {
            string sql = "insert into eventstable (EventId, UserId, Seconds) values (@EventId, @UserId, @Seconds);";
            await _data.SaveData(sql, new { EventId = eventid, UserId = userid, Seconds = seconds }, _config.GetConnectionString("default"));

        }

        public async Task<List<Event>> DownLoadEvents()
        {
            string sql = "select * from eventstable";
            List<Event> events = await _data.LoadData<Event, dynamic>(sql, new { }, _config.GetConnectionString("default"));
            return events;
        }
        public async Task UpdateData(string userid, string eventid, string updatedsecs)
        {
            string sql = "update eventstable set Seconds = @Seconds where EventId = @EventId and UserId = @UserId";

            await _data.SaveData(sql, new { Seconds = updatedsecs, EventId = eventid, UserId = userid }, _config.GetConnectionString("default"));


        }
    }
}
