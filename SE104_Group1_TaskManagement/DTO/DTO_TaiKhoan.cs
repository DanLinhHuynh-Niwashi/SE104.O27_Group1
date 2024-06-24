using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_TaiKhoan
    {
        private string _maqh;
        private string _email;
        private string _pass;
        private string _manv;


        public DTO_TaiKhoan(string maqh = "", string email = "", string pass = "", string manv = "")
        {
            _maqh = maqh;
            _email = email;
            _pass = pass;
            _manv = manv;
        }

        public string MAQH { get { return _maqh; } set { _maqh = value; } }
        public string EMAIL { get { return _email; } set { _email = value; } }
        public string PASS { get { return _pass; } set { _pass = value; } }
        public string MANV { get { return _manv; } set { _manv = value; } }

    }
}
