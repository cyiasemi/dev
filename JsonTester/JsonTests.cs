using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JsonTester
{
    public class JsonTests
    {
        public JsonTests()
        {
            Console.WriteLine("Reading file...");

            _byteFile = File.ReadAllBytes("generated.json");
            var runIntervalCount = 6;
            var runEachDeserializerCount = 100;
            _jsondotText = new SystemTextConverter<List<FamilyMember>>();
            _jsonNewton = new NewtonsoftConverter<List<FamilyMember>>();
            Console.WriteLine($"File size in Bytes = {_byteFile.Length}");
            //warming up
            for (int i = 1; i <= runIntervalCount; i++)
            {

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[Test {i}]");
                Console.WriteLine($"Reading {runEachDeserializerCount} times, total bytes {_byteFile.Length * runEachDeserializerCount}");
                Console.ResetColor();
                _testTimeNewton = 0;
                _testTimeSystemText = 0;
                if (i % 2 == 0)
                {

                    Console.WriteLine($"First System.Text and then NewtonSoft");
                    for (int k = 0; k < runEachDeserializerCount; k++)
                    {
                        RunSystemText();
                        RunNewtonsoft();
                    }

                }
                else
                {
                    Console.WriteLine($"First NewtonSoft and then System.Text ");
                    for (int k = 0; k < runEachDeserializerCount; k++)
                    {
                        RunNewtonsoft();
                        RunSystemText();
                    }
                }
                _totalTimeNewton += _testTimeNewton;
                _totalTimeSystemText += _testTimeSystemText;
                Console.WriteLine();
                Console.WriteLine($"{new string('*', 20)}");
                Console.WriteLine($"{new string('*', 20)} Results: Test Time Newton: {_testTimeNewton} ms");
                Console.WriteLine($"{new string('*', 20)} Results: Test Time System: {_testTimeSystemText} ms");
                Console.WriteLine($"{new string('*', 20)}");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(new string('=', 100));
            Console.WriteLine($"{new string('#', 20)}");
            Console.WriteLine($"{new string('#', 20)} Total MB read: {Convert.ToDecimal(_byteFile.Length * runEachDeserializerCount * runIntervalCount) / 1024 / 1024 }");
            Console.WriteLine($"{new string('#', 20)} Results: Total Time Newton: {_totalTimeNewton} ms");
            Console.WriteLine($"{new string('#', 20)} Results: Total Time System: {_totalTimeSystemText} ms");
            Console.WriteLine($"{new string('#', 20)}");
            Console.WriteLine(new string('=', 100));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{new string('#', 20)} Diferrence {Math.Abs(_totalTimeNewton - _totalTimeSystemText)} ms");
            Console.WriteLine(new string('=', 100));
        }
        private NewtonsoftConverter<List<FamilyMember>> _jsonNewton;
        private SystemTextConverter<List<FamilyMember>> _jsondotText;
        private byte[] _byteFile;
        private long _totalTimeNewton;
        private long _totalTimeSystemText;
        private long _testTimeNewton;
        private long _testTimeSystemText;

        [Benchmark]
        public long RunNewtonsoft()
        {
            _testTimeNewton += RunTests(() => _jsonNewton.Deserialize(_byteFile), "Json.NewtonSoft", false);
            return _testTimeNewton;
        }
        [Benchmark]
        public long RunSystemText()
        {
            _testTimeSystemText += RunTests(() => _jsondotText.Deserialize(_byteFile), "Json.Text", false);
            return _testTimeSystemText;
        }

        private long RunTests(Func<List<FamilyMember>> p, string testType, bool showLogs = true)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            p.Invoke();
            sw.Stop();

            if (showLogs)
                Console.WriteLine($"Test results for {testType}: {sw.ElapsedMilliseconds} ms");
            return sw.ElapsedMilliseconds;
        }
    }
}
