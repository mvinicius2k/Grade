namespace Grade.Helpers
{
    public class WebHostUtils
    {
        public static IWebHostEnvironment? GetWebEnvironment()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            
            if(httpContext != null)
            {
                return httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

            }
            return null;
        }
    }
}
