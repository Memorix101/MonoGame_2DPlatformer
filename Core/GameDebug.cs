using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_2DPlatformer.Core
{
    class GameDebug
    {

        public static void Log(string text)
        {
            //  Trace.WriteLine(text);
            Console.WriteLine(text);
        }

    }
}
