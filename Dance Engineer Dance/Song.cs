using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class Song
        {
            public static Dictionary<string,Song> SongList = new Dictionary<string,Song>();
            public static string SongListString
            {
                get
                {
                    string result = "";
                    SongList.Keys.ToList().ForEach(key => result += SongList[key].title+ "\n");
                    return result;
                }
            }
            public static string GetAlbumSongs(string album)
            {
                string result = "";
                SongList.Keys.ToList().ForEach(key => { if (SongList[key].album == album) result += SongList[key].title + "\n"; });
                return result;
            }
            public static List<string> GetAlbums()
            {
                List<string> result = new List<string>();
                SongList.Keys.ToList().ForEach(key => { if (!result.Contains(SongList[key].album)) result.Add(SongList[key].album); });
                return result;
            }
            public static void LoadSongs(string data)
            {
                GridInfo.Echo("Loading songs... "+data.Length);
                string[] songs = data.Split('\n');
                foreach(string song in songs)
                {
                    new Song(song);
                }
            }
            public static Song GetSong(string title)
            {
                if(SongList.ContainsKey(title)) return SongList[title];
                return null;
            }
            static Random random = new Random();
            public static Song GetRandomSong()
            {
                int index = random.Next(SongList.Count);
                return SongList.Values.ToList()[index];
            }
            public string title;
            public string track;
            public string arrows;
            public int bpm;
            public int length;
            public string album;
            public Song(string data)
            {
                string[] parts = data.Split('-');
                if(parts.Length == 2) arrows = parts[1];
                else arrows = "";
                parts = parts[0].Split(',');
                foreach(string part in parts)
                {
                    string[] subparts = part.Split(':');
                    if(subparts.Length == 2)
                    {
                        switch(subparts[0])
                        {
                            case "title":
                                title = subparts[1];
                                break;
                            case "track":
                                track = subparts[1];
                                break;
                            case "bpm":
                                bpm = int.Parse(subparts[1]);
                                break;
                            case "length":
                                length = int.Parse(subparts[1]);
                                break;
                            case "album":
                                album = subparts[1];
                                break;
                        }
                    }
                }
                GridInfo.Echo("Loaded song: " + title);
                if(title != null) SongList.Add(title, this);
            }
        }
    }
}
