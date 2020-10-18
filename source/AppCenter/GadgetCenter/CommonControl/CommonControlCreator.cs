using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Gadget.Math.Data;
using System.Windows.Controls;
using GadgetCenter.CommonControl;
using System.Windows.Media;
using SoonLearning.AppCenter.Controls;

namespace Math.Basic.CommonControl
{
    public delegate void ContentPartCreated(UIElement element);

    internal static class CommonControlCreator
    {
        public static UIElement CreateUIPart(QuestionContentPart part)
        {
            if (part is QuestionBlank)
            {
                IntegerOnlyTextBox blankTextBox = new IntegerOnlyTextBox();
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
                    return CreateText(string.Empty);

                return CreateText(decimalPart.Value.ToString());
            }
            else if (part is ArithmeticFractionValuePart)
            {
                ArithmeticFractionValuePart fractionPart = part as ArithmeticFractionValuePart;

                FractionControl fractionControl = new FractionControl(20, FontWeights.Medium, 2, new SolidColorBrush(Colors.White));
                fractionControl.Numerator = fractionPart.Numerator;
                fractionControl.Denominator = fractionPart.Denominator;
                fractionControl.FontSize = 20;
                fractionControl.FontWeight = FontWeights.Medium;
                fractionControl.VerticalAlignment = VerticalAlignment.Center;
                return fractionControl;
            }
            else if (part is ArithmeticSignPart)
            {
            }
            else if (part is QuestionTextPart)
            {
                QuestionTextPart textPart = part as QuestionTextPart;

                return CreateText(textPart.Text);
            }
            else if (part is QuestionFlowDocumentPart)
            {

            }

            throw new NotSupportedException();
        }

        internal static Panel CreateContentControl(QuestionContent content, Panel contentPanel, ContentPartCreated contentPartCreated)
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
                                itemPanel.Children.Add(CreateText(temp));

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
                            itemPanel.Children.Add(CreateText(text));
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
                    UIElement element = CommonControlCreator.CreateUIPart(part);
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
                        itemPanel.Children.Add(CreateText(text));

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

        internal static Label CreateText(string text)
        {
            Label subContentLabel = new Label();
            subContentLabel.FontSize = 20;
            subContentLabel.Content = text;
            subContentLabel.FontWeight = FontWeights.Medium;
            subContentLabel.VerticalAlignment = VerticalAlignment.Center;
            subContentLabel.VerticalContentAlignment = VerticalAlignment.Center;
            return subContentLabel;
        }
    }
}
