using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class InspectionCorrespondence : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridDataItem r in gvCorrespondence.Items)
		{
			if (r is GridDataItem)
			{
				Page.ClientScript.RegisterForEventValidation
						(r.UniqueID + "$ctl00");
				Page.ClientScript.RegisterForEventValidation
						(r.UniqueID + "$ctl01");
			}
		}
		base.Render(writer);
	}

    protected void Page_Load(object sender, EventArgs e)
    {       
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

			PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionCorrespondence.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCorrespondence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
			toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('CI','','"+Session["sitepath"]+"/Crew/CrewCorrespondenceFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
			MenuCrewCorrespondence.AccessRights = this.ViewState;
			MenuCrewCorrespondence.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
            
            MenuCorrespondence.AccessRights = this.ViewState;
            MenuCorrespondence.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                string strEmployeeId = string.Empty;
                if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                    strEmployeeId = string.Empty;
                else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                    strEmployeeId = Request.QueryString["empid"];
                else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                    strEmployeeId = Filter.CurrentCrewSelection;
                if (!string.IsNullOrEmpty(Request.QueryString["PNIID"]))
                {
                    ViewState["PNIID"] = Request.QueryString["PNIID"];
                }
                ViewState["EMPID"] = strEmployeeId;
                DataTable dt = PhoenixIntegrationQuality.ListEmployeeAddress(int.Parse(strEmployeeId));
                ViewState["EMPEMAIL"] = dt.Rows.Count > 0 ? dt.Rows[0]["FLDEMAIL"].ToString() : string.Empty;
                
                if (General.GetNullableInteger(ViewState["EMPID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;
                ViewState["CORRESPONDENCEID"] = string.Empty;
                ViewState["mailsessionid"] = string.Empty;
                SetEmployeePrimaryDetails();
                trBCC.Visible = false;
                trCC.Visible = false;
                trAtt.Visible = false;
                trFrom.Visible = false;
                trTO.Visible = false;
                txtTO.CssClass = "input";
                txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);

                txtSubject.CssClass = "readonlytextbox";
                txtSubject.ReadOnly = true;
                ddlCorrespondenceType.Enabled = false;
                txtBodyDiv.Visible = true;
                txtBodyDiv.InnerText = "";
                txtBody.Visible = false;
            }
            BindData();
               
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

	protected void CrewCorrespondence_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
			{
				BindData();
			}
          else  if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void Correspondence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["CORRESPONDENCEID"].ToString() == string.Empty)
                {
                    PhoenixInspectionPNI.InsertPNICorrespondence(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
                                                                    , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content,1);
                }
                else
                {
                    PhoenixInspectionPNI.UpdatePNICorrespondence(int.Parse(ViewState["CORRESPONDENCEID"].ToString()), int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
                                                                        , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content,1);
                    ucStatus.Text = "Correspondence Information Updated";
                }
                BindData();
                gvCorrespondence.Rebind();
               
            }
            else if (CommandName.ToUpper().Equals("SEND"))
            {
                if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick))
                {
                    ucError.Visible = true;
                    return;
                }
                string dtkey = string.Empty;
                string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString();
                DataSet ds = PhoenixInspectionPNI.InsertPNICorrespondence(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
                                                                , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content,null);
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
                            string desgpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                            desgpath = desgpath + "/Crew/" + tempkey + f.Extension;
                            f.CopyTo(desgpath, true);

                        }
                    }
                }
                PhoenixMail.SendMail(txtTO.Text, txtCC.Text, txtCC.Text, txtSubject.Text, txtBody.Content
                               , true, System.Net.Mail.MailPriority.Normal, ViewState["mailsessionid"].ToString(), General.GetNullableInteger(ViewState["EMPID"].ToString()));
              
                gvCorrespondence.Rebind();
               
            }
			
            if (!CommandName.ToUpper().Equals("NEWMAIL"))
            {
                ViewState["CORRESPONDENCEID"] = string.Empty;
                ResetFields();
                txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
           
                trBCC.Visible = false;
                trCC.Visible = false;
                trAtt.Visible = false;
                trFrom.Visible = false;
                trTO.Visible = false;
                txtTO.CssClass = "input";
                
                if (CommandName.ToUpper().Equals("DISCARD"))
                {
                    if (lstAttachments.Items.Count > 0)
                    {
                        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString();
                        if (Directory.Exists(path))
                            Directory.Delete(path, true);
                        lstAttachments.Items.Clear();
                    }
                }
                ViewState["mailsessionid"] = string.Empty;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW",ToolBarDirection.Right);  
               
                MenuCorrespondence.MenuList = toolbar.Show();

				txtSubject.CssClass = "input_mandatory";
				txtSubject.ReadOnly = false;
				ddlCorrespondenceType.Enabled = true;
                txtBodyDiv.Visible = false;
                txtBody.Visible = true;
                //txtBody.ActiveMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Design;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditCorrespondence(string correspondenceid, string employeeid)
    {
        DataTable dt = PhoenixInspectionPNI.EditPNICorrespondence(General.GetNullableInteger(correspondenceid).Value, General.GetNullableInteger(employeeid));

        if (dt.Rows.Count > 0)
        {
            ViewState["CORRESPONDENCEID"] = dt.Rows[0]["FLDCORRESPONDENCEID"].ToString();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
			    
            //toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewCorrespondenceLockAndUnlock.aspx?id=" + dt.Rows[0]["FLDCORRESPONDENCEID"].ToString() + "&empid=" + ViewState["EMPID"].ToString() + "','medium'); return false;", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "Lock" : "UnLock", "", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "LOCK" : "UNLOCK");
            MenuCorrespondence.MenuList = toolbar.Show();
            txtFrom.Text = dt.Rows[0]["FLDFROM"].ToString();
            txtTO.Text = dt.Rows[0]["FLDTO"].ToString();
            txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
            txtBody.Content = dt.Rows[0]["FLDBODY"].ToString();
            txtBodyDiv.InnerHtml = dt.Rows[0]["FLDBODY"].ToString();
            txtDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDCREATEDDATE"].ToString()));
            ddlCorrespondenceType.SelectedQuick = dt.Rows[0]["FLDCORRESPONDENCETYPE"].ToString();
            ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
        }

		txtSubject.CssClass = "readonlytextbox";
		txtSubject.ReadOnly = true;
		ddlCorrespondenceType.Enabled = false;
		//ddlEmailTemplate.Enabled = false;
		txtBody.Visible = false;
        txtBodyDiv.Visible = true;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCREATEDDATE", "FLDSUBJECT", "FLDCORRESPONDENCETYPENAME", "FLDCREATEDBYNAME", "FLDTYPEOF" };
        string[] alCaptions = { "Created Date", "Subject", "Correspondence Type", "Created By", "Type Of" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.CurrentCorrespondenceFilter;
        DataTable dt = new DataTable();
        if (Filter.CurrentCorrespondenceFilter != null)
        {
            nvc["filter"] = "0";
            dt = PhoenixInspectionPNI.SearchPNICorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
                                                                                           sortexpression, sortdirection,
                                                                                     (int)ViewState["PAGENUMBER"],
                                                                                     General.ShowRecords(null),
                                                                                     ref iRowCount,
                                                                                     ref iTotalPageCount,
                                                                                     General.GetNullableString(nvc.Get("txtSubject").ToString()),
                                                                                     General.GetNullableInteger(nvc.Get("ddlCorrespondenceType").ToString()));


        }
        else
        {
            dt = PhoenixInspectionPNI.SearchPNICorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
                                                                               sortexpression, sortdirection,
                                                                         (int)ViewState["PAGENUMBER"],
                                                                         General.ShowRecords(null),
                                                                         ref iRowCount,
                                                                         ref iTotalPageCount,
                                                                         null, null);
        }

        General.ShowExcel("Crew P&I Correspondence", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCREATEDDATE", "FLDSUBJECT", "FLDCORRESPONDENCETYPENAME", "FLDCREATEDBYNAME", "FLDTYPEOF" };
            string[] alCaptions = { "Created Date", "Subject", "Correspondence Type", "Created By", "Type Of" };
			NameValueCollection nvc = Filter.CurrentCorrespondenceFilter;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
			DataTable dt = new DataTable();
			if (Filter.CurrentCorrespondenceFilter != null)
			{
				nvc["filter"] = "0";
                dt = PhoenixInspectionPNI.SearchPNICorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
																							   sortexpression, sortdirection,
																						 (int)ViewState["PAGENUMBER"],
																						 gvCorrespondence.PageSize,
																						 ref iRowCount,
																						 ref iTotalPageCount,
																						 General.GetNullableString(nvc.Get("txtSubject").ToString()),
																						 General.GetNullableInteger(nvc.Get("ddlCorrespondenceType").ToString()));


			}
			else
			{
                dt = PhoenixInspectionPNI.SearchPNICorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
																				   sortexpression, sortdirection,
																			 (int)ViewState["PAGENUMBER"],
                                                                             gvCorrespondence.PageSize,
																			 ref iRowCount,
																			 ref iTotalPageCount,
																			 null, null);
			}
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            ds.AcceptChanges();
            General.SetPrintOptions("gvCorrespondence", "Crew P&I Correspondence", alCaptions, alColumns, ds);
            gvCorrespondence.DataSource = dt;
            gvCorrespondence.VirtualItemCount = iRowCount;
           

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
      
    private void ResetFields()
    {
		ViewState["CORRESPONDENCEID"] = string.Empty;
        txtFrom.Text = string.Empty;
        txtTO.Text = string.Empty;
        txtSubject.Text = string.Empty;
        txtBody.Content = string.Empty;
        ddlCorrespondenceType.SelectedQuick = string.Empty;
    }
    public bool IsValidCorrespondence(string subject, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;

        if (string.IsNullOrEmpty(subject))
            ucError.ErrorMessage = "Subject is required.";

        if (!int.TryParse(type, out resultInt))
            ucError.ErrorMessage = "Correspondence Type is required.";

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
            DataTable dt = PhoenixIntegrationQuality.EmployeeList(General.GetNullableInteger(ViewState["EMPID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
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
				lstAttachments.Items.Add(new ListItem(dt.Rows[i]["Text"].ToString(), dt.Rows[i]["Value"].ToString()));
			}
			Session["AttachFiles"] = null;
		}
		else if (!trFrom.Visible && Session["corres"].ToString() == "1")
		{
            EditCorrespondence(ViewState["CORRESPONDENCEID"].ToString(), ViewState["EMPID"].ToString());
		}
		else
			ResetFields();
			BindData();
	}

    protected void gvCorrespondence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCorrespondence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCorrespondence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName.ToUpper()== "DELETE")
        {
            try
            {
               
                string id = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;

                PhoenixInspectionPNI.DeletePNICorrespondence(Convert.ToInt32(id), int.Parse(ViewState["EMPID"].ToString()));

                BindData();
                gvCorrespondence.Rebind();

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvCorrespondence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                string strlock = ((RadLabel)e.Item.FindControl("lblLockYN")).Text;
                if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('CI','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.CORRESPONDENCE + (strlock == "1" ? "&U=N" : string.Empty) + "&cmdname=CORRESUPLOAD'); return false;");

                HtmlAnchor mail = (HtmlAnchor)e.Item.FindControl("cmdMail");
                mail.HRef = "InspectionPersonalGeneral.aspx?email=true&cid=" + drv["FLDCORRESPONDENCEID"].ToString() + "&PNIID=" + ViewState["PNIID"].ToString();

                LinkButton chkMail = (LinkButton)e.Item.FindControl("lnkCorrespondence");

                if (drv["FLDTYPE"].ToString() != "1")
                {
                    chkMail.Attributes["target"] = "filterandsearch";
                    chkMail.Attributes["href"] = "CrewPersonalGeneral.aspx?email=true&cid=" + drv["FLDCORRESPONDENCEID"].ToString();
                }
            }
        
        if (e.Item is GridDataItem)
        {
            //// Get the LinkButton control in the first cell
            //LinkButton _doubleClickButton = (LinkButton)e.Item.FindControl("lnkCorrespondence");
            //// Get the javascript which is assigned to this LinkButton
            //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            //// Add this javascript to the onclick Attribute of the row
            //e.Item.Attributes["onclick"] = _jsDouble;
        }
    }

    protected void gvCorrespondence_EditCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

           
            string id = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;
            string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
            e.Item.Selected = true;

            EditCorrespondence(id, empid);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
