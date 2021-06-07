using Progra1Bot.Clases.Alumnos;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;
using System.Collections.Generic;
using Progra1Bot.Clases.emojis;
using System.Text.RegularExpressions;
using PrimerBot.Clases.correo;
using BaseDeDatos.Clases.BaseDatos;

namespace Progra1Bot.Clases
{
    class clsBotAlumnos
    {
        private static TelegramBotClient Bot;
        private List<mdAlumnos> TodosLosAlumnos = new mdAlumnos().cargaTodosAlumnosBaseDatos();

        public async Task IniciarTelegram()
        {
            Bot = new TelegramBotClient("973975320:AAFPrCbiuLOPcYyn99Tw6wRWM1kmJvldQVk");

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;

            Bot.OnMessage += BotCuandoRecibeMensajes;
            Bot.OnMessageEdited += BotCuandoRecibeMensajes;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"escuchando solicitudes del BOT @{me.Username}");



            Console.ReadLine();
            Bot.StopReceiving();
        }











        // cuando recibe mensajes
        private  async void BotCuandoRecibeMensajes(object sender, MessageEventArgs messageEventArgumentos)
        {
            var ObjetoMensajeTelegram = messageEventArgumentos;
            var mensajes = ObjetoMensajeTelegram.Message;
            string mensajeEntrante = mensajes.Text.ToLower();
            string Telegram_id_manda_mensaje = mensajes.Chat.Id.ToString();
            string respuesta = "No te entiendo";



            // si el mensaje viene nulo, lo retorna
            if (mensajes.Text == null)
            {
                return;
            }

            Console.WriteLine($"Recibiendo Mensaje del ID {Telegram_id_manda_mensaje}.");
            Console.WriteLine($"Dice {mensajeEntrante}.");




            // busca entre todos los alumnos si existe el idbot que escribe, todos por defecto tienen 0
            mdAlumnos alumnoEscribe = TodosLosAlumnos.Find(x => x.idbot.Equals(Telegram_id_manda_mensaje));
            TodosLosAlumnos = new mdAlumnos().cargaTodosAlumnosBaseDatos();

            if (mensajeEntrante.Contains("hola"))
            {

                respuesta = emojis.mdEmoji.telefono + " Hola mi pequeño Crack  " + alumnoEscribe.nombre +" "+ alumnoEscribe.apellido+"   Bienvenido A La Tienda de Tecnologia Mas Grande de Guatemala";
                respuesta  += "\n\nESTOS SON LOS PRODUCTOS QUE TENGO PARA TI";
                respuesta += "\n\n1. SAMSUNG GALAXY S10";
                respuesta += "\n\n2. HUAWEI P30 PRO";
                respuesta += "\n\n3. HUAWEI P40 PRO";
                respuesta += "\n\n4. IPHONE XS Max";
                respuesta += "\n\n5. IPHONE 11 PRO MAX";
                respuesta += "\n\n6. AIRPORDS-APPLE";
                respuesta += "\n\n7. HUAWEI WHATCH GT2";
                respuesta += "\n\n8. APPLE WHATCH";
                respuesta += "\n\nPara Eleguir El Que Más Te guste Solo Digitaliza El Numero";
               
            }

           
            // SI EL OBJETO ES NULO ES PORQUE NO EXITE EN LA BASE DE DATOS, ES DECIR, TIENE 0 EN IDBOT PORQUE NO LO ENCONTRO
            if (alumnoEscribe == null)
            {
                respuesta = mdEmoji.saludo + "  Hola, pequeño padawan!!";
                respuesta += " por motivos de seguridad y confidencialidad quiero saber si eres parte de este selecto grupo\n";
                respuesta += "Para que pueda verificar si eres parte de las futuras RockStar de Guatemala, escribe la palabra *carnet* y tu carnet solo apartir del año e incluyendo el guion \n";
                respuesta += "Ejemplo:\n *carnet* 19-xxxx ";
                respuesta += "si eres el jedi (ingeniero) solo pon carnet 2020";
            }
           
            if (mensajes == null || mensajes.Type != MessageType.Text) return;

            if (mensajeEntrante.Contains("papá")  || mensajeEntrante.Contains("padre"))
            {
                if (Telegram_id_manda_mensaje.Equals("1045892569"))
                {
                    respuesta = " Ernesto!! tu eres mi Padre!!!";
                }
                else
                {
                    respuesta = "Mi papa se llama Ernesto";
                }

            }

            //verifica  el codigo de registro que se mandó
            if (mensajeEntrante.ToLower().StartsWith("verificar"))
            {
                Match m = Regex.Match(mensajeEntrante, "(\\d+)");
                string num = string.Empty;

                if (m.Success)
                {
                    num = m.Value;
                }
                else { num = "0"; }

                mdAlumnos alumnoVerificar = TodosLosAlumnos.Find(x => x.idbot.Equals("P" + num));
                if (alumnoVerificar != null)
                {
                    alumnoVerificar.actualizaInicio(alumnoVerificar, Telegram_id_manda_mensaje, true);
                    //resp = mdEmoji.caralentesoscuros+" Bienvenido!!!! ya eres de mi confianza!!!";
                    respuesta = "Ya eres parte de este selecto grupo, ganaste mi confianza!!\n";

                }
                else
                {
                    respuesta = "Lo siento, el códido no es el que te mandé, preguntale al creador de este bot!!";
                }
            }


            //|| mensajeEntrante.StartsWith("carné")

            //si la palabra es correo, se le envia el correo de verificaion
            if (mensajeEntrante.StartsWith("carnet"))
            {
                
                ClsConexion basedato = new ClsConexion();

                string correoAlumno = mensajeEntrante.Replace("carnet", "").Trim();//en correoAlumno ahora es igual a la info del carnet
                mdAlumnos alumno = TodosLosAlumnos.Find(x => x.carnet.Equals(correoAlumno));

                if (alumno == null)
                {
                    respuesta = "El carnet que ingresaste no aparece en la base de datos de Padawans y Jedi";
                }
                else
                {
                    if (!correoAlumno.ToLower().EndsWith("@miumg.edu.gt"))
                    {
                        respuesta = "Lo lamento, el correo:\n *" + correoAlumno + "* que tengo registrado en la base de datos,no pertenece a  *UMG*,\n Qué puede hacer? \n Estudie en la UMG, especificamente en la Facultad de Ingenieria en Sistemas\n Gracias!";
                          
                    }


                    if (new clsMailBussiness().CorreoVerificacionInicial(alumno).Equals("OK"))
                    {
                        respuesta = "Hola, si tu eres *" + alumno.nombre +" "+ alumno.apellido+ "*, ahorita te mandé un correo  " + mdEmoji.correo + " de verificación a " + alumno.correo + " \n con instrucciones para registrarte " + mdEmoji.smile;
                    }
                    else
                    {
                        respuesta = "Lo lamento hubo un inconveniente al envíar el correo de verificación";
                    }

                    TodosLosAlumnos = new mdAlumnos().cargaTodosAlumnosBaseDatos(); //recarga la info de alumnos


                }

            }

            if (mensajeEntrante.Contains("archivos"))
            {
                respuesta = new ClsManejoArchivos().ListaArchivos("C:\\tmp");
            }


            if (respuesta.Equals("No te entiendo") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre +" "+ alumnoEscribe.apellido +" no entiendo lo que me quieres decir "+mdEmoji.chinoEscritura+ mdEmoji.chinoEscritura + mdEmoji.chinoEscritura + mdEmoji.chinoEscritura + mdEmoji.chinoEscritura;
            }

            mdAlumnos alumnoescribiendo = new mdAlumnos();

            if (mensajes.Text.ToLower().Contains("hora") && alumnoEscribe !=null)
            {
                DateTime fecha = DateTime.Now;
                respuesta = "Pues fijate, "+ alumnoEscribe.nombre + alumnoEscribe.apellido +" que ahorita son las " + fecha.Hour + " con " + fecha.Minute;
              
            }

            if (mensajes.Text.ToLower().Contains("1") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + alumnoEscribe.apellido + "\tUsted a elegido, SAMSUNG GALAXY S10 ";
                respuesta += "\nEl Precio Es de Q8,600.00";
                respuesta += "\nDeseas Compralo";

            }
            if (mensajes.Text.ToLower().Contains("si") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " " + alumnoEscribe.apellido + "\tMuy Buena Elección Crack";
                respuesta += "\n\nDigitaliza La siguiente Información Para completar Tu Pedido";
                respuesta += "\nDigitaliza el Nombre de la persona que resivira el producto, Seguido de la Palabra Nombre, Ejemplo Nombre Juan";  
            }
            if (mensajes.Text.ToLower().Contains("nombre") && alumnoEscribe != null)
            {
                respuesta = "\tahora Digitaliza el Apellido de la persona que resivira el producto,seguido de la palabra Apellido, ejemplo Apelldio Galicia";
            }

            if (mensajes.Text.ToLower().Contains("apellido") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu direccion Para poder Enviar Tu producto,seguido de la palabra Direccion,Ejemplo Direccion Guatemala";
            }
            if (mensajes.Text.ToLower().Contains("direccion") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu correo electronico,seguido de la palabra correo, ejemplo Correo juangalicia@miumg.edu.gt";
            }

            if (mensajeEntrante.StartsWith("correo") && alumnoEscribe != null)
            {
                string correo = mensajeEntrante.Replace("correo", "").Trim();
                clsMailBussiness corr = new clsMailBussiness();
                respuesta = "Gracias Por Realizar Tu Compra\n\n Tu Producto Llegara de 1 a 2 Dias\n\n\n#QUEDATE EN CASA...";

            }

            if (mensajes.Text.ToLower().Contains("2") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + alumnoEscribe.apellido + "\tUsted a elegido, HUAWEI P30 PRO ";
                respuesta += "\nEl Precio Es de Q7,300.00";
                respuesta += "\nDeseas Compralo";
            }
            if (mensajes.Text.ToLower().Contains("si") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " " + alumnoEscribe.apellido + "\tMuy Buena Elección Crack";
                respuesta += "\n\nDigitaliza La siguiente Información Para completar Tu Pedido";
                respuesta += "\nDigitaliza el Nombre de la persona que resivira el producto, Seguido de la Palabra Nombre, Ejemplo Nombre Juan";
            }
            if (mensajes.Text.ToLower().Contains("nombre") && alumnoEscribe != null)
            {
                respuesta = "\tahora Digitaliza el Apellido de la persona que resivira el producto,seguido de la palabra Apellido, ejemplo Apelldio Galicia";
            }

            if (mensajes.Text.ToLower().Contains("apellido") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu direccion Para poder Enviar Tu producto,seguido de la palabra Direccion,Ejemplo Direccion Guatemala";
            }
            if (mensajes.Text.ToLower().Contains("direccion") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu correo electronico,seguido de la palabra correo, ejemplo Correo juangalicia@miumg.edu.gt";
            }

            if (mensajeEntrante.StartsWith("correo") && alumnoEscribe != null)
            {
                string correo = mensajeEntrante.Replace("correo", "").Trim();
                clsMailBussiness corr = new clsMailBussiness();
                respuesta = "Gracias Por Realizar Tu Compra\n\n Tu Producto Llegara de 1 a 2 Dias\n\n\n#QUEDATE EN CASA...";

            }

            if (mensajes.Text.ToLower().Contains("3") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + alumnoEscribe.apellido + "\tUsted a elegido,Huawei P40 PRO ";
                respuesta += "\nEl Precio Es de Q10,600.00";
                respuesta += "\nDeseas Compralo";

            
        }
            if (mensajes.Text.ToLower().Contains("si") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " " + alumnoEscribe.apellido + "\tMuy Buena Elección Crack";
                respuesta += "\n\nDigitaliza La siguiente Información Para completar Tu Pedido";
                respuesta += "\nDigitaliza el Nombre de la persona que resivira el producto, Seguido de la Palabra Nombre, Ejemplo Nombre Juan";  
            }
            if (mensajes.Text.ToLower().Contains("nombre") && alumnoEscribe != null)
            {
                respuesta = "\tahora Digitaliza el Apellido de la persona que resivira el producto,seguido de la palabra Apellido, ejemplo Apelldio Galicia";
            }

            if (mensajes.Text.ToLower().Contains("apellido") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu direccion Para poder Enviar Tu producto,seguido de la palabra Direccion,Ejemplo Direccion Guatemala";
            }
            if (mensajes.Text.ToLower().Contains("direccion") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu correo electronico,seguido de la palabra correo, ejemplo Correo juangalicia@miumg.edu.gt";
            }

            if (mensajeEntrante.StartsWith("correo") && alumnoEscribe != null)
            {
                string correo = mensajeEntrante.Replace("correo", "").Trim();
            clsMailBussiness corr = new clsMailBussiness();
              respuesta = "Gracias Por Realizar Tu Compra\n\n Tu Producto Llegara de 1 a 2 Dias\n\n\n#QUEDATE EN CASA...";

            }
            if (mensajes.Text.ToLower().Contains("4") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + alumnoEscribe.apellido + "\tUsted a elegido, IPHONE XS MAX ";
                respuesta += "\nEl Precio Es de Q10,800.00";
                respuesta += "\nDeseas Compralo";

            }
            if (mensajes.Text.ToLower().Contains("si") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " " + alumnoEscribe.apellido + "\tMuy Buena Elección Crack";
                respuesta += "\n\nDigitaliza La siguiente Información Para completar Tu Pedido";
                respuesta += "\nDigitaliza el Nombre de la persona que resivira el producto, Seguido de la Palabra Nombre, Ejemplo Nombre Juan";
            }
            if (mensajes.Text.ToLower().Contains("nombre") && alumnoEscribe != null)
            {
                respuesta = "\tahora Digitaliza el Apellido de la persona que resivira el producto,seguido de la palabra Apellido, ejemplo Apelldio Galicia";
            }

            if (mensajes.Text.ToLower().Contains("apellido") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu direccion Para poder Enviar Tu producto,seguido de la palabra Direccion,Ejemplo Direccion Guatemala";
            }
            if (mensajes.Text.ToLower().Contains("direccion") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu correo electronico,seguido de la palabra correo, ejemplo Correo juangalicia@miumg.edu.gt";
            }

            if (mensajeEntrante.StartsWith("correo") && alumnoEscribe != null)

            {
                
                string correo = mensajeEntrante.Replace("correo", "").Trim();
                clsMailBussiness corr = new clsMailBussiness();
                respuesta = "Gracias Por Realizar Tu Compra\n\n Tu Producto Llegara de 1 a 2 Dias\n\n\n#QUEDATE EN CASA...";

            }
            if (mensajes.Text.ToLower().Contains("5") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + alumnoEscribe.apellido + "\tUsted a elegido, IPHONE 11 PRO MAX";
                respuesta += "\nEl Precio Es de Q12,600.00";
                respuesta += "\nDeseas Compralo";

            }
            if (mensajes.Text.ToLower().Contains("si") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " " + alumnoEscribe.apellido + "\tMuy Buena Elección Crack";
                respuesta += "\n\nDigitaliza La siguiente Información Para completar Tu Pedido";
                respuesta += "\nDigitaliza el Nombre de la persona que resivira el producto, Seguido de la Palabra Nombre, Ejemplo Nombre Juan";
            }
            if (mensajes.Text.ToLower().Contains("nombre") && alumnoEscribe != null)
            {
                respuesta = "\tahora Digitaliza el Apellido de la persona que resivira el producto,seguido de la palabra Apellido, ejemplo Apelldio Galicia";
            }

            if (mensajes.Text.ToLower().Contains("apellido") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu direccion Para poder Enviar Tu producto,seguido de la palabra Direccion,Ejemplo Direccion Guatemala";
            }
            if (mensajes.Text.ToLower().Contains("direccion") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu correo electronico,seguido de la palabra correo, ejemplo Correo juangalicia@miumg.edu.gt";
            }

            if (mensajeEntrante.StartsWith("correo") && alumnoEscribe != null)
            {
                string correo = mensajeEntrante.Replace("correo", "").Trim();
                clsMailBussiness corr = new clsMailBussiness();
                respuesta = "Gracias Por Realizar Tu Compra\n\n Tu Producto Llegara de 1 a 2 Dias\n\n\n#QUEDATE EN CASA...";

            
        }
            if (mensajes.Text.ToLower().Contains("6") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + alumnoEscribe.apellido + "\tUsted a elegido,AIRPORD-APPLE";
                respuesta += "\nEl Precio Es de Q2,600.00";
                respuesta += "\nDeseas Compralo";

            }
            if (mensajes.Text.ToLower().Contains("si") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " " + alumnoEscribe.apellido + "\tMuy Buena Elección Crack";
                respuesta += "\n\nDigitaliza La siguiente Información Para completar Tu Pedido";
                respuesta += "\nDigitaliza el Nombre de la persona que resivira el producto, Seguido de la Palabra Nombre, Ejemplo Nombre Juan";
            }
            if (mensajes.Text.ToLower().Contains("nombre") && alumnoEscribe != null)
            {
                respuesta = "\tahora Digitaliza el Apellido de la persona que resivira el producto,seguido de la palabra Apellido, ejemplo Apelldio Galicia";
            }

            if (mensajes.Text.ToLower().Contains("apellido") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu direccion Para poder Enviar Tu producto,seguido de la palabra Direccion,Ejemplo Direccion Guatemala";
            }
            if (mensajes.Text.ToLower().Contains("direccion") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu correo electronico,seguido de la palabra correo, ejemplo Correo juangalicia@miumg.edu.gt";
            }

            if (mensajeEntrante.StartsWith("correo") && alumnoEscribe != null)
            {
                string correo = mensajeEntrante.Replace("correo", "").Trim();
                clsMailBussiness corr = new clsMailBussiness();
                respuesta = "Gracias Por Realizar Tu Compra\n\n Tu Producto Llegara de 1 a 2 Dias\n\n\n#QUEDATE EN CASA...";

            }
            if (mensajes.Text.ToLower().Contains("7") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + alumnoEscribe.apellido + "\tUsted a elegido,HUAWEI WHATCH GT2";
                respuesta += "\nEl Precio Es de Q2,800.00";
                respuesta += "\nDeseas Compralo";
            }
            if (mensajes.Text.ToLower().Contains("si") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " " + alumnoEscribe.apellido + "\tMuy Buena Elección Crack";
                respuesta += "\n\nDigitaliza La siguiente Información Para completar Tu Pedido";
                respuesta += "\nDigitaliza el Nombre de la persona que resivira el producto, Seguido de la Palabra Nombre, Ejemplo Nombre Juan";
            }
            if (mensajes.Text.ToLower().Contains("nombre") && alumnoEscribe != null)
            {
                respuesta = "\tahora Digitaliza el Apellido de la persona que resivira el producto,seguido de la palabra Apellido, ejemplo Apelldio Galicia";
            }

            if (mensajes.Text.ToLower().Contains("apellido") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu direccion Para poder Enviar Tu producto,seguido de la palabra Direccion,Ejemplo Direccion Guatemala";
            }
            if (mensajes.Text.ToLower().Contains("direccion") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu correo electronico,seguido de la palabra correo, ejemplo Correo juangalicia@miumg.edu.gt";
            }

            if (mensajeEntrante.StartsWith("correo") && alumnoEscribe != null)
            {
                string correo = mensajeEntrante.Replace("correo", "").Trim();
                clsMailBussiness corr = new clsMailBussiness();
                respuesta = "Gracias Por Realizar Tu Compra\n\n Tu Producto Llegara de 1 a 2 Dias\n\n\n#QUEDATE EN CASA...";

            }
            if (mensajes.Text.ToLower().Contains("8") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + alumnoEscribe.apellido + "\tUsted a elegido,APPLE WHATCH";
                respuesta += "\nEl Precio Es de Q3,100.00";
                respuesta += "\nDeseas Compralo";

            }
            if (mensajes.Text.ToLower().Contains("si") && alumnoEscribe != null)
            {
                respuesta = alumnoEscribe.nombre + " " + alumnoEscribe.apellido + "\tMuy Buena Elección Crack";
                respuesta += "\n\nDigitaliza La siguiente Información Para completar Tu Pedido";
                respuesta += "\nDigitaliza el Nombre de la persona que resivira el producto, Seguido de la Palabra Nombre, Ejemplo Nombre Juan";
            }
            if (mensajes.Text.ToLower().Contains("nombre") && alumnoEscribe != null)
            {
                respuesta = "\tahora Digitaliza el Apellido de la persona que resivira el producto,seguido de la palabra Apellido, ejemplo Apelldio Galicia";
            }

            if (mensajes.Text.ToLower().Contains("apellido") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu direccion Para poder Enviar Tu producto,seguido de la palabra Direccion,Ejemplo Direccion Guatemala";
            }
            if (mensajes.Text.ToLower().Contains("direccion") && alumnoEscribe != null)
            {
                respuesta = "\nahora digitaliza tu correo electronico,seguido de la palabra correo, ejemplo Correo juangalicia@miumg.edu.gt";
            }

            if (mensajeEntrante.StartsWith("correo") && alumnoEscribe != null)
                
            {
                
                string correo = mensajeEntrante.Replace("correo", "").Trim();
                clsMailBussiness corr = new clsMailBussiness();
                respuesta = "Gracias Por Realizar Tu Compra\n\n Tu Producto Llegara de 1 a 2 Dias\n\n\n#QUEDATE EN CASA...";

            }

            // envia la respuesta del diaglo
            if (!String.IsNullOrEmpty(respuesta))//    
            {
                await Bot.SendTextMessageAsync(
                    chatId: ObjetoMensajeTelegram.Message.Chat,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    text: respuesta

            );
            }

        } // fin del metodo de recepcion de mensajes




        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("UPS!!! Recibo un error!!!: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }

    }//fin clase
}
