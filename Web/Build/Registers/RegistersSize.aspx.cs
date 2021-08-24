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

public partial class Registers_RegistersSize : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersSize.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersSize.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuRegistersSize.AccessRights = this.ViewState;
            MenuRegistersSize.MenuList = toolbar.Show();

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

    protected void MenuRegistersSize_TabStripCommand(object sender, EventArgs e)
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
                RadGrid1.Rebind();
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
        string[] alColumns = { "FLDSIZENAME", "FLDDESCRIPTION", "FLDACTIVE" };
        string[] alCaptions = { "Size Name", "Description", "Active Y/N" };
        string sortexpression;
        int? sortdirection = null;


        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersSize.SizeSearch(txtSearch.Text, sortexpression, sortdirection,
            RadGrid1.CurrentPageIndex + 1,
            RadGrid1.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Size.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Size</h3></td>");
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

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSIZENAME", "FLDDESCRIPTION", "FLDACTIVE" };
            string[] alCaptions = { "Size Name", "Description", "Active Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixRegistersSize.SizeSearch(txtSearch.Text, sortexpression, sortdirection,
                RadGrid1.CurrentPageIndex + 1,
                RadGrid1.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("RadGrid1", "Size", alCaptions, alColumns, ds);

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
        try
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

            if ((e.Item is GridEditableItem) && (e.Item.IsInEditMode) && (!e.Item.OwnerTableView.IsItemInserted))
            {
                if (eb != null)
                {
                    TableCell SizeName = (TableCell)e.Item.FindControl("SizeName");
                    SizeName.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //private void BindData()
    //{
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;
    //    string[] alColumns = { "FLDSIZENAME","FLDDESCRIPTION", "FLDACTIVE" };
    //    string[] alCaptions = { "Size Name","Description", "Active Y/N" };
    //    string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
    //    int? sortdirection = null;
    //    if (ViewState["SORTDIRECTION"] != null)
    //        sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


    //    DataSet ds = PhoenixRegistersSize.SizeSearch(txtSearch.Text,sortexpression, sortdirection,
    //        (int)ViewState["PAGENUMBER"],
    //        General.ShowRecords(null),
    //        ref iRowCount,
    //        ref iTotalPageCount);
    //    General.SetPrintOptions("gvSize", "Size", alCaptions, alColumns, ds);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {

    //        gvSize.DataSource = ds;
    //        gvSize.DataBind();
    //    }
    //    else
    //    {

    //        DataTable dt = ds.Tables[0];
    //        ShowNoRecordsFound(dt, gvSize);
    //    }

    //    ViewState["ROWCOUNT"] = iRowCount;
    //    ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    //}

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];

                if (!IsValidSize(((RadTextBox)item.FindControl("txtSizeNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSize(
                    ((RadTextBox)item.FindControl("txtSizeNameAdd")).Text,
                    ((RadTextBox)item.FindControl("txtSizeDescriptionAdd")).Text,
                    (((CheckBox)item.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0
                );

                ucStatus.Text = "Information Added";
                RadGrid1.Rebind();
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
                string lblSizeCodeEdit = ((RadLabel)e.Item.FindControl("lblSizeCodeEdit")).Text;
                if (lblSizeCodeEdit == string.Empty || lblSizeCodeEdit == "")
                {
                    //if (!IsValidSize(((RadTextBox)eeditedItem.FindControl("txtSizeNameEdit")).Text))
                    //{
                    //    ucError.Visible = true;
                    //    return;
                    //}
                    //InsertSize(
                    //    ((RadTextBox)eeditedItem.FindControl("txtSizeNameEdit")).Text,
                    //    ((RadTextBox)eeditedItem.FindControl("txtSizeDescriptionEdit")).Text,
                    //    (((CheckBox)eeditedItem.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                    //);

                    //ucStatus.Text = "Information Added";
                    //RadGrid1.Rebind();
                }
                else
                {
                    string sizeid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDSIZEID"].ToString();
                    UpdateSize(
                             Int32.Parse(sizeid),
                              ((RadTextBox)eeditedItem.FindControl("txtSizeNameEdit")).Text,
                              ((RadTextBox)eeditedItem.FindControl("txtSizeDescriptionEdit")).Text,
                             (((CheckBox)eeditedItem.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0

                         );
                    ucStatus.Text = "Size information updated";
                    RadGrid1.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        var editableItem = ((GridEditableItem)e.Item);
    //        if (!IsValidSize(((RadTextBox)editableItem.FindControl("txtSizeNameEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        InsertSize(
    //            ((RadTextBox)editableItem.FindControl("txtSizeNameEdit")).Text,
    //            ((RadTextBox)editableItem.FindControl("txtSizeDescriptionEdit")).Text,
    //            (((CheckBox)editableItem.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
    //        );

    //        ucStatus.Text = "Information Added";
    //        RadGrid1.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridEditableItem eeditedItem = e.Item as GridEditableItem;
    //        //RadGrid _gridView = (RadGrid)sender;
    //        int nCurrentRow = e.Item.RowIndex;

    //        string sizeid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDSIZEID"].ToString();
    //        UpdateSize(
    //                 Int32.Parse(sizeid),
    //                  ((RadTextBox)eeditedItem.FindControl("txtSizeNameEdit")).Text,
    //                  ((RadTextBox)eeditedItem.FindControl("txtSizeDescriptionEdit")).Text,
    //                 (((CheckBox)eeditedItem.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0

    //             );
    //        ucStatus.Text = "Size information updated";
    //        RadGrid1.Rebind();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            ViewState["SIZEID"] = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDSIZEID"].ToString();
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

    }
    private void InsertSize(string Sizename, string description, int isactive)
    {

        PhoenixRegistersSize.InsertSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Sizename, description, isactive);
    }

    private void UpdateSize(int Sizeid, string Sizename, string description, int isactive)
    {

        PhoenixRegistersSize.UpdateSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Sizeid, Sizename, description, isactive);
    }

    private bool IsValidSize(string Sizename)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Sizename.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        return (!ucError.IsError);
    }

    private void DeleteSize(int Sizecode)
    {
        PhoenixRegistersSize.DeleteSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Sizecode);
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteSize(Int32.Parse(ViewState["SIZEID"].ToString()));
            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
