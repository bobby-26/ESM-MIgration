using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.Profile;

public partial class RegistersCertificateEndorsements : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersCertificateEndorsements.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar.AddImageLink("javascript:CallPrint('gvCertificateEndorsement')", "Print Grid", "icon_print.png", "PRINT");
            MenuCertificateEndorsements.AccessRights = this.ViewState;
            MenuCertificateEndorsements.MenuList = toolbar.Show();

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

        string[] alColumns = { "FLDENDORSEMENTNAME", "FLDVALIDITYINMONTHS" };
        string[] alCaptions = { "Endorsement", "Validity (in months)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCertificateEndorsements.CertificateEndorsementSearch(sortexpression,
                                                                    sortdirection,
                                                                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                    iRowCount,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselCertificateEndorsements.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Certificate Endorsements</h3></td>");
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

    protected void MenuCertificateEndorsements_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDENDORSEMENTNAME", "FLDVALIDITYINMONTHS" };
        string[] alCaptions = { "Endorsement", "Validity (in months)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixRegistersCertificateEndorsements.CertificateEndorsementSearch(sortexpression,
                                                                    sortdirection,
                                                                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                    General.ShowRecords(null),
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);

        General.SetPrintOptions("gvCertificateEndorsement", "Vessel Certificate Endorsements", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvCertificateEndorsement.DataSource = ds;
            gvCertificateEndorsement.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCertificateEndorsement);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvCertificateEndorsement_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    private bool IsValidEndorsement(string endorsement, string validity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(endorsement) == null)
            ucError.ErrorMessage = "Endorsement is required.";

        if (string.IsNullOrEmpty(validity))
            ucError.ErrorMessage = "Validity (in months) is required.";

        return (!ucError.IsError);
    }

    protected void gvCertificateEndorsement_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvCertificateEndorsement.EditIndex = -1;
        gvCertificateEndorsement.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvCertificateEndorsement_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidEndorsement(((UserControlHard)_gridView.FooterRow.FindControl("ucEndorsementAdd")).SelectedHard,
                                ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtValidityAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersCertificateEndorsements.InsertCertificateEndorsement(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((UserControlHard)_gridView.FooterRow.FindControl("ucEndorsementAdd")).SelectedHard),
                    int.Parse(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtValidityAdd")).Text));

                ((UserControlHard)_gridView.FooterRow.FindControl("ucEndorsementAdd")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidEndorsement(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucEndorsementEdit")).SelectedHard,
                                ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtValidityEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersCertificateEndorsements.UpdateCertificateEndorsement(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEndorsementIdEdit")).Text)
                    , int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucEndorsementEdit")).SelectedHard)
                    , int.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtValidityEdit")).Text));

                _gridView.EditIndex = -1;
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersCertificateEndorsements.DeleteCertificateEndorsement(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEndorsementId")).Text));

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

    protected void gvCertificateEndorsement_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }

            UserControlHard ucEndorsementEdit = (UserControlHard)e.Row.FindControl("ucEndorsementEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ucEndorsementEdit != null)
            {
                ucEndorsementEdit.SelectedHard = drv["FLDENDORSEMENT"].ToString();
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
    }

    protected void gvCertificateEndorsement_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvCertificateEndorsement_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvCertificateEndorsement_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

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
        gvCertificateEndorsement.SelectedIndex = -1;
        gvCertificateEndorsement.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvCertificateEndorsement.SelectedIndex = -1;
        gvCertificateEndorsement.EditIndex = -1;
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
        gvCertificateEndorsement.SelectedIndex = -1;
        gvCertificateEndorsement.EditIndex = -1;
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
