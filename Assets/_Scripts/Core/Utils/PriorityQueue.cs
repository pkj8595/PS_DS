using System.Collections.Generic;
using System;

public class PriorityQueue<T>
{
    private List<(T item, int priority)> heap;

    public PriorityQueue()
    {
        heap = new List<(T, int)>();
    }

    // 요소 추가
    public void Enqueue(T item, int priority)
    {
        heap.Add((item, priority));
        HeapifyUp(heap.Count - 1);
    }

    // 우선순위가 가장 높은 요소 제거 및 반환
    public T Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        T rootItem = heap[0].item;
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);

        HeapifyDown(0);
        return rootItem;
    }

    // 우선순위가 가장 높은 요소 확인
    public T Peek()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        return heap[0].item;
    }

    // 큐가 비어있는지 확인
    public bool IsEmpty()
    {
        return heap.Count == 0;
    }

    // 힙을 위로 정렬
    private void HeapifyUp(int index)
    {
        int parentIndex = (index - 1) / 2;
        if (index > 0 && heap[index].priority > heap[parentIndex].priority)
        {
            Swap(index, parentIndex);
            HeapifyUp(parentIndex);
        }
    }

    // 힙을 아래로 정렬
    private void HeapifyDown(int index)
    {
        int leftChildIndex = 2 * index + 1;
        int rightChildIndex = 2 * index + 2;
        int largestIndex = index;

        if (leftChildIndex < heap.Count && heap[leftChildIndex].priority > heap[largestIndex].priority)
        {
            largestIndex = leftChildIndex;
        }

        if (rightChildIndex < heap.Count && heap[rightChildIndex].priority > heap[largestIndex].priority)
        {
            largestIndex = rightChildIndex;
        }

        if (largestIndex != index)
        {
            Swap(index, largestIndex);
            HeapifyDown(largestIndex);
        }
    }

    // 두 요소의 위치를 변경
    private void Swap(int index1, int index2)
    {
        var temp = heap[index1];
        heap[index1] = heap[index2];
        heap[index2] = temp;
    }
}