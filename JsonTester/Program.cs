using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace JsonTester
{
    class Program
    {
        static void Main(string[] args)
        {

            // BenchmarkRunner.Run<JsonTests>();
            JsonTests jt = new JsonTests();
            jt.RunNewtonsoft();
            Console.ReadLine();
        }
    }
}
