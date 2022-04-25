using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optika
{
    public class tovarcls
    {
        private string tovname;
        private string category;
        private int tovcount;

        public tovarcls()
        {
        }

        public tovarcls(string tovname, int tovcount)
        {
            this.tovname = tovname;
            this.tovcount = tovcount;
        }

        public string gettovname()
        {
            return this.tovname;
        }

        public int getcount()
        {
            return this.tovcount;
        }
    }
}
