using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;
using WizLib_Model.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BookController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // This Include method is making the process as Eager loading
            // Eager loading also fixes the N+1 issues
            List<Book> books = _dbContext.Books
                .Include(p => p.Publisher)
                .Include(p => p.BookAuthors)
                .ThenInclude(a => a.Author)
                .ToList();
            //foreach (var book in books)
            //{
            //    // Least efficient way
            //    //book.Publisher = _dbContext.Publishers
            //    //    .FirstOrDefault(p => p.Publisher_Id == book.Publisher_Id);

            //    // explicit Loading is more efficient
            //    _dbContext.Entry(book).Reference(u => u.Publisher).Load();
            //}
            return View(books);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM bookVM = new BookVM();
            bookVM.PublisherList = _dbContext.Publishers.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Publisher_Id.ToString()
            });

            if (id == null)
                return View(bookVM);
            else
                bookVM.Book = _dbContext.Books.FirstOrDefault(c => c.Book_Id == id);
            return View(bookVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM bookVM)
        {

            if (bookVM.Book.Book_Id == 0)
            {
                // This is create
                _dbContext.Books.Add(bookVM.Book);
            }
            else
            {
                // This is update
                _dbContext.Books.Update(bookVM.Book);
            }
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Details(int? id)
        {
            BookVM bookVM = new BookVM();

            if (id == null)
                return View(bookVM);
            else
                bookVM.Book = _dbContext.Books.Include(b => b.BookDetail).FirstOrDefault(c => c.Book_Id == id);
            return View(bookVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM bookVM)
        {

            if (bookVM.Book.BookDetail.BookDetail_Id == 0)
            {
                // This is create
                _dbContext.BookDetails.Add(bookVM.Book.BookDetail);
                _dbContext.SaveChanges();

                var bookFromDb = _dbContext.Books
               .FirstOrDefault(u => u.Book_Id == bookVM.Book.Book_Id);

                // This BookDetail_Id will be automatically filled once the _dbContext.SaveChanges() will be executed
                bookFromDb.BookDetail_Id = bookVM.Book.BookDetail.BookDetail_Id;
                _dbContext.SaveChanges();
            }
            else
            {
                // This is update
                _dbContext.BookDetails.Update(bookVM.Book.BookDetail);
                _dbContext.SaveChanges();
            }

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(p => p.Book_Id == id);
            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM bookAuthorVM = new BookAuthorVM
            {
                BookAuthorList = _dbContext.BookAuthors
                .Include(a => a.Author).Include(b => b.Book)
                .Where(b => b.Book_Id == id).ToList(),

                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },

                Book = _dbContext.Books.FirstOrDefault(b => b.Book_Id == id)
            };

            List<int> tempListOfAssignedAuthors =
                bookAuthorVM.BookAuthorList.Select(a => a.Author_Id).ToList();
            // NOT IN Caluse in LINQ
            // Get all the auhtors whos id not in tempListOfAssignedAuthors
            var tempList = _dbContext.Authors
                .Where(a => !tempListOfAssignedAuthors.Contains(a.Author_Id))
                .ToList();

            // The output of above query
            // SELECT [a].[Author_Id], [a].[BirthDate], [a].[FirstName], [a].[LastName], [a].[Location]
            // FROM[Authors] AS[a]
            // WHERE[a].[Author_Id] NOT IN(1, 3)

            bookAuthorVM.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });

            return View(bookAuthorVM);

        }
        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if (bookAuthorVM.BookAuthor.Book_Id != 0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _dbContext.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _dbContext.SaveChanges();
            }

            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.Book_Id;
            BookAuthor bookAuthor = _dbContext.BookAuthors
                .FirstOrDefault(a => a.Author_Id == authorId && a.Book_Id == bookId);
            _dbContext.BookAuthors.Remove(bookAuthor);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }

        public IActionResult PlayGround()
        {
            //var bookTemp = _dbContext.Books.FirstOrDefault();
            //bookTemp.Price = 100;

            //var bookCollection = _dbContext.Books;
            //double totalPrice = 0;

            //foreach (var book in bookCollection)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookList = _dbContext.Books.ToList();
            //foreach (var book in bookList)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookCollection2 = _dbContext.Books;
            //var bookCount1 = bookCollection2.Count();
            //var bookCount2 = _dbContext.Books.Count();

            //IEnumerable<Book> bookList1 = _dbContext.Books;
            //var filteredBookI = bookList1.Where(p => p.Price > 30).ToList();

            //IQueryable<Book> bookList2 = _dbContext.Books;
            //var filteredBook2 = bookList2.Where(p => p.Price > 30).ToList();

            //var category = _dbContext.Categories.FirstOrDefault();
            //_dbContext.Entry(category).State = EntityState.Modified;
            //_dbContext.SaveChanges();

            //// Updating related data
            //var bookTemp1 = _dbContext.Books.Include(b => b.BookDetail)
            //    .FirstOrDefault(b => b.Book_Id == 4);
            //bookTemp1.BookDetail.NumberOfChapters = 1000;
            //_dbContext.Books.Update(bookTemp1);
            //_dbContext.SaveChanges();

            //var bookTemp2 = _dbContext.Books.Include(b => b.BookDetail)
            //    .FirstOrDefault(b => b.Book_Id == 4);
            //bookTemp2.BookDetail.Weight = 199;
            //_dbContext.Books.Attach(bookTemp2);
            //_dbContext.SaveChanges();

            // Views
            //var viewList = _dbContext.BookDetailsFromView.ToList();
            //var viewList2 = _dbContext.BookDetailsFromView.FirstOrDefault();
            //var viewList3 = _dbContext.BookDetailsFromView.Where(x => x.Price > 30);

            // RAW SQL
            // var booksRaw = _dbContext.Books.FromSqlRaw("SELECT * FROM books").ToList();

            // SQL Injection attack prone
            var id = 2;
            //var booksRaw1 = _dbContext.Books.FromSqlInterpolated($"SELECT * FROM books WHERE Book_Id={id}").ToList();

            // Call Store Procedure

            var booksSproc = _dbContext.Books.FromSqlInterpolated($"EXEC getAllBookDetails {id}").ToList();

            // .Net 5 Only
            var bookFilter1 = _dbContext.Books
                .Include(x => x.BookAuthors.Where(a => a.Author_Id == 1))
                .ToList();

            var bookFilter2 = _dbContext.Books
                .Include(x => x.BookAuthors.OrderByDescending(a => a.Author_Id).Take(2))
                .ToList();

            return RedirectToAction(nameof(Index));
        }
    }
}
