using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace EasyDataSave
{
    static class SerializationSystem
    {
        static readonly JsonSerializerSettings settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };
        public static T Deserializar<T>(this string s, string key, bool deserializePrivate = false)
        {
            try
            {
                return deserializePrivate ? JsonConvert.DeserializeObject<T>(s.Desencrypt(key), settings) : JsonConvert.DeserializeObject<T>(s.Desencrypt(key));
            }
            catch (Exception)
            {
                object obj = deserializePrivate ? JsonConvert.DeserializeObject<object>(s.Desencrypt(key), settings) : JsonConvert.DeserializeObject<T>(s.Desencrypt(key));
                T data = default;
                Type originType = data.GetType();
                Type newDataType = obj.GetType();
                foreach(var originValue in originType.GetFields()){
                    foreach(var newDataValue in newDataType.GetFields()){
                        if (originValue.Name == newDataValue.Name)
                            originValue.SetValue(data, newDataValue.GetValue(obj));
                    }
                }
                return data;
            }
        }
        public static string Serializar(this object o, string key, bool serializePrivate = false) => serializePrivate ? JsonConvert.SerializeObject(o, settings).Encrypt(key) : JsonConvert.SerializeObject(o).Encrypt(key);
        static string Encrypt(this string text, string key)
        {
            byte[] keyArray;
            byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(text);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

            tdes.Clear();
            return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
        }
        static string Desencrypt(this string text, string key)
        {
            byte[] keyArray;
            byte[] Array_a_Descifrar = Convert.FromBase64String(text);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        private class MyContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                .Select(p => base.CreateProperty(p, memberSerialization))
                            .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                       .Select(f => base.CreateProperty(f, memberSerialization)))
                            .ToList();
                props.ForEach(p => { p.Writable = true; p.Readable = true; });
                return props;
            }
        }
    }
}
