using System;
using System.Collections.Generic;
using System.Linq;

namespace Cryptocop.Software.API.Models
{
    public class Envelope<T> where T : class
    {
        public Envelope(int pageNumber, IEnumerable<T> items)
        {
            Items = items;
            PageNumber = pageNumber;
        }

        public int PageNumber { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}