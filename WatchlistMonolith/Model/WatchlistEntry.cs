using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WatchlistMonolith.Model
{
    public class WatchlistEntry
    {
        [Required]
        public string WatchlistId { get; set; }

        [Required]
        public string MediaId { get; set; }
    }
}
