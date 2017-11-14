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

using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Qooxdoo.WebDriver;
using Qooxdoo.WebDriver.UI;
using Wisej.Web.Ext.Selenium.UI;
using By = Qooxdoo.WebDriver.By;

namespace Wisej.Web.Ext.Selenium
{
    /// <summary>
    /// Represents a selenium Web Driver to test Wisej UI components.
    /// </summary>
    public class WisejWebDriver : QxWebDriver
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WisejWebDriver"/> class.
        /// </summary>
        /// <param name="browser">The <see cref="Browser"/> of the webdriver to wrap.</param>
        public WisejWebDriver(Browser browser) : base(browser, new WisejWidgetFactory())
        {
            SetWidgetFactoryDriver();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WisejWebDriver" /> class.
        /// </summary>
        /// <param name="browser">The <see cref="Browser"/> of the webdriver to wrap.</param>
        /// <param name="implicitWaitSeconds">The implicit wait duration in seconds.</param>
        public WisejWebDriver(Browser browser, int implicitWaitSeconds) : base(browser, new WisejWidgetFactory(),
            implicitWaitSeconds)
        {
            SetWidgetFactoryDriver();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WisejWebDriver"/> class.
        /// </summary>
        /// <param name="webdriver">The webdriver to wrap.</param>
        public WisejWebDriver(IWebDriver webdriver) : base(webdriver, new WisejWidgetFactory())
        {
            SetWidgetFactoryDriver();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WisejWebDriver" /> class.
        /// </summary>
        /// <param name="webdriver">The webdriver to wrap.</param>
        /// <param name="implicitWaitSeconds">The implicit wait duration in seconds.</param>
        public WisejWebDriver(IWebDriver webdriver, int implicitWaitSeconds) : base(webdriver, new WisejWidgetFactory(),
            implicitWaitSeconds)
        {
            SetWidgetFactoryDriver();
        }

        private void SetWidgetFactoryDriver()
        {
            ((DefaultWidgetFactory) WidgetFactory).Driver = this;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Sleep for the specified number of milliseconds.
        /// </summary>
        /// <param name="milliseconds"></param>
        public void Sleep(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// Removes the <see cref="IWidget"/> from the cache.
        /// </summary>
        /// <param name="path">The path string.</param>
        public void Clear(string path)
        {
            _cache.Remove("$/" + path);
        }

        /// <summary>
        /// Removes the child <see cref="IWidget"/> from the cache.
        /// </summary>
        /// <param name="parent">The parent widget.</param>
        /// <param name="path">The path string.</param>
        public void ClearChildWidget(IWidget parent, string path)
        {
            _cache.Remove(parent.QxHash + "/" + path);
        }

        /// <summary>
        /// Gets a newly fecthed <see cref="IWidget"/> from the browser.
        /// </summary>
        /// <param name="path">The path string.</param>
        /// <param name="timeoutInSeconds">The time to wait for the widget (seconds).</param>
        /// <returns>The <see cref="IWidget"/> that matches the <para>path</para>.</returns>
        public IWidget Refresh(string path, long timeoutInSeconds = 5)
        {
            Clear(path);
            return FindWidget(path, timeoutInSeconds);
        }

        /// <summary>
        /// Gets a newly fecthed child <see cref="IWidget"/> from the browser.
        /// </summary>
        /// <param name="parent">The parent widget.</param>
        /// <param name="path">The path string.</param>
        /// <param name="timeoutInSeconds">The time to wait for the widget (seconds).</param>
        /// <returns>The child <see cref="IWidget"/> that matches the <para>path</para>.</returns>
        public IWidget RefreshChildWidget(IWidget parent, string path, long timeoutInSeconds = 5)
        {
            ClearChildWidget(parent, path);
            return FindChildWidget(parent, path, timeoutInSeconds);
        }

        /// <summary>
        /// Returns the widget identified by the specified <para>path</para>.
        /// </summary>
        /// <param name="path">The path string.</param>
        /// <param name="timeoutInSeconds">The time to wait for the widget (seconds).</param>
        /// <returns>The <see cref="IWidget"/> that matches the <para>path</para>.</returns>
        public IWidget FindWidget(string path, long timeoutInSeconds = 5)
        {
            var key = "$/" + path;
            return Cache(key, () =>
            {
                var widgetBy = By.Qxh(By.Namespace(path));
                var widget = FindWidget(widgetBy, timeoutInSeconds);
                return widget;
            });
        }

        /// <summary>
        /// Returns the child widget identified by the specified <para>path</para>.
        /// </summary>
        /// <param name="parent">The parent widget.</param>
        /// <param name="path">The path string.</param>
        /// <param name="timeoutInSeconds">The time to wait for the widget (seconds).</param>
        /// <returns>The child <see cref="IWidget"/> that matches the <para>path</para>.</returns>
        public IWidget FindChildWidget(IWidget parent, string path, long timeoutInSeconds = 5)
        {
            var key = parent.QxHash + "/" + path;
            return Cache(key, () =>
            {
                var widgetBy = By.Qxh(By.Namespace(path));
                var widget = parent.WaitForWidget(widgetBy, timeoutInSeconds);
                return widget;
            });
        }

        #endregion

        #region Cache

        private Dictionary<string, IWidget> _cache = new Dictionary<string, IWidget>();

        // Finds the widget in the cache using the path.
        private IWidget Cache(string key, Func<IWidget> callback)
        {
            IWidget widget = null;
            if (_cache.TryGetValue(key, out widget))
            {
                // refresh if disposed.
                if (widget.IsDisposed)
                    _cache.Remove(key);
                else
                    return widget;
            }

            widget = callback();
            _cache[key] = widget;
            return widget;
        }

        #endregion
    }
}