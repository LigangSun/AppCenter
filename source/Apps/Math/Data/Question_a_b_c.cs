using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Math.Data
{
    public class Question_a_b_c : Question, IComparable<Question_a_b_c>
    {
        private int a;
        private int b;
        private int c;
        private int? result;
        private Operator op;

        public const int type = 0;

        public int A
        {
            get { return this.a; }
            set { this.a = value; }
        }

        public int B
        {
            get { return this.b; }
            set { this.b = value; }
        }

        public int C
        {
            get { return this.c; }
            set { this.c = value; }
        }

        public int? Result
        {
            get { return this.result; }
            set
            {
                this.result = value;
                OnPropertyChanged("Result");
                this.IsCorrect = true;
            }
        }

        public bool? IsCorrect
        {
            get 
            {
                if (this.result == null)
                    return null;

                return this.c == this.result;
            }
            set
            {
                OnPropertyChanged("IsCorrect");
            }
        }

        public Operator Op
        {
            get { return this.op; }
            set { this.op = value; }
        }

        public Question_a_b_c()
        {
        }

        public Question_a_b_c(int a, int b, int c, Operator op)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.op = op;
        }

        public override bool Equals(object obj)
        {
            return (this.CompareTo(obj as Question_a_b_c) == 0);
        }

        public int CompareTo(Question_a_b_c other)
        {
            if (other == null)
                return -1;

            if (other.a == this.a &&
                other.b == this.b &&
                other.c == this.c &&
                other.op == this.op)
                return 0;

            return 1;
        }

        public override void Save(System.IO.Stream stream)
        {
            QuestionData.WriteValue(stream, type);
            QuestionData.WriteValue(stream, this.a);
            QuestionData.WriteValue(stream, this.b);
            QuestionData.WriteValue(stream, (double)this.op);
            QuestionData.WriteValue(stream, this.c);
            if (this.result == null)
                QuestionData.WriteText(stream, "{NULL}");
            else
                QuestionData.WriteText(stream, this.Result.Value.ToString());
            if (this.IsCorrect == null)
                QuestionData.WriteText(stream, "{NULL}");
            else
                QuestionData.WriteText(stream, this.IsCorrect.Value.ToString());
        }

        protected override void LoadQuestion(System.IO.Stream stream)
        {
            this.a = (int)QuestionData.ReadValue(stream);
            this.b = (int)QuestionData.ReadValue(stream);
            this.op = (Operator)QuestionData.ReadValue(stream);
            this.c = (int)QuestionData.ReadValue(stream);

            string resultText = QuestionData.ReadText(stream);
            if (resultText == "{NULL}")
                this.result = null;
            else
                this.result = Convert.ToInt32(resultText);

            string isCorrectText = QuestionData.ReadText(stream);
            if (isCorrectText == "{NULL}")
                this.IsCorrect = null;
            else
                this.IsCorrect = Convert.ToBoolean(isCorrectText);
        }

        public override int Type
        {
            get { return type; }
        }

        public override string ToString()
        {
            OperatorStringConverter converter = new OperatorStringConverter();
            string opStr = (string)converter.Convert(this.op, null, null, null);

            return string.Format("{0} {1} {2} = ", this.A, opStr, this.B);
        }
    }
}
