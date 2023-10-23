using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using FluentValidation.Results;
using TestShop.Models.Models;

namespace TestShop.Models.Validation;

public class ProductValidator:AbstractValidator<Product>
{
	public ProductValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty();

		RuleFor(x => x.Author)
			.NotEmpty();

		RuleFor(x => x.ISBN)
			.NotEmpty();

		RuleFor(x=> x.Description)
			.NotEmpty();

		RuleFor(x => x.ListPrice)
			.NotEmpty()
			.InclusiveBetween(1, 1000).WithMessage("Value must be between 1 and 1000");

		RuleFor(x => x.Price)
			.NotEmpty()
			.InclusiveBetween(1, 1000).WithMessage("Value must be between 1 and 1000");

		RuleFor(x => x.Price100)
			.NotEmpty()
			.InclusiveBetween(1, 1000).WithMessage("Value must be between 1 and 1000");

		RuleFor(x => x.Price50)
			.NotEmpty()
			.InclusiveBetween(1, 1000).WithMessage("Value must be between 1 and 1000");

	}
}