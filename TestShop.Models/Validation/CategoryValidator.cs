using FluentValidation;
using TestShop.Models.Models;

namespace TestShop.Models.Validation;
	
public class CategoryValidator:AbstractValidator<Category>
{
	public CategoryValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(30);
		RuleFor(x => x.CreatedDateTime)
			.NotEmpty()
			.LessThanOrEqualTo(DateTime.UtcNow);
		RuleFor(x => x.DisplayOrder)
			.NotEmpty()
			.InclusiveBetween(1, 100)
			.WithMessage("Display Order must be between 1-100");
	}
}