using System;

namespace AppMovies.Functions
{
    public class Base64ToImageConverter
    {
        public byte[] ConvertBase64ToImage(string base64String)
        {
            // Remove the prefix if present (e.g., "data:image/png;base64,")
            var base64Data = base64String.Split(',')[1];

            // Convert the base64 string to a byte array
            var bytes = Convert.FromBase64String(base64Data);

            return bytes;
        }
    }
}