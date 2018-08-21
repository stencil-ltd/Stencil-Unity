namespace UI
{
    public abstract class Controller<T> : RegisterableBehaviour where T : Controller<T>
    {
        public static T Instance { get; private set; }

        public override void Register()
        {
            Instance = (T)this;
        }

        public override void Unregister()
        {
            Instance = Instance == this ? null : Instance;
        }
    }
}