using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame_2DPlatformer.Core
{
    class Time
    {

        /*
        public static IEnumerable<string> getList
        {
            get
            {
                return store.AsReadonly();
            }
        }

        */

        public static float DeltaTime
        {
            get
                {
                return (float)Game1._gameTime.ElapsedGameTime.TotalSeconds;
                }
        }        

    }
}
