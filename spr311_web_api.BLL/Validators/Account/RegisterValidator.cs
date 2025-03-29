using FluentValidation;
using spr311_web_api.BLL.Dtos.Account;

namespace spr311_web_api.BLL.Validators.Account
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Вкажіть пошту")
                .EmailAddress().WithMessage("Невірний формат пошти");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Вкажіть ім'я користувача")
                .MinimumLength(4).WithMessage("Ім'я повинне містити мінімум 4 символи");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль не може бути порожнім")
                .MinimumLength(6).WithMessage("Мінімальа довжина паролю 6 символів");
        }
    }
}
