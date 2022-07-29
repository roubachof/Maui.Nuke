﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ImageSourceHandlers.Forms.Sample.Profiler
{
	public static class Profiler
	{
		const string Tag = "XFProfiler";
		static readonly ConcurrentDictionary<string, Stopwatch> watches = new ConcurrentDictionary<string, Stopwatch> ();

		public static void Start (object view)
		{
			Start (view.GetType ().Name);
		}

		public static void Start (string tag)
		{
			Console.WriteLine("{0}Starting Stopwatch {1}", Tag, tag);

			var watch =
				watches [tag] = new Stopwatch ();
			watch.Start ();
		}

		public static void Stop (string tag)
		{
			Stopwatch watch;
			if (watches.TryRemove (tag, out watch)) {
                Console.WriteLine("{0}Stopwatch {1} took {2}", Tag, tag, watch.Elapsed);
			}
		}
	}
}
