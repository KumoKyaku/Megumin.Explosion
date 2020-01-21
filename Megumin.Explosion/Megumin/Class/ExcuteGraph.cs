using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Megumin.Class
{
    /// <summary>
    /// 设计原则,每个节点对应一个函数对应一个执行步骤
    /// 每个节点函数必须支持取消令牌，不然无法实现中断
    /// </summary>
    /// 编辑器反射所有运行时接口，取得所有符合规则的函数，
    /// Task<int> Play(CancellationToken cancellationToken)
    /// 可等待必须可取消。 int Play(); 
    /// 特殊分支节点支持返回枚举 Enum Branch
    /// 每个节点函数必须返回成功失败，或者分支编号。
    /// 函数名字就是节点名字。
    /// 外部只能调用起点函数。可以设置多个起点函数。
    /// 
    ///
    /// 想用节点图来处理，生成代码也不行……，仔细考虑，节点图的表达能力不够，处理不了中断的情况
    /// 最终结论还是需要硬编码，不过可以有个固定模式模板套路.
    /// 
    /// 不同的入口其实是不同的图.
    /// 一个子图有不同的入口不同的出口
    /// 先在草纸上画图，先后写子图，先后链接图
    class ExcuteGraph<T> where T:class
    {
        public T Agent { get; set; }
    }

    class SubGraphTemp1
    {
        /// <summary>
        /// 入口没有返回值
        /// </summary>
        /// <param name="token"></param>
        public void EnterPoint1(CancellationToken token)
        {
            
        }

        public void EnterPoint2(CancellationToken token)
        {
            
        }

        /// <summary>
        /// 出口是可等带的, 等带结果是一个状态码和在图中传递的取消令牌，避免抛出取消异常
        /// </summary>
        public Task<(int code, CancellationToken)> Outpint1 { get; }
        /// <summary>
        /// 
        /// </summary>
        public Task<(int, CancellationToken)> Outpint2 { get; }
    } 

    class PlaySongGraph: ExcuteGraph<Song>
    {
        PlaySongGraph(CancellationToken token)
        {
            Node node = new Node(Agent.Play);
            EnterPoint1 = node.ExcuteAsync;
            Node destory = new Node(Agent.Destrory);
            node.Next[0] = destory;
            Outpint1 = destory.Result;
        }

        /// <summary>
        /// 出口是可等带的, 等带结果是一个状态码和在图中传递的取消令牌，避免抛出取消异常
        /// </summary>
        public Task<(int code, CancellationToken)> Outpint1 { get; }

        Action<CancellationToken> EnterPoint1 { get; set; }

        class Node
        {
            private Func<CancellationToken, Task<(int code, CancellationToken token)>> play;
            private TaskCompletionSource<(int code, CancellationToken)> source;

            public Node(Func<CancellationToken, Task<(int code, CancellationToken token)>> destrory)
            {
                this.play = destrory;
                source = new TaskCompletionSource<(int code, CancellationToken)>();
            }

            public List<Node> Next { get; set; }
            public Task<(int code, CancellationToken)> Result => source.Task;

            internal async void ExcuteAsync(CancellationToken obj)
            {
                var res = await play?.Invoke(obj);
                Next[res.code]?.ExcuteAsync(res.Item2);
                source.SetResult(res);
            }
        }
        /// <summary>
        /// 构建一个流程，播放一首音乐，如果成功播放完成一次，结束时等待N秒自动销毁。
        /// 播放其他音乐会打断当前音乐，如果在此播放正在自动销毁的音乐，停止自动销毁，重置倒计时.
        /// 
        /// 为每个节点分配一个位置编号，位置不同，相同的函数成功失败后执行的逻辑可能不同.
        /// </summary>

        public async void PLay(CancellationToken token)
        {
            EnterPoint1?.Invoke(token);
        }
    }

    class Song
    {
        internal Task<(int code, CancellationToken token)> Destrory(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        internal Task<(int code, CancellationToken token)> Play(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
