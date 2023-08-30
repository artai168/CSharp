namespace BlazorApp.Interfaces.Data
{
    public interface ITable<T_String, T_DateTime>
    {
        public T_String TableName { get; set; }

        public T_DateTime CreateDate { get; set; }
        public T_DateTime UpdateDate { get; set; }
    }
}
