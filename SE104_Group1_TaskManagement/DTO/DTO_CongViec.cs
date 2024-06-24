using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_CongViec
    {
        string _macv;
        string _mada;
        string _macm;
        string _tencv;
        DateTime? _tstart;
        DateTime? _tend;
        long _ngansach;
        long _dadung;
        int _tiendo;
        string _ycdinhkem;
        string _tepdinhkem;
        int _isdeleted;

        public DTO_CongViec(string macv = "", string mada = "", string macm = "", string tencv = "", string tstart = "", string tend = "", long ngansach = 0, long dadung = 0, int tiendo = 0, string ycdk = "", string dk = "", int isdel = 0)
        {
            _macv = macv;
            _mada = mada;
            _macm = macm;
            _tencv = tencv;
            _tstart = null;
            _tend = null; ;
            _ngansach = ngansach;
            _dadung = dadung;
            _tiendo = tiendo;
            _ycdinhkem = ycdk;
            _tepdinhkem = dk;
            _isdeleted = isdel;
        }

        public string MACV
        {
            get { return _macv; }
            set { _macv = value; }
        }
        public string MADA
        {
            get { return _mada; }
            set { _mada = value; }
        }
        public string MACM
        {
            get { return _macm; }
            set { _macm = value; }
        }
        public string TENCV
        {
            get { return _tencv; }
            set { _tencv = value; }
        }
        public DateTime? TSTART
        {
            get { return _tstart; }
            set { _tstart = value; }
        }
        public DateTime? TEND
        {
            get { return _tend; }
            set { _tend = value; }
        }
        public long NGANSACH
        {
            get { return _ngansach; }
            set { _ngansach = value; }
        }
        public long DADUNG
        {
            get { return _dadung; }
            set { _dadung = value; }
        }
        public int TIENDO
        {
            get { return _tiendo; }
            set { _tiendo = value; }
        }
        public string YCDK
        {
            get { return _ycdinhkem; }
            set { _ycdinhkem = value; }
        }
        public string TEPDK
        {
            get { return _tepdinhkem; }
            set { _tepdinhkem = value; }
        }
    }
}
