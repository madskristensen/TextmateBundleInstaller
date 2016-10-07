using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace TextmateBundleInstaller
{
    internal class CommentCommandTarget : CommandTargetBase<VSConstants.VSStd2KCmdID>
    {
        private string _symbol;
        private bool _onlyLineStart;

        public CommentCommandTarget(IVsTextView adapter, IWpfTextView textView, string commentSymbol, bool onlyLineStart)
            : base(adapter, textView, VSConstants.VSStd2KCmdID.COMMENT_BLOCK, VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK)
        {
            _symbol = commentSymbol;
            _onlyLineStart = onlyLineStart;
        }

        protected override bool Execute(VSConstants.VSStd2KCmdID commandId, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            StringBuilder sb = new StringBuilder();
            SnapshotSpan span = GetSpan();

            var startLine = span.Start.GetContainingLine();
            var endLine = span.End.GetContainingLine();

            string[] lines;

            if (startLine.LineNumber != endLine.LineNumber)
            {
                var rawLines = span.Snapshot.Lines.Where(l => l.LineNumber >= startLine.LineNumber && l.LineNumber <= endLine.LineNumber);
                lines = rawLines.Select(l => l.GetText()).ToArray();
            }
            else
            {
                lines = span.GetText().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            }

            switch (commandId)
            {
                case VSConstants.VSStd2KCmdID.COMMENT_BLOCK:
                    Comment(sb, lines);
                    break;

                case VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK:
                    Uncomment(sb, lines);
                    break;
            }

            UpdateTextBuffer(span, sb.ToString().TrimEnd());

            return true;
        }

        private void Comment(StringBuilder sb, string[] lines)
        {
            foreach (string line in lines)
            {
                sb.AppendLine(_symbol + line);
            }
        }

        private void Uncomment(StringBuilder sb, string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.StartsWith(_symbol, StringComparison.Ordinal))
                {
                    sb.AppendLine(line.Substring(_symbol.Length));
                }
                else
                {
                    sb.AppendLine(line);
                }
            }
        }

        private void UpdateTextBuffer(SnapshotSpan span, string text)
        {
            try
            {
                TextView.TextBuffer.Replace(span.Span, text);

                var newSpan = new SnapshotSpan(TextView.TextBuffer.CurrentSnapshot, span.Start, text.Length);
                TextView.Selection.Select(newSpan, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }
        }

        private SnapshotSpan GetSpan()
        {
            var sel = TextView.Selection.StreamSelectionSpan;
            var startLine = new SnapshotPoint(TextView.TextSnapshot, sel.Start.Position).GetContainingLine();
            var endLine = new SnapshotPoint(TextView.TextSnapshot, sel.End.Position).GetContainingLine();

            if (_onlyLineStart || TextView.Selection.IsEmpty || startLine.LineNumber != endLine.LineNumber)
            {
                return new SnapshotSpan(startLine.Start, endLine.End);
            }
            else
            {
                return new SnapshotSpan(sel.Start.Position, sel.Length);
            }
        }

        protected override bool IsEnabled()
        {
            return true;
        }
    }
}