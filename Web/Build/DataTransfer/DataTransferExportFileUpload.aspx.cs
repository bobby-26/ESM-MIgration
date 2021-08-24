using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DataTransfer;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using Telerik.Web.UI;

public partial class DataTransferExportFileUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD", ToolBarDirection.Right);
        MenuUtilities.MenuList = toolbar.Show();
    }

    protected void MenuUtilities_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("UPLOAD"))
            {
                if (!IsValidInput(CommandName.ToUpper()))
                {
                    ucError.Visible = true;
                    return;
                }

                DataTable dt = PhoenixDataTransferCommon.Configuration();

                try
                {
                    string dtpath = dt.Rows[0]["FLDDTPATH"].ToString();
                    string path = dtpath.Substring(0, dtpath.IndexOf(@"\DT"));
                    path = path + @"\INBOX\" + filUpload.FileName;
                    string fileex = Path.GetFileNameWithoutExtension(path);

                    filUpload.SaveAs(path);
                    lblFilename.Visible = true;
                    lblFileUploadStatus.Text = filUpload.PostedFile.FileName;
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidInput(string commandname)
    {
        if (!filUpload.HasFile)
        {
            ucError.ErrorMessage = "You have not specified the file to upload";
        }

        if (filUpload.HasFile)
        {
            if (!filUpload.PostedFile.ContentType.Equals("application/x-zip-compressed"))
                ucError.ErrorMessage = "The file you have specified is not a zip file.";
        }

        string[] filenameparts;
        int seq, vslid;
        string filename;

        if (filUpload.HasFile)
        {
            if (filUpload.PostedFile.ContentType.Equals("application/x-zip-compressed"))
            {
                filename = Path.GetFileNameWithoutExtension(filUpload.FileName);
                filenameparts = filename.Split('_');

                bool isseq = int.TryParse(filenameparts[0], out seq);
                bool isvslid = int.TryParse(filenameparts[3], out vslid);

                if (!isseq || !isvslid)
                    ucError.ErrorMessage = "The zip file you have specified is not a valid zip file.";

                if (filenameparts.Length != 4)
                {
                    ucError.ErrorMessage = "The zip file you have specified is not a valid zip file.";
                }
            }
        }

        return !ucError.IsError;
    }

}
