using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using SoonLearning.AppCenter.Utility;
using System.Windows.Documents;
using System.Windows.Markup;

namespace SoonLearning.Math.Data
{
    public delegate void QuestionGeneratingDelegate(int index);

    public abstract class QuestionData
    {
        public event QuestionGeneratingDelegate QuestionGeneratingEvent;
        public event QuestionGeneratingDelegate QuestionGeneratedEvent;

        public const string ext = "qdf";

        private ObservableCollection<Question> questionCollection = new ObservableCollection<Question>();
        protected int maxNumber = 10;
        protected int minNumber = 0;
        private int examTime = 5;
        private int questionCount = 0;
        private int leftTime = 0; // in second
        private float pointPerQuestion = 1.0f;
        private int passQuizCorrectCount = 0;
        private string id = Guid.NewGuid().ToString("N");
        private DateTime createTime = DateTime.Now;

        private BackgroundWorker worker;

        public ObservableCollection<Question> Items
        {
            get { return this.questionCollection; }
        }

        public string Id
        {
            get { return this.id; }
        }

        public int MaxNumber
        {
            get { return this.maxNumber; }
            set { this.maxNumber = value; }
        }

        public int MinNumber
        {
            get { return this.minNumber; }
            set { this.minNumber = value; }
        }

        public DateTime CreateTime
        {
            get { return this.createTime; }
        }

        public int ExamTime
        {
            get { return this.examTime; }
            set 
            { 
                this.examTime = value;
                this.leftTime = this.examTime * 60;
            }
        }

        public int LeftTime
        {
            get { return this.leftTime; }
            set 
            { 
                this.leftTime = value;
                this.AutoSave();
            }
        }

        public string UsedTimeText
        {
            get
            {
                int usedTime = this.examTime * 60 - this.leftTime;
                return string.Format("{0}:{1}", (usedTime / 60).ToString("00"), (usedTime % 60).ToString("00"));
            }
        }

        public string RangeText
        {
            get
            {
                return string.Format("{0}以内", this.maxNumber);
            }
        }

        public float PointPerQuestion
        {
            get { return this.pointPerQuestion; }
            set { this.pointPerQuestion = value; }
        }

        public int PassQuizCorrectCount
        {
            get { return this.passQuizCorrectCount; }
            set { this.passQuizCorrectCount = value; }
        }

        public int QuestionCount
        {
            get { return this.questionCollection.Count; }
        }

        public int CorrectCount
        {
            get
            {
                int correct = 0;
                foreach (Question_a_b_c q in this.questionCollection)
                {
                    if (q.IsCorrect != null && q.IsCorrect.Value)
                        correct++;
                }

                return correct;
            }
        }

        public int NotAnswerCount
        {
            get
            {
                int noAnswer = 0;
                foreach (Question_a_b_c q in this.questionCollection)
                {
                    if (q.IsCorrect == null)
                        noAnswer++;
                }

                return noAnswer;
            }
        }

        public int AnsweredCount
        {
            get { return this.questionCollection.Count - this.NotAnswerCount; }
        }

        public int IncorrectCount
        {
            get
            {
                int incorrect = 0;
                foreach (Question_a_b_c q in this.questionCollection)
                {
                    if (q.IsCorrect != null && !q.IsCorrect.Value)
                        incorrect++;
                }

                return incorrect;
            }
        }

        public bool AllQuestionAnswered
        {
            get
            {
                foreach (Question_a_b_c q in this.questionCollection)
                {
                    if (q.Result == null)
                        return false;
                }

                return true;
            }
        }

        public float Score
        {
            get
            {
                float ret = 0.0f;
                foreach (Question_a_b_c q in this.questionCollection)
                {
                    if (q.IsCorrect != null &&
                        q.IsCorrect.Value)
                        ret += this.pointPerQuestion;
                }

                return ret;
            }
        }

        public abstract int MaxResultNumber
        {
            get;
        }

        public QuestionData()
        {
            this.maxNumber = MathSetting.Instance.MaxNumber;
            this.minNumber = MathSetting.Instance.MinNumber;
            this.questionCount = MathSetting.Instance.QuestionCount;
            this.examTime = MathSetting.Instance.ExamTime;
            this.pointPerQuestion = MathSetting.Instance.PointPerQuestion;
            this.passQuizCorrectCount = MathSetting.Instance.PassQuizCorrectCount;
        }

        internal void Save(string file)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenWrite(file);

                // Write header
                string header = CreateHeader();
                byte[] headerData = Encoding.ASCII.GetBytes(header);
                fs.Write(headerData, 0, headerData.Length);

                // Write data
                WriteText(fs, this.id);
                WriteText(fs, this.createTime.ToString());
                WriteValue(fs, this.maxNumber);
                WriteValue(fs, this.examTime);
                WriteValue(fs, this.questionCount);
                WriteValue(fs, this.leftTime);
                WriteValue(fs, this.pointPerQuestion);
                WriteValue(fs, this.passQuizCorrectCount);
                WriteValue(fs, this.questionCollection.Count);
                foreach (Question question in this.questionCollection)
                {
                    question.Save(fs);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        protected abstract string CreateHeader();

        internal void AutoSave()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string dataFolder = System.IO.Path.GetDirectoryName(assembly.Location);
            dataFolder = System.IO.Path.Combine(dataFolder, "Data\\Rapid");
            if (!System.IO.Directory.Exists(dataFolder))
                System.IO.Directory.CreateDirectory(dataFolder);
            string dataFile = System.IO.Path.Combine(dataFolder, this.id + "." + ext);
            this.Save(dataFile);
        }

        public static QuestionData Load(string file)
        {
            QuestionData data = null;
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(file);

                // Read Header;
                byte[] headerData = new byte[38];
                fs.Read(headerData, 0, 38);

                string header = Encoding.ASCII.GetString(headerData);
                if (header == MathSetting.Instance.CurrentQuestionHeader)
                {
                    data = MathSetting.Instance.GetQuestionData();
                }

                data.id = ReadText(fs);
                data.createTime = DateTime.Parse(ReadText(fs));
                data.maxNumber = (int)ReadValue(fs);
                data.examTime = (int)ReadValue(fs);
                data.questionCount = (int)ReadValue(fs);
                data.leftTime = (int)ReadValue(fs);
                data.pointPerQuestion = (float)ReadValue(fs);
                data.passQuizCorrectCount = (int)ReadValue(fs);
                int questionCount = (int)ReadValue(fs);
                for (int i = 0; i < questionCount; i++)
                {
                    data.questionCollection.Add(Question.Load(fs));
                }
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return data;
        }

        protected abstract Question GenerateQuestion();

        public void Generate(int count)
        {
            if (this.worker != null)
                return;

            this.worker = new BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            this.worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            this.worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.questionCollection.Clear();

            List<Question> questionList = new List<Question>();
            List<Question> repeatList = new List<Question>();

            int sleepTime = 50;// 1000 / maxNumber;

            int getCount = (int)(this.questionCount * 1.2);
            for (int i = 0; i < getCount; i++)
            {
                try
                {
                    Question question = this.GenerateQuestion(); 
                    question.Index = i;

                    bool found = false;
                    foreach (Question temp in questionList)
                    {
                        if (temp.Equals(question))
                        {
                            repeatList.Add(question);
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        continue;

                    questionList.Add(question);

                    if (questionList.Count <= this.questionCount)
                    {
                        worker.ReportProgress(questionList.Count);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }

                Thread.Sleep(sleepTime);
            }

            Random rand = new Random(this.questionCount * 8 * 8);

            if (questionList.Count < this.questionCount)
            {
                int leftCount = this.questionCount - questionList.Count;
                for (int i = 0; i < leftCount; i++)
                {
                    int randValue = rand.Next(repeatList.Count - 1);
                    Question question = repeatList[randValue];
                    questionList.Add(question);
                    repeatList.RemoveAt(randValue);

                    if (questionList.Count <= this.questionCount)
                    {
                        worker.ReportProgress(questionList.Count);
                    }
                }
            }

            for (int i = 0; i < this.questionCount; i++)
            {
                int randValue = rand.Next(questionList.Count - 1);
                Console.WriteLine(randValue);

                Question question = questionList[randValue];
                question.Index = i;

                this.questionCollection.Add(questionList[randValue]);

                questionList.RemoveAt(randValue);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.QuestionGeneratingEvent != null)
                this.QuestionGeneratingEvent(e.ProgressPercentage);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.QuestionGeneratedEvent != null)
                this.QuestionGeneratedEvent(this.questionCount);
        }

        internal static void WriteText(Stream stream, string text)
        {
            byte[] textData = Encoding.UTF8.GetBytes(text);
            byte[] textDataLength = BitConverter.GetBytes(textData.Length);
            stream.Write(textDataLength, 0, 4);
            stream.Write(textData, 0, textData.Length);
        }

        internal static string ReadText(Stream stream)
        {
            byte[] textDataLength = new byte[4];
            stream.Read(textDataLength, 0, 4);

            byte[] textData = new byte[BitConverter.ToInt32(textDataLength, 0)];
            stream.Read(textData, 0, textData.Length);
            return Encoding.UTF8.GetString(textData);
        }

        internal static void WriteValue(Stream stream, double value)
        {
            byte[] valueData = BitConverter.GetBytes(value);
            stream.Write(valueData, 0, valueData.Length);
        }

        internal static double ReadValue(Stream stream)
        {
            byte[] valueData = new byte[8];
            stream.Read(valueData, 0, 8);
            return BitConverter.ToDouble(valueData, 0);
        }

        internal FlowDocument ToFlowDocument(PrintInfo info)
        {
            string questionPaper = SoonLearning.Math.Fast.Properties.Resources.QuestionPaperDocument;
            questionPaper = questionPaper.Replace("_$PAPER_TITLE$_", info.Title);
            if (info.ShowDateTime)
                questionPaper = questionPaper.Replace("_$PAPER_DESPRITON$_", DateTime.Now.ToString());
            else
                questionPaper = questionPaper.Replace("_$PAPER_DESPRITON$_", string.Empty);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(questionPaper));

            FlowDocument flowDocument = (FlowDocument)XamlReader.Load(ms);
            Paragraph questionPara;
            string line = string.Empty;
            int colIndex = 0;
            for (int i = 0; i < this.questionCollection.Count; i++)
            {
                if (line != string.Empty)
                    line += "\t\t\t\t";
                line += this.CreateQuestionBlock(this.questionCollection[i], info.ShowAnswer, info.ShowResult);
                colIndex++;
                if (colIndex == info.CountPerLine)
                {
                    colIndex = 0;

                    questionPara = new Paragraph();
                    questionPara.TextAlignment = System.Windows.TextAlignment.Center;
                    questionPara.Inlines.Add(line);
                    flowDocument.Blocks.Add(questionPara);

                    line = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(line))
            {
                questionPara = new Paragraph();
                questionPara.Inlines.Add(line);
                flowDocument.Blocks.Add(questionPara);
            }

            return flowDocument;
        }

        private string CreateQuestionBlock(Question question, bool showAnswer, bool showResult)
        {
            if (question is Question_a_b_c)
            {
                Question_a_b_c abcQuestion = question as Question_a_b_c;
                string questionText = question.ToString();
                if (showResult && abcQuestion.Result != null)
                    questionText += abcQuestion.Result.Value;
                if (showAnswer)
                    questionText += string.Format("\t{0}", abcQuestion.C);

                return questionText;
            }

            return string.Empty;
        }
    }
}
