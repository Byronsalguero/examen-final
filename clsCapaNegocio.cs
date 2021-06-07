using BaseDeDatos.Clases.BaseDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Progra1Bot.Clases.Alumnos
{
    class clsCapaNegocio
    {

        public clsCapaNegocio()
        {
                
        }


        public mdAlumnos LocalizaAlumnoPorMail(String carnet,String IDUsuarioTelegram)
        {
            mdAlumnos AlumnoEncontrado = new mdAlumnos();
            AlumnoEncontrado.nombre = "no encontrado";
            int linea = 1;
            int linea_encontrada = 0;

            var TodosLosAlunnos = new mdAlumnos().cargaDatos("c:\\tmp\\alumno.xlsx","T");
            ClsManejoArchivos ClaseArchivos = new ClsManejoArchivos();
            ClsConexion conect = new ClsConexion();
           
            foreach (mdAlumnos item in TodosLosAlunnos)
            {
                //conect.abrirConexion();
                string sql = "INSERT INTO [alumnos] values ('" + item.nombre + "','" + item.apellido + "','"+item.correo+"','"+item.carnet+"','"+item.seccion+"','"+item.idbot+"')";

                if (carnet.ToLower().Equals(item.carnet))
                {
                    linea_encontrada = linea;
                    AlumnoEncontrado = item;
                    string nuevaLinea = item.nombre + ";" + item.apellido + ";" + item.correo + ";" + item.carnet + ";" + item.seccion + ";" + IDUsuarioTelegram;
                    ClaseArchivos.CambioLinea(nuevaLinea, "c:\\tmp\\alumno.xlsx", linea_encontrada);


                }
                linea++;
            }
            return AlumnoEncontrado;
        }

        public List<mdAlumnos> CargarAlumnosBaseDatos()
        {
            ClsConexion cn = new ClsConexion();
            mdAlumnos Alumno = new mdAlumnos();
            List<mdAlumnos> TodosLosAlumnos = new List<mdAlumnos>();

            DataTable dt = cn.consultaTablaDirecta("SELECT *  FROM [alumnos]");

            foreach (DataRow dr in dt.Rows)
            {
                Alumno.nombre = dr["nombre"].ToString();
                Alumno.apellido = dr["apellido"].ToString();
                Alumno.correo = dr["correo"].ToString();
                Alumno.carnet = dr["carnet"].ToString();
                Alumno.seccion = dr["seccion"].ToString();
                Alumno.idbot = dr["idbot"].ToString();
                TodosLosAlumnos.Add(Alumno);
                Alumno = new mdAlumnos();

            }
            return TodosLosAlumnos;
        }


        public mdAlumnos EncontrarAlumnoPorMail(String correo)
        {
            ClsConexion cn = new ClsConexion();
            mdAlumnos Alumno = new mdAlumnos();
            Alumno.idbot = "NO HAY";
            DataTable dt = cn.consultaTablaDirecta("SELECT * FROM [alumnos] where correo='"+correo+"'");

            foreach (DataRow dr in dt.Rows)
            {
                Alumno.nombre = dr["nombre"].ToString();
                Alumno.apellido = dr["apellido"].ToString();
                Alumno.correo = dr["correo"].ToString();
                Alumno.carnet = dr["carnet"].ToString();
                Alumno.seccion = dr["seccion"].ToString();
                Alumno.idbot = dr["idbot"].ToString();
            }
            return Alumno;
        }


    }
}
