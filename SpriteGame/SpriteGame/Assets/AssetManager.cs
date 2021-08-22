using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace SpriteGame.Assets
{
    /// <summary>
    /// Global class managing asset packs and their contents
    /// </summary>
    static class AssetManager
    {
        /// <summary>
        /// Collection of all asset packs
        /// </summary>
        private static Dictionary<string, AssetPack> Packs { get; set; }
            = new Dictionary<string, AssetPack>();

        /// <summary>
        /// Try to discover all packs available to the game
        /// </summary>
        public static void DiscoverPacks()
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "assets");

            foreach(var dir in Directory.GetDirectories(basePath))
            {
                var pack = AssetPack.Create(dir);
                Packs.Add(pack.Identifier, pack);
            }
        }

        /// <summary>
        /// Forces the asset manager to load pack with given name
        /// </summary>
        public static void ForceLoadPack(string name)
        {
            if (!Packs.ContainsKey(name))
                throw new ArgumentException($"Asset pack with name \"{name}\" does not exist");

            if(!Packs[name].IsLoaded)
                Packs[name].LoadAssets();
        }

        /// <summary>
        /// Forces the asset manager to unload pack with given name
        /// </summary>
        public static void ForceUnloadPack(string name)
        {
            if (!Packs.ContainsKey(name))
                throw new ArgumentException($"Asset pack with name \"{name}\" does not exist");

            if (Packs[name].IsLoaded)
                Packs[name].UnloadAssets();
        }

        /// <summary>
        /// Get texture asset from a pack
        /// </summary>
        /// <param name="pack">Name of the asset pack to load asset from</param>
        /// <param name="name">Name of the asset to load</param>
        public static Image<Rgba32> GetTexture(string pack, string name)
        {
            if (!Packs.ContainsKey(pack))
                throw new ArgumentException($"Asset pack with name \"{pack}\" does not exist");

            var p = Packs[pack];

            if (!p.IsLoaded)
                p.LoadAssets();

            return p.GetTexture(name);
        }

        /// <summary>
        /// Get text asset from a pack
        /// </summary>
        /// <param name="pack">Name of the asset pack to load asset from</param>
        /// <param name="name">Name of the asset to load</param>
        public static string GetText(string pack, string name)
        {
            if (!Packs.ContainsKey(pack))
                throw new ArgumentException($"Asset pack with name \"{pack}\" does not exist");

            var p = Packs[pack];

            if (!p.IsLoaded)
                p.LoadAssets();

            return p.GetText(name);
        }
    }
}
