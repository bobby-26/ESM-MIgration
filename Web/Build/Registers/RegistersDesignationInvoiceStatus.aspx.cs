using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDesignationInvoiceStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersDesignationInvoiceStatus.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('dgInvoicestatus')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersDesignationInvoiceStatus.AccessRights = this.ViewState;
            MenuRegistersDesignationInvoiceStatus.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                dgInvoicestatus.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDINVOICETYPENAME", "FLDINVOICESTATUSNAME", "FLDDESIGNATIONNAME" };
        string[] alCaptions = { "Invoice Type", "Invoice Status", "Designation" };

        ds = PhoenixRegistersDesignationInvoiceStatus.DesignationMappingList();

        Response.AddHeader("Content-Disposition", "attachment; filename=DesignationinvoiceStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Country Register</h3></td>");
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

    private void BindData()
    {

        int iRowCount = 0;
        DataSet ds = PhoenixRegistersDesignationInvoiceStatus.DesignationMappingList();
        string[] alColumns = { "FLDINVOICETYPENAME", "FLDINVOICESTATUSNAME", "FLDDESIGNATIONNAME" };
        string[] alCaptions = { "Invoice Type", "Invoice Status", "Designation" };

        iRowCount = ds.Tables[0].Rows.Count;
        General.SetPrintOptions("dgInvoicestatus", "Designation Invoice Status", alCaptions, alColumns, ds);

        dgInvoicestatus.DataSource = ds;
        dgInvoicestatus.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;


    }

    protected void dgInvoicestatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgInvoicestatus.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegistersDesignationInvoiceStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
    protected void Rebind()
    {
        dgInvoicestatus.SelectedIndexes.Clear();
        dgInvoicestatus.DataSource = null;
        dgInvoicestatus.Rebind();
    }
    protected void dgInvoicestatus_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                _gridView.MasterTableView.ClearEditItems();
            }
            if (e.CommandName == "EDIT")
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDesignationInvoiceStatus(
                ((UserControlHard)e.Item.FindControl("ddlInvoiceType")).SelectedHard,
                ((UserControlHard)e.Item.FindControl("ddlInvoiceStatus")).SelectedHard,
                ((UserControlDesignation)e.Item.FindControl("ddlDesignation")).SelectedDesignation,
                ((RadComboBox)e.Item.FindControl("ddlTypeAdd")).SelectedValue
                ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDesignationInvoiceStatus(
                int.Parse(((UserControlHard)e.Item.FindControl("ddlInvoiceType")).SelectedHard),
                int.Parse(((UserControlHard)e.Item.FindControl("ddlInvoiceStatus")).SelectedHard),
                int.Parse(((UserControlDesignation)e.Item.FindControl("ddlDesignation")).SelectedDesignation),
                int.Parse(((RadComboBox)e.Item.FindControl("ddlTypeAdd")).SelectedValue)
                );
                Rebind();
                ((UserControlHard)e.Item.FindControl("ddlInvoiceType")).Focus();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                deleteDesignationInvoiceStatus(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDesignationInvoiceId")).Text));
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                Rebind();
                return;

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgInvoicestatus_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }
            ImageButton add = (ImageButton)e.Item.FindControl("cmdAdd");
            if (add != null)
                add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        }

    }
    //protected void dgInvoicestatusr_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        //ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
    //        //if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
    //
    //        UserControlHard ucHard = (UserControlHard)e.Row.FindControl("ddlInvoiceTypeEdit");
    //        DataRowView drvHard = (DataRowView)e.Row.DataItem;
    //        if (ucHard != null)
    //        {
    //            ucHard.SelectedHard = drvHard["FLDINVOICETYPE"].ToString();
    //        }
    //
    //        UserControlHard ucHardStatus = (UserControlHard)e.Row.FindControl("ddlInvoiceStatusEdit");
    //        DataRowView drvHardStatus = (DataRowView)e.Row.DataItem;
    //        if (ucHardStatus != null)
    //        {
    //            ucHardStatus.SelectedHard = drvHardStatus["FLDINVOICESTATUS"].ToString();
    //        }
    //
    //        UserControlDesignation ucDesignation = (UserControlDesignation)e.Row.FindControl("ddlDesignationEdit");
    //        DataRowView drvDesignation = (DataRowView)e.Row.DataItem;
    //        if (ucDesignation != null)
    //        {
    //            ucDesignation.SelectedDesignation = drvDesignation["FLDDESIGNATIONID"].ToString();
    //        }
    //
    //        DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlTypeEdit");
    //        DataRowView drvType = (DataRowView)e.Row.DataItem;
    //        if (ddlType != null)
    //        {
    //            ddlType.SelectedValue = drvType["FLDTYPE"].ToString();
    //        }
    //    }
    //
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        UserControlHard ucHard = (UserControlHard)e.Row.FindControl("ddlInvoiceType");
    //
    //        if (ucHard != null)
    //        {
    //            ucHard.HardList = PhoenixRegistersHard.ListHard(1, 59);
    //            ucHard.DataBind();
    //        }
    //    }
    //}

    private void InsertDesignationInvoiceStatus(int invoicetype, int invoicestatus, int designationid, int type)
    {
        PhoenixRegistersDesignationInvoiceStatus.DesignationMappingInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            invoicetype, invoicestatus, designationid, type);
    }


    private void deleteDesignationInvoiceStatus(Guid? dtkey)
    {
        PhoenixRegistersDesignationInvoiceStatus.DesignationMappingDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            dtkey);
    }

    private bool IsValidDesignationInvoiceStatus(string invoicetype, string invoicestatus, string designationid, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        RadGrid _gridView = dgInvoicestatus;

        if (General.GetNullableInteger(invoicetype) == null)
        {
            ucError.ErrorMessage = "Invoice Type is required.";
        }
        if (General.GetNullableInteger(invoicestatus) == null)
        {
            ucError.ErrorMessage = "Invoice Status is required.";
        }
        if (General.GetNullableInteger(designationid) == null)
        {
            ucError.ErrorMessage = "Designation is required.";
        }
        if (General.GetNullableInteger(type) == null)
        {
            ucError.ErrorMessage = "Type is required.";
        }

        return (!ucError.IsError);
    }

}
