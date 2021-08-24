using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class CrewWorkingGearIssueSummary : PhoenixBasePage
{
    string username;
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvRegistersworkinggearitem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvRegistersworkinggearitem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }

        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        username = PhoenixSecurityContext.CurrentSecurityContext.FirstName + "" + PhoenixSecurityContext.CurrentSecurityContext.MiddleName
                     + "" + PhoenixSecurityContext.CurrentSecurityContext.LastName;

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddImageButton("../Crew/CrewWorkingGearIssueSummary.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Crew/CrewWorkingGearIssueSummary.aspx", "<b>Find</b>", "search.png", "FIND");
            toolbar.AddImageButton("../Crew/CrewWorkingGearIssueSummary.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuRegistersWorkingGearItem.AccessRights = this.ViewState;
            MenuRegistersWorkingGearItem.MenuList = toolbar.Show();
            MenuRegistersWorkingGearItem.SetTrigger(pnlWorkingGearItem);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDREFNUMBER", "FLDNAME", "FLDRANKNAME", "FLDISSUEDATE", "FLDVESSELNAME" };
        string[] alCaptions = { "Reference No", "SeaFarer Name", "Rank", "Issue Date", "Vessel Issued for" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewWorkingGearIssueSummary.WorkingGearIssueSummarySearch(General.GetNullableString(txtInvoiceNO.Text.Trim()),
                                                                        General.GetNullableString(txtSeaFarerName.Text.Trim()),
                                                                        General.GetNullableDateTime(txtFromDate.Text),
                                                                        General.GetNullableDateTime(txtToDate.Text),
                                                                        null,//General.GetNullableByte(ddlPayBy.SelectedValue),
                                                                        null,

                                                                        sortexpression,
                                                                        sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        iRowCount, ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("Issue", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersWorkingGearItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                txtInvoiceNO.Text = "";
                txtSeaFarerName.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREFNUMBER", "FLDNAME", "FLDRANKNAME", "FLDISSUEDATE", "FLDVESSELNAME" };
        string[] alCaptions = { "Reference No", "SeaFarer Name", "Rank", "Issue Date", "Vessel Issued for" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewWorkingGearIssueSummary.WorkingGearIssueSummarySearch(General.GetNullableString(txtInvoiceNO.Text.Trim()),
                                                                        General.GetNullableString(txtSeaFarerName.Text.Trim()),
                                                                                General.GetNullableDateTime(txtFromDate.Text),
                                                                                General.GetNullableDateTime(txtToDate.Text),
                                                                                null,// General.GetNullableByte(ddlPayBy.SelectedValue),
                                                                               null,
                                                                                sortexpression,
                                                                                sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvRegistersworkinggearitem", "Issue", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {

            gvRegistersworkinggearitem.DataSource = ds;
            gvRegistersworkinggearitem.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvRegistersworkinggearitem);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvRegistersworkinggearitem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //DropDownList dlpay = (DropDownList)e.Row.FindControl("ddlPayBy");
            //if (dlpay != null)
            //{
            //    dlpay.DataBind();
            //    dlpay.SelectedValue = drv["FLDPAYABLEBY"].ToString();
            //}
            //CheckBox chk = (CheckBox)e.Row.FindControl("chkVerified");
            //if (chk != null)
            //    chk.Checked = drv["FLDVERIFIED"].ToString().Equals("Yes") ? true : false;
            Label lblCrewPlanid = (Label)e.Row.FindControl("lblCrewPlanid");
            Label lblIssueid = (Label)e.Row.FindControl("lblIssueid");
            Label lblCrewId = (Label)e.Row.FindControl("lblCrewId");
            //Label lblTranType       = (Label)e.Row.FindControl("lblTranType");
            //Label lblTransactionId  = (Label)e.Row.FindControl("lblTransactionId");


            LinkButton lb = (LinkButton)e.Row.FindControl("lnkCrew");
            if (lb != null)
                lb.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + lblCrewId.Text + "'); return false;");

            ImageButton rpt = (ImageButton)e.Row.FindControl("cmdRpt");
            //if (rpt != null && lblTranType != null)
            //{

            //    rpt.Visible = SessionUtil.CanAccess(this.ViewState, rpt.CommandName);
            //    rpt.Visible = lblTranType.Text.Equals("2");

            rpt.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=WORKGEARINDIVIDUALREQUEST&Planid=" + lblCrewPlanid.Text + "&Issueid+" + lblCrewPlanid.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=1');return false;");
            //rpt.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=WORKGEARINDIVIDUALREQUEST&Planid=" + lblCrewPlanid.Text + "&Issueid+" + lblCrewPlanid.Text + "&showmenu=0&showword=no&showexcel=no&emailyn=no');return false;");
            //}

        }


    }
    protected void gvRegistersworkinggearitem_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        gvRegistersworkinggearitem.SelectedIndex = se.NewSelectedIndex;
        string lblCrewPlanid = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblCrewPlaniditem")).Text;
        //string Neededid = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblNeededId")).Text;
        //ViewState["Neededid"] = Neededid;
        //ViewState["ORDERID"] = orderid;
        Response.Redirect("CrewWorkGearNeededItem.aspx?crewplanid=" + lblCrewPlanid, false);
        BindData();
    }

    protected void gvRegistersworkinggearitem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvRegistersworkinggearitem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {

            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            //((TextBox)((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantity")).FindControl("txtNumber")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegistersworkinggearitem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string tranid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTransIdEdit")).Text;
            string empid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeIdEdit")).Text;
            string date = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTranDateEdit")).Text;
            string issuevessel = ((Label)_gridView.Rows[nCurrentRow].FindControl("LblIssueVesselIdEdit")).Text;
            string joinvessel = ((Label)_gridView.Rows[nCurrentRow].FindControl("LblJoinVesseIdEdit")).Text;
            string trantype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTranTypeEdit")).Text;
            string refno = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRefNoEdit")).Text;

            //string payby = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlPayBy")).SelectedValue;
            string verified = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkVerified")).Checked ? "1" : "0";
            string verifiedby = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkVerified")).Checked ? username : "";

            //if (General.GetNullableInteger(payby).HasValue)
            //{
            PhoenixCrewWorkingGearIssueSummary.WorkingGearIssuePayByAccountUpdate(General.GetNullableGuid(tranid)
                                                                    , General.GetNullableByte(trantype)
                                                                    , General.GetNullableString(refno)
                                                                    , General.GetNullableInteger(empid)
                                                                    , General.GetNullableDateTime(date)
                                                                    , General.GetNullableInteger(issuevessel)
                                                                    , General.GetNullableInteger(joinvessel)
                                                                    , null//,General.GetNullableByte(payby)
                                                                    , General.GetNullableByte(verified)
                                                                    , General.GetNullableString(username));
            //}
            //else
            //{
            //    ucError.ErrorMessage = "Please fill the required fields";
            //    ucError.ErrorMessage = "Payable by Account is Required.";
            //    ucError.Visible = true;
            //    return;
            //}
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
            gvRegistersworkinggearitem.SelectedIndex = -1;
            gvRegistersworkinggearitem.EditIndex = -1;
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

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
    }
}
