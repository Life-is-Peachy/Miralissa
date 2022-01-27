using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Miralissa.Client.Classes;
using Miralissa.Client.Components;
using Miralissa.Client.Services;
using Miralissa.Shared;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miralissa.Client.Pages
{
	[Authorize]
	public partial class Pipeline : ComponentBase
	{
		private ICollection<PreparedSubscript> _subscripts;

		[Inject] public ISnackbar Snackbar { get; private set; }
		[Inject] public IDialogService DialogService { get; private set; }
		[Inject] public IPreparedService SubscriptService { get; private set; }
		[Inject] public NavigationManager NavigationManager { get; private set; }

		[Parameter] public MudTable<PreparedSubscript> Table { get; set; }
		[Parameter] public TableGroupDefinition<PreparedSubscript> GroupDefenition { get; set; }

		public Func<PreparedSubscript, object> SortFunction { get; private set; }
		public Func<PreparedSubscript, bool> SearchFunction { get; private set; }
		public string SearchString { get; private set; }

		protected override async Task OnInitializedAsync()
		{
			await PresetComponent();
			await base.OnInitializedAsync();
		}

		private async Task CreateAsync(PreparedSubscript subscript)
		{
			var dialog = DialogService.Show<CadastralDialog>("Новый кадастровый");
			var result = await dialog.Result;

			if (result.Cancelled) return;

			string input = result.Data.ToStringSafe();
			if (input.IsEmpty())
			{
				Snackbar.NotifyWarning("Новый кадастровый не может быть пустым");
				return;
			}

			subscript.CadastralNumber = input;

			var success = await SubscriptService.TryActionNotifiableAsync(async x => await x.CreateAsync(subscript), Snackbar, "Новая выписка помещена в очередь");
			if (success) RemoveRequest(subscript.ID_Request);
		}

		private async Task MarkAsync(IEnumerable<PreparedSubscript> subscripts)
		{
			var currentGroup = subscripts.Where(x => x.IsChecked)
				.Select(t => t.ID)
				.ToArray();

			if (currentGroup.IsEmpty())
			{
				Snackbar.NotifyInfo("Выберите хотя бы одну запись");
				return;
			}

			var success = await SubscriptService.TryActionNotifiableAsync(async x => await x.MarkAsync(currentGroup), Snackbar, "Выписка перенесена в очередь");
			if (success) RemoveRequest(subscripts.First().ID_Request);
		}

		private bool Search(PreparedSubscript subscript)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(SearchString))
					return true;

				if (subscript.CadastralNumber.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
					return true;

				if (subscript.Address.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
					return true;

				if (subscript.ID.ToString().Contains(SearchString))
					return true;

				return false;
			}
			catch
			{
				return false;
			}
		}

		private async Task PresetComponent()
		{
			GroupDefenition = new()
			{
				GroupName = "RID",
				Indentation = false,
				Expandable = true,
				IsInitiallyExpanded = false,
				Selector = e => $"{e.ID_Request}  |  Адрес: {e.Address}"
			};

			_subscripts = await SubscriptService.GetAsync();
			SearchFunction = new Func<PreparedSubscript, bool>(Search);
			SortFunction = new Func<PreparedSubscript, object>(x => x.Result);
		}

		private static Color ColorizeResult(string result)
			=> result switch
			{
				"Кадастровый номер получен" => Color.Info,
				"Адрес не найден" => Color.Error,
				"Не корректный кадастровый" => Color.Warning,
				"Анулирован" => Color.Secondary,
				_ => Color.Primary,
			};

		private async Task DeleteAsync(PreparedSubscript subscript)
		{
			var success = await SubscriptService.TryActionNotifiableAsync(async x => await x.DeleteAsync(subscript), Snackbar, "Выписка удалена");
			if (success) RemoveRequest(subscript.ID_Request);
		}

		private void Select(int ID)
			=> _subscripts.First(x => x.ID == ID).IsChecked = !_subscripts.First(x => x.ID == ID).IsChecked;

		private void RemoveRequest(int? idr)
			=> _subscripts.RemoveWhere(x => x.ID_Request == idr);
	}
}
