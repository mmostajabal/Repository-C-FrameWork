using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkSRV
{
    public interface IGenericRepository<T, TResult> where T : class
        where TResult : class
    {

        Tuple<IQueryable<T>, IQueryable<TResult>> GetAll(bool changeType);
        IQueryable<T> GetAll();
        //IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Tuple<IQueryable<T>, IQueryable<TResult>> FindBy(Expression<Func<T, bool>> predicate, bool changeType = false);
        void Add(T entity);
        void AddRange(T[] entity);
        void Delete(T entity);
        void Update(T entity, string fieldsName = "");
        void Save();

        IEnumerable<TResult> GetWithRawSql(string query, params object[] parameters);
        IQueryable<TResult> Join(IQueryable outer, IEnumerable inner, string outerSelector, string innerSelector,
            string resultsSelector, params object[] values);

        TResult Cast<TResult>(Object myobj, short infType);
        Expression<Func<T, TResult>> CreateNewStatement(string fields);

    }
}
