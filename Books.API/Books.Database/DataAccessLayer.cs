using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Database
{
    public class DataAccessLayer<T> where T : class
    {
        private readonly LibraryContext context;
        public DataAccessLayer(LibraryContext context)
        {
                this.context = context; 
        }

        public IEnumerable<T>? GetAll()
        {
            try
            {
                return this.context.Set<T>().ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void Add(T entity)
        {
            try
            {
                this.context.Set<T>().Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(T entity)
        {
            try
            {
                this.context.Set<T>().Remove(entity);
                this.context.SaveChanges(); 
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void Update(T entity)
        {
            try
            {
                this.context.Set<T>().Update(entity);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public T? GetEntityByFilter(Func<T,bool> func)
        {
            try
            {
                return this.context.Set<T>().FirstOrDefault(func);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<T>? GetEntitiesByFilter(Func<T, bool> func)
        {
            try
            {
                return this.context.Set<T>().Where(func).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
