using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using NUnit.Framework;

namespace CVARC.Graphics.DirectX.Utils
{
	public class ThreadActionQueue
	{
		public ThreadActionQueue()
		{
			_thread = new Thread(MainLoop)
			          {
			          	IsBackground = true
			          };
			_thread.SetApartmentState(ApartmentState.STA);
			_thread.IsBackground = true;
			_thread.Start();
		}

		public void Enqueue(Action action)
		{
			_queue.Enqueue(action);
			_emptyLock.Set();
			_completion.WaitOne();
			if (_savedException != null)
			{
				Exception exception = _savedException;
				_savedException = null;
				throw exception;
			}
		}

		private void MainLoop()
		{
			while (true)
				TryDequeue();
		}

		

		private void TryDequeue()
		{
			_emptyLock.Wait();
			Action action = _queue.Dequeue();
			if (_queue.Count == 0)
				_emptyLock.Reset();
			try
			{
				action();
			}
			catch (Exception e)
			{
				typeof (Exception).GetMethod("InternalPreserveStackTrace",
				                             BindingFlags.Instance | BindingFlags.NonPublic)
					.Invoke(e, null);
				_savedException = e;
			}
			finally
			{
				_completion.Set();
			}
		}

		private readonly AutoResetEvent _completion = new AutoResetEvent(false);
		private readonly ManualResetEventSlim _emptyLock = new ManualResetEventSlim();
		private readonly Thread _thread;
		private readonly Queue<Action> _queue = new Queue<Action>();
		private Exception _savedException;
		
		
		public class ThreadActionQueueTests
		{
			[Test]
			public void TestException()
			{
				var q = new ThreadActionQueue();
				Assert.Throws<Exception>(() =>
										 q.Enqueue(() => { throw new Exception("test message"); }));
				Assert.DoesNotThrow(() => q.Enqueue(() => new int()));
			}
		}
	}
}