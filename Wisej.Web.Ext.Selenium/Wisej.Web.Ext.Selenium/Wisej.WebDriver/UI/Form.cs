﻿///////////////////////////////////////////////////////////////////////////////
//
// (C) 2017 ICE TEA GROUP LLC - ALL RIGHTS RESERVED
//
//
//
// ALL INFORMATION CONTAINED HEREIN IS, AND REMAINS
// THE PROPERTY OF ICE TEA GROUP LLC AND ITS SUPPLIERS, IF ANY.
// THE INTELLECTUAL PROPERTY AND TECHNICAL CONCEPTS CONTAINED
// HEREIN ARE PROPRIETARY TO ICE TEA GROUP LLC AND ITS SUPPLIERS
// AND MAY BE COVERED BY U.S. AND FOREIGN PATENTS, PATENT IN PROCESS, AND
// ARE PROTECTED BY TRADE SECRET OR COPYRIGHT LAW.
//
// DISSEMINATION OF THIS INFORMATION OR REPRODUCTION OF THIS MATERIAL
// IS STRICTLY FORBIDDEN UNLESS PRIOR WRITTEN PERMISSION IS OBTAINED
// FROM ICE TEA GROUP LLC.
//
///////////////////////////////////////////////////////////////////////////////

using OpenQA.Selenium;
using QX = Qooxdoo.WebDriver;

namespace Wisej.Web.Ext.Selenium.UI
{
    /// <summary>
    /// Represents a <see cref="T:Wisej.Web.Form"/> widget.
    /// </summary>
    public class Form : QX.UI.Core.WidgetImpl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Form"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="webDriver">The web driver.</param>
        public Form(IWebElement element, QX.QxWebDriver webDriver) : base(element, webDriver)
        {
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        public void Close()
        {
            Call("close");
            Driver.Wait(() => IsDisposed);
        }

        /// <summary>
        /// Maximizes the form.
        /// </summary>
        public void Maximize()
        {
            Call("maximize");
            Driver.Wait(() => Displayed);
        }

        /// <summary>
        /// Minimizes the form.
        /// </summary>
        public void Minimize()
        {
            Call("minimize");
            Driver.Wait(() => !Displayed);
        }

        /// <summary>
        /// Restores the form.
        /// </summary>
        public void Restore()
        {
            Call("restore");
            Driver.Wait(() => Displayed);
        }
    }
}