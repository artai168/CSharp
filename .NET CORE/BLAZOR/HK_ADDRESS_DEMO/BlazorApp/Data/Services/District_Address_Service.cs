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
    public class District_Address_Service : IAddress_Service<District_Address>
    {
        private readonly SQLConnectionConfiguration _configuration;
        private Entities.T_District_Address t_District_Address;
        public District_Address_Service(SQLConnectionConfiguration sQLConnectionConfiguration)
        {
            _configuration = sQLConnectionConfiguration;
            t_District_Address = new Entities.T_District_Address();
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

        private DynamicParameters GetParameters(Entities.District_Address district_Address)
        {
            var parameters = new DynamicParameters();

            parameters.Add(t_District_Address.Street_C, district_Address.Street_C, DbType.String);
            parameters.Add(t_District_Address.District_C, district_Address.District_C, DbType.String);
            parameters.Add(t_District_Address.Area_C, district_Address.Area_C, DbType.String);
            parameters.Add(t_District_Address.Street_E, district_Address.Street_E, DbType.String);
            parameters.Add(t_District_Address.District_E, district_Address.District_E, DbType.String);
            parameters.Add(t_District_Address.Area_E, district_Address.Area_E, DbType.String);

            return parameters;
        }

        public async Task<bool> Insert_Address(Entities.District_Address district_Address)
        {
            string strSQL = $"INSERT INTO {t_District_Address.TableName} ";
            strSQL += $"({t_District_Address.Street_C}, {t_District_Address.District_C}, {t_District_Address.Area_C}, {t_District_Address.Street_E}, {t_District_Address.District_E}, {t_District_Address.Area_E})";
            strSQL += $" VALUES (@{t_District_Address.Street_C}, @{t_District_Address.District_C}, @{t_District_Address.Area_C}, @{t_District_Address.Street_E}, @{t_District_Address.District_E}, @{t_District_Address.Area_E})";

            var parameters = new DynamicParameters();
            parameters = GetParameters(district_Address);

            bool result = await Execute_Query(strSQL, parameters);

            return result;
        }

        public async Task<bool> UPDATE_Address(Entities.District_Address district_Address)
        {
            string strSQL = $"UPDATE {t_District_Address.TableName} ";
            strSQL += $" SET {t_District_Address.District_C} = @{t_District_Address.District_C},";
            strSQL += $" {t_District_Address.Area_C} = @{t_District_Address.Area_C},";
            strSQL += $" {t_District_Address.Street_E} = @{t_District_Address.Street_E},";
            strSQL += $" {t_District_Address.District_E} = @{t_District_Address.District_E},";
            strSQL += $" {t_District_Address.Area_E} = @{t_District_Address.Area_E} ";
            strSQL += $"WHERE {t_District_Address.Street_C} = {district_Address.Street_C})";

            var parameters = new DynamicParameters();
            parameters = GetParameters(district_Address);

            bool result = await Execute_Query(strSQL, parameters);

            return result;
        }

        public async Task<IEnumerable<Entities.District_Address>> GetAll_Address()
        {
            string strSQL = $"SELECT * FROM {t_District_Address.TableName};";
            List<Entities.District_Address> Address_List = new List<Entities.District_Address>();

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    Address_List = (await conn.QueryAsync<Entities.District_Address>(strSQL)).ToList();
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

        public async Task<District_Address> Get_Address(string inString, int inInt)
        {
            switch (inInt)
            {
                case 1:
                    //Building Chinese
                    return await _Get_Street_C_Address(inString);
                    break;

                case 2:
                    //Building Chinese
                    return await _Get_Street_E_Address(inString);
                    break;
                case 3:
                    //Building Chinese
                    return await _Get_District_C_Address(inString);
                    break;
                case 4:
                    //Building Chinese
                    return await _Get_District_E_Address(inString);
                    break;
                case 5:
                    //Building Chinese
                    return await _Get_Area_C_Address(inString);
                    break;
                default:
                    return await _Get_Area_E_Address(inString);
                    break;
            }
        }

        private async Task<Entities.Building_Address> _Get_Street_Address(string inString, DynamicParameters parameters)
        {
            string strSQL = inString;
            Entities.Building_Address Address_List = new Entities.Building_Address();

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    Address_List = (await conn.QueryAsync<Entities.Building_Address>(strSQL, parameters)).FirstOrDefault();
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

        private async Task<Entities.Building_Address> _Get_Street_C_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_District_Address.Street_C}, {t_District_Address.District_C}, {t_District_Address.Area_C} , {t_District_Address.Street_E},  {t_District_Address.District_E}, {t_District_Address.Area_E}";
            strSQL += $" FROM {t_District_Address.TableName}";
            strSQL += $" WHERE ({t_District_Address.Street_C} LIKE '%' +@Condition + '%') ";
            strSQL += $" ORDER BY {t_District_Address.Area_E}, {t_District_Address.District_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Street_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_Street_E_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_District_Address.Street_C}, {t_District_Address.District_C}, {t_District_Address.Area_C} , {t_District_Address.Street_E},  {t_District_Address.District_E}, {t_District_Address.Area_E}";
            strSQL += $" FROM {t_District_Address.TableName}";
            strSQL += $" WHERE (UPPER({t_District_Address.Street_E}) LIKE '%' +@Condition + '%') ";
            strSQL += $" ORDER BY {t_District_Address.Area_E}, {t_District_Address.District_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Street_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_District_C_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_District_Address.District_C}, {t_District_Address.Area_C} , {t_District_Address.District_E}, {t_District_Address.Area_E}";
            strSQL += $" FROM {t_District_Address.TableName}";
            strSQL += $" WHERE ({t_District_Address.District_C}  LIKE '%' +@Condition + '%') ";
            strSQL += $" ORDER BY {t_District_Address.Area_E}, {t_District_Address.District_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Street_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_District_E_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_District_Address.District_C}, {t_District_Address.Area_C} , {t_District_Address.District_E}, {t_District_Address.Area_E}";
            strSQL += $" FROM {t_District_Address.TableName}";
            strSQL += $" WHERE (UPPER({t_District_Address.District_E}) LIKE '%' +@Condition + '%') ";
            strSQL += $" ORDER BY {t_District_Address.Area_E}, {t_District_Address.District_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString.ToUpper(), DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Street_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_Area_E_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_District_Address.Area_C} , {t_District_Address.Area_E}";
            strSQL += $" FROM {t_District_Address.TableName}";
            strSQL += $" WHERE (UPPER({t_District_Address.Area_E}) LIKE '%' +@Condition + '%') ";
            strSQL += $" ORDER BY {t_District_Address.Area_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Street_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_Area_C_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_District_Address.Area_C} , {t_District_Address.Area_E}";
            strSQL += $" FROM {t_District_Address.TableName}";
            strSQL += $" WHERE ({t_District_Address.Area_C} LIKE '%' +@Condition + '%') ";
            strSQL += $" ORDER BY {t_District_Address.Area_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Street_Address(strSQL, parameters);
        }



        public async Task<IEnumerable<District_Address>> GetAll_Area()
        {
            string strSQL = $"SELECT  {t_District_Address.Area_C}, {t_District_Address.Area_E} ";
            strSQL += $" FROM {t_District_Address.TableName}";
            strSQL += $" GROUP BY {t_District_Address.Area_C}, {t_District_Address.Area_E}";
            strSQL += $" ORDER BY {t_District_Address.Area_E}; ";

            List<Entities.Building_Address> Address_List = new List<Entities.Building_Address>();

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    Address_List = (await conn.QueryAsync<Entities.Building_Address>(strSQL)).ToList();
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

        public async Task<IEnumerable<Entities.District_Address>> GetAll_District()
        {
            string strSQL = $"SELECT {t_District_Address.District_C}, {t_District_Address.District_E}, {t_District_Address.Area_C}, {t_District_Address.Area_E} ";
            strSQL += $" FROM {t_District_Address.TableName}";
            strSQL += $" GROUP BY {t_District_Address.Area_E}, {t_District_Address.Area_C}, {t_District_Address.District_C}, {t_District_Address.District_E}";
            strSQL += $" ORDER BY {t_District_Address.Area_E}, {t_District_Address.District_E}; ";

            List<Entities.Building_Address> Address_List = new List<Entities.Building_Address>();

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    Address_List = (await conn.QueryAsync<Entities.Building_Address>(strSQL)).ToList();
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

    }
}
