using AtlasReaper.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtlasReaper.Jira
{
    internal class DownloadBOFNET
    {
        internal void DownloadAttachmentsThroughBOFNET(JiraOptions.DownloadBOFNETOptions options)
        {
            try
            {
                List<string> attachmentIds = options.Attachments.Split(',').ToList();
                foreach (string attachmentId in attachmentIds)
                {
                    Utils.WebRequestHandler webRequestHandler = new Utils.WebRequestHandler();
                    string url = options.Url + "/rest/api/3/attachment/" + attachmentId;
                    Attachment attachment = webRequestHandler.GetJson<Attachment>(url, options.Cookie);
                    string downloadUrl = attachment.Content;
                    string fileName = attachment.FileName;
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
