using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewLicenceRequestStatusList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvLicenceRequestStatus.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {

            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewLicenceRequestStatusList.aspx?pid=" + Request.QueryString["pid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvLicenceRequestStatus')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("../Crew/CrewLicenceRequestStatusList.aspx?pid=" + Request.QueryString["pid"].ToString(), "Find", "search.png", "FIND");
            toolbar.AddImageLink("../Crew/CrewLicenceRequestStatus.aspx?pid=" + Request.QueryString["pid"].ToString(), "Add", "add.png", "ADDCRA");
			LicenceRequestStatus.AccessRights = this.ViewState;
            LicenceRequestStatus.MenuList = toolbar.Show();
            LicenceRequestStatus.SetTrigger(pnlLicenceRequestStatusEntry);

            toolbar = new PhoenixToolbar();

            toolbar.AddButton("List", "LIST");
            toolbar.AddButton("Status", "STATUS");
			MenuStatus.AccessRights = this.ViewState;
            MenuStatus.MenuList = toolbar.Show();

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }
        MenuStatus.SelectedMenuIndex = 0;
        BindData();
        SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDFLAGSTATEPROCESSDATE", "FLDCRANUMBER", "FLDCRAHANDEDOVERPO", "FLDDATEOFHANDOVER", "FLDDATEOFEXPIRY" };
        string[] alCaptions = { "Name", "Date of Flag State Process", "CRA No.", "CRA Handed over to", "Date of Handover", "Expiry Date of CRA" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceRequestStatusSearch(
            General.GetNullableGuid(Request.QueryString["pid"].ToString())
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=LicenceRequestStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Licence request Status</h3></td>");
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

        foreach (DataRow dr in dt.Rows)
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

    protected void Status_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("STATUS"))
        {
            if (ViewState["STATUSID"] != null)
            {
                Response.Redirect("CrewLicenceRequestStatus.aspx?pid=" + Request.QueryString["pid"].ToString() + "&licreqstatusId=" + ViewState["STATUSID"].ToString(), true);
            }
            else
            {
                ucError.ErrorMessage = "Please select one CRA from the list and then navigate to Status tab.";
                ucError.Visible = true;
                return;
            }
        }
    }

    protected void LicenceRequestStatus_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
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
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDFLAGSTATEPROCESSDATE", "FLDCRANUMBER", "FLDCRAHANDEDOVERPO", "FLDDATEOFHANDOVER", "FLDDATEOFEXPIRY" };
        string[] alCaptions = { "Name", "Date of Flag State Process", "CRA No.", "CRA Handed over to", "Date of Handover", "Expiry Date of CRA" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceRequestStatusSearch(
            General.GetNullableGuid(Request.QueryString["pid"].ToString())
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        DataSet ds = new DataSet();

        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvLicenceRequestStatus", "Licence Request Status", alCaptions, alColumns, ds);

        if (dt.Rows.Count > 0)
        {
            gvLicenceRequestStatus.DataSource = dt;
            gvLicenceRequestStatus.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvLicenceRequestStatus);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvLicenceRequestStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteLicenceRequestStatus(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblLicenceRequestStatusId")).Text));
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Label l = (Label)_gridView.Rows[nCurrentRow].FindControl("lblLicenceRequestStatusId");

                ViewState["STATUSID"] = l.Text;
            }
            else
            {
                _gridView.EditIndex = -1;
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

    protected void gvLicenceRequestStatus_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLicenceRequestStatus_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvLicenceRequestStatus_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                //Label l = (Label)e.Row.FindControl("lblLicenceRequestStatusId");

                //LinkButton lb = (LinkButton)e.Row.FindControl("lnkName");
                //lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewLicenceRequestStatus.aspx?pid=" + Request.QueryString["pid"].ToString() + "&licreqstatusId=" + l.Text + "');return false;");
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {

        }
    }

    protected void gvLicenceRequestStatus_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvLicenceRequestStatus.SelectedIndex = -1;
        gvLicenceRequestStatus.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvLicenceRequestStatus.SelectedIndex = -1;
        gvLicenceRequestStatus.EditIndex = -1;
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

    private void DeleteLicenceRequestStatus(Guid requeststatusid)
    {
        PhoenixCrewLicenceRequest.DeleteCrewLicenceStatus(requeststatusid);
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
}
