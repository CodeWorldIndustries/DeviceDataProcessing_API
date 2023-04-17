using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Validators
{
    public static class ValidationHelper
    {
        public static void ValidateProperties<T>(T request) where T : class
        {
            // Initialize validation context
            var context = new ValidationContext(request, null, null);
            var results = new List<ValidationResult>();

            // Validate request model
            if (!Validator.TryValidateObject(request, context, results, true))
                foreach (var result in results)
                    throw new ValidationException(result.ErrorMessage);
        }
    }
}
