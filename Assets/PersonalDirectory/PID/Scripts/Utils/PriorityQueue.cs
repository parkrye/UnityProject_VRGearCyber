using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PID 
{
    public class PriorityQueue<TElement> where TElement : IComparable<TElement>
    {
        private List<TElement> nodes;

        public PriorityQueue()
        {
            this.nodes = new List<TElement>();
        }


        public int Count { get { return nodes.Count; } }

        public void Enqueue(TElement element)
        {
            PushHeap(element);
        }

        public TElement Peek()
        {
            if (nodes.Count == 0)
                throw new InvalidOperationException();

            return nodes[0];
        }

        public bool TryPeek(out TElement element)
        {
            if (nodes.Count == 0)
            {
                element = default(TElement);
                return false;
            }

            element = nodes[0];
            return true;
        }

        public bool Contains(TElement element)
        {
            return nodes.Contains(element);
        }

        public TElement Dequeue()
        {
            if (nodes.Count == 0)
                throw new InvalidOperationException();

            TElement rootNode = nodes[0];
            PopHeap();
            return rootNode;
        }

        public bool TryDequeue(out TElement element)
        {
            if (nodes.Count == 0)
            {
                element = default(TElement);
                return false;
            }
            TElement rootNode = nodes[0];
            element = rootNode;
            PopHeap();
            return true;
        }
        public void UpdateHeap(TElement element)
        {
            int currentIndex = nodes.IndexOf(element);

            while (true)
            {
                int parentIndex = GetParentIndex(currentIndex);
                TElement parent = nodes[parentIndex];
                if (nodes[currentIndex].CompareTo(parent) < 0)
                {
                    nodes[currentIndex] = parent; //swap parent to the newCell's index 
                    currentIndex = parentIndex;
                }
                else
                    break;
            }
            nodes[currentIndex] = element;
        }
        private void PushHeap(TElement newCell)
        {
            nodes.Add(newCell);
            int newNodeIndex = nodes.Count - 1;
            while (newNodeIndex > 0)
            {
                int parentIndex = GetParentIndex(newNodeIndex);
                TElement parentNode = nodes[parentIndex];

                // if newCell priority is higher than the parent, 
                if (newCell.CompareTo(parentNode) < 0)
                {
                    nodes[newNodeIndex] = parentNode; //swap parent to the newCell's index 
                    newNodeIndex = parentIndex;
                }
                else
                {
                    break;
                }
            }
            nodes[newNodeIndex] = newCell;
        }

        private void PopHeap()
        {
            TElement lastNode = nodes[nodes.Count - 1];
            nodes.RemoveAt(nodes.Count - 1);

            int index = 0;
            while (index < nodes.Count)
            {
                int leftChildIndex = GetLeftChildIndex(index);
                int rightChildIndex = GetRightChildIndex(index);

                if (rightChildIndex < nodes.Count)
                {
                    int compareIndex = nodes[leftChildIndex].CompareTo(nodes[rightChildIndex]) < 0 ?
                    leftChildIndex : rightChildIndex;

                    if (nodes[compareIndex].CompareTo(lastNode) < 0)
                    {
                        nodes[index] = nodes[compareIndex];
                        index = compareIndex;
                    }
                    else
                    {
                        nodes[index] = lastNode;
                        break;
                    }
                }
                else if (leftChildIndex < nodes.Count)
                {
                    //There could be a case where there's only leftchildindex remaining
                    if (nodes[leftChildIndex].CompareTo(lastNode) < 0)
                    {
                        nodes[index] = nodes[leftChildIndex];
                        index = leftChildIndex;
                    }
                    else
                    {
                        nodes[index] = lastNode;
                        break;
                    }
                }
                else
                {
                    nodes[index] = lastNode;
                    break;
                }
            }
        }

        private int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        private int GetLeftChildIndex(int parentIndex)
        {
            return parentIndex * 2 + 1;
        }

        private int GetRightChildIndex(int parentIndex)
        {
            return parentIndex * 2 + 2;
        }
    }
}

