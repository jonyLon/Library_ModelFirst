using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Library_ModelFirst
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (var context = new LibraryModelContainer())
            {
                //var usa = new Country { CountryName = "United States" };
                //var ukraine = new Country { CountryName = "Ukraine" };

                //var english = new Language { LangName = "English", Country = usa };
                //var ukrainian = new Language { LangName = "Ukrainian", Country = ukraine };
                //var americanEnglish = new Language { LangName = "American English", Country = usa };

                //var shakespeare = new Author { AuthorName = "William", Surname = "Shakespeare", Age = 50 };
                //var tolkien = new Author { AuthorName = "J.R.R.", Surname = "Tolkien", Age = 81 };

                //var hamlet = new Book { BookName = "Hamlet", PagesCount = 123, Author = shakespeare, Language = english };
                //var lotr = new Book { BookName = "The Lord of the Rings", PagesCount = 1178, Author = tolkien, Language = english };
                //var shortBook = new Book { BookName = "Short", PagesCount = 95, Author = tolkien, Language = ukrainian };
                //var bookA = new Book { BookName = "A Fantasy", PagesCount = 200, Author = tolkien, Language = americanEnglish };


                //var americanBook = new Book { BookName = "American Novel", PagesCount = 300, Author = tolkien, Language = americanEnglish };

                //var rowling = new Author { AuthorName = "J.K.", Surname = "Rowling", Age = 55 };

                //var ukrainianBook = new Book { BookName = "Ukrainian Story", PagesCount = 150, Author = tolkien, Language = ukrainian };

                //context.Countries.Add(usa);
                //context.Countries.Add(ukraine);

                //context.Languages.Add(english);
                //context.Languages.Add(ukrainian);
                //context.Languages.Add(americanEnglish);

                //context.Authors.Add(shakespeare);
                //context.Authors.Add(tolkien);

                //context.Books.Add(hamlet);
                //context.Books.Add(lotr);
                //context.Books.Add(shortBook);
                //context.Books.Add(bookA);

                //context.Books.Add(americanBook);
                //context.Authors.Add(rowling);
                //context.Books.Add(ukrainianBook);

                //context.SaveChanges();

                void PrintBooks(IQueryable<Book> books)
                {
                    Console.WriteLine($"\tId{null,7} BookName{null,7} AuthorId{null,10} Author{null,10} LanguageId {null,7}Language{null,10} PagesCount");
                    foreach (var b in books)
                    {
                        Console.WriteLine($"\t{b.Id,-2} {b.BookName,-25} {b.AuthorId,-8} {b.Author.AuthorName,-10} {b.Author.Surname,-15} {b.LanguageId,-10} {b.Language.LangName,-25} {b.PagesCount,-10}");
                    }
                    Console.WriteLine();
                }
                void PrintBook(Book b)
                {
                    Console.WriteLine($"\tId{null,7} BookName{null,7} AuthorId{null,10} Author{null,10} LanguageId {null,7}Language{null,10} PagesCount");
                    Console.WriteLine($"\t{b.Id,-2} {b.BookName,-25} {b.AuthorId,-8} {b.Author.AuthorName,-10} {b.Author.Surname,-15} {b.LanguageId,-10} {b.Language.LangName,-25} {b.PagesCount,-10}");
                    Console.WriteLine();
                }

                var bigbooks = context.Books.Where(b => b.PagesCount > 100);
                Console.WriteLine("Books with more than 100 pages:");
                PrintBooks(bigbooks);
                var abooks = context.Books.Where(b => b.BookName.StartsWith("A"));
                Console.WriteLine("Books starts with 'A':");
                PrintBooks(abooks);
                var WSbooks = context.Books.Where(b => b.Author.AuthorName == "William" && b.Author.Surname == "Shakespeare");
                Console.WriteLine("Books writen by William Shakespeare:");
                PrintBooks(WSbooks);
                Console.WriteLine("Books writen in Ukrainian:");
                var ukrbooks = context.Books.Where(b => b.Language.LangName == "Ukrainian");
                PrintBooks(ukrbooks);
                var shortNameBooks = context.Books.Where(b => b.BookName.Length<10);
                Console.WriteLine("Books with short names:");
                PrintBooks(shortNameBooks);
                var bigestAmericanBook = context.Books.Where(b=> b.Language.LangName == "American English").OrderByDescending(b => b.PagesCount).First();
                Console.WriteLine("The Bigest American Book:");
                PrintBook(bigestAmericanBook);

                var AuthorWithFewestBooks = context.Authors.OrderBy(a => a.Book.Count()).First();
                Console.WriteLine("Author With The Fewest Books: ");
                Console.WriteLine($"Id: {AuthorWithFewestBooks.Id}  FullName: {AuthorWithFewestBooks.AuthorName + " " + AuthorWithFewestBooks.Surname}  Age: {AuthorWithFewestBooks.Age}  BooksCount: {AuthorWithFewestBooks.Book.Count()}");
                Console.WriteLine();

                var notAmericanAuthors = context.Authors.Where(a => a.Book.All(b => b.Language.LangName != "American English")).OrderBy(a => a.AuthorName).Select(a => a.AuthorName + " " + a.Surname);
                Console.WriteLine("Not American Authors: ");
                foreach (var name in notAmericanAuthors)
                {
                    Console.WriteLine(name);
                }

                var CountryWithBigestNumberOfBooks = context.Countries.OrderByDescending(c=> c.Language.Sum(l => l.Book.Count())).FirstOrDefault();
                Console.WriteLine("Country With The Bigest Number Of Books: ");
                Console.WriteLine(CountryWithBigestNumberOfBooks.CountryName);

                var MultiLanguagesAuthors = context.Authors.Where(a => a.Book.Select(b => b.LanguageId).Distinct().Count() > 1).Select(a => new { Author = a, LanguagesCount = a.Book.Select(b => b.LanguageId).Distinct().Count()}).OrderByDescending(i => i.LanguagesCount);
                Console.WriteLine("Multi Languages Authors: ");
                foreach (var item in MultiLanguagesAuthors)
                {
                    Console.WriteLine($"Id: {item.Author.Id}  FullName: {item.Author.AuthorName + " " + item.Author.Surname}  Age: {item.Author.Age}  LanguagesCount: {item.LanguagesCount}");
                }
            }

        }
    }
}
