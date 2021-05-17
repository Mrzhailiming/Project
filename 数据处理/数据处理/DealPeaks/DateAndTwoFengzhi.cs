using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;


namespace Data
{
    /// <summary>
    /// 求txt文件的两个峰值
    /// </summary>
    public class DateAndTwoFengzhi : PeaksModoule
    {
        //功能
        Dictionary<UInt32, string> _func = new Dictionary<uint, string>();
        public DateAndTwoFengzhi(string path)
            :base(path)
        {

        }
        /// <summary>
        /// 注册功能
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func"></param>
        public void RegisterFunc(uint key, string func)
        {
            string outFunc;
            if (!_func.TryGetValue(key, out outFunc))
            {
                _func.Add(key, func);
            }
        }
        override public void Reset()
        {
            base.Reset();

        }
    }

    
}
