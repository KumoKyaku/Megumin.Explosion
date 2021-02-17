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
                Ex(Current, count - 1);
            }
        }

        /// <summary>
        /// 扩容
        /// </summary>
        public void Ex(Node<T> node, int count)
        {
            var p = node;
            var last = node.Next;
            for (int i = 0; i < count; i++)
            {
                RingNode<T> ringNode = new RingNode<T>(this);
                p.Next = ringNode;
                ringNode.Previous = p;
                p = ringNode;
            }
            
            p.Next = last;
            last.Previous = p;
        }
    }
}
