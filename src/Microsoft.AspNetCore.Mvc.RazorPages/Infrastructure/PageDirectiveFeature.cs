// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Evolution;

namespace Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure
{
    public static class PageDirectiveFeature
    {
        public static bool TryGetRouteTemplate(RazorProjectItem projectItem, out string template)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException(nameof(projectItem));
            }

            const string PageDirective = "@page";

            var stream = projectItem.Read();

            if (stream == null)
            {
                throw new ArgumentOutOfRangeException($"{nameof(projectItem)}.{nameof(projectItem.Read)} can't return null.");
            }

            string content;
            using (var streamReader = new StreamReader(stream))
            {
                content = streamReader.ReadToEnd().TrimStart();
            }

            if (content.StartsWith(PageDirective, StringComparison.Ordinal))
            {
                var endOfDirective = content.IndexOf(Environment.NewLine, PageDirective.Length);
                if (endOfDirective < 0)
                {
                    var otherNewLine = Environment.NewLine == "/r/n" ? "/n" : "/r/n";
                    endOfDirective = content.IndexOf(otherNewLine, PageDirective.Length);
                }

                // No Newlines, read to end of content
                if (endOfDirective < 0)
                {
                    endOfDirective = content.Length;
                }

                template = content.Substring(PageDirective.Length, endOfDirective - PageDirective.Length).Trim().Trim('"');

                // No path on this RazorPage
                if (string.IsNullOrEmpty(template))
                {
                    template = null;
                    return false;
                }

                return true;
            }

            template = null;
            return false;
        }
    }
}
