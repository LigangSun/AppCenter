using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SoonLearning.TeachAppMaker.Commands;
using System.Windows.Threading;
using HTMLConverter;
using System.Globalization;
using SoonLearning.Assessment.Data;
using SoonLearning.TeachAppMaker.AttachedProperties;

namespace SoonLearning.TeachAppMaker.Controls
{
    /// <summary>
    /// Interaction logic for BindableRichTextbox.xaml
    /// </summary>
    public partial class RichTextEditor : UserControl
    {
        private const string htmlHeader = "<!DOCTYPE HTML PUBLIC";

        public static readonly DependencyProperty TextProperty =
                      DependencyProperty.Register("Text", typeof(string), typeof(RichTextEditor),
                      new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsReadOnlyProperty =
                      DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RichTextEditor),
                      new PropertyMetadata(false, IsReadOnlyChanged));

        private bool updateSelectionPropertiesPending = false;

        private object textChangedLocker = new object();

        public RichTextBox RichTextBox
        {
            get { return this.mainRTB; }
        }

        private QuestionPartCollection contentPartList = new QuestionPartCollection();

        internal QuestionPartCollection Parts
        {
            get 
            {
                if (this.Tag is QuestionContent)
                {
                    QuestionContent content = this.Tag as QuestionContent;
                    return content.QuestionPartCollection;
                }

                return this.contentPartList; 
            }
        }

        public string HTML
        {
            get { return HtmlFromXamlConverter.ConvertXamlToHtml(this.Document); }
            set
            {
                this.Document = HtmlToXamlConverter.ConvertHtmlToXaml(value);
            }
        }

        public string DocumentBody
        {
            get { return HtmlFromXamlConverter.SerializeFlowDocument(this.Document); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                if (value.StartsWith(htmlHeader, true, CultureInfo.CurrentCulture))
                {
                    //### DOcument type is HTML (backward compatibility)
                    this.HTML = value;
                    return;
                }

                this.Document = HtmlToXamlConverter.DeserializeFlowDocument(value);
            }
        }

        private FlowDocument Document
        {
            get { return this.RichTextBox.Document; }
            set
            {
                this.RichTextBox.Selection.Changed -= new EventHandler(OnRichTextBox_SelectionChanged);
                try
                {
                    this.RichTextBox.Document = value;

                    foreach (Block block in this.RichTextBox.Document.Blocks)
                    {
                        if (block is Table)
                        {
                            MiscCommands.InstallTableCallbacks(block as Table);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(SoonLearning.TeachAppMaker.Properties.Resources.txtLoadFailed + " :\n\r" + ex, string.Empty);
                    //Clipboard.SetData(DataFormats.Text, value);
                }
                finally
                {
                    this.RichTextBox.Selection.Changed += new EventHandler(OnRichTextBox_SelectionChanged);
                }
            }
        }
        
        public RichTextEditor()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { 
                SetValue(TextProperty, value);
            }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        private static void IsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RichTextEditor editor = d as RichTextEditor;
            editor.ReadOnlyChanged((bool)e.NewValue);
        }

        private void ReadOnlyChanged(bool value)
        {
            this.mainRTB.IsReadOnly = value;
        }

        private void uxRichTextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            this.Document.FontSize = 18f;
        }

        private void OnRichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            //if (!this.updateSelectionPropertiesPending)
            //{
            //    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
            //                                             new DispatcherOperationCallback(UpdateSelectionProperties),
            //                                             null);
            //    this.updateSelectionPropertiesPending = true;
            //}
        }

        internal QuestionContentPart GetQuestionContentPart(string holder)
        {
            var query = from temp in this.contentPartList
                        where temp.PlaceHolder == holder
                        select temp;

            if (query.Count() > 0)
                return query.First();

            return null;
        }

        private void mainRTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            lock (this.textChangedLocker)
            {
                string doc = HtmlFromXamlConverter.SerializeFlowDocument(this.mainRTB.Document); ;

                var query = from part in this.Parts
                            where !doc.Contains(part.PlaceHolder)
                            select part;

                List<QuestionContentPart> tempList = new List<QuestionContentPart>();

                foreach (var part in query)
                {
                    tempList.Add(part);
                }

                foreach (var part in tempList)
                {
                    this.Parts.Remove(part);
                }
            }
        }
    }
}
