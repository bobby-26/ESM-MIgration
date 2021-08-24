using System;
using System.Data;
using System.IO;
using System.Web;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Text;
using System.Collections.Generic;

public partial class CrewNewApplicantCorrespondenceEmail : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbar.AddButton("Discard", "DISCARD", ToolBarDirection.Right);
            toolbar.AddButton("Send", "SEND", ToolBarDirection.Right);
            
            MenuCorrespondenceEmail.AccessRights = this.ViewState;
			MenuCorrespondenceEmail.MenuList = toolbar.Show();
			if (!IsPostBack)
			{
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["USEREMAIL"] = "";

				string strEmployeeId = string.Empty;
				if (string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
					strEmployeeId = string.Empty;
				else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
					strEmployeeId = Request.QueryString["empid"];
				else if (!string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection))
					strEmployeeId = Filter.CurrentNewApplicantSelection;
                SetUserMailId();
                SetDefaultMessage();
				ViewState["EMPID"] = strEmployeeId;
				DataTable dt = PhoenixCrewAddress.ListEmployeeAddress(int.Parse(strEmployeeId));
				ViewState["EMPEMAIL"] = dt.Rows.Count > 0 ? dt.Rows[0]["FLDEMAIL"].ToString() : string.Empty;

				if (General.GetNullableInteger(ViewState["EMPID"].ToString()) == null)
				{
					ucError.ErrorMessage = "Select a Employee from Query Activity";
					ucError.Visible = true;
					return;
				}

				ViewState["CORRESSPONDENCEID"] = string.Empty;
				ViewState["mailsessionid"] = string.Empty;
				SetEmployeePrimaryDetails();
				txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);

				if (lstAttachments.Items.Count > 0)
				{
					string path = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
					if (Directory.Exists(path))
						Directory.Delete(path, true);
					lstAttachments.Items.Clear();
				}
				ViewState["mailsessionid"] = System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.Month.ToString() + "_" + System.DateTime.Now.Year.ToString() + "_" + System.DateTime.Now.Hour.ToString() + "_" + System.DateTime.Now.Minute.ToString() + "_" + System.DateTime.Now.Second.ToString() + "_" + System.DateTime.Now.Millisecond.ToString();
				lnkAttachment.OnClientClick = "javascript:openNewWindow('MailAttachment', 'Email Attachment', '" + Session["sitepath"] + "/Options/OptionsAttachment.aspx?mailsessionid=" + ViewState["mailsessionid"] + "', 'xdata');";
				lstAttachments.Attributes["onkeypress"] = "javascript:DeleteFiles(event,'" + ViewState["mailsessionid"] + "');";
				if (Request.QueryString["cid"] != "" && Request.QueryString["cid"] != null)
                    EditCorrespondenceEmail(Request.QueryString["cid"], Request.QueryString["empid"]);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void CorrespondenceEmail_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEND"))
			{
				if (!IsValidCorrespondenceEmail(txtSubject.Text, ddlCorrespondenceType.SelectedQuick))
				{
					ucError.Visible = true;
					return;
				}
				string dtkey = string.Empty;
				string path = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
				DataSet ds = PhoenixCrewCorrespondence.InsertCorrespondence(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
																, txtFrom.Text, txtTO.Text,txtCC.Text, txtSubject.Text, txtBody.Content,1);
				
				if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					dtkey = ds.Tables[0].Rows[0][0].ToString();
					if (Directory.Exists(path))
					{
						DirectoryInfo di = new DirectoryInfo(path);
						FileInfo[] fi = di.GetFiles();
						foreach (FileInfo f in fi)
						{

							string tempkey = PhoenixCommonFileAttachment.GenerateDTKey();
							PhoenixCommonFileAttachment.InsertAttachment(new Guid(dtkey), f.Name, "Crew/" + tempkey + f.Extension, f.Length
								, PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, PhoenixCrewAttachmentType.CORRESPONDENCE.ToString(), new Guid(tempkey));
							string desgpath = HttpContext.Current.Request.MapPath("~/");
							desgpath = desgpath + "Attachments/Crew/" + tempkey + f.Extension;
							f.CopyTo(desgpath, true);

						}
					}
				}
				PhoenixMail.SendMail(txtTO.Text, txtCC.Text, txtBCC.Text, txtSubject.Text, txtBody.Content
							   , true, System.Net.Mail.MailPriority.Normal, ViewState["mailsessionid"].ToString(), General.GetNullableInteger(ViewState["EMPID"].ToString()));


				ucStatus.Text = "Email sent";
			}
            else if(CommandName.ToUpper() == "BACK")
            {
                if(Request.QueryString["LAUNCH"] != null && Request.QueryString["LAUNCH"].ToString()=="OFFSHORE")
                    Response.Redirect("../CrewOffshore/CrewOffshoreNewApplicantList.aspx?p=1&launchedfrom=OFFSHORE", false);
                else
                    Response.Redirect("CrewNewApplicantCorrespondence.aspx", true);
            }
			else
			{
				ResetFields();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    private void EditCorrespondenceEmail(string Correspondenceid, string employeeid)
	{
        DataTable dt = PhoenixCrewCorrespondence.EditCorrespondence(General.GetNullableInteger(Correspondenceid).Value, General.GetNullableInteger(employeeid));

		if (dt.Rows.Count > 0)
		{
			ViewState["CORRESSPONDENCEID"] = dt.Rows[0]["FLDCORRESPONDENCEID"].ToString();
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Send", "SEND");
			toolbar.AddButton("Discard", "DISCARD");
            toolbar.AddButton("Back", "BACK");
            toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewCorrespondenceLockAndUnlock.aspx?id=" + dt.Rows[0]["FLDCORRESPONDENCEID"].ToString() + "&empid=" + ViewState["EMPID"].ToString() + "','medium'); return false;", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "Lock" : "UnLock", "", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "LOCK" : "UNLOCK");
			MenuCorrespondenceEmail.MenuList = toolbar.Show();
			txtFrom.Text = dt.Rows[0]["FLDFROM"].ToString();
			txtTO.Text = dt.Rows[0]["FLDTO"].ToString();
			txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
			txtBody.Content = dt.Rows[0]["FLDBODY"].ToString();
			txtDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDCREATEDDATE"].ToString()));
			ddlCorrespondenceType.SelectedQuick = dt.Rows[0]["FLDCORRESPONDENCETYPE"].ToString();
			ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
			DataTable dt1 = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["DTKEY"].ToString()));
			if (dt1.Rows.Count > 0)
			{
				string dirpath = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
				if (!Directory.Exists(dirpath))
					Directory.CreateDirectory(dirpath);
				string filepath = string.Empty;
				lstAttachments.Items.Clear();
				for (int i = 0; i < dt1.Rows.Count; i++)
				{
					filepath = Server.MapPath("~/Attachments/" + dt1.Rows[i]["FLDFILEPATH"].ToString()).ToString();
					if (File.Exists(filepath))
					{
						FileInfo fi = new FileInfo(filepath);
						fi.CopyTo(dirpath + "/" + dt1.Rows[i]["FLDFILENAME"].ToString(), true);
                        lstAttachments.Items.Add(new RadListBoxItem(dt1.Rows[i]["FLDFILENAME"].ToString()));                       
					}
				}
			}
		}
	}



	private void ResetFields()
	{
		ViewState["CORRESSPONDENCEID"] = string.Empty;
		txtFrom.Text = string.Empty;
        txtTO.Text = ViewState["EMPEMAIL"].ToString();
		txtCC.Text = string.Empty;
		txtBCC.Text = string.Empty;
		txtSubject.Text = string.Empty;
		txtBody.Content = string.Empty;
		ddlCorrespondenceType.SelectedQuick = string.Empty;
	}
	public bool IsValidCorrespondenceEmail(string subject, string type)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		int resultInt;

		if (string.IsNullOrEmpty(subject))
			ucError.ErrorMessage = "Subject is required.";

		if (!int.TryParse(type, out resultInt))
			ucError.ErrorMessage = "CorrespondenceEmail Type is required.";

		if (txtFrom.Text.Trim() != string.Empty && !General.IsvalidEmail(txtFrom.Text))
			ucError.ErrorMessage = "Please enter valid From E-Mail Address";

		if ((txtTO.Text.Trim() != string.Empty || txtTO.CssClass == "input_mandatory") && !General.IsvalidEmail(txtTO.Text))
			ucError.ErrorMessage = "Please enter valid To E-Mail Address";

		return (!ucError.IsError);
	}

	private void SetEmployeePrimaryDetails()
	{
		try
		{
			DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["EMPID"].ToString()));

			if (dt.Rows.Count > 0)
			{
				txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
				txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
				txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
				txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
				txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtTO.Text = ViewState["EMPEMAIL"].ToString();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		if (Session["AttachFiles"] != null)
		{
			lstAttachments.Items.Clear();
			DataTable dt = (DataTable)Session["AttachFiles"];
			for (int i = 0; i < dt.Rows.Count; i++)
			{
                lstAttachments.Items.Add(new RadListBoxItem(dt.Rows[i]["Text"].ToString(), dt.Rows[i]["Value"].ToString()));                
			}
			Session["AttachFiles"] = null;
		}
		else if (!trFrom.Visible && Session["corres"].ToString() == "1")
		{
            EditCorrespondenceEmail(ViewState["CORRESSPONDENCEID"].ToString(), ViewState["EMPID"].ToString());
		}
		else
			ResetFields();

	}
    private void SetUserMailId()
    {
        DataSet ds = PhoenixUser.UserEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["USEREMAIL"] = ds.Tables[0].Rows[0]["FLDEMAIL"].ToString();
            if (General.GetNullableString(ViewState["USEREMAIL"].ToString()) != null)
                txtCC.Text = ViewState["USEREMAIL"].ToString();
        }
    }
    private void SetDefaultMessage()
    {
        txtBody.Content = "Note: Kindly do not use the reply button.Please use  " + ViewState["USEREMAIL"].ToString() + " mail id  to reply";
    }

    protected void btnAttDel_Click(object sender, EventArgs e)
    {
        ShowCheckedItems(lstAttachments);
    }

    private void ShowCheckedItems(RadListBox listBox)
    {
        StringBuilder sb = new StringBuilder();
        IList<RadListBoxItem> collection = listBox.CheckedItems;

        string sessionid = ViewState["mailsessionid"].ToString();

        foreach (RadListBoxItem item in collection)
        {
            listBox.Delete(item);
            Message(sessionid, item.Text);
        }
    }

    public static void Message(string sessionid, string filename)
    {
        try
        {
            string destPath = HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/" + sessionid + "/" + filename);
            System.IO.File.Delete(destPath);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }

    protected void ddlEmailTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetEmailTemplate(General.GetNullableInteger(ddlEmailTemplate.SelectedTemplate));
    }
    protected void SetEmailTemplate(int? iTempldateId)
    {
        try
        {
            if (iTempldateId.HasValue)
            {
                DataSet ds = PhoenixCommonEmailTemplate.ListEmailTemplate(null, iTempldateId);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtBody.Content = ds.Tables[0].Rows[0]["FLDTEMPLATE"].ToString();
                    txtSubject.Text = ds.Tables[0].Rows[0]["FLDSUBJECT"].ToString();
                }
                DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(ViewState["EMPID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtBody.Content = txtBody.Content.Replace("#FIRSTNAME#", dt.Rows[0]["FLDFIRSTNAME"].ToString()).Replace("#MIDDLENAME#", dt.Rows[0]["FLDMIDDLENAME"].ToString())
                        .Replace("#LASTNAME#", dt.Rows[0]["FLDLASTNAME"].ToString()).Replace("#PORTALLINK#", Session["sitepath"] + "/Portal/PortalSeafarerDefault.aspx");
                    txtSubject.Text = txtSubject.Text + " " + dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
