using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GUI
{
    public class TokenizingControl : FlowDocumentScrollViewer
    {
        public static readonly DependencyProperty TokenTemplateProperty =
            DependencyProperty.Register("TokenTemplate", typeof(DataTemplate), typeof(TokenizingControl));

        public DataTemplate TokenTemplate
        {
            get { return (DataTemplate)GetValue(TokenTemplateProperty); }
            set { SetValue(TokenTemplateProperty, value); }
        }


        public List<string> getAllDataPresented()
        {
            List<string> data = new List<string>();
            var para = Document.Blocks.FirstBlock as Paragraph;
            if (para == null)
            {
                para = new Paragraph();
                Document.Blocks.Add(para);
            }
            foreach (InlineUIContainer inline in para.Inlines)
            {
                ContentPresenter child = inline.Child as ContentPresenter;
                if (child == null) continue;
                if (child.Content == null) continue;
                data.Add(child.Content.ToString());
            }
            return data;
        }

        public bool deleteToken(InlineUIContainer inline)
        {
            var para = Document.Blocks.FirstBlock as Paragraph;
            if (para == null)
            {
                para = new Paragraph();
                Document.Blocks.Add(para);
            }
            if (para.Inlines.Contains(inline))
            {
                para.Inlines.Remove(inline);
                return true;
            }
            return false;
        }
        public TokenizingControl()
        {

        }

        public void AddToken(string text)
        {
            ReplaceTextWithToken(text);

        }

        private void ReplaceTextWithToken(string inputText)
        {
            var para = Document.Blocks.FirstBlock as Paragraph;
            if (para == null)
            {
                para = new Paragraph();
                Document.Blocks.Add(para);
            }    
            var tokenContainer = CreateTokenContainer(inputText);
            para.Inlines.Add(tokenContainer);
        }

        private InlineUIContainer CreateTokenContainer(string inputText)
        {
            // Note: we are not using the inputText here, but could be used in future

            var presenter = new ContentPresenter()
            {
                Content = inputText,
                ContentTemplate = TokenTemplate,
            };

            // BaselineAlignment is needed to align with Run
            return new InlineUIContainer(presenter) { BaselineAlignment = BaselineAlignment.TextBottom };
        }


    }
}