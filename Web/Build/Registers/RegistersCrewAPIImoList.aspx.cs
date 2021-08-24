using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegistersCrewAPIImoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewAPIImoList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvValidIMO')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewAPIImoList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewAPIImoList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            gvValidIMOTab.AccessRights = this.ViewState;
            gvValidIMOTab.MenuList = toolbar.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvValidIMO.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvValidIMOTab_TabStripCommand(object sender, EventArgs e)
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
                ViewState["PAGENUMBER"] = 1;
                txtIMONumber.Text = "";
                gvValidIMO.CurrentPageIndex = 0;
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

        string[] alColumns = { "FLDVESSELNAME", "FLDIMONUMBER", "FLDPRINCIPAL" , "FLDACTIVEYESNO" };
        string[] alCaptions = { "Vessel", "IMO Number", "Principal" , "Active" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCrewAPIImo.RegistersValidImoSearch(txtIMONumber.Text.Trim()
                    , sortexpression, sortdirection,
                    1,
                    gvValidIMO.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Service Vessel Mapping", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void Rebind()
    {
        gvValidIMO.SelectedIndexes.Clear();
        gvValidIMO.EditIndexes.Clear();
        gvValidIMO.DataSource = null;
        gvValidIMO.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvValidIMO_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvValidIMO.CurrentPageIndex + 1;
            BindData();
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

        string[] alColumns = { "FLDVESSELNAME", "FLDIMONUMBER", "FLDPRINCIPAL", "FLDACTIVEYESNO" };
        string[] alCaptions = { "Vessel", "IMO Number", "Principal", "Active" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
         ds = PhoenixRegistersCrewAPIImo.RegistersValidImoSearch(txtIMONumber.Text.Trim()
                       , sortexpression, sortdirection,
                       int.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvValidIMO.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);

        General.SetPrintOptions("gvValidIMO", "Service Vessel Mapping", alCaptions, alColumns, ds);

        gvValidIMO.DataSource = ds;
        gvValidIMO.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvValidIMO_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                PhoenixRegistersCrewAPIImo.RegistersValidImoInsert(
                    General.GetNullableInteger(((UserControlVesselCommon)e.Item.FindControl("ucVessel")).SelectedVessel)
                     , (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true) ? 1 : 0
                     );

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

    protected void gvValidIMO_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            PhoenixRegistersCrewAPIImo.RegistersValidImoUpdate(
                            int.Parse(((RadLabel)e.Item.FindControl("lblIDEdit")).Text)                          
                            ,((RadTextBox)e.Item.FindControl("lblIMONumberEdit")).Text                        
                            , (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0
                            );
            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvValidIMO_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            PhoenixRegistersCrewAPIImo.RegistersValidImoDelete(int.Parse(((RadLabel)e.Item.FindControl("lblID")).Text));

            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvValidIMO_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.IsInEditMode)
        {

        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            

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

    protected void gvValidIMO_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace("ASC", "").Replace("DESC", "");
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
