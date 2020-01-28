using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var context = new PlutoContext();

            // ********************************** LINQ Syntax ************************************ //
            // ********************************** LINQ Syntax ************************************ //
            // ********************************** LINQ Syntax ************************************ //
            // ********************************** LINQ Syntax ************************************ //
            // ********************************** LINQ Syntax ************************************ //

            // Projection
            var queryP =
                from c in context.Courses
                where c.AuthorId == 1
                orderby c.Level descending, c.Name
                select new { Name = c.Name, Author = c.Author.Name };

            // Grouping
            var queryG =
                from c in context.Courses
                group c by c.Level
                into g
                select g;

            //foreach (var group in queryG)
            //    System.Console.WriteLine("Group Key(Level): {0} - Group Count: {1}", group.Key,group.Count());

            // Joining

            // Inner Join - when there is a navigation property or relationship between two entities
            var queryIJ =
                from c in context.Courses
                select new { CourseName = c.Name, AuthorName = c.Author.Name };

            //foreach (var course in queryIJ)
            //    System.Console.WriteLine("Course Name: {0}, Author Name: {1}", course.CourseName, course.AuthorName);

            // Inner Join - when there is NO a navigation property or relationship between two entities
            var queryIJ2 =
                from c in context.Courses
                join a in context.Authors on c.AuthorId equals a.Id
                select new { CourseName = c.Name, AuthorName = a.Name };

            // Group Join
            var queryGJ =
                from a in context.Authors
                join c in context.Courses on a.Id equals c.AuthorId into g
                select new { AuthorName = a.Name, CoursesQuantity = g.Count() };

            //foreach (var group in queryGJ)
            //    System.Console.WriteLine("Author Name: {0}, Courses Quantity: {1}", group.AuthorName, group.CoursesQuantity);

            // Cross Join
            var queryCJ =
                from a in context.Authors
                from c in context.Courses
                select new { AuthorName = a.Name, CourseName = c.Name };

            //foreach (var course in queryCJ)
            //    System.Console.WriteLine("Author Name: {0}, Course Name: {1}", course.AuthorName, course.CourseName);

            // **************************** LINQ Extension Methods ******************************* //
            // **************************** LINQ Extension Methods ******************************* //
            // **************************** LINQ Extension Methods ******************************* //
            // **************************** LINQ Extension Methods ******************************* //
            // **************************** LINQ Extension Methods ******************************* //

            // Projection
            var courses = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenBy(c => c.Level)
                //.Select(c => new { CourseName = c.Name, AuthorName = c.Author.Name });
                .Select(c => c.Tags);
            //.Distinct();

            //foreach (var course in courses)
            //{
            //    foreach (var tag in course)
            //        System.Console.WriteLine(tag.Name);
            //}

            // Projection
            var tags = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenBy(c => c.Level)
                .SelectMany(c => c.Tags)
                .Distinct(); // Set Operators

            //foreach (var tag in tags)
            //    System.Console.WriteLine(tag.Name);

            // Grouping
            var groups = context.Courses.GroupBy(c => c.Level);

            //foreach (var group in groups)
            //{
            //    System.Console.WriteLine("Level (key): " + group.Key);

            //    foreach (var course in group)
            //        System.Console.WriteLine("\t" + course.Name);
            //}

            // Inner Join - when there is NO a navigation property or relationship between two entities
            context.Courses.Join(context.Authors,
                c => c.AuthorId,
                a => a.Id,
                (course, author) => new
                {
                    CourseName = course.Name,
                    AuthorName = author.Name
                });

            // Group Join
            context.Authors.GroupJoin(context.Courses,
                a => a.Id,
                c => c.AuthorId,
                (author, courses2) => new
                {
                    //AuthorName = author.Name,
                    //Courses = courses2
                    Author = author,
                    Courses = courses2.Count()
                });

            // Cross Join
            context.Authors.SelectMany(a => context.Courses, (author, course) => new
            {
                AuthorName = author.Name,
                CourseName = course.Name
            });

            //***************** LINQ Extension Methods: Additional Methods  **********************//
            //***************** LINQ Extension Methods: Additional Methods  **********************//
            //***************** LINQ Extension Methods: Additional Methods  **********************//
            //***************** LINQ Extension Methods: Additional Methods  **********************//
            //***************** LINQ Extension Methods: Additional Methods  **********************//

            // Partitioning
            var courses3 = context.Courses.Skip(10).Take(10);

            // Element Operators
            context.Courses.OrderBy(c => c.Level).FirstOrDefault(c => c.FullPrice > 100);
            context.Courses.OrderBy(c => c.Level).LastOrDefault(c => c.FullPrice > 100);
            context.Courses.OrderBy(c => c.Level).SingleOrDefault(c => c.FullPrice > 100);

            var allAbove10Dolars = context.Courses.All(c => c.FullPrice > 10); // return a boolean
            var anyLevel1 = context.Courses.Any(c => c.Level == 1); // return a boolean

            // Agragating
            var count = context.Courses.Count();
            var count2 = context.Courses.Where(c => c.Level == 1).Count(); // you can use the predicated inside the Count() also.

            context.Courses.Max(c => c.FullPrice);
            context.Courses.Min(c => c.FullPrice);
            context.Courses.Average(c => c.FullPrice);

            //***************** Deferred Execution  **********************//
            //***************** Deferred Execution  **********************//
            //***************** Deferred Execution  **********************//
            //***************** Deferred Execution  **********************//
            //***************** Deferred Execution  **********************//

            // none of these queries are executed against the database yet. The query is just extended:
            var courses4 = context.Courses;
            var filtered = courses4.Where(c => c.Level == 1);
            var sorted = filtered.OrderBy(c => c.Name);

            // For optimized solution, DO NOT USE:
            var courseNOT = context.Courses.ToList().Where(c => c.IsBeginnerCourse == true);

            foreach (var course in courses4)
                System.Console.WriteLine(course.Name);

            //***************** IQueryable Explained **********************//
            //***************** IQueryable Explained **********************//
            //***************** IQueryable Explained **********************//
            //***************** IQueryable Explained **********************//
            //***************** IQueryable Explained **********************//
            IQueryable<Course> courses5 = context.Courses;    // The query can be extended:
            var filtered2 = courses5.Where(c => c.Level == 1); // Level equal to 1 is part of the query

            IEnumerable<Course> courses6 = context.Courses;   // Retrieve all courses, load in memory
            var filtered3 = courses6.Where(c => c.Level == 1); // then, apply the filter (you cannot extend the query)

            foreach (var course in filtered)
                System.Console.WriteLine(course.Name);

            System.Console.ReadLine();
        }
    }
}

//// LINQ syntax
//var query =
//    from c in context.Courses
//    where c.Name.Contains("C#")
//    orderby c.Name
//    select c;

//foreach (var course in query)
//    System.Console.WriteLine(course.Name);

//// Extension methods
//var courses = context.Courses
//    .Where(c => c.Name.Contains("C#"))
//    .OrderBy(c => c.Name);

//foreach (var course in courses)
//    System.Console.WriteLine(course.Name);