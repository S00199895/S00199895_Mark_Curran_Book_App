using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;

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
		//initialize lists for both data grids
		List<BookRead> booksRead = new List<BookRead>();
		List<Book> booksTBR = new List<Book>();

		private void Update()
		{
			//updates the data grids when called
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

		public void GetBookInfo(string query)
		{
			//retrieving a book
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
			//Adds the selected book's details to the text boxes and
			//text blocks for easier adding to the lists easier
			try
			{
				var item = info[0];
				img_book.Source = new BitmapImage(new Uri(item.volumeInfo.imageLinks.smallThumbnail));
				tblk_description.Text = item.volumeInfo.description;
				tbx_title.Text = item.volumeInfo.title;
				tbx_author.Text = item.volumeInfo.authors[0];
			}
			catch (Exception e)
			{
				MessageBox.Show("An error occurred displaying the book's info:\n", e.Message);
				throw;

			}


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
			//resets the placeholder styling in the searchbar
			ResetDescriptionStyles();
			if (searchbar.Text != null)
			{
				string query = searchbar.Text;
				GetBookInfo(query);
			}
		}

		private void btn_save_bookInfo_Click(object sender, RoutedEventArgs e)
		{
			//write a single book's details returned from the API to JSON
			Book tempBook = new Book();
			tempBook.Title = tbx_title.Text;
			tempBook.Author = tbx_author.Text;
			tempBook.Description = tblk_description.Text;

			string data = JsonConvert.SerializeObject(tempBook, Formatting.Indented);
			StreamWriter sW = new StreamWriter("bookInfo.json");

			sW.Write(data);
			sW.Flush();
			sW.Close();


			MessageBox.Show("Wrote to JSON");
		}

		private void btn_save_books_Click(object sender, RoutedEventArgs e)
		{
			//saves a json array of books
			List<Book> tempBooks = new List<Book>();
			StreamWriter sW = new StreamWriter("booksList.json");

			foreach (Book b in booksRead)
			{
				tempBooks.Add(b);
			}

			string data = JsonConvert.SerializeObject(tempBooks, Formatting.Indented);
			sW.Write(data);

			sW.Flush();
			sW.Close();
			Insert();
		}

		//Change the placeholder text set in the xaml black
		private void searchbar_GotFocus(object sender, RoutedEventArgs e)
		{
			searchbar.Text = "";
			searchbar.Foreground = Brushes.Black;
			searchbar.FontStyle = FontStyles.Normal;

		}

		private void dataGrid_read_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			//select the book as an implicit type
			var bookSelected = dataGrid_read.SelectedItem;
			ResetDescriptionStyles();

			//checks the type of object the book is
			//this problem arose from making the db in a the EF Designer
			//and not code first
			//this created two similar book classes to work with
			//instead of one
			if (bookSelected is Book)
			{
				Book bR = dataGrid_read.SelectedItem as Book;

				GetBookInfo(bR.Title + " " + bR.Author);
			}
			else if (bookSelected is BookTBL)
			{
				BookTBL b = dataGrid_read.SelectedItem as BookTBL;
				GetBookInfo(b.Title + " " + b.Author);
			}
		}

		private void dataGrid_tbr_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			ResetDescriptionStyles();
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

		private void ResetDescriptionStyles()
		{
			tblk_description.FontStyle = FontStyles.Normal;
			tblk_description.Foreground = Brushes.Black;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//Load books
			Model1Container2 db = new Model1Container2();

			var query = from b in db.BookTBLs
						select b;

			dataGrid_read.ItemsSource = query.ToList();
		}

		public void Insert()
		{
			//Inserts the books from the data grid into the db
			try
			{
				Model1Container2 db = new Model1Container2();


				foreach (var book in booksRead)
				{
					BookTBL newBook = new BookTBL() { Author = $"{book.Author}", Title = $"{book.Title}", AuthorTBLId = 1 };

					db.BookTBLs.Add(newBook);
					db.SaveChanges();
				}
			}
			catch (NullReferenceException nre)
			{
				MessageBox.Show(nre.Message);
			}
			catch (Exception)
			{
				MessageBox.Show("An error inserting into the database occurred");
			}
		}
	}
}