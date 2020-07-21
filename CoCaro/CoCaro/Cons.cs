using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCaro
{
    public class Cons // lớp hằng số lưu các chỉ số kích thước bàn cờ ô cờ,....

    {
        public static int Chess_Width = 30;     //kích thước 1 ô cờ
        public static int Chess_Height = 30;

        public static int Chess_Board_Width = 16;   // kich thước bàn cờ
        public static int Chess_Board_Height = 14;

        public static int CoolDownStep = 100;
        public static int CoolDownTime = 10000; //10k mili giay = 10s
        public static int CoolDownInterval = 100;     //sau bao lâu process bar tăng lên 1 lần

    }
}
