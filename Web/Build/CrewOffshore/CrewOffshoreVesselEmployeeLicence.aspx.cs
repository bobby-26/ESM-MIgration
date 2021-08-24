using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;

public partial class CrewOffshoreVesselEmployeeLicence : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCrewLicence.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridViewRow r in gvFlagEndorsement.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridViewRow r in gvDCE.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
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
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                Filter.CurrentVesselCrewSelection = Request.QueryString["empid"];
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreVesselEmployeeLicence.aspx?e=1", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewLicence')", "Print Grid", "icon_print.png", "PRINT");
            MenuCrewLicence.AccessRights = this.ViewState;
            MenuCrewLicence.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreVesselEmployeeLicence.aspx?e=2", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar.AddImageLink("javascript:CallPrint('gvFlagEndorsement')", "Print Grid", "icon_print.png", "PRINT");
            CrewFE.AccessRights = this.ViewState;
            CrewFE.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreVesselEmployeeLicence.aspx?e=3", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar.AddImageLink("javascript:CallPrint('gvDCE')", "Print Grid", "icon_print.png", "PRINT");
            CrewDCE.AccessRights = this.ViewState;
            CrewDCE.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                DataSet ds = PhoenixRegistersThirdPartyLinks.EditThirdPartyLinks(2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cocChecker.HRef = ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString();
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
            }

            BindData();
            SetPageNavigator();
            BindFEData();
            SetFEPageNavigator();
            BindDCEData();
            SetDCEPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
        BindFEData();
        SetFEPageNavigator();
        BindDCEData();
        SetDCEPageNavigator();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
            string[] alCaptions = { "Licence", "Licence Number ", "Place of Issue", "Issue Date", "Expiry Date", "Nationality", "Verified", "Issuing Authority" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(Filter.CurrentVesselCrewSelection.ToString()), 0, 1
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , General.ShowRecords(null)
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvCrewLicence", "National Documents", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewLicence.DataSource = ds;
                gvCrewLicence.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewLicence);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "1")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
                string[] alCaptions = { "Licence", "Licence Number ", "Place of Issue", "Issue Date", "Expiry Date", "Nationality", "Verified", "Issuing Authority" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                       Int32.Parse(Filter.CurrentVesselCrewSelection.ToString()), 0, 1
                       , sortexpression, sortdirection
                       , 1
                       , General.ShowRecords(null)
                       , ref iRowCount
                       , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("National Documents", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "2")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
                string[] alCaptions = { "Licence", "Licence Number ", "Place of Issue", "Issue Date", "Expiry Date", "Flag", "Verified", "Issuing Authority" };
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;

                if (ViewState["FESORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["FESORTDIRECTION"].ToString());
                int? iEmpLicenceId = null;
                if (gvCrewLicence.SelectedIndex > -1)
                {
                    iEmpLicenceId = General.GetNullableInteger(((Label)gvCrewLicence.Rows[gvCrewLicence.SelectedIndex].FindControl(gvCrewLicence.EditIndex > -1 ? "lblLicenceIdEdit" : "lblLicenceId")).Text);
                }

                DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(Filter.CurrentVesselCrewSelection),
                             iEmpLicenceId, 1
                            , sortexpression, sortdirection
                            , 1
                            , General.ShowRecords(null)
                            , ref iRowCount
                            , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Flag Documents", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "3")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
                string[] alCaptions = { "DCE", "Number ", "Place of Issue", "Issue Date", "Expiry Date", "Flag", "Verified", "Issuing Authority" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                       Int32.Parse(Filter.CurrentVesselCrewSelection.ToString()), 1, 1
                       , sortexpression, sortdirection
                       , 1
                       , General.ShowRecords(null)
                       , ref iRowCount
                       , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Dangerous Cargo Endorsements", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewLicence_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToString().ToUpper() == "VIEW")
            {
                BindData();
                SetPageNavigator();
                _gridView.SelectedIndex = nCurrentRow;
                BindFEData();
                SetFEPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCrewLicence_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = e.NewSelectedIndex;
        _gridView.EditIndex = -1;
        BindData();
        SetPageNavigator();
        gvFlagEndorsement.EditIndex = -1;
        gvFlagEndorsement.SelectedIndex = -1;
        BindFEData();
        SetFEPageNavigator();
    }

    protected void gvCrewLicence_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["onclick"] = _jsDouble;

        }
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
                if (drv["FLDISATTACHMENT"].ToString() == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");

                Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
                Image imgFlag = (Image)e.Row.FindControl("imgFlag");
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

                Label lbl = (Label)e.Row.FindControl("lblLicenceId");

                Label lbtn = (Label)e.Row.FindControl("lblVerified");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipLicense");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            else
            {
                e.Row.Attributes["onclick"] = "";
            }
            UserControlDocuments ucDocuments = (UserControlDocuments)e.Row.FindControl("ddlLicenceEdit");
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDLICENCE"].ToString();

            UserControlCountry ucFlag = (UserControlCountry)e.Row.FindControl("ddlFlagEdit");
            if (ucFlag != null) ucFlag.SelectedCountry = drv["FLDFLAGID"].ToString();
        }

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvCrewLicence.SelectedIndex = -1;
            gvCrewLicence.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private Boolean IsPreviousEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;

    }

    private Boolean IsNextEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private bool IsValidLicence(string licence, string licencenumber, UserControlDate dateofissue, UserControlDate dateofexpiry, string placeofissue, string country)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(licence, out resultInt))
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
        if (!Int16.TryParse(country, out resultInt))
            ucError.ErrorMessage = "Nationality is required";

        return (!ucError.IsError);
    }

    private bool IsValidLicenceFlagEndorsement(string empliecneid, string licence, string licencenumber, string placeofissue, string dateofissue, UserControlDate dateofexpiry, string flag)
    {
        Int16 resultInt;
        DateTime resultDate;
        DateTime dtExpiryDate = DateTime.Now;
        DateTime dtIddueDate = DateTime.Now;
        if (gvCrewLicence.EditIndex == -1 && gvCrewLicence.SelectedIndex > -1)
        {
            DateTime.TryParse(((Label)gvCrewLicence.Rows[gvCrewLicence.SelectedIndex].FindControl("lblIssueDate")).Text, out dtIddueDate);
            DateTime.TryParse(((Label)gvCrewLicence.Rows[gvCrewLicence.SelectedIndex].FindControl("lblExpiryDate")).Text, out dtExpiryDate);
        }
        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(empliecneid, out resultInt))
            ucError.ErrorMessage = "Select a Licence to add Endorsement";

        if (!Int16.TryParse(licence, out resultInt))
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
        if (DateTime.TryParse(dateofexpiry.Text, out resultDate) && DateTime.Compare(resultDate, dtExpiryDate) > 0)
        {
            ucError.ErrorMessage = "Exipry Date should be less than National Document Expiry Date";
        }
        if (!Int16.TryParse(flag, out resultInt))
            ucError.ErrorMessage = "Flag is required";
        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentVesselCrewSelection)); 
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();
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
                issuedate = (UserControlDate)gvCrewLicence.FooterRow.FindControl("txtIssueDateAdd");
                expirydate = (UserControlDate)gvCrewLicence.FooterRow.FindControl("txtExpiryDateAdd");

            }
            else
            {
                GridViewRow row = ((GridViewRow)dc.Parent.Parent);
                issuedate = (UserControlDate)row.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
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
                issuedate = (UserControlDate)gvDCE.FooterRow.FindControl("txtIssueDateAdd");
                expirydate = (UserControlDate)gvDCE.FooterRow.FindControl("txtExpiryDateAdd");

            }
            else
            {
                GridViewRow row = ((GridViewRow)dc.Parent.Parent);
                issuedate = (UserControlDate)row.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
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
                expirydate = (UserControlDate)gvFlagEndorsement.FooterRow.FindControl("txtExpiryDateAdd");

            }
            else
            {
                GridViewRow row = ((GridViewRow)dc.Parent.Parent);
                expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
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
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
            string[] alCaptions = { "Licence", "Licence Number ", "Place of Issue", "Issue Date", "Expiry Date", "Flag", "Verified", "Issuing Authority" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["FESORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["FESORTDIRECTION"].ToString());
            int? iEmpLicenceId = null;
            if (gvCrewLicence.SelectedIndex > -1)
            {
                iEmpLicenceId = General.GetNullableInteger(((Label)gvCrewLicence.Rows[gvCrewLicence.SelectedIndex].FindControl(gvCrewLicence.EditIndex > -1 ? "lblLicenceIdEdit" : "lblLicenceId")).Text);
            }

            DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(Filter.CurrentVesselCrewSelection),
                         iEmpLicenceId, 1
                        , sortexpression, sortdirection
                        , (int)ViewState["FEPAGENUMBER"]
                        , General.ShowRecords(null)
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvFlagEndorsement", "Flag Documents", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFlagEndorsement.DataSource = ds;
                gvFlagEndorsement.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvFlagEndorsement);
            }

            ViewState["FEROWCOUNT"] = iRowCount;
            ViewState["FETOTALPAGECOUNT"] = iTotalPageCount;
            //if (gvCrewLicence.SelectedIndex > -1)
            //{
            //    string licname = ((Label)gvCrewLicence.Rows[gvCrewLicence.SelectedIndex].FindControl(gvCrewLicence.EditIndex == -1 ? "lblLicenceName" : "lblLicenceNameEdit")).Text;
            //    ((TextBox)gvFlagEndorsement.FooterRow.FindControl("txtLicenceName")).Text = licname;
            //}

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFlagEndorsement_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToString().ToUpper() == "VIEW")
            {
                _gridView.SelectedIndex = nCurrentRow;
                BindFEData();
                SetFEPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFlagEndorsement_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["onclick"] = _jsDouble;

        }
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

                Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
                Image imgFlag = (Image)e.Row.FindControl("imgFlag");
                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
                if (drv["FLDISATTACHMENT"].ToString() == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");

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
                Label lbtn = (Label)e.Row.FindControl("lblVerified");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipLicense");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                UserControlCountry ucCountry = (UserControlCountry)e.Row.FindControl("ddlFlagEdit");
                if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDFLAGID"].ToString();
            }
            else
            {
                e.Row.Attributes["onclick"] = "";
            }
            UserControlDocuments ucDocuments = (UserControlDocuments)e.Row.FindControl("ddlLicenceEdit");
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDLICENCEID"].ToString();

            UserControlFlag ucFlag = (UserControlFlag)e.Row.FindControl("ddlFlagEdit");
            if (ucFlag != null) ucFlag.SelectedFlag = drv["FLDFLAGID"].ToString();
        }

    }

    protected void gvFlagEndorsement_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = e.NewSelectedIndex;
        _gridView.EditIndex = -1;
        BindFEData();
        SetFEPageNavigator();
    }

    private Boolean IsFEPreviousEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["FEPAGENUMBER"];
        iTotalPageCount = (int)ViewState["FETOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;

    }

    private Boolean IsFENextEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["FEPAGENUMBER"];
        iTotalPageCount = (int)ViewState["FETOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void SetFEPageNavigator()
    {
        try
        {
            cmdFEPrevious.Enabled = IsFEPreviousEnabled();
            cmdFENext.Enabled = IsFENextEnabled();
            lblFEPagenumber.Text = "Page " + ViewState["FEPAGENUMBER"].ToString();
            lblFEPages.Text = " of " + ViewState["FETOTALPAGECOUNT"].ToString() + " Pages. ";
            lblFERecords.Text = "(" + ViewState["FEROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdFEGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtfenopage.Text, out result))
            {
                ViewState["FEPAGENUMBER"] = Int32.Parse(txtfenopage.Text);

                if ((int)ViewState["FETOTALPAGECOUNT"] < Int32.Parse(txtfenopage.Text))
                    ViewState["FEPAGENUMBER"] = ViewState["FETOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtfenopage.Text))
                    ViewState["FEPAGENUMBER"] = 1;

                if ((int)ViewState["FEPAGENUMBER"] == 0)
                    ViewState["FEPAGENUMBER"] = 1;

                txtfenopage.Text = ViewState["FEPAGENUMBER"].ToString();
            }
            BindFEData();
            SetFEPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void FEPagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvFlagEndorsement.SelectedIndex = -1;
            gvFlagEndorsement.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["FEPAGENUMBER"] = (int)ViewState["FEPAGENUMBER"] - 1;
            else
                ViewState["FEPAGENUMBER"] = (int)ViewState["FEPAGENUMBER"] + 1;

            BindFEData();
            SetFEPageNavigator();
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
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
            string[] alCaptions = { "DCE", "Number ", "Place of Issue", "Issue Date", "Expiry Date", "Flag", "Verified", "Issuing Authority" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(Filter.CurrentVesselCrewSelection.ToString()), 1, 1
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , General.ShowRecords(null)
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvDCE", "Dangerous Cargo Endorsements", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDCE.DataSource = ds;
                gvDCE.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvDCE);
            }

            ViewState["DCEROWCOUNT"] = iRowCount;
            ViewState["DCETOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDCE_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToString().ToUpper() == "VIEW")
            {
                _gridView.SelectedIndex = nCurrentRow;
                BindDCEData();
                SetDCEPageNavigator();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDCE_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["onclick"] = _jsDouble;

        }
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
              && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
                if (drv["FLDISATTACHMENT"].ToString() == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");

                Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
                Image imgFlag = (Image)e.Row.FindControl("imgFlag");
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

                Label lbtn = (Label)e.Row.FindControl("lblVerified");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipLicense");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            }
            else
            {
                e.Row.Attributes["onclick"] = "";
            }
            UserControlDocuments ucDocuments = (UserControlDocuments)e.Row.FindControl("ddlLicenceEdit");
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDLICENCE"].ToString();

            UserControlFlag ucFlag = (UserControlFlag)e.Row.FindControl("ddlFlagEdit");
            if (ucFlag != null) ucFlag.SelectedFlag = drv["FLDFLAGID"].ToString();

        }
    }

    private Boolean IsDCEPreviousEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["DCEPAGENUMBER"];
        iTotalPageCount = (int)ViewState["DCETOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;

    }
    private Boolean IsDCENextEnabled()
    {

        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["DCEPAGENUMBER"];
        iTotalPageCount = (int)ViewState["DCETOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void SetDCEPageNavigator()
    {
        try
        {
            cmdDCEPrevious.Enabled = IsDCEPreviousEnabled();
            cmdDCENext.Enabled = IsDCENextEnabled();
            lblDCEPagenumber.Text = "Page " + ViewState["DCEPAGENUMBER"].ToString();
            lblDCEPages.Text = " of " + ViewState["DCETOTALPAGECOUNT"].ToString() + " Pages. ";
            lblDCERecords.Text = "(" + ViewState["DCEROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdDCEGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtdcenopage.Text, out result))
            {
                ViewState["DCEPAGENUMBER"] = Int32.Parse(txtdcenopage.Text);

                if ((int)ViewState["DCETOTALPAGECOUNT"] < Int32.Parse(txtdcenopage.Text))
                    ViewState["DCEPAGENUMBER"] = ViewState["DCETOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtdcenopage.Text))
                    ViewState["DCEPAGENUMBER"] = 1;

                if ((int)ViewState["DCEPAGENUMBER"] == 0)
                    ViewState["DCEPAGENUMBER"] = 1;

                txtdcenopage.Text = ViewState["DCEPAGENUMBER"].ToString();
            }
            BindDCEData();
            SetDCEPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DCEPagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvDCE.SelectedIndex = -1;
            gvDCE.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["DCEPAGENUMBER"] = (int)ViewState["DCEPAGENUMBER"] - 1;
            else
                ViewState["DCEPAGENUMBER"] = (int)ViewState["DCEPAGENUMBER"] + 1;

            BindDCEData();
            SetDCEPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
