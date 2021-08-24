using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRACategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRACategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRACategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRACategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRACategory.AccessRights = this.ViewState;
            MenuRACategory.MenuList = toolbar.Show();

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
        string[] alColumns = { "FLDCODE", "FLDNAME" };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentCategory.RiskAssessmentCategorySearch(General.GetNullableString(txtCode.Text),
            General.GetNullableString(txtName.Text), sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=RACategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Type</h3></td>");
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

    protected void RACategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        string[] alColumns = { "FLDCODE", "FLDNAME" };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentCategory.RiskAssessmentCategorySearch(General.GetNullableString(txtCode.Text),
            General.GetNullableString(txtName.Text), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvRACategory", "Type", alCaptions, alColumns, ds);

        gvRACategory.DataSource = ds;
        gvRACategory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void Rebind()
    {
        gvRACategory.SelectedIndexes.Clear();
        gvRACategory.EditIndexes.Clear();
        gvRACategory.DataSource = null;
        gvRACategory.Rebind();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        LinkButton ib = (LinkButton)sender;
        Rebind();
    }

    protected void gvRACategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRACategory(((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertRACategory(
                    ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
            }
             if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidRACategory(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateRACategory(
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblCategoryIdEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text
                 );
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDesignation(Int32.Parse(((RadLabel)e.Item.FindControl("lblCategoryId")).Text));
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvRACategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        UpdateRACategory(
    //                Int32.Parse(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblCategoryIdEdit")).Text),
    //                 ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtCodeEdit")).Text,
    //                 ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text
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
    protected void gvRACategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
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

    private void InsertRACategory(string code, string name)
    {

        PhoenixInspectionRiskAssessmentCategory.InsertRiskAssessmentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              name, code);
    }

    private void UpdateRACategory(int categoryid, string code, string name)
    {
        if (!IsValidRACategory(name))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixInspectionRiskAssessmentCategory.UpdateRiskAssessmentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             categoryid, name, code);
        ucStatus.Text = "Information updated";
    }

    private bool IsValidRACategory(string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvRACategory;

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteDesignation(int categoryid)
    {
        PhoenixInspectionRiskAssessmentCategory.DeleteRiskAssessmentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, categoryid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRACategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
