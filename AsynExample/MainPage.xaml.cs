using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace AsynExample
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            progress.IsActive = true;
            List<string> list = await MyFunctions.ReadJson("file.json");
            lista.ItemsSource = MyFunctions.Order(list);
            progress.IsActive = false;
            base.OnNavigatedTo(e);
        }
    }
    public class MyFunctions
    {
        public async static Task<List<string>> ReadJson(String filename)
        {
            await Task.Factory.StartNew(() => {
                // Simulamos que es un archivo con muchos registros
                for (int i = 0; i < 1000000; i++)
                {
                    Debug.WriteLine(i);
                }
            });
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = await folder.GetFileAsync(filename);
            var text = await Windows.Storage.FileIO.ReadTextAsync(file);
            return await Task<List<string>>.Factory.StartNew(() => {
                return JsonConvert.DeserializeObject<List<string>>(text);
            });
        }
        public static List<string> Order(List<string> list)
        {
            return (from x in list orderby x.Length select x).ToList();
        }
    }
}
