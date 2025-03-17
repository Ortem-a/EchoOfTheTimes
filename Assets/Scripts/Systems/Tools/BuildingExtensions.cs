using System;

namespace Systems.Tools
{
    public static class BuildingExtensions
    {
        public static T With<T>(this T self, Action<T> setter)
        {
            setter.Invoke(self);
            return self;
        }
        
        public static T With<T>(this T self, Action<T> apply, Func<bool> when)
        {
            if (when())
            {
                apply?.Invoke(self);
            }

            return self;
        }

        public static T With<T>(this T self, Action<T> apply, bool when)
        {
            if (when)
            {
                apply?.Invoke(self);
            }

            return self;
        }
    }
}