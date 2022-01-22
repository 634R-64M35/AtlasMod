using Terraria.ModLoader;

namespace AtlasMod.Common.Hooks
{
    public abstract class ModHook : ILoadable
    {
        public Mod Mod { get; private set; }

        public virtual void Load() { }
        public virtual void Unload() { }

        void ILoadable.Load(Mod mod)
        {
            Mod = mod;
            this.Load();
        }

        void ILoadable.Unload()
        {
            this.Unload();
            Mod = null;
        }
    }
}
