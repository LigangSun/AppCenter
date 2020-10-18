using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SoonLearning.AppCenter.Controls;
using System.Windows.Media;
using SoonLearning.Assessment.Data;

namespace SoonLearning.Math.UI
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

            return stackPanel;
        }

        internal static Label CreateText(string text, Brush foreground)
        {
            Label subContentLabel = new Label();
            subContentLabel.FontSize = 20;
            subContentLabel.Content = text;
            subContentLabel.FontWeight = FontWeights.Medium;
            subContentLabel.VerticalAlignment = VerticalAlignment.Center;
            subContentLabel.VerticalContentAlignment = VerticalAlignment.Center;
            subContentLabel.Foreground = foreground;
            return subContentLabel;
        }
    }
}
