using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Picassi.Core.Accounts.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class FakeDbSet<T> : DbSet<T>, IQueryable<T> where T : class
    {
        protected readonly List<T> FakeData;
        private readonly string idProperty;

        public FakeDbSet()
            : this(new List<T>())
        { }

        public FakeDbSet(T fakeData)
            : this(new List<T> { fakeData })
        { }

        public FakeDbSet(IEnumerable<T> fakeData, string idProperty = "Id")
        {
            this.idProperty = idProperty;
            FakeData = fakeData.ToList();
        }

        public override ObservableCollection<T> Local
        {
            get { return new ObservableCollection<T>(FakeData); }
        }

        Type IQueryable.ElementType
        {
            get { return FakeData.AsQueryable().ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return FakeData.AsQueryable().Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return FakeData.AsQueryable().Provider; }
        }

        public override T Add(T entity)
        {
            FakeData.Add(entity);
            return entity;
        }

        public override T Attach(T entity)
        {
            return entity;
        }

        public override T Remove(T entity)
        {
            FakeData.Remove(entity);
            return entity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return FakeData.GetEnumerator();
        }

        private IEnumerator GetEnumerator1()
        {
            return FakeData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        public override IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            var addRange = entities.ToList();
            FakeData.AddRange(addRange);
            return addRange;
        }

        public override IEnumerable<T> RemoveRange(IEnumerable<T> entities)
        {
            var removeRange = entities.ToList();
            FakeData.RemoveAll(removeRange.Contains);
            return removeRange;
        }

        public override T Find(params object[] keyValues)
        {
            var keyProperty = typeof(T).GetProperty(idProperty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            var result = this.SingleOrDefault(obj => keyProperty.GetValue(obj).ToString() == keyValues.First().ToString());
            return result;
        }
    }
}
