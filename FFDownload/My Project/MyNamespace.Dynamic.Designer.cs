using System;
using System.ComponentModel;
using System.Diagnostics;

namespace HtmlGrabber.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Debug m_Debug;

            public Debug Debug
            {
                [DebuggerHidden]
                get
                {
                    m_Debug = Create__Instance__(m_Debug);
                    return m_Debug;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Debug))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Debug);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public frmCategory m_frmCategory;

            public frmCategory frmCategory
            {
                [DebuggerHidden]
                get
                {
                    m_frmCategory = Create__Instance__(m_frmCategory);
                    return m_frmCategory;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_frmCategory))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_frmCategory);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public frmPath m_frmPath;

            public frmPath frmPath
            {
                [DebuggerHidden]
                get
                {
                    m_frmPath = Create__Instance__(m_frmPath);
                    return m_frmPath;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_frmPath))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_frmPath);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public Html m_Html;

            public Html Html
            {
                [DebuggerHidden]
                get
                {
                    m_Html = Create__Instance__(m_Html);
                    return m_Html;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Html))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Html);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public HtmlGrabber m_HtmlGrabber;

            public HtmlGrabber HtmlGrabber
            {
                [DebuggerHidden]
                get
                {
                    m_HtmlGrabber = Create__Instance__(m_HtmlGrabber);
                    return m_HtmlGrabber;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_HtmlGrabber))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_HtmlGrabber);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public Story m_Story;

            public Story Story
            {
                [DebuggerHidden]
                get
                {
                    m_Story = Create__Instance__(m_Story);
                    return m_Story;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Story))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Story);
                }
            }

        }


    }
}