using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WatchlistMonolith.Entities
{
    [Table("WatchLists")]
    public class Watchlist
    {
        public Watchlist() { }

        public Watchlist(string userName)
        {
          
            Name = $"WatchlaterFor{userName}";
        }

        [Key]
        public Guid Id { get; set; }


        public string Name { get; set; }


        public List<WatchlistMedia> Medias { get; set; }
    }
}
