using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 简单的匹配器
    /// </summary>
    public partial class SimpleMatcher:IMatcher<string,string>
    {
        public bool Match(string input, string target)
        {
            if (target.Contains(input))
            {
                return true;
            }
            return false;
        }
    }
}
