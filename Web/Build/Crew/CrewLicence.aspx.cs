using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewLicence : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            UserControlDocuments dc = ((GridFooterItem)gvCrewLicence.MasterTableView.GetItems(GridItemType.Footer)[0]).FindControl("ddlLicenceAdd") as UserControlDocuments;
            dc.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 1, null, null);
            dc = ((GridFooterItem)gvDCE.MasterTableView.GetItems(GridItemType.Footer)[0]).FindControl("ddlLicenceAdd") as UserControlDocuments;
            dc.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 2, null, null);
            dc = ((GridFooterItem)gvFlagEndorsement.MasterTableView.GetItems(GridItemType.Footer)[0]).FindControl("ddlLicenceAdd") as UserControlDocuments;
            dc.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 3, null, null);
        }
        foreach (GridDataItem r in gvCrewLicence.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridDataItem r in gvFlagEndorsement.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridDataItem r in gvDCE.Items)
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

            PhoenixToolbar toolbarMenu = new PhoenixToolbar();
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbarMenu.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewLicence.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewLicence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewLicenceArchived.aspx?empid=" + Filter.CurrentCrewSelection + "&type=1');return false", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");

            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToUpper() == "OFFSHORE")
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('LICENCE','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreLicenceRequest.aspx?empid=" + Filter.CurrentCrewSelection + "&personalmaster=1');return false", "Initiate Licence Request", "<i class=\"fas fa-passport\"></i>", "Initiate");
            }
            else
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('LICENCE','','" + Session["sitepath"] + "/Crew/CrewLicenceRequestAdd.aspx?empid=" + Filter.CurrentCrewSelection + "&newapp=0');return false", "Initiate Licence Request", "<i class=\"fas fa-passport\"></i>", "Add Licence Request");            
            }
            MenuCrewLicence.AccessRights = this.ViewState;
            MenuCrewLicence.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewLicence.aspx?e=2", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFlagEndorsement')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewFE.AccessRights = this.ViewState;
            CrewFE.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewLicence.aspx?e=3", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDCE')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewDCE.AccessRights = this.ViewState;
            CrewDCE.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                AutoArchive();
                DataSet ds = PhoenixRegistersThirdPartyLinks.ListThirdPartyLinks("COC", PhoenixGeneralSettings.CurrentGeneralSetting.Nationality);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cocChecker.InnerText = "\"" + ds.Tables[0].Rows[0]["FLDNATIONALITYNAME"].ToString() + " COC\" Checker";
                    cocChecker.HRef = ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString().IndexOf("http://") > -1 ? ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString() : "http://" + ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString();
                }
               

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["FEPAGENUMBER"] = 1;
                ViewState["FESORTEXPRESSION"] = null;
                ViewState["FESORTDIRECTION"] = null;
                ViewState["FECURRENTINDEX"] = 1;

                ViewState["DCEPAGENUMBER"] = 1;
                ViewState["DCESORTEXPRESSION"] = null;
                ViewState["DCESORTDIRECTION"] = null;
                ViewState["DCECURRENTINDEX"] = 1;
                SetEmployeePrimaryDetails();

                gvCrewLicence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvFlagEndorsement.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvDCE.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
                    
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

	private void AutoArchive()
	{
		PhoenixCrewLicence.AutoArchiveCrewLicence(Int32.Parse(Filter.CurrentCrewSelection.ToString()));
	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
        gvCrewLicence.Rebind();	
		BindFEData();
        gvFlagEndorsement.Rebind();
        BindDCEData();
        gvDCE.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
			string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDISSUEDBY","FLDREMARKS" };
			string[] alCaptions = { "Licence", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Licence Issuing Country", "Issuing Authority","Limitation Remarks" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()),0,1
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewLicence.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvCrewLicence", "Crew Licence", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewLicence.DataSource = ds;
                gvCrewLicence.VirtualItemCount = iRowCount;               
            }
            else
            {
                gvCrewLicence.DataSource = "";                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "1")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
				string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME",  "FLDISSUEDBY", "FLDREMARKS" };
				string[] alCaptions = { "Licence", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Licence Issuing Country",  "Issuing Authority", "Limitation Remarks" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                       Int32.Parse(Filter.CurrentCrewSelection.ToString()), 0, 1
                       , sortexpression, sortdirection
                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewLicence.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Licence", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "2")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDISSUEDBY" };
                string[] alCaptions = { "Licence", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Flag", "Issuing Authority" };
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;

                if (ViewState["FESORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["FESORTDIRECTION"].ToString());
                int? iEmpLicenceId = null;
                if (gvCrewLicence.SelectedIndexes != null)
                {
                    GridDataItem item = (GridDataItem)gvCrewLicence.Items[0];
                    iEmpLicenceId = General.GetNullableInteger(((RadLabel)item.FindControl("lblLicenceId")).Text);                    
                }                
                DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(Filter.CurrentCrewSelection),
                             iEmpLicenceId, 1
                            , sortexpression, sortdirection
                            , int.Parse(ViewState["FEPAGENUMBER"].ToString())
                            , gvFlagEndorsement.PageSize                           
                            , ref iRowCount
                            , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Flag Endorsement", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "3")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDISSUEDBY" };
                string[] alCaptions = { "Licence", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Flag", "Issuing Authority" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                       Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1, 1
                       , sortexpression, sortdirection
                       , int.Parse(ViewState["DCEPAGENUMBER"].ToString())
                       , gvDCE.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("DCE", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    private bool IsValidLicence(string licence, string licencenumber, UserControlDate dateofissue, UserControlDate dateofexpiry,string placeofissue, string country)
    {
        int resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!int.TryParse(licence, out resultInt))
            ucError.ErrorMessage = "Licence is required";

        if (licencenumber.Trim() == "")
            ucError.ErrorMessage = "Licence Number is required";

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of Issue is required";

        if (string.IsNullOrEmpty(dateofissue.Text) && dateofissue.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Issue Date is required.";
        else if (DateTime.TryParse(dateofissue.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(dateofexpiry.Text) && dateofexpiry.CssClass == "input_mandatory")       
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue.Text != null && dateofexpiry.Text != null)
        {
            if ((DateTime.TryParse(dateofissue.Text, out resultDate)) && (DateTime.TryParse(dateofexpiry.Text, out resultDate)))
                if ((DateTime.Parse(dateofissue.Text)) >= (DateTime.Parse(dateofexpiry.Text)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }
        if (!int.TryParse(country, out resultInt))
            ucError.ErrorMessage = "Nationality is required";

        return (!ucError.IsError);
    }

    private bool IsValidLicenceFlagEndorsement(string empliecneid,string licence, string licencenumber,string placeofissue, string dateofissue, UserControlDate dateofexpiry, string flag)
    {
        int resultInt;
        DateTime resultDate;
        DateTime dtExpiryDate = DateTime.Now;
        DateTime dtIddueDate = DateTime.Now;
		string expirydate="";
		string watchkeeping = "";
        if (ViewState["RowIndex"] != null)
        {
            GridDataItem item = gvCrewLicence.Items[int.Parse(ViewState["RowIndex"].ToString()) - 2];
            DateTime.TryParse(((RadLabel)item.FindControl("lblIssueDate")).Text, out dtIddueDate);
            DateTime.TryParse(((RadLabel)item.FindControl("lblExpiryDate")).Text, out dtExpiryDate);
            RadLabel lblExpirydate = (RadLabel)item.FindControl("lblExpiryDate");
            RadLabel lblwatchkeepingyn = (RadLabel)item.FindControl("lblWatchkeepingyn");
            expirydate = lblExpirydate.Text;
            watchkeeping = lblwatchkeepingyn.Text;
        }        
       
        ucError.HeaderMessage = "Please provide the following required information";

        if (!int.TryParse(empliecneid, out resultInt))
            ucError.ErrorMessage = "Select a Licence to add Endorsement";

        if (!int.TryParse(licence, out resultInt))
            ucError.ErrorMessage = "Licence is required";

        if (licencenumber.Trim() == "")
            ucError.ErrorMessage = "Licence Number is required";

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of Issue is required";

        if (!DateTime.TryParse(dateofissue, out resultDate))
            ucError.ErrorMessage = "Issue Date is required.";
        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }
        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(dtIddueDate, resultDate) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be greater than National Document Issue Date";
        }

        if (!DateTime.TryParse(dateofexpiry.Text, out resultDate) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry Date is required.";


        if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry.Text, out resultDate)))
        {
            if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry.Text)))
                ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }
		if (expirydate != "" || watchkeeping !="1")
		{
			if (DateTime.TryParse(dateofexpiry.Text, out resultDate) && DateTime.Compare(resultDate, dtExpiryDate) > 0)
			{
				ucError.ErrorMessage = "Exipry Date should be less than National Document Expiry Date";
			}
		}
        if (!int.TryParse(flag, out resultInt))
            ucError.ErrorMessage = "Flag is required";
        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
				lblGroup.Text = dt.Rows[0]["FLDGROUP"].ToString();
				if (dt.Rows[0]["FLDOFFCREW"].ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
															50,"OFF"))
				{
					lblOffcrew.Text = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
															50, "RAT");

				}
				else
				{
					lblOffcrew.Text = dt.Rows[0]["FLDOFFCREW"].ToString();
				}

                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ddlDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlDocuments dc = (UserControlDocuments)sender;
        DataSet ds = new DataSet();
        UserControlDate issuedate;
        UserControlDate expirydate;
        if (dc.SelectedDocument != "Dummy")
        {
            ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(General.GetNullableInteger(dc.SelectedDocument).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlLicenceAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
                issuedate = (UserControlDate)Item.FindControl("txtIssueDateAdd");
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateAdd");
                RadTextBox txtLicenceNumberAdd = (RadTextBox)Item.FindControl("txtLicenceNumberAdd");
                txtLicenceNumberAdd.Focus();
            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;                
                issuedate = (UserControlDate)dataItem.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)dataItem.FindControl("txtExpiryDateEdit");
                RadTextBox txtLicenceNumberEdit = (RadTextBox)dataItem.FindControl("txtLicenceNumberEdit");
                txtLicenceNumberEdit.Focus();
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                issuedate.CssClass = "input_mandatory";
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                issuedate.CssClass = "input";
                expirydate.CssClass = "input";
            }
        }
    }
    protected void ddlDCEDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlDocuments dc = (UserControlDocuments)sender;
        DataSet ds = new DataSet();
        UserControlDate issuedate;
        UserControlDate expirydate;
        if (dc.SelectedDocument != "Dummy")
        {
            ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(General.GetNullableInteger(dc.SelectedDocument).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlLicenceAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
                issuedate = (UserControlDate)Item.FindControl("txtIssueDateAdd");
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateAdd");
                RadTextBox txtLicenceNumberAdd = (RadTextBox)Item.FindControl("txtLicenceNumberAdd");
                txtLicenceNumberAdd.Focus();
            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;                
                issuedate = (UserControlDate)dataItem.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)dataItem.FindControl("txtExpiryDateEdit");
                RadTextBox txtLicenceNumberEdit = (RadTextBox)dataItem.FindControl("txtLicenceNumberEdit");
                txtLicenceNumberEdit.Focus();
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                issuedate.CssClass = "input_mandatory";
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                issuedate.CssClass = "input";
                expirydate.CssClass = "input";
            }
        }
    }
    protected void ddlFEDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlDocuments dc = (UserControlDocuments)sender;
        DataSet ds = new DataSet();
        UserControlDate expirydate;
        
        if (dc.SelectedDocument != "Dummy")
        {
            ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(General.GetNullableInteger(dc.SelectedDocument).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlLicenceAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateAdd");
                RadTextBox txtLicenceNumberAdd = (RadTextBox)Item.FindControl("txtLicenceNumberAdd");
                txtLicenceNumberAdd.Focus();

            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
                expirydate = (UserControlDate)dataItem.FindControl("txtExpiryDateEdit");
                RadTextBox txtLicenceNumberEdit = (RadTextBox)dataItem.FindControl("txtLicenceNumberEdit");
                txtLicenceNumberEdit.Focus();
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                expirydate.CssClass = "input";
            }
        }
    }
   
    private void BindFEData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
			string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDISSUEDBY" };
			string[] alCaptions = { "Licence", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Nationality", "Issuing Authority" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["FESORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["FESORTDIRECTION"].ToString());
            int? iEmpLicenceId = null;            
            if (ViewState["RowIndex"] != null)
            {
                GridDataItem item = gvCrewLicence.Items[int.Parse(ViewState["RowIndex"].ToString())-2];
                iEmpLicenceId = General.GetNullableInteger(((RadLabel)item.FindControl("lblLicenceId")).Text);                
            }
            DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(Filter.CurrentCrewSelection),
                         iEmpLicenceId, 1
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["FEPAGENUMBER"].ToString())
                        , gvFlagEndorsement.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvFlagEndorsement", "Flag Endorsement", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFlagEndorsement.DataSource = ds;
                gvFlagEndorsement.VirtualItemCount = iRowCount;
            }
            else
            {
                gvFlagEndorsement.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDCEData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
			string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME",  "FLDISSUEDBY" };
			string[] alCaptions = { "DCE", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Flag","Issuing Authority" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(Filter.CurrentCrewSelection.ToString()), 1, 1
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["DCEPAGENUMBER"].ToString())
                        , gvDCE.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvDCE", "DCE", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDCE.DataSource = ds;
                gvDCE.VirtualItemCount = iRowCount;               
            }
            else
            {
                gvDCE.DataSource = "";
                DataTable dt = ds.Tables[0];                
            }       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewLicence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;

                PhoenixCrewLicence.DeleteCrewLicence(
                   Convert.ToInt32(licenceid)
                    , int.Parse(Filter.CurrentCrewSelection));
                BindData();
                gvCrewLicence.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));                
                UserControlCountry ddlFlag = ((UserControlCountry)e.Item.FindControl("ddlFlagAdd"));
                string verifiedyn = ((CheckBox)e.Item.FindControl("chkVerifiedAdd")).Checked ? "1" : "0";
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;
                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedCountry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewLicence.InsertCrewLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)                    
                    , General.GetNullableInteger(ddlFlag.SelectedCountry)
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , remarks
                    , 0
                     , null
                    );
                BindData();           
                gvCrewLicence.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;                

                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceEdit")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberEdit")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceIdEdit")).Text;                
                UserControlCountry ddlFlag = ((UserControlCountry)e.Item.FindControl("ddlFlagEdit"));
                CheckBox chk = ((CheckBox)e.Item.FindControl("chkVerifiedEdit"));
                string verifiedyn = chk.Checked && chk.Enabled ? "1" : "0";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByEdit")).Text;
                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedCountry))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewLicence.UpdateCrewLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(licenceid)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)                    
                    , General.GetNullableInteger(ddlFlag.SelectedCountry)
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , 0
                    , null
                   );
                BindData();
                gvCrewLicence.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                PhoenixCrewLicence.ArchiveCrewLicence(int.Parse(licenceid), 0);
                BindData();
                gvCrewLicence.Rebind();
                BindFEData();
                gvFlagEndorsement.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                ViewState["RowIndex"] = e.Item.RowIndex;
                //BindFEData();
                gvFlagEndorsement.Rebind();
               
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewLicence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewLicence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewLicence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
        LinkButton cmdArchive = (LinkButton)e.Item.FindControl("cmdArchive");
        if(cmdArchive!=null)
        {
            cmdArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            cmdArchive.Visible = SessionUtil.CanAccess(this.ViewState, cmdArchive.CommandName);
        }
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (cmdEdit != null)
            cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
        
        LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
        if (cmdAttachment != null)
        {
            cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            if (lblIsAtt.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                cmdAttachment.Controls.Add(html);
            }
          
            cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.Trim() + "&mod=" + PhoenixModule.CREW + "&type="+ PhoenixCrewAttachmentType.LICENCE + " &cmdname = LICENCEUPLOAD'); return false;");            

        }        
        RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
        System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
        if (expdate != null)
        {
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Row.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }
        }
        LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");
        RadLabel lbl = (RadLabel)e.Item.FindControl("lblLicenceId");
        RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
        if (imgRemarks != null)
        {
            if (lblR.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();                
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                imgRemarks.Controls.Add(html);
            }
            imgRemarks.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.LICENCE + "','xlarge')");
        }      
        RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
        if (lbtn != null)
        {
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
        DataRowView drv = (DataRowView)e.Item.DataItem;
        UserControlDocuments ddlLicenceEdit = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
        if (ddlLicenceEdit != null)
        {
            ddlLicenceEdit.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 1, null, null);
            if (ddlLicenceEdit.SelectedDocument == "")
                ddlLicenceEdit.SelectedDocument = drv["FLDLICENCE"].ToString();
        }
        UserControlCountry ddlFlagEdit = (UserControlCountry)e.Item.FindControl("ddlFlagEdit");
        if (ddlFlagEdit != null)
        {
            ddlFlagEdit.CountryList = PhoenixRegistersCountry.ListCountry(null);            
            if (ddlFlagEdit.SelectedCountry == "")
                ddlFlagEdit.SelectedCountry = drv["FLDFLAGID"].ToString();
        }
    }
    
    protected void gvFlagEndorsement_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["FEPAGENUMBER"] = ViewState["FEPAGENUMBER"] != null ? ViewState["FEPAGENUMBER"] : gvFlagEndorsement.CurrentPageIndex + 1;
            BindFEData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFlagEndorsement_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
        LinkButton cmdArchive = (LinkButton)e.Item.FindControl("cmdArchive");
        if (cmdArchive != null)
        {
            cmdArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            cmdArchive.Visible = SessionUtil.CanAccess(this.ViewState, cmdArchive.CommandName);
        }
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (cmdEdit != null)
            cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

        LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
        if (cmdAttachment != null)
        {
            cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            if (lblIsAtt.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                cmdAttachment.Controls.Add(html);
            }
            
            cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.Trim() + "&mod=" + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + " &cmdname=ENDORSEMENTUPLOAD'); return false;");
        }
        RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
        System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
        if (expdate != null)
        {
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {                  
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {                 
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }
        }
        RadLabel lbl = (RadLabel)e.Item.FindControl("lblLicenceId");
        RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
        if (lbtn != null)
        {
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
        LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
        if (cmdAdd != null)
        {
            cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
        DataRowView drv = (DataRowView)e.Item.DataItem;
        UserControlDocuments ddlLicenceEdit = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
        if (ddlLicenceEdit != null)
        {
            ddlLicenceEdit.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 3, null, null);
            if (ddlLicenceEdit.SelectedDocument == "")
                ddlLicenceEdit.SelectedDocument = drv["FLDLICENCEID"].ToString();
        }
        UserControlDocuments ddlLicenceAdd = (UserControlDocuments)e.Item.FindControl("ddlLicenceAdd");
        if (ddlLicenceAdd != null)
        {
            ddlLicenceAdd.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 3, null, null);
        }
        UserControlFlag ddlFlagEdit = (UserControlFlag)e.Item.FindControl("ddlFlagEdit");
        if (ddlFlagEdit != null)
        {
            ddlFlagEdit.FlagList = PhoenixRegistersFlag.ListFlag();
            if (ddlFlagEdit.SelectedFlag == "")
                ddlFlagEdit.SelectedFlag = drv["FLDFLAGID"].ToString();
        }

    }

    protected void gvFlagEndorsement_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string endorsementno = ((RadLabel)e.Item.FindControl("lblEndorsementId")).Text;

                PhoenixCrewLicenceFlagEndorsement.DeleteCrewLicenceFlagEndorsement(Convert.ToInt32(endorsementno), int.Parse(Filter.CurrentCrewSelection));
                BindFEData();         
                gvFlagEndorsement.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                int? iEmpLicenceId = null;
                if (ViewState["RowIndex"] != null)
                {
                    GridDataItem item = gvCrewLicence.Items[int.Parse(ViewState["RowIndex"].ToString()) - 2];
                    iEmpLicenceId = General.GetNullableInteger(((RadLabel)item.FindControl("lblLicenceId")).Text);
                }
                else
                {
                    ucError.ErrorMessage = "Complete the National Document amendments before adding a flag document ";
                    ucError.Visible = true;
                    return;
                }
                
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagAdd"));
                string verifiedyn = ((CheckBox)e.Item.FindControl("chkVerifiedAdd")).Checked ? "1" : "0";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;
                string connectedtovessel = ((CheckBox)e.Item.FindControl("chkConnectedToVesselAdd")).Checked ? "1" : "0";
                if (!IsValidLicenceFlagEndorsement(iEmpLicenceId.ToString(), licence, licencenumber, placeofissue, dateofissue, dateofexpiry, ddlFlag.SelectedFlag))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewLicenceFlagEndorsement.InsertCrewLicenceFlagEndorsement(
                      iEmpLicenceId.Value
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue).Value
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    , General.GetNullableInteger(ddlFlag.SelectedFlag).Value
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , byte.Parse(connectedtovessel)
                    );
                gvFlagEndorsement.Rebind();
                BindFEData();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;                
                string endorsementno = ((RadLabel)e.Item.FindControl("lblEndorsementIdEdit")).Text;
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceEdit")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberEdit")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagEdit"));
                RadCheckBox chk = ((RadCheckBox)e.Item.FindControl("chkVerifiedEdit"));
                string verifiedyn;
                if (chk.Enabled == true)
                    verifiedyn = (chk.Checked==true && chk.Enabled==true) ? "1" : "0";
                else
                    verifiedyn = "1";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByEdit")).Text;

                RadCheckBox chk1 = ((RadCheckBox)e.Item.FindControl("chkConnectedtoVesselEdit"));
                string connectedtovessel;
                if (chk1.Enabled == true)
                    connectedtovessel = (chk1.Checked==true && chk1.Enabled==true) ? "1" : "0";
                else
                    connectedtovessel = "1";
                if (!IsValidLicenceFlagEndorsement(endorsementno, licence, licencenumber, placeofissue, dateofissue, dateofexpiry, ddlFlag.SelectedFlag))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewLicenceFlagEndorsement.UpdateCrewLicenceFlagEndorsement(
                      int.Parse(endorsementno)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue).Value
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    , General.GetNullableInteger(ddlFlag.SelectedFlag).Value
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , byte.Parse(connectedtovessel)
                    );                                
                BindFEData();
                gvFlagEndorsement.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblEndorsementId")).Text;
                PhoenixCrewLicenceFlagEndorsement.ArchiveCrewLicenceFlagEndorsement(int.Parse(licenceid), 0);                             
                BindFEData();
                gvFlagEndorsement.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["FEPAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDCE_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;

                PhoenixCrewLicence.DeleteCrewLicence(
                    Convert.ToInt32(licenceid)
                    , int.Parse(Filter.CurrentCrewSelection));
                BindDCEData();
                gvDCE.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));                
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagAdd"));
                string verifiedyn = ((RadCheckBox)e.Item.FindControl("chkVerifiedAdd")).Checked==true ? "1" : "0";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;
                string connectedtovessel = ((RadCheckBox)e.Item.FindControl("chkDCEConnectedToVesselAdd")).Checked==true ? "1" : "0";
                RadComboBox ddlFullterm = ((RadComboBox)e.Item.FindControl("ddlFullTermAdd"));
                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedFlag))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewLicence.InsertCrewLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)                    
                    , General.GetNullableInteger(ddlFlag.SelectedFlag)
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , string.Empty
                    , byte.Parse(connectedtovessel)
                    , General.GetNullableInteger(ddlFullterm.SelectedValue)
                    );
                BindDCEData();
                gvDCE.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceEdit")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberEdit")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceIdEdit")).Text;                
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagEdit"));
                RadCheckBox chk = ((RadCheckBox)e.Item.FindControl("chkVerifiedEdit"));
                RadComboBox ddlFullterm = ((RadComboBox)e.Item.FindControl("ddlFullTermEdit"));
                string verifiedyn;
                if (chk.Enabled == true)
                    verifiedyn = (chk.Checked==true && chk.Enabled==true) ? "1" : "0";
                else
                    verifiedyn = "1";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByEdit")).Text;

                RadCheckBox chk1 = ((RadCheckBox)e.Item.FindControl("chkDCEConnectedtoVesselEdit"));
                string connectedtovessel;
                if (chk1.Enabled == true)
                    connectedtovessel = (chk1.Checked==true && chk1.Enabled==true) ? "1" : "0";
                else
                    connectedtovessel = "1";
                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedFlag))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewLicence.UpdateCrewLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    , Convert.ToInt32(licenceid)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    //, nationallicence.Checked ? byte.Parse("1") : byte.Parse("0")
                    , General.GetNullableInteger(ddlFlag.SelectedFlag)
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , byte.Parse(connectedtovessel)
                    , General.GetNullableInteger(ddlFullterm.SelectedValue)
                   );
                BindDCEData();
                gvDCE.Rebind();
            }

            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                PhoenixCrewLicence.ArchiveCrewLicence(int.Parse(licenceid), 0);

                BindDCEData();
                gvDCE.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["DCEPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDCE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["DCEPAGENUMBER"] = ViewState["DCEPAGENUMBER"] != null ? ViewState["DCEPAGENUMBER"] : gvDCE.CurrentPageIndex + 1;
            BindDCEData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDCE_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
        LinkButton cmdArchive = (LinkButton)e.Item.FindControl("cmdArchive");
        if (cmdArchive != null)
        {
            cmdArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            cmdArchive.Visible = SessionUtil.CanAccess(this.ViewState, cmdArchive.CommandName);
        }
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (cmdEdit != null)
            cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

        LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
        if (cmdAttachment != null)
        {
            cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            if (lblIsAtt.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                cmdAttachment.Controls.Add(html);
            }

            cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.Trim() + "&mod=" + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + " &cmdname=DCEUPLOAD'); return false;");
        }
        RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
        System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
        if (expdate != null)
        {
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Row.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }
        }
        RadLabel lbl = (RadLabel)e.Item.FindControl("lblLicenceId");
        RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
        if (lbtn != null)
        {
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
        DataRowView drv = (DataRowView)e.Item.DataItem;
        UserControlDocuments ddlLicenceEdit = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
        if (ddlLicenceEdit != null)
        {
            ddlLicenceEdit.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 2, null, null);            
            if (ddlLicenceEdit.SelectedDocument == "")
                ddlLicenceEdit.SelectedDocument = drv["FLDLICENCE"].ToString();
        }
        LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
        if (cmdAdd != null)
        {
            cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
      
        UserControlFlag ddlFlagEdit = (UserControlFlag)e.Item.FindControl("ddlFlagEdit");
        if (ddlFlagEdit != null)
        {
            ddlFlagEdit.FlagList = PhoenixRegistersFlag.ListFlag();            
            if (ddlFlagEdit.SelectedFlag == "")
                ddlFlagEdit.SelectedFlag = drv["FLDFLAGID"].ToString();
        }
        RadComboBox ddlFullterm = (RadComboBox)e.Item.FindControl("ddlFullTermEdit");
        if (ddlFullterm != null) ddlFullterm.SelectedValue = drv["FLDFULLTERMYN"].ToString();

    }
}
