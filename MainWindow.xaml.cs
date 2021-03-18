using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
			Update();   //Resets the dataGrid ItemSource
		}

		private void searchbar_click(object sender, RoutedEventArgs e)
		{
			tblk_description.FontStyle = FontStyles.Normal;
			tblk_description.Foreground = Brushes.Black;
			if (searchbar.Text != null)
			{
				string query = searchbar.Text;
				GetBookInfo(query);
			}
		}

		private void btn_save_bookInfo_Click(object sender, RoutedEventArgs e)
		{
			StreamWriter sW = new StreamWriter("bookInfo.txt");

			//if (GetBookInfo())

			sW.WriteLine($"{tbx_title.Text}, By {tbx_author.Text}\n\n");
			sW.WriteLine(tblk_description.Text);
			sW.Flush();
			sW.Close();
			MessageBox.Show("Book info saved to file");
		}
		private void btn_save_books_Click(object sender, RoutedEventArgs e)
		{
			foreach (Book b in booksRead)
			{
				StreamWriter sW = new StreamWriter("booksList.txt");

				sW.WriteLine($"{b.Title}, by {b.Author}");
			}
		}

		//Change the placeholder text set in the xaml back
		private void searchbar_GotFocus(object sender, RoutedEventArgs e)
		{
			searchbar.Text = "";
			searchbar.Foreground = Brushes.Black;
			searchbar.FontStyle = FontStyles.Normal;

		}

		private void dataGrid_read_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			BookRead bR = dataGrid_read.SelectedItem as BookRead;

			GetBookInfo(bR.Title + " " + bR.Author);
		}

		private void dataGrid_tbr_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			Book book = dataGrid_tbr.SelectedItem as Book;

			if (book != null)
			{
				GetBookInfo(book.Title + " " + book.Author);
			}
			
		}

		private void tbx_title_GotFocus(object sender, RoutedEventArgs e)
		{
			tbx_title.Clear();
			tbx_author.Clear();
		}
	}
}