using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;

public partial class Accounts_Download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        try
        {
            //string dtkey = Request["dtkey"];
            string strfilename = Request["filename"];
            string strfilepath = Request["filepath"];
            //DataTable dt = PhoenixCommonFileAttachment.EditAttachment(new Guid(dtkey));
            //if (dt.Rows.Count > 0)
            //{
            //    string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
            //    if (!path.EndsWith("/"))
            //        path = path + "/";
            //    fileDownload(dt.Rows[0]["FLDFILENAME"].ToString(), path + dt.Rows[0]["FLDFILEPATH"].ToString());
            //}
            fileDownload(strfilename, strfilepath);
        }
        catch(Exception ex)
        {
            Response.Write("Downloading Error! " +  ex.Message);
            Page.Response.End();
        }
    }
    private void fileDownload(string fileName, string fileUrl)
    {
        Page.Response.Clear();
        bool success = ResponseFile(Page.Request, Page.Response, fileName, fileUrl, 1024000);
        if (!success)
            Response.Write("Downloading Error!");
        Page.Response.End();

    }
    public bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
    {
        try
        {
            FileInfo fi = new FileInfo(_fullPath);
            FileStream file = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(file);
            try
            {
                _Response.AddHeader("Accept-Ranges", "bytes");
                _Response.Buffer = false;
                long fileLength = file.Length;
                long startBytes = 0;

                int pack = 10240; //10K bytes
                int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;
                if (_Request.Headers["Range"] != null)
                {
                    _Response.StatusCode = 206;
                    string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                    startBytes = Convert.ToInt64(range[1]);
                }
                _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                if (startBytes != 0)
                {
                    _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                }
                _Response.AddHeader("Connection", "Keep-Alive");
                _Response.ContentType = ResolveMimeType(fi.Extension);
                _Response.AddHeader("Content-Disposition", "inline;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                for (int i = 0; i < maxCount; i++)
                {
                    if (_Response.IsClientConnected)
                    {
                        _Response.BinaryWrite(br.ReadBytes(pack));
                        Thread.Sleep(sleep);
                    }
                    else
                    {
                        i = maxCount;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                br.Close();
                file.Close();
            }
        }
        catch
        {
            return false;
        }
        return true;
    }
    private string ResolveMimeType(string extn)
    {
        string mimetype = "";
        switch (extn.ToLower())
        {
            case ".htm":
            case ".html":
                mimetype = "text/html";
                break;
            case ".txt":
                mimetype = "text/plain";
                break;
            case ".csv":
                mimetype = "application/x-msexcel";
                break;
            case ".xlsx":
            case ".xls":
                mimetype = "application/vnd.ms-excel|application/x-msexcel|application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case ".docx":
            case ".doc":
                mimetype = "application/msword|application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;
            case ".jpeg":
            case ".jpg":
                mimetype = "image/jpeg|image/pjpeg";
                break;
            case ".gif":
                mimetype = "image/gif";
                break;
            case ".png":
                mimetype = "image/png|image/x-png";
                break;
            case ".bmp":
                mimetype = "image/bmp";
                break;
            case ".pdf":
                mimetype = "application/pdf";
                break;
            case ".msg":
                mimetype = "application/vnd.ms-outlook";
                break;
            case ".pot":
            case ".pps":
            case ".ppt":
                mimetype = "application/vnd.ms-powerpoint";
                break;
            default:
                mimetype = "application/octet-stream";
                break;
        }
        return mimetype;
    }
}