using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txtWorkorderNumber.Focus();
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-stack-2x\"></i>", "CLEAR");

            MenuDivWorkOrderList.AccessRights = this.ViewState;
            MenuDivWorkOrderList.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REPORTID"] = Request.QueryString["REPORTID"].ToString();
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                ddlyear.QuickTypeCode = "55";
                ddlyear.bind();
                ddlyear.SelectedText = DateTime.Today.Year.ToString();
                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrderList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper()=="FIND")
            {
                RadGrid1.CurrentPageIndex = 0;
                BindData();
                RadGrid1.Rebind();
            }
            else if (CommandName.ToUpper()=="CLEAR")
            {
                ViewState["WORKORDERID"] = null;
                txtComponentNumber.Text = "";
                txtWorkorderName.Text = "";
                txtWorkorderNumber.Text = "";
                ddlMonth.SelectedValue = "";
                ddlyear.SelectedText = DateTime.Today.Year.ToString();
                BindData();
                RadGrid1.Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds;
            ds = PhoenixCommonPlannedMaintenance.WorkOrderListSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                    , General.GetNullableString(txtWorkorderNumber.Text)
                                                                    , General.GetNullableString(txtWorkorderName.Text)
                                                                    , General.GetNullableString(txtComponentNumber.Text)
                                                                    , General.GetNullableInteger(ddlMonth.SelectedValue)
                                                                    , General.GetNullableInteger(ddlyear.SelectedText.ToString())
                                                                    , sortexpression, sortdirection
                                                                    , RadGrid1.CurrentPageIndex + 1
                                                                    , RadGrid1.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

            RadGrid1.DataSource = ds;
            RadGrid1.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //if (e.CommandName.ToUpper().Equals("PAGE"))
        //{
        //    RadGrid1.CurrentPageIndex = 0;
        //}

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            RadGrid _gridView = (RadGrid)sender;

            int nCurrentRow = e.Item.RowIndex;

            ViewState["COMPONENTID"] = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
            ViewState["WORKORDERID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
            ViewState["JOBID"] = ((RadLabel)e.Item.FindControl("lblJobID")).Text;
            ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;

            PhoenixCommonPlannedMaintenance.PMSWorkOrderInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , new Guid(ViewState["WORKORDERID"].ToString())
                                                                        , new Guid(ViewState["FORMID"].ToString())
                                                                        , new Guid(ViewState["COMPONENTID"].ToString())
                                                                        , new Guid(ViewState["REPORTID"].ToString())
                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            BindData();
            RadGrid1.Rebind();

            ucStatus.Text = "Work Order Selected";
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += " fnReloadList();";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);


        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
     
    }

}

