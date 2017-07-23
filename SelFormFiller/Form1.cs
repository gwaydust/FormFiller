using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.IO;
using Csv;

namespace SelFormFiller
{
    public partial class Form1 : Form
    {
        private static Filler filler = null;
        public Form1()
        {
            InitializeComponent();
            this.FormClosed += Form1_FormClosed;
            listBox1.Click += ListBox1_Click;
            //this.textBox1.Text = ConfigurationManager.AppSettings["baseURL"];
            filler = new Filler(ConfigurationManager.AppSettings["baseURL"]);            
            MethodInfo[] methodInfos = typeof(Filler)
                           .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var info in methodInfos) {
                if (info.Name=="Dispose" || info.Name=="Filler")
                {
                    continue;
                }
                this.listBox1.Items.Add(info.Name);
            }
            string[] filePaths = System.IO.Directory.GetFiles(ConfigurationManager.AppSettings["csvDir"],"*.csv");
            foreach (var filePath in filePaths)
            {
                comboBox1.Items.Add(filePath);
            }
        }

        private void ListBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem==null)
            {
                return;
            }

            MethodInfo method = typeof(Filler).GetMethod(listBox1.SelectedItem.ToString());
            var paramInfos = method.GetParameters();
            int count = 0;
            panel2.Controls.Clear();
            foreach (var paramInfo in paramInfos)
            {                
                if (paramInfo.ParameterType.FullName == typeof(String).FullName)
                {
                    var lbl = new Label();
                    lbl.Text = paramInfo.Name;
                    lbl.Location = new Point(4, 4 + count * 25);
                    lbl.Show();
                    lbl.Visible = true;
                    count++;
                    var tb = new TextBox();
                    tb.Location = new Point(4, 4 + count * 25);
                    tb.MaxLength = 1024;
                    tb.Name = "tb_" + paramInfo.Name;
                    tb.Show();
                    count++;
                    panel2.Controls.Add(lbl);
                    panel2.Controls.Add(tb);
                } else
                {
                    MessageBox.Show($"Only support type String as parameter.\r\nPlease update parameter {paramInfo.Name}'s type, or change method: {method.Name}");
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (filler!=null)
            {
                filler.Dispose();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem==null)
            {
                MessageBox.Show("No item selected");
                return;
            }
            MethodInfo method = typeof(Filler).GetMethod(listBox1.SelectedItem.ToString());
            var args = new List<String>();
            if (cbParams.Checked)
            {
                foreach (var paramInfo in method.GetParameters())
                {

                    foreach (var ctrl in this.panel2.Controls)
                    {
                        if (ctrl.GetType() != typeof(TextBox) || ((TextBox)ctrl).Name != $"tb_{paramInfo.Name}")
                            continue;
                        args.Add(((TextBox)ctrl).Text);
                    }                    
                }
                method.Invoke(filler, args.ToArray());
                return;
            }
            if (String.IsNullOrWhiteSpace(comboBox1.SelectedItem.ToString()))
            {
                MessageBox.Show("No CSV file selected");
                return;
            }
            string csv = System.IO.File.ReadAllText(comboBox1.SelectedItem.ToString());
            int count = 0;
            foreach (var line in Csv.CsvReader.ReadFromText(csv))
            {
                for (int i = 0; i < line.ColumnCount; i++)
                {
                    args.Add(line[i]);
                }
                if (args.Count < method.GetParameters().Length)
                {
                    MessageBox.Show($"Number of parameters provided is less than what the method need:\r\n{line.Raw}");
                    return;
                }
                method.Invoke(filler, args.ToArray());
                args.Clear();
                count++;
                MessageBox.Show($"{count}: {line.Raw}");                
            }
        }
    }
}
