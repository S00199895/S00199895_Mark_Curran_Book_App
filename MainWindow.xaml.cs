using System.Collections.Generic;
using System.Windows;
using RestSharp;
using Newtonsoft;
using RestSharp.Serialization.Json;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;

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

		private void Update()
		{
			dataGrid_read.ItemsSource = null;
			dataGrid_tbr.ItemsSource = null;
			dataGrid_read.ItemsSource = booksRead;
			dataGrid_tbr.ItemsSource = booksTBR;
		}

		//when the window loads
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			combo_rating.ItemsSource = FillComboBox();

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
		//Adds 1-10 to the comboBox
		private byte[] FillComboBox()
		{
			byte[] ratings = new byte[10];
			byte z = 0;

			for (byte i = 1; i <= 10; i++)
			{
				
				ratings[z] = i;
				z++;
			}

			return ratings;
		}

		private void GetBookInfo()
		{
			var apiURL = "https://www.googleapis.com/books/v1/volumes?q=the+stand";

			using (WebClient wc = new WebClient())
			{
				var jsondata = wc.DownloadString(apiURL);
				JObject jsonobject = JObject.Parse(jsondata.ToString());

				var obj = JsonConvert.DeserializeObject<Root>(jsondata);

				var results = obj.items;

				foreach (var item in results)
				{
					MessageBox.Show(item.volumeInfo.title);
					MessageBox.Show(item.volumeInfo.description);
				}
			}
			//REST Google Books API call
			/*
			var client = new RestClient("https://www.googleapis.com/books/v1/volumes?q=the+stand");
			var request = new RestRequest(Method.GET);
			var response = client.Execute(request);

			var content = response.Content;

			BookRESTAPI bookInfo = JsonConvert.DeserializeObject<BookRESTAPI>(content);
			//string name = volume.title;*/

		}
		//Add a new book from the bottom left UI
		private void btn_addBook_Click(object sender, RoutedEventArgs e)
		{
			//If it's read
			if (radio_read.IsChecked == true)
			{
				BookRead bookRead = new BookRead();
				bookRead.Title = tbx_title.Text;
				bookRead.Author = tbx_author.Text;
				bookRead.Rating = (byte)combo_rating.SelectedItem;
				booksRead.Add(bookRead);
				
			}
			//If it's not read
			else if (radio_tbr.IsChecked == true)
			{
				Book book = new Book();
				book.Title = tbx_title.Text;
				book.Author = tbx_author.Text;
				booksTBR.Add(book);
			}
			//Clear the text boxes
			tbx_title.Clear();
			tbx_author.Clear();
			combo_rating.SelectedIndex = 0;
			Update();	//Resets the dataGrid ItemSource
		}
	}
}