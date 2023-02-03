using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelForConsoleApp
{
  internal class Program
  {
    static void Main(string[] arguments)
    {
      var limit = 4_000_000; // 80 or 4_000_000
      if (arguments.Length != 0)
      {
        limit = int.Parse(arguments[0]);
      }

      var numbers = Enumerable.Range(2, limit + 1).ToList();
      var watch = Stopwatch.StartNew();
      var primeNumbersFromForeach = GetPrimeList(numbers);
      watch.Stop();

      var watchForParallel = Stopwatch.StartNew();
      var primeNumbersFromParallelForeach = GetPrimeListWithParallel(numbers);
      watchForParallel.Stop();

      Console.WriteLine($"Classical foreach loop | Total prime numbers : {primeNumbersFromForeach.Count} | Time Taken : {watch.ElapsedMilliseconds} ms.");
      foreach (int number in primeNumbersFromForeach.Take(20))
      {
        Console.Write($"{number} ");
      }

      Console.WriteLine("");
      Console.WriteLine($"Parallel.ForEach loop  | Total prime numbers : {primeNumbersFromParallelForeach.Count} | Time Taken : {watchForParallel.ElapsedMilliseconds} ms.");
      foreach (int number in primeNumbersFromParallelForeach.Where(n => n < 80).OrderBy(n => n))
      {
        Console.Write($"{number} ");
      }

      Console.WriteLine("");
      Console.WriteLine("Press any key to exit.");
      Console.ReadLine();
    }

    /// <summary>
    /// GetPrimeList returns Prime numbers by using sequential ForEach
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    private static IList<int> GetPrimeList(IList<int> numbers) => numbers.Where(IsPrime).ToList();

    /// <summary>
    /// GetPrimeListWithParallel returns Prime numbers by using Parallel.ForEach
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    private static IList<int> GetPrimeListWithParallel(IList<int> numbers)
    {
      var primeNumbers = new ConcurrentBag<int>();

      Parallel.ForEach(numbers, number =>
      {
        if (IsPrime(number))
        {
          primeNumbers.Add(number);
        }
      });

      return primeNumbers.ToList();
    }

    /// <summary>
    /// IsPrime returns true if number is Prime, else false.(https://en.wikipedia.org/wiki/Prime_number)
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private static bool IsPrime(int number)
    {
      if (number < 2)
      {
        return false;
      }

      if (number == 2 || number == 3 || number == 5 || number == 7)
      {
        return true;
      }

      if (number % 2 == 0)
      {
        return false;
      }

      for (var divisor = 3; divisor <= Math.Sqrt(number); divisor += 2)
      {
        if (number % divisor == 0)
        {
          return false;
        }
      }

      return true;
    }
  }
}
