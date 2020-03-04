using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask_matStat
{
    class Program
    {
        static Random rnd = new Random();
        static double QuantileExpFunc(double p) {
            return -Math.Log(1 - p);
        }

        static double QuantileFunc(double p) {
            if (p == 0) return 0;
            return 1 - Math.Log(1/p - 1)/10;
        }
        static void Main(string[] args)
        {
            string GeneralPath = "../../GeneralInfo.txt";
            string GeneralInfo = "";
            for (int j = 0; j < 1; j++)
            {
                string pathTimes1 = "../../t"+j+".csv";
                string pathTimes2 = "../../r" + j + ".csv";
                List<double> timesBetween = new List<double>();
                List<double> timeRequesting = new List<double>();

                int part = 1;

                double Time = 0;
                double TimeSystem = QuantileFunc(rnd.NextDouble());
                timeRequesting.Add(TimeSystem);
                double waiting = 0;
                for (int i = 1; i < 1000; i++)
                {
                    double tmp = QuantileExpFunc(rnd.NextDouble());
                    timesBetween.Add(tmp);
                    Time += tmp;
                    if (Time >= TimeSystem)
                    {
                        waiting += Time - TimeSystem;
                        double tmp2 = QuantileFunc(rnd.NextDouble());
                        timeRequesting.Add(tmp2);
                        TimeSystem = Time + tmp2;
                        part++;
                    }

                }

                GeneralInfo += "Доля получивших отказ: " + (1000 - part) / 1000.0 + "\n";
                GeneralInfo += "Доля простоя: " + waiting/TimeSystem + "\n";

                FileStream fsWrite = File.OpenWrite(pathTimes1);
                StreamWriter sw = new StreamWriter(fsWrite);

                for (int i = 0; i < timesBetween.Count; i++)
                {
                    sw.Write(timesBetween[i] + ";");
                }
                sw.Flush();
                sw.Close();
                fsWrite.Close();
                GeneralInfo += "Времена между заказами записаны в файл "+pathTimes1 +"\n";
                FileStream fsWrite2 = File.OpenWrite(pathTimes2);
                StreamWriter sw2 = new StreamWriter(fsWrite2);

                for (int i = 0; i < timeRequesting.Count; i++)
                {
                    sw2.Write(timeRequesting[i] + ";");
                }
                sw2.Flush();
                sw2.Close();
                fsWrite2.Close();
                GeneralInfo += "Времена между обработкой записаны в файл " + pathTimes2 + "\n";
                timesBetween.Sort();
                GeneralInfo += "Максимальный элемент в "+pathTimes1+ " "+ timesBetween[timesBetween.Count - 1] +"\n";
                GeneralInfo += "Минимальный элемент в " + pathTimes1 + " " + timesBetween[0] + "\n";
                timeRequesting.Sort();
                GeneralInfo += "Максимальный элемент в " + pathTimes2 + " " + timeRequesting[timeRequesting.Count - 1]+"\n";
                GeneralInfo += "Минимальный элемент в " + pathTimes2 + " " + timeRequesting[0] + "\n";
                GeneralInfo += "\n";
                Console.WriteLine(part == timeRequesting.Count);
                Console.WriteLine(part);
            }
            FileStream fsWriten = File.OpenWrite(GeneralPath);
            StreamWriter swn = new StreamWriter(fsWriten);

            swn.Write(GeneralInfo);
            swn.Flush();
            swn.Close();
            fsWriten.Close();
            Console.ReadKey();
        }
    }
}
