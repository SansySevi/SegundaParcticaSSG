using Oracle.ManagedDataAccess.Client;
using SegundaParcticaSSG.Models;
using System.Data;

#region

//CREATE OR REPLACE PROCEDURE SP_INSERT_COMIC
//(P_NOMBRE NVARCHAR2, P_IMAGEN NVARCHAR2, P_DESCRIPCION NVARCHAR2)
//AS
//BEGIN
//  INSERT INTO COMICS VALUES ((SELECT MAX(IDCOMIC)+1 FROM COMICS), P_NOMBRE, P_IMAGEN, P_DESCRIPCION);
//COMMIT;
//END;

#endregion

namespace SegundaParcticaSSG.Repositories
{
    public class RepositoryComicsOracle : IRepository
    {

        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataAdapter adapter;
        private DataTable tableComics;

        public RepositoryComicsOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521;Persist Security Info=false;User ID=SYSTEM;Password=oracle"; 
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from comics";
            this.adapter = new OracleDataAdapter(sql, connectionString);
            this.tableComics = new DataTable();
            this.adapter.Fill(this.tableComics);
        }


        public void CreateComic(string nombre, string imagen, string descripcion)
        {
            OracleParameter pamnom = new OracleParameter(":NOMBRE", nombre);
            OracleParameter pamimg = new OracleParameter(":IMAGEN", imagen);
            OracleParameter pamdesc = new OracleParameter(":DESCRIPCION", descripcion);

            this.com.Parameters.Add(pamnom);
            this.com.Parameters.Add(pamimg);
            this.com.Parameters.Add(pamdesc);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tableComics.AsEnumerable()
                           select datos;

            List<Comic> comics = new List<Comic>();

            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")

                };
                comics.Add(comic);
            }
            return comics;
        }
    }
}
