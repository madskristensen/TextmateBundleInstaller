using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace TextmateBundleInstaller
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("code++.ini")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class CommentRegistration : IWpfTextViewCreationListener
    {
        [Import]
        IVsEditorAdaptersFactoryService EditorAdapterService { get; set; }

        [Import]
        ITextDocumentFactoryService DocumentService { get; set; }

        public void TextViewCreated(IWpfTextView textView)
        {
            var textViewAdapter = EditorAdapterService.GetViewAdapter(textView);

            textView.Properties.GetOrCreateSingletonProperty(() => new CommentCommandTarget(textViewAdapter, textView, "#"));
        }
    }
}
