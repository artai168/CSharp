using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using BlazorApp.Entities;

namespace BlazorApp.Data.Services
{
    public class Raw_Address_Service : IAddress_Service<Raw_Address>
    {
        private readonly SQLConnectionConfiguration _configuration;
        private Entities.T_Raw_Address t_Raw_Address;
        public Raw_Address_Service(SQLConnectionConfiguration sQLConnectionConfiguration)
        {
            _configuration = sQLConnectionConfiguration;
            t_Raw_Address = new Entities.T_Raw_Address();
        }

        private async Task<bool> Execute_Query(string strSQL, DynamicParameters parameters)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    await conn.ExecuteAsync(strSQL, parameters, commandType: CommandType.Text);
                }
                catch (Exception e)
                {
                    //throw e;
                    // Display exception e                    
                    return false;
                }
            }
            return true;
        }

        private DynamicParameters GetParameters(Entities.Raw_Address raw_Address)
        {
            var parameters = new DynamicParameters();

            parameters.Add(t_Raw_Address.Raw_Address_C, raw_Address.Raw_Address_C, DbType.String);
            parameters.Add(t_Raw_Address.Raw_Address_E, raw_Address.Raw_Address_E, DbType.String);
            parameters.Add(t_Raw_Address.Dentist_ID, raw_Address.Dentist_ID, DbType.String);

            return parameters;
        }


        public async Task<bool> Insert_Address(Entities.Raw_Address raw_Address)
        {
            string strSQL = $"INSERT INTO {t_Raw_Address.TableName} ";
            strSQL += $"({t_Raw_Address.Raw_Address_C}, {t_Raw_Address.Raw_Address_E}, {t_Raw_Address.Dentist_ID})";
            strSQL += $" VALUES (@{t_Raw_Address.Raw_Address_C}, @{t_Raw_Address.Raw_Address_E}, @{t_Raw_Address.Dentist_ID})";

            var parameters = new DynamicParameters();
            parameters = GetParameters(raw_Address);

            bool result = await Execute_Query(strSQL, parameters);

            return result;
        }

        public async Task<bool> UPDATE_Address(Entities.Raw_Address raw_Address)
        {
            string strSQL = $"UPDATE {t_Raw_Address.TableName} ";
            strSQL += $" SET {t_Raw_Address.Raw_Address_C} = @{t_Raw_Address.Raw_Address_C},";
            strSQL += $" {t_Raw_Address.Raw_Address_E} = @{t_Raw_Address.Raw_Address_E},";
            strSQL += $" {t_Raw_Address.Dentist_ID} = @{t_Raw_Address.Dentist_ID} ";
            strSQL += $"WHERE {t_Raw_Address.Dentist_ID} = {raw_Address.Dentist_ID})";

            var parameters = new DynamicParameters();
            parameters = GetParameters(raw_Address);

            bool result = await Execute_Query(strSQL, parameters);

            return result;
        }

        public async Task<IEnumerable<Entities.Raw_Address>> GetAll_Address()
        {
            string strSQL = $"SELECT * FROM {t_Raw_Address.TableName};";
            List<Entities.Raw_Address> Address_List = new List<Entities.Raw_Address>();

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    Address_List = (await conn.QueryAsync<Entities.Raw_Address>(strSQL)).ToList();
                }
                catch (Exception e)
                {
                    //throw e;
                    // Display exception e                    
                    return null;
                }
            }
            return Address_List;
        }

        public Task<Raw_Address> Get_Address(string intString, int inInt)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Raw_Address>> IAddress_Service<Raw_Address>.GetAll_District()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Raw_Address>> IAddress_Service<Raw_Address>.GetAll_Area()
        {
            throw new NotImplementedException();
        }
    }
}
