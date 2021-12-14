using System;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;

namespace ApiRestaurante
{
    public static class Crypto
    {
        [Obsolete]
        public static string GetMd5(this string value)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(value, "MD5");
        }

        private static readonly string Password = "senha";

        public static string GerarHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString().ToLower();
        }

        public static string EncryptMD5(this string value)
        {
            string result = GerarHashMd5(value);
            string resultEncrypt = Encrypt(result);
            return resultEncrypt;
        }

        public static string Encrypt(this string value)
        {
            string result = null;
            try
            {
                using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
                {
                    byte[] hash = provider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Password));
                    byte[] key = new byte[32];
                    Array.Copy(hash, 0, key, 0, 16);
                    Array.Copy(hash, 0, key, 15, 16);
                    using (RijndaelManaged manager = new RijndaelManaged())
                    {
                        manager.Key = key;
                        manager.Mode = CipherMode.ECB;
                        using (ICryptoTransform encryptor = manager.CreateEncryptor())
                        {
                            byte[] buffer = ASCIIEncoding.ASCII.GetBytes(value);
                            result = Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            return result;
        }

        public static string Decrypt(this string value)
        {
            string result = null;
            try
            {
                using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
                {
                    byte[] hash = provider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Password));
                    byte[] key = new byte[32];
                    Array.Copy(hash, 0, key, 0, 16);
                    Array.Copy(hash, 0, key, 15, 16);
                    using (RijndaelManaged manager = new RijndaelManaged())
                    {
                        manager.Key = key;
                        manager.Mode = CipherMode.ECB;
                        using (ICryptoTransform decryptor = manager.CreateDecryptor())
                        {
                            byte[] buffer = Convert.FromBase64String(value);
                            result = ASCIIEncoding.ASCII.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            return result;
        }
    }
}