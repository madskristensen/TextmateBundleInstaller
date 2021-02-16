using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace TextmateBundleInstaller
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("code++")]
    [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
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
                var onlyLineStart = IsOnlyLineStartSupported(contentType);
                textView.Properties.GetOrCreateSingletonProperty(() => new CommentCommandTarget(viewAdapter, textView, symbol, onlyLineStart));
            }
        }

        private static bool IsOnlyLineStartSupported(string contentType)
        {
            switch (contentType)
            {
                case "code++.apache":
                    return true;
            }

            return false;
        }

        private static string GetSymbol(string contentType)
        {
            switch (contentType.Replace("code++", string.Empty))
            {
                case ".batch file":
                    return "::";

                case ".c#":
                case ".f#":
                case ".go":
                case ".groovy":
                case ".jade":
                case ".java":
                case ".javascript":
                case ".json":
                case ".less":
                case ".mk":
                case ".makefile":
                case ".objective c":
                case ".rust":
                case ".scala":
                case ".stylus":
                case ".swift":
                case ".typescript":
                    return "//";

                case ".clojure":
                case ".lisp":
                case ".scheme":
                    return ";;";

                case ".apache":
                case ".cmake cache":
                case ".cmake listfile":
                case ".elixir":
                case ".eyaml":
                case ".ini":
                case ".perl 6":
                case ".perl":
                case ".powershell":
                case ".qmake project file":
                case ".r":
                case ".ruby":
                case ".shell script (bash)":
                case ".toml":
                case ".yaml":
                    return "#";

                case ".lua":
                case ".sql":
                    return "--";

                case ".matlab":
                case ".latex":
                    return "%";
            }

            return null;
        }
    }
}
