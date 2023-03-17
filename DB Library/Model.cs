using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public abstract class Model
    {

            public int id { get; set; }
            private string sql = "";
            private IDataReader reader;

            public static Dictionary<string, T> ObjectToDictionary<T>(object obj)
            {
                if (obj == null)
                    throw new ArgumentNullException(nameof(obj), "Object cannot be null.");

                var dictionary = new Dictionary<string, T>();

                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
                {
                    var value = property.GetValue(obj);
                    if (value is T typedValue)
                    {
                        dictionary.Add(property.Name, typedValue);
                    }
                    else if (value != null)
                    {
                        throw new InvalidCastException($"Cannot cast {value.GetType()} to {typeof(T)}.");
                    }
                }

                return dictionary;
            }


            private static dynamic DictionaryToObject(Dictionary<String, object> dico)
            {
                dynamic obj = new System.Dynamic.ExpandoObject();
                var dict = (IDictionary<String, object>)obj;

                foreach (var item in dico)
                {
                    dict.Add(item.Key, item.Value);
                }

                return obj;
            }
            public int save(string procedureName = null)
            {
                Dictionary<string, object> dico = new Dictionary<string, object>();

                dico = ObjectToDictionary<object>(this);

                if (procedureName == null)
                {
                    try
                    {
                        // inserting the keys=col names/values=row values
                        //connect first 
                        Connexion.Connect();


                        sql = "insert into " + this.GetType().Name + " values(";
                        int i = 0;

                    sql += dico.Last().Value + ",";
                        foreach (var item in dico)
                        {
                            if (i == dico.Count - 2)
                            {
                                if (item.Value is string)
                                {
                                    sql += "'" + item.Value + "'";

                                }
                                else
                                {
                                    sql += item.Value;

                                }
                                break;
                            }
                            else
                            {

                                if (item.Value is string)
                                {
                                    sql += "'" + item.Value + "',";

                                }
                                else
                                {
                                    sql += item.Value + ",";
                                }

                            }
                            i++;
                        }

                        sql += ")";
                        Console.WriteLine(sql);

                    Connexion.cmd = (Connexion.con).CreateCommand();
                    (Connexion.cmd).CommandText = sql;
                    
                    (Connexion.cmd).ExecuteNonQuery();



                    return 0;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Error: " + ex.Message);
                        return 1;
                    }
                    finally
                    {
                        (Connexion.con).Close();
                    }
                }

                else
                {
                    try
                    {

                        Connexion.Connect();

                        Connexion.cmd = (Connexion.con).CreateCommand();
                        Connexion.cmd.CommandText = procedureName;
                        Connexion.cmd.CommandType = CommandType.StoredProcedure;
                        if (Connexion.con is MySqlConnection)
                        {
                            foreach (var item in dico)
                            {
                                if (item.Key.Equals("id"))
                                {

                                    ((MySqlCommand)Connexion.cmd).Parameters.AddWithValue("@new_" + item.Key, item.Value);
                                }

                                ((MySqlCommand)Connexion.cmd).Parameters.AddWithValue("@" + item.Key, item.Value);
                            }
                        }

                        else if (Connexion.con is OracleConnection)
                        {
                            foreach (var item in dico)
                            {
                                if (item.Key.Equals("id"))
                                {
                                    ((OracleCommand)Connexion.cmd).Parameters.Add("@new_id", OracleDbType.Int32).Value = this.id;
                                }
                                else
                                {
                                    ((OracleCommand)Connexion.cmd).Parameters.Add("@" + item.Key, item.Value);

                                }
                            }
                        }

                        Connexion.cmd.ExecuteNonQuery();
                        return 0;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Error: " + ex.Message);
                        return 1;
                    }
                    finally
                    {
                        (Connexion.con).Close();
                    }
                }

                Console.ReadLine();
            }


            public dynamic find()
            {
                Dictionary<string, object> dico = new Dictionary<string, object>();
                this.sql = "select * from " + this.GetType().Name + " where id=" + id + " ";

                try
                {
                    Connexion.Connect();

                    Connexion.cmd = (Connexion.con).CreateCommand();
                    (Connexion.cmd).CommandText = sql;

                    (Connexion.cmd).ExecuteNonQuery();

                    if (Connexion.con is MySqlConnection)
                    {
                        reader = (MySqlDataReader)(Connexion.cmd).ExecuteReader();
                    }

                    else if (Connexion.con is OracleConnection)
                    {
                        reader = (OracleDataReader)(Connexion.cmd).ExecuteReader();
                    }

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dico.Add(reader.GetName(i), reader.GetValue(i));

                        }


                    }

                    reader.Close();

                    return DictionaryToObject(dico);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("here's smth wrong...");
                    return 1;
                }
                finally
                {
                    (Connexion.con).Close();
                }

                Console.ReadLine();


            }
            public static dynamic find<T>(int id)
            {

                Dictionary<string, object> dico = new Dictionary<string, object>();
                string tableName = typeof(T).Name;
                string sql = "select * from " + tableName + "  where id=" + id + " ";

                try
                {
                    Connexion.Connect();
                    Connexion.cmd = (Connexion.con).CreateCommand();
                    (Connexion.cmd).CommandText = sql;
                    (Connexion.cmd).ExecuteNonQuery();

                    if (Connexion.con is MySqlConnection)
                    {
                        MySqlDataReader reader = (MySqlDataReader)(Connexion.cmd).ExecuteReader();
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dico.Add(reader.GetName(i), reader.GetValue(i));

                            }


                        }

                        reader.Close();
                    }

                    else if (Connexion.con is OracleConnection)
                    {
                        OracleDataReader reader = (OracleDataReader)(Connexion.cmd).ExecuteReader();
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dico.Add(reader.GetName(i), reader.GetValue(i));

                            }


                        }

                        reader.Close();
                    }

                    return DictionaryToObject(dico);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return 1;
                }
                finally
                {
                    (Connexion.con).Close();
                }

                Console.ReadLine();



            }
            public int delete(string procedureName= null)
            {
                if (procedureName == null)
                {
                    sql = "delete from " + this.GetType().Name + " where id=" + this.id;

                    try
                    {
                        Connexion.Connect();
                        Connexion.cmd = (Connexion.con).CreateCommand();
                        (Connexion.cmd).CommandText = sql;
                        (Connexion.cmd).ExecuteNonQuery();
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 1;
                    }
                    finally
                    {
                        (Connexion.con).Close();
                    }
                }

                else
                {


                    try
                    {
                        Connexion.Connect();
                        Connexion.cmd = (Connexion.con).CreateCommand();
                        Connexion.cmd.CommandText = procedureName;
                        Connexion.cmd.CommandType = CommandType.StoredProcedure;
                        if (Connexion.con is MySqlConnection)
                        {
                            ((MySqlCommand)Connexion.cmd).Parameters.AddWithValue("@new_id", this.id);
                        }
                        else if (Connexion.con is OracleConnection)
                        {
                            ((OracleCommand)Connexion.cmd).Parameters.Add("@new_id", OracleDbType.Int32).Value = this.id;
                        }

                        Connexion.cmd.ExecuteNonQuery();
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 1;
                    }
                    finally
                    {
                        (Connexion.con).Close();
                    }
                }




            }



            public List<dynamic> All()
            {
                List<dynamic> results = new List<dynamic>();
                sql = "select * from  " + this.GetType().Name + " ";

                try
                {
                    Connexion.Connect();
                    Connexion.cmd = (Connexion.con).CreateCommand();
                    (Connexion.cmd).CommandText = sql;
                    (Connexion.cmd).ExecuteNonQuery();
                    if (Connexion.con is MySqlConnection)
                    {
                        reader = (MySqlDataReader)(Connexion.cmd).ExecuteReader();
                    }

                    else if (Connexion.con is OracleConnection)
                    {
                        reader = (OracleDataReader)(Connexion.cmd).ExecuteReader();
                    }


                    int k = this.GetType().GetProperties().Length;

                    while (reader.Read())
                    {

                            Dictionary<string, object> dico = new Dictionary<string, object>();
                            for (int j = 0; j < k; j++)
                            {
                                dico.Add(reader.GetName(j), reader.GetValue(j));

                            }

                            results.Add(DictionaryToObject(dico));

                    }




                    reader.Close();

                Console.WriteLine("returned lines ALL : " + results.Count);

                return results;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return null;
                }
                finally
                {
                    (Connexion.con).Close();
                }
                Console.ReadLine();
            }
            public static List<dynamic> All<T>()
            {
                List<dynamic> results = new List<dynamic>();
                string tableName = typeof(T).Name;
                string sql = "select * from " + tableName + " ";

                try
                {
                    Connexion.Connect();
                    Connexion.cmd = (Connexion.con).CreateCommand();
                    (Connexion.cmd).CommandText = sql;
                    (Connexion.cmd).ExecuteNonQuery();
                    if (Connexion.con is MySqlConnection)
                    {
                        MySqlDataReader reader = (MySqlDataReader)(Connexion.cmd).ExecuteReader();
                        int k = typeof(T).GetProperties().Length;

                        while (reader.Read())
                        {


                                Dictionary<string, object> dico = new Dictionary<string, object>();
                                for (int j = 0; j < k; j++)
                                {
                                    dico.Add(reader.GetName(j), reader.GetValue(j));

                                }

                                results.Add(DictionaryToObject(dico));

                            
                        }
                    Console.WriteLine("returned lines ALL generique : " + results.Count);
                        reader.Close();
                    }

                    else if (Connexion.con is OracleConnection)
                    {
                        OracleDataReader reader = (OracleDataReader)(Connexion.cmd).ExecuteReader();

                        int k = typeof(T).GetProperties().Length;

                        while (reader.Read())
                        {


                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Dictionary<string, object> dico = new Dictionary<string, object>();
                                for (int j = 0; j < k; j++)
                                {
                                    dico.Add(reader.GetName(j), reader.GetValue(j));

                                }

                                results.Add(DictionaryToObject(dico));

                            }
                        }
                        reader.Close();
                    }


                    return results;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return null;
                }
                finally
                {
                    (Connexion.con).Close();
                }

                Console.ReadLine();
            }
            public List<dynamic> Select(Dictionary<string, object> dico)
            {
                List<dynamic> results = new List<dynamic>();
                sql = "SELECT * FROM " + this.GetType().Name + " WHERE ";
                foreach (KeyValuePair<string, object> item in dico)
                {
                    sql += item.Key + "=" + item.Value + " AND ";
                }

                sql = sql.Substring(0, sql.Length - 5); // Remove the last " AND " from the query string

                try
                {
                    Connexion.Connect();
                    Connexion.cmd = (Connexion.con).CreateCommand();
                    (Connexion.cmd).CommandText = sql;
                    (Connexion.cmd).ExecuteNonQuery();
                    if (Connexion.con is MySqlConnection)
                    {

                        MySqlDataReader reader = (MySqlDataReader)Connexion.cmd.ExecuteReader();
                        int i = 0;

                        int k = this.GetType().GetProperties().Length;

                        while (reader.Read())
                        {

                            if (i == reader.FieldCount - 1)
                            {
                                break;
                            }
                            else
                            {
                                int j = 0;

                                Dictionary<string, object> dict = new Dictionary<string, object>();
                                while (j < k)
                                {

                                    dict.Add(reader.GetName(j), reader.GetValue(j));
                                    j++;
                                    i++;
                                }

                                results.Add(DictionaryToObject(dict));

                            }

                        }
                    }

                    else if (Connexion.con is OracleConnection)
                    {
                        OracleDataReader reader = (OracleDataReader)Connexion.cmd.ExecuteReader();
                        int i = 0;

                        int k = this.GetType().GetProperties().Length;

                        while (reader.Read())
                        {

                            if (i == reader.FieldCount - 1)
                            {
                                break;
                            }
                            else
                            {
                                int j = 0;

                                Dictionary<string, object> dict = new Dictionary<string, object>();
                                while (j < k)
                                {

                                    dict.Add(reader.GetName(j), reader.GetValue(j));
                                    j++;
                                    i++;
                                }

                                results.Add(DictionaryToObject(dict));

                            }

                        }
                    }

                    return results;
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return null;
                }
                finally
                {
                    (Connexion.con).Close();
                }
                Console.ReadLine();
            }
            public static List<dynamic> Select<T>(Dictionary<string, object> dico)

            {
                List<dynamic> results = new List<dynamic>();
                string tableName = typeof(T).Name;
                string sql = "SELECT * FROM " + tableName + " WHERE ";
                foreach (KeyValuePair<string, object> item in dico)
                {
                    sql += item.Key + "=" + item.Value + " AND ";
                }

                sql = sql.Substring(0, sql.Length - 5); // Remove the last " AND " from the query string

                try
                {
                    Connexion.Connect();
                    Connexion.cmd = (Connexion.con).CreateCommand();
                    (Connexion.cmd).CommandText = sql;
                    (Connexion.cmd).ExecuteNonQuery();
                    if (Connexion.con is MySqlConnection)
                    {
                        MySqlDataReader reader = (MySqlDataReader)(Connexion.cmd).ExecuteReader();
                        int i = 0;

                        int k = typeof(T).GetProperties().Length;

                        while (reader.Read())
                        {

                            if (i == reader.FieldCount - 1)
                            {
                                break;
                            }
                            else
                            {
                                int j = 0;

                                Dictionary<string, object> dict = new Dictionary<string, object>();
                                while (j < k)
                                {

                                    dict.Add(reader.GetName(j), reader.GetValue(j));
                                    j++;
                                    i++;
                                }

                                results.Add(DictionaryToObject(dict));

                            }


                        }
                    }

                    else if (Connexion.con is OracleConnection)
                    {
                        OracleDataReader reader = (OracleDataReader)(Connexion.cmd).ExecuteReader();
                        int i = 0;

                        int k = typeof(T).GetProperties().Length;

                        while (reader.Read())
                        {

                            if (i == reader.FieldCount - 1)
                            {
                                break;
                            }
                            else
                            {
                                int j = 0;

                                Dictionary<string, object> dict = new Dictionary<string, object>();
                                while (j < k)
                                {

                                    dict.Add(reader.GetName(j), reader.GetValue(j));
                                    j++;
                                    i++;
                                }

                                results.Add(DictionaryToObject(dict));

                            }


                        }
                    }
                    return results;
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    return null;
                }
                finally
                {
                    (Connexion.con).Close();
                }
                Console.ReadLine();
            }



        public int update(string column, Object val)
        {
            if(val is string)
            {
                sql = "update " + this.GetType().Name + " set " + column + " = '" + val + "' where id = " + this.id;
            }
            else
            {
                sql = "update " + this.GetType().Name + " set " + column + " =  " + val + " where id = " + this.id;
            }
                
            Console.WriteLine(sql);
                try
                {
                    Connexion.Connect();
                    Connexion.cmd = (Connexion.con).CreateCommand();
                    (Connexion.cmd).CommandText = sql;
                    (Connexion.cmd).ExecuteNonQuery();
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 1;
                }
                finally
                {
                    (Connexion.con).Close();
                }
            }

           




        }


    }

    


