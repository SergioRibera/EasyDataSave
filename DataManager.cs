using System;
using System.IO;
using System.Text;

namespace EasyDataSave
{
    public static partial class SaveDataManager
    {
        static T Instancia<T>()
        {
            T obj = default;
            if (typeof(T).IsValueType)
            {
                obj = default;
            }
            else if (typeof(T) == typeof(string))
            {
                obj = (T)Convert.ChangeType(string.Empty, typeof(T));
            }
            else
            {
                obj = Activator.CreateInstance<T>();
            }
            return obj;
        }
        /// <summary>
        /// Genera una cadena de texto con caracteres aleatorios
        /// </summary>
        /// <param name="Length">Corresponde al tamaño que debería tener el texto generado (Por defecto su valor es de 15)</param>
        /// <param name="Mayusculas">Indica si el texto debe incluir mayusculas</param>
        /// <param name="minusculas">Indica si el texto debe incluir minuscular</param>
        /// <param name="simbolos">Indica si el texto debe incluir simbolos</param>
        /// <returns></returns>
        public static string RandomId(int Length = 15, bool Mayusculas = true, bool minusculas = true, bool simbolos = true)
        {
            if (!Mayusculas && !minusculas && !simbolos || Length <= 0)
                return "Null";
            string newId = "";
            string abecedarioMayus = "A-B-C-D-E-F-G-H-I-J-K-L-M-N-Ñ-O-P-Q-R-S-T-U-V-W-X-Y-Z";
            string abecedarioMinus = "a-b-c-d-e-f-g-h-i-j-k-l-m-n-ñ-o-p-q-r-s-t-u-v-w-x-y-z";
            string caracteres = "- _ / &";
            Random r = new Random();
            int actualLength = 0;
            while (actualLength < Length)
            {
                int value = r.Next(0, 1);
                string letra = "";
                switch (value)
                {
                    case 0:
                        int proba_Char = r.Next(0, 5);
                        if (Mayusculas)
                        {
                            if (proba_Char == 2)
                            {
                                if (simbolos)
                                {
                                    letra = caracteres.Split(' ')[r.Next(0, 4)] + abecedarioMayus.Split('-')[r.Next(0, 26)];
                                    actualLength++;
                                }
                            }
                            else
                            {
                                letra = abecedarioMayus.Split('-')[r.Next(0, 26)];
                                actualLength++;
                            }
                        }
                        break;
                    case 1:
                        int proba_Char2 = r.Next(0, 5);
                        if (minusculas)
                        {
                            if (proba_Char2 == 3)
                            {
                                if (simbolos)
                                {
                                    letra = caracteres.Split(' ')[r.Next(0, 4)] + abecedarioMinus.Split('-')[r.Next(0, 26)];
                                    actualLength++;
                                }
                            }
                            else
                            {
                                letra = abecedarioMinus.Split('-')[r.Next(0, 26)];
                                actualLength++;
                            }
                        }
                        break;
                }
                newId = newId + letra;
            }
            return newId;
        }
        /// <summary>
        /// ESta funcion devuelve true en caso de que existan datos de una clase determinada
        /// </summary>
        /// <param name="type">Corresponde a la clase a Buscar</param>
        /// <param name="path">Indica la ruta donde se buscará el archivo de datos</param>
        /// <returns></returns>
        public static bool Exist(Type type, string path)
        {
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(type.Name));
            return File.Exists(path);
        }
        /// <summary>
        /// Esta funcion Guarda los datos encriptados del objeto especificado
        /// </summary>
        /// <param name="data">Instancia del objeto a Guardar</param>
        /// <param name="path">Indica la ruta donde se guardará el archivo de datos</param>
        /// <param name="key">Corresponde a la clave de encriptacion</param>
        /// <returns></returns>
        public static object Save(object data, string path, string key)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(data.GetType().Name));
            File.WriteAllText(path, data.Serializar(key));
            return data;
        }
        /// <summary>
        /// Funcion heredada para Guardar los Datos de los Objetos indicados
        /// </summary>
        /// <typeparam name="T">Indica la clase de Datos que guardará</typeparam>
        /// <param name="o">Instancia del objeto a Guardar</param>
        /// <param name="path">Indica la ruta donde se guardará el archivo de datos</param>
        /// <param name="key">Corresponde a la clave de encriptacion</param>
        /// <returns></returns>
        public static T Save<T>(this object o, string path, string key)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(typeof(T).Name));
            File.WriteAllText(path, o.Serializar(key));
            return (T)o;
        }
        /// <summary>
        /// Esta funcion Guarda los datos encriptados del objeto especificado
        /// </summary>
        /// <param name="data">Instancia del objeto a Guardar</param>
        /// <param name="path">Indica la ruta donde se guardará el archivo de datos</param>
        /// <param name="key">Corresponde a la clave de encriptacion</param>
        /// <param name="savePrivates">Indica si tambien se deben guardar los parametros privados</param>
        /// <returns></returns>
        public static object Save(object data, string path, string key, bool savePrivates = false)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(data.GetType().Name));
            File.WriteAllText(path, data.Serializar(key, savePrivates));
            return data;
        }
        /// <summary>
        /// Funcion heredada para Guardar los Datos de los Objetos indicados
        /// </summary>
        /// <typeparam name="T">Indica la clase de Datos que guardará</typeparam>
        /// <param name="o">Instancia del objeto a Guardar</param>
        /// <param name="path">Indica la ruta donde se guardará el archivo de datos</param>
        /// <param name="key">Corresponde a la clave de encriptacion</param>
        /// <param name="savePrivates">Indica si tambien se deben guardar los parametros privados</param>
        /// <returns></returns>
        public static T Save<T>(this object o, string path, string key, bool savePrivates = false)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(typeof(T).Name));
            File.WriteAllText(path, o.Serializar(key, savePrivates));
            return (T)o;
        }
        /// <summary>
        /// Carga Los datos de una determinada Clase
        /// </summary>
        /// <typeparam name="T">Indica el tipo de clase que debe cargar</typeparam>
        /// <param name="path">Indica la Ruta donde esta guardado el archivo</param>
        /// <param name="key">Corresponde a la clave de encriptacion</param>
        /// <returns></returns>
        public static T Load<T>(string path, string key)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(typeof(T).Name));
            if (!File.Exists(path)) return Instancia<T>();
            T data = File.ReadAllText(path).Deserializar<T>(key);
            if (data == null) return Instancia<T>();
            return data;
        }
        /// <summary>
        /// Carga Los datos de una determinada Clase
        /// </summary>
        /// <typeparam name="T">Indica el tipo de clase que debe cargar</typeparam>
        /// <param name="path">Indica la Ruta donde esta guardado el archivo</param>
        /// <param name="key">Corresponde a la clave de encriptacion</param>
        /// <param name="loadPrivates">Indica si tambien se deben guardar los parametros privados</param>
        /// <returns></returns>
        public static T Load<T>(string path, string key, bool loadPrivates = false)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(typeof(T).Name));
            if (!File.Exists(path)) return Instancia<T>();
            T data = File.ReadAllText(path).Deserializar<T>(key, loadPrivates);
            if (data == null) return Instancia<T>();
            return data;
        }
        /// <summary>
        /// Funcion heredada para Cargar los Datos de los Objetos indicados
        /// </summary>
        /// <typeparam name="T">Indica la clase de Datos que cargará</typeparam>
        /// <param name="o">Instancia del objeto a Cargar</param>
        /// <param name="path">Indica la Ruta donde esta guardado el archivo</param>
        /// <param name="key">Corresponde a la clave de encriptacion</param>
        /// <returns></returns>
        public static T Load<T>(this object o, string path, string key)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (o == null) return Instancia<T>();
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(o.GetType().Name));
            if (!File.Exists(path)) return Instancia<T>();
            o = File.ReadAllText(path).Deserializar<T>(key);
            return (T)o;
        }
        /// <summary>
        /// Funcion heredada para Cargar los Datos de los Objetos indicados
        /// </summary>
        /// <typeparam name="T">Indica la clase de Datos que cargará</typeparam>
        /// <param name="o">Instancia del objeto a Cargar</param>
        /// <param name="path">Indica la Ruta donde esta guardado el archivo</param>
        /// <param name="key">Corresponde a la clave de encriptacion</param>
        /// <param name="loadPrivates">Indica si tambien se deben guardar los parametros privados</param>
        /// <returns></returns>
        public static T Load<T>(this object o, string path, string key, bool loadPrivates = false)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (o == null) return Instancia<T>();
            path += "/" + Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(o.GetType().Name));
            if (!File.Exists(path)) return Instancia<T>();
            o = File.ReadAllText(path).Deserializar<T>(key, loadPrivates);
            return (T)o;
        }
    }
}