using MES.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MES.Web.Controllers
{
    public class DBHelper
    {
        public static Dictionary<Type, DbType> DBTypesMapping = new Dictionary<Type, DbType>()
        {
            {typeof(String), DbType.AnsiString},

            {typeof(Byte), DbType.Byte},
            {typeof(Decimal), DbType.Decimal},
            {typeof(Int16), DbType.Decimal},
            {typeof(Int32), DbType.Decimal},
            {typeof(Int64), DbType.Decimal},
            {typeof(UInt16), DbType.Decimal},
            {typeof(UInt32), DbType.Decimal},
            {typeof(UInt64), DbType.Decimal},

            {typeof(float), DbType.VarNumeric},
            {typeof(double), DbType.VarNumeric},

            {typeof(DateTime), DbType.DateTime},
        };

        public MESDbContext db = new MESDbContext();


        public DataTable Query(String sql, params object[] args)
        {
            using (DbConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                conn.Open();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                if (null != args)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        Object v = args[i];
                        DbParameter param = cmd.CreateParameter();
                        param.ParameterName = (i + 1) + "";
                        param.Value = v ?? DBNull.Value;
                        param.DbType = v == null ? DbType.AnsiString : (DBTypesMapping.ContainsKey(v.GetType()) ? DBTypesMapping[v.GetType()] : DbType.AnsiString);
                        cmd.Parameters.Add(param);
                    }
                }

                DbDataReader ddr = cmd.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add(dt);
                ds.Load(ddr, LoadOption.OverwriteChanges, dt);
                ds.EnforceConstraints = false;
                return dt;
            }
        }

        public async Task<DataTable> QueryAsync(String sql, params object[] args)
        {
            return await Task<DataTable>.Run(() =>
            {
                return Query(sql, args);
            });
        }
    }
}