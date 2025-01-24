using RepositoryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryAPI.Models.OrdersDTO
{
    public class OrderDTO
    {
        public int UserId { get; set; }

        public int SizeId { get; set; }

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

    }
}
