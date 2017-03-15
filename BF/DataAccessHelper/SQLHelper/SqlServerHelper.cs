﻿
using FastReflectionLib;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using DataAccessHelper.Interface;

namespace DataAccessHelper.SQLHelper
{
    public class SqlServerHelper : ISQLHelper
    {
        #region Fields

        public SqlServerHelper(string sqlConnStringName)
        {
            SqlConnStringName = sqlConnStringName;
        }

        private string SqlConnStringName { get; set; }


        private string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[SqlConnStringName].ConnectionString;
            }
        }
        private static readonly BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行sql命令，返回影响行数 (启用事务)
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType)
        {
            var result = 0;
            
            SqlTransaction trans = null;
            
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                trans = conn.BeginTransaction();
                
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                cmd.Transaction = trans;
                result = cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回影响行数
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="isUseTrans">是否启用事务</param> 
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteNonQuery(sqlText, cmdType);
            }
            var result = 0;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回影响行数 (启用事务)
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParam">sql命令的一个参数 （可为空）</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, SqlParameter sqlParam)
        {
            
            var result = 0;
            SqlTransaction trans = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                trans = conn.BeginTransaction();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                if (sqlParam != null)
                {
                    cmd.Parameters.Add(sqlParam);
                }
                cmd.Transaction = trans;
                result = cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回影响行数 
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParam">sql命令的一个参数 （可为空）</param>
        /// <param name="isUseTrans">是否启用事务</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, SqlParameter sqlParam, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteNonQuery(sqlText, cmdType, sqlParam);
            }
            var result = 0;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                if (sqlParam != null)
                {
                    cmd.Parameters.Add(sqlParam);
                }
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回影响行数 (启用事务)
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令的参数数组（可为空）</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, SqlParameter[] sqlParams)
        {
            var result = 0;
            SqlTransaction trans = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                trans = conn.BeginTransaction();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                if (sqlParams != null && sqlParams.Length > 0)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                cmd.Transaction = trans;
                result = cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回影响行数 
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令的参数数组（可为空）</param>
        /// <param name="isUseTrans">是否启用事务</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, SqlParameter[] sqlParams, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteNonQuery(sqlText, cmdType, sqlParams);
            }
            var result = 0;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                if (sqlParams != null && sqlParams.Length > 0)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回影响行数 (启用事务)
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">sql命令的参数数组（可为空）</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            var result = 0;
            SqlTransaction trans = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                trans = conn.BeginTransaction();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                PrepareCommand(cmd, dictParams, cmdType, sqlText);
                cmd.Transaction = trans;
                result = cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回影响行数 
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="dictParams">sql命令的参数数组（可为空）</param>
        /// <param name="isUseTrans">是否启用事务</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteNonQuery(sqlText, cmdType, dictParams);
            }
            var result = 0;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                PrepareCommand(cmd, dictParams, cmdType, sqlText);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行sql命令，返回第一行第一列（启用事务）
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType)
        {
            object result = null;
            SqlTransaction trans = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                trans = conn.BeginTransaction();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                cmd.Transaction = trans;
                result = cmd.ExecuteScalar();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="isUseTrans">是否启用事务</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteScalar(sqlText, cmdType);
            }
            object result = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回第一行第一列（启用事务）
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParam">sql命令参数 （可为空）</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, SqlParameter sqlParam)
        {
            object result = null;
            SqlTransaction trans = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                trans = conn.BeginTransaction();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                if (sqlParam != null)
                {
                    cmd.Parameters.Add(sqlParam);
                }
                cmd.Transaction = trans;
                result = cmd.ExecuteScalar();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParam">sql命令参数 （可为空）</param>
        /// <param name="isUseTrans">是否启用事务</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, SqlParameter sqlParam, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteScalar(sqlText, cmdType, sqlParam);
            }
            object result = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                if (sqlParam != null)
                {
                    cmd.Parameters.Add(sqlParam);
                }
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回第一行第一列（启用事务）
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令参数 （可为空）</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, SqlParameter[] sqlParams)
        {
            object result = null;
            SqlTransaction trans = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                trans = conn.BeginTransaction();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                if (sqlParams != null && sqlParams.Length > 0)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                cmd.Transaction = trans;
                result = cmd.ExecuteScalar();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令参数 （可为空）</param>
        /// <param name="isUseTrans">是否使用事务</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, SqlParameter[] sqlParams, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteScalar(sqlText, cmdType, sqlParams);
            }
            object result = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                if (sqlParams != null && sqlParams.Length > 0)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回第一行第一列（启用事务）
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令参数 （可为空）</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            object result = null;
            SqlTransaction trans = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                trans = conn.BeginTransaction();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                PrepareCommand(cmd, dictParams, cmdType, sqlText);
                cmd.Transaction = trans;
                result = cmd.ExecuteScalar();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        /// <summary>
        /// 执行sql命令，返回第一行第一列
        /// </summary>
        /// <param name="strSqlConn">数据库连接字符串</param>
        /// <param name="sqlText">数据库命令：存储过程名或sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="sqlParams">sql命令参数 （可为空）</param>
        /// <param name="isUseTrans">是否使用事务</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams, bool isUseTrans)
        {
            if (isUseTrans)
            {
                return ExecuteScalar(sqlText, cmdType, dictParams);
            }
            object result = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;

                PrepareCommand(cmd, dictParams, cmdType, sqlText);
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ex.Source += sqlText;
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }
            return result;
        }

        #endregion

        #region Batch Insert


        /// <summary>
        /// 拼接字符串批量插入(安全插入)
        /// </summary>
        /// <param name="sqlString">sql插入语句，形如INSERT INTO test.Person(FirstName) VALUES或 INSERT INTO test.Person(FirstName)</param>
        /// <param name="columes">插入的列数</param>
        /// <param name="paramValues">需要插入的值</param>
        /// <param name="strConnection">数据库连接字符串</param>
        /// <returns></returns>
        public int BatchInsert( string sqlString, int columes, object[] paramValues)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(sqlString);
            if (sqlString.LastIndexOf(" VALUES", StringComparison.OrdinalIgnoreCase) == -1)
            {
                sb.Append(" VALUES ");
            }
            var listParamKeys = new List<string>();//参数的键值
            string paramKey = string.Empty;
            int rows = paramValues.Length / columes;
            for (int i = 0; i < rows; i++) //拼接参数
            {
                sb.Append("(");
                for (int j = 0; j < columes; j++)
                {
                    paramKey = string.Format("@v_{0}", columes * i + j); //参数前必须加入@
                    sb.Append(paramKey);
                    listParamKeys.Add(paramKey);
                    if (j < columes - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("),");
            }
            string sqlText = sb.ToString().Trim(',') + ";";
            int affectNum = ExecuteNonQuery(sqlText, CommandType.Text, PrepareParameters(listParamKeys.ToArray(), paramValues), true);//拼接字符串批量插入
            return affectNum;
        }

        /// <summary>
        /// 拼接字符串批量插入
        /// </summary>
        /// <param name="sqlString">sql插入语句，形如INSERT INTO test.Person(FirstName) VALUES或 INSERT INTO test.Person(FirstName)</param>
        /// <param name="columes">插入的列数</param>
        /// <param name="paramValues">需要插入的值</param>
        /// <param name="strConnection">数据库连接字符串</param>
        /// <param name="isSafe">是否直接拼接字符串安全插入</param>        
        /// <returns></returns>
        public int BatchInsert( string sqlString, int columes, object[] paramValues, bool isSafe)
        {
            if (isSafe == true) //安全插入
            {
                return BatchInsert(sqlString, columes, paramValues);
            }
            if (paramValues.Length % columes != 0)
            {
                throw new ArgumentException("参数个数有误");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(sqlString);
            if (sqlString.LastIndexOf(" VALUES", StringComparison.OrdinalIgnoreCase) == -1)
            {
                sb.Append(" VALUES ");
            }
            int rows = paramValues.Length / columes;
            for (int i = 0; i < rows; i++)
            {
                sb.Append("(");
                for (int j = 0; j < columes; j++)
                {
                    sb.AppendFormat("'{0}'", paramValues[columes * i + j]);
                    if (j < columes - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("),");
            }
            int affectNum = ExecuteNonQuery(sb.ToString().Trim(',') + ";", CommandType.Text, true);//拼接字符串批量插入
            return affectNum;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbName">要插入的目标表名称</param>
        /// <param name="columeArr">要插入的列名数组</param>
        /// <param name="listModels">要插入的实体数组</param>
        /// <param name="strConnection">数据库连接字符串</param>
        /// <returns></returns>
        public int BatchInsert<T>( string tbName, string[] columeArr, IList<T> listModels) where T : class, new()
        {
            if (listModels == null || listModels.Count == 0)
            {
                throw new ArgumentException("没有需要批量插入的数据");
            }
            int columes = columeArr.Length;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} ", tbName);
            AppendColumes(sb, columeArr);
            sb.Append(" VALUES ");
            var listParamKeys = new List<string>();//参数的键值
            string paramKey = string.Empty;
            for (int i = 0; i < listModels.Count; i++)  //构造参数
            {
                sb.Append("(");
                for (int j = 0; j < columes; j++)
                {
                    paramKey = string.Format("@v_{0}_{1}", columeArr[j], columes * i + j); //参数前必须加入@
                    sb.Append(paramKey);
                    listParamKeys.Add(paramKey);
                    if (j < columes - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("),");
            }
            var listParamValues = new List<object>();
            for (int i = 0; i < listModels.Count; i++)  //构造参数值数组
            {
                FastPrepareParamValue<T>(listModels[i], columeArr, listParamValues);
            }
            string sqlText = sb.ToString().Trim(',') + ";";
            int affectNum = ExecuteNonQuery(sqlText, CommandType.Text, PrepareParameters(listParamKeys.ToArray(), listParamValues.ToArray()), true);//拼接字符串批量插入
            return affectNum;
        }

        /// <summary>
        /// 拼接需要插入的列
        /// </summary>
        /// <param name="sb">StringBuilder对象，附加sql字符串</param>
        /// <param name="columeArr">列名数组 （不能为空且必须有值）</param>
        private static void AppendColumes(StringBuilder sb, string[] columeArr)
        {
            if (columeArr == null || columeArr.Length == 0)
            {
                throw new ArgumentException("插入列不能为空");
            }
            sb.Append("(");
            for (int i = 0; i < columeArr.Length; i++)
            {
                sb.Append(columeArr[i]);
                if (i < columeArr.Length - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(")");
        }

        /// <summary>
        /// 通过反射将数据列对应的实体中的值附加到参数数组中
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="model">实体对象</param>
        /// <param name="columeArr">列名数组</param>
        /// <param name="listPramValues">参数对象列表</param>
        private static void FastPrepareParamValue<T>(T model, string[] columeArr, List<object> listPramValues)
        {
            object objValue = null;
            var objType = model.GetType();
            var properties = objType.GetProperties(bf);
            foreach (var columeName in columeArr)
            {
                foreach (var propInfo in properties)
                {
                    if (string.Compare(columeName, propInfo.Name, true) != 0)
                    {
                        continue;
                    }
                    try
                    {
                        objValue = propInfo.FastGetValue(model);
                    }
                    catch
                    {
                        objValue = null;
                    }
                    finally
                    {
                        listPramValues.Add(objValue);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="tbName">要插入的目标表名称</param>
        /// <param name="columeArr">要插入的列名数组</param>
        /// <param name="dt">要插入的datatable</param>
        /// <param name="strConnection">数据库连接字符串</param>
        /// <returns></returns>
        public int BatchInsert( string tbName, string[] columeArr, DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ArgumentException("没有需要批量插入的数据");
            }
            int columes = columeArr.Length;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} ", tbName);
            AppendColumes(sb, columeArr);
            sb.Append(" VALUES ");
            var listParamKeys = new List<string>();//参数的键值
            string paramKey = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)  //构造参数
            {
                sb.Append("(");
                for (int j = 0; j < columes; j++)
                {
                    paramKey = string.Format("@v_{0}_{1}", columeArr[j], columes * i + j); //参数前必须加入@
                    sb.Append(paramKey);
                    listParamKeys.Add(paramKey);
                    if (j < columes - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("),");
            }
            var listParamValues = new List<object>();
            for (int i = 0; i < dt.Rows.Count; i++)  //构造参数值数组
            {
                foreach (var item in columeArr)
                {
                    object objValue = dt.Rows[i][item];
                    listParamValues.Add(objValue);
                }
            }
            string sqlText = sb.ToString().Trim(',') + ";";
            int affectNum = ExecuteNonQuery(sqlText, CommandType.Text, PrepareParameters(listParamKeys.ToArray(), listParamValues.ToArray()), true);//拼接字符串批量插入
            return affectNum;
        }

        #endregion

        #region Collections

        public IDataReader ExecuteReader(string sqlText, CommandType cmdType)
        {
            IDataReader reader = null;
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlText, conn);
            cmd.CommandType = cmdType;
            conn.Open();
            reader = cmd.ExecuteReader();
            return reader;
        }

        public IDataReader ExecuteReader(string sqlText, CommandType cmdType, SqlParameter[] sqlParams)
        {
            IDataReader reader = null;
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlText, conn);
            conn.Open();
            PrepareCommand(cmd, sqlParams, cmdType);
            reader = cmd.ExecuteReader();
            return reader;
        }

        public IDataReader ExecuteReader(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            IDataReader reader = null;
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(sqlText, conn);
            conn.Open();
            PrepareCommand(cmd, dictParams, cmdType, sqlText);
            reader = cmd.ExecuteReader();
            return reader;
        }

        public DataSet QueryForDataSet(string sqlText, CommandType cmdType)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlText, conn);
                cmd.CommandType = cmdType;
                conn.Open();
                
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
            }
            return ds;
        }

        public DataSet QueryForDataSet(string sqlText, CommandType cmdType, SqlParameter[] sqlParams)
        {
            if (sqlParams == null || sqlParams.Length == 0)
            {
                return QueryForDataSet(sqlText, cmdType);
            }
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlText, conn);
                PrepareCommand(cmd, sqlParams, cmdType);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
            }
            return ds;
        }

        public DataSet QueryForDataSet(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            if (dictParams == null || dictParams.Count == 0)
            {
                return QueryForDataSet(sqlText, cmdType);
            }
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlText, conn);
                PrepareCommand(cmd, dictParams, cmdType, sqlText);
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
            }
            return ds;
        }

        public DataTable QueryForDataTable(string sqlText, CommandType cmdType)
        {
            DataTable dt = null;
            DataSet ds = QueryForDataSet(sqlText, cmdType);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public DataTable QueryForDataTable(string sqlText, CommandType cmdType, SqlParameter[] sqlParams)
        {
            if (sqlParams == null || sqlParams.Length == 0)
            {
                return QueryForDataTable(sqlText, cmdType);
            }
            DataTable dt = null;
            DataSet ds = QueryForDataSet(sqlText, cmdType, sqlParams);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }

        public DataTable QueryForDataTable(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            if (dictParams == null || dictParams.Count == 0)
            {
                return QueryForDataTable(sqlText, cmdType);
            }
            DataTable dt = null;
            DataSet ds = QueryForDataSet(sqlText, cmdType, dictParams);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }

        public DataTable[] QueryForDataTables(string sqlText, CommandType cmdType)
        {
            DataTable[] dt = null;
            DataSet ds = QueryForDataSet(sqlText, cmdType);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = new DataTable[ds.Tables.Count];
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    dt[i] = ds.Tables[i];
                }
            }
            return dt;
        }

        public DataTable[] QueryForDataTables(string sqlText, CommandType cmdType, SqlParameter[] sqlParams)
        {
            if (sqlParams == null || sqlParams.Length == 0)
            {
                return QueryForDataTables(sqlText, cmdType);
            }
            DataTable[] dt = null;
            DataSet ds = QueryForDataSet(sqlText, cmdType, sqlParams);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = new DataTable[ds.Tables.Count];
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    dt[i] = ds.Tables[i];
                }
            }
            return dt;
        }

        public DataTable[] QueryForDataTables(string sqlText, CommandType cmdType, IDictionary<string, object> dictParams)
        {
            if (dictParams == null || dictParams.Count == 0)
            {
                return QueryForDataTables(sqlText, cmdType);
            }
            DataTable[] dt = null;
            DataSet ds = QueryForDataSet(sqlText, cmdType, dictParams);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = new DataTable[ds.Tables.Count];
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    dt[i] = ds.Tables[i];
                }
            }
            return dt;
        }

        #endregion

        #region Prepare Parameter

        public SqlParameter PrepareParameter(string paramName, object paramValue)
        {
            var parameter = new SqlParameter(paramName, paramValue);
            return parameter;
        }

        public SqlParameter PrepareParameter(string paramName, DbType dbType, object paramValue)
        {
            var parameter = new SqlParameter(paramName, dbType);
            parameter.Value = paramValue;
            return parameter;
        }

        public SqlParameter[] PrepareParameters(string[] paramNames, object[] paramValues)
        {
            var parameters = new SqlParameter[paramNames.Length];
            for (int i = 0; i < paramNames.Length; i++)
            {
                parameters[i] = new SqlParameter(paramNames[i], paramValues[i]);
            }
            return parameters;
        }

        public SqlParameter[] PrepareParameters(string[] paramNames, DbType[] dbTypes, object[] paramValues)
        {
            var parameters = new SqlParameter[paramNames.Length];
            for (int i = 0; i < paramNames.Length; i++)
            {
                parameters[i] = new SqlParameter(paramNames[i], dbTypes[i]);
                parameters[i].Value = paramValues[i];
            }
            return parameters;
        }


        #endregion

        #region prepare parameter

        public void PrepareCommand(SqlCommand cmd, SqlParameter parameter, CommandType cmdType)
        {
            cmd.CommandType = cmdType;
            if (parameter != null)
            {
                cmd.Parameters.Add(parameter);
            }
        }

        public void PrepareCommand(SqlCommand cmd, SqlParameter[] sqlParams, CommandType cmdType)
        {
            cmd.CommandType = cmdType;
            if (sqlParams != null && sqlParams.Length > 0)
            {
                cmd.Parameters.AddRange(sqlParams);
            }
        }

        public void PrepareCommand(SqlCommand cmd, IDictionary<string, object> dictParams, CommandType cmdType,string sqlText)
        {
            cmd.CommandType = cmdType;
            if (dictParams == null || dictParams.Count == 0)
            {
                return;
            }
            foreach (KeyValuePair<string, object> kv in dictParams)
            {
                if (sqlText.IndexOf(string.Format("@{0}", kv.Key)) >= 0)
                {
                    SqlParameter param = new SqlParameter(kv.Key, kv.Value);
                    cmd.Parameters.Add(param);
                }
            }
        }

        public void PrepareCommand(SqlCommand cmd, string[] paraNames, object[] paraValues, CommandType cmdType)
        {
            cmd.CommandType = cmdType;
            SqlParameter[] sqlParas = PrepareParameters(paraNames, paraValues);
            if (sqlParas == null)
            {
                return;
            }
            cmd.Parameters.AddRange(sqlParas);
        }

        public void PrepareCommand(SqlCommand cmd, string paraName, object paraValue, CommandType cmdType)
        {
            cmd.CommandType = cmdType;
            SqlParameter sqlPara = PrepareParameter(paraName, paraValue);
            cmd.Parameters.Add(sqlPara);
        }

        public void PrepareCommand(SqlCommand cmd, int commandTimeout)
        {
            cmd.CommandTimeout = commandTimeout;
        }

        #endregion

        
        #region Close Connection

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="conn"></param>
        public void CloseConnection(IDbConnection conn)
        {
            if (conn == null)
            {
                return;
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        #endregion
    }
}