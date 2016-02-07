using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGame_2DPlatformer.Core;

namespace MonoGame_2DPlatformer
{
     class SheetLoader
    {
       
        private struct UI
        {
          public Texture2D texture2d;
          public Vector2 pos;
        }

        static List<UI> _uiList = new List<UI>();


        public static void LoadSheet(Game1 game)
        {
           // string pathIO = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FMG_Tech\\Sheet");
           // string dirIO = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FMG_Tech\\Sheet");
           string pathIO = Game1.content.RootDirectory.ToString() + "\\Test";

            GameDebug.Log(pathIO);

            if (Directory.Exists(pathIO))
            {
                //Exists

                string[] data;

                FileStream fileStream = File.OpenRead(pathIO + "\\test.fmgui");

                byte[] bytes = new byte[fileStream.Length];

                fileStream.Read(bytes, 0, bytes.Length);
                data = System.Text.Encoding.UTF8.GetString(bytes).Split('\n');

                if (!data[0].Contains("fmgui"))
                {
                    Console.WriteLine("Wrong format !");
                }

                for (int i = 0; i < data.Length; i++)
                {
                    string match = Regex.Match(data[i], @"\[([^]]*)\]").Groups[1].Value;

                    match = match.Replace(" ", "");

                    string[] cords = match.Split(',');

                    if (cords[0].Contains("Image"))
                    {
                        // GameObject.Instantiate(_texture2D, new Vector2(float.Parse(cords[1]), float.Parse(cords[2])));
                        Console.WriteLine("Item Found!");

                        UI _ui = new UI();
                        _ui.pos = new Vector2(float.Parse(cords[1]), float.Parse(cords[2]));
                        _ui.texture2d = game.Content.Load<Texture2D>((cords[3]));
                        _uiList.Add(_ui);

                        GameDebug.Log((cords[3]).ToString());

                    }
                    /*     else if (cords[0].Contains("Start"))
                         {
                             GameObject.Instantiate(Startline, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                         }
                         else if (cords[0].Contains("Finish"))
                         {
                             GameObject.Instantiate(Finish, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                         }
                         */
                }

                //  Debug.Log("loaded");
                Console.WriteLine("Loaded File !");
                return;
            }
            else
            {
                //    Debug.LogError("Directory is not existing");
                Console.WriteLine("Directory is not existing !");
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UI u in _uiList)
            {
                spriteBatch.Draw(u.texture2d, u.pos, null, Color.White);// 0f, Vector2.Zero, 1f, SpriteEffects.None, 5f);
            }
        }


    }
}


