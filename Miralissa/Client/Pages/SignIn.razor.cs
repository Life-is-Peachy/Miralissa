using Microsoft.AspNetCore.Components;
using Miralissa.Client.Classes;
using Miralissa.Client.Services;
using Miralissa.Shared;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Miralissa.Client.Pages
{
	public partial class SignIn : ComponentBase
	{
		protected override async Task OnInitializedAsync()
		{
			PresetComponent();
			await base.OnInitializedAsync();
		}

		private SignInModel _signInModel;
		private SignInValidator _validator;

		[Inject] public ISnackbar Snackbar { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public IdentityAuthenticationStateProvider IdentityService { get; set; }

		[Parameter]
		public MudForm Form { get; set; }

		public bool PasswordVisibility { get; private set; }
		public InputType PasswordInput { get; private set; }
		public string PasswordInputIcon { get; private set; }

		protected async Task Submit()
		{
			await Form.Validate();
			if (!Form.IsValid)
			{
				Snackbar!.Add("Не все поля заполнены корректно");
				return;
			}

			try
			{
				await IdentityService.SignInUserAsync(_signInModel);
				NavigationManager.NavigateTo("/", true);
			}
			catch (Exception ex)
			{
				Snackbar.Add(ex.Message);
			}
		}

		protected void TogglePasswordVisibility()
		{
			if (PasswordVisibility)
			{
				PasswordVisibility = false;
				PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
				PasswordInput = InputType.Password;
			}
			else
			{
				PasswordVisibility = true;
				PasswordInputIcon = Icons.Material.Filled.Visibility;
				PasswordInput = InputType.Text;
			}
		}
		private void PresetComponent()
		{
			_signInModel = new();
			_validator = new();

			PasswordVisibility = true;
			PasswordInput = InputType.Text;
			PasswordInputIcon = Icons.Material.Filled.Visibility;
		}
	}
}
