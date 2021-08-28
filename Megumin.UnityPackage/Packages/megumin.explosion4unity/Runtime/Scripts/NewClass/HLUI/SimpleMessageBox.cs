
#if UNITY_2021_2_OR_NEWER

using System.Threading.Tasks;

namespace Megumin
{
    /// <summary>
    /// 简易对话框组件
    /// </summary>
    public class SimpleMessageBox : MessageBox
    {

#if MEGUMIN_TMPROEX
        public TMPro.TextMeshProUGUI Title;
        public TMPro.TextMeshProUGUI Content;

        public override ValueTask<int> Show(object title, object content, IMessageBox.MessageBoxButtons buttons = IMessageBox.MessageBoxButtons.OKCancel, object options = null)
        {
            SetTitle(title);
            SetContent(content);
            return base.Show(title, content, buttons, options);
        }

        private void SetContent(object content)
        {
            if (Content)
            {
                Title.text = content.ToString();
            }
        }

        private void SetTitle(object title)
        {
            if (Title)
            {
                Title.text = title.ToString();
            }
        }

#endif
    }
}



#endif

