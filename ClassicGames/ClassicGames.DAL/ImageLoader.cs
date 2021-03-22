using Serilog;
using System.IO;
using System.Reflection;

namespace ClassicGames.DAL
{
    public static class ImageLoader
    {
        public static byte[] GetGameCover(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Assets." + fileName);
            if (stream != null)
            {
                BinaryReader reader = new BinaryReader(stream);
                byte[] content = new byte[stream.Length];
                stream.Read(content, 0, content.Length);
                reader.Close();
                return content;
            }
            else
            {
                Log.Warning("{0} isimli resource dosyası bulunamadı.", fileName);
                return null;
            }
        }
    }
}
