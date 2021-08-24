using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Framework;
using System.Web;


public partial class PurchaseAttachmentUpload : PhoenixBasePage
{
	string formno = null;
	string vendorid = null;
	
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Upload", "UPLOAD");
        AttachmentList.AccessRights = this.ViewState;  
		AttachmentList.MenuList = toolbarmain.Show();

    }
	protected void AttachmentList_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToString().ToUpper() == "UPLOAD")
			{
				if (txtFileUpload.HasFile)
				{
					string filename = txtFileUpload.PostedFile.FileName.ToString();
					string substr = filename.Substring(filename.LastIndexOf('_') + 1);
					if (!ValidateFileExtension(filename))
					{
						ucError.Visible = true;

						return;
					}
					string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Purchase/RFQ/") + filename;
					txtFileUpload.PostedFile.SaveAs(strpath);
					int extensionIndex = substr.LastIndexOf(".");
					string uniqueid = substr.Substring(0, extensionIndex);
					
					PhoenixPurchaseQuotation.InsertImportRFQLookUpExcel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, uniqueid, formno, vendorid);
					ucStatus.Text = "Quotation information updated";
				}
				else
				{
					ucError.ErrorMessage = "File not found";
					ucError.Visible = true;
				}

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	public bool ValidateFileExtension(string filename)
	{
		string fileextension = filename.Substring(filename.LastIndexOf('.'));
		try
		{
			string[] splitfilename = filename.Split('_');
			string a = splitfilename[0];
			string b = splitfilename[1];
			string c = splitfilename[2];
			vendorid = splitfilename[3]; 
			formno = a + "/" + b + "/" + c;
		}
		catch (Exception)
		{
			ucError.ErrorMessage = "Invalid File Name";
		}
		if (fileextension != ".xls")
			ucError.ErrorMessage = "File format not in correct format.It should of '.xls' file type";

		return (!ucError.IsError);
		
	}


}
