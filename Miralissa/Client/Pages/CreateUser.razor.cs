using Microsoft.AspNetCore.Components;
using Miralissa.Client.Classes;
using Miralissa.Client.Services;
using Miralissa.Shared;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Miralissa.Client.Pages
{
	public partial class CreateUser : ComponentBase
	{
		protected override async Task OnInitializedAsync()
		{
			PresetComponent();
			await base.OnInitializedAsync();
		}


		private CreateUserModel _CreateUserModel;
		private CreateUserValidator _Validator;


		[Inject] public ISnackbar Snackbar { get; private set; }
		[Inject] public NavigationManager NavigationManager { get; private set; }
		[Inject] public IdentityAuthenticationStateProvider IdentityService { get; private set; }


		[Parameter]
		public MudForm Form { get; set; }


		public bool PasswordVisibility { get; set; }
		public InputType PasswordInput { get; set; }
		public string PasswordInputIcon { get; set; }



		private async Task Submit()
		{
			await Form.Validate();
			if (!Form.IsValid)
			{
				Snackbar!.Add("Не все поля заполнены корректно");
				return;
			}

			try
			{
				await IdentityService.CreateUserAsync(_CreateUserModel);
				NavigationManager.NavigateTo("/", true);
			}
			catch (Exception ex)
			{
				Snackbar.Add(ex.Message);
			}
		}


		private void TogglePasswordVisibility()
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
			_CreateUserModel = new();
			_Validator = new();
			_CreateUserModel.Credentials = new();
			_CreateUserModel.Claims = new();

			PasswordVisibility = true;
			PasswordInput = InputType.Text;
			PasswordInputIcon = Icons.Material.Filled.Visibility;
		}
	}
}
