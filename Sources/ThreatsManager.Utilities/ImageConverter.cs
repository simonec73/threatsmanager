using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Newtonsoft.Json;

namespace ThreatsManager.Utilities
{
    public class ImageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Bitmap);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string image = (string) reader.Value;

            if (image != null)
            {
                byte[] byteBuffer = Convert.FromBase64String(image);
                MemoryStream memoryStream = new MemoryStream(byteBuffer);
                memoryStream.Position = 0;

                return (Bitmap) Bitmap.FromStream(memoryStream);
            }

            return null;
        }

        //convert bitmap to byte (serialize)
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Bitmap bitmap)
            {
                //System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                //writer.WriteValue((byte[]) converter.ConvertTo(bitmap, typeof(byte[])));

                using (var newBitmap = new Bitmap(bitmap))
                {
                    using (var stream = new MemoryStream())
                    {
                        newBitmap.Save(stream, ImageFormat.Png);
                        writer.WriteValue(stream.ToArray());
                    }
                }
            }
        }
    }
}