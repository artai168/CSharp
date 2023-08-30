namespace BlazorApp.Interfaces.Data
{
    public interface IRaw_Address <T_Integer, T_String>
    {
        public T_Integer ID { get; set; }
        public T_String Raw_Address_C { get; set; }
        public T_String Raw_Address_E { get; set; }
        public T_String Dentist_ID { get; set; }
    }
}
