using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SpriteGame.Assets
{
    /// <summary>
    /// Represents a single, isolated and namespaced asset pack
    /// </summary>
    class AssetPack
    {
        /// <summary>
        /// The identifier of the pack. Used when specified from which pack to load an asset
        /// </summary>
        public string Identifier { get; protected set; }

        /// <summary>
        /// Human-readable name of the pack
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// A short descriptive text about the asset pack.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// The file system base path for this asset path
        /// </summary>
        public string BasePath { get; protected set; }

        /// <summary>
        /// Whether this asset pack is currently loaded
        /// </summary>
        public bool IsLoaded { get; protected set; }
            = false;

        /// <summary>
        /// All string assets stored by their identifier
        /// </summary>
        [JsonIgnore]
        protected Dictionary<string, string> StringAssets { get; set; }
            = new Dictionary<string, string>();

        /// <summary>
        /// All texture assets stored by their identifier
        /// </summary>
        [JsonIgnore]
        protected Dictionary<string, Image<Rgba32>> TextureAssets { get; set; }
            = new Dictionary<string, Image<Rgba32>>();

        /// <summary>
        /// Load asset pack information from file system
        /// </summary>
        /// <returns></returns>
        public static AssetPack Create(string basePath)
        {
            var jsonPath = Path.Combine(basePath, "pack.json");
            if (!File.Exists(jsonPath))
                throw new ArgumentException($"Asset pack at \"{basePath}\" doesnt contain pack.json");

            var jsonData = File.ReadAllText(jsonPath);

            AssetPack pack = JsonSerializer.Deserialize<AssetPack>(jsonData, JsonOptions.DefaultOptions);
            pack.BasePath = basePath;
            return pack;
        }


        /// <summary>
        /// Make sure that all assets of this pack are loaded into memory
        /// </summary>
        public void LoadAssets()
        {
            this.LoadTextAssets();
            this.LoadTextureAssets();
            this.IsLoaded = true;
        }

        /// <summary>
        /// Discover and load all text assets
        /// </summary>
        protected void LoadTextAssets()
        {
            var textPath = Path.Combine(this.BasePath, "text");
            if(Directory.Exists(textPath))
            {
                foreach(var file in Directory.GetFiles(textPath))
                {
                    var extension = Path.GetExtension(file);
                    var filename = Path.GetFileName(file);
                    var filenameRaw = Path.GetFileNameWithoutExtension(file);

                    if(extension == ".txt" || extension == ".json")
                    {
                        var text = File.ReadAllText(file);
                        this.StringAssets.Add(filenameRaw, text);

                        Console.WriteLine($"Loaded: {filename}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Ignored: {filename} (not a text resource)");
                        Console.ResetColor();
                    }
                }
            }
        }

        /// <summary>
        /// Discover and load all texture assets
        /// </summary>
        protected void LoadTextureAssets()
        {
            var texPath = Path.Combine(this.BasePath, "texture");
            if (Directory.Exists(texPath))
            {
                foreach (var file in Directory.GetFiles(texPath))
                {
                    var extension = Path.GetExtension(file);
                    var filename = Path.GetFileName(file);
                    var filenameRaw = Path.GetFileNameWithoutExtension(file);

                    if (extension == ".png")
                    {
                        var img = Image<Rgba32>.Load(file);
                        this.TextureAssets.Add(filenameRaw, img);

                        Console.WriteLine($"Loaded: {filename}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Ignored: {filename} (not a PNG texture resource)");
                        Console.ResetColor();
                    }
                }
            }
        }

        /// <summary>
        /// Unload all assets of this pack.
        /// </summary>
        public void UnloadAssets()
        {
            this.StringAssets.Clear();
            this.TextureAssets.Clear();
            GC.Collect();
            this.IsLoaded = false;
        }

        /// <summary>
        /// Get texture asset from this pack with given name
        /// </summary>
        public Image<Rgba32> GetTexture(string name)
        {
            if (!this.TextureAssets.ContainsKey(name))
                throw new ArgumentException($"Asset pack {this.Identifier} does not contain a texture asset with name {name}");

            return this.TextureAssets[name];
        }

        /// <summary>
        /// Get text asset from this pack with given name
        /// </summary>
        public string GetText(string name)
        {
            if (!this.StringAssets.ContainsKey(name))
                throw new ArgumentException($"Asset pack {this.Identifier} does not contain a text asset with name {name}");

            return this.StringAssets[name];
        }
    }
}
