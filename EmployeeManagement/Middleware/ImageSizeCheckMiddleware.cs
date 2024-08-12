using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagement.Middleware
{
    public class ImageSizeCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private const long MaxSizeInBytes = 1024 * 1024; // 1 MB

        public ImageSizeCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post &&
                context.Request.ContentType != null &&
                context.Request.ContentType.Contains("multipart/form-data"))
            {
                // Create a temporary stream to hold the request body
                var originalBodyStream = context.Request.Form.Files[0].Length;

                if (originalBodyStream > MaxSizeInBytes)
                {
                    
                    //Optional: Add a custom response or log the file details
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("File size should be less than 500kb");
                    return; // Stop further processing if only one file is expected
                }

            }

            // Call the next middleware in the pipeline
            await _next(context);
        }

    }

}
