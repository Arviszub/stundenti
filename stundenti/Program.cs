using System.IO;
using System.Text.Json;
using System.Text.Encodings.Web;
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "students.csv");

            string jsonpath = Path.Combine(AppContext.BaseDirectory, "results.json");

            string reportspath = Path.Combine(AppContext.BaseDirectory, "reports.txt");


            if (!File.Exists(path))
            {
                Console.WriteLine("File not found: " + path);
                return;
            }
            string[] lines = File.ReadAllLines(path);

            var data = new List<Data> { };

            double totalavg = 0.0;
            int passednr = 0;
            int totalstudents = 0;

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                string name = parts[0];
                string lastname = parts[1];
                int[] grades = [int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5])];
                double average = (int.Parse(parts[2]) + int.Parse(parts[3]) + int.Parse(parts[4]) + int.Parse(parts[5])) / 4.0;
                string passed = (average >= 6) ? "passed" : "not passed";

                data.Add(new Data()
                {
                    Name = name,
                    Lastname = lastname,
                    Grades = grades,
                    Average = average,
                    Passed = passed
                });
                totalavg += average;
                if (passed == "passed") passednr += 1;
                totalstudents++;
            }
            totalavg = totalavg / totalstudents;
            File.WriteAllText(reportspath, $"Klasses vidējais ir {totalavg + Environment.NewLine}");
            File.AppendAllText(reportspath, $"{passednr} skolēni tika tālāk");

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(jsonpath, json);


        }
        class Data
        {
            public string Name { get; set; } = "";
            public string Lastname { get; set; } = "";
            public int[] Grades { get; set; }
            public double Average { get; set; }
            public string Passed { get; set; } = "";
        }
    }
}