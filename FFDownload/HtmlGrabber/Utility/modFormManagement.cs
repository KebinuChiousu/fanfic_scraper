using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlGrabber
{
    static class FormManagement
    {

        public static Debug frmDebug;
        public static HtmlGrabber frmMain;
        public static Html frmHtml;
        public static Story frmStory;

        public enum forms
        {
            frmMain = 0,
            frmDebug = 1,
            frmHtml = 2,
            frmStory = 3
        }

        public static void Initialize(forms cls)

        {

            Form? frm = null;

            switch (cls)
            {
                case forms.frmDebug:
                    {
                        frm = frmDebug;
                        break;
                    }
                case forms.frmHtml:
                    {
                        frm = frmHtml;
                        break;
                    }
                case forms.frmStory:
                    {
                        frm = frmStory;
                        break;
                    }

            }

            if (frm != null)
            {
                if (Conversions.ToBoolean(frm.IsDisposed))
                {
                    CreateForm(cls);
                }
            }
            else
            {
                CreateForm(cls);
            }

        }

        public static void CreateForm(forms cls)

        {

            switch (cls)
            {

                case forms.frmMain:
                    {
                        break;
                    }
                // #################################
                // # Instance of Class HtmlGrabber #
                // # Default instance created by   #
                // # .Net Framework because        #
                // # screen is loaded at start up  #
                // #################################
                case forms.frmDebug:
                    {
                        // Create an Instance of class frmDebug
                        frmDebug = new Debug();
                        break;
                    }
                case forms.frmHtml:
                    {
                        // Create an Instance of class frmHtml
                        frmHtml = new Html();
                        break;
                    }
                case forms.frmStory:
                    {
                        frmStory = new Story();
                        PlaceStoryWindow();
                        break;
                    }
            }

        }

        public static void PlaceDebugWindow()
        {
            frmDebug.Top = frmMain.Top + 168 + 35;
            frmDebug.Left = frmMain.Left + 28;
        }

        public static void PlaceHtmlWindow()
        {
            frmHtml.Top = frmMain.Top + 168 + 35;
            frmHtml.Left = frmMain.Left + 28;
        }

        public static void PlaceStoryWindow()
        {

            frmStory.Top = frmDebug.Top - frmDebug.grdRSS.Height;
            frmStory.Left = frmDebug.Left;

        }



    }
}