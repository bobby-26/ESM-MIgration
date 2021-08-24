using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Text;
public partial class Registers_RegistersVesselSurvey : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Registers/RegistersVesselSurvey.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvSurvey')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageButton("../Registers/RegistersVesselSurvey.aspx", "Filter", "search.png", "SEARCH");
                toolbar.AddImageButton("../Registers/RegistersVesselSurvey.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                toolbar.AddImageButton("../Registers/RegistersVesselSurvey.aspx", "Add Survey", "add.png", "ADD");
                MenuRegistersSurvey.AccessRights = this.ViewState;
                MenuRegistersSurvey.MenuList = toolbar.Show();
                BindSurveyType();
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
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }
    private void BindSurveyType()
    {
        try
        {
            ddlSurveyType.DataSource = PhoenixRegistersVesselSurvey.SurveyTypeList();
            ddlSurveyType.DataValueField = "FLDSURVEYTYPEID";
            ddlSurveyType.DataTextField = "FLDSURVEYTYPENAME";
            ddlSurveyType.DataBind();
            ddlSurveyType.Items.Insert(0, new ListItem("--Select--", string.Empty));
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

        string[] alColumns = { "FLDVESSELNAME", "FLDSURVEYNAME", "FLDSURVEYTYPENAME", "FLDFREQUENCY", "FLDCOMMENCEMENTDATE", "FLDWINDOWPERIOD", "FLDSCHEDULEDATE" };
        string[] alCaptions = { "Vessel", "Survey", "Survey Type", "Frequency", "Commencement Date", "Window period", "Schedule Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        
        ds = PhoenixRegistersVesselSurvey.VesselSurveySearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
               , ""
               , General.GetNullableInteger(ddlSurveyType.SelectedValue)
               , txtSurvey.Text
               , General.GetNullableDateTime(ucFromDate.Text)
               , General.GetNullableDateTime(ucToDate.Text)
               , sortexpression
               , sortdirection
               ,1
               ,iRowCount
               ,ref iRowCount
               ,ref iTotalPageCount
               ,General.GetNullableInteger(ddlStatus.SelectedValue));


        Response.AddHeader("Content-Disposition", "attachment; filename=Survey.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Survey</h3></td>");
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

    protected void MenuRegistersSurvey_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlSurveyType.SelectedValue = "";
                txtSurvey.Text = "";
                ucFromDate.Text = "";
                ucToDate.Text = "";
                ddlStatus.SelectedValue = "";
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("ADD"))
            {

                string surl = "javascript:parent.Openpopup('codehelp1','','RegistersVesselSurveyAdd.aspx?type=ADD&SurveyId=&VesselId=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "add", surl, true);
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

        string[] alColumns = { "FLDVESSELNAME", "FLDSURVEYNAME", "FLDSURVEYTYPENAME", "FLDFREQUENCY", "FLDCOMMENCEMENTDATE", "FLDWINDOWPERIOD","FLDSCHEDULEDATE" };
        string[] alCaptions = { "Vessel", "Survey", "Survey Type", "Frequency", "Commencement Date", "Window period","Schedule Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVesselSurvey.VesselSurveySearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
              , ""
              , General.GetNullableInteger(ddlSurveyType.SelectedValue)
              , txtSurvey.Text
              , General.GetNullableDateTime(ucFromDate.Text)
              , General.GetNullableDateTime(ucToDate.Text)
              , sortexpression, sortdirection,
               (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount
               , General.GetNullableInteger(ddlStatus.SelectedValue));

        General.SetPrintOptions("gvSurvey", "Survey", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSurvey.DataSource = ds;
            gvSurvey.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSurvey);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvSurvey_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvSurvey.EditIndex = -1;
        gvSurvey.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvSurvey_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        //{
        //    e.Row.TabIndex = -1;
        //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSurvey, "Edit$" + e.Row.RowIndex.ToString(), false);
        //}

        //SetKeyDownScroll(sender, e);
    }

    protected void gvSurvey_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string sVesselId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
                string sSurveyId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSurveyId")).Text;
                DeleteSurvey(sSurveyId, int.Parse(sVesselId));
            }
            else if (e.CommandName.ToUpper().Equals("INITIATESURVEY"))
            {
                string sVesselId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
                string sSurveyId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSurveyId")).Text;
                PhoenixRegistersVesselSurvey.InitiateSurvey(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Int16.Parse(sVesselId)
                    , General.GetNullableGuid(sSurveyId));
                ucStatus.Text = "Survey Initiated Sucessfully";
                ucStatus.Visible = true;
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
    protected void gvSurvey_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    protected void gvSurvey_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
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
            string sSurveyId = ((Label)e.Row.FindControl("lblSurveyId")).Text;
            string sVesselid = ((Label)e.Row.FindControl("lblVesselId")).Text;
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','RegistersVesselSurveyAdd.aspx?type=EDIT&SurveyId=" + sSurveyId + "&VesselId=" + sVesselid + "');");
            }
            ImageButton ib = (ImageButton)e.Row.FindControl("cmdInitiateSurvey");
            if (ib != null)
            {
                ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
            }
            Label lblSurveyId = (Label)e.Row.FindControl("lblSurveyId");
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            //HtmlImage img = (HtmlImage)e.Row.FindControl("imgCategoryList");
            //img.Attributes.Add("onclick", "showMoreInformation(ev, 'RegistersMoreInfoCertificateList.aspx?SurveyId=" + lblSurveyId.Text + "')");
            //ImageButton cmdCertificate = (ImageButton)e.Row.FindControl("cmdCertificate");
            //if (cmdCertificate != null)
            //{
            //    cmdCertificate.Visible = SessionUtil.CanAccess(this.ViewState, cmdCertificate.CommandName);
            //    cmdCertificate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','RegistersSurveyCertificatesMaping.aspx?type=EDIT&SurveyId=" + sSurveyId + "&VesselId=" + sVesselid + "');");
            //}
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
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvSurvey.EditIndex = -1;
        gvSurvey.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvSurvey.EditIndex = -1;
        gvSurvey.SelectedIndex = -1;
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvSurvey.SelectedIndex = -1;
        gvSurvey.EditIndex = -1;
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

    private void DeleteSurvey(string Surveyid, int VesselId)
    {
        PhoenixRegistersVesselSurvey.DeleteVesselSurvey(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , VesselId, General.GetNullableGuid(Surveyid));
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

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }
}
