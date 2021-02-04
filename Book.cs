using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S00199895_Mark_Curran_Book_App
{
	public class Book
	{
		//props
		public string Title { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }
		public int Rating { get; set; }
		public string ImageURI { get; set; }

		//ctor
		public Book(string title, string author, int rating)
		{
			Title = title;
			Author = author;
			Rating = rating;
		}
		public Book(string title, string author)
		{
			Title = title;
			Author = author;
		}
	}

}
