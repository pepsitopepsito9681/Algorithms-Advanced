﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _2._Knapsack
{
    public class Item
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            int maxCapacity = int.Parse(Console.ReadLine());
            var items = ReadItems();
            var dp = new int[items.Count + 1, maxCapacity + 1];
            var included = new bool[items.Count + 1, maxCapacity + 1];
            for (int row = 1; row < dp.GetLength(0); row++)
            {
                var currentItem = items[row - 1];
                for (int capacity = 1; capacity < dp.GetLength(1); capacity++)
                {
                    int skip = dp[row - 1, capacity];
                    if (currentItem.Weight>capacity)
                    {
                        dp[row, capacity] = skip;
                        continue;
                    }

                    var take = currentItem.Value+dp[row-1,capacity-currentItem.Weight];
                    if (take>skip)
                    {
                        dp[row, capacity] = take;
                        included[row, capacity] = true;
                    }
                    else
                    {
                        dp[row, capacity] = skip;
                    }
                }
            }
            
            var includedItems = new SortedSet<Item>(
                Comparer<Item>.Create((f,s)=>string.Compare(f.Name,s.Name,StringComparison.Ordinal)));
            int totalValue = dp[items.Count, maxCapacity];
            for (int row = included.GetLength(0)-1; row >= 0; row--)
            {
                if (included[row,maxCapacity])
                {
                    var includedItem = items[row - 1];
                    maxCapacity -= includedItem.Weight;
                    includedItems.Add(includedItem);
                }
            }
            int totalWeight = includedItems.Sum(item => item.Weight);
            
            Console.WriteLine("Total Weight: {0}",totalWeight);
            Console.WriteLine("Total Value: {0}", totalValue);
            foreach (var item in includedItems)
            {
                Console.WriteLine(item.Name);
            }
        }

        private static List<Item> ReadItems()
        {
            var result = new List<Item>();
            while (true)
            {
                string line = Console.ReadLine();
                if (line=="end")
                {
                    break;
                }
                var itemData = line.Split();
                result.Add(new Item
                {
                    Name = itemData[0],
                    Weight = int.Parse(itemData[1]),
                    Value = int.Parse(itemData[2])
                }
                    );
            }
            return result;
        }
    }
}
