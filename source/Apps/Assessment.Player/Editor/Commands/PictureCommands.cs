using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using SoonLearning.Assessment.Data;
using SoonLearning.Assessment.Player.Properties;

namespace SoonLearning.Assessment.Player.Editor
{
    public class PictureCommands
    {
        private static RoutedUICommand insertPictureCommand;
        private static RoutedUICommand insertHyperlinkCommand;
        private static RoutedUICommand editPictureCommand;

        public static RoutedUICommand InsertPictureCommand
        {
            get { return insertPictureCommand; }
        }

        /// <summary>
        /// Command for inserting hyperlink
        /// </summary>
        public static RoutedUICommand InsertHyperlinkCommand
        {
            get { return insertHyperlinkCommand; }
        }

        /// <summary>
        /// Command for editing picture
        /// </summary>
        public static RoutedUICommand EditPictureCommand
        {
            get { return editPictureCommand; }
        }

        static PictureCommands()
        {
            insertPictureCommand = new RoutedUICommand("InsertPicture", "InsertPicture", typeof(RichTextEditor));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextEditor),
            new CommandBinding(insertPictureCommand, OnInsertPicture, OnCanExecuteInsertPicture));

            insertHyperlinkCommand = new RoutedUICommand("InsertHyperlink", "InsertHyperlink", typeof(RichTextEditor));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextEditor),
            new CommandBinding(insertHyperlinkCommand, OnInsertHyperlink, OnCanExecuteInsertPicture));

            editPictureCommand = new RoutedUICommand("EditPicture", "EditPicture", typeof(RichTextEditor));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextEditor),
            new CommandBinding(editPictureCommand, OnEditPictureProperties, OnCanExecuteInsertPicture));
        }

        private static void OnCanExecuteInsertPicture(object target, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;

            RichTextEditor control = (RichTextEditor)target;
            RichTextBox richTextBox = control.RichTextBox;
            TextPointer insertionPosition = richTextBox.Selection.Start;

            // Disable pictures inside lists and hyperlinks
            if (Helper.HasAncestor(insertionPosition, typeof(List)) || Helper.HasAncestor(insertionPosition, typeof(Hyperlink)))
            {
                e.CanExecute = false;
            }
        }

        private static Hyperlink GetHyperlinkAncestor(TextPointer position)
        {
            Inline parent = position.Parent as Inline;

            while (parent != null && !(parent is Hyperlink))
            {
                parent = parent.Parent as Inline;
            }
            return parent as Hyperlink;
        }

        /// <summary>
        /// Handler for inserting of the hyperlink
        /// </summary>
        public static void OnInsertHyperlink(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextEditor control = (RichTextEditor)sender;
            RichTextBox richTextBox = control.RichTextBox;


            if (richTextBox.Selection.IsEmpty)
            {
                return;
            }

            TextPointer insertionPosition = richTextBox.Selection.Start;
            Paragraph paragraph = insertionPosition.Paragraph;
            Hyperlink hyperlink = null;

            Image image = Helper.GetImageAncestor(insertionPosition);
            InlineUIContainer inlineUIContainer = Helper.GetInlineUIContainer(insertionPosition);
            if (image != null)
            {
                OnEditPictureProperties(sender, e);
                return;
            }

            //textRange.Text
            //### Detect existing hyperlink
            //hyperlink = GetHyperlinkAncestor(insertionPosition);
            TextRange textRange = richTextBox.Selection;
            foreach (Inline inline in paragraph.Inlines)
            {
                //if (inline is Hyperlink && ((Hyperlink)inline).Tag is TextPointerPaar)
                if (inline is Hyperlink)
                {
                    hyperlink = (Hyperlink)inline;
                    TextRange textRangeHyper = new TextRange(hyperlink.ElementStart, hyperlink.ElementEnd);

                    //if (0 == richTextBox.Selection.Start.CompareTo(((TextPointerPaar)hyperlink.Tag).Start) && 0 == richTextBox.Selection.End.CompareTo(((TextPointerPaar)hyperlink.Tag).End))
                    if (textRange.Text == textRangeHyper.Text)
                    {
                        break;
                    }
                    else
                    {
                        hyperlink = null;
                    }
                }
            }

            HyperlinkPropertiesDialog hyperlinkPropertiesDlg = new HyperlinkPropertiesDialog(hyperlink);
            hyperlinkPropertiesDlg.ShowDialog();

            if (true == hyperlinkPropertiesDlg.DialogResult)
            {
                try
                {
                    Uri uri = new Uri(hyperlinkPropertiesDlg.NavigateUri);
                    if (null == hyperlink)
                    {
                        hyperlink = new Hyperlink(richTextBox.Selection.Start, richTextBox.Selection.End);
                        //hyperlink.Tag = new TextPointerPaar(richTextBox.Selection.Start,richTextBox.Selection.End);
                    }
                    hyperlink.NavigateUri = uri;
                    /*
                    if (String.IsNullOrEmpty(hyperlinkPropertiesDlg.HyperlinkDesc))
                    {
                        hyperlink.ToolTip = null;
                    }
                    else
                    {
                        ToolTip toolTip = new ToolTip();
                        toolTip.Content = hyperlinkPropertiesDlg.HyperlinkDesc;
                        hyperlink.ToolTip = toolTip;
                    }*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("{0} ({1}) :\n\r{2}", SoonLearning.Assessment.Player.Properties.Resources.txtLoadHyperlinkFailed, hyperlinkPropertiesDlg.NavigateUri, ex), SoonLearning.Assessment.Player.Properties.Resources.txtEditorName);
                }
            }

            //paragraph.Inlines.Add(hyperlink);
        }

        /// <summary>
        /// Handler for inserting of the picture
        /// </summary>
        public static void OnInsertLine(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextEditor control = (RichTextEditor)sender;
            RichTextBox richTextBox = control.RichTextBox;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Selection.Text = String.Empty;
            }

            TextPointer insertionPosition = richTextBox.Selection.Start;
            Paragraph paragraph = insertionPosition.Paragraph;

            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(7, 1);

            LineSegment myLineSegment = new LineSegment();
            myLineSegment.Point = new Point(800, 1);

            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            myPathSegmentCollection.Add(myLineSegment);

            myPathFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            // myPath.Height = 1;
            myPath.Data = myPathGeometry;


            //<Path  Height="10" Fill="Black" Stretch="Fill" Stroke="Black" StrokeThickness="1" Visibility="Visible" Data="M7,34 L390,34" />
            paragraph.Inlines.Add(myPath);
        }

        public static void OnInsertPicture(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextEditor control = (RichTextEditor)sender;
            RichTextBox richTextBox = control.RichTextBox;

            //###
            TextPointer textPosition = richTextBox.Selection.Start;

            Image image = Helper.GetImageAncestor(textPosition);
            if (null != image)
            {
                if (image.Tag is string)
                {
                    string tag = image.Tag as string;
                    if (!tag.StartsWith("_$FRACTION_"))
                    {
                        PictureCommands.OnEditPictureProperties(sender, e);
                        return;
                    }
                }
                else
                {
                    PictureCommands.OnEditPictureProperties(sender, e);
                    return;
                }
            }
            //###

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Selection.Text = String.Empty;
            }

            TextPointer insertionPosition = richTextBox.Selection.Start;

            ImagePropertiesDialog imageProperties = new ImagePropertiesDialog(null);
            imageProperties.ShowDialog();

            if (true == imageProperties.DialogResult)
            {
                try
                {
                    Paragraph paragraph = insertionPosition.Paragraph;

                    // Split current paragraph at insertion position
                //    insertionPosition = insertionPosition.InsertParagraphBreak();
                //    paragraph = insertionPosition.Paragraph;
                    //paragraph.Inlines.Add("Some ");

                    //AcmBitmapImageHolder acmBitmapImageHolder = new AcmBitmapImageHolder(new Uri(@"http://www.cetelem.cz/images/cetelem2/cetelem_logo_cl_small.gif"));
                    AcmBitmapImageHolder acmBitmapImageHolder = new AcmBitmapImageHolder(new Uri(imageProperties.ImagePath));

                    //acmBitmapImageHolder.Image.Style= sender.
                    paragraph.Inlines.Add(acmBitmapImageHolder.Image);
                    paragraph.Inlines.LastInline.BaselineAlignment = BaselineAlignment.Center;

                    if (!String.IsNullOrEmpty(imageProperties.ImageHyperlink))
                    {
                        //### Set hyperlink
                    //    acmBitmapImageHolder.Image.Tag = imageProperties.ImageHyperlink;
                    //    Hyperlink hyperlink = new Hyperlink(paragraph.Inlines.LastInline, insertionPosition);
                    //    hyperlink.NavigateUri = new Uri(imageProperties.ImageHyperlink);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("{0} ({1}) :\n\r{2}", SoonLearning.Assessment.Player.Properties.Resources.txtLoadPictureFailed, imageProperties.ImageHyperlink, ex),
                        SoonLearning.Assessment.Player.Properties.Resources.txtEditorName);
                }
            }
        }

        public static void InsertMathPicture(RichTextEditor control, string imageFile, string tag)
        {
            RichTextBox richTextBox = control.RichTextBox;

            //###
            TextPointer textPosition = richTextBox.Selection.Start;

            //###

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Selection.Text = String.Empty;
            }

            TextPointer insertionPosition = richTextBox.Selection.Start;

            try
            {
                Paragraph paragraph = insertionPosition.Paragraph;

                // Split current paragraph at insertion position
            //    insertionPosition = insertionPosition.InsertParagraphBreak();
            //    paragraph = insertionPosition.Paragraph;
                //paragraph.Inlines.Add("Some ");

                paragraph.Inlines.Add(new Run());

                AcmBitmapImageHolder acmBitmapImageHolder = new AcmBitmapImageHolder(new Uri(imageFile));

                //acmBitmapImageHolder.Image.Style= sender.
                paragraph.Inlines.Add(acmBitmapImageHolder.Image);
                paragraph.Inlines.LastInline.BaselineAlignment = BaselineAlignment.Center;
                
                if (!String.IsNullOrEmpty(imageFile))
                {
                    //### Set hyperlink
                    acmBitmapImageHolder.Image.Tag = tag;
                //    Hyperlink hyperlink = new Hyperlink(paragraph.Inlines.LastInline, insertionPosition);
                //    hyperlink.NavigateUri = new Uri(imageFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0} ({1}) :\n\r{2}", Resources.txtLoadPictureFailed, imageFile, ex),
                    SoonLearning.Assessment.Player.Properties.Resources.txtEditorName);
            }
        }

        /// <summary>
        /// Handler for editing of the picture
        /// </summary>
        public static void OnEditPictureProperties(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextEditor control = (RichTextEditor)sender;
            RichTextBox richTextBox = control.RichTextBox;
            TextPointer insertionPosition = richTextBox.Selection.Start;

            Image image = Helper.GetImageAncestor(insertionPosition);
            InlineUIContainer inlineUIContainer = Helper.GetInlineUIContainer(insertionPosition);
            if (image != null)
            {
                ImagePropertiesDialog imageProperties = new ImagePropertiesDialog(image);
                imageProperties.ShowDialog();
                if (true == imageProperties.DialogResult)
                {
                    try
                    {
                        if (imageProperties.PictureSourceChanged)
                        {
                            AcmBitmapImageHolder acmBitmapImageHolder = new AcmBitmapImageHolder(image, new Uri(imageProperties.ImagePath));
                        }
                        if (!String.IsNullOrEmpty(imageProperties.ImageHyperlink))
                        {
                            //### Set hyperlink
                            image.Tag = imageProperties.ImageHyperlink;
                            Hyperlink hyperlink = new Hyperlink(inlineUIContainer, insertionPosition);
                            hyperlink.NavigateUri = new Uri(imageProperties.ImageHyperlink);
                        }
                    //    control.OnRichTextBox_TextChanged(control, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format("{0} ({1}) :\n\r{2}", SoonLearning.Assessment.Player.Properties.Resources.txtLoadPictureFailed, imageProperties.ImageHyperlink, ex), SoonLearning.Assessment.Player.Properties.Resources.txtEditorName);
                    }
                }
            }
        }
    }
}
