using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpriteGame.Assets
{
    /// <summary>
    /// Project-wide default options for JSON deserialization
    /// </summary>
    static class JsonOptions
    {
        public static JsonSerializerOptions DefaultOptions { get; }
            = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                WriteIndented = true
            };
    }
}
