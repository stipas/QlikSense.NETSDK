// See https://aka.ms/new-console-template for more information
using System;
using Qlik.Engine;
using Qlik.Engine.Communication;
using Qlik.Sense.Client;

namespace ConnectDirectLocalServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // The default port number is 4747 but can be customized
            var uri = new Uri("https://ip-172-31-94-250.ec2.internal");
            var certs = CertificateManager.LoadCertificateFromStore();

            var location = Location.FromUri(uri);
			//ILocation local = Qlik.Engine.Location.FromUri(new Uri("https://ip-172-31-94-250.ec2.internal"));
            location.AsDirectConnection(userDirectory: "EC2QLIK-DEV", userId: "qlikpro", certificateCollection: certs);

            using (var hub = location.Hub())
            {
                Console.WriteLine(hub.EngineVersion().ComponentVersion);
            }
			
			try
				{
				using (var doc = location.App("b388e7c1-632e-4d0b-a591-2da718e245d9"))//id app 
					{
						//Console.WriteLine(doc);
					var fields = doc.GetDimensionList();
					var theList2 = fields.Layout.DimensionList;
					//Dimensiones
					List<string> lines = new List<string>();
					lines.Add("Id|FieldLabels|FieldDefs ");
					foreach (var appField in theList2.Items)
					 {
						var field = doc.GetGenericDimension(appField.Info.Id); 
						var field2= field.Properties; 
						// Console.WriteLine($"{string.Join(", ",field2.Info.Id)}  | {string.Join(", ",field2.Dim.FieldLabels)}  | {string.Join(", ",field2.Dim.FieldDefs)}  ");                                           
						lines.Add($"{field2.Info.Id}|{string.Join(", ",field2.Dim.FieldLabels)}|{string.Join(", ",field2.Dim.FieldDefs)} ");
					 }
					File.WriteAllLines("D:/QLIK_DATA/Files/Master Items/Dimensions.txt", lines.ToArray());
					lines.Clear();                       
					
					//Medidas
					var measures = doc.GetMeasureList();
					var theList1 = measures.Layout.MeasureList;
					lines.Add("Id|Label|Def ");
					foreach (var appMeasure in theList1.Items)
						{
						var field = doc.GetGenericMeasure(appMeasure.Info.Id); 
						var field2= field.Properties; 
					   // Console.WriteLine($"{field2.Info.Id} | {field2.Measure.Label} |{string.Join(", ",field2.Measure.Def)}");                                           
						lines.Add($"{field2.Info.Id}|{field2.Measure.Label}|{string.Join(", ",field2.Measure.Def)}");
						}
					File.WriteAllLines("D:/QLIK_DATA/Files/Master Items/Measures.txt", lines.ToArray());
					lines.Clear();  
                    //Fields
                    List<string> lines = new List<string>();
                    lines.Add("Id");
                    foreach (var appField in theList2.Items)
                    	{                              
                        	//Console.WriteLine(appField.Name);
                        	lines.Add($"{appField.Name}");
                    	}                           
                    File.WriteAllLines("D:/QLIK_DATA/Files/Master Items/Fields.txt", lines.ToArray());
                    lines.Clear(); 					
					}

				}
				catch (MethodInvocationException methodInvocationException)
				{
					Console.WriteLine("Error: {0} recieved {1}", methodInvocationException.Message);
				}
        }
    }
}
