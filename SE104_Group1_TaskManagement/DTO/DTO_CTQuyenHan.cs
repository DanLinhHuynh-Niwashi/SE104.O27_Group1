using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_CTQuyenHan
    {
        private string _mact;
        private string _maqh;
        private string _action;

        public DTO_CTQuyenHan(string mact = "", string maqh = "", string action = "")
        {
            _mact = mact;
            _maqh = maqh;
            _action = action;
        }

        public string MACT { get { return _mact; } set { _mact = value; } }
        public string MAQH { get { return _maqh; } set { _maqh = value; } }
        public string ACTION { get { return _action; } set { _action = value; } }

    }
}