using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics2
{
    //Структура с данными которые необходимо сохранить в файл
    [Serializable]
    public struct Point
    {
       public double[] masX;
       public double[] masY;
        public int size;
        public double xleft;
        public double xright;
        public double ydown;
        public double yup;
    }
    
}
