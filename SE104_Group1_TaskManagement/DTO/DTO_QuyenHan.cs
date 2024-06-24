using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_QuyenHan
    {
        private string _maqh;
        private string _tenqh;



        public DTO_QuyenHan(string maqh = "", string tenqh = "")
        {
            _maqh = maqh;
            _tenqh = tenqh;
        }

        public string MAQH { get { return _maqh; } set { _maqh = value; } }
        public string TENQH { get { return _tenqh; } set { _tenqh = value; } }

    }
}