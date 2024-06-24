using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_ThamSo
    {
        float _percentUp;

        public DTO_ThamSo(float percentUp = 0f)
        {
            _percentUp = percentUp;
        }

        public float PERCENTUP
        {
            get { return _percentUp; }
            set { _percentUp = value; }
        }
    }
}
