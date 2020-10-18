using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// Exercise类是创建练习的类，同时也是考试类Exam的基类。
    /// </summary>
    public class Exercise : BaseObject
    {
        private SectionCollection sectionCollection = new SectionCollection();
        private DateTime createTime;
        private int currentSectionIndex = -1;
        private ExerciseResponse response;

        /// <summary>
        /// 获取或设置练习/测验的标题。
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的描述。
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        // -1: Infinit retry 0: No retry
        /// <summary>
        /// 获取或设置练习/测验允许用户重试的次数。
        /// -1表示可以无限次重试，0表示不能重试。
        /// </summary>
        public int RetryTimes
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的评分方式。
        /// AutoAfterEachQuestion: 答完一题后自动评分。
        /// AutoAfterExam: 练习/测验后自动评分。
        /// Manual: 手动评分。
        /// </summary>
        public MarkType MarkType
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的答案显示方式。
        /// None: 不显示答案。
        /// AutoAfterEachQuestion: 答完一题后自动显示答案。
        /// AutoAfterExam: 完成练习/测验后显示答案。
        /// Manual: 手动查看答案。
        /// </summary>
        public DisplayAnswerType DisplayAnswerType
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的解题方法显示方式。
        /// None: 不显示解题方法。
        /// AutoAfterEachQuestion: 答完一题后自动显示解题方法。
        /// AutoAfterExam: 完成练习/测验后显示解题方法。
        /// Manual: 手动查看解题方法。
        /// </summary>
        public DisplaySolutionType DisplaySolutionType
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的反馈显示方式。
        /// None: 不显示解题方法。
        /// AfterEachQuestion: 答完一题后自动显示反馈。
        /// AfterExam: 完成练习/测验后显示反馈。
        /// </summary>
        public DisplayFeedbackType DisplayFeedbackType
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的反馈内容。
        /// </summary>
        public Feedback Feedback
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验是否显示分数。
        /// </summary>
        public bool DisplayMark
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验是否显示得分。
        /// </summary>
        public bool DisplayScore
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的试题显示方式。
        /// Full: 在一页内显示整个试卷。
        /// Section: 在一页内显示一道大题的所有试题。
        /// Question: 在一页内显示一道试题。
        /// </summary>
        public QuestionDisplayType QuestionDisplayType
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验每页显示的试题数目。
        /// </summary>
        public int QuestionCountPerPage
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的创建时间。
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.createTime.ToLocalTime(); }
            set { this.createTime = value.ToUniversalTime(); }
        }

        /// <summary>
        /// 获取或设置练习/测验的考生，指那位学生可以使用该试卷。
        /// </summary>
        public User User
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置练习/测验的用时。
        /// </summary>
        public TimeSpan UsedTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取练习/测验的下一道大题内容。
        /// </summary>
        public Section NextSection
        {
            get
            {
                if (this.currentSectionIndex < this.sectionCollection.Count - 1)
                {
                    this.currentSectionIndex++;
                    return this.sectionCollection[this.currentSectionIndex];
                }

                return null;
            }
        }

        /// <summary>
        /// 获取练习/测验的上一道大题内容。
        /// </summary>
        public Section PreSection
        {
            get
            {
                if (this.currentSectionIndex > 0)
                {
                    this.currentSectionIndex--;
                    return this.sectionCollection[this.currentSectionIndex];
                }

                return null;
            }
        }

        /// <summary>
        /// 获取练习/测验的下一道试题。
        /// </summary>
        public Question NextQuestion
        {
            get
            {
                Section section = null;
                if (this.currentSectionIndex == -1)
                    section = this.NextSection;
                else
                    section = this.sectionCollection[this.currentSectionIndex];

                if (section.IsLastQuestion)
                {
                    section = this.NextSection;
                    if (section == null)
                        return null;
                    return section.FirstQuestion;
                }

                if (section == null)
                    return null;

                return section.NextQuestion;
            }
        }

        /// <summary>
        /// 获取练习/测验的上一道试题。
        /// </summary>
        public Question PreQuestion
        {
            get
            {
                Section section = null;
                if (this.currentSectionIndex == -1)
                    section = this.NextSection;
                else
                    section = this.sectionCollection[this.currentSectionIndex];

                if (section.IsFirstQuestion)
                {
                    section = this.PreSection;
                    if (section == null)
                        return null;
                    return section.LastQuestion;
                }

                if (section == null)
                    return null;

                return section.PreQuestion;
            }
        }

        /// <summary>
        /// 获取练习/测验的当前试题的序号。
        /// </summary>
        public int QuestionIndex
        {
            get { return this.sectionCollection[this.currentSectionIndex].QuestionIndex; }
        }

        /// <summary>
        /// 获取练习/测验的当前大题。
        /// </summary>
        public Section CurrentSection
        {
            get { return this.sectionCollection[this.currentSectionIndex]; }
        }

        /// <summary>
        /// 获取练习/测验的当前大题的序号。
        /// </summary>
        public int CurrentSectionIndex
        {
            get { return this.currentSectionIndex; }
        }

        /// <summary>
        /// 获取练习/测验的大题列表。
        /// </summary>
        public SectionCollection SectionCollection
        {
            get { return this.sectionCollection; }
        }

        /// <summary>
        /// 获取练习/测验的答题结果。
        /// </summary>
        public ExerciseResponse ExerciseResponse
        {
            get
            {
                return this.response;
            }
        }

        /// <summary>
        /// 初始化Exercise类的新实例。
        /// </summary>
        public Exercise()
        {
            this.createTime = DateTime.Now.ToUniversalTime();
            this.response = new ExerciseResponse(base.Id);
        }

        /// <summary>
        /// 获取试题的答题结果。
        /// </summary>
        /// <param name="questionId">试题的Id</param>
        /// <returns>返回一个QuestionResponse实例。</returns>
        public QuestionResponse GetQuestionResponse(string questionId)
        {
            QuestionResponse ret = null;
            foreach (QuestionResponse response in this.response.QuestionResponseList)
            {
                if (response.ObjectId == questionId)
                {
                    ret = response;
                    break;
                }
            }

            if (ret == null)
            {
                ret = this.CreateQuestionResponse(questionId);
                this.response.QuestionResponseList.Add(ret);
            }

            return ret;
        }

        private QuestionResponse CreateQuestionResponse(string questionId)
        {
            QuestionResponse response;

            Question question = this.FindQuestion(questionId);
            if (question is MCQuestion ||
                question is MRQuestion ||
                question is MAQuestion ||
                question is TableQuestion ||
                question is TFQuestion)
                response = new SelectableQuestionResponse(questionId);
            else if (question is FIBQuestion)
                response = new FIBQuestionResponse(questionId);
            else if (question is ESQuestion)
                response = new ESQuestionResponse(questionId);
            else
                response = new QuestionResponse(questionId);

            return response;
        }

        private Question FindQuestion(string questionId)
        {
            foreach (Section section in this.SectionCollection)
            {
                foreach (Question question in section.QuestionCollection)
                {
                    if (question.Id == questionId)
                        return question;
                }
            }

            throw new DllNotFoundException(string.Format("The Question not exist. ID: {0}", questionId));
        }
    }
}
