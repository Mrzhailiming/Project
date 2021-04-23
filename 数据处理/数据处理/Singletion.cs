using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Singletion<T> where T :  class , new()
    {
        static private T _instance = default(T);
        static private object locker = new object();
        static public T Instance()
        {
            if (null == _instance)
            {
                lock(locker)
                {
                    if (null == _instance)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}
