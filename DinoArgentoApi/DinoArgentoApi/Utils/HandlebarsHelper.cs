using System;
using System.IO;
using System.Text;
using HandlebarsDotNet;

namespace DinoArgentoApi.Utils
{
    
    public static class HandlebarsHelper
    {
        private static readonly IHandlebars HB = Handlebars.Create();

        public static string Render(string source, object data)
        {
            var template = HB.Compile(source);
            var result = template(data);
            return result;
        }

        public static string RenderFile(string filePath, object data)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"No se encontró la plantilla en: {filePath}");
            }
            var source = File.ReadAllText(filePath, Encoding.UTF8);
            return Render(source, data);
        }

        public static string GenerateResetPwdTemplate(object data)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Templates", "reset-password.html");
            return RenderFile(filePath, data);
        }

        public static string GenerateConfirmEmailTemplate(object data)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Templates", "confirm-email.html");
            return RenderFile(filePath, data);
        }
    }
}