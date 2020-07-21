using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCaro
{
    public partial class Form2 : Form
    {
        private string currentNamePlayer1, currentNamePlayer2;
        public Form2(string namePlay1,string namePlay2) 
        {
            InitializeComponent();
            drawChessBoard();
            loaddata();

            currentNamePlayer1 = namePlay1;
            currentNamePlayer2 = namePlay2;
            this.Player = new List<Player>()
             {
             new Player(currentNamePlayer1, Image.FromFile(Application.StartupPath + "\\Resources\\xanh.jpg")),
             new Player(currentNamePlayer2, Image.FromFile(Application.StartupPath + "\\Resources\\do.jpg")),
               };   
            currentplayer = 0;
            changePlayer();
            pb_CoolDown.Step = Cons.CoolDownStep;
            pb_CoolDown.Maximum = Cons.CoolDownTime;
            pb_CoolDown.Value = 0;
            tmCoolDown.Interval = Cons.CoolDownInterval;

            tmCoolDown.Start();
            this.playMarked += chessBoard_PlayMarked;
            this.endedGame += chessBoard_EndedGame;

        }

        public Form2()
        {
        }

        QLP3Entities ql = new QLP3Entities();        
        private List<Player> Player;
        public List<Player> Player1 { get => Player; set => Player = value; }

        private int currentplayer;

        private TextBox tb_PlayName;
        public TextBox Tb_PlayName { get => tb_PlayName; set => tb_PlayName = value; } // set tb tên ng chơi để nó nhận giá trị tương ứng vs ng chơi hiện hành

        private PictureBox pb_play;
        public PictureBox Pb_play { get => pb_play; set => pb_play = value; } // set ảnh ng chơi để nó nhận giá trị ảnh của ng chơi hiện hành


        List<List<Button>> matrix;
        public List<List<Button>> Matrix { get => matrix; set => matrix = value; }
        

        void drawChessBoard() //vẽ bàn cờ
        {
            pn_chessBoard.Enabled = true;
            pn_chessBoard.Controls.Clear();
            matrix = new List<List<Button>>();
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) }; // OLD bt để lưu nút cũ, kích thước nó
                                                                                       //ban đầu lơn nên đặt width =0 để nó ở đầu dòng
            for (int i = 0; i < Cons.Chess_Board_Height; i++)
            {
                matrix.Add(new List<Button>());
                for (int j = 0; j < Cons.Chess_Board_Width; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cons.Chess_Width,
                        Height = Cons.Chess_Height,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y), // lấy vị trí ở kế bên nút cũ, vs vị trí đc xác định là kích thước nút cũ
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString() //xác định à mày nằm ở hàng i côt j
                    };
                    btn.Click += btn_click; //phát sinh sự kiện button click cho all button
                    pn_chessBoard.Controls.Add(btn);
                    matrix[i].Add(btn);

                    oldButton = btn; // lưu old button để tái sử dụng
                }
                //mỗi lần xong 1 hàng

                oldButton.Location = new Point(0, oldButton.Location.Y + Cons.Chess_Height);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }

        }

        void mark(Button btn) //hàm xét nút cho ng chơi, kiểm tra xem lượt của ng chơi nào
        {
            btn.BackgroundImage = Player[currentplayer].Mark; // button của người chơi hiện tại sẽ là ảnh mark tương ứng
            currentplayer = currentplayer == 1 ? 0 : 1; // người chơi hiện tai là 1 thì đổi sang 0 và ngược lại
        }
        void changePlayer() // hàm đổi lượt của ng chơi
        {
            tb_PlayerName.Text = Player[currentplayer].Name; // lượt người chơi nào chơi sẽ hiện tên ng đó
            pb_player.Image = Player[currentplayer].Mark;   // lượt ng nào chơi sẽ hiện ảnh ng đó
        }
        void EndGame()
        {
            tmCoolDown.Stop();
            pn_chessBoard.Enabled = false;
            MessageBox.Show("Người chiến thắng là :" + "\n"+Player[currentplayer].Name, "kết thúc trò chơi", MessageBoxButtons.OKCancel);
        }
        void chessBoard_PlayMarked(object sender, EventArgs e)
        {
            tmCoolDown.Start();
            pb_CoolDown.Value = 0;

        }
        void chessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
        }
        bool isEndGame(Button btn)  //hàm kiểm tra đã kết thúc gàme chưa
        {
            
            return (checkNgang(btn) || checkDoc(btn) || checkCheo1(btn) || checkCheo2(btn));
        }
        private Point GetChessPoint(Button btn)
        {
            int doc = Convert.ToInt32(btn.Tag);
            int ngang = matrix[doc].IndexOf(btn);
            Point point = new Point(ngang, doc);
            return point;
        }
        bool checkNgang(Button btn) // kết thúc khi có 5 ngang

        {
            Point point = GetChessPoint(btn);
            int countLeft = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if (matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;
            }

            int countRight = 0;
            for (int i = point.X + 1; i < Cons.Chess_Board_Width; i++)
            {
                if (matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;
            }
  
            return (countLeft + countRight) > 4;

        }
        bool checkDoc(Button btn) //5 dọc là end 
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = point.Y + 1; i < Cons.Chess_Board_Height; i++)
            {
                if (matrix[i][point.Y].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return (countTop + countBottom) > 4;


        }
        bool checkCheo1(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;
                if (matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Cons.Chess_Board_Width - point.X; i++)
            {
                if (point.Y + i >= Cons.Chess_Board_Height || point.X + i >= Cons.Chess_Board_Width)
                    break;
                if (matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return (countTop + countBottom) > 4; ;

        }
        bool checkCheo2(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Cons.Chess_Board_Width || point.Y - i < 0)
                    break;
                if (matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Cons.Chess_Board_Width - point.X; i++)
            {
                if (point.Y + i >= Cons.Chess_Board_Height || point.X - i < 0)
                    break;
                if (matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return (countTop + countBottom) > 4;

        }


        //public void EndGame()
        //{
        //    if (endedGame != null)
        //         endedGame(this, new EventArgs());
        // }
        void btn_click(object sender, EventArgs e) //sự kiện click vào các ô button
        {
            Button btn = sender as Button;
            if (btn.BackgroundImage != null) // nếu ô ko rỗng kết thúc 
                return;
            mark(btn);
            changePlayer();
            if (playMarked != null)
                playMarked(this, new EventArgs());

            if (isEndGame(btn))
            {
                //đổi dữ liệu người chơi
                currentplayer = currentplayer == 1 ? 0 : 1;
                changePlayer();
                //edit điểm chơi
                Play diemWin = ql.Plays.Find(Player[currentplayer].Name);
                diemWin.wins +=1;
                //đổi dữ liệu người chơi
                currentplayer = currentplayer == 1 ? 0 : 1;
                changePlayer();
                //edit điểm chơi
                Play diemLose = ql.Plays.Find(Player[currentplayer].Name);
                diemLose.loses += 1;
                ql.SaveChanges();
                loaddata();
                currentplayer = currentplayer == 1 ? 0 : 1;
                changePlayer();
                EndGame();                
            }
        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            pb_CoolDown.PerformStep();
            if (pb_CoolDown.Value >= pb_CoolDown.Maximum)
            {
                //Đổi dữ liệu người chơi
                currentplayer = currentplayer == 1 ? 0 : 1;
                changePlayer();
                //edit điểm chơi
                Play diemWin = ql.Plays.Find(Player[currentplayer].Name);
                diemWin.wins += 1;
                //Đổi dữ liệu người chơi
                currentplayer = currentplayer == 1 ? 0 : 1;
                changePlayer();
                //edit điểm chơi
                Play diemLose = ql.Plays.Find(Player[currentplayer].Name);
                diemLose.loses += 1;
                ql.SaveChanges();
                loaddata();
                //Đổi dữ liệu người chơi
                currentplayer = currentplayer == 1 ? 0 : 1;
                changePlayer();
                EndGame();
            }
        }

        private event EventHandler playMarked;
        public event EventHandler PlayMarked
        {
            add
            {
                playMarked += value;
            }
            remove
            {
                playMarked -= value;
            }
        }

        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add
            {
                endedGame += value;
            }
            remove
            {
                endedGame -= value;
            }
        }
        void loaddata()//load dữ liệu từ data xuống
        {
            var result = from c in ql.Plays
                         select c;
            dataGridView1.DataSource = result.ToList();
        }     
        private void pn_chessBoard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void quitGameToolStripMenuItem_Click(object sender, EventArgs e)//thoát chương trình
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.Cancel)
                Application.Exit();
        }

        private void newGameToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            pb_CoolDown.Value = 0; //trả giá trị thanh process bar về đầu
            tmCoolDown.Stop();  //dừng chạy timer
            drawChessBoard(); // vẽ lại bàn cờ
        }  
        void deletedata()//xóa dữ liệu data trên tên người chơi nhập vào
        {
            string play1 = txbDelete.Text;
            Play play = ql.Plays.Where(p => p.Name == play1).SingleOrDefault();
            ql.Plays.Remove(play);
            ql.SaveChanges();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (txbDelete.Text.Trim() != "")
            {
                if(txbDelete.Text!= currentNamePlayer1 && txbDelete.Text != currentNamePlayer2)
                {
                    deletedata();
                    loaddata();
                    dataGridView1.Show();
                }
                else
                {
                    MessageBox.Show("Tên người chơi đang dùng", "Thông báo", MessageBoxButtons.OKCancel);
                }
            }
            else
            {
                MessageBox.Show("Chưa nhập dữ liệu", "Thông báo", MessageBoxButtons.OKCancel);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)//tạm dừng game
        {
            tmCoolDown.Stop();
        }

        private void dataPlayerToolStripMenuItem_Click(object sender, EventArgs e)//chuyển sang form1
        {
            tmCoolDown.Stop();
            this.Hide();
            Form1 mlem = new Form1();
            mlem.ShowDialog();
        }

        private void txbDelete_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)//tiếp tục game
        {
            tmCoolDown.Start();
        }
    }
}
