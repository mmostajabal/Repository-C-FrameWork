using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
//using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using AutoMapper.QueryableExtensions;
//using FrameWorkData;
//using DynamicExpression = System.Linq.Dynamic.DynamicExpression;
//using System.Linq.Dynamic.Core;
//using System.Linq.Dynamic.Core.CustomTypeProviders;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;
using System.Linq.Expressions;
//using DelegateDecompiler;

namespace FrameWorkSRV
{
    public abstract class GenericRepository<C, T, TResult> :
        IGenericRepository<T, TResult>
        where T : class
        where TResult : class, new() where C : DbContext, new()
    {

        private C _entities = new C();

        public C Context
        {

            get { return _entities; }
            set { _entities = value; }
        }

        public virtual Tuple<IQueryable<T>, IQueryable<TResult>> GetAll(bool changeType)
        {
            IQueryable<T> query = _entities.Set<T>();
            if (changeType == false)
            {
                return new Tuple<IQueryable<T>, IQueryable<TResult>>(query, null);
            }
            else
            {
                IList<TResult> tResultsList = new List<TResult>();
                foreach (var q in query)
                {
                    TResult tr = Cast<TResult>(q, 1);
                    tResultsList.Add(tr);
                }

                return new Tuple<IQueryable<T>, IQueryable<TResult>>(null, tResultsList.AsQueryable());
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        //public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        //{

        //    IQueryable<T> query = _entities.Set<T>().Where(predicate);
        //    return query;
        //}

        public Tuple<IQueryable<T>, IQueryable<TResult>>  FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate, bool changeType)
        {
            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            if (changeType == false)
            {                
                return new Tuple< IQueryable<T>, IQueryable < TResult >> (query, null);
            }
            else 
            {
                IList<TResult> tResultsList = new List<TResult>();
                foreach (var q in query)
                {
                    TResult tr = Cast<TResult>(q, 1);
                    tResultsList.Add(tr);
                }

                return new Tuple<IQueryable<T>, IQueryable<TResult>>(null, tResultsList.AsQueryable());
                //return tResultsList.AsQueryable();
            }
            
        }


        public virtual IEnumerable<TResult>  GetWithRawSql(string query, params object[] parameters)
        {
            //return new IEnumerable<TResult>(_entities.Set<TResult>().SqlQuery(query, parameters).ToList());
            return _entities.Database.SqlQuery<TResult>(query, parameters).ToList();
        }
        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void AddRange(T[] entity)
        {
            _entities.Set<T>().AddRange(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Attach(entity);
            _entities.Set<T>().Remove(entity);
        }


        public virtual void Update(T entity, string fieldsName = "")
        {
            
            if (fieldsName.Trim() == "")
            {                
                _entities.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                DbEntityEntry<T> dbEntityEntry = _entities.Entry(entity);
                int ind;
                string upperProperty;
                var arrFields = fieldsName.Split(',');
                for (ind = 0; ind < arrFields.Length; ind++)
                {
                    arrFields[ind] = arrFields[ind].Trim().ToUpper();
                }

                var filedsName = typeof(T).GetProperties()
                        .Select(property => property.Name.Trim())
                        .ToArray();

                DbEntityEntry<T> entry = _entities.Entry(entity);
                _entities.Set<T>().Attach(entity);

                foreach (var property in filedsName)
                {
                    upperProperty = property.ToUpper();
                    if (arrFields.Contains(upperProperty))
                    {

                        _entities.Entry(entity).Property(property).IsModified = true;
                    }
                }
            }
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }


        public  IQueryable<T> Join<T>(IQueryable<T> outer, IEnumerable<T> inner, string outerSelector, string innerSelector, string resultsSelector, params object[] values)
        {
            return (IQueryable<T>)Join((IQueryable)outer, (IEnumerable)inner, outerSelector, innerSelector, resultsSelector, values);
        }
        public  IQueryable<TResult> Join(IQueryable outer, IEnumerable inner, string outerSelector, string innerSelector, string resultsSelector, params object[] values)
        {

            if (inner == null) throw new ArgumentNullException("inner");
            if (outerSelector == null) throw new ArgumentNullException("outerSelector");
            if (innerSelector == null) throw new ArgumentNullException("innerSelector");
            if (resultsSelector == null) throw new ArgumentNullException("resultsSelctor");

            LambdaExpression outerSelectorLambda = DynamicExpression.ParseLambda(outer.ElementType, null, outerSelector, values);
            LambdaExpression innerSelectorLambda = DynamicExpression.ParseLambda(inner.AsQueryable().ElementType, null, innerSelector, values);

            ParameterExpression[] parameters = new ParameterExpression[] {
            Expression.Parameter(outer.ElementType, "outer"), Expression.Parameter(inner.AsQueryable().ElementType, "inner") };
            LambdaExpression resultsSelectorLambda = DynamicExpression.ParseLambda(parameters, null, resultsSelector, values);

            IQueryable joinQuery = outer.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Join",
                    new Type[] { outer.ElementType, inner.AsQueryable().ElementType, outerSelectorLambda.Body.Type, resultsSelectorLambda.Body.Type },
                    outer.Expression, inner.AsQueryable().Expression, Expression.Quote(outerSelectorLambda), Expression.Quote(innerSelectorLambda), Expression.Quote(resultsSelectorLambda)));

            IList<TResult> tResultsList = new List<TResult>();
            foreach (dynamic r in joinQuery)
            {
                TResult r1 = Cast<TResult>(r, 2);
                tResultsList.Add(r1);

                //PersonalTbl emp2 = presonalGrade.Cast<PersonalTbl>(r);
                //personalTbls.Add(emp2);
            }
            return tResultsList.AsQueryable();
            //return (IQueryable) Cast<TResult>(joinQuery);
        }

        public TCast Cast<TCast>(Object myobj, short infType)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(TCast);
            TCast[] tempTCast;
            PropertyInfo propertyInfo;
            FieldInfo fieldInfo;
            object value;
            
            var x = Activator.CreateInstance(target, false);
            if (infType == 1)
            {


                var targetmembers = from source in target.GetMembers().ToList()
                        where source.MemberType == MemberTypes.Property
                        select source;

                List<MemberInfo> membersinf = targetmembers.Where(memberInfo => targetmembers.Select(c => c.Name)
                    .ToList().Contains(memberInfo.Name)).ToList();

                var props = myobj.GetType().GetProperties();

                foreach (var memberInfo in membersinf)
                {
                    propertyInfo = typeof (TCast).GetProperty(memberInfo.Name);
                    
                    if (myobj.GetType().GetProperty(memberInfo.Name) != null)
                    {
                        value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);
                        propertyInfo.SetValue(x, value, null);    
                    }
                    else if (myobj.GetType().GetField(memberInfo.Name) != null)
                    {
                        value = myobj.GetType().GetField(memberInfo.Name).GetValue(myobj);
                        propertyInfo.SetValue(x, value, null);                            
                    }
                }

            }else
            {
                var runtimeProperty = from source in objectType.GetRuntimeProperties().ToList()
                        //where source.MemberType == MemberTypes.Property
                        select source;
                List<PropertyInfo> propertyInfos = runtimeProperty.Where(memberInfo => runtimeProperty.Select(c => c.Name)
                        .ToList().Contains(memberInfo.Name)).ToList();

                foreach (var pInfo in propertyInfos)
                {
                    fieldInfo = typeof(TCast).GetRuntimeField(pInfo.Name);
                    if (myobj.GetType().GetProperty(pInfo.Name) != null)
                    {
                        value = myobj.GetType().GetProperty(pInfo.Name).GetValue(myobj, null);


                        fieldInfo.SetValue(x, value);
                    }

                }

                
            }
            return (TCast)x;
        }  
        public Expression<Func<T, TResult>> CreateNewStatement(string fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(T), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(TResult));
            var mi1 = typeof(T).GetProperties();
            var mi2 = typeof(TResult).GetProperties();
            // create initializers
            var bindings = fields.Split(',').Select(o => o.Trim())
                .Select(o =>
                {

                    // property "Field1"
                    var mi = typeof(T).GetProperty(o);

                    // original value "o.Field1"
                    var xOriginal = Expression.Property(xParameter, mi);

                    // set value "Field1 = o.Field1"
                    var t001 = Expression.Bind(mi, xOriginal);
                    return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, TResult>>(xInit, xParameter);

            
            // compile to Func<Data, Data>
            return lambda;
            //return lambda.Compile();
        }



        //The generic overload.

        private static void FixLambdaReturnTypes(ref LambdaExpression lambda1, ref LambdaExpression lambda2)
        {

            Type type1 = lambda1.Body.Type;

            Type type2 = lambda2.Body.Type;

            if (type1 != type2)
            {

                if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof (Nullable<>) &&
                    type1 == type2.GetGenericArguments()[0])
                {

                    lambda1 = Expression.Lambda(Expression.Convert(lambda1.Body, type2), lambda1.Parameters.ToArray());

                }

                else
                {

                    // this may fail because types are incompatible

                    lambda2 = Expression.Lambda(Expression.Convert(lambda2.Body, type1), lambda2.Parameters.ToArray());

                }

            }

        }

    }
}