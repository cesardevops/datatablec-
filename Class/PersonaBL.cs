using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DataTablesjs.Class
{
    public class PersonaBL
    {
        private static int PersonaCount;

        public PersonaBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public Paginator GetAllPersonsPaginator(Paginator obj_response )
        {
            List<PersonaBE> result;
            int totalEmployees = 0;
            if (string.IsNullOrEmpty(obj_response.SortBy))
                obj_response.SortBy = "intIdPersona";
            result = GetALLPersons(obj_response.pageIndex, obj_response.queryRecordCount, obj_response.SortBy, ref totalEmployees);
            PersonaCount = totalEmployees;
            obj_response.records = result;
            obj_response.totalRecordCount = PersonaCount;
            return obj_response;
        }

        public int GetTotalEmployeesCount()
        {
            return PersonaCount;
        }

        public List<PersonaBE> GetALLPersons(int startIndex, int pageSize, string sortBy, ref int totalEmployees)
        {
            System.Data.SqlClient.SqlConnection connection;
            System.Data.SqlClient.SqlCommand selectCommand = null;
            List<PersonaBE> PERSONA_LIST = new List<PersonaBE>();

            try
            {
                using (connection = new System.Data.SqlClient.SqlConnection(@"Data Source=CESARPERSONAL\SQLEXPRESS;Initial Catalog=dbprueba;Integrated Security=SSPI;"))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (selectCommand = new System.Data.SqlClient.SqlCommand())
                    {
                        selectCommand.CommandText = "[dbo].[GETALLPERSONSPAGING]";
                        selectCommand.Connection = connection;
                        selectCommand.CommandType = CommandType.StoredProcedure;

                        selectCommand.Parameters.Add(new SqlParameter("@PageIndex", SqlDbType.Int));
                        selectCommand.Parameters[0].Value = startIndex;
                        selectCommand.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
                        selectCommand.Parameters[1].Value = pageSize;
                        selectCommand.Parameters.Add(new SqlParameter("@sortBy", SqlDbType.VarChar, 30));
                        selectCommand.Parameters[2].Value = sortBy;
                        selectCommand.Parameters.Add(new SqlParameter("@RecordCount", SqlDbType.Int));
                        selectCommand.Parameters[3].Value = totalEmployees;
                        ((System.Data.SqlClient.SqlParameter) selectCommand.Parameters["@RecordCount"]).Direction = ParameterDirection.Output;
                        SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                        DataSet empDS = new DataSet();
                        adapter.Fill(empDS);
                        totalEmployees = Convert.ToInt32((( System.Data.SqlClient.SqlParameter) selectCommand.Parameters["@RecordCount"]).Value);

                        for (int index = 0; index < empDS.Tables[0].Rows.Count; index++)
                        {
                            PersonaBE entity = new PersonaBE()
                            {
                                intIdPersona = Convert.ToInt32(empDS.Tables[0].Rows[index][0].ToString()),
                                vchApePat = empDS.Tables[0].Rows[index][1].ToString(),
                                vchApeMat = empDS.Tables[0].Rows[index][2].ToString(),
                                vchNombres = empDS.Tables[0].Rows[index][3].ToString(),
                                vchDocumento = empDS.Tables[0].Rows[index][4].ToString(),
                                vchCorreo = empDS.Tables[0].Rows[index][5].ToString(),
                                intIdEvento = Convert.ToInt32(empDS.Tables[0].Rows[index][6].ToString())
                            };

                            PERSONA_LIST.Add(entity);
                        }
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            return PERSONA_LIST;
        }
    }
}