using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Extensions
{
    /// <summary>
    /// Swagger operation filter to handle file upload parameters in API documentation
    /// </summary>
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParameters = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile))
                .ToList();

            if (!fileParameters.Any())
                return;

            operation.Parameters?.Clear();

            var properties = new Dictionary<string, IOpenApiSchema>();
            var required = new HashSet<string>();

            foreach (var parameter in context.MethodInfo.GetParameters())
            {
                if (parameter.ParameterType == typeof(IFormFile))
                {
                    properties[parameter.Name!] = new OpenApiSchema
                    {

                        Type = JsonSchemaType.String,
                        Format = "binary"
                    };
                }
                else if (parameter.ParameterType == typeof(Guid))
                {
                    properties[parameter.Name!] = new OpenApiSchema
                    {
                        Type = JsonSchemaType.String,
                        Format = "uuid"
                    };
                }
                else
                {
                    properties[parameter.Name!] = new OpenApiSchema
                    {
                        Type = JsonSchemaType.String
                    };
                }

                required.Add(parameter.Name!);
            }

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Object,
                            Properties = properties,
                            Required = required
                        }
                    }
                }
            };
        }
    }
}
