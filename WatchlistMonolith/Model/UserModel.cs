using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchlistMonolith.Model
{
    public class UserModel
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; }

        public Guid WatchLaterId { get; set; }

        public List<MediaModel> WatchLaterMedias { get; set; }

        public string AuthorizationToken { get; set; }
    }
}
