﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Console;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WriteLine("*************** Range Expansion *************************************");
            RangeExpansion();

            WriteLine("*************** Sort By Age *************************************");
            SortByAge();

            // WriteLine("*************** List Bishop Moves (dynamic) *************************************");
            // ListBishopMovesDynamic();
            // WriteLine("*************** List Bishop Moves (refactor) *************************************");
            // ListBishopMovesRefactor();
            WriteLine("*************** List Bishop Moves (query) *************************************");
            ListBishopMovesQuery();

            WriteLine("*************** Get Longest Book (aggregate) *************************************");
            GetLongestBookUseAggregate();
        }

        static void RangeExpansion()
        {
            // e.g. "2,3-5,7" should be 2,3,4,5,7
            // e.g. "6,1-3,2-4" should be 1,2,3,4,6
            var origin = "6,1-3,2-4,36-41";
            var result = origin
                .Split(',')
                .Select(x => x.Split('-'))
                .Select(p => new { 
                            First = int.Parse(p[0]), 
                            Last = int.Parse(p.Last())})
                .SelectMany(r => Enumerable.Range(
                   r.First, r.Last - r.First + 1))
                .OrderBy(r => r)
                .Distinct();
                
            foreach(var i in result)
            {
                WriteLine(i);
            }
        }

        static void SortByAge()
        {
            var result = "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988"
                .Split(';')
                .Select(n => n.Split(','))
                .Select(n => new { Name = n[0].Trim(), DateOfBirth = ParseDob(n[1])})
                .OrderByDescending(n => n.DateOfBirth)
                .Select(n => new { Name = n.Name, Age = GetAge(n.DateOfBirth) });
            
            foreach(var item in result)
            {
                WriteLine($"{item.Name, -20}: {item.Age}");
            }
                
        }

        static int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age;
        }

        static DateTime ParseDob(string date) => DateTime.ParseExact(date.Trim(), "d/M/yyyy", CultureInfo.InvariantCulture);
    
        static void ListBishopMovesDynamic()
        {
            // we start with a Bishop on c6
            // what positions can it reach in one move?
            // output should include b5, a4, b7, a8
            var chessBoardPositions = Enumerable.Range('a',8)
                .SelectMany(x => Enumerable.Range('1', 8),
                    (f, r) => new { File = (char)f, Rank = (char)r });

            var validMoves = chessBoardPositions
                .Where(x => BishopCanMoveTo(new { File = 'c', Rank = '6' }, x))
                .Select(x => string.Format("{0}{1}",x.File,x.Rank));
            
            foreach(var item in validMoves)
            {
                WriteLine(item);
            }
        }

        static void ListBishopMovesRefactor()
        {
            var result = GetBoardPositions()
		                    .Where(p => BishopCanMoveTo(p,"c6"));
            foreach(var item in result)
            {
                WriteLine(item);
            }
        }

        static void ListBishopMovesQuery()
        {
            var result = 
                from row in Enumerable.Range('a', 8)
                from col in Enumerable.Range('1', 8)
                let dx = Math.Abs(row - 'c')
                let dy = Math.Abs(col - '6')
                where dx == dy
                where dx != 0
                select String.Format("{0}{1}", (char)row, (char)col);

            foreach(var item in result)
            {
                WriteLine(item);
            }
        }

        static bool BishopCanMoveTo(dynamic startingPosition, dynamic targetLocation)
        {
            var dx = Math.Abs(startingPosition.File - targetLocation.File);
            var dy = Math.Abs(startingPosition.Rank - targetLocation.Rank);
            return dx == dy && dx != 0;

        }

        static IEnumerable<string> GetBoardPositions()
        {
            return Enumerable.Range('a', 8).SelectMany(
                x => Enumerable.Range('1', 8), (f, r) => 
                    String.Format("{0}{1}",(char)f, (char)r));
        }

        static bool BishopCanMoveTo(string startPos, string targetPos)
        {
            var dx = Math.Abs(startPos[0] - targetPos[0]);
            var dy = Math.Abs(startPos[1] - targetPos[1]);
            return dx == dy && dx != 0;
        }

        static void GetLongestBookUseAggregate()
        {
            var books = new[] {
                    new { Author = "Robert Martin", Title = "Clean Code", Pages = 464 },
                    new { Author = "Oliver Sturm", Title = "Functional Programming in C#" , Pages = 270 },
                    new { Author = "Martin Fowler", Title = "Patterns of Enterprise Application Architecture", Pages = 533 },
                    new { Author = "Bill Wagner", Title = "Effective C#", Pages = 328 },
                    };

            // books.First(b => b.Pages == books.Max(x => x.Pages)); // bad for performance

            // var mostPages = books.Max(x => x.Pages);
            // books.First(b => b.Pages == mostPages); // better for performance than above

            // books.OrderByDescending(b => b.Pages).First(); // hmmm

            var result = books.Aggregate((agg, next) => next.Pages > agg.Pages ? next : agg); // good but hard to read and understand

            // books.MaxBy(b => b.Pages); // best, but need to create the method

            WriteLine(result);
        }
    }
}