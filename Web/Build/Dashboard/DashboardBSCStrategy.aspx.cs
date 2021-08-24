using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardBSCStrategy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardBSCStrategy.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvBSSP')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar kpitab = new PhoenixToolbar();
        kpitab.AddButton("Strategic Perspectives", "Toggle1", ToolBarDirection.Left);
        Tabbss.MenuList = kpitab.Show();

        Tabbss.SelectedMenuIndex = 0;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvBSSP.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {

    }

    protected void Tabbss_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("TOGGLE1"))
            {

                Response.Redirect("../Dashboard/DashboardBSCStrategy.aspx");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void TabstripMenu_TabStripCommand(object sender, EventArgs e)
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

    protected void gvBSSP_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixDashboardBSSP.BSSPSearch(gvBSSP.CurrentPageIndex + 1,
                                                gvBSSP.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvBSSP.DataSource = dt;
        gvBSSP.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDBSSPID", "FLDBSSHORTCODE",  "FLDDESCRIPTION" };
        string[] alCaptions = { "ID", "Shortcode",  "Description" };
        General.SetPrintOptions("gvBSSP", "Strategic Perspectives", alCaptions, alColumns, ds);

    }

    protected void gvBSSP_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvBSSP_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem fi = e.Item as GridFooterItem;
                RadTextBox spidentry = (RadTextBox)fi.FindControl("radtbbsspidentry");
                RadTextBox spcodeentry = (RadTextBox)fi.FindControl("radtbbsspcodeentry");
                RadTextBox spdescriptionentry = (RadTextBox)fi.FindControl("radtbbsspdescriptionentry");

              
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
               
                string   spid = General.GetNullableString(spidentry.Text);
                string spicode = General.GetNullableString(spcodeentry.Text);
                string spidescription = General.GetNullableString(spdescriptionentry.Text);
                

                if (!IsValidBSSP(spid, spicode, spidescription))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDashboardBSSP.BSSPInsert(rowusercode, spid, spicode, spidescription);

                gvBSSP.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem ei = e.Item as GridEditableItem;
                RadTextBox spidentry = (RadTextBox)ei.FindControl("radtbbsspidedit");
                RadTextBox spcodeentry = (RadTextBox)ei.FindControl("radtbbsspcodeedit");
                RadTextBox spdescriptionentry = (RadTextBox)ei.FindControl("radtbbsspdescriptionedit");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? strategyid = General.GetNullableGuid(ei.OwnerTableView.DataKeyValues[ei.ItemIndex]["FLDBSSTRATEGICPERSPECTIVEID"].ToString());

                string spid = General.GetNullableString(spidentry.Text);
                string spicode = General.GetNullableString(spcodeentry.Text);
                string spidescription = General.GetNullableString(spdescriptionentry.Text);

                if (!IsValidBSSP(spid, spicode, spidescription))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSSP.BSSPUpdate(rowusercode, spid, spicode, spidescription, strategyid);

                gvBSSP.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidBSSP(string id, string shortcode, string description)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (id == null)
        {
            ucError.ErrorMessage = "ID.";
        }
        if (shortcode == null)
        {
            ucError.ErrorMessage = "Short Code.";
        }
        if (description == null)
        {
            ucError.ErrorMessage = "Description.";
        }

       
        return (!ucError.IsError);
    }
}