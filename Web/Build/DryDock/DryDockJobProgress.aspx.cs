using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class DryDock_DryDockJobProgress : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../DryDock/DryDockJobProgress.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        gvmenu.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            radtbdate.Text = DateTime.Now.Date.ToString();
            gvprogress.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["ORDERID"] = General.GetNullableGuid(Request.QueryString["orderid"]);
            ViewState["VESSELID"] = General.GetNullableInteger(Request.QueryString["vslid"]);
            ViewState["QUOTATIONID"] = General.GetNullableGuid(Request.QueryString["quotationid"]);
            DataTable dt = PhoenixDryDockOrder.Yardname(General.GetNullableGuid(Request.QueryString["quotationid"]));

            lblyard.Text = dt.Rows[0]["FLDNAME"].ToString();
        }
    }

    protected void gvmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {

            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidSearch())
                {
                    ucError.Visible = true;

                }
                gvprogress.Rebind();
               

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvprogress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        Guid? orderid = General.GetNullableGuid(ViewState["ORDERID"].ToString());
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        Guid? Quotationid = General.GetNullableGuid(ViewState["QUOTATIONID"].ToString());
        DateTime? date = General.GetNullableDateTime(radtbdate.Text);
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataTable dt = PhoenixDryDockOrder.DrydockProgressSearch(orderid, vesselid, Quotationid, date, gvprogress.CurrentPageIndex + 1, gvprogress.PageSize, ref iRowCount, ref iTotalPageCount);
        gvprogress.DataSource = dt;
        gvprogress.VirtualItemCount = iRowCount;

    }

    protected void gvprogress_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvprogress_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvprogress_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        try {
            RadGrid grid = (RadGrid)sender;
            foreach (GridBatchEditingCommand cmd in e.Commands)
            {

                PhoenixDryDockOrder.DrydockProgressInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(cmd.NewValues["FLDORDERLINEID"].ToString())
                                                            , General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                            , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                            , General.GetNullableGuid(ViewState["QUOTATIONID"].ToString())
                                                            , General.GetNullableDecimal(cmd.NewValues["FLDPROGRESS"].ToString())
                                                            , General.GetNullableDateTime(radtbdate.Text)
                                                            );
            }
            PhoenixDryDockOrder.DrydockcostIncurred(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                            , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                            , General.GetNullableGuid(ViewState["QUOTATIONID"].ToString())
                                                            , General.GetNullableDateTime(radtbdate.Text));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    public bool IsValidSearch()
    {


        ucError.HeaderMessage = "Please provide following information.";
        if (General.GetNullableDateTime(radtbdate.Text) == null)
        {
            ucError.ErrorMessage = "Date.";

        }
        return (!ucError.IsError);
    }
}