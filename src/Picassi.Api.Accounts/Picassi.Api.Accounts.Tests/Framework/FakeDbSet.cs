using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class FakeDbSet<T> : IDbSet<T> where T : class
    {
        private readonly List<T> _data;

        public FakeDbSet()
        {
            _data = new List<T>();
        }

        public FakeDbSet(params T[] entities)
        {
            _data = new List<T>(entities);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public Expression Expression => Expression.Constant(_data.AsQueryable());

        public Type ElementType => typeof(T);

        public IQueryProvider Provider => _data.AsQueryable().Provider;

        public T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Wouldn't you rather use Linq .SingleOrDefault()?");
        }

        public T Add(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<T> Local => new ObservableCollection<T>(_data);

        public void ValidateConstraints()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var hasUniqueConstraint = property.GetCustomAttributes(typeof(IndexAttribute), true).FirstOrDefault() as IndexAttribute;
                if (hasUniqueConstraint?.IsUnique == true) AssertUniqueness(property);
            }
        }

        private void AssertUniqueness(PropertyInfo property)
        {
            var values = _data.Select(property.GetValue).ToList();
            if (values.Count != values.Distinct().Count())
            {
                throw new ValidationException();
            }
        }
    }
}