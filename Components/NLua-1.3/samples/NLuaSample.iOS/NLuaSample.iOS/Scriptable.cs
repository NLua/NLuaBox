using System;
using System.Net;


namespace NLuaSample
{
#if MONOTOUCH
	[MonoTouch.Foundation.Preserve (AllMembers = true)]
#endif

	public class Scriptable : IDisposable
	{
		string param1;
		static int    param2;
		double param3;

		public Scriptable (string param1)
		{
			this.param1 = param1;
		}

		public void DoSomething ()
		{
			Console.WriteLine ("Inside DoSomething ()");
		}

		public string Param1 
		{
			get { return param1; }
		}

		public int SumOfLengths (string name, int number)
		{
			return param1.Length + name.Length + number;
		}

		public static void Print (string args)
		{
			Console.WriteLine (args);
		}

		public double Param3 {
			get {
				return param3;
			}
			set {
				param3 = value;
			}
		}

		public static int Param2 {
			get {
				return param2;
			}
			set {
				param2 = value;
			}
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			Console.WriteLine ("Disposing Scriptable");
		}

		#endregion
	}
}

