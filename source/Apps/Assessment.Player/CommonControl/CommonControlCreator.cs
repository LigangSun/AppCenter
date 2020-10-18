using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SoonLearning.Assessment.Data;
using System.Windows.Controls;
using SoonLearning.AppCenter.Controls;
using System.Windows.Media;
using SoonLearning.AppCenter.Controls;
using HTMLConverter;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using SoonLearning.Assessment.Player.Entry;

namespace SoonLearning.Assessment.Player.CommonControl
{
    public delegate void ContentPartCreated(UIElement element);

    public static class CommonControlCreator
    {
        public static UIElement CreateUIPart(QuestionContentPart part, Brush foreground)
        {
            if (part is QuestionBlank)
            {
                NumberOnlyTextBox blankTextBox = new NumberOnlyTextBox();
                blankTextBox.Tag = part;
                blankTextBox.Width = 80;
                blankTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                blankTextBox.FontSize = 20;
                blankTextBox.Margin = new Thickness(1);
                return blankTextBox;
            }
            else if (part is ArithmeticDecimalValuePart)
            {
                ArithmeticDecimalValuePart decimalPart = part as ArithmeticDecimalValuePart;

                if (decimalPart.Value == null)
                    return CreateText(string.Empty, foreground);

                return CreateText(decimalPart.Value.ToString(), foreground);
            }
            else if (part is QuestionFractionPart)
            {
                QuestionFractionPart fractionPart = part as QuestionFractionPart;

                FractionControl fractionControl = new FractionControl(20, FontWeights.Medium, 2, new SolidColorBrush(Colors.Black));
                fractionControl.FractionPart = fractionPart;
                fractionControl.FontSize = 20;
                fractionControl.FontWeight = FontWeights.Medium;
                fractionControl.VerticalAlignment = VerticalAlignment.Center;
                fractionControl.Foreground = foreground;
                return fractionControl;
            }
            else if (part is ArithmeticSignPart)
            {
            }
            else if (part is QuestionTextPart)
            {
                QuestionTextPart textPart = part as QuestionTextPart;

                return CreateText(textPart.Text, foreground);
            }
            else if (part is QuestionPowerExponentPart)
            {
                PowerExponentControl powerExponentControl = new PowerExponentControl(20, FontWeights.Medium, new SolidColorBrush(Colors.Black));
                powerExponentControl.PowerExponentPart = part as QuestionPowerExponentPart;
                powerExponentControl.Foreground = foreground;

                return powerExponentControl;
            }
            else if (part is QuestionPowerPart)
            {

            }
            else if (part is QuestionFlowDocumentPart)
            {

            }
            else if (part is ArithmeticFractionValuePart)
            {
                ArithmeticFractionValuePart arithmeticFractionValuePart = part as ArithmeticFractionValuePart;
                ArithmeticFractionControl arithmeticFractionControl = new ArithmeticFractionControl(20, FontWeights.Medium, 2, foreground);
                arithmeticFractionControl.Denominator = arithmeticFractionValuePart.Denominator;
                arithmeticFractionControl.Numerator = arithmeticFractionValuePart.Numerator;
                arithmeticFractionControl.Foreground = foreground;
                return arithmeticFractionControl;
            }

            throw new NotSupportedException();
        }

        public static Panel CreateContentControl(QuestionContent content, Panel contentPanel, Brush foreground, ContentPartCreated contentPartCreated)
        {
            Panel stackPanel = contentPanel;
            if (stackPanel == null)
            {
                stackPanel = new StackPanel();
                ((StackPanel)stackPanel).Orientation = Orientation.Vertical;
            }

            if (content.ContentType == ContentType.Html ||
                content.ContentType == ContentType.FlowDocument)
            {
                FlowDocumentScrollViewer documentViewer = new FlowDocumentScrollViewer();
                documentViewer.FocusVisualStyle = null;
                documentViewer.VerticalContentAlignment = VerticalAlignment.Center;
                documentViewer.VerticalAlignment = VerticalAlignment.Center;
                documentViewer.HorizontalContentAlignment = HorizontalAlignment.Left;
                documentViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                documentViewer.Background = Brushes.Transparent;
                documentViewer.FontSize = 20;

                FlowDocument document = null;
                if (content.ContentType == ContentType.Html)
                    document = HtmlToXamlConverter.ConvertHtmlToXaml(content.Content);
                else
                    document = HtmlToXamlConverter.DeserializeFlowDocument(content.Content);
                replaceTextBoxWithRichTextBox(document, content, contentPartCreated);
                document.FontSize = 20;
                documentViewer.Document = document;

                stackPanel.Children.Add(documentViewer);
            }
            else
            {
                string questionContent = content.Content;
                if (string.IsNullOrEmpty(questionContent))
                    return stackPanel;

                int startIndex = 0;
                StackPanel itemPanel = new StackPanel();
                itemPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(itemPanel);
                bool newLine = false;
                while (true)
                {
                    startIndex = questionContent.IndexOf("_$", 0);
                    if (startIndex >= 0)
                    {
                        int endIndex = questionContent.IndexOf("$_", startIndex);
                        string placeHolder = questionContent.Substring(startIndex, endIndex - startIndex + 2);

                        string text = questionContent.Substring(0, startIndex);

                        if (!string.IsNullOrEmpty(text))
                        {
                            string[] textParts = text.Split(new char[] { '\n' });
                            if (textParts.Length > 1)
                            {
                                for (int i = 0; i < textParts.Length; i++)
                                {
                                    string temp = textParts[i];
                                    itemPanel.Children.Add(CreateText(temp, foreground));

                                    if (i == textParts.Length - 1)
                                        break;

                                    StackPanel nextItemPanel = new StackPanel();
                                    nextItemPanel.Orientation = Orientation.Horizontal;
                                    stackPanel.Children.Add(nextItemPanel);
                                    itemPanel = nextItemPanel;
                                }
                            }
                            else
                            {
                                itemPanel.Children.Add(CreateText(text, foreground));
                            }
                        }
                        //else
                        //{
                        //    string[] textParts = text.Split(new char[] { '\n' });
                        //    if (textParts.Length > 1)
                        //    {
                        //        foreach (string temp in textParts)
                        //        {
                        //            itemPanel.Children.Add(CreateText(temp));

                        //            StackPanel nextItemPanel = new StackPanel();
                        //            nextItemPanel.Orientation = Orientation.Horizontal;
                        //            stackPanel.Children.Add(nextItemPanel);
                        //            itemPanel = nextItemPanel;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        itemPanel.Children.Add(CreateText(text));
                        //    }
                        //}

                        if (newLine)
                        {
                            StackPanel nextItemPanel = new StackPanel();
                            nextItemPanel.Orientation = Orientation.Horizontal;
                            stackPanel.Children.Add(nextItemPanel);
                            itemPanel = nextItemPanel;
                        }

                        newLine = false;

                        QuestionContentPart part = content.GetContentPart(placeHolder);
                        UIElement element = CommonControlCreator.CreateUIPart(part, foreground);
                        itemPanel.Children.Add(element);
                        if (contentPartCreated != null)
                            contentPartCreated(element);

                        questionContent = questionContent.Remove(0, endIndex + 2);
                        if (string.IsNullOrEmpty(questionContent))
                            break;
                    }
                    else
                    {
                        string[] textParts = questionContent.Split(new char[] { '\n' });
                        foreach (string text in textParts)
                        {
                            itemPanel.Children.Add(CreateText(text, foreground));

                            StackPanel nextItemPanel = new StackPanel();
                            nextItemPanel.Orientation = Orientation.Horizontal;
                            stackPanel.Children.Add(nextItemPanel);
                            itemPanel = nextItemPanel;
                        }

                        break;
                    }
                }
            }

            return stackPanel;
        }

        internal static TextBlock CreateText(string text, Brush foreground)
        {
            TextBlock subContentLabel = new TextBlock();
            subContentLabel.FontSize = 20;
            subContentLabel.Text = text;
            subContentLabel.FontWeight = FontWeights.Medium;
            subContentLabel.VerticalAlignment = VerticalAlignment.Top;
         //   subContentLabel.VerticalContentAlignment = VerticalAlignment.Top;
            subContentLabel.Foreground = foreground;
            return subContentLabel;
        }

        public static void replaceTextBoxWithRichTextBox(FlowDocument document, QuestionContent content, ContentPartCreated contentPartCreated)
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

                    Uri newUri = new Uri(getImageFile(image.Tag as string, content));
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

                    RichTextBox richTextBox = new RichTextBox();
                    richTextBox.Width = 120;
                    richTextBox.Height = 30;
                    richTextBox.Margin = new Thickness(3);
                    richTextBox.Tag = textBox.Tag;
                    inlineUIContainer.Child = richTextBox;
                    if (contentPartCreated != null)
                        contentPartCreated(richTextBox);
                }
            }
        }

        internal static string getImageFile(string holder, QuestionContent content)
        {
            string path = System.IO.Path.GetDirectoryName(AssessmentAppControl.Instance.AssessmentFile);
            path = System.IO.Path.Combine(path, "Images");

            foreach (var part in content.QuestionPartCollection)
            {
                if (part.PlaceHolder == holder)
                {
                    return System.IO.Path.Combine(path, part.Id + ".png");
                }
            }

            return System.IO.Path.Combine(path, System.IO.Path.GetFileName(holder));
        }
    }
}
