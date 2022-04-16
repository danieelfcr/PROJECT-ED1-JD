using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROJECT_ED1.Helpers
{
    public class Data
    {
        //Singleton
        private static Data _instance = null;

        public static Data Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Data();
                }
                return _instance;
            }
        }
    }
}
