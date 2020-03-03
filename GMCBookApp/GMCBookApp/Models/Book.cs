using System;
using SQLite;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 

namespace GMCBookApp.Models
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string WriterName { get; set; }
        public string BookName { get; set; }
        public int YearPublished { get; set; }
        public int Price { get; set; }
        public byte[] PDF { get; set; }

        public Book()
        {

        }

        public Book(Book other)
        {
            this.ID = other.ID;
            this.WriterName = other.WriterName;
            this.BookName = other.BookName;
            this.YearPublished = other.YearPublished;
            this.Price = other.Price;
            this.PDF = other.PDF;
        }
    }


}
