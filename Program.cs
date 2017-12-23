using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSample
{
	class Program
	{
		static void Main(string[] args)
		{
			var tokenSource = new CancellationTokenSource();
			var token = tokenSource.Token;
			List<Task> tasks = new List<Task>();
			tasks.Add(Task.Factory.StartNew(() => DoWork(token), token));
			tasks.Add(Task.Factory.StartNew(() => DoWork(token), token));
			Thread.Sleep(5000);

			tokenSource.Cancel();
			Task.WaitAll(tasks.ToArray());
		}

		private static void DoWork(CancellationToken token)
		{
			Console.WriteLine("**Processor starting, {0}", Thread.CurrentThread.ManagedThreadId);

			try
			{
				while (!token.IsCancellationRequested)
				{
					Console.WriteLine("Performing work, {0}...", Thread.CurrentThread.ManagedThreadId);
					Thread.Sleep(1000);
				}

				Console.WriteLine("**Processor terminating gracefully, {0}", Thread.CurrentThread.ManagedThreadId);
			}
			catch (Exception e)
			{
				Console.WriteLine("**Processor exception, {0}\n" + e, Thread.CurrentThread.ManagedThreadId);
			}
		}

	}
}
