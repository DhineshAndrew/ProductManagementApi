using FluentValidation;
using ProductManagementApi.Models;

namespace ProductManagementApi.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.productName)
                .NotEmpty().WithMessage("Product Name is required.")
                .MaximumLength(100).WithMessage("Product Name cannot exceed 100 characters.");

            RuleFor(product => product.description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(product => product.price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(product => product.category)
                .NotEmpty().WithMessage("Category is required.");
        }
    }
}
