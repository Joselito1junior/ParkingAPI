using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Helpers
{
    public class PaginationList<T> : List<T>
    {
        public Pagination Pagination { get; set; }
    }
}
