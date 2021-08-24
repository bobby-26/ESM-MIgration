using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class RegistersUnit : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersUnit.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersUnit.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersUnit.AccessRights = this.ViewState;
            MenuRegistersUnit.MenuList = toolbar.Show();

             
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRegistersUnit_TabStripCommand(object sender, EventArgs e) 
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.CurrentPageIndex = 0;
                Rebind();
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
        string[] alColumns = { "FLDUNITNAME", "FLDACTIVE" };
        string[] alCaptions = { "Name", "Active Y/N" };
        string sortexpression;
        int? sortdirection = null;

        RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersUnit.UnitSearch(0, txtSearch.Text, null, sortexpression, sortdirection,
            RadGrid1.CurrentPageIndex + 1,
            RadGrid1.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Unit.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Unit</h3></td>");
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
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridFooterItem item = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (e.CommandName.ToUpper().Equals("ADD"))
            {             
                if (!IsValidUnit(((RadTextBox)item.FindControl("txtUnitNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertUnit(
                    ((RadTextBox)item.FindControl("txtUnitNameAdd")).Text,
                    (((CheckBox)item.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0
                );
                ucStatus.Text = "Information Added";
                Rebind();
            }

            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            if (e.CommandName == "InitInsert")
            {
                RadGrid1.EditIndexes.Clear();
            }
            if (e.CommandName == RadGrid.EditCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string UnitCode = ((RadLabel)e.Item.FindControl("lblUnitCodeEdit")).Text;
                if (UnitCode == string.Empty || UnitCode == "")
                {
                    var editableItem = ((GridEditableItem)e.Item);                 
                }
                else
                {
                    string unitid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDUNITID"].ToString();

                    if (!IsValidUnit(((RadTextBox)eeditedItem.FindControl("txtUnitNameEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateUnit(
                         Int32.Parse(unitid),
                         ((RadTextBox)eeditedItem.FindControl("txtUnitNameEdit")).Text,
                         (((CheckBox)eeditedItem.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                     );
                    ucStatus.Text = "Unit information updated";
                    Rebind();
                }
            }
            if (e.CommandName == "Page")
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
    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : RadGrid1.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        RadGrid1.SelectedIndexes.Clear();
        RadGrid1.EditIndexes.Clear();
        RadGrid1.DataSource = null;
        RadGrid1.Rebind();
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDUNITNAME", "FLDACTIVE" };
            string[] alCaptions = { "Name", "Active Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixRegistersUnit.UnitSearch(1, txtSearch.Text, null, sortexpression, sortdirection,
                RadGrid1.CurrentPageIndex + 1,
                RadGrid1.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("RadGrid1", "Unit", alCaptions, alColumns, ds);

            RadGrid1.DataSource = ds;
            RadGrid1.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
    }
    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            ViewState["UNITID"] = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDUNITID"].ToString(); 
            RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
            return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {

    }
    protected void RadGrid1_SortCommand(object sender, GridSortCommandEventArgs e)
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
    private void InsertUnit(string Unitname, int isactive)
    {

        PhoenixRegistersUnit.InsertUnit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Unitname, isactive);
    }

    private void UpdateUnit(int unitid, string Unitname, int isactive)
    {

        PhoenixRegistersUnit.UpdateUnit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, unitid, Unitname, isactive);
    }

    private bool IsValidUnit(string Unitname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Unitname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        return (!ucError.IsError);
    }

    private void DeleteUnit(int Unitcode)
    {
        PhoenixRegistersUnit.DeleteUnit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Unitcode);
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteUnit(Int32.Parse(ViewState["UNITID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
