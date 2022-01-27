using FluentValidation;
using Miralissa.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miralissa.Client.Classes
{
	public class SignInValidator : AbstractValidator<SignInModel>
	{
		public SignInValidator()
		{
			RuleSet("SignInRules", () =>
			{
				RuleFor(p => p.Login)
				.NotEmpty().WithMessage("Поле не может быть пустым");

				RuleFor(p => p.Password)
				.NotEmpty().WithMessage("Поле не может быть пустым");
			});
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue()
		{
			return async (model, property) =>
			{
				var result = await ValidateAsync(ValidationContext<SignInModel>.CreateWithOptions((SignInModel)model, x => x.IncludeProperties(property)));
				return result.IsValid ? Enumerable.Empty<string>() : result.Errors.Select(x => x.ErrorMessage);
			};
		}
	}

	public class CreateUserValidator : AbstractValidator<CreateUserModel>
	{
		public CreateUserValidator()
		{
			RuleSet("CreateUserRules", () =>
			{
				RuleFor(p => p.Credentials.Login)
				.NotEmpty().WithMessage("Логин обязателен")
				.MinimumLength(5).WithMessage("Минимальная длина 5 символом")
				.MaximumLength(20).WithMessage("Предельная длина 20 символов");

				RuleFor(p => p.Credentials.Email)
				.NotEmpty().WithMessage("E-mail обязателен")
				.EmailAddress().WithMessage("Не корректный формат E-mail адресса");

				RuleFor(p => p.Credentials.Password)
				.NotEmpty().WithMessage("Пароль обязателен")
				.MinimumLength(5).WithMessage("Пароль должен содержать не менее 5 символов включая хотя бы одну цифру")
				.Must(x => x.Any(char.IsDigit)).WithMessage("Пароль должен содержать хотя бы одну цифру");

				RuleFor(p => p.Credentials.WorkPhone)
				.NotEmpty().WithMessage("Телефон обязателен")
				.MinimumLength(4).WithMessage("Минимальная длина 4 цифры")
				.MaximumLength(5).WithMessage("Максимальная длина 5 цифр");

				RuleFor(p => p.Claims.Forename)
				.NotEmpty().WithMessage("Имя обязательно")
				.MinimumLength(2).WithMessage("Имя не может содержать меньше 2-х символов");

				RuleFor(p => p.Claims.Surname)
				.NotEmpty().WithMessage("Фамилия обязательна")
				.MinimumLength(2).WithMessage("Фамилия не может содержать меньше 2-х символов");

				RuleFor(p => p.Claims.DateOfBirth)
				.NotEmpty().WithMessage("Укажите свою дату рождения")
				.GreaterThanOrEqualTo(new DateTime(1945, 12, 31)).WithMessage("Пора на пенсию");

				RuleFor(p => p.Claims.Gender.ToString())
				.IsEnumName(typeof(Gender)).WithMessage("Укажите пол");
			});
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue()
		{
			return async (model, property) =>
			{
				var result = await ValidateAsync(ValidationContext<CreateUserModel>.CreateWithOptions((CreateUserModel)model, x => x.IncludeProperties(property)));
				return result.IsValid ? Enumerable.Empty<string>() : result.Errors.Select(x => x.ErrorMessage);
			};
		}
	}
}
