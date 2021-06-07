using BaseDeDatos.Clases.BaseDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Progra1Bot.Clases.Alumnos
{
    class mdAlumnos
    {


        public String apellido { get; set; }
        public String nombre { get; set; }
        public String correo { get; set; }
        public String carnet { get; set; }
        public String seccion { get; set; }
        public String idbot { get; set; }
        public List<mdAlumnos> Alumnos { get; set; }


        public mdAlumnos()
        {
            Alumnos = new List<mdAlumnos>();
        }


        public List<mdAlumnos> cargaDatos(String archivo, String Seccion="T")
        {
            return new ClsManejoArchivos().LeerArchivoCargaDatos(archivo, Seccion);
        }

        /// <summary>
        /// carga todos los alumnos de la tabla de alumnos
        /// los regresa en una estructura de datos tipo List
        /// </summary>
        /// <returns></returns>
        public List<mdAlumnos> cargaTodosAlumnosBaseDatos()
        {

            return new clsCapaNegocio().CargarAlumnosBaseDatos();
        }


        public mdAlumnos cargaAlumnoDatosBaseDatos(String correo)
        {
            clsCapaNegocio ReglaNegocio = new clsCapaNegocio();
            return ReglaNegocio.EncontrarAlumnoPorMail(correo);
        }



        public bool verifica_Alumno_Registro(String correo)
        {
            mdAlumnos alumnoBuscar = cargaAlumnoDatosBaseDatos(correo);

            if (alumnoBuscar.correo.Equals("NO HAY"))
            {
                return false;
            } else
            {
                return true;
            }


        }

        //actuliza el inicio
        public void actualizaInicio(mdAlumnos alum, string clave, bool verificado)
        {
            ClsConexion cn = new ClsConexion();
            cn.abrirConexion();
            string cuerito;
            if (verificado)
            {
                cuerito = clave.ToString();
            }
            else
            {
                cuerito = "P" + clave.ToString();
            }
            String sql;

            if (verifica_Alumno_Registro(alum.correo))
            {
                sql = "update alumnos  set  idbot='" + cuerito + "' where correo='" + alum.correo + "'";

            }
            else
            {
                sql = "update alumnos  set  idbot='" + cuerito + "' where correo='" + alum.correo+"'";
            }

            SqlCommand command = new SqlCommand(sql, cn.conexion);
            command.ExecuteNonQuery();

        }

    }// fin de la clase
}
