using FluentValidation;
using TestShop.Models.Models;
using TestShop.Models.ViewModels;

namespace TestShop.Models.Validation;

public class ProductVMValidator:AbstractValidator<ProductVM>
{
	public ProductVMValidator(IValidator<Product> productValidator)
	{
		RuleFor(x => x.Product).SetValidator(productValidator);

		RuleFor(x=> x.CategoryIds)
			.NotNull()
			.WithMessage("At least 1 category must be chosen")
			.NotEmpty();
	}
}