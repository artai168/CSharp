using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using BlazorApp.Entities;

namespace BlazorApp.Data.Services
{
    public class Building_Address_Service : IAddress_Service<Entities.Building_Address>
    {
        private readonly SQLConnectionConfiguration _configuration;
        private Entities.T_Building_Address t_Building_Address;

        public Building_Address_Service(SQLConnectionConfiguration sqlConnectionConfiguration)
        {
            _configuration = sqlConnectionConfiguration;
            t_Building_Address = new Entities.T_Building_Address();
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

        /*
            Building_C = "Building_C";
            Building_E = "Building_E";
            Street_Num = "Street_Num";
            Street_C = "Street_C";
            District_C = "District_C";
            Area_C = "Area_C";
            Street_E = "Street_E";
            District_E = "District_E";
            Area_E = "Area_E";
         */
        private DynamicParameters GetParameters(Entities.Building_Address building_Address)
        {
            var parameters = new DynamicParameters();
            parameters.Add(t_Building_Address.Building_C, building_Address.Building_C, DbType.String);
            parameters.Add(t_Building_Address.Building_E, building_Address.Building_E, DbType.String);
            parameters.Add(t_Building_Address.Street_Num, building_Address.Street_Num, DbType.String);

            parameters.Add(t_Building_Address.Street_C, building_Address.Street_C, DbType.String);
            parameters.Add(t_Building_Address.District_C, building_Address.District_C, DbType.String);
            parameters.Add(t_Building_Address.Area_C, building_Address.Area_C, DbType.String);
            parameters.Add(t_Building_Address.Street_E, building_Address.Street_E, DbType.String);
            parameters.Add(t_Building_Address.District_E, building_Address.District_E, DbType.String);
            parameters.Add(t_Building_Address.Area_E, building_Address.Area_E, DbType.String);

            return parameters;
        }

        public async Task<bool> Insert_Address(Entities.Building_Address building_Address)
        {
            string strSQL = $"INSERT INTO {t_Building_Address.TableName} ";
            strSQL += $"({t_Building_Address.Building_C}, {t_Building_Address.Building_E}, {t_Building_Address.Street_Num}, {t_Building_Address.Street_C}, {t_Building_Address.District_C}, {t_Building_Address.Area_C}, {t_Building_Address.Street_E}, {t_Building_Address.District_E}, {t_Building_Address.Area_E})";
            strSQL += $" VALUES (@{t_Building_Address.Building_C},@{t_Building_Address.Building_E},@{t_Building_Address.Street_Num},@{t_Building_Address.Street_C}, @{t_Building_Address.District_C}, @{t_Building_Address.Area_C}, @{t_Building_Address.Street_E}, @{t_Building_Address.District_E}, @{t_Building_Address.Area_E})";

            var parameters = new DynamicParameters();
            parameters = GetParameters(building_Address);

            bool result = await Execute_Query(strSQL, parameters);

            return result;
        }

        public async Task<bool> UPDATE_Address(Entities.Building_Address building_Address)
        {
            string strSQL = $"UPDATE {t_Building_Address.TableName} ";
            strSQL += $" SET {t_Building_Address.Building_E} = @{building_Address.Building_E},";
            strSQL += $" {t_Building_Address.Street_Num} = @{building_Address.Street_Num},";
            strSQL += $" {t_Building_Address.Street_C} = @{building_Address.Street_C},";
            strSQL += $" {t_Building_Address.District_C} = @{building_Address.District_C},";
            strSQL += $" {t_Building_Address.Area_C} = @{building_Address.Area_C},";
            strSQL += $" {t_Building_Address.Street_E} = @{building_Address.Street_E},";
            strSQL += $" {t_Building_Address.District_E} = @{building_Address.District_E},";
            strSQL += $" {t_Building_Address.Area_E} = @{building_Address.Area_E} ";
            strSQL += $"WHERE {t_Building_Address.Building_C} = {building_Address.Building_C})";

            var parameters = new DynamicParameters();
            parameters = GetParameters(building_Address);

            bool result = await Execute_Query(strSQL, parameters);

            return result;
        }

        public async Task<IEnumerable<Entities.Building_Address>> GetAll_Address()
        {
            string strSQL = $"SELECT * FROM {t_Building_Address.TableName}";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}, {t_Building_Address.District_E}; ";
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

        public async Task<IEnumerable<Building_Address>> GetAll_Area()
        {
            string strSQL = $"SELECT {t_Building_Address.Area_C}, {t_Building_Address.Area_E} ";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" GROUP BY {t_Building_Address.Area_C}, {t_Building_Address.Area_E}";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}; ";

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

        public async Task<IEnumerable<Entities.Building_Address>> GetAll_District()
        {
            string strSQL = $"SELECT {t_Building_Address.District_C}, {t_Building_Address.District_E}, {t_Building_Address.Area_C}, {t_Building_Address.Area_E} ";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" GROUP BY {t_Building_Address.Area_E}, {t_Building_Address.Area_C}, {t_Building_Address.District_C}, {t_Building_Address.District_E}";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}, {t_Building_Address.District_E}; ";
            
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


        private async Task<Entities.Building_Address> _Get_Bud_Address(string inString, DynamicParameters parameters)
        {
            string strSQL = inString;
            Entities.Building_Address Address_Result = new Entities.Building_Address();

            using (var conn = new SqlConnection(_configuration.Value))
            {
                try
                {
                    Address_Result = (await conn.QueryAsync<Entities.Building_Address>(strSQL, parameters)).FirstOrDefault();
                }
                catch (Exception e)
                {
                    //throw e;
                    // Display exception e                    
                    return null;
                }
            }
            return Address_Result;
        }

        private async Task<Entities.Building_Address> _Get_Bud_C_Address(string inString)
        {
            string strSQL = "SELECT TOP 1";
            strSQL += $" dbo.Building_Dic.{t_Building_Address.Building_C}, dbo.Building_Dic.{t_Building_Address.Building_E}, dbo.Building_Dic.{t_Building_Address.Street_Num}, dbo.Building_Dic.{t_Building_Address.Street_C}, dbo.Building_Dic.{t_Building_Address.District_C}, dbo.Building_Dic.{t_Building_Address.Area_C} , dbo.Building_Dic.{t_Building_Address.Street_E},  dbo.Building_Dic.{t_Building_Address.District_E}, dbo.Building_Dic.{t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" INNER JOIN dbo.BuildingNames ON dbo.Building_Dic.Building_C = dbo.BuildingNames.Building_C";
            strSQL += " WHERE (";
            strSQL += $"(dbo.Building_Dic.{t_Building_Address.Building_C} = @Condition) ";
            strSQL += " OR ";
            strSQL += $" (dbo.BuildingNames.O_{t_Building_Address.Building_C} =@Condition) ";
            strSQL += ") ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            Entities.Building_Address tempResult = new Entities.Building_Address();
            tempResult = await _Get_Bud_Address(strSQL, parameters);
            if (tempResult == null)
            {
                return await _Get_Bud_C_the_Address(inString);
            }
            else
            {
                return tempResult;
            }
        }

        private async Task<Entities.Building_Address> _Get_Bud_C_the_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $" dbo.Building_Dic.{t_Building_Address.Building_C}, dbo.Building_Dic.{t_Building_Address.Building_E}, dbo.Building_Dic.{t_Building_Address.Street_Num}, dbo.Building_Dic.{t_Building_Address.Street_C}, dbo.Building_Dic.{t_Building_Address.District_C}, dbo.Building_Dic.{t_Building_Address.Area_C} , dbo.Building_Dic.{t_Building_Address.Street_E},  dbo.Building_Dic.{t_Building_Address.District_E}, dbo.Building_Dic.{t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += " WHERE ";
            strSQL += $"(dbo.Building_Dic.{t_Building_Address.Building_C} =@Condition) ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Bud_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_Bud_E_Address(string inString)
        {
            string strSQL = "SELECT TOP 1";
            strSQL += $" dbo.Building_Dic.{t_Building_Address.Building_C}, dbo.Building_Dic.{t_Building_Address.Building_E}, dbo.Building_Dic.{t_Building_Address.Street_Num}, dbo.Building_Dic.{t_Building_Address.Street_C}, dbo.Building_Dic.{t_Building_Address.District_C}, dbo.Building_Dic.{t_Building_Address.Area_C} , dbo.Building_Dic.{t_Building_Address.Street_E},  dbo.Building_Dic.{t_Building_Address.District_E}, dbo.Building_Dic.{t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" INNER JOIN dbo.BuildingNames ON dbo.Building_Dic.Building_C = dbo.BuildingNames.Building_C";
            strSQL += " WHERE (";
            strSQL += $"(dbo.Building_Dic.{t_Building_Address.Building_E} =@Condition) ";
            strSQL += " OR ";
            strSQL += $" (dbo.BuildingNames.O_{t_Building_Address.Building_E} =@Condition) ";
            strSQL += ") ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            Entities.Building_Address tempResult = new Entities.Building_Address();
            tempResult = await _Get_Bud_Address(strSQL, parameters);
            if (tempResult == null)
            {
                return await _Get_Bud_E_the_Address(inString);
            }
            else
            {
                return tempResult;
            }
        }

        private async Task<Entities.Building_Address> _Get_Bud_E_the_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $" dbo.Building_Dic.{t_Building_Address.Building_C}, dbo.Building_Dic.{t_Building_Address.Building_E}, dbo.Building_Dic.{t_Building_Address.Street_Num}, dbo.Building_Dic.{t_Building_Address.Street_C}, dbo.Building_Dic.{t_Building_Address.District_C}, dbo.Building_Dic.{t_Building_Address.Area_C} , dbo.Building_Dic.{t_Building_Address.Street_E},  dbo.Building_Dic.{t_Building_Address.District_E}, dbo.Building_Dic.{t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += " WHERE ";
            strSQL += $"(dbo.Building_Dic.{t_Building_Address.Building_E} =@Condition) ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Bud_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_Street_C_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_Building_Address.Street_C}, {t_Building_Address.District_C}, {t_Building_Address.Area_C} , {t_Building_Address.Street_E},  {t_Building_Address.District_E}, {t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" WHERE ({t_Building_Address.Street_C} LIKE '%'+ @Condition +'%') ";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}, {t_Building_Address.District_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Bud_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_Street_E_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_Building_Address.Street_C}, {t_Building_Address.District_C}, {t_Building_Address.Area_C} , {t_Building_Address.Street_E},  {t_Building_Address.District_E}, {t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" WHERE (UPPER({t_Building_Address.Street_E}) LIKE '%'+ @Condition + '%') ";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}, {t_Building_Address.District_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString.ToUpper(), DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Bud_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_District_C_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_Building_Address.District_C}, {t_Building_Address.Area_C} , {t_Building_Address.District_E}, {t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" WHERE ({t_Building_Address.District_C} LIKE '%' + @Condition + '%') ";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}, {t_Building_Address.District_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Bud_Address(strSQL, parameters);
        }
        private async Task<Entities.Building_Address> _Get_District_E_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $"{t_Building_Address.District_C}, {t_Building_Address.Area_C} , {t_Building_Address.District_E}, {t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" WHERE (UPPER({t_Building_Address.District_E}) LIKE '%' + @Condition +'%') ";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}, {t_Building_Address.District_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Bud_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_Area_C_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $" {t_Building_Address.Area_C} , {t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" WHERE ({t_Building_Address.Area_C} LIKE '%' + @Condition +'%') ";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString, DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Bud_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_Area_E_Address(string inString)
        {
            string strSQL = "SELECT ";
            strSQL += $" {t_Building_Address.Area_C} , {t_Building_Address.Area_E}";
            strSQL += $" FROM {t_Building_Address.TableName}";
            strSQL += $" WHERE (UPPER({t_Building_Address.Area_E}) LIKE '%' + + '%') ";
            strSQL += $" ORDER BY {t_Building_Address.Area_E}; ";

            var parameters = new DynamicParameters();
            parameters.Add("@Condition", inString.ToUpper(), DbType.String, ParameterDirection.Input, inString.Length);

            return await _Get_Bud_Address(strSQL, parameters);
        }

        private async Task<Entities.Building_Address> _Get_District_Address(string inString, int theInt)
        {
            District_Address_Service district_Address_Service = new District_Address_Service(_configuration);
            Entities.District_Address district_Address = new Entities.District_Address();
            district_Address = await district_Address_Service.Get_Address(inString, 1);
            Entities.Building_Address building_Address = new Entities.Building_Address
            {
                Street_C = district_Address.Street_C,
                Street_E = district_Address.Street_E,
                District_C = district_Address.District_C,
                District_E = district_Address.District_E,
                Area_C = district_Address.Area_C,
                Area_E = district_Address.Area_E
            };
            return building_Address;
        }

        public async Task<Entities.Building_Address> Get_Address(string inString, int inInt)
        {
            Entities.Building_Address building_Address = new Entities.Building_Address();
            switch (inInt)
            {
                case 1:
                    //Building Chinese
                    return await _Get_Bud_C_Address(inString);
                    break;
                case 2:
                    //Building English
                    return await _Get_Bud_E_Address(inString);
                    break;
                case 3:
                    //Street Chinese
                    
                    building_Address = await _Get_Street_C_Address(inString);
                    if (building_Address != null)
                    {
                        if (string.IsNullOrEmpty(building_Address.Street_C))
                        {
                            return await _Get_District_Address(inString, 1);
                        }
                        else
                        {
                            return building_Address;
                        }
                    }
                    return building_Address;
                    break;
                case 4:
                    //Street English
                    building_Address = await _Get_Street_E_Address(inString);
                    if (building_Address != null)
                    {
                        if (string.IsNullOrEmpty(building_Address.Street_E))
                        {
                            return await _Get_District_Address(inString, 2);
                        }
                        else
                        {
                            return building_Address;
                        }                        
                    }
                    return building_Address;
                    break;
                case 5:
                    //District Chinese
                    building_Address = await _Get_District_C_Address(inString);
                    if (building_Address != null)
                    {
                        if (string.IsNullOrEmpty(building_Address.District_C))
                        {
                            return await _Get_District_Address(inString, 3);
                        }
                        else
                        {
                            return building_Address;
                        }
                    }
                    return building_Address;
                    break;
                case 6:
                    //District English
                    building_Address = await _Get_District_E_Address(inString);
                    if (building_Address != null)
                    {
                        if (string.IsNullOrEmpty(building_Address.District_C))
                        {
                            return await _Get_District_Address(inString, 4);
                        }
                        else
                        {
                            return building_Address;
                        }
                    }
                    return building_Address;
                    break;
                case 7:
                    //Area Chinese
                    building_Address = await _Get_Area_C_Address(inString);
                    if (building_Address != null)
                    {
                        if (string.IsNullOrEmpty(building_Address.Area_C))
                        {
                            return await _Get_District_Address(inString, 5);
                        }
                        else
                        {
                            return building_Address;
                        }
                    }
                    return building_Address;
                    break;

                default:
                    //Area English
                    building_Address = await _Get_Area_E_Address(inString);
                    if (string.IsNullOrEmpty(building_Address.Area_E))
                    {
                        return await _Get_District_Address(inString, 6);
                    }
                    else
                    {
                        return building_Address;
                    }
                    break;
            }

            
            
        }

        
    }        
}