using System.Linq;

namespace API.Extractor.Helpers
{
    public class ImageHelper
    {
        public static string SanitizeBase64String(string base64)
        {
            var pieces = base64.Split(";base64,");
            return pieces.Count() == 2 ? pieces[1] : base64;
        }

        public static string GetExtensionFromBase64String(string base64)
        {
            var pieces = base64.Split(";");
            pieces = pieces[0].Split("/");
            return pieces.Count() == 2 ? $".{pieces[1]}" : "";
        }
    }
}
