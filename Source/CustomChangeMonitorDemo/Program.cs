using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomChangeMonitorDemo
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			FileContentProvider textFile = new FileContentProvider();
			Stopwatch sw = new Stopwatch();
			while (true)
			{
				sw.Reset();
				sw.Start();
				Console.WriteLine(textFile.Content);
				sw.Stop();
				Console.WriteLine("Elapsed Time: {0} ms", sw.ElapsedMilliseconds);
				Console.WriteLine(new string('=', 50));
				Console.ReadLine();
				Application.DoEvents();
			}
		}
	}

	public class FileContentProvider
	{
		public String Content
		{
			get
			{
				const string CACHE_KEY = "Content";
				string content = m_Cache[CACHE_KEY] as string;
				if (content == null)
				{
					CacheItemPolicy policy = new CacheItemPolicy();
					//policy.SlidingExpiration = TimeSpan.FromMilliseconds(1500);

					var changeMonitor = new ClipboardChangeMonitor();

					policy.ChangeMonitors.Add(changeMonitor);

					content = Guid.NewGuid().ToString();
					Thread.Sleep(1000);
					m_Cache.Set(CACHE_KEY, content, policy);
				}
				return content;
			}
		}

		private ObjectCache _cache;
		private ObjectCache m_Cache
		{
			get
			{
				if (_cache == null)
					_cache = MemoryCache.Default;
				return _cache;
			}
		}
	}
}
