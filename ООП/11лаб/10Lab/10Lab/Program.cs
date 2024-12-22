using System;
using _10labLib;

class Program
{
    static void Main(string[] args)
    {
        TestCollections testCollections = new TestCollections(1000);
        testCollections.MeasureSearchTimes();
    }
}
