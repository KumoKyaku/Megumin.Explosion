using Megumin;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Megumin
{
    /// <summary>
    /// ���׶Ի������
    /// </summary>
    public class MessageBox : MonoBehaviour, IMessageBox
    {
        protected TaskCompletionSource<int> source;

        public virtual ValueTask<int> Show(object title,
                                   object content,
                                   IMessageBox.MessageBoxButtons buttons = IMessageBox.MessageBoxButtons.OKCancel,
                                   object options = null)
        {
            source = new TaskCompletionSource<int>();
            return new ValueTask<int>(source.Task);
        }

        [EditorButton]
        public virtual void OK()
        {
            source?.TrySetResult(1);
        }

        [EditorButton]
        public virtual void Cancle()
        {
            source?.TrySetResult(0);
        }

        [EditorButton]
        public virtual void Help()
        {
            source?.TrySetResult(2);
        }
    }
}


