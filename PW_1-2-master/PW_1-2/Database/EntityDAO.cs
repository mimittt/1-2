namespace PW_1_2.Database
{
    public abstract class EntityDAO : Entity
    {
        public abstract void Add();
        public abstract void Remove();
        public abstract void Change();
        public abstract void Find();
        protected abstract void Update<T>(string operation, T entity, int Id);
        public abstract void GetList();
       
    }

}
