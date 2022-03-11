using BLL.Entities;
using CatsCRUDApp.Models;
using FluentValidation;

namespace CatsCRUDApp.Validators
{
    public class CatViewModelValidator : AbstractValidator<CatViewModel>
    {
        public CatViewModelValidator()
        {
            RuleFor(c => c.Id > 0).NotEmpty();
            RuleFor(c => c.Name).NotEmpty().MaximumLength(20);
            RuleFor(c => c.Description).NotEmpty().MaximumLength(30);
        }
    }
}
