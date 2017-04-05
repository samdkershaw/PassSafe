using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Data;

namespace PassSafe.Models
{
    class Services
    {
        List<Service> services = new List<Service>();

        public Services()
        {

        }

        public void Update()
        {
            Database db = new Database();
            this.services = db.GetServices();
        }
    }
}
