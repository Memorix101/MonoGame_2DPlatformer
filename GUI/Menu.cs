using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame_2DPlatformer.Core;

namespace MonoGame_2DPlatformer
{

    enum MenuState
    {
        Play,
        Quit
    }

    static class Menu
    {

        public static bool enabled = true;

        static bool pressed = false;

        static Texture2D title;

        static GUI copyright;

        static List<GUI> MenuItems;
        static int currentItem = 0;

        static KeyboardState keyCurrent;
        static KeyboardState keyPrev;
        static MenuState menuState;

        static Menu()
        {

            title = Game1.content.Load<Texture2D>("Sprites/title");
            copyright = new GUI("A Game by Memorix101");
            copyright.Load("Fonts\\Pixel");
            copyright.Position = new Vector2(Screen.width / 2 - copyright.Size.X / 2, Screen.height - copyright.Size.Y);
            copyright.Color = Color.White;

            MenuItems = new List<GUI>();

            MenuItems.Add(new GUI("Play"));
            //  MenuItems.Add(new GUI("World Map"));
            MenuItems.Add(new GUI("Quit"));

            for (int i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].Load("Fonts\\70sPixel_20");
                MenuItems[i].Position = new Vector2(Screen.width / 2 - MenuItems[i].Size.X / 2, 50 * i + Screen.height / 2 + 50);
            }
        }


        public static void Update()
        {
            if (enabled)
            {
                Input();
                Selection();
            }
        }

        private static void Selection()
        {
            switch (currentItem)
            {
                case 0:
                    ResetColor();
                    MenuItems[0].Color = Color.Red;
                    break;
                case 1:
                    ResetColor();
                    MenuItems[1].Color = Color.Red;
                    break;
                case 2:
                    ResetColor();
                    MenuItems[2].Color = Color.Red;
                    break;

            }
        }


        static void ItemSelect()
        {
            switch (currentItem)
            {
                case 0:
                    enabled = false;
                    Level.Dispose();
                    Level.LoadLevel("test");
                    break;
                case 1:
                    System.Environment.Exit(1);
                    break;
                case 2:
                    Game1.quit = true;
                    break;
            }
        }

        private static void Input()
        {
            currentItem = MathHelper.Clamp(currentItem, (int)0, (int)MenuItems.Count);
            keyCurrent = Keyboard.GetState();

            if (keyCurrent.IsKeyDown(Keys.Up) && !keyPrev.IsKeyDown(Keys.Up))
            {
                currentItem--;            
            }

            if (keyCurrent.IsKeyDown(Keys.Down) && !keyPrev.IsKeyDown(Keys.Down))
            {
                currentItem++;
            }

            if (keyCurrent.IsKeyDown(Keys.Enter) && !keyPrev.IsKeyDown(Keys.Enter))
            {
                ItemSelect();
            }

            keyPrev = keyCurrent;
        }

        static void ResetColor()
        {
            foreach (GUI m in MenuItems)
            {
                m.Color = Color.Blue;
            }
            
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (enabled)
            {
                spriteBatch.Draw(title, new Vector2(Screen.width / 2 - title.Width / 2, Screen.height / 2 - title.Height), Color.White);
                copyright.Draw(spriteBatch);

                foreach (GUI m in MenuItems)
                {
                    m.Draw(spriteBatch);
                }
            }
        }



    }
}
