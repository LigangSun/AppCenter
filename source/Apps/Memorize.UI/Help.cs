using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.Memorize.UI
{
    internal class Help
    {
        public static string ResultMessage
        {
            get
            {
                try
                {
                    if (MemorizeDataMgr.Instance.Entry.IsPkMode)
                    {
                        if (MemorizeDataMgr.Instance.CurrentTimingMode == TimingMode.Count)
                        {
                            return "你勇敢的挑战了记忆力无敌的电脑，并战胜了他，太厉害了，原来你才是真正的记忆大师！";
                        }
                        else
                        {
                            string message = string.Empty;
                            if (!MemorizeDataMgr.Instance.IsLastStage)
                                message = "你在短短的一分钟内成功的挑战了电脑{0}关，成绩很不错，继续努力，来争取更好的成绩吧！";
                            else
                                message = "你在短短的一分钟内成功的挑战了电脑{0}关，成绩很不错，原来你才是真正的记忆大师！";

                            return string.Format(message,
                                MemorizeDataMgr.Instance.CurrentStage);
                        }
                    }
                    else
                    {
                        if (MemorizeDataMgr.Instance.CurrentTimingMode == TimingMode.Count)
                        {
                            TimeSpan timeSpan = TimeSpan.FromSeconds(MemorizeDataMgr.Instance.UsedTime);
                            return string.Format("你在{0}内通过记忆力挑战的所有关卡，成绩相当不错，让你的朋友们也来挑战一下吧！",
                                string.Format("{0}分{1}秒", timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00")));
                        }
                        else
                        {
                            string message = string.Empty;
                            if (!MemorizeDataMgr.Instance.IsLastStage)
                            {
                                if (MemorizeDataMgr.Instance.CurrentStage == 0)
                                    message = "你在短短的一分钟内成功的通过了{0}关，别灰心，再挑战一次试试吧！";
                                else
                                    message = "你在短短的一分钟内成功的通过了{0}关，成绩很不错，，继续努力，来争取更好的成绩吧！";
                            }
                            else
                                message = "你在短短的一分钟内成功的通过了{0}关，成绩很不错，原来你才是真正的记忆大师！";

                            return string.Format(message,
                                MemorizeDataMgr.Instance.CurrentStage);
                        }
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}
