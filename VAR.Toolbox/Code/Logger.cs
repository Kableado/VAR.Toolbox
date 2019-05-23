using System;
using System.IO;

namespace VAR.Toolbox.Code
{
    public class Logger
    {
        /// <summary>
        /// Obtiene el StreamWritter de salida
        /// </summary>
        /// <returns></returns>
        private static StreamWriter GetOutputStreamWritter()
        {
            try
            {
                string location = System.Reflection.Assembly.GetEntryAssembly().Location;
                string path = Path.GetDirectoryName(location);
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(location);

                string fileOut = string.Format("{0}/{1}.{2}.txt", path, filenameWithoutExtension,
                                               DateTime.UtcNow.ToString("yyyy-MM"));
                return File.AppendText(fileOut);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Cierra el StreamWritter de salida
        /// </summary>
        /// <param name="stream">The stream.</param>
        private static void CloseOutputStreamWritter(StreamWriter stream)
        {
            if (stream != null)
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Escribe una linea. En el stream y en la consola
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="line">The line.</param>
        private static void WriteLine(StreamWriter stream, string line)
        {
            if (stream != null)
            {
                stream.WriteLine(line);
            }
            Console.Out.WriteLine(line);
        }

        /// <summary>
        /// Logea el marcador
        /// </summary>
        /// <param name="text">The text.</param>
        public static void Marker(string text)
        {
            try
            {
                StreamWriter outStream = GetOutputStreamWritter();
                WriteLine(outStream, string.Empty);
                WriteLine(outStream, string.Format("---------------------------- {0} -----------------------", text));
                WriteLine(outStream, string.Format("\\- Date: {0}", DateTime.UtcNow.ToString("s")));
                WriteLine(outStream, string.Empty);
                CloseOutputStreamWritter(outStream);
            }
            catch (Exception) { /* Nom Nom Nom */}
        }

        /// <summary>
        /// Logea el texto especificado
        /// </summary>
        /// <param name="text">The text.</param>
        public static void Log(string text)
        {
            try
            {
                StreamWriter outStream = GetOutputStreamWritter();
                WriteLine(outStream, string.Format("{0} -- {1}", DateTime.UtcNow.ToString("s"), text));
                CloseOutputStreamWritter(outStream);
            }
            catch (Exception) { /* Nom Nom Nom */}
        }


        /// <summary>
        /// Logea una excepcion
        /// </summary>
        /// <param name="ex">The Exception.</param>
        public static void Log(Exception ex)
        {
            try
            {
                StreamWriter outStream = GetOutputStreamWritter();
                WriteLine(outStream, string.Empty);
                WriteLine(outStream, string.Format("!!!!!!!!!!!!!!!!!!!!!!!!!!!! {0} !!!!!!!!!!!!!!!!!!!!!!!", "Exception"));
                WriteLine(outStream, string.Format("\\- Date: {0}", DateTime.UtcNow.ToString("s")));
                WriteLine(outStream, string.Empty);
                Exception exAux = ex;
                while (exAux != null)
                {
                    WriteLine(outStream, string.Format("Message: {0}", exAux.Message));
                    WriteLine(outStream, string.Format("Stacktrace: {0}", exAux.StackTrace));
                    exAux = exAux.InnerException;
                }
                CloseOutputStreamWritter(outStream);
            }
            catch (Exception) { /* Nom Nom Nom */}
        }

    }
}