using System.Collections;
using System.Collections.Generic;
using Ollie;
using UnityEngine;

public class Heap<T> where T : iHeapItem<T>
{
    private T[] items;
    private int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public bool Contains(T item)
    {
        //if our heap at address of item passed in is equal to the item passed in, returns true
        return Equals(items[item.heapIndex], item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public void UpdateItem(T item)
    {
        //only ever increasing priority in A* so don't need to SortDown
        SortUp(item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = (item.heapIndex * 2) + 1;
            int childIndexRight = (item.heapIndex * 2) + 2;
            int swapIndex = 0;

            //if child on left exists
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                //if child on right exists
                if (childIndexRight < currentItemCount)
                {
                    //compare children
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        //if child right is lower priority than child left, switch swapIndex
                        swapIndex = childIndexRight;
                    }
                }

                //if parent is lower priority than child (swapIndex)
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item,items[swapIndex]);
                }
                //if parent is higher priority, then it's in correct position
                else
                {
                    return;
                }
            }
            //if parent doesn't have any children, then it's in correct position
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.heapIndex - 1) / 2;
        
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.heapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;
        int itemAIndex = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = itemAIndex;
    }
}
