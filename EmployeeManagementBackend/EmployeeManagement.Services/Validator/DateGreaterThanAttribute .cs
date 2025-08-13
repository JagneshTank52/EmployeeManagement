using System;
using System.ComponentModel.DataAnnotations;

public class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not DateTime currentValue)
            return new ValidationResult($"{validationContext.DisplayName} must be a valid date.");

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property == null)
            return new ValidationResult($"Unknown property: {_comparisonProperty}");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance);
        if (comparisonValue is not DateTime comparisonDate)
            return new ValidationResult($"{_comparisonProperty} must be a valid date.");

        if (currentValue <= comparisonDate)
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} must be after {_comparisonProperty}."
            );

        return ValidationResult.Success!;
    }
}
