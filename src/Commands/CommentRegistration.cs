using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace TextmateBundleInstaller
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("code++")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class BaseCommentRegistration : IWpfTextViewCreationListener
    {
        [Import]
        IVsEditorAdaptersFactoryService EditorAdapterService { get; set; }

        [Import]
        ITextDocumentFactoryService DocumentService { get; set; }

        public void TextViewCreated(IWpfTextView textView)
        {
            var contentType = textView.TextBuffer?.ContentType?.DisplayName?.ToLowerInvariant();
            string symbol = GetSymbol(contentType);

            if (!string.IsNullOrEmpty(symbol))
            {
                var viewAdapter = EditorAdapterService.GetViewAdapter(textView);
                textView.Properties.GetOrCreateSingletonProperty(() => new CommentCommandTarget(viewAdapter, textView, symbol));
            }
        }

        private static string GetSymbol(string contentType)
        {
            switch (contentType)
            {
                case "code++.batch file":
                    return "::";

                case "code++.c#":
                case "code++.f#":
                case "code++.go":
                case "code++.groovy":
                case "code++.jade":
                case "code++.java":
                case "code++.makefile":
                case "code++.objective c":
                case "code++.rust":
                case "code++.scala":
                case "code++.stylus":
                case "code++.swift":
                case "code++.typescript":
                    return "//";

                case "code++.clojure":
                case "code++.lisp":
                case "code++.scheme":
                    return ";;";

                case "code++.apache":
                case "code++.cmake cache":
                case "code++.cmake listfile":
                case "code++.ini":
                case "code++.perl 6":
                case "code++.perl":
                case "code++.powershell":
                case "code++.qmake project file":
                case "code++.r":
                case "code++.ruby":
                case "code++.shell script (bash)":
                case "code++.toml":
                case "code++.yaml":
                    return "#";

                case "code++.lua":
                case "code++.sql":
                    return "--";

                case "code++.matlab":
                    return "%";
            }

            return null;
        }
    }
}
