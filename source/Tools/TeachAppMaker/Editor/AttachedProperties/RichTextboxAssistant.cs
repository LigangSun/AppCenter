using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.IO;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Data;
using HTMLConverter;

namespace SoonLearning.TeachAppMaker.AttachedProperties
{
    public static class RichTextboxAssistant
    {
        public static readonly DependencyProperty BoundDocument =
           DependencyProperty.RegisterAttached("BoundDocument", typeof(string), typeof(RichTextboxAssistant),
           new FrameworkPropertyMetadata(null,
               FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
               OnBoundDocumentChanged)
               );

        private static void OnBoundDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RichTextBox box = d as RichTextBox;

            if (box == null)
                return;

            RemoveEventHandler(box);

            string newXAML = GetBoundDocument(d);
            if (!string.IsNullOrEmpty(newXAML))
                box.Document = HtmlToXamlConverter.DeserializeFlowDocument(newXAML);

            //box.Document.Blocks.Clear();

            //if (!string.IsNullOrEmpty(newXAML))
            //{
            //    using (MemoryStream xamlMemoryStream = new MemoryStream(Encoding.ASCII.GetBytes(newXAML)))
            //    {
            //        ParserContext parser = new ParserContext();
            //        parser.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            //        parser.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            //        FlowDocument doc = new FlowDocument();
            //        Section section = XamlReader.Load(xamlMemoryStream, parser) as Section;

            //        box.Document.Blocks.Add(section);

            //    }
            //}

            AttachEventHandler(box);

        }

        private static void RemoveEventHandler(RichTextBox box)
        {
            Binding binding = BindingOperations.GetBinding(box, BoundDocument);

            if (binding != null)
            {
                if (binding.UpdateSourceTrigger == UpdateSourceTrigger.Default ||
                    binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus)
                {

                    box.LostFocus -= HandleLostFocus;
                }
                else
                {
                    box.TextChanged -= HandleTextChanged;
                }
            }
        }

        private static void AttachEventHandler(RichTextBox box)
        {
            Binding binding = BindingOperations.GetBinding(box, BoundDocument);

            if (binding != null)
            {
                if (binding.UpdateSourceTrigger == UpdateSourceTrigger.Default ||
                    binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus)
                {

                    box.LostFocus += HandleLostFocus;
                }
                else
                {
                    box.TextChanged += HandleTextChanged;
                }
            }
        }

        private static void HandleLostFocus(object sender, RoutedEventArgs e)
        {
            RichTextBox box = sender as RichTextBox;
            if (box == null)
                return;

            SetBoundDocument(box, box.Document);
            return;

            TextRange tr = new TextRange(box.Document.ContentStart, box.Document.ContentEnd);

            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Xaml);
                string xamlText = ASCIIEncoding.Default.GetString(ms.ToArray());
             //   SetBoundDocument(box, xamlText);
            }
        }

        private static void HandleTextChanged(object sender, RoutedEventArgs e)
        {
            // TODO: TextChanged is currently not working!
            RichTextBox box = sender as RichTextBox;
            if (box == null)
                return;

            SetBoundDocument(box, box.Document);
            return;

            TextRange tr = new TextRange(box.Document.ContentStart,
                               box.Document.ContentEnd);

            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Xaml);
                string xamlText = ASCIIEncoding.Default.GetString(ms.ToArray());
             //   SetBoundDocument(box, xamlText);
            }
        }

        public static string GetBoundDocument(DependencyObject dp)
        {
#if _SAVE_CONTENT_AS_XAML_
            return dp.GetValue(BoundDocument) as string;
#else
            var html = dp.GetValue(BoundDocument) as string;
            var xaml = string.Empty;

            if (!string.IsNullOrEmpty(html))
                xaml = HtmlToXamlConverter.ConvertHtmlToXaml(html, true);

            return xaml;
#endif
        }

        public static void SetBoundDocument(DependencyObject dp, FlowDocument value)
        {
            // I don't want to convert xaml to html now. I want to user flowdocument format directly.
            // For other platform, we will convert flowdocument to html on player side.
            // Set Directly
#if _SAVE_CONTENT_AS_XAML_
            FlowDocument document = value as FlowDocument;
            dp.SetValue(BoundDocument, HtmlFromXamlConverter.SerializeFlowDocument(document));
#else
            // Convert then set.
            var xaml = value;
            var html = HtmlFromXamlConverter.ConvertXamlToHtml(value);
            dp.SetValue(BoundDocument, html);
#endif
        }
    }
}