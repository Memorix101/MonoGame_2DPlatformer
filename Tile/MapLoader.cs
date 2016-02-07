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
    class MapLoader
    {

        private struct MapItems
        {
            public ItemTile itemTile;
            public Vector2 pos;
        }

        static List<MapItems> _mapItemList = new List<MapItems>();


        public void LoadSheet(Game1 game)
        {
            string pathIO = Game1.content.RootDirectory.ToString() + "\\Levels";

            GameDebug.Log(pathIO);

            if (Directory.Exists(pathIO))
            {
                string[] data;

                FileStream fileStream = File.OpenRead(pathIO + "\\test.map");

                byte[] bytes = new byte[fileStream.Length];

                fileStream.Read(bytes, 0, bytes.Length);
                data = System.Text.Encoding.UTF8.GetString(bytes).Split('\n');

                if (!data[0].Contains("sheetmap"))
                {
                    throw new Exception(String.Format("Wrong Map Format"));
                }

                for (int i = 0; i < data.Length; i++)
                {
                    string match = Regex.Match(data[i], @"\[([^]]*)\]").Groups[1].Value;

                    match = match.Replace(" ", "");

                    string[] cords = match.Split(',');

                    if (cords[0].Contains("Block"))
                    {
                        Console.WriteLine("Item Found!");

                        MapItems _mapitems = new MapItems();

                        _mapitems.itemTile = new ItemTile(_mapitems.pos, 1f, ItemTileType.Blank);
                        _mapitems.pos = new Vector2(float.Parse(cords[1]), float.Parse(cords[2]));
                       // _mapitems.itemTile.Init(_mapitems.pos, 1f);
                      //  _mapitems.itemTile.LoadBlock();
                        _mapItemList.Add(_mapitems);

                        //GameDebug.Log((cords[3]).ToString());

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

                // Console.WriteLine("Loaded File !");
                return;
            }
            else
            {
                // Console.WriteLine("Directory is not existing !");
                throw new Exception(String.Format("Directory is not existing !"));
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MapItems u in _mapItemList)
            {
                //   spriteBatch.Draw(u.itemTile, u.pos, null, Color.White);// 0f, Vector2.Zero, 1f, SpriteEffects.None, 5f);
                u.itemTile.Draw(spriteBatch);
            }
        }


    }
}


