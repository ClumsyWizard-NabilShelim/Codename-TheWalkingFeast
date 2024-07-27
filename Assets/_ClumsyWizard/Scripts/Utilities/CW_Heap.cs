using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClumsyWizard.Utilities
{
    public interface IHeapItem<T> : IComparable<T>
    {
        public int HeapIndex { get; set; }
    }

    public class CW_Heap<T> where T : IHeapItem<T>
    {
        private T[] items;
        private int currentItemCount;
        public int Count => currentItemCount;

        public CW_Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        public void Add(T item)
        {
            items[currentItemCount] = item;
            item.HeapIndex = currentItemCount;
            currentItemCount++;
            SortUp(item);
        }

        public T RemoveFirst()
        {
            T itemRemoved = items[0];
            currentItemCount--;

            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;

            SortDown(items[0]);

            return itemRemoved;
        }

        //Helper Functions
        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                    Swap(item, parentItem);
                else
                    break;

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }
        private void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = (item.HeapIndex * 2) + 1;
                int childIndexRight = (item.HeapIndex * 2) + 2;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount)
                    {
                        if (items[childIndexRight].CompareTo(items[childIndexLeft]) < 0)
                            swapIndex = childIndexRight;
                    }

                    if (item.CompareTo(items[swapIndex]) < 0)
                        Swap(item, items[swapIndex]);
                    else
                        return;
                }
                else
                {
                    return;
                }
            }
        }
        private void Swap(T itemA, T itemB)
        {
            int itemAIndex = itemA.HeapIndex;

            items[itemB.HeapIndex] = itemA;
            itemA.HeapIndex = itemB.HeapIndex;

            items[itemAIndex] = itemB;
            itemB.HeapIndex = itemAIndex;
        }

        //Getters
        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }
    }
}