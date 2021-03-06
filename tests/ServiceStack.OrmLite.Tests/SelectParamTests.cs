﻿using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;
using ServiceStack.OrmLite.Tests.Shared;

namespace ServiceStack.OrmLite.Tests
{
    [TestFixture]
    public class SelectParamTests : OrmLiteTestBase
    {
        [Test]
        public void Can_Select_with_Different_APIs()
        {
            using (var db = OpenDbConnection())
            {
                db.CreateTable<Person>();
                db.InsertAll(Person.Rockstars);

                Assert.That(db.Select<Person>(x => x.Age == 27).Count, Is.EqualTo(4));
                Assert.That(db.Select(db.From<Person>().Where(x => x.Age == 27)).Count, Is.EqualTo(4));
                Assert.That(db.Select<Person>("Age = @age", new { age = 27 }).Count, Is.EqualTo(4));
                Assert.That(db.Select<Person>("Age = @age", new Dictionary<string, object> { { "age", 27 } }).Count, Is.EqualTo(4));
                Assert.That(db.Select<Person>("Age = @age", new[] { db.CreateParam("age", 27) }).Count, Is.EqualTo(4));
                Assert.That(db.Select<Person>("SELECT * FROM Person WHERE Age = @age", new { age = 27 }).Count, Is.EqualTo(4));
                Assert.That(db.Select<Person>("SELECT * FROM Person WHERE Age = @age", new Dictionary<string, object> { { "age", 27 } }).Count, Is.EqualTo(4));
                Assert.That(db.Select<Person>("SELECT * FROM Person WHERE Age = @age", new[] { db.CreateParam("age", 27) }).Count, Is.EqualTo(4));

                Assert.That(db.Select<Person>("Age = @age", new { age = 27 }).Count, Is.EqualTo(4));

                Assert.That(db.SelectNonDefaults(new Person { Age = 27 }).Count, Is.EqualTo(4));

                Assert.That(db.SqlList<Person>("SELECT * FROM Person WHERE Age = @age", new { age = 27 }).Count, Is.EqualTo(4));
                Assert.That(db.SqlList<Person>("SELECT * FROM Person WHERE Age = @age", new Dictionary<string, object> { { "age", 27 } }).Count, Is.EqualTo(4));
                Assert.That(db.SqlList<Person>("SELECT * FROM Person WHERE Age = @age", new[] { db.CreateParam("age", 27) }).Count, Is.EqualTo(4));
            }
        }
    }
}