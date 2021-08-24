using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Data;
public partial class RegistersSFITypes : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddImageButton("../Registers/RegistersSFITypes.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSFIType')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersSFITypes.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersSFITypes.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuSFIType.AccessRights = this.ViewState;
            MenuSFIType.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSFIType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ddlType.HardTypeCode = ((int)PhoenixHardTypeCode.SFITYPE).ToString();
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

        string[] alColumns = { "FLDCODE", "FLDCATEGORY" };
        string[] alCaptions = { "Code", "Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegisterSFIType.SearchSFIType(
                    General.GetNullableInteger(ddlType.SelectedHard)
                    , sortexpression
                    , sortdirection
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvSFIType.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

        General.SetPrintOptions("gvSFIType", "SFI Types", alCaptions, alColumns, ds);

        gvSFIType.DataSource = ds;
        gvSFIType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void gvSFIType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSFIType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSFIType_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidconfiguration(((RadTextBox)e.Item.FindControl("txtCode")).Text,
                   ((RadTextBox)e.Item.FindControl("txtCategoryAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegisterSFIType.InsertSFIType(
                    int.Parse(ddlType.SelectedHard), 
                    ((RadTextBox)e.Item.FindControl("txtCode")).Text.Trim(),
                    ((RadTextBox)e.Item.FindControl("txtCategoryAdd")).Text.Trim()
                    );
                Rebind();
                ucStatus.Text = "Information Added";
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidconfiguration(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegisterSFIType.UpdateSFIType(
                    int.Parse(((RadLabel)e.Item.FindControl("lblCategoryEditId")).Text),
                    int.Parse(ddlType.SelectedHard),
                    ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text.Trim(),
                    ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text.Trim()
                     );
                Rebind();
                ucStatus.Text = "Information Updated";
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CATEGORYID"] = ((RadLabel)e.Item.FindControl("lblCategoryId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
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
    private bool IsValidconfiguration(string Code, string Category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvSFIType;

        if (ddlType.SelectedHard.Equals("Dummy") || ddlType.SelectedHard.Equals(""))
            ucError.ErrorMessage = "Type is required.";

        if (Code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Category.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }
    //private bool IsValidType()
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";

    //    RadGrid _gridView = gvSFIType;

    //    if (ddlType.SelectedHard.Equals("Dummy") || ddlType.SelectedHard.Equals(""))
    //        ucError.ErrorMessage = "Type is required.";

    //    return (!ucError.IsError);
    //}
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegisterSFIType.DeleteSFIType(Int32.Parse(ViewState["CATEGORYID"].ToString()));
            Rebind();
            ucStatus.Text = "Information deleted";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSFIType_TabStripCommand(object sender, EventArgs e)
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

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlType.SelectedHard = "";
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDCATEGORY"};
        string[] alCaptions = { "Code", "Category"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterSFIType.SearchSFIType(
                    General.GetNullableInteger(ddlType.SelectedHard)
                    , sortexpression
                    , sortdirection
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SFI Types.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>SFI Types</h3></td>");
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
    protected void Rebind()
    {
        gvSFIType.SelectedIndexes.Clear();
        gvSFIType.EditIndexes.Clear();
        gvSFIType.DataSource = null;
        gvSFIType.Rebind();
    }

    protected void ddlType_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvSFIType.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvSFIType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancle = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancle != null) cancle.Visible = SessionUtil.CanAccess(this.ViewState, cancle.CommandName);

        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvSFIType_SortCommand(object sender, GridSortCommandEventArgs e)
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