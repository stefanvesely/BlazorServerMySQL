using IntensityServer.Models;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using DataAccess;

namespace IntensityServer.Data
{
    public class PlayerData
    {
        private Player _player;
        private IDataAccess _data;
        private IConfiguration _config;

        public PlayerData (Player playerc, IDataAccess ida, IConfiguration cfg)
        {
            _player = playerc;
            _data = ida;
            _config = cfg;
        }
        public PlayerData(IDataAccess ida, IConfiguration cfg)
        {            
            _data = ida;
            _config = cfg;
            _player = new Player();
        }

        public async Task UploadPlayer ()
        {
            if (_player.FirstName != null && _player.Surname != null)
            {
                string sql = "insert into playertable (FirstName, Surname) values (@FirstName, @Surname);";
                await _data.SaveData(sql, new { FirstName = _player.FirstName, Surname = _player.Surname }, _config.GetConnectionString("default"));
                
            }
        }
        public async Task<List<Player>> DowloadPlayers()
        {
            string sql = "select * from playertable";
            List<Player> players = await _data.LoadData<Player, dynamic>(sql, new { }, _config.GetConnectionString("default"));
            return players;
        }
    }
}
