using System.ComponentModel.DataAnnotations;

namespace TopFilmsRazor.Annotations
{
    public class MyFilmsAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int year)
            {
                return year <= DateTime.Now.Year;
            }
            return false;
        }
    }
}
