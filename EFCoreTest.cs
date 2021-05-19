using BenchmarkDotNet.Attributes;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCore.ChangeTracker.Benchmarks
{
    [WarmupCount(3)]
    [IterationCount(10)]
    public class EFCoreTest
    {
        [Params(50000, 100000, 150000, 200000, 250000, 300000, 350000, 400000)]
        public int RowsCount { get; set; }

        public EFCoreTest()
        {
            using var context = new PersonDbContext();

            //Seed Database
            if (!context.Persons.Any())
            {
                var index = 1;
                var personFaker = new Faker<Person>()
                    .RuleFor(p => p.Id, f => index++)
                    .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
                    .RuleFor(p => p.Name, (f, p) => f.Name.FindName())
                    .RuleFor(p => p.Age, (f, p) => f.Random.Number(1, 98));

                //Seed Database
                context.Persons.AddRange(personFaker.GenerateForever().Take(1000000));
                context.SaveChanges();
            }
        }

        [Benchmark]
        public void FetchWithoutTracking()
        {
            int luckyPersonId = 0;

            using (var context = new PersonDbContext())
            {
                var persons = context.Persons.Where(_ => _.Id < RowsCount)
                    .AsNoTracking()
                    .ToList();

                luckyPersonId = persons.ElementAt(new Random().Next(0, persons.Count)).Id;
            }

            using (var context = new PersonDbContext())
            {
                var luckyPerson = context.Persons.Find(luckyPersonId);

                luckyPerson.Name = "Lucky Person";

                context.SaveChanges();
            }
        }

        [Benchmark]
        public void FetchWithTracking()
        {
            int luckyPersonId = 0;

            using (var context = new PersonDbContext())
            {
                var persons = context.Persons.Where(_ => _.Id < RowsCount)
                    .ToList();

                luckyPersonId = persons.ElementAt(new Random().Next(0, persons.Count)).Id;
            }

            using (var context = new PersonDbContext())
            {
                var luckyPerson = context.Persons.Find(luckyPersonId);

                luckyPerson.Name = "Lucky Person";

                context.SaveChanges();
            }
        }
    }
}
