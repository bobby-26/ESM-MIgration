using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DataTransfer;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class DataTransferFileUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD");
        toolbar.AddButton("Import", "FORCEIMPORT");
        MenuUtilities.MenuList = toolbar.Show();
    }

    protected void MenuUtilities_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("UPLOAD") || dce.CommandName.ToUpper().Equals("FORCEIMPORT"))
            {
                if (!IsValidInput(dce.CommandName.ToUpper()))
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

                    filUpload.SaveAs(path);
                    lblFileUploadStatus.Text = "File name: " +
                         filUpload.PostedFile.FileName + "<br>" +
                         filUpload.PostedFile.ContentLength + " kb<br>" +
                         "Content type: " +
                         filUpload.PostedFile.ContentType;
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                if(dce.CommandName.ToUpper().Equals("FORCEIMPORT") && txtForceImportPassword.Text.Equals("u4sit@1s"))
                {
                    try
                    {
                        string[] filenameparts = filUpload.FileName.Substring(0, filUpload.FileName.IndexOf(".zip")).Split('_');
                        string vesselid = filenameparts[3];
                        PhoenixDataTransferImport.DataImportAdhocJob(short.Parse(vesselid), null, filUpload.FileName);
                        PhoenixDataTransferImport.AttachmentImportAdhocJob(short.Parse(vesselid), null, filUpload.FileName);
                    }
                    catch (Exception ex)
                    {
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                    }
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
            ucError.ErrorMessage = "You have not specified the file to upload";

        if (filUpload.HasFile)
            if (!filUpload.PostedFile.ContentType.Equals("application/x-zip-compressed"))
                ucError.ErrorMessage = "The file you have specified is not a zip file.";

        string[] filenameparts; 
        if (filUpload.HasFile)
            if (filUpload.PostedFile.ContentType.Equals("application/x-zip-compressed"))
            {
                filenameparts = filUpload.FileName.Split('_');
                if(filenameparts.Length != 4)
                    ucError.ErrorMessage = "The zip file you have specified is not a valid zip file.";
            }
        

        if (commandname.Equals("FORCEIMPORT") && !txtForceImportPassword.Text.Equals("u4sit@1s"))
            ucError.ErrorMessage = "You have not specified the password for force import OR the password is not correct";

        
        return !ucError.IsError;
    }

}
