using System;
using System.Collections.Generic;
using System.Text;

namespace Progra1Bot.Clases.base2
{
    class Clsgenerartoken
    {
        public string generaTokenAlfaNumerico(int LongitudToken=5)
        {
            //instanciamos el objeto random con una variable llamada aleatorio.
            Random aleatorio = new Random();
            // declaramos un string llamado abcdario con todas las letras del abecedario
            String abcdario = "abcdefghijklmnñopqrstuvwxyz0123456789";
            //String abcdario = "abcdef";
            // declaramos la longitud del token a generar
            int longitudToken = 5;

            //declaramos la variable token de tipo string que contiene el token
            String token = "";
            // hacemos un ciclo de 0 al tamano del token que queremos generar.
            for (int i = 0; i < longitudToken; i++)
            {
                int a = aleatorio.Next(abcdario.Length); // pregunta, porque 26?
                token = token + abcdario[a]; // String es un arreglo de tipo char, por lo que podemos acceder aun valor por ubicacion
            }
            return token;
        }
    }
}
