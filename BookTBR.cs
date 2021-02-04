using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S00199895_Mark_Curran_Book_App
{
	public class BookTBR : Book
	{
		//props
		public DateTime DateRead { get; set; }

		//inherits base class Book
		public BookTBR(string title, string author) : base(title, author)
		{

		}
	}
}