using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHangBanhNgot_BanhKem.Domain
{
    public class Event : EventArgs
    {
        class InventoryLowHandler
        {
            public string Mess;
            public InventoryLowHandler(string mess)
            {
                this.Mess = mess;
            } 

        }
    }
}
