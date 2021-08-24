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

public partial class RegistersState: PhoenixBasePage
{
   protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersState.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvState')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersState.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersState.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersState.AccessRights = this.ViewState;
            MenuRegistersState.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvState.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDSTATENAME" ,"FLDACTIVE"};
        string[] alCaptions = { "State Name","Active Y/N"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersState.StateSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(ddlcountrylist.SelectedCountry), txtSearch.Text, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=State.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>State</h3></td>");
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


    protected void RegistersState_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlcountrylist.SelectedCountry = "";
                txtSearch.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        gvState.SelectedIndexes.Clear();
        gvState.EditIndexes.Clear();
        gvState.DataSource = null;
        gvState.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSTATENAME", "FLDACTIVEYN" };
        string[] alCaptions = { "State Name", "Active Y/N" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersState.StateSearch(0, General.GetNullableInteger(ddlcountrylist.SelectedCountry), txtSearch.Text, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvState.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvState", "State", alCaptions, alColumns, ds);

        gvState.DataSource = ds;
        gvState.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvState_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidState(((RadTextBox)e.Item.FindControl("txtStateNameAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertState(General.GetNullableInteger(ddlcountrylist.SelectedCountry),
                    ((RadTextBox)e.Item.FindControl("txtStateNameAdd")).Text,
                        (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0);
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtStateNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidState(((RadTextBox)e.Item.FindControl("txtStateNameEdit")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                    UpdateState(
                         Int32.Parse(((RadLabel)e.Item.FindControl("lblStateCodeEdit")).Text),
                        General.GetNullableInteger(ddlcountrylist.SelectedCountry),
                         ((RadTextBox)e.Item.FindControl("txtStateNameEdit")).Text,
                         (((RadCheckBox)e.Item.FindControl("chkActiveYNedit")).Checked.Equals(true)) ? 1 : 0);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteState(Int32.Parse(((RadLabel)e.Item.FindControl("lblStateCode")).Text));
                Rebind();
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
    protected void gvState_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
        }

    }
    private void InsertState(int? countrycode, string statename,int active)
    {
        PhoenixRegistersState.InsertState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, countrycode, statename,active);
    }

    private void UpdateState(int statecode, int? countrycode, string statename, int active)
    {
        PhoenixRegistersState.UpdateState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, statecode, countrycode, statename, active);
        ucStatus.Text = "State information updated";
    }

    private bool IsValidStateUpdate(string statename)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvState;

        if (statename.Trim().Equals(""))
            ucError.ErrorMessage = "State Name is required.";

        return (!ucError.IsError);
    }

    private bool IsValidState(string statename)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        RadGrid _gridView = gvState;

        if (statename.Trim().Equals(""))
            ucError.ErrorMessage = "State Name is required.";

        if (ddlcountrylist.SelectedCountry == "" || !Int16.TryParse(ddlcountrylist.SelectedCountry, out result))
           ucError.ErrorMessage = "First select a country";


        return (!ucError.IsError);
    }

    private void DeleteState(int statecode)
    {
        PhoenixRegistersState.DeleteState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, statecode);
    }
    protected void gvState_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvState_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvState.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

