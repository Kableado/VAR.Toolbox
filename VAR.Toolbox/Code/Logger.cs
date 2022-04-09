using System;
using System.IO;

namespace VAR.Toolbox.Code
{
    public static class Logger
    {
        /// <summary>
        /// Obtiene el StreamWriter de salida
        /// </summary>
        /// <returns></returns>
        private static StreamWriter GetOutputStreamWriter()
        {
            try
            {
                string location = System.Reflection.Assembly.GetEntryAssembly()?.Location;
                string path = Path.GetDirectoryName(location);
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(location);

                string fileOut = $"{path}/{filenameWithoutExtension}.{DateTime.UtcNow:yyyy-MM}.txt";
                return File.AppendText(fileOut);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Cierra el StreamWriter de salida
        /// </summary>
        /// <param name="stream">The stream.</param>
        private static void CloseOutputStreamWriter(StreamWriter stream)
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
                StreamWriter outStream = GetOutputStreamWriter();
                WriteLine(outStream, string.Empty);
                WriteLine(outStream, $"---------------------------- {text} -----------------------");
                WriteLine(outStream, $"\\- Date: {DateTime.UtcNow:s}");
                WriteLine(outStream, string.Empty);
                CloseOutputStreamWriter(outStream);
            }
            catch (Exception)
            {
                /* Nom Nom Nom */
            }
        }

        /// <summary>
        /// Logea el texto especificado
        /// </summary>
        /// <param name="text">The text.</param>
        public static void Log(string text)
        {
            try
            {
                StreamWriter outStream = GetOutputStreamWriter();
                WriteLine(outStream, $"{DateTime.UtcNow:s} -- {text}");
                CloseOutputStreamWriter(outStream);
            }
            catch (Exception)
            {
                /* Nom Nom Nom */
            }
        }


        /// <summary>
        /// Logea una excepción
        /// </summary>
        /// <param name="ex">The Exception.</param>
        public static void Log(Exception ex)
        {
            try
            {
                StreamWriter outStream = GetOutputStreamWriter();
                WriteLine(outStream, string.Empty);
                WriteLine(outStream,
                    "!!!!!!!!!!!!!!!!!!!!!!!!!!!! Exception !!!!!!!!!!!!!!!!!!!!!!!");
                WriteLine(outStream, $"\\- Date: {DateTime.UtcNow:s}");
                WriteLine(outStream, string.Empty);
                Exception exAux = ex;
                while (exAux != null)
                {
                    WriteLine(outStream, $"Message: {exAux.Message}");
                    WriteLine(outStream, $"Stacktrace: {exAux.StackTrace}");
                    exAux = exAux.InnerException;
                }

                CloseOutputStreamWriter(outStream);
            }
            catch (Exception)
            {
                /* Nom Nom Nom */
            }
        }
    }
}