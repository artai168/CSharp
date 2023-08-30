using BlazorApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Data.Services
{
    public interface IAddress_Service<T_Address>
    {
        Task<IEnumerable<T_Address>> GetAll_Address();
        Task<bool> Insert_Address(T_Address building_Address);
        Task<bool> UPDATE_Address(T_Address building_Address);

        Task<T_Address> Get_Address(string intString, int inInt);

        Task<IEnumerable<T_Address>> GetAll_District();
        Task<IEnumerable<T_Address>> GetAll_Area();
    }
}