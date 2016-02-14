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

    enum GameUIState
    {
        None,
        Clear,
        Died,
    }

    static class GameUI
    {

        public static bool enabled = false;

        static bool pressed = false;

        static Texture2D title;
        static Texture2D background;


        static List<GUI> MenuItems;
        static int currentItem = 0;

        static KeyboardState keyCurrent;
        static KeyboardState keyPrev;

        static GameUI()
        {

            background = Game1.content.Load<Texture2D>("Sprites/gfxlib/area04_bkg0");
            MenuItems = new List<GUI>();

            MenuItems.Add(new GUI("Replay"));
            //  MenuItems.Add(new GUI("World Map"));
            MenuItems.Add(new GUI("Quit"));

            for (int i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].Load("Fonts\\70sPixel_20");
                MenuItems[i].Position = new Vector2(Screen.width / 2 - MenuItems[i].Size.X / 2, 50 * i + Screen.height / 2 + 50);
            }
        }


        public static void Display(GameUIState ui)
        {
            if (ui == GameUIState.Clear)
                title = Game1.content.Load<Texture2D>("Sprites/stage_clear");
            else if (ui == GameUIState.Died)
                title = Game1.content.Load<Texture2D>("Sprites/died");

            enabled = true;
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
                Rectangle screenRectangle = new Rectangle(0, 0, Screen.width, Screen.height);
                spriteBatch.Draw(background, screenRectangle, Color.White * 0.5f);
                spriteBatch.Draw(title, new Vector2(Screen.width / 2 - title.Width / 2, Screen.height / 2 - title.Height), Color.White);

                foreach (GUI m in MenuItems)
                {
                    m.Draw(spriteBatch);
                }
            }
        }



    }
}
