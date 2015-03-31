using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCup
{
    public static class Functions
    {
        public static int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            else
                return n * Factorial(n - 1);
        }

        public static void Sort(ref IComparable[] array, int first, int last)
        {
            if (first < last)
            {
                IComparable pivotValue;
                int leftPointer, rightPointer, pivot;
                IComparable temp;

                leftPointer = first + 1;
                rightPointer = last;
                pivot = first;
                pivotValue = array[pivot];


                //Commences sort
                while (leftPointer <= rightPointer)
                {
                    //scan to the left of the pivot
                    while ((leftPointer <= rightPointer) && array[leftPointer].CompareTo(pivotValue) > 0)
                    {
                        leftPointer++;
                    }
                    //scan to the right of the pivot
                    while ((leftPointer <= rightPointer) && array[rightPointer].CompareTo(pivotValue) <= 0)
                    {
                        rightPointer--;
                    }

                    //swap if two values need to be swapped.
                    if (leftPointer < rightPointer)
                    {
                        temp = array[leftPointer];
                        array[leftPointer] = array[rightPointer];
                        array[rightPointer] = temp;
                    }
                }

                temp = array[first];
                array[first] = array[rightPointer];
                array[rightPointer] = temp;

                //Quicksort the remaining values
                Sort(ref array, first, rightPointer - 1);
                Sort(ref array, rightPointer + 1, last);
            }
        }
    }
}
