﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyLib
{
    public class MyArrayList<T>:IMyList<T>
    {
        T[] elementData;
        int size;

        public MyArrayList()
        {
            elementData = null;
            size = 0;
        }
        public MyArrayList(T[] array)
        {
            elementData = new T[(int)(array.Length * 1.5)];
            for (int i = 0; i < array.Length; i++)
            {
                elementData[i] = array[i];
            }
            size = array.Length;
        }
        public MyArrayList(IMyCollection<T> list)
        {
            elementData = list.ToArray();
            size = list.Size();
        }
        public MyArrayList(int capacity)
        {
            elementData = new T[capacity];
            size = capacity;
        }
        public void Add(T item)
        {
            if (size == elementData.Length)
            {
                T[] newArray = new T[(int)(size * 1.5) + 1];
                for (int i = 0; i < size; i++) newArray[i] = elementData[i];
                elementData = newArray;
            }
            elementData[size] = item;
            size++;
        }
        public void Add(params T[] array)
        {
            foreach (T item in array) Add(item);
        }
        public void Add(IMyCollection<T> items)
        {
            Add(items.ToArray());
        }
        public void Add(int index, params T[] array)
        {
            if (index > size)
            {
                Add(array);
                return;
            }
            T[] newData = null;
            if (array.Length + size > elementData.Length)
            {
                newData = new T[size + array.Length];
                int i = 0, j = 0;
                while (i < newData.Length)
                {
                    if (i == index)
                    {
                        while (i < newData.Length && j < array.Length)
                        {
                            newData[i++] = array[j++];
                        }
                    }
                    if (i < newData.Length)
                    {
                        newData[i] = elementData[i - j];
                        i++;
                    }
                }
                elementData = newData;
                size = newData.Length;
                return;
            }
            int k = 0;
            for (int i = index; i < elementData.Length; i++)
            {
                if (k < array.Length)
                {
                    elementData[i] = array[k];
                    k++;
                    continue;
                }
                elementData[i] = elementData[i + k];
            }
            size += array.Length;
        }
        public void Add(int index, IMyCollection<T> items)
        {
            Add(index, items.ToArray());
        }
        public void Clear()
        {
            elementData = null;
            size = 0;
        }
        public bool Contains(params T[] array)
        {
            foreach (T item in array)
            {
                for (int i = 0; i < size; i++)
                {
                    T element = elementData[i];
                    if (element.Equals(item)) return true;
                }
            }
            return false;
        }
        public bool Contains(IMyCollection<T> items)
        {
            return Contains(items.ToArray());
        }
        public bool IsEmpty()
        {
            return size == 0;
        }
        public void Remove(params T[] obj)
        {
            foreach (T item in obj)
            {
                int i = 0;
                while (i < size)
                {
                    if (item.Equals(elementData[i]))
                    {
                        for (int j = i; j < size - 1; j++) elementData[j] = elementData[j + 1];
                        size--;
                    }
                    i++; ;
                }
            }
        }
        public T Remove(int index)
        {
            if ((index < 0) || (index >= size)) throw new ArgumentOutOfRangeException("index");
            T element = elementData[index];
            for (int i = index; i < size - 1; i++) elementData[i] = elementData[i + 1];
            size--;
            return element;
        }
        public void Remove(IMyCollection<T> items)
        {
            Remove(items.ToArray());
        }
        public void Retain(params T[] obj)
        {
            T[] newValue = new T[size];
            int newSize = 0;
            for (int i = 0; i < size; i++)
                foreach (T item in obj)
                    if (item.Equals(elementData[i]))
                    {
                        newValue[newSize] = elementData[i];
                        newSize++;
                    }
            size = newSize;
            elementData = newValue;
        }
        public void Retain(IMyCollection<T> items)
        {
            Retain(items.ToArray());
        }
        public int Size() { return size; }
        public T[] ToArray()
        {
            T[] answerArray = new T[size];
            for (int i = 0; i < size; i++) answerArray[i] = elementData[i];
            return answerArray;
        }
        public void ToArray(T[] array)
        {
            for (int i = 0; i < array.Length && i < size; i++) array[i] = (T)elementData[i];
        }
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= size)
                {
                    throw new ArgumentOutOfRangeException("index out of range");
                }
                return elementData[index];
            }
            set
            {
                if (index >= size || index < 0) throw new ArgumentOutOfRangeException("index out of range");
                if (value == null) throw new ArgumentNullException("value is null");
                elementData[index] = value;
            }
        }
        public int IndexOf(T element)
        {
            if (element == null) throw new ArgumentNullException("arg element is null");
            for (int i = 0; i < size; i++) if (element.Equals(elementData[i])) return i;
            return -1;
        }
        public int LastIndexOf(T element)
        {
            if (element == null) throw new ArgumentNullException("arg element is null");
            int index = -1;
            for (int i = 0; i < size; i++) if (element.Equals(elementData[i])) index = i;
            return index;
        }
        public IMyList<T> SubList(int indexFrom, int indexTo)
        {
            if (indexFrom < 0 || indexFrom >= size) throw new ArgumentOutOfRangeException("fromindex is out of range");
            if (indexTo < 0 || indexTo >= size) throw new ArgumentOutOfRangeException("indexTo is out of range");
            MyArrayList<T> result = new MyArrayList<T>(indexTo - indexFrom);
            for (int i = 0; i < result.size; i++)
            {
                result[i] = elementData[indexFrom + i];
            }
            return result;
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return elementData[i];
            }
        }
        public IEnumerator<T> ListIterator(int index = 0)
        {
            for (; index < size; index++) yield return elementData[index];
        }

        public void Print()
        {
            for (int i = 0; i < size; i++) Console.Write(elementData[i] + " ");
            Console.WriteLine();
        }
    }

}
