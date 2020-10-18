using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading;
using SoonLearning.Assessment.Data;
using SoonLearning.Assessment.Player.Properties;
using SoonLearning.Assessment.Player.CommonControl;
using HTMLConverter;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace SoonLearning.Assessment.Player.Data
{
    internal class BaseObjectToFlowDocument
    {
        internal static FlowDocument CreateExerciseFlowDocument(Exercise exercise, bool showAnswer, bool showResponse)
        {
            string questionPaper = Resources.QuestionPaperDocument;
            questionPaper = questionPaper.Replace("_$PAPER_TITLE$_", exercise.Title);
            questionPaper = questionPaper.Replace("_$PAPER_DESPRITON$_", exercise.Description);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(questionPaper));

            FlowDocument flowDocument = (FlowDocument)XamlReader.Load(ms);

            int index = 0;
            foreach (SoonLearning.Assessment.Data.Section section in exercise.SectionCollection)
            {
                CreateSection(section, index, flowDocument, showAnswer, showResponse, exercise);
            }

            return flowDocument;
        }

        internal static FlowDocument CreateExamFlowDocument(Exam exam, bool showAnswer, bool showResponse)
        {
            string questionPaper = Resources.QuestionPaperDocument;
            questionPaper = questionPaper.Replace("_$PAPER_TITLE$_", exam.Title);
            questionPaper = questionPaper.Replace("_$PAPER_DESPRITON$_", exam.Description);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(questionPaper));

            FlowDocument flowDocument = (FlowDocument)XamlReader.Load(ms);

            int index = 0;
            foreach (SoonLearning.Assessment.Data.Section section in exam.SectionCollection)
            {
                CreateSection(section, index, flowDocument, showAnswer, showResponse, exam);
            }

            return flowDocument;
        }

        private static void CreateSection(SoonLearning.Assessment.Data.Section section, 
            int index, 
            FlowDocument flowDocument,
            bool showAnswer,
            bool showResponse,
            Exercise exercise)
        {
            Paragraph sectionPara = new Paragraph();
            flowDocument.Blocks.Add(sectionPara);

            // Section Title
            Run runTItle = new Run(section.Title + section.Description);
            sectionPara.Inlines.Add(runTItle);

            // Create Questions
            for (int i = 0; i < section.QuestionCollection.Count; i++)
            {
                CreateQuestion(section.QuestionCollection[i],
                    i,
                    flowDocument,
                    showAnswer, 
                    showResponse,
                    exercise.GetQuestionResponse(section.QuestionCollection[i].Id));
            }
        }

        private static void CreateQuestion(Question question,
            int index,
            FlowDocument flowDocument, 
            bool showAnswer,
            bool showResponse,
            Response response)
        {
            if (question is TableQuestion)
            {
                CreateTableQuestion(question as TableQuestion, index, flowDocument, showAnswer, showResponse, response as SelectableQuestionResponse);
            }
            else if (question is SelectableQuestion)
            {
                CreateMCQuestion(question as SelectableQuestion, index, flowDocument, showAnswer, showResponse, response as SelectableQuestionResponse);
            }
            else if (question is FIBQuestion)
            {
                CreateFIBQuestion(question as FIBQuestion, index, flowDocument, showAnswer, showResponse, response as FIBQuestionResponse);
            }
            else if (question is ESQuestion)
            {
                CreateESQuestion(question as ESQuestion, index, flowDocument, showAnswer, showResponse, response as ESQuestionResponse);
            }
        }

        private static void CreateMCQuestion(SelectableQuestion question, 
            int index,
            FlowDocument flowDocument,
            bool showAnswer, 
            bool showResponse,
            SelectableQuestionResponse response)
        {
            System.Windows.Documents.Section questionSection = new System.Windows.Documents.Section();
            flowDocument.Blocks.Add(questionSection);

            // Shuffle options
            List<int> optionIndexList = new List<int>();
            if (question.RandomOption)
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                while (true)
                {
                    int i = rand.Next(question.QuestionOptionCollection.Count * 10) % question.QuestionOptionCollection.Count;
                    bool found = false;
                    foreach (int temp in optionIndexList)
                    {
                        if (temp == i)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        optionIndexList.Add(i);

                    if (optionIndexList.Count == question.QuestionOptionCollection.Count)
                        break;
                    
                    Thread.Sleep(10);
                }
            }
            else
            {
                for (int i = 0; i < question.QuestionOptionCollection.Count; i++)
                {
                    optionIndexList.Add(i);
                }
            }

            System.Windows.Documents.Section questionContentBlock = CreateContentControl(question.Content, (index + 1).ToString() + ". ", null, response);
            Paragraph lastParagraph = null;
            if (questionContentBlock.Blocks.LastBlock is Paragraph)
                lastParagraph = questionContentBlock.Blocks.LastBlock as Paragraph;
            if (lastParagraph == null)
            {
                lastParagraph = new Paragraph();
                questionContentBlock.Blocks.Add(lastParagraph);
            }
            if (showResponse)
            {
                if (Enumerable.Count<string>(response.OptionIdList) > 0)
                {
                    for (int i = 0; i < question.QuestionOptionCollection.Count; i++)
                    {
                        QuestionOption option = question.QuestionOptionCollection[optionIndexList[i]];
                        if (option.Id == response.OptionIdList[0])
                        {
                            lastParagraph.Inlines.Add(CreateText(" ( " + (char)('A' + i) + " )"));
                            break;
                        }
                    }
                }
                else
                {
                    lastParagraph.Inlines.Add(CreateText("(    )"));
                }
            }
            else
            {
                lastParagraph.Inlines.Add(CreateText("(    )"));
            }

            if (showAnswer)
            {
                for (int i = 0; i < question.QuestionOptionCollection.Count; i++)
                {
                    QuestionOption option = question.QuestionOptionCollection[optionIndexList[i]];
                    if (option.IsCorrect)
                    {
                        lastParagraph.Inlines.Add(CreateText("    正确答案：" + (char)('A' + i)));
                        break;
                    }
                }
            }

            questionSection.Blocks.Add(questionContentBlock);

            for (int i = 0; i < question.QuestionOptionCollection.Count; i++)
            {
                QuestionOption option = question.QuestionOptionCollection[optionIndexList[i]];
                System.Windows.Documents.Section optionSec = CreateContentControl(option.OptionContent, string.Format("\t{0}. ", (char)('A' + i)), null, response);
                List<Block> tempList = new List<Block>();
                tempList.AddRange(optionSec.Blocks);
                optionSec.Blocks.Clear();

                questionSection.Blocks.AddRange(tempList);
            }
        }

        private static void CreateFIBQuestion(FIBQuestion question,
            int index,
            FlowDocument flowDocument, 
            bool showAnswer, 
            bool showResponse,
            FIBQuestionResponse response)
        {
            System.Windows.Documents.Section questionSection = new System.Windows.Documents.Section();
            flowDocument.Blocks.Add(questionSection);

            System.Windows.Documents.Section questionContentBlock = CreateContentControl(question.Content,
                (index + 1).ToString() + ". ",
                null,
                showResponse ? response : null);
            Paragraph lastParagraph = null;
            if (questionContentBlock.Blocks.LastBlock is Paragraph)
                lastParagraph = questionContentBlock.Blocks.LastBlock as Paragraph;
            if (lastParagraph == null)
            {
                lastParagraph = new Paragraph();
                questionContentBlock.Blocks.Add(lastParagraph);
            }

            if (showAnswer)
            {
                lastParagraph.Inlines.Add(CreateText("。   正确答案："));
                foreach (QuestionBlank blank in question.QuestionBlankCollection)
                {
                    lastParagraph.Inlines.Add(CreateText(blank.ReferenceAnswerList[0].Content + "  "));
                }
            }

            questionSection.Blocks.Add(questionContentBlock);
        }

        private static Paragraph CreateTableQuestion(TableQuestion question, 
            int index,
            FlowDocument flowDocument,
            bool showAnswer,
            bool showResponse,
            SelectableQuestionResponse response)
        {
            Paragraph questionPara = new Paragraph();
            flowDocument.Blocks.Add(questionPara);

            Run questionBodyRun = new Run((index + 1).ToString() + ". " + question.Content.Content + "\n");
            questionPara.Inlines.Add(questionBodyRun);

            Table optionTable = new Table();
            TableRowGroup tableRowGroup = new TableRowGroup();
            optionTable.RowGroups.Add(tableRowGroup);
            flowDocument.Blocks.Add(optionTable);

            int i = 0;
            int rowCount = (int)System.Math.Sqrt(question.QuestionOptionCollection.Count);
            for (int row = 0; row < rowCount; row++)
            {
                tableRowGroup.Rows.Add(new TableRow());
                TableRow tableRow = tableRowGroup.Rows[row];
                for (int col = 0; col < rowCount; col++)
                {
                    QuestionOption option = question.QuestionOptionCollection[i++];

                    System.Windows.Documents.Section optionPara = CreateContentControl(option.OptionContent, string.Empty, null, response);
                    if (showResponse)
                    {
                        if (Enumerable.Count<string>(Enumerable.Where<string>(response.OptionIdList, (c => (c == option.Id)))) > 0)
                        {
                            optionPara.FontWeight = FontWeights.Bold;
                            optionPara.FontSize = 16f;
                            optionPara.FontStyle = FontStyles.Italic;
                        }
                    }
                    TableCell cell = new TableCell(optionPara);
                    cell.BorderThickness = new Thickness(1);
                    cell.BorderBrush = new SolidColorBrush(Colors.Black);
                    cell.TextAlignment = TextAlignment.Center;
                    cell.LineHeight = 30f;
                    tableRow.Cells.Add(cell);
                }
            }

            if (showAnswer)
            {
                System.Windows.Documents.Section answerSection = new System.Windows.Documents.Section();

                Paragraph answerPara = new Paragraph();

                answerSection.Blocks.Add(answerPara);

                flowDocument.Blocks.Add(answerSection);

                answerPara.Inlines.Add("答案：");
                foreach (QuestionOption option in question.QuestionOptionCollection)
                {
                    if (option.IsCorrect)
                    {
                        CreateContentControl(option.OptionContent, " ", answerSection, response);
                        answerPara.Inlines.Add(", ");
                    }
                }
            }

            return questionPara;
        }

        private static void CreateESQuestion(ESQuestion question,
            int index,
            FlowDocument flowDocument,
            bool showAnswer,
            bool showResponse,
            ESQuestionResponse response)
        {
            System.Windows.Documents.Section questionSection = new System.Windows.Documents.Section();
            flowDocument.Blocks.Add(questionSection);

            System.Windows.Documents.Section questionContentBlock = CreateContentControl(question.Content,
                (index + 1).ToString() + ". ",
                null,
                showResponse ? response : null);

            Paragraph lastParagraph = null;
            if (questionContentBlock.Blocks.LastBlock is Paragraph)
                lastParagraph = questionContentBlock.Blocks.LastBlock as Paragraph;
            if (lastParagraph == null)
            {
                lastParagraph = new Paragraph();
                questionContentBlock.Blocks.Add(lastParagraph);
            }
            lastParagraph.Inlines.AddRange(new LineBreak[] { new LineBreak(), new LineBreak(), new LineBreak() });

            if (showAnswer)
            {
                
            }

            questionSection.Blocks.Add(questionContentBlock);
        }

        internal static System.Windows.Documents.Section CreateContentControl(QuestionContent content, string prefix, System.Windows.Documents.Section paragraph, Response response)
        {
            System.Windows.Documents.Section rootSection = paragraph;
            if (rootSection == null)
                rootSection = new System.Windows.Documents.Section();
            
            if (content.ContentType == ContentType.Html ||
                content.ContentType == ContentType.FlowDocument)
            {
                string doc = string.Empty;
                if (content.ContentType == ContentType.Html)
                    doc = HtmlToXamlConverter.ConvertHtmlToXaml(content.Content, true);
                else
                    doc = content.Content;

                System.Windows.Documents.FlowDocument flowDocument = HtmlToXamlConverter.DeserializeFlowDocument(doc);

                replaceTextBoxWithText(flowDocument, content, null);

                List<Block> tempList = new List<Block>();
                tempList.AddRange(flowDocument.Blocks);
                flowDocument.Blocks.Clear();                      
                rootSection.Blocks.AddRange(tempList);

                Paragraph firstPara = null;
                if (rootSection.Blocks.FirstBlock is Paragraph)
                {
                    firstPara = rootSection.Blocks.FirstBlock as Paragraph;
                }
                else
                {
                    firstPara = new Paragraph();
                    rootSection.Blocks.InsertBefore(rootSection.Blocks.FirstBlock, firstPara);
                }

                if (firstPara.Inlines.Count > 0)
                    firstPara.Inlines.InsertBefore(firstPara.Inlines.FirstInline, new Run(prefix));
                else
                    firstPara.Inlines.Add(prefix);
            }
            else
            {
                Paragraph para = null;
                if (rootSection.Blocks.LastBlock is Paragraph)
                    para = rootSection.Blocks.LastBlock as Paragraph;
                else
                {
                    para = new Paragraph();
                    rootSection.Blocks.Add(para);
                }

                string questionContent = content.Content;
                int startIndex = 0;

                para.Inlines.Add(prefix);

                while (true)
                {
                    startIndex = questionContent.IndexOf("_$", 0);
                    if (startIndex >= 0)
                    {
                        int endIndex = questionContent.IndexOf("$_", startIndex);
                        string placeHolder = questionContent.Substring(startIndex, endIndex - startIndex + 2);

                        string text = questionContent.Substring(0, startIndex);

                        if (!string.IsNullOrEmpty(text) &&
                            text[text.Length - 1] == '\n')
                        {
                            para.Inlines.Add(CreateText(text.Remove(text.Length - 1)));
                        }
                        else
                        {
                            para.Inlines.Add(CreateText(text));
                        }

                        QuestionContentPart part = content.GetContentPart(placeHolder);
                        if (response is FIBQuestionResponse)
                        {
                            FIBQuestionResponse fibResponse = response as FIBQuestionResponse;
                            para.Inlines.Add(CreateUIPart(part, fibResponse.GetBlankResponse(part.Id, false)));
                        }
                        else
                        {
                            para.Inlines.Add(CreateUIPart(part, null));
                        }

                        questionContent = questionContent.Remove(0, endIndex + 2);
                        if (string.IsNullOrEmpty(questionContent))
                            break;
                    }
                    else
                    {
                        para.Inlines.Add(CreateText(questionContent));
                        break;
                    }
                }
            }

            return rootSection;
        }

        public static Inline CreateUIPart(QuestionContentPart part, Response response)
        {
            if (part is QuestionBlank)
            {
                if (response is QuestionBlankResponse)
                {
                    QuestionBlankResponse blankResponse = response as QuestionBlankResponse;

                    System.Windows.Documents.Section blankPara = CreateContentControl(blankResponse.BlankContent, string.Empty, null, null);

                    Paragraph lastParagraph = null;
                    if (blankPara.Blocks.LastBlock is Paragraph)
                        lastParagraph = blankPara.Blocks.LastBlock as Paragraph;
                    if (lastParagraph == null)
                    {
                        lastParagraph = new Paragraph();
                        blankPara.Blocks.Add(lastParagraph);
                    }

                    Span span = new Span();
                    span.Inlines.Add("( ");

                    List<Inline> inlineList = new List<Inline>();
                    inlineList.AddRange(lastParagraph.Inlines);
                    foreach (Inline inline in inlineList)
                    {
                        span.Inlines.Add(inline);
                    }
                    span.Inlines.Add(" )");
                    span.BaselineAlignment = BaselineAlignment.Center;
                    return span;
                }

                Run blankRun = new Run("(    )");
                blankRun.BaselineAlignment = BaselineAlignment.Center;
                return blankRun;
            }
            else if (part is ArithmeticDecimalValuePart)
            {
                ArithmeticDecimalValuePart decimalPart = part as ArithmeticDecimalValuePart;

                if (decimalPart.Value == null)
                    return CreateText(string.Empty);

                return CreateText(decimalPart.Value.ToString());
            }
            else if (part is ArithmeticFractionValuePart)
            {
                ArithmeticFractionValuePart fractionPart = part as ArithmeticFractionValuePart;

                return CreateFraction(fractionPart);
            }
            else if (part is ArithmeticSignPart)
            {
            }
            else if (part is QuestionTextPart)
            {
                QuestionTextPart textPart = part as QuestionTextPart;

                return CreateText(textPart.Text);
            }
            else if (part is QuestionPowerExponentPart)
            {
                return CreatePowerExponent(part as QuestionPowerExponentPart);
            }
            else if (part is QuestionPowerPart)
            {
            }
            else if (part is QuestionFlowDocumentPart)
            {

            }

            throw new NotSupportedException();
        }

        internal static Inline CreateText(string text)
        {
            Run subContentRun = new Run();
            subContentRun.Text = text;
            subContentRun.BaselineAlignment = BaselineAlignment.Center;
            return subContentRun;
        }

        internal static Inline CreateFraction(ArithmeticFractionValuePart fractionPart)
        {
            InlineUIContainer uiContainer = new InlineUIContainer();
            uiContainer.BaselineAlignment = BaselineAlignment.Center;
            ArithmeticFractionControl fractionControl = new ArithmeticFractionControl(14, FontWeights.Normal, 1, new SolidColorBrush(Colors.Black));
            fractionControl.Numerator = fractionPart.Numerator;
            fractionControl.Denominator = fractionPart.Denominator;
            fractionControl.VerticalAlignment = VerticalAlignment.Center;
            uiContainer.Child = fractionControl;
            uiContainer.BaselineAlignment = BaselineAlignment.Center;
            return uiContainer;
        }

        internal static Inline CreatePowerExponent(QuestionPowerExponentPart powerExponentPart)
        {
            InlineUIContainer uiContainer = new InlineUIContainer();
            uiContainer.BaselineAlignment = BaselineAlignment.Center;
            PowerExponentControl powerExponentControl = new PowerExponentControl(20, FontWeights.Medium, new SolidColorBrush(Colors.Black));
            powerExponentControl.PowerExponentPart = powerExponentPart;
            uiContainer.Child = powerExponentControl;
            uiContainer.BaselineAlignment = BaselineAlignment.Center;
            return uiContainer;
        }

        internal static Inline CreateBlankWithResponse(FIBQuestionResponse fibResponse)
        {
            return null;
        }

        private static void replaceTextBoxWithText(FlowDocument document, QuestionContent content, ContentPartCreated contentPartCreated)
        {
            List<InlineUIContainer> tempList = new List<InlineUIContainer>();

            foreach (var block in document.Blocks)
            {
                if (block is Paragraph)
                {
                    Paragraph para = block as Paragraph;
                    foreach (var inline in para.Inlines)
                    {
                        if (inline is InlineUIContainer)
                        {
                            tempList.Add(inline as InlineUIContainer);
                        }
                    }
                }
            }

            foreach (var inlineUIContainer in tempList)
            {
                if (inlineUIContainer.Child is Image) // replace image url with local url.
                {
                    Image image = inlineUIContainer.Child as Image;

                    Image newImage = new Image();

                    Uri newUri = new Uri(CommonControlCreator.getImageFile(image.Tag as string, content));
                    BitmapImage newBi = new BitmapImage(newUri);
                    newImage.Source = newBi;
                    newImage.Width = (1.0 == newBi.Width ? 50 : newBi.Width);
                    newImage.Height = (1.0 == newBi.Height ? 50 : newBi.Height);
                    newImage.Tag = image.Tag;
                    inlineUIContainer.Child = newImage;
                }
                else if (inlineUIContainer.Child is TextBox) // replace textbox with richtextbox
                {
                    TextBox textBox = inlineUIContainer.Child as TextBox;

                    TextBlock richTextBox = new TextBlock();
                    richTextBox.Text = "(                )";
                    richTextBox.Tag = textBox.Tag;
                    inlineUIContainer.Child = richTextBox;
                    if (contentPartCreated != null)
                        contentPartCreated(richTextBox);
                }
            }
        }
    }
}
