namespace Plugins.Util
{
    public static class Rando
    {
        public static string Guid(int length = 8) 
        {
            var guid = System.Guid.NewGuid();
            var str = guid.ToString().Replace("-", "");
            return str.Substring(0, length);
        }
    }
}