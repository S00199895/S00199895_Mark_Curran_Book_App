using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestSharp;
using System.Net;
using RestSharp.Serialization.Json;

namespace S00199895_Mark_Curran_Book_App
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		List<BookRead> booksRead = new List<BookRead>();
		List<Book> booksTBR = new List<Book>();

		//when the window loads
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//Create new books
			BookRead b1 = new BookRead("The Stand", "Stephen King", 8);
			BookRead b2 = new BookRead("Blood of Elves", "Andrzej Sapkowski", 8);
			BookRead b3 = new BookRead("Neuromancer", "William Gibson", 6);
			BookRead b4 = new BookRead("Jade City", "Fonda Lee", 9);

			Book tbr1 = new Book("Jade War", "Fonda Lee");
			Book tbr2 = new Book("It", "Stephen King");
			Book tbr3 = new Book("Baptism of Fire", "Andrzej Sapkowski");

			//Add books to list
			booksRead.Add(b1);
			booksRead.Add(b2);
			booksRead.Add(b3);
			booksRead.Add(b4);

			booksTBR.Add(tbr1);
			booksTBR.Add(tbr2);
			booksTBR.Add(tbr3);

			dataGrid_read.ItemsSource = booksRead;
			dataGrid_tbr.ItemsSource = booksTBR;

			GetBookInfo();
		}

		private void GetBookInfo()
		{
			//REST Google Books API call
			
			var client = new RestClient("https://www.googleapis.com/books/v1/volumes?q=the+stand");
			var request = new RestRequest( Method.GET);
			var deserial = new JsonDeserializer();
			var response = client.Execute(request);

			var content = response.Content;

			var output = deserial.Deserialize<Dictionary<string, string>>(response);

			//object? 
			//need to reference object output[0].volumeInfo

			//tblk_description.Text = output.volumeInfo;
		}
	}
}