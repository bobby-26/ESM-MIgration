using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;

public partial class RegistersOffshoreVesselManningScale : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvManningScale.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvManningScale.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Registers/RegistersOffshoreVesselManningScale.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvManningScaleRevision')", "Print Grid", "icon_print.png", "PRINT");
        MenuRegistersRevision.AccessRights = this.ViewState;
        MenuRegistersRevision.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            ViewState["RPAGENUMBER"] = 1;
            ViewState["RSORTEXPRESSION"] = null;
            ViewState["RSORTDIRECTION"] = null;

            ViewState["REVISIONID"] = "";
            ViewState["REQUIRESUPDATEYN"] = "";
            BindRevision();
        }
       
        SetPageNavigatorR();

        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Registers/RegistersOffshoreVesselManningScale.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvManningScale')", "Print Grid", "icon_print.png", "PRINT");
        if(ViewState["REQUIRESUPDATEYN"].ToString().Equals("1"))
            toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','RegistersOffshoreVesselManningScaleDate.aspx','medium')", "Update Effective Date for existing records", "modify.png", "UPDATEDATE");
        MenuRegistersManningScale.AccessRights = this.ViewState; 
        MenuRegistersManningScale.MenuList = toolbar.Show();
        MenuRegistersManningScale.SetTrigger(pnlManningScaleEntry);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKNAME", "FLDEQUIVALENTRANKNAME", "FLDOWNERSCALE", "FLDSAFESCALE", "FLDCBASCALE", "FLDCONTRACTPERIODDAYS", "FLDREMARKS" };
        string[] alCaptions = { "Rank Name", "Equivalent Rank", "Owner Scale", "Safe Scale", "CBA Scale", "Contract Period(days)", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselManningScale.ManningScaleSearch(Int16.Parse(Filter.CurrentVesselMasterFilter)
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(ViewState["REVISIONID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=ManningScale.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel ManningScale</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersManningScale_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDRANKNAME", "FLDEQUIVALENTRANKNAME", "FLDOWNERSCALE", "FLDSAFESCALE", "FLDCBASCALE", "FLDCONTRACTPERIODDAYS", "FLDREMARKS" };
        string[] alCaptions = { "Rank Name", "Equivalent Rank", "Owner Scale", "Safe Scale", "CBA Scale", "Contract Period(days)", "Remarks" };

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));

        DataRow drVessel = dsVessel.Tables[0].Rows[0];

        txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();

        DataSet ds = PhoenixRegistersVesselManningScale.ManningScaleSearch(Int16.Parse(Filter.CurrentVesselMasterFilter)
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableGuid(ViewState["REVISIONID"].ToString()));

        General.SetPrintOptions("gvManningScale", "Registers", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvManningScale.DataSource = ds;
            gvManningScale.DataBind();

            DataRow dr = ds.Tables[1].Rows[0];

            txtOwnerScaleTotal.Text = dr["FLDOWNERSCALETOTAL"].ToString();
            txtSafeScaleTotal.Text = dr["FLDSAFESCALETOTAL"].ToString();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvManningScale);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvManningScale_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvManningScale, "Edit$" + e.Row.RowIndex.ToString(), false);
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvManningScale, "Edit$" + e.Row.RowIndex.ToString(), false);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvManningScale, "Edit$" + e.Row.RowIndex.ToString(), false);
            e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvManningScale, "Edit$" + e.Row.RowIndex.ToString(), false);
            e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvManningScale, "Edit$" + e.Row.RowIndex.ToString(), false);
            e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvManningScale, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvManningScale_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvManningScale.SelectedIndex = -1;
        gvManningScale.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvManningScale_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidManningScale(((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRank")).SelectedRank
              , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerScaleEdit")).Text
              , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSafeScaleEdit")).Text
              , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCBAScaleEdit")).Text
              , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtContractPeriodEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            UpdateManningScale(
                Filter.CurrentVesselMasterFilter
                , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblManningScaleIdEdit")).Text
                , ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRank")).SelectedRank
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerScaleEdit")).Text
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSafeScaleEdit")).Text
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCBAScaleEdit")).Text
                , null //((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtContractPeriodEdit")).Text
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtContractPeriodEdit")).Text);

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvManningScale_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvManningScale.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvManningScale_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((UserControlRank)_gridView.Rows[de.NewEditIndex].FindControl("ucRank")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvManningScale_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertManningScale(
                    Filter.CurrentVesselMasterFilter
                    , ((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).SelectedRank.ToString()
                    , ((TextBox)_gridView.FooterRow.FindControl("txtOwnerScaleAdd")).Text
                    , ((TextBox)_gridView.FooterRow.FindControl("txtSafeScaleAdd")).Text
                    , ((TextBox)_gridView.FooterRow.FindControl("txtCBAScaleAdd")).Text
                    , null //((TextBox)_gridView.FooterRow.FindControl("txtContractPeriodAdd")).Text
                    , ((TextBox)_gridView.FooterRow.FindControl("txtRemarksAdd")).Text
                    , ((TextBox)_gridView.FooterRow.FindControl("txtContractPeriodAdd")).Text
                    , General.GetNullableString(ViewState["REVISIONID"].ToString()));

                BindData();
                ((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                UpdateManningScale(
                    Filter.CurrentVesselMasterFilter
                    , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblManningScaleIdEdit")).Text
                    , ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRank")).SelectedRank.ToString()
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerScaleEdit")).Text
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSafeScaleEdit")).Text
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCBAScaleEdit")).Text
                    , null //((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtContractPeriodEdit")).Text
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtContractPeriodEdit")).Text);

                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteManningScale(Int16.Parse(Filter.CurrentVesselMasterFilter), Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblManningScaleId")).Text));
                BindData();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvManningScale_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvManningScale_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvManningScale_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

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

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            db = (ImageButton)e.Row.FindControl("cmdEdit");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton lb = (LinkButton)e.Row.FindControl("lnkRankEdit");
            if (lb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.CommandName = "";
            }

            UserControlRank ucRank = (UserControlRank)e.Row.FindControl("ucRank");
            DataRowView drvRank = (DataRowView)e.Row.DataItem;
            if (ucRank != null) ucRank.SelectedRank = drvRank["FLDRANK"].ToString();

            ImageButton anl = (ImageButton)e.Row.FindControl("cmdEquivalentRank");
            Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                anl.Attributes.Add("onclick", "javascript:parent.Openpopup('MoreInfo', '', 'RegistersManningEquivalentRank.aspx?manningscalekey=" + lblDTKey.Text + "'); return false;");
            }
            if (lblDTKey != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl("imgGroupList");
                img.Attributes.Add("onclick", "showMoreInformation(ev, 'RegistersManningEquivalentRank.aspx?readonly=1&manningscalekey=" + lblDTKey.Text + "')");
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvManningScale.SelectedIndex = -1;
        gvManningScale.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvManningScale.SelectedIndex = -1;
        gvManningScale.EditIndex = -1;

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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvManningScale.SelectedIndex = -1;
        gvManningScale.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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
            return true;

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

    private void InsertManningScale(
        string vesselid
        , string rank
        , string ownerscale
        , string safescale
        , string CBAScale
        , string contractperiod
        , string remarks
        , string contractperioddays
        , string revisionid)
    {
        if (!IsValidManningScale(rank, ownerscale, safescale, CBAScale, contractperioddays))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersVesselManningScale.InsertManningScale(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Convert.ToInt16(vesselid)
            , Convert.ToInt16(rank)
            , null                                  // Nationality
            , Convert.ToInt16(ownerscale)
            , Convert.ToInt16(safescale)
            , Convert.ToInt16(CBAScale)
            , General.GetNullableInteger(contractperiod)
            , remarks
            , General.GetNullableInteger(contractperioddays)
            , General.GetNullableGuid(revisionid));
    }

    private void UpdateManningScale(
        string vesselid
        , string manningscaleid
        , string rank
        , string ownerscale
        , string safescale
        , string CBAScale
        , string contractperiod
        , string remarks
        , string contractperioddays)
    {

        if (!IsValidManningScale(rank, ownerscale, safescale, CBAScale, contractperioddays))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersVesselManningScale.UpdateManningScale(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Convert.ToInt16(manningscaleid)
            , Convert.ToInt16(vesselid), Convert.ToInt16(rank)
            , null                                 // Nationality
            , Convert.ToInt16(ownerscale)
            , Convert.ToInt16(safescale)
            , Convert.ToInt16(CBAScale)
            , General.GetNullableInteger(contractperiod)
            , remarks
            , General.GetNullableInteger(contractperioddays));

        ucStatus.Text = "Manning Scale information updated";
    }

    private bool IsValidManningScale(string rank, string ownerscale, string safescale, string CBAscale, string contractperiod)
    {
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
            ucError.ErrorMessage = "Please select the revision to add manning scale.";

        if (!Int16.TryParse(rank, out resultInt))
            ucError.ErrorMessage = "Rank is required.";

        if (!Int16.TryParse(ownerscale, out resultInt))
            ucError.ErrorMessage = "Valid Owner Scale is required.";

        if (!Int16.TryParse(safescale, out resultInt))
            ucError.ErrorMessage = "Safe Scale is required.";

        if (!Int16.TryParse(CBAscale, out resultInt))
            ucError.ErrorMessage = "CBA Scale is required.";

        if (!Int16.TryParse(contractperiod, out resultInt))
            ucError.ErrorMessage = "Contract Period is required.";

        return (!ucError.IsError);
    }

    private void DeleteManningScale(int vesselid, int manningscaleid)
    {
        PhoenixRegistersVesselManningScale.DeleteManningScale(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, manningscaleid, vesselid);
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    // Manning Scale Revision

    protected void ShowExcelR()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEFFECTIVEDATE", "FLDREVISIONNO" };
        string[] alCaptions = { "Effective Date", "Revision No" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["RSORTEXPRESSION"] == null) ? null : (ViewState["RSORTEXPRESSION"].ToString());
        if (ViewState["RSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["RSORTDIRECTION"].ToString());

        if (ViewState["RROWCOUNT"] == null || Int32.Parse(ViewState["RROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["RROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersVesselManningScale.SearchManningScaleRevision(General.GetNullableInteger(Filter.CurrentVesselMasterFilter),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["RPAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount);

        General.ShowExcel("Manning Scale Revision", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuRegistersRevision_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelR();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindRevision()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEFFECTIVEDATE", "FLDREVISIONNO" };
        string[] alCaptions = { "Effective Date", "Revision No" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["RSORTEXPRESSION"] == null) ? null : (ViewState["RSORTEXPRESSION"].ToString());
        if (ViewState["RSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["RSORTDIRECTION"].ToString());

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));

        DataRow drVessel = dsVessel.Tables[0].Rows[0];
        ViewState["REQUIRESUPDATEYN"] = drVessel["FLDREQUIRESUPDATEYN"].ToString();
        txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();

        DataTable dt = PhoenixRegistersVesselManningScale.SearchManningScaleRevision(General.GetNullableInteger(Filter.CurrentVesselMasterFilter),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["RPAGENUMBER"].ToString()),
                            General.ShowRecords(null),
                            ref iRowCount,
                            ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvManningScaleRevision", "Manning Scale Revision", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvManningScaleRevision.DataSource = ds;
            gvManningScaleRevision.DataBind();

            if (ViewState["REVISIONID"] == null || ViewState["REVISIONID"].ToString() == "")
            {
                gvManningScaleRevision.SelectedIndex = 0;
                ViewState["REVISIONID"] = gvManningScaleRevision.DataKeys[0].Value.ToString();
            }

            SetRowSelection();
        }
        else
        {
            ShowNoRecordsFound(dt, gvManningScaleRevision);
            ViewState["REVISIONID"] = "";
        }

        ViewState["RROWCOUNT"] = iRowCount;
        ViewState["RTOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigatorR();

        BindData();
        SetPageNavigator();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["REVISIONID"] = gvManningScaleRevision.DataKeys[rowindex].Value.ToString();
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvManningScaleRevision.SelectedIndex = -1;
        for (int i = 0; i < gvManningScaleRevision.Rows.Count; i++)
        {
            if (gvManningScaleRevision.DataKeys[i].Value.ToString().Equals(ViewState["REVISIONID"].ToString()))
            {
                gvManningScaleRevision.SelectedIndex = i;
                break;
            }
        }
    }

    protected void gvManningScaleRevision_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADDREVISION"))
            {
                if (!IsValidData(((UserControlDate)_gridView.FooterRow.FindControl("ucEffectiveDateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVesselManningScale.InsertManningScaleRevision(int.Parse(Filter.CurrentVesselMasterFilter),
                    DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("ucEffectiveDateAdd")).Text));

                BindRevision();
                ((UserControlDate)_gridView.FooterRow.FindControl("ucEffectiveDateAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETEREVISION"))
            {
                ViewState["REVISIONID"] = "";
                Guid revisionid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixRegistersVesselManningScale.DeleteManningScaleRevision(revisionid);
                BindRevision();
            }
            else if (e.CommandName.ToUpper().Equals("SELECTREVISION"))
            {
                BindPageURL(nCurrentRow);
                SetRowSelection();
                BindData();
                SetPageNavigator();
            }
            else if (e.CommandName.ToUpper().Equals("EDITREVISION"))
            {
                _gridView.EditIndex = nCurrentRow;
                _gridView.SelectedIndex = nCurrentRow;
                BindRevision();
                ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucEffectiveDateEdit")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("CANCELREVISION"))
            {
                _gridView.EditIndex = -1;
                BindRevision();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATEREVISION"))
            {
                if (!IsValidData(
                    ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucEffectiveDateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid revisionid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

                PhoenixRegistersVesselManningScale.UpdateManningScaleRevision(revisionid
                    , int.Parse(Filter.CurrentVesselMasterFilter)
                    , DateTime.Parse(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucEffectiveDateEdit")).Text)
                    );

                _gridView.EditIndex = -1;
                BindRevision();
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                Guid revisionid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixRegistersVesselManningScale.CopyPreviousRevManningScale(revisionid);
                BindRevision();
            }
            SetPageNavigatorR();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvManningScaleRevision_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["RSORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["RSORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["RSORTDIRECTION"] == null || ViewState["RSORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton dc = (ImageButton)e.Row.FindControl("cmdCopy");
            if (dc != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, dc.CommandName))
                    dc.Visible = false;

                dc.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to copy manning scale from previous revision?')");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
        }
    }

    private bool IsValidData(string effectivedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(effectivedate) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        return (!ucError.IsError);
    }

    private void SetPageNavigatorR()
    {
        cmdPreviousR.Enabled = IsPreviousEnabledR();
        cmdNextR.Enabled = IsNextEnabledR();
        lblPagenumberR.Text = "Page " + ViewState["RPAGENUMBER"].ToString();
        lblPagesR.Text = " of " + ViewState["RTOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecordsR.Text = "(" + ViewState["RROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledR()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["RPAGENUMBER"];
        iTotalPageCount = (int)ViewState["RTOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabledR()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["RPAGENUMBER"];
        iTotalPageCount = (int)ViewState["RTOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void cmdGoR_Click(object sender, EventArgs e)
    {
        int result;
        gvManningScaleRevision.SelectedIndex = -1;
        gvManningScaleRevision.EditIndex = -1;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["RPAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["RTOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["RPAGENUMBER"] = ViewState["RTOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["RPAGENUMBER"] = 1;

            if ((int)ViewState["RPAGENUMBER"] == 0)
                ViewState["RPAGENUMBER"] = 1;

            txtnopage.Text = ViewState["RPAGENUMBER"].ToString();
        }
        BindRevision();
        SetPageNavigatorR();
    }

    protected void PagerButtonClickR(object sender, CommandEventArgs ce)
    {
        gvManningScaleRevision.SelectedIndex = -1;
        gvManningScaleRevision.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["RPAGENUMBER"] = (int)ViewState["RPAGENUMBER"] - 1;
        else
            ViewState["RPAGENUMBER"] = (int)ViewState["RPAGENUMBER"] + 1;

        ViewState["REVISIONID"] = "";

        BindRevision();
        SetPageNavigatorR();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindRevision();
        SetPageNavigatorR();
    }
}
