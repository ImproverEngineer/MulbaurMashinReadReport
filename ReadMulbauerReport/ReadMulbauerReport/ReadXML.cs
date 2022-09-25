using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReadMulbauerReport
{
    class ReadXML
    {
        XmlDocument xDoc;
        Job job;
        
        public ReadXML(string fileName)
        {
            xDoc = new XmlDocument();
            xDoc.Load(fileName);
            if (xDoc != null)
            {
                job = new Job();
            }
        }

        public void ReadXml()
        {
            XmlElement xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    if (xnode.Name == "MachineSerialNumber")//получить машину 
                    {
                        job.MasinSerialNumber = xnode.InnerText;
                    }
                    if (xnode.Name == "MachineType")//получить тип машины
                    {
                        job.MasinType = xnode.InnerText;
                    }
                    if (xnode.Name == "DataUnit")// парсим DataUnit
                    {
                        job.Type = xnode.Attributes.GetNamedItem("Type").Value.ToString();
                        job.Name = xnode.Attributes.GetNamedItem("Name").Value.ToString();
                        job.Id = xnode.Attributes.GetNamedItem("ID").Value.ToString();
                        foreach (XmlElement DataUnit in xnode)
                        {
                            switch (DataUnit.Name)
                            {
                                case "State": job.state = DataUnit.InnerText; break;
                                case "StateDescr": job.StateDescr = DataUnit.InnerText;break;
                                case "ProductName": job.productName = DataUnit.InnerText; break;
                                case "Users":
                                    {
                                        foreach (XmlElement User in DataUnit)
                                        {
                                            if (User.Name == "User") { job.Users.Add(DataUnit.InnerText); }
                                        }
                                    } break;
                                case "NumberOfSubUnits": job.NumberOfSubUnits = DataUnit.InnerText;break;
                                case "NumberOfGoodSubUnits": job.NumberOfGoodSubUnits = DataUnit.InnerText;break;
                                case "NumberOfRejectedSubUnits": job.NumberOfRejectedSubUnits = DataUnit.InnerText;break;
                                case "PhysicalUnits":
                                    {
                                        foreach (XmlElement PhysicalUnit in DataUnit)
                                        {
                                            foreach (XmlElement TimeWork in PhysicalUnit)
                                            {
                                                switch(TimeWork.Name)
                                                {
                                                    case "Start":job.start = TimeWork.InnerText; break;
                                                    case "End": job.stop = TimeWork.InnerText; break;
                                                }
                                            }
                                        }
                                    }break;
                                case "SubDataUnits": // Получение значения по каждой карте
                                    foreach (XmlElement SubDataUnits in DataUnit)
                                    {
                                        if (SubDataUnits.Name == "DataUnit")
                                        {
                                            DataField df = new DataField(SubDataUnits.Attributes.GetNamedItem("Name").Value, SubDataUnits.Attributes.GetNamedItem("Type").Value);
                                            foreach (XmlElement DataUnitElements in SubDataUnits)
                                            {
                                                if (DataUnitElements.Name == "DataFields")
                                                {
                                                    foreach (XmlElement DataField in DataUnitElements)
                                                    {
                                                        if (DataField.Name == "DataField")
                                                        {
                                                            df.WriteValNameDiscription(DataField.Attributes.GetNamedItem("Name").Value, DataField.InnerText);                                                           
                                                        }
                                                    }
                                                }
                                            }
                                            if (df != null) { job.DataFields.Add(df); }
                                        }                                      
                                    }
                                    break;
                            }                           
                        }
                    }
                    
                }
            }

        }
        /// <summary>
        /// вывести на экран получившейся результат
        /// </summary>
        public void displayXML()
        {
            job.display();
            Console.ReadKey();
        }        
    }



    /// <summary>
    /// Пишим все что получили с машины
    /// </summary>
    class Job
    {
        public List<DataField> DataFields = new List<DataField>();
        public List<string> Users = new List<string>();
        public string MasinSerialNumber { get; set; }
        public string MasinType { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string state { get; set; }
        public string StateDescr { get; set; }
        public string productName { get; set; }
        public string NumberOfSubUnits { get; set; } // Количество сделаных юнитов
        public string NumberOfGoodSubUnits { get; set; } // Количество годных юнитов
        public string NumberOfRejectedSubUnits { get; set;} // Количество не годных юнитов
        public string start { get; set; }
        public string stop { get; set; }

        public void display()
        {
            Console.WriteLine($"MasinSerialNumber: {MasinSerialNumber}");
            Console.WriteLine($"MasinType: {MasinType}");
            Console.WriteLine($"Type: {Type}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"state: {state}");
            Console.WriteLine($"StateDescr: {StateDescr}");
            Console.WriteLine($"productName: {productName}");
            foreach (string st in Users)
            {
                Console.WriteLine("User: " + st);
            }
            Console.WriteLine($"Всего изготовлено: {NumberOfSubUnits}");
            Console.WriteLine($"Годные: {NumberOfGoodSubUnits}");
            Console.WriteLine($"Не годные: {NumberOfRejectedSubUnits}");
            Console.WriteLine($"Время начала: {start}");
            Console.WriteLine($"Время окончания: {stop}");
            Console.WriteLine();
            foreach (DataField df in DataFields)
            {
                Console.Write($"Name: {df.Name} ");
                Console.Write($"Val: {df.Type } ");
                for (int i = 0; i < df.DiscriptionName.Count; i++)
                {
                    Console.WriteLine($"DName: {df.DiscriptionName[i].ToString()}; DVal: {df.DiscriptionVal[i].ToString()}");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }   
    }
    public class DataField
    {
      
        public DataField(string Name, string Type)
        {
            this.Name = Name;
            this.Type = Type;
        }
        public void WriteValNameDiscription(string DisName,string DisVal)
        {
            DiscriptionName.Add(DisName);
            DiscriptionVal.Add(DisVal);
        }
 
        public string Name { get; set; }
        public string Type { get; set; }

        public List<string> DiscriptionName = new List<string>();
        public List<string> DiscriptionVal = new List<string>();  
        

    }
}
