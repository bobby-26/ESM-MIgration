using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class UserControls_UserControlAttachment : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
        }
    }
    protected void btnAttach_Click(object sender, EventArgs e)
    {
        DataTable tblattachment = new DataTable();
        tblattachment.Columns.Add("Text");
        tblattachment.Columns.Add("Value");
        DataRow rowattachment = null;
        Int64 attachmentsize = 0;

        string destinationpath = HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/" + Request.QueryString["mailsessionid"]);
        if (!System.IO.Directory.Exists(destinationpath))
        {
            System.IO.Directory.CreateDirectory(destinationpath);
        }

        DirectoryInfo dir = new DirectoryInfo(destinationpath);
        FileInfo[] files = dir.GetFiles();
        Array.Sort(files, (x, y) => x.CreationTime.CompareTo(y.CreationTime));

        for (int i = 0; i < files.Length; i++)
        {
            rowattachment = tblattachment.NewRow();
            rowattachment["Text"] = files[i].Name.ToString();
            rowattachment["Value"] = "EmailAttachments/" + Request.QueryString["mailsessionid"] + "/" + files[i].Name;
            tblattachment.Rows.Add(rowattachment);
            attachmentsize = attachmentsize + files[i].Length;
        }

        if (fileUpload1.HasFile)
        {
            if (fileUpload1.PostedFile.ContentLength + attachmentsize < PhoenixMail.MaxAttachmentSize)
            {
                //lstAttachments.Items.Add(new ListItem(fileUpload1.FileName.ToString(), fileUpload1.PostedFile.FileName.ToString()));
                if (!System.IO.File.Exists(destinationpath + "\\" + fileUpload1.FileName.ToString()))
                {
                    rowattachment = tblattachment.NewRow();
                    rowattachment["Text"] = fileUpload1.FileName.ToString();
                    rowattachment["Value"] = "EmailAttachments/" + Request.QueryString["mailsessionid"] + "/" + fileUpload1.PostedFile.FileName.ToString();
                    tblattachment.Rows.Add(rowattachment);
                }
                fileUpload1.SaveAs(destinationpath + "\\" + fileUpload1.FileName.ToString());
                //System.IO.File.Copy(fileUpload1.PostedFile.FileName.ToString(), , true);
                attachmentsize += fileUpload1.PostedFile.ContentLength;
            }
        }

        if (fileUpload2.HasFile)
        {
            if (fileUpload2.PostedFile.ContentLength + attachmentsize < PhoenixMail.MaxAttachmentSize)
            {
                if (!System.IO.File.Exists(destinationpath + "\\" + fileUpload2.FileName.ToString()))
                {
                    rowattachment = tblattachment.NewRow();
                    rowattachment["Text"] = fileUpload2.FileName.ToString();
                    rowattachment["Value"] = "EmailAttachments/" + Request.QueryString["mailsessionid"] + "/" + fileUpload2.PostedFile.FileName.ToString();
                    tblattachment.Rows.Add(rowattachment);
                }
                fileUpload2.SaveAs(destinationpath + "\\" + fileUpload2.FileName.ToString());
                //System.IO.File.Copy(fileUpload2.PostedFile.FileName.ToString(), destinationpath + "\\" + fileUpload2.FileName.ToString(), true);
                attachmentsize += fileUpload2.PostedFile.ContentLength;
            }
        }

        if (fileUpload3.HasFile)
        {
            if (fileUpload3.PostedFile.ContentLength + attachmentsize < PhoenixMail.MaxAttachmentSize)
            {
                if (!System.IO.File.Exists(destinationpath + "\\" + fileUpload3.FileName.ToString()))
                {
                    rowattachment = tblattachment.NewRow();
                    rowattachment["Text"] = fileUpload3.FileName.ToString();
                    rowattachment["Value"] = "EmailAttachments/" + Request.QueryString["mailsessionid"] + "/" + fileUpload3.PostedFile.FileName.ToString();
                    tblattachment.Rows.Add(rowattachment);
                }
                fileUpload3.SaveAs(destinationpath + "\\" + fileUpload3.FileName.ToString());
                //System.IO.File.Copy(fileUpload3.PostedFile.FileName.ToString(), destinationpath + "\\" + fileUpload3.FileName.ToString(), true);
                attachmentsize += fileUpload3.PostedFile.ContentLength;
            }
        }

        Session["AttachFiles"] = tblattachment;
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('MailAttachment','ifMoreInfo');";
        Script += "</script>" + "\n";

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }    
}
