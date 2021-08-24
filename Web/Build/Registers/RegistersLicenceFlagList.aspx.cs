using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Registers_RegistersLicenceFlagList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersLicenceFlagList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFlagList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            //toolbar.AddImageButton("../Registers/RegistersLicenceFlagList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvFlagList')", "Print Grid", "icon_print.png", "PRINT");
            SubMenu.AccessRights = this.ViewState;
            SubMenu.MenuList = toolbar.Show();
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Licence", "LICENCE");
            toolbarmain.AddButton("Flag List", "FLAGLIST");
            toolbarmain.AddButton("Cost of Licence", "COSTOFLICENCE");
            MainMenu.AccessRights = this.ViewState;
            MainMenu.MenuList = toolbarmain.Show();
            MainMenu.SelectedMenuIndex = 1;
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SELECTEDINDEX"] = 0;
                gvFlagList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDROWNUMBER", "FLDFLAGNAME", "FLDCONSULATENAME" };
        string[] alCaptions = { "S.No", "Flag", "Consulate" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersLicenceCost.GetFlagList(1
                , gvFlagList.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

        General.ShowExcel("Flag List", ds.Tables[0], alColumns, alCaptions, null, "");
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDFLAGNAME", "FLDCONSULATENAME" };
        string[] alCaptions = { "S.No", "Flag", "Consulate" };

        DataSet ds = PhoenixRegistersLicenceCost.GetFlagList(Int32.Parse(ViewState["PAGENUMBER"].ToString())
               , gvFlagList.PageSize
               , ref iRowCount
               , ref iTotalPageCount);

        General.SetPrintOptions("gvFlagList", "Flag List", alCaptions, alColumns, ds);

        //gvFlagList.SelectedIndex = int.Parse(ViewState["SELECTEDINDEX"].ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFlagList.DataSource = ds;
            gvFlagList.VirtualItemCount = iRowCount;
        }
        else
        {
            gvFlagList.DataSource = "";
        }        
    }

    protected void MainMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            int RowIndex = string.IsNullOrEmpty(ViewState["SELECTEDINDEX"].ToString()) ? 0 : int.Parse(ViewState["SELECTEDINDEX"].ToString());

            RadLabel lblFlagId = (RadLabel)gvFlagList.MasterTableView.Items[RowIndex].FindControl("lblFlagId");
            RadLabel lblConsulateId = (RadLabel)gvFlagList.MasterTableView.Items[RowIndex].FindControl("lblConsulateId");

            if (CommandName.ToUpper().Equals("FLAGLIST"))
            {
                MainMenu.SelectedMenuIndex = 1;
                return;
            }
            else if (CommandName.ToUpper().Equals("LICENCE"))
            {
                Response.Redirect("../Registers/RegistersDocumentLicence.aspx");
            }
            else if (CommandName.ToUpper().Equals("COSTOFLICENCE"))
            {
                string sScript = "../Registers/RegistersLicenceCost.aspx?FLAGID=" + (lblFlagId != null ? lblFlagId.Text.Trim() : "")
                    + "&CONSULATEID=" + (lblConsulateId != null ? lblConsulateId.Text.Trim() : "");
                Response.Redirect(sScript);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SubMenu_TabStripCommand(object sender, EventArgs e)
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

    //protected void gvFlagList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
      
    //}

    //protected void gvFlagList_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            LinkButton cb = (LinkButton)e.Row.FindControl("lnkConsulate");
    //            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvFlagList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    try
    //    {
    //        gvFlagList.SelectedIndex = e.NewSelectedIndex;
    //        ViewState["SELECTEDINDEX"] = e.NewSelectedIndex;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    

    protected void gvFlagList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel lblFlagId = (RadLabel)e.Item.FindControl("lblFlagId");
                RadLabel lblConsulateId = (RadLabel)e.Item.FindControl("lblConsulateId");

                ViewState["SELECTEDINDEX"] = e.Item.RowIndex;

                string sScript = "../Registers/RegistersLicenceCost.aspx?FLAGID=" + (lblFlagId != null ? lblFlagId.Text.Trim() : "")
                + "&CONSULATEID=" + (lblConsulateId != null ? lblConsulateId.Text.Trim() : "");

                Response.Redirect(sScript);
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

    protected void gvFlagList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFlagList.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvFlagList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton cb = (LinkButton)e.Item.FindControl("lnkConsulate");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
