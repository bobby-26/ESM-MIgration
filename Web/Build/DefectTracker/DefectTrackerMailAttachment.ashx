<%@ WebHandler Language="C#" Class="DefectTrackerMailAttachment" %>

using System;
using System.Web;
using System.IO;

public class DefectTrackerMailAttachment : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        string path = context.Request.QueryString["path"].ToString();
        
        byte[] buffer = new byte[1 << 16]; // 64kb 
        int bytesRead = 0;

        string filename = path.Substring(path.LastIndexOf('\\') + 1);
        if (File.Exists(path))
        {
            context.Response.AppendHeader("content-disposition", "filename=" + filename);
            context.Response.AppendHeader("content-type", ResolveAttachmentType(filename));
            
            using (var file = File.Open(path, FileMode.Open))
            {
                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) != 0)
                {
                    context.Response.OutputStream.Write(buffer, 0, bytesRead);
                }
            }
            context.Response.Flush();
            context.Response.Close();
            context.Response.End();
        }
    }


    private string ResolveAttachmentType(string filename)
    {
        if (filename.EndsWith(".xls"))
            return "application/vnd.ms-excel";
        if (filename.EndsWith(".xlsx"))
            return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        if (filename.EndsWith(".docx"))
            return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        if (filename.EndsWith(".pptx"))
            return "application/vnd.openxmlformats-officedocument.presentationml.presentation";       
        if (filename.EndsWith(".jpg"))
            return "image/jpg";
        if (filename.EndsWith(".zip"))
            return "application/zip";
        
        return "text/plain";
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}