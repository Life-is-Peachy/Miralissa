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
	public partial class Review : ComponentBase
	{
		private ICollection<OrderedSubscript> _subscripts;

		[Parameter] public MudTable<OrderedSubscript> Table { get; set; }
		[Parameter] public TableGroupDefinition<OrderedSubscript> GroupDefenition { get; set; }

		[Inject] public ISnackbar Snackbar { get; private set; }
		[Inject] public IDialogService DialogService { get; private set; }
		[Inject] public IOrderedService SubscriptService { get; private set; }
		[Inject] public NavigationManager NavigationManager { get; private set; }

		public Func<OrderedSubscript, bool> SearchFunction { get; private set; }
		public Func<OrderedSubscript, object> SortFunction { get; private set; }
		public string SearchString { get; private set; }

		protected override async Task OnInitializedAsync()
		{
			await PresetComponentAsync();
			await base.OnInitializedAsync();
		}

		private async Task MarkAsync(IEnumerable<OrderedSubscript> subscripts)
		{
			var selected = _subscripts.Where(x => x.CadastralNumber == subscripts.First().CadastralNumber)
				.FirstOrDefault(y => y.IsChecked);

			if (selected.IsNull())
			{
				Snackbar.NotifyInfo("Выберите запись");
				return;
			}

			var success = await SubscriptService.TryActionNotifiableAsync(async x => await x.MarkAsync(selected.ID), Snackbar, "Готово!");
			if (success) RemoveCadastral(selected.CadastralNumber);
		}

		private bool Search(OrderedSubscript subscript)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(SearchString))
					return true;

				if (subscript.CadastralNumber.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
					return true;

				if ($"{subscript.ID}".Contains(SearchString))
					return true;

				return false;
			}
			catch
			{
				return false;
			}
		}

		private async Task PresetComponentAsync()
		{
			GroupDefenition = new()
			{
				GroupName = "Кадастровый",
				Indentation = false,
				Expandable = true,
				IsInitiallyExpanded = false,
				Selector = e => $"{e.CadastralNumber}"
			};

			_subscripts = await SubscriptService.GetAsync();
			SearchFunction = new Func<OrderedSubscript, bool>(Search);
			SortFunction = new Func<OrderedSubscript, object>(x => x.Result);
		}

		private static Color ColorizeResult(string result)
			=> result switch
			{
				"Получены сведения о собственнике" => Color.Info,
				"Отсутствуют сведения о собственнике" => Color.Error,
				"Собственник ДИЗО" => Color.Warning,
				"Собственник ЮЛ" => Color.Secondary,
				_ => Color.Primary,
			};

		private void ChangeSelection(OrderedSubscript subscript)
		{
			_subscripts.Where(x => x.CadastralNumber == subscript.CadastralNumber)
				.ForEach(e => e.IsChecked = false);
			_subscripts.First(x => x.ID == subscript.ID).IsChecked = true;
		}

		private async Task ShowHtml(string html)
		{
			var parameter = new DialogParameters { ["Html"] = html };
			var dialog = DialogService.Show<HtmlViewer>("Выписка", parameter);
			await dialog.Result;
		}

		private async Task DeleteAsync(OrderedSubscript subscript)
		{
			var success = await SubscriptService.TryActionNotifiableAsync(async x => await x.DeleteAsync(subscript), Snackbar, "Выписка удалена");
			if (success) RemoveCadastral(subscript.CadastralNumber);
		}

		private async Task GetHtmlAsync(int ID)
		{
			string html = await SubscriptService.GetHtml(ID);
			await ShowHtml(html);
		}

		private void RemoveCadastral(string cadnum)
			=> _subscripts.RemoveWhere(x => x.CadastralNumber == cadnum);
	}
}