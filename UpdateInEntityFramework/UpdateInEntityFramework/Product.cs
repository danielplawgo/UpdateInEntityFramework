using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateInEntityFramework
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? LockTime { get; set; }

        public int Count { get; set; }
    }
}
