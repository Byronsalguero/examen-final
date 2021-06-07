
using Progra1Bot.Clases.Alumnos;
using Progra1Bot.Clases.base2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimerBot.Clases.correo
{
    class clsMailBussiness
    {

        /// <summary>
        /// envia correo para verificar a la persona

        /// </summary>
        /// <param name="alumnoObj"></param>
        public String CorreoVerificacionInicial(mdAlumnos alumnoObj)
        {

            //String ret = "OK";
            Clsgenerartoken clave1 = new Clsgenerartoken();
            Random rnd = new Random();
            clsCorreo cor = new clsCorreo();
            mdCorreoParametro par = new mdCorreoParametro();
            string clave;
            clave = Convert.ToString(rnd.Next(10000, 50000));

            par.CORREODESTINO = alumnoObj.correo;
            par.AsuntoCorreo = "Verificacion de identidad";
            par.Cuerpo = "Hola, este correo es para verificar tu identidad para usar este Bot!\n";
            par.Cuerpo += "usando telegram manda la palabra verificar seguido de " + clave;
            par.Cuerpo += " \nEjemplo:\n verificar 12345";
            par.Cuerpo += " \nEres un RockStar de la Progra!!!";

            alumnoObj.actualizaInicio(alumnoObj, clave, false);

            return cor.enviarCorreoHotMail(par);
        }


        public string CorreoverificacionInicial(string correo)
        {
            mdAlumnos cort = new mdAlumnos();
            clsCorreo cor = new clsCorreo();
            mdCorreoParametro par = new mdCorreoParametro();
            par.CORREODESTINO = correo;
            par.AsuntoCorreo = "Tienda de Tecnologia";
            par.Cuerpo = "Hola Gracias por realizar tu Compra Tu Producto sera entregado de 1 a 2 dias";
          

           

            return cor.enviarCorreoHotMail(par);

        }

    }
    
}
