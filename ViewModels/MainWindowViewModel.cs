using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Themes.Fluent;
using ReactiveUI;

namespace Notebook.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _data = "";
        private string? _openedFile = null;
        private FluentThemeMode _mode;

        public string? OpenedFile
        {
            get => _openedFile??"Untitled";
            set => this.RaiseAndSetIfChanged(ref _openedFile, value);
        }
        
        public FluentThemeMode AppMode { 
            get => _mode;
            set => this.RaiseAndSetIfChanged(ref _mode, value); }

        public string Data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }
        
        public async Task Open()
        {
            var dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() {Name = "Text", Extensions = {"txt"}});
            var result = await dialog.ShowAsync(new Window());
            if (result != null)
            {
                Data = await File.ReadAllTextAsync(result.First());
                OpenedFile = result.First();
            }
        }
        
        public async Task SaveAs()
        {
            var dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter() 
         
                {Name = "Text", Extensions = {"txt"}});
            var result = await dialog.ShowAsync(new Window());
            if (result != null)
            {
                await File.WriteAllTextAsync(result, Data);
                OpenedFile = result;
            }
        }
        
        public async Task Save()
        {
            if (_openedFile == null) await SaveAs();
            else
            {
                await File.WriteAllTextAsync(OpenedFile, Data);
            }
        }
        
        public async Task ChangeTheme()
        {
            AppMode = AppMode == FluentThemeMode.Light ? FluentThemeMode.Dark : FluentThemeMode.Light;
        }
        
        public async Task NewFile()
        {
            if (_openedFile!=null)
            {
                await Save();
            }
            OpenedFile = null;
            Data = "";
        }
    }
}