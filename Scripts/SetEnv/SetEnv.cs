#if UNITY_IPHONE
using System.Runtime.InteropServices;

public static class SetEnv {
	
	[DllImport ("__Internal")]
	private static extern int unitySetEnv(string name, string value);

	public static void Set(string name, string value) => unitySetEnv(name, value);
}
#endif