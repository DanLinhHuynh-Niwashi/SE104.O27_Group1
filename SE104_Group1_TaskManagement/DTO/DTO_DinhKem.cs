using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_DinhKem
    {

        string _manv;
        string _macv;
        string _madk;
        string _tep;

        public DTO_DinhKem(string manv = "", string macv = "", string madk = "", string tep = "")
        {
            _macv = macv;
            _manv = manv;
            _madk = madk;
            _tep = tep;
        }

        public string MANV { get { return _manv; } set { _manv = value; } }
        public string MACV { get { return _macv; } set { _macv = value; } }
        public string TEP { get { return _tep; } set { _tep = value; } }
        public string MADK { get { return _madk; } set { _madk = value; } }

    }
}
