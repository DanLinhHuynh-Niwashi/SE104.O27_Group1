using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_DuAn
    {
        string _mada;
        string _malsk;
        string _maowner;
        string _tenda;
        long _ngansach;
        DateTime? _tstart;
        DateTime? _tend;
        string _stat;
        long _dadung;
        int _tiendo;

        public DTO_DuAn(string mada = "", string malsk = "", string maowner = "", string tenda = "", long ngansach = -1, string tstart = "", string tend = "", string stat = "", long dadung = 0, int tiendo = 0)
        {
            _mada = mada;
            _malsk = malsk;
            _maowner = maowner;
            _tenda = tenda;
            _ngansach = ngansach;
            _tstart = null; 
            _tend = null;
            _stat = stat;
            _dadung = dadung;
            _tiendo = tiendo;
        }

        public string MADA
        { get { return _mada; } set { _mada = value; } }
        public string MALSK
        {
            get { return _malsk; }
            set { _malsk = value; }
        }
        public string MAOWNER
        { get { return _maowner; } set { _maowner = value; } }
        public string TENDA
        { get { return _tenda; } set { _tenda = value; } }

        public long NGANSACH
        { get { return _ngansach; } set { _ngansach = value; } }

        public DateTime? TSTART
        {
            get { return _tstart; }
            set { _tstart = value; }
        }
        public DateTime? TEND
        { get { return _tend; } set { _tend = value; } }

        public string STAT
        { get { return _stat; } set { _stat = value; } }

        public long DADUNG
        { get { return _dadung; } set { _dadung = value; } }

        public int TIENDO
        { get { return _tiendo; } set { _tiendo = value; } }
    }
}
