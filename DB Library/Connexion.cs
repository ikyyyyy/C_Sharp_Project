using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DB
{
    public class Connexion
    {

            static public IDbConnection con = null;
            static public IDbCommand cmd = null;


            public static void Connect()
            {
                XDocument doc = XDocument.Load(@"<PATH TO DB_CONFIGURATION_FILE>");
                XElement root = doc.Root;


                var elements = root.Elements("SGBD");

                if (elements != null)
                {
                    foreach (XElement elem in doc.Descendants("SGBD"))
                    {

                        try
                        {
                            try
                            {
                                cmd = new OracleCommand();
                                string conStringOracle = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" +
                                elem.Element("sgbd").Element("DB_HOST").Value + ")(PORT=" +
                                elem.Element("sgbd").Element("DB_PORT").Value + ")))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" +
                                elem.Element("sgbd").Element("DB_DATABASE").Value + ")));User Id=" +
                                elem.Element("sgbd").Element("DB_USERNAME").Value + ";Password=" +
                                elem.Element("sgbd").Element("DB_PASSWORD").Value + ";";
                                con = new OracleConnection(conStringOracle);
                                con.Open();
                                cmd.Connection = con;
                            }
                            catch (OracleException e)
                            {

                                con.Close();
                                string conStringMysql = "server=" + elem.Element("sgbd").Element("DB_HOST").Value +
                                    ";user id=" + elem.Element("sgbd").Element("DB_USERNAME").Value +
                                    "; database= " + elem.Element("sgbd").Element("DB_DATABASE").Value +
                                    ";password=" + elem.Element("sgbd").Element("DB_PASSWORD").Value + ";";
                                con = new MySqlConnection(conStringMysql);
                                cmd = new MySqlCommand();
                                con.Open();
                                cmd.Connection = con;
                            }
                        }
                        catch (MySqlException e)
                        {




                        }

                        Console.WriteLine(elem.Element("sgbd").Element("DB_CONNECTION").Value + ": Connection etablie...");
                    }
                }
                else
                {
                    Console.WriteLine("empty file...");
                }
            }



            public static int IUD(string req)
            {
                cmd.CommandText = req;
                return cmd.ExecuteNonQuery();
            }

            public static IDataReader Select(string req)
            {
                cmd.CommandText = req;
                return (IDataReader)cmd.ExecuteReader();
            }

            public static Dictionary<string, string> GetChamps_table(string table)
            {
                Dictionary<string, string> champs = new Dictionary<string, string>();
                string query = null;

                if (string.Compare(Connexion.con.ToString(), "MySql.Data.MySqlClient.MySqlConnection") == 0)
                {
                    query = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + table + "' ";
                }
                else if (string.Compare(Connexion.con.ToString(), "Oracle.ManagedDataAccess.Client.OracleConnection") == 0)
                {
                    query = "SELECT COLUMN_NAME, DATA_TYPE FROM USER_TAB_COLS WHERE TABLE_NAME = '" + table + "';";
                }

                IDataReader dataR = Select(query);
                while (dataR.Read())
                {
                    champs.Add(dataR.GetString(0), dataR.GetString(1));
                }
                dataR.Close();

                return champs;
            }


        }
    }

