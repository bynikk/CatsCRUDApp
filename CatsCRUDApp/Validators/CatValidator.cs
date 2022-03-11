using BLL.Entities;
using FluentValidation;

namespace CatsCRUDApp.Validators
{
    public class CatValidator : AbstractValidator<Cat>
    {
        public CatValidator()
        {
            RuleFor(c => c.Id > 0).NotEmpty();
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.CreatedDate).NotEmpty();
        }
    }
}
