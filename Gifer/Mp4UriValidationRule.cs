using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace Gifer
{
    public class Mp4UriValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var uriString = value as String;
            if (String.IsNullOrWhiteSpace(uriString))
            {
                return new ValidationResult(false, "URL box is empty");
            }

            Uri uri;
            try
            {
                uri = new Uri(uriString);
            }
            catch (UriFormatException)
            {
                return Error("Invalid URI");
            }
            
            var path = uri.AbsolutePath;
            var extension = Path.GetExtension(path);

            if (extension != ".mp4")
            {
                return Error(
                    String.Format("Invalid extension: {0}", extension));
            }

            return ValidationResult.ValidResult;
        }

        private static ValidationResult Error(String msg)
        {
            return new ValidationResult(false, String.Format("Not a valid URL ({0})", msg));
        }
    }
}