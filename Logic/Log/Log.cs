using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Render
{
    class Log
    {
        private static Log _instance = null;
        private Log()
        {

        }

        string filePath = "L:\\C#\\渲染器\\Render\\Render\\TextFile1.txt";

        public void writeLog(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                // 好像没有什么处理异常的好手段。。。
            }
        }

        // 获取单例
        public static Log instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Log();
                return _instance;
            }
        }
        /// <summary>
        /// 写日志的，我想了想，这个函数设计得尽量方便点debug用，所以基本上就是你放什么内容就是什么内容
        /// ，文件也由你自己指定，我也提供了可以只指定一次文件名的方法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fileName"></param>
        public void writeLog(string message,string fileName)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter(fileName,true))
                {
                    sw.WriteLine(message);
                }
            }catch(Exception e)
            {
                // 好像没有什么处理异常的好手段。。。
            }
        }

        public void writeLog<T>(T message, string fileName)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine(message);
                }
            }catch(Exception e)
            {
                //
            }
        }


        public void setFileName(string name)
        {
            fileName = name;
        }
        private string fileName;
    }
}
