using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionRASeverity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRASeverity.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRASeverity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRASeverity.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRASeverity.AccessRights = this.ViewState;
            MenuRASeverity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
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
        string[] alColumns = { "FLDSEVERITY", "FLDSCORE" };
        string[] alCaptions = { "Severity", "Score" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentSeverity.RiskAssessmentSeveritySearch(General.GetNullableString(txtName.Text),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=RASeverity.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Severity</h3></td>");
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

    protected void RASeverity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvRASeverity.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
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
        string[] alColumns = { "FLDSEVERITY", "FLDSCORE" };
        string[] alCaptions = { "Severity", "Score" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentSeverity.RiskAssessmentSeveritySearch(General.GetNullableString(txtName.Text),
            sortexpression, sortdirection,
            gvRASeverity.CurrentPageIndex + 1,
            gvRASeverity.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        General.SetPrintOptions("gvRASeverity", "Severity ", alCaptions, alColumns, ds);

        gvRASeverity.DataSource = ds;
        gvRASeverity.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvRASeverity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidRASeverity(((RadTextBox)e.Item.FindControl("txtSeverityAdd")).Text,
                            ((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertRASeverity(
                        ((RadTextBox)e.Item.FindControl("txtSeverityAdd")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("ucScoreAdd")).Text
                    );
                    ((RadTextBox)e.Item.FindControl("txtSeverityAdd")).Focus();
                    BindData();
                    gvRASeverity.Rebind();
                }
            }

            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {

                    if (!IsValidRASeverity(((RadTextBox)e.Item.FindControl("txtSeverityEdit")).Text,
                       ((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateRASeverity(
                            Int32.Parse(((RadLabel)e.Item.FindControl("lblSeverityIdEdit")).Text),
                             ((RadTextBox)e.Item.FindControl("txtSeverityEdit")).Text,
                             ((UserControlMaskNumber)e.Item.FindControl("ucScoreEdit")).Text
                         );
                    gvRASeverity.Rebind();
                }

                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteDesignation(Int32.Parse(((RadLabel)e.Item.FindControl("lblSeverityId")).Text));
                    gvRASeverity.Rebind();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvRASeverity_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        if (!IsValidRASeverity(((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtSeverityEdit")).Text,
    //                    ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucScoreEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        UpdateRASeverity(
    //                Int32.Parse(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblSeverityIdEdit")).Text),
    //                 ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtSeverityEdit")).Text,
    //                 ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucScoreEdit")).Text
    //             );
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvRASeverity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            //UserControlMaskNumber ucSeveritytype = (UserControlRAMiscellaneous)e.Row.FindControl("ucSeverityEdit");
            //DataRowView drv = (DataRowView)e.Row.DataItem;
            //if (ucSeveritytype != null) ucSeveritytype.SelectedMiscellaneous = drv["FLDSeverityTYPEID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    private void InsertRASeverity(string severity, string score)
    {

        PhoenixInspectionRiskAssessmentSeverity.InsertRiskAssessmentSeverity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              severity, Convert.ToInt32(score));
    }

    private void UpdateRASeverity(int Severityid, string severity, string score)
    {

        PhoenixInspectionRiskAssessmentSeverity.UpdateRiskAssessmentSeverity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             Severityid, severity, Convert.ToInt32(score));
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRASeverity(string severity, string score)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //    RadGrid _gridView = gvRASeverity;

        if (severity.Trim().Equals(""))
            ucError.ErrorMessage = "Severity is required.";

        if (General.GetNullableInteger(score) == null)
            ucError.ErrorMessage = "Score is required.";
        return (!ucError.IsError);
    }

    private void DeleteDesignation(int Severityid)
    {
        PhoenixInspectionRiskAssessmentSeverity.DeleteRiskAssessmentSeverity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Severityid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRASeverity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRASeverity_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
