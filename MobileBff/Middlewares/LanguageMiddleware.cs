using System.Globalization;

namespace MobileBff.Middlewares
{
    public class LanguageMiddleware
    {
        public static readonly string LanguageHeaderEnglish = "en-GB";
        public static readonly string LanguageHeaderSwedish = "sv-SE";

        private readonly RequestDelegate next;

        public LanguageMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var supportedLanguages = new[] { LanguageHeaderEnglish, LanguageHeaderSwedish };

            var language = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            if (language == null || !supportedLanguages.Contains(language))
            {
                language = LanguageHeaderEnglish;
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);

            // Call the next delegate/middleware in the pipeline.
            await next(context);
        }
    }
}
