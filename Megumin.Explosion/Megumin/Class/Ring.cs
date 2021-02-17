using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    public class Node<T>
    {
        public Node<T> Next { get; set; }

        public Node<T> Previous { get; set; }

        public T Value { get; set; }

        /// <summary>
        /// 从原位置摘出来并修复前后链接
        /// </summary>
        public void Pop()
        {
            var p = this.Previous;
            var n = this.Next;
            if (p != null)
            {
                p.Next = n;
            }

            if (n != null)
            {
                n.Previous = p;
            }
        }

        /// <summary>
        /// 在 this 后面插入 node
        /// </summary>
        /// <param name="node"></param>
        public void AfterInsert(Node<T> node)
        {
            node.Pop();

            node.Previous = this;
            node.Next = Next;
            if (Next != null)
            {
                Next.Previous = node;
            }
            Next = node;
        }

        /// <summary>
        /// 在 this 前面插入 node
        /// </summary>
        /// <param name="node"></param>
        public void BeforeInsert(Node<T> node)
        {
            node.Pop();

            node.Next = this;
            node.Previous = Previous;
            if (Previous != null)
            {
                Previous.Next = node;
            }
            Previous = node;
        }
    }

    /// <summary>
    /// 双向环
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Ring<T>
    {
        public class RingNode<T> : Node<T>
        {
            public RingNode(Ring<T> ring)
            {
                Ring = ring;
            }

            public Ring<T> Ring { get; protected set; }
        }

        public Node<T> Current { get; set; }

        public Ring(int count)
        {
            count = Math.Max(0, count);

            if (count > 0)
            {
                Current = new RingNode<T>(this);
                Current.Next = Current;
                Current.Previous = Current;
                Expand(Current, count - 1);
            }
        }

        /// <summary>
        /// 扩容
        /// </summary>
        public void Expand(Node<T> node, int count, T value = default)
        {
            var p = node;
            var last = node.Next;
            for (int i = 0; i < count; i++)
            {
                RingNode<T> ringNode = new RingNode<T>(this);
                ringNode.Value = default;
                p.Next = ringNode;
                ringNode.Previous = p;
                p = ringNode;
            }

            p.Next = last;
            if (last != null)
            {
                last.Previous = p;
            }
        }


        //public readonly static Ring<int> TestRing = new TestRing01234();
        public class TestRing01234 : Ring<int>
        {
            public TestRing01234() : base(5)
            {
                Current.Value = 0;
                Current.Next.Value = 1;
                Current.Next.Next.Value = 2;
                Current.Next.Next.Next.Value = 3;
                Current.Next.Next.Next.Next.Value = 4;
            }
        }
    }
}



