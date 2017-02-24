// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Evolution;

namespace Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure
{
    public static class PageDirectiveFeature
    {
        public static bool TryGetPageDirective(RazorProjectItem projectItem, out string template)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException(nameof(projectItem));
            }

            const string PageDirective = "@page";

            var stream = projectItem.Read();

            string content = null;
            using (var streamReader = new StreamReader(stream))
            {
                do
                {
                    content = streamReader.ReadLine();
                } while (content != null && string.IsNullOrWhiteSpace(content));
                content = content != null ? content.Trim() : content;
            }

            if (content != null && content.StartsWith(PageDirective, StringComparison.Ordinal))
            {
                template = content.Substring(PageDirective.Length, content.Length - PageDirective.Length).Trim();

                // If it's not in quotes it's not our template
                if (!template.StartsWith("\"") || !template.EndsWith("\""))
                {
                    template = string.Empty;
                }
                else
                {
                    template = template.Substring(1, template.Length - 2);
                }

                return true;
            }

            template = null;
            return false;
        }
    }
}
