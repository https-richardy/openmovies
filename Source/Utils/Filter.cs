using System.Linq.Expressions;
using OpenMovies.Models;

namespace OpenMovies.Utils;

public class Filter<TEntity>
    where TEntity : Entity
{
    private readonly List<Expression<Func<TEntity, bool>>> _filterExpressions = new List<Expression<Func<TEntity, bool>>>();

    public Filter<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        _filterExpressions.Add(predicate);
        return this;
    }

    public IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query)
    {
        foreach (var filterExpressions in _filterExpressions)
            query = query.Where(filterExpressions);

        return query;
    }
}