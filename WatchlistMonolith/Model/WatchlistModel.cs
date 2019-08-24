using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchlistMonolith.Model
{
    public class WatchlistModel
    {
        public Guid WatchlistId { get; set; }
        public string WatchlistName { get; set; }
        public List<MediaModel> MediaModel { get; set; } = new List<MediaModel>();
    }
}
