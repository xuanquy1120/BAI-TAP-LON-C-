using EO.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CoCaro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loaddata();
        }
        QLP3Entities ql = new QLP3Entities();
        void loaddata()//load dữ liệu từ data xuống
        {
            var result = from c in ql.Plays
                         select c;
            dataGridView1.DataSource = result.ToList();
        }
        public void addNamePlay1()
        {
            var result = from c in ql.Plays
                         select c;
            foreach (var data in result)
            {
                if (data.Name == txtNamePlay2.Text)
                {
                    Play NguoiChoi1 = new Play();
                    {
                        NguoiChoi1.Name = txtNamePlay1.Text;
                        NguoiChoi1.wins = 0;
                        NguoiChoi1.loses = 0;
                    }
                    ql.Plays.Add(NguoiChoi1);
                }
            } 
            Play NguoiChoi2 = new Play();
                    {
                        NguoiChoi2.Name = txtNamePlay2.Text;
                        NguoiChoi2.wins = 0;
                        NguoiChoi2.loses = 0;
                    }
                    ql.Plays.Add(NguoiChoi2);
        }
        public void addNamePlay2()
        {
            var result = from c in ql.Plays
                         select c;
            foreach (var data in result)
            {
                if (data.Name == txtNamePlay1.Text)
                {
                    Play NguoiChoi2 = new Play();
                    {
                        NguoiChoi2.Name = txtNamePlay2.Text;
                        NguoiChoi2.wins = 0;
                        NguoiChoi2.loses = 0;
                    }
                    ql.Plays.Add(NguoiChoi2);
                }
            }
            Play NguoiChoi1 = new Play();
            {
                NguoiChoi1.Name = txtNamePlay1.Text;
                NguoiChoi1.wins = 0;
                NguoiChoi1.loses = 0;
            }
            ql.Plays.Add(NguoiChoi1);
        }
        public void Start()
        {
            Play NguoiChoi = new Play();
            {
                loaddata();
                this.Hide();
                Form2 xo = new Form2(txtNamePlay1.Text, txtNamePlay2.Text);
                xo.ShowDialog();
            }
        }
        public void addOneData()
        {
            addNamePlay1();            
            addNamePlay2();
            ql.SaveChanges();
            Start();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtNamePlay1.Text.Trim() != "" && txtNamePlay2.Text.Trim() != "")
            {
                var result = from c in ql.Plays
                             select c;
                foreach (var data in result)
                {
                    if (data.Name == txtNamePlay1.Text)
                    {
                        foreach (var data1 in result)
                        {
                            if (data1.Name == txtNamePlay2.Text)
                            {
                                Start();
                                break;
                            }                           
                        }
                    }             
                }
                addOneData();
            }
            else
                MessageBox.Show("Chưa nhập dữ liệu", "Thông báo", MessageBoxButtons.OKCancel);
        }
    }
}
