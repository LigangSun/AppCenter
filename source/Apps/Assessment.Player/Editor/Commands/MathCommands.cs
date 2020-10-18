using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SoonLearning.Assessment.Player.Editor;
using System.Windows.Controls;
using System.Windows.Documents;
using SoonLearning.Assessment.Data;
using System.Windows;

namespace SoonLearning.Assessment.Player.Editor
{
    public class MathCommands
    {
        private static RoutedCommand insertFractionCommand;
        private static RoutedCommand editFractionCommand;
        private static RoutedCommand superscriptCommand;
        private static RoutedCommand subscriptCommand;
        private static RoutedCommand insertBlankCommand;
                           
        private static ExecutedRoutedEventHandler InsertFractionExcuted;
        private static CanExecuteRoutedEventHandler InsertFractionCanExcute;

        private static ExecutedRoutedEventHandler EditFractionExcuted;
        private static CanExecuteRoutedEventHandler EditFractionCanExcute;

        private static ExecutedRoutedEventHandler SuperscriptExcuted;
        private static CanExecuteRoutedEventHandler SuperscriptCanExcute;

        private static ExecutedRoutedEventHandler SubscriptExcuted;
        private static CanExecuteRoutedEventHandler SubscriptCanExcute;

        private static ExecutedRoutedEventHandler InsertBlankExcuted;
        private static CanExecuteRoutedEventHandler InsertBlankCanExcute;

        public static RoutedCommand InsertFractionCommand
        {
            get { return insertFractionCommand; }
        }

        public static RoutedCommand EditFractionCommand
        {
            get { return editFractionCommand; }
        }

        public static RoutedCommand SuperscriptCommand
        {
            get { return superscriptCommand; }
        }

        public static RoutedCommand SubscriptCommand
        {
            get { return subscriptCommand; }
        }

        public static RoutedCommand InsertBlankCommand
        {
            get { return insertBlankCommand; }
        }

        static MathCommands()
        {
            HandleExcute();

            insertFractionCommand = new RoutedCommand("InsertFraction", typeof(RichTextEditor));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextEditor),
                new CommandBinding(insertFractionCommand, InsertFractionExcuted, InsertFractionCanExcute));

            editFractionCommand = new RoutedCommand("EditFraction", typeof(RichTextEditor));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextEditor),
                new CommandBinding(editFractionCommand, EditFractionExcuted, EditFractionCanExcute));

            superscriptCommand = new RoutedCommand("Superscript", typeof(RichTextEditor));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextEditor),
                new CommandBinding(superscriptCommand, SuperscriptExcuted, SuperscriptCanExcute));

            subscriptCommand = new RoutedCommand("Subscript", typeof(RichTextEditor));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextEditor),
                new CommandBinding(subscriptCommand, SubscriptExcuted, SubscriptCanExcute));

            insertBlankCommand = new RoutedCommand("InsertBlank", typeof(RichTextEditor));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextEditor),
                new CommandBinding(insertBlankCommand, InsertBlankExcuted, InsertBlankCanExcute));
        }

        private static void HandleExcute()
        {
            InsertFractionExcuted = (s, e) => {
                insertFraction(s as RichTextEditor);
            };

            InsertFractionCanExcute = (s, e) => {
                e.CanExecute = canInsertFraction(s as RichTextEditor);
            };

            EditFractionExcuted = (s, e) =>
            {
                editFraction(s as RichTextEditor);
            };

            EditFractionCanExcute = (s, e) =>
            {
                e.CanExecute = canEditFraction(s as RichTextEditor);
            };

            SuperscriptExcuted = (s, e) =>
            {
                superscript(s as RichTextEditor);
            };

            SuperscriptCanExcute = (s, e) =>
            {
                e.CanExecute = canSuperscript(s as RichTextEditor);
            };

            SubscriptExcuted = (s, e) =>
            {
                subscript(s as RichTextEditor);
            };

            SubscriptCanExcute = (s, e) =>
            {
                e.CanExecute = canSubscript(s as RichTextEditor);
            };

            InsertBlankExcuted = (s, e) =>
            {
                insertBlank(s as RichTextEditor);
            };

            InsertBlankCanExcute = (s, e) =>
            {
                e.CanExecute = canInsertBlank(s as RichTextEditor);
            };
        }

        private static void insertFraction(RichTextEditor editor)
        {
            FractionDialog dlg = new FractionDialog();
            if (dlg.ShowDialog().Value)
            {
                PictureCommands.InsertMathPicture(editor, dlg.FractionImage, dlg.FractionPart.PlaceHolder);
                editor.Parts.Add(dlg.FractionPart);
            }
        }

        private static bool canInsertFraction(RichTextEditor editor)
        {
            TextPointer insertionPosition = editor.RichTextBox.Selection.Start;

            // Disable pictures inside lists and hyperlinks
            if (Helper.HasAncestor(insertionPosition, typeof(List)) || Helper.HasAncestor(insertionPosition, typeof(Hyperlink)))
            {
                return false;
            }

            return true;
        }

        private static void editFraction(RichTextEditor editor)
        {
            RichTextBox richTextBox = editor.RichTextBox;
            TextPointer insertionPosition = richTextBox.Selection.End;

            Image image = Helper.GetImageAncestor(insertionPosition);
            FractionDialog dlg = new FractionDialog();
            if (image != null)
            {
                dlg.FractionPart = editor.GetQuestionContentPart(image.Tag as string) as QuestionFractionPart;
            }

            if (dlg.ShowDialog().Value)
            {
                PictureCommands.InsertMathPicture(editor, dlg.FractionImage, dlg.FractionPart.PlaceHolder);
                if (image != null)
                {
                    foreach (var temp in editor.Parts)
                    {
                        if (temp.PlaceHolder == (image.Tag as string))
                        {
                            editor.Parts.Remove(temp);
                            break;
                        }
                    }
                }

                editor.Parts.Add(dlg.FractionPart);
            }
        }

        private static bool canEditFraction(RichTextEditor editor)
        {
            TextPointer insertionPosition = editor.RichTextBox.Selection.Start;

            // Disable pictures inside lists and hyperlinks
            if (Helper.HasAncestor(insertionPosition, typeof(List)) || Helper.HasAncestor(insertionPosition, typeof(Hyperlink)))
            {
                return false;
            }

            return true;
        }

        private static void superscript(RichTextEditor editor)
        {
            if (editor == null)
                return;

            RichTextBox richTextBox = editor.RichTextBox;
            if (richTextBox == null || richTextBox.Selection == null)
                return;

            var currentAlignment = richTextBox.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
            if (currentAlignment == DependencyProperty.UnsetValue)
                return;

            BaselineAlignment newAlignment = ((BaselineAlignment)currentAlignment == BaselineAlignment.Superscript) ? BaselineAlignment.Baseline : BaselineAlignment.Superscript;
            richTextBox.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
        }

        private static bool canSuperscript(RichTextEditor editor)
        {
            if (editor == null)
                return false;

            RichTextBox richTextBox = editor.RichTextBox;
            if (richTextBox == null || richTextBox.Selection == null)
                return false;

            return true;
        }

        private static void subscript(RichTextEditor editor)
        {
            if (editor == null)
                return;

            RichTextBox richTextBox = editor.RichTextBox;
            if (richTextBox == null || richTextBox.Selection == null)
                return;

            var currentAlignment = richTextBox.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
            if (currentAlignment == DependencyProperty.UnsetValue)
                return;

            BaselineAlignment newAlignment = ((BaselineAlignment)currentAlignment == BaselineAlignment.Subscript) ? BaselineAlignment.Baseline : BaselineAlignment.Subscript;
            richTextBox.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
        }

        private static bool canSubscript(RichTextEditor editor)
        {
            if (editor == null)
                return false;

            RichTextBox richTextBox = editor.RichTextBox;
            if (richTextBox == null || richTextBox.Selection == null)
                return false;

            return true;
        }

        private static void insertBlank(RichTextEditor editor)
        {
            if (editor == null)
                return;

            RichTextBox richTextBox = editor.RichTextBox;
            if (richTextBox == null)
                return;

            TextPointer insertionPosition = richTextBox.Selection.Start;
            if (insertionPosition == null)
                return;

            QuestionBlank blank = new QuestionBlank();
            var query = from temp in editor.Parts
                        where temp is QuestionBlank
                        select temp;

            TextBox blankTb = new TextBox();
            blankTb.Text = string.Format("空{0}", query.Count());
            blankTb.IsReadOnly = true;
            blankTb.Tag = blank.PlaceHolder;
            blankTb.TextAlignment = TextAlignment.Center;
            blankTb.Width = 100;
            insertionPosition.Paragraph.Inlines.Add(blankTb);
            insertionPosition.Paragraph.Inlines.LastInline.BaselineAlignment = BaselineAlignment.Center;

            editor.Parts.Add(blank);
        }

        private static bool canInsertBlank(RichTextEditor editor)
        {
            return true;
        }
    }
}
