using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Blazor_Test2.Data
{
    public class SQL_data
    {

        public Task<List<List<string>>> GetProductAsync(string startDate)

        {

            List<List<string>> data = new List<List<string>>();
            data.Clear();
            string strConn = Global_Variables.SQL_CONFIG;

            SqlConnection conn = new SqlConnection(strConn);

            SqlCommand command = new SqlCommand(startDate, conn);

            //opening connection and executing the query

            conn.Open();

            SqlDataReader rd = command.ExecuteReader();

            while (rd.Read())
            {

                data.Add(new List<string>
                                    {
                                    rd["WH_ID"].ToString(),//0
                                    rd["Loc"].ToString(),//1
                                    rd["In_Date"].ToString() ?? "",//2
                                    rd["Plt_Qty"].ToString() ?? "",//3
                                    rd["Item_No"].ToString() ?? "",//4
                                    rd["Customer_ID"].ToString() ?? "",//5
                                    rd["Prod_Date"].ToString() ?? "",//6
                                    rd["Prod_Type"].ToString() ?? "",//7
                                    rd["Created_by"].ToString() ?? "",//8
                                    rd["Created_Date"].ToString() ?? "",//9
                                    rd["Updated_Date"].ToString() ?? "",//10
                                    rd["Updated_by"].ToString() ?? "",//12
                                    rd["barcode"].ToString() ?? ""//13
                                    });

            }

            conn.Close();

            Task<List<List<string>>> task = Task.FromResult(data);

            //return personModels;

            return task;

        }




        public enum COMMIT
        {
            BEGIN,
            COMMIT,
            ROLLBACK
        }
        private SqlConnection conn;
        private SqlTransaction tran;
        private bool isSQLOpen = false;

        // 打開 DB
        public bool openDB(string config)
        {
            try
            {
                var sqlSetting = config;
                conn = new SqlConnection(sqlSetting);
                conn.Open();
                isSQLOpen = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        // 關閉 DB
        public bool closeDB()
        {
            try
            {
                if (isSQLOpen)
                {
                    conn.Close();
                    conn.Dispose();
                    SqlConnection.ClearAllPools();
                    conn = null;
                    isSQLOpen = false;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        // 確認 DB 連線
        public bool isOpen()
        {
            return isSQLOpen;

        }

        // 確認 DB 連線狀態
        public bool Open_check()
        {
            bool a = true;
            if (conn.State == ConnectionState.Closed)
            {
                a = false;
            }
            else if (conn.State == ConnectionState.Broken)
            {
                a = false;

            }
            else
            {
                a = true;
            }

            return a;

        }

        // 交易控制
        public bool commitCtrl(COMMIT commit)
        {
            switch (commit)
            {
                case COMMIT.BEGIN:
                    try
                    {
                        tran = conn.BeginTransaction();

                        return true;
                    }
                    catch (Exception e)
                    {

                        return false;
                    }
                case COMMIT.COMMIT:
                    try
                    {
                        tran.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {

                        return false;
                    }
                case COMMIT.ROLLBACK:
                    try
                    {
                        tran.Rollback();
                        return true;
                    }
                    catch (Exception e)
                    {

                        return false;
                    }
                default:
                    return false;
            }
        }

        // select
        public bool readSql(string query, ref DbDataReader rd)
        {
            try
            {
                SqlCommand SqlDbCmd = new SqlCommand(query);
                SqlDbCmd.Connection = conn;
                rd = SqlDbCmd.ExecuteReader();
                return true;
            }
            catch
            {
                return false;
            }
        }
        // insert / update / delete
        public bool execSql(string query)
        {
            try
            {
                SqlCommand SqlDbCmd = new SqlCommand(query);
                SqlDbCmd.Connection = conn;
                SqlDbCmd.Transaction = tran;
                bool bReturnValue;
                bReturnValue = (SqlDbCmd.ExecuteNonQuery() <= 0 ? false : true);
                return bReturnValue;
            }
            catch
            {
                return false;
            }
        }
        public int execSql_count(string query)
        {
            try
            {
                SqlCommand SqlDbCmd = new SqlCommand(query);
                SqlDbCmd.Connection = conn;
                SqlDbCmd.Transaction = tran;
                int bReturnValue;
                bReturnValue = SqlDbCmd.ExecuteNonQuery();
                return bReturnValue;
            }
            catch
            {
                return 0;
            }
        }

    }
}
