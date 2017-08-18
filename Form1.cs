using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        List<Word> localtemp1 = new List<Word>();
        List<Word> localtemp2 = new List<Word>();
        List<Word> temp1 = new List<Word>();
        List<Word> temp2 = new List<Word>();
        int totalNumberOfText1 = 0;
        int totalNumberOfText2 = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // checks if entered word is already in the excluded list 
            string[] str = Regex.Matches(textBox1.Text.ToLower(), "\\w+")
              .OfType<Match>()
              .Select(m => m.Value)
              .ToArray();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter a word!");
            }

            else if (!listBox1.Items.Contains(str[0]))
            {
                listBox1.Items.Add(str[0]);
                if (temp1.Count()!=0)
                {
                    Word itemToRemove = temp1.SingleOrDefault(r => r.Value == str[0]);
                    temp1.Remove(itemToRemove);

                }
                if (temp2.Count() != 0)
                {
                    Word itemToRemove = temp2.SingleOrDefault(r => r.Value == str[0]);
                    temp2.Remove(itemToRemove);
                }

            }
            else
            {
                MessageBox.Show("\"" + textBox1.Text.ToLower() + "\" is already excluded!");
            }


            if (richTextBox1.TextLength != 0 || richTextBox2.TextLength != 0)
            {
                Word itemToRemove = localtemp1.SingleOrDefault(r => r.Value == str[0]);
                localtemp1.Remove(itemToRemove);
                itemToRemove = localtemp2.SingleOrDefault(r => r.Value == str[0]);
                localtemp2.Remove(itemToRemove);

                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView3.DataSource = null;

                dataGridView1.DataSource = localtemp1;
                dataGridView2.DataSource = localtemp2;
                dataGridView3.DataSource = CommonList(localtemp1, localtemp2);
            }
            else
            {
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView3.DataSource = null;

                dataGridView1.DataSource = temp1;
                dataGridView2.DataSource = temp2;
                dataGridView3.DataSource = CommonList(temp1, temp2);
            }

            dataGridView1.Refresh();
            dataGridView2.Refresh();
            dataGridView3.Refresh();

            textBox1.Clear();
            button1.Enabled = false;
            label3.Text = dataGridView1.RowCount.ToString();
            label2.Text = dataGridView2.RowCount.ToString();
            label8.Text = dataGridView3.RowCount.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            listBox1.Items.Remove(listBox1.SelectedItems[0]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
            string[] excluded = listBox1.Items.OfType<string>().ToArray();
            localtemp1.Clear();
            localtemp2.Clear();

            // TEXT FIELD ONE 
            // create array of first text field
            string text1 = richTextBox1.Text.ToLower();
            string[] words1 = Regex.Matches(text1, "\\w+")
              .OfType<Match>()
              .Select(m => m.Value)
              .ToArray();

            // delete excluded words from array
            foreach (var item in excluded)
            {
                for (int i = 0; i < words1.Length; i++)
                {
                    if (item.Equals(words1[i]))
                    {
                        words1[i] = null;
                    }
                }
            }

            // removes null element from array
            words1 = words1.Where(s => !String.IsNullOrEmpty(s)).ToArray();

            // Create a list composed of uniqe word objects
            foreach (string word in words1)
            {
                bool flag = false;
                foreach (var item in localtemp1)
                {
                    if ( word == item.Value)
                    {
                        item.Frequency += 1;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    localtemp1.Add(new Word(word, 1));
                }
            }

            // order by frequnecy // add it to datagridview // show the number of words in list
            localtemp1 = localtemp1.OrderByDescending(x => x.Frequency).ToList();
            dataGridView1.DataSource = localtemp1;
            label3.Text = localtemp1.Count().ToString();

            // TEXT FIELD TWO
            // create array of first text field
            string text2 = richTextBox2.Text.ToLower();
            string[] words2 = Regex.Matches(text2, "\\w+")
              .OfType<Match>()
              .Select(m => m.Value)
              .ToArray();

            // delete excluded words from array
            foreach (var item in excluded)
            {
                for (int i = 0; i < words2.Length; i++)
                {
                    if (item.Equals(words2[i]))
                    {
                        words2[i] = null;
                    }
                }
            }

            // removes null element from array
            words2 = words2.Where(s => !String.IsNullOrEmpty(s)).ToArray();

            // Create a list composed of uniqe word objects
            foreach (string word in words2)
            {
                bool flagg = false;
                foreach (var item in localtemp2)
                {
                    if ( word == item.Value)
                    {
                        item.Frequency += 1;
                        flagg = true;
                    }
                }
                if (!flagg)
                {
                    localtemp2.Add(new Word(word, 1));
                }
            }

            // order by frequnecy // add it to datagridview // show the number of words in list
            localtemp2 = localtemp2.OrderByDescending(x => x.Frequency).ToList();
            dataGridView2.DataSource = localtemp2;
            label2.Text = localtemp2.Count().ToString();

            //****************************
            //*     compare panel data    
            //****************************
            List<CommonWord> commonWordList = CommonList(localtemp1, localtemp2);

            double percentage1 = Convert.ToDouble( commonWordList.Count()) / localtemp1.Count();
            label_uniqe_per1.Text = (percentage1 * 100).ToString("#.##");
            double percentage2 = Convert.ToDouble(commonWordList.Count()) / localtemp2.Count();
            label_uniqe_per2.Text = (percentage2 * 100).ToString("#.##");

            int per1 = 0;
            int per2 = 0;

            foreach (var item in commonWordList)
            {
                per1 += item.Freq1;
                per2 += item.Freq2;
            }

            label_per1.Text = ((Convert.ToDouble(per1)*100) / words1.Length).ToString("#.##");
            label_per2.Text = ((Convert.ToDouble(per2)*100) / words2.Length).ToString("#.##");

            commonWordList = commonWordList.OrderByDescending(x => (x.Freq1 + x.Freq2)).ToList();
            dataGridView3.DataSource = commonWordList;
            label8.Text = commonWordList.Count().ToString();

            totalNumberOfText1 += words1.Length;
            totalNumberOfText2 += words2.Length;
        }

        private void button4_Click(object sender, EventArgs e)  // clear txt1
        {
            richTextBox1.Clear();
            localtemp1.Clear();
            dataGridView1.DataSource = richTextBox1;
            label3.Text = localtemp1.Count().ToString();
        }

        private void button5_Click(object sender, EventArgs e)  // clear txt 2
        {
            richTextBox2.Clear();
            localtemp2.Clear();
            dataGridView2.DataSource = richTextBox2;
            label2.Text = localtemp2.Count().ToString();
        }

        private void button6_Click(object sender, EventArgs e)  // add to mem
        {
            button6.Enabled = false;
            #region Temp1 initialization
            if (temp1.Count != 0)
            {
                foreach (var item in localtemp1)
                {
                    if (temp1.Exists(x=> x.Value==item.Value))
                    {
                        temp1.FirstOrDefault(x => x.Value == item.Value).Frequency += item.Frequency;
                    }
                    else
                    {
                        temp1.Add(item);
                    }
                }
            }
            else
            {
                temp1.AddRange(localtemp1);
            }
            #endregion endregion

            #region Temp2 initialization
            if (temp2.Count != 0)
            {
                foreach (var item in localtemp2)
                {
                    if (temp2.Exists(x => x.Value == item.Value))
                    {
                        temp2.FirstOrDefault(x => x.Value == item.Value).Frequency += item.Frequency;
                    }
                    else
                    {
                        temp2.Add(item);
                    }
                }
            }
            else
            {
                temp2.AddRange(localtemp2);
            }
            #endregion endregion
            temp1 = temp1.OrderByDescending(x => x.Frequency).ToList();
            temp2 = temp2.OrderByDescending(x => x.Frequency).ToList();
        }

        private void button7_Click(object sender, EventArgs e) // Clear Mem
        {
            temp1.Clear();
            temp2.Clear();
            dataGridView1.DataSource = temp1;
            dataGridView2.DataSource = temp2;
            label3.Text = temp1.Count().ToString();
            label2.Text = temp2.Count().ToString();
            button6.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e) // Show Mem  
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = temp1;
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = temp2;
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            label3.Text = temp1.Count().ToString();
            label2.Text = temp2.Count().ToString();

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        // comparation Method
        public List<CommonWord> CommonList(List<Word> list1, List<Word> list2)
        {
            List<CommonWord> commonWordList = new List<CommonWord>();
            foreach (var item1 in list1)
            {
                foreach (var item2 in list2)
                {
                    if (item2.Value == item1.Value)
                    {
                        commonWordList.Add(new CommonWord(item1.Value, item1.Frequency, item2.Frequency));
                    }
                }
            }
            return commonWordList;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        // Compare MEM
        private void button9_Click(object sender, EventArgs e)
        {
            List<CommonWord> commonWordList = CommonList(temp1, temp2);

            double percentage1 = Convert.ToDouble(commonWordList.Count()) / temp1.Count();
            label_uniqe_per1.Text = (percentage1 * 100).ToString("#.##");
            double percentage2 = Convert.ToDouble(commonWordList.Count()) / temp2.Count();
            label_uniqe_per2.Text = (percentage2 * 100).ToString("#.##");

            int per1 = 0;
            int per2 = 0;

            foreach (var item in commonWordList)
            {
                per1 += item.Freq1;
                per2 += item.Freq2;
            }

            label_per1.Text = ((Convert.ToDouble(per1) * 100) / totalNumberOfText1).ToString("#.##");
            label_per2.Text = ((Convert.ToDouble(per2) * 100) / totalNumberOfText2).ToString("#.##");

            commonWordList = commonWordList.OrderByDescending(x => (x.Freq1 + x.Freq2)).ToList();
            dataGridView3.DataSource = commonWordList;
            label8.Text = commonWordList.Count().ToString();
       }
    }
}
