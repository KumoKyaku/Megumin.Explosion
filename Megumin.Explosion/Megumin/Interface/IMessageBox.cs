using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 返回值约定[0：取消;1：确定；2：帮助；]
    /// </summary>
    public interface IMessageBox
    {
        /// <summary>
        /// 参考 https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.messageboxbuttons?view=net-5.0
        /// </summary>
        public enum MessageBoxButtons
        {
            OK = 0,
            OKCancel = 1,
            AbortRetryIgnore = 2,
            YesNoCancel = 3,
            YesNo = 4,
            RetryCancel = 5,
        }

        /// <summary>
        /// 对话框通用api
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="buttons"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        ValueTask<int> Show(object title, object content, MessageBoxButtons buttons = MessageBoxButtons.OKCancel, object options = null);
    }
}



