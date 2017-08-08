using System.Collections;
using System;

namespace TKF
{
	public static class ActionExtensions
	{
		public static void SafeInvoke (this Action action)
		{
			if (action != null) {
				action ();
			}
		}

		public static void SafeInvoke<T> (this Action<T> action, T arg)
		{
			if (action != null) {
				action (arg);
			}
		}

		public static void SafeInvokeWithArg<T> (this Action<T> action, T arg)
		{
			if (action != null && arg != null) {
				action (arg);
			}
		}

		public static void SafeInvoke<T1, T2> (this Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			if (action != null) {
				action (arg1, arg2);
			}
		}

		public static void SafeInvokeWithArg<T1,T2> (this Action<T1,T2> action, T1 arg1, T2 arg2)
		{
			if (action != null && arg1 != null && arg2 != null) {
				action (arg1, arg2);
			}
		}

		public static void SafeInvoke<T1, T2, T3> (this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			if (action != null) {
				action (arg1, arg2, arg3);
			}
		}
	}
}