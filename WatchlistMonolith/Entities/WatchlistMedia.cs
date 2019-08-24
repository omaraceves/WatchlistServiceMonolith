using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchlistMonolith.Entities
{
    public class WatchlistMedia
    {
        public Guid Id { get; set; }
        public Guid WatchlistId { get; set; }
        public Watchlist WatchList { get; set; }
        public Guid MediaId { get; set; }
        public Media Media { get; set; }
    }
}
