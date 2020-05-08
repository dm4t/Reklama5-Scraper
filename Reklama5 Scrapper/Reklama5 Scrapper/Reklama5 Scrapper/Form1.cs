using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;





/*

    Selenium ChromeDrive treba da bide kompetabilen so Chrome na PC

    https://chromedriver.chromium.org/downloads  moze da se simen driver i da se dodade vo bin/Debug 

    ili da se napravi Updata na NuGet Pucket vo Visual Studio 


    */

namespace Reklama5_Scrapper
{
    public partial class Form1 : Form
    {
        static string titleS;
        static string cenaS;

        static int s = 0;
        static int end = 0;

        string model;
        string godinaOd;
        string godinaDo;
        string cenaOd;
        string cenaDo;
        string kmOd;
        string kmDo;
        static IWebDriver Driver; 
        string Grad;
        string Gorivo;
        string Menuvac;
        string Registracija;
        Thread Car;

        static object lockObject = new object();  
        







        public Form1()
        {
            InitializeComponent();


            // Kriiranje na koloniv vo DataGrideView
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Оглас", "Оглас");
            dataGridView1.Columns.Add("Цена", "Цена");





        }



        public void CarScrap()              //Funkcija za kriiranje na thread 
        {
            IWebElement element;            //elementi
            Driver = new ChromeDriver();    //Dreiver

            //Navaigate() otvaranje an link vo chrome
            Driver.Navigate().GoToUrl("https://reklama5.mk/Search?q=&city=&sell=0&sell=1&buy=0&buy=1&trade=0&trade=1&includeOld=0&includeOld=1&includeNew=0&includeNew=1&f31=&priceFrom=9000&priceTo=12000&f33_from=&f33_to=&f36_from=&f36_to=&f35=&f37=&f138=&f10016_from=&f10016_to=&private=0&company=0&page=1&SortByPrice=1&zz=1&cat=24");


            

            //Selektiranje na godinaOd prku ID HTML element
            if(godinaOd!=null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f33_from")))).SelectByValue(godinaOd);
            try
            {


                //Selektiranje na godinaDo prku ID HTML element
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f33_to")))).SelectByText(godinaDo);
            }catch (Exception e) { }

            try
            {
                //Selektiranje na modele od prku ID HTML element
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f31")))).SelectByText(model);
            }
            catch (Exception e) { }

            //Selektiranje na cenaOd prku ID HTML element

            if (cenaOd!=null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("priceFrom")))).SelectByValue(cenaOd);

            //Selektiranje na cenaDo prku ID HTML element
            if (cenaDo != null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("priceTo")))).SelectByValue(cenaDo);

            //Selektiranje na pominati kilometriOd prku ID HTML element
            if (kmOd != null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f36_from")))).SelectByValue(kmOd);

            //Selektiranje na pominati kilometriDo prku ID HTML element

            if (kmDo != null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f36_to")))).SelectByValue(kmDo);

            //Selektiranje na grad prku ID HTML element

            if (Grad != null) try
                {
                    (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("city")))).SelectByText(Grad);
                }
                catch (Exception ea) { }


            //Selektiranje na tip na gorivo prku ID HTML element

            if (Gorivo != null)
                try
                {
                    (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f35")))).SelectByText(Gorivo);
                }
                catch (Exception vb) { }

            //Selektiranje na tip na menuvac prku ID HTML element

            if (Menuvac != null) try
                {
                    (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f37")))).SelectByText(Menuvac);
                }
                catch (Exception ma) { }

            //Selektiranje na registracija stranska/mkedonskai preku ID HTML element

            if (Registracija != null) try
                {
                    (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f138")))).SelectByText(Registracija);
                }
                catch (Exception am) { }




            //Pronaoganje na prebaraj kopce preku cssSelector
            element = Driver.FindElement(By.CssSelector("input.btn.btn-xs.btn-primary"));
            element.Click(); //Simuliranje na kliki na button 

            IList<IWebElement> test = Driver.FindElements(By.ClassName("OglasResults")); //Dodavanje na site formi koj imaat Class Name  OglasResults vo lista

            IWebElement temp;

            Thread.Sleep(2000);


            //veriabli koj se kporistat da se spravat so brojot na stranici pri 
            int pageLock = 0;    
            int endLock = 0;
            try
            {
                while ((pageLock != 1)||(endLock != 1))
                {
                    string[] strana = null;
                    try
                    {   
                        // Se bara element vo HTML koj pokazuva kolku ima strani
                        temp = Driver.FindElement(By.ClassName("number-of-pages"));

                         strana = temp.Text.Split(' '); // Kastiranje na brojot an strani
                    }
                    catch (Exception e) {
                        pageLock = 1;
                    }
                    string title; //Titie na oglas
                    string cena; //Cena na oglas
                    test = Driver.FindElements(By.ClassName("OglasResults"));
                    foreach (IWebElement element1 in test) //Prelistuvanje na site oglasi koj prethodno se najdeni i dadadeni vo lista
                    {
                        //pronaoganje na Titlei I cena na avtompbil preku className i cssselector
                        temp = element1.FindElement(By.ClassName("SearchAdTitle"));
                        title = temp.Text;
                        temp = element1.FindElement(By.CssSelector("div.text-left.text-success"));
                        cena = temp.Text;

                        //Koristenje Invoke za da se azurira datagridview so novite podatoci 
                        this.Invoke((MethodInvoker)delegate
                        {
                            dataGridView1.Rows.Add(s++, title, cena);

                        });


                        

                    }
                    Thread.Sleep(500);
                    
                    //Proverka dali stranicata ja koja se naogame e posledna za da zavrsi sobiranjeto podatoci
                    if (strana[1].Equals(strana[3]) == false)
                    {

                        temp = Driver.FindElement(By.ClassName("prev-nextPage")); //klikanje next page doloku ima uste strani 
                        temp.Click();
                        Thread.Sleep(5000); //pauza na threa 5 s za da loadira sledan stranica
                    }
                    else {

                        //
                        endLock = 1; //endlock koga e  1 zavrsuva prebaruvanje 
                        MessageBox.Show("Завршено");

                        break;
                    }



                }
            }
            catch (Exception e) { MessageBox.Show("Завршено"); }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            model = comboBox1.Text;

            godinaOd = comboBox2.Text;
            godinaDo = comboBox3.Text;

            cenaOd = comboBox5.Text;
            cenaDo = comboBox4.Text;

            kmOd = comboBox7.Text;
            kmDo = comboBox6.Text;

            Grad = comboBox8.Text;

            Gorivo = comboBox9.Text;

            Menuvac = comboBox10.Text;

            Registracija = comboBox11.Text;
            
            Car = new Thread(CarScrap); //kriiranje na thread 
            Car.Start();  // start na thread
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

           

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
