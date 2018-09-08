namespace UI
{
    public abstract class FakeController<T> where T : FakeController<T>, new()
    {
        private static T _instance;
        public static T Instance 
            => _instance ?? (_instance = new T());
    }
}