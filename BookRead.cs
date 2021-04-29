using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S00199895_Mark_Curran_Book_App
{
	//The BookRead class implements Book but has extra properties Rating and DateRead
	public class BookRead : Book
	{
		//props
		public DateTime DateRead { get; set; }
		public byte Rating { get; set; }

		//inherits base class Book
		public BookRead(string title, string author) : base(title, author)
		{
		}
		public BookRead(string title, string author, byte rating)
		{
			Title = title;
			Author = author;
			Rating = rating;
			DateRead = DateTime.Now;
		}
		public BookRead()
		{
			DateRead = DateTime.Now;
		}
	}
}