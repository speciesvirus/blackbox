using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Awecent.Back.Serial.Models
{
    public class LogFile
    {

        string location = System.Web.Configuration.WebConfigurationManager.AppSettings["LocationFile"].ToString();

        public void WriterError(LogModel model)
        {
            string datefolder = DateTime.Now.ToString("yyyy-MM");
            string datename = DateTime.Now.ToString("yyyy-MM-dd");
            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            model.DateTime = datetime;
            try
            {
                var folder = location + "LogError";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var path = location + "LogError\\" + datefolder;
                if (!Directory.Exists(path))  Directory.CreateDirectory(path);

                var filename = path + "\\" + datename + ".txt";
                var sw = new System.IO.StreamWriter(filename, true);
                string strinput = JsonConvert.SerializeObject(model);
                sw.WriteLine(strinput+",");
                sw.Close();
            }catch (Exception e)
            {

            }
        }

        public void Writer(LogModel model)
        {
            string datefolder = DateTime.Now.ToString("yyyy-MM");
            string datename = DateTime.Now.ToString("yyyy-MM-dd");
            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            model.DateTime = datetime;
            try
            {
                var folder = location + "Log";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var path = location + "Log\\" + datefolder;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var filename = path + "\\" + datename + ".txt";
                var sw = new System.IO.StreamWriter(filename, true);
                string strinput = JsonConvert.SerializeObject(model);
                sw.WriteLine(strinput + ",");
                sw.Close();
            }
            catch (Exception e)
            {

            }
        }
    }
}