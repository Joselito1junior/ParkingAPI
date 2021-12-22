using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Helpers
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int RegisterByPage { get; set; }
        public int TotalRegister { get; set; }
        public int TotalPages { get; set; }

        public Pagination(int pageNumber, int registerByPage, int totalRegister)
        {
            PageNumber = pageNumber;
            RegisterByPage = registerByPage;
            TotalRegister = totalRegister;
            TotalPages = (int)Math.Ceiling((double)TotalRegister / RegisterByPage);
        }

        public bool IsValidPage(int page)
        {
            return page <= TotalPages;
        }
    }
}
