using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Text;
using Telerik.Web.UI;
public partial class CrewPlanEventOnboardCrew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("ADD", "ADDPLAN", ToolBarDirection.Right);
            gvPlanTab.AccessRights = this.ViewState;
            gvPlanTab.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["Vesselid"] = Request.QueryString["VESSELID"].ToString();
                }
                if (Request.QueryString["eventid"] != null && Request.QueryString["eventid"].ToString() != "")
                {
                    ViewState["eventid"] = Request.QueryString["eventid"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPlanTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "ADDPLAN")
            {
                StringBuilder stremployeelist = new StringBuilder();
                StringBuilder strSignonoffid = new StringBuilder();
                if (gvPlan.MasterTableView.Items.Count > 0)
                {
                    foreach (GridDataItem gvr in gvPlan.MasterTableView.Items)
                    {
                        RadCheckBox chkAdd = (RadCheckBox)gvr.FindControl("chkAdd");

                        if (chkAdd.Checked == true)
                        {
                            RadLabel lblSignonoffid = (RadLabel)gvr.FindControl("lblSignonoffid");

                            strSignonoffid.Append(lblSignonoffid.Text);
                            strSignonoffid.Append(",");
                        }
                    }

                    if (strSignonoffid.Length > 1)
                    {
                        strSignonoffid.Remove(strSignonoffid.Length - 1, 1);
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please select any seafarer to Add";
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    ucError.ErrorMessage = "Please select plan to Add";
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewChangeEventDetail.CrewReliefPlanAdd(
                     General.GetNullableGuid(ViewState["eventid"].ToString())
                    , General.GetNullableInteger(ViewState["Vesselid"].ToString())
                    , null
                    , strSignonoffid.ToString()
                   );

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);

                BindData();
                gvPlan.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            string Vesselid = (ViewState["Vesselid"] == null) ? null : (ViewState["Vesselid"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewChangeEventDetail.CrewEventPlanOnboardSearch(int.Parse(Vesselid)
                                                                         , sortexpression, sortdirection
                                                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                         , gvPlan.PageSize
                                                                         , ref iRowCount
                                                                         , ref iTotalPageCount);


            gvPlan.DataSource = ds;
            gvPlan.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
}