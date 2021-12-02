using MikroSzim.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MikroSzim
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<int> malePopulation = new List<int>();
        List<int> femalePopulation = new List<int>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        Random rng = new Random(1234);
        public Form1()
        {
            InitializeComponent();
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }
            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbabilities.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }
            return birthProbabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }
            return deathProbabilities;
        }

        public void Simulation()
        {
            var nepFajl = textBox1.Text;
            Population = GetPopulation(nepFajl);
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");

            var zaroEv = numericUpDown1.Value;
            for (int year = 2005; year <= (int)zaroEv; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                malePopulation.Add(nbrOfMales);
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                femalePopulation.Add(nbrOfFemales);
                Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
        }

        public void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;
            int age = (int)(year - person.BirthYear);

            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            if (rng.NextDouble() <= pDeath) person.IsAlive = false;

            if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                if (rng.NextDouble() <= pBirth)
                {
                    Person gyermek = new Person();
                    gyermek.BirthYear = year;
                    gyermek.NbrOfChildren = 0;
                    gyermek.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(gyermek);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Population.Clear();
            malePopulation.Clear();
            femalePopulation.Clear();
            Simulation();
            DisplayResults();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Column Seperated Values (*.csv) | *.csv";
            ofd.DefaultExt = "csv";
            ofd.AddExtension = true;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            textBox1.Text = ofd.FileName.ToString();
        }

        public void DisplayResults()
        {
            var zaroEv = numericUpDown1.Value;
            for (int year = 2005; year <= (int)zaroEv; year++)
            {
                richTextBox1.Text += "Szimulációs év: " + year + "\n" + "\t" + "Fiúk: " + malePopulation[year-2005] + "\n" + "\t" + "Lányok: " + femalePopulation[year-2005] + "\n" + "\n";
            }
        }
    }
}
