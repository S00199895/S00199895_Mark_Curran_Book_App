using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using System;

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

		private void GetBookInfo(string query)
		{
			var apiURL = "https://www.googleapis.com/books/v1/volumes?q=" + query;

			using (WebClient wc = new WebClient())
			{
				var jsondata = wc.DownloadString(apiURL);
				JObject jsonobject = JObject.Parse(jsondata.ToString());

				var obj = JsonConvert.DeserializeObject<Root>(jsondata);

				var results = obj.items;

				
				
				DisplayBookInfo(results);
			}
		}

		private void DisplayBookInfo(List<Item> info)
		{
			var item = info[0];
				img_book.Source = new BitmapImage(new Uri(item.volumeInfo.imageLinks.smallThumbnail));
				tblk_description.Text = item.volumeInfo.description;
				tbx_title.Text = item.volumeInfo.title;
				tbx_author.Text = item.volumeInfo.authors[0];
			
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

		private void searchbar_click(object sender, RoutedEventArgs e)
		{
			if (searchbar.Text != null)
			{
				string query = searchbar.Text;
				GetBookInfo(query);
			}
		}
	}
}