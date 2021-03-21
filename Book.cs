namespace S00199895_Mark_Curran_Book_App
{
    using System;
    using System.Collections.Generic;

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book(string t, string author)
        {
            Title = t;
            Author = author;
        }

        public Book()
        {

        }
    }
}