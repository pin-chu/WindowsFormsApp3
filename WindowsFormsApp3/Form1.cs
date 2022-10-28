using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private GuessNumber game;
        public Form1()
        {
            InitializeComponent();

            game = new GuessNumber();
            resultLabel.Text=String.Empty;

        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            game.NewGame();
            resultLabel.Text = game.Hint;
        }

        private void guessButton_Click(object sender, EventArgs e)
        {
            int? guessNumber = GetGuessNumber();
            if (guessNumber.HasValue)
            {
                MessageBox.Show("您輸入的是"+guessNumber.Value);
            }
            else
            {
                MessageBox.Show("請輸入整數,在試一次");
            }

            GuessResult result = game.Guess(guessNumber.Value);
            if(result.IsSuccess==true)
            {
                MessageBox.Show("您答對了");
            }
            else
            {
                MessageBox.Show("您答錯了");
                resultLabel.Text = result.Hint;
            }
        }

        /// <summary>
        /// 取得使用者輸入的值
        /// </summary>
        /// <returns></returns>

        private int? GetGuessNumber()
        {
            TextBox txt = this.guessTextBox;
            string input = txt.Text;
            if (string.IsNullOrEmpty(input)) return null;

            bool isInt = int.TryParse(input, out int number);
            return isInt ? number : (int?)null;

        }
        public class GuessNumber
        {
            const int minValue = 1;
            const int maxValue = 99;

            private int answer;//這次遊戲的答案
            //目前的數值範圍
            private int min = 1, max = 99;
            /// <summary>
            /// 新遊戲,重新生出一個亂數[1~99]
            /// </summary>
            public void NewGame()
            {
                int seed = Guid.NewGuid().GetHashCode();
                var random = new Random(seed);
                answer = random.Next(1, 100);
                min = 1;
                max = 99;

            }
            /// <summary>
            /// 猜猜看,並回傳猜測結果
            /// </summary>
            /// <param name="guessNumber"></param>
            /// <returns></returns>
            public GuessResult Guess(int guessNumber){
                if(this.answer== guessNumber)
                {
                    //對了
                    return GuessResult.Success();
                    //return new GuessResult { IsSuccess = true, Hint = string.Empty };
                }
                //錯了
                //更新範圍1~99,answer=50,猜12=>更新12~99
                //1.如果猜的答案根本不再範圍內,就不必更動
                if(guessNumber<min||guessNumber>max)
                {
                    //return GuessResult.Failed(string errMessage);
                    return new GuessResult { IsSuccess = false, Hint = this.Hint};
                }
                //2.變更範圍
                if(guessNumber<answer)
                {
                    min = guessNumber;
                }
                else
                {
                    max = guessNumber;
                }

                //傳回答錯的資訊
                return new GuessResult { IsSuccess = false, Hint = this.Hint };
                //return GuessResult.Failed(string errMessage);


            }
            public string Hint
            {
                get
                {
                    return $"{min}~{max},answer ={answer}";
                }
            }
        }
        public class GuessResult
        {
            public static GuessResult Success()
            {
                return new GuessResult { IsSuccess = true,Hint=String.Empty };
            }
            public static GuessResult Failed(string errMessage)
            {
                return new GuessResult { IsSuccess = false, Hint = errMessage};
            }
            public bool IsSuccess { get; set; }
            public bool IsFailed
            {
                get
                {
                    return IsSuccess;
                }
            }
        
            public string Hint { get; set; }
        }

    }
    
}
