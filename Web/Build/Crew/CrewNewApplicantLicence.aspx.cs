using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Drawing;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewNewApplicantLicence : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantLicence.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "LICENCEEXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvNewApplicantLicence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "LICENCEPRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewLicenceArchived.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=1'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            //toolbar.AddImageButton("javascript:parent.Openpopup('LICENCE','','CrewLicenceRequestSelection.aspx?empid=" + Filter.CurrentNewApplicantSelection + "'); return false;", "Initiate Licence Request", "initiate-licence.png", "Initiate");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('LICENCE','','" + Session["sitepath"] + "/Crew/CrewLicenceRequestAdd.aspx?Empid=" + Filter.CurrentNewApplicantSelection + "&newapp=1'); return false;", "Add Licence Request", "<i class=\"fa fa-plus-circle\"></i>", "Add");
            MenuNewApplicantLicence.AccessRights = this.ViewState;
            MenuNewApplicantLicence.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantLicence.aspx?e=2", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "FLAGEXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFlagEndorsement')", "Print Grid", "<i class=\"fas fa-print\"></i>", "FLAGPRINT");
            NewApplicantFE.AccessRights = this.ViewState;
            NewApplicantFE.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantLicence.aspx?e=3", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "DCEEXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDCE')", "Print Grid", "<i class=\"fas fa-print\"></i>", "DCEPRINT");
            NewApplicantDCE.AccessRights = this.ViewState;
            NewApplicantDCE.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            CrewLicence.Title = "Licence Document";
            CrewLicence.AccessRights = this.ViewState;
            CrewLicence.MenuList = toolbar1.Show();

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

                gvNewApplicantLicence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvFlagEndorsement.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvDCE.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            //BindData();
            //SetPageNavigator();
            //BindFEData();
            //SetFEPageNavigator();
            //BindDCEData();
            //SetDCEPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void AutoArchive()
    {
        PhoenixCrewLicence.AutoArchiveCrewLicence(Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvNewApplicantLicence.Rebind();
        gvFlagEndorsement.Rebind();
        gvDCE.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDISSUEDBY", "FLDREMARKS" };
            string[] alCaptions = { "Licence", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Licence Issuing Country", "Issuing Authority", "Limitation Remarks" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixNewApplicantLicence.NewApplicantLicenceSearch(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 0, 1
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , gvNewApplicantLicence.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvNewApplicantLicence", "Licence", alCaptions, alColumns, ds);

            gvNewApplicantLicence.DataSource = ds;
            gvNewApplicantLicence.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void NewApplicantLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LICENCEEXCEL") && Request.QueryString["e"] == "1")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDISSUEDBY", "FLDREMARKS" };
                string[] alCaptions = { "Licence", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Licence Issuing Country", "Issuing Authority", "Limitation Remarks" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataSet ds = PhoenixNewApplicantLicence.NewApplicantLicenceSearch(
                       Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 0, 1
                       , sortexpression, sortdirection
                       , 1
                       , General.ShowRecords(null)
                       , ref iRowCount
                       , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Licence", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("FLAGEXCEL") && Request.QueryString["e"] == "2")
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

                if (gvNewApplicantLicence.SelectedIndexes != null)
                {
                    GridDataItem item = (GridDataItem)gvNewApplicantLicence.Items[0];
                    iEmpLicenceId = General.GetNullableInteger(((RadLabel)item.FindControl("lblLicenceId")).Text);
                }
                DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(Filter.CurrentNewApplicantSelection),
                             iEmpLicenceId, 1
                            , sortexpression, sortdirection
                            , 1
                            , General.ShowRecords(null)
                            , ref iRowCount
                            , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Flag Endorsement", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("DCEEXCEL") && Request.QueryString["e"] == "3")
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
                DataSet ds = PhoenixNewApplicantLicence.NewApplicantLicenceSearch(
                       Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1, 1
                       , sortexpression, sortdirection
                       , 1
                       , General.ShowRecords(null)
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
            gv.Rows[0].Attributes["ondblclick"] = "";
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
            ucError.ErrorMessage = "Flag is required";
        return (!ucError.IsError);
    }
    private bool IsValidLicenceFlagEndorsement(string empliecneid, string licence, string licencenumber, string placeofissue, string dateofissue, UserControlDate dateofexpiry, string flag)
    {
        int resultInt;
        DateTime resultDate;
        DateTime dtExpiryDate = DateTime.Now;
        DateTime dtIddueDate = DateTime.Now;
        string expirydate = "";
        string watchkeeping = "";
        if (ViewState["nCurrentRow"] != null)
        {
            GridDataItem item = gvNewApplicantLicence.Items[int.Parse(ViewState["nCurrentRow"].ToString()) - 2];
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
        if (expirydate != "" || watchkeeping != "1")
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
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                lblGroup.Text = dt.Rows[0]["FLDGROUP"].ToString();
                if (dt.Rows[0]["FLDOFFCREW"].ToString() != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            50, "OFF"))
                {
                    lblOffcrew.Text = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            50, "RAT");

                }
                else
                {
                    lblOffcrew.Text = dt.Rows[0]["FLDOFFCREW"].ToString();
                }
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
                GridFooterItem footerItem = (GridFooterItem)gvNewApplicantLicence.MasterTableView.GetItems(GridItemType.Footer)[0];
                // Button btn = (Button)footerItem.FindControl("Button1");

                issuedate = (UserControlDate)footerItem.FindControl("txtIssueDateAdd");
                expirydate = (UserControlDate)footerItem.FindControl("txtExpiryDateAdd");
                RadTextBox txtLicenceNumberAdd = (RadTextBox)footerItem.FindControl("txtLicenceNumberAdd");
                txtLicenceNumberAdd.Focus();
            }
            else
            {
                GridDataItem row = ((GridDataItem)dc.Parent.Parent);
                issuedate = (UserControlDate)row.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
                RadTextBox txtLicenceNumberEdit = (RadTextBox)row.FindControl("txtLicenceNumberEdit");
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
                GridDataItem Item = (GridDataItem)dc.NamingContainer;
                issuedate = (UserControlDate)Item.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateEdit");
                RadTextBox txtLicenceNumberEdit = (RadTextBox)Item.FindControl("txtLicenceNumberEdit");
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
                GridFooterItem footerItem = (GridFooterItem)gvFlagEndorsement.MasterTableView.GetItems(GridItemType.Footer)[0];

                expirydate = (UserControlDate)footerItem.FindControl("txtExpiryDateAdd");
                RadTextBox txtLicenceNumberAdd = (RadTextBox)footerItem.FindControl("txtLicenceNumberAdd");
                txtLicenceNumberAdd.Focus();
            }
            else
            {
                GridDataItem row = ((GridDataItem)dc.Parent.Parent);
                expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
                TextBox txtLicenceNumberEdit = (TextBox)row.FindControl("txtLicenceNumberEdit");
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
            if (ViewState["nCurrentRow"] != null)
            {
                GridDataItem item = gvNewApplicantLicence.Items[int.Parse(ViewState["nCurrentRow"].ToString()) - 2];
                iEmpLicenceId = General.GetNullableInteger(((RadLabel)item.FindControl("lblLicenceId")).Text);
            }


            DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(Filter.CurrentNewApplicantSelection),
                         iEmpLicenceId, 1
                        , sortexpression, sortdirection
                        , (int)ViewState["FEPAGENUMBER"]
                        , gvFlagEndorsement.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvFlagEndorsement", "Flag Endorsement", alCaptions, alColumns, ds);

            gvFlagEndorsement.DataSource = ds;
            gvFlagEndorsement.VirtualItemCount = iRowCount;


            ViewState["FEROWCOUNT"] = iRowCount;
            ViewState["FETOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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



    private void BindDCEData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDISSUEDBY", "FLDFULLTERM" };
            string[] alCaptions = { "DCE", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Flag", "Issuing Authority", "Full Term YN" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixNewApplicantLicence.NewApplicantLicenceSearch(
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), 1, 1
                        , sortexpression, sortdirection
                        , (int)ViewState["DCEPAGENUMBER"]
                        , gvDCE.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvDCE", "DCE", alCaptions, alColumns, ds);
            gvDCE.DataSource = ds;
            gvDCE.VirtualItemCount = iRowCount;

            ViewState["DCEROWCOUNT"] = iRowCount;
            ViewState["DCETOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvNewApplicantLicence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvNewApplicantLicence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvNewApplicantLicence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        


        try
        {
            if (e.CommandName.ToString().ToUpper() == "NATIONALADD")
            {
               
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                //CheckBox nationallicence = e.Item.FindControl("chkNationalLicenceAdd") as CheckBox;
                UserControlCountry ddlFlag = ((UserControlCountry)e.Item.FindControl("ddlFlagAdd"));
                string verifiedyn = ((CheckBox)e.Item.FindControl("chkVerifiedAdd")).Checked ? "1" : "0";
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;
                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedCountry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixNewApplicantLicence.InsertNewApplicantLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    //, nationallicence.Checked ? byte.Parse("1") : byte.Parse("0")
                    , General.GetNullableInteger(ddlFlag.SelectedCountry)
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , remarks
                    , null
                    );
                BindData();
                gvNewApplicantLicence.Rebind();

            }
            else if (e.CommandName.ToString().ToUpper() == "NATIONALARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                PhoenixCrewLicence.ArchiveCrewLicence(int.Parse(licenceid), 0);

                BindData();
                gvNewApplicantLicence.Rebind();
                BindFEData();
                gvFlagEndorsement.Rebind();
            }

            else if (e.CommandName.ToString().ToUpper() == "NATIONALDELETE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;

                PhoenixNewApplicantLicence.DeleteNewApplicantLicence(
                   Convert.ToInt32(licenceid)
                    , int.Parse(Filter.CurrentNewApplicantSelection));
                BindData();
                gvNewApplicantLicence.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
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

                PhoenixNewApplicantLicence.UpdateNewApplicantLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(licenceid)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    , General.GetNullableInteger(ddlFlag.SelectedCountry)
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , null
                   );
                BindData();
                gvNewApplicantLicence.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["nCurrentRow"] = e.Item.RowIndex;// Int32.Parse(e.CommandArgument.ToString());

                e.Item.Selected = true;
                //BindFEData();
                gvFlagEndorsement.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvNewApplicantLicence_ItemDataBound1(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null) db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            if (db1 != null) db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt1");
            RadLabel lblIsAtt1 = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html2 = new HtmlGenericControl();
                if (lblIsAtt1 != null && lblIsAtt1.Text == string.Empty)
                {
                    html2.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html2);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=NLICENCEUPLOAD'); return false;");
            }



            //if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";


            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            DateTime? d = null;
            if (expdate != null) d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue && imgFlag != null)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Item.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Item.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
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
                imgRemarks.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.LICENCE + "','xlarge'); return false;");
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }


            UserControlCountry ucCountry = (UserControlCountry)e.Item.FindControl("ddlFlagEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDFLAGID"].ToString();
            //UserControlDocuments ucDocuments = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
            //DataRowView drvDocuments = (DataRowView)e.Item.DataItem;
            //if (ucDocuments != null) ucDocuments.SelectedDocument = drvDocuments["FLDLICENCE"].ToString();

            UserControlDocuments ddlLicenceEdit = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
            if (ddlLicenceEdit != null)
            {
                ddlLicenceEdit.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 1, null, null);
                //ddlLicenceEdit.DocumentType = "LICENCE";
                if (ddlLicenceEdit.SelectedDocument == "")
                    ddlLicenceEdit.SelectedDocument = drv["FLDLICENCE"].ToString();
                ddlLicenceEdit.Enabled = (drv["FLDVERIFIEDYN"].ToString() == "1") ? false : true;
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
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

    protected void gvFlagEndorsement_ItemCommand(object sender, GridCommandEventArgs e)
    {

        ViewState["FECURRENTROW"] = null;
        try
        {
            if (e.CommandName.ToString().ToUpper() == "FLAGADD")
            {
                ViewState["FECURRENTROW"] = Int32.Parse(e.CommandArgument.ToString());
                int? iEmpLicenceId = null;

                if (gvNewApplicantLicence.EditItems.Count > 0)
                {
                    ucError.ErrorMessage = "Complete the National Document amendments before adding a flag document ";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["nCurrentRow"] != null)
                {
                    GridDataItem item = gvNewApplicantLicence.Items[int.Parse(ViewState["nCurrentRow"].ToString()) - 2];
                    iEmpLicenceId = General.GetNullableInteger(((RadLabel)item.FindControl("lblLicenceId")).Text);
                }

                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagAdd"));
                string verifiedyn = ((CheckBox)e.Item.FindControl("chkVerifiedAdd")).Checked ? "1" : "0";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;
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
                    , 0
                    );
                gvFlagEndorsement.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "FLAGARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblEndorsementId")).Text;
                PhoenixCrewLicenceFlagEndorsement.ArchiveCrewLicenceFlagEndorsement(int.Parse(licenceid), 0);
                gvFlagEndorsement.Rebind();
            }

            else if (e.CommandName.ToString().ToUpper() == "FLAGDELETE")
            {
                string endorsementno = ((RadLabel)e.Item.FindControl("lblEndorsementId")).Text;

                PhoenixCrewLicenceFlagEndorsement.DeleteCrewLicenceFlagEndorsement(Convert.ToInt32(endorsementno), int.Parse(Filter.CurrentNewApplicantSelection));
                gvFlagEndorsement.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "FLAGUPDATE")
            {
                string endorsementno = ((RadLabel)e.Item.FindControl("lblEndorsementIdEdit")).Text;
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceEdit")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberEdit")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagEdit"));
                CheckBox chk = ((CheckBox)e.Item.FindControl("chkVerifiedEdit"));
                string verifiedyn = chk.Checked && chk.Enabled ? "1" : "0";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByEdit")).Text;

                if (!IsValidLicenceFlagEndorsement(endorsementno, licence, licencenumber, placeofissue, dateofissue, dateofexpiry, ddlFlag.SelectedFlag))
                {
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
                    , 0
                    );
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

    protected void gvFlagEndorsement_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
            RadLabel lblIsAtt2 = (RadLabel)e.Item.FindControl("lblIsAtt");
            HtmlGenericControl html3 = new HtmlGenericControl();
            if (att != null)
            {
                if (lblIsAtt2 != null && lblIsAtt2.Text == string.Empty)
                {
                    html3.InnerHtml = "<span class=\"icon\" style=\"color:gray;\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html3);
                }
                // if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                  + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=NENDORSEMENTUPLOAD'); return false;");
            }

            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            DateTime? d = null;
            if (expdate != null) d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue && imgFlag != null)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Item.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Item.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            UserControlDocuments ucDocuments = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
            DataRowView drvDocuments = (DataRowView)e.Item.DataItem;
            if (ucDocuments != null) ucDocuments.SelectedDocument = drvDocuments["FLDLICENCEID"].ToString();

            UserControlFlag ucFlag = (UserControlFlag)e.Item.FindControl("ddlFlagEdit");
            DataRowView drvFlag = (DataRowView)e.Item.DataItem;
            if (ucFlag != null) ucFlag.SelectedFlag = drvFlag["FLDFLAGID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }

    }

    protected void gvDCE_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {


                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                //CheckBox nationallicence = e.Item.FindControl("chkNationalLicenceAdd") as CheckBox;
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagAdd"));
                string verifiedyn = ((CheckBox)e.Item.FindControl("chkVerifiedAdd")).Checked ? "1" : "0";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;
                DropDownList ddlFullterm = ((DropDownList)e.Item.FindControl("ddlFullTermAdd"));
                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedFlag))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixNewApplicantLicence.InsertNewApplicantLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    //, nationallicence.Checked ? byte.Parse("1") : byte.Parse("0")
                    , General.GetNullableInteger(ddlFlag.SelectedFlag)
                    , byte.Parse(verifiedyn)
                    , issuingauthority
                    , string.Empty
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

            else if (e.CommandName.ToString().ToUpper() == "DCEDELETE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;

                PhoenixNewApplicantLicence.DeleteNewApplicantLicence(
                    Convert.ToInt32(licenceid)
                    , int.Parse(Filter.CurrentNewApplicantSelection));

                BindDCEData();
                gvDCE.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceEdit")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberEdit")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceIdEdit")).Text;
                //CheckBox nationallicence = e.Item.FindControl("chkNationalLicenceEdit") as CheckBox;
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagEdit"));
                CheckBox chk = ((CheckBox)e.Item.FindControl("chkVerifiedEdit"));
                string verifiedyn = chk.Checked && chk.Enabled ? "1" : "0";
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByEdit")).Text;
                DropDownList ddlFullterm = ((DropDownList)e.Item.FindControl("ddlFullTermEdit"));
                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedFlag))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixNewApplicantLicence.UpdateNewApplicantLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
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
                    , General.GetNullableInteger(ddlFullterm.SelectedValue)
                   );

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


        if (e.Item is GridDataItem)
        {

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null) db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
            RadLabel lblIsAtt3 = (RadLabel)e.Item.FindControl("lblIsAtt");

            HtmlGenericControl html1 = new HtmlGenericControl();
            if (lblIsAtt3 != null && lblIsAtt3.Text == string.Empty && att != null)
            {
                html1.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                att.Controls.Add(html1);
            }
            if (att != null)
            {
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=NDCEUPLOAD'); return false;");
            }
            RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            DateTime? d = null;
            if (expdate != null) d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue && imgFlag != null)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Item.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Item.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }

             

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            UserControlDocuments ucDocuments = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
            DataRowView drvDocuments = (DataRowView)e.Item.DataItem;
            if (ucDocuments != null)
            {
                ucDocuments.DocumentList = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 2, null, null);
                if (ucDocuments.SelectedDocument == "")
                    ucDocuments.SelectedDocument = drvDocuments["FLDLICENCE"].ToString();
            }
            UserControlFlag ucFlag = (UserControlFlag)e.Item.FindControl("ddlFlagEdit");
            DataRowView drvFlag = (DataRowView)e.Item.DataItem;
            if (ucFlag != null) ucFlag.SelectedFlag = drvFlag["FLDFLAGID"].ToString();

            DropDownList ddlFullterm = (DropDownList)e.Item.FindControl("ddlFullTermEdit");
            if (ddlFullterm != null) ddlFullterm.SelectedValue = drvDocuments["FLDFULLTERMYN"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }

    }
}
