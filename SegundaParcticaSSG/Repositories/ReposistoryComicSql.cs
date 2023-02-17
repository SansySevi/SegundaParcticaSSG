using SegundaParcticaSSG.Models;
using System.Data;
using System.Data.SqlClient;

#region PROCEDIMIENTOS

//CREATE PROCEDURE SP_INSERT_COMIC(@NOMBRE NVARCHAR(20), @IMAGEN NVARCHAR(100), @DESCRIPCION NVARCHAR(50))
//AS
// DECLARE @ID INT
// SELECT @ID = MAX(IDCOMIC)+1 FROM COMICS
// INSERT INTO COMICS VALUES(@ID, @NOMBRE, @IMAGEN, @DESCRIPCION)
//GO

#endregion

namespace SegundaParcticaSSG.Repositories
{
    public class ReposistoryComicSql : IRepository
    {

        SqlConnection cn;
        SqlCommand com;
        SqlDataAdapter adapter;
        private DataTable tableComics;

        public ReposistoryComicSql()
        {
            string connectionString = @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2022";
            string sql = "SELECT * FROM COMICS";

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

            this.adapter = new SqlDataAdapter(sql, connectionString);
            this.tableComics = new DataTable();
            adapter.Fill(this.tableComics);
        }

        //METODO PARA INSERTAR UN COMIC
        public void CreateComic(string nombre, string imagen, string descripcion)
        {
            SqlParameter pamnom = new SqlParameter("@NOMBRE", nombre);
            SqlParameter pamimg = new SqlParameter("@IMAGEN", imagen);
            SqlParameter pamdesc = new SqlParameter("@DESCRIPCION", descripcion);

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

        //METODO PARA GENERAR UNA LISTA DE COMIC
        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tableComics.AsEnumerable()
                           select datos;

            List<Comic> comics = new List<Comic>();

            //RECORREMOS RESULTADOS DE LA CONSULTA, GENERAMOS LOS OBJETOS COMIC Y AÑADIMOS A LISTA
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
