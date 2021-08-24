using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComJobListForPlan : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Add", "ADD", ToolBarDirection.Right);

        toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComJobListForPlan.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuComponentType.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            gvComponentJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["groupId"] = "";

            ViewState["CompNoFilter"] = "";
            ViewState["CompNameFilter"] = "";
            ViewState["filterDiscipline"] = "";
            ViewState["FilterFrequencyType"] = "";

            ViewState["compCategoryId"] = null;
            ViewState["disciplineId"] = null;
            ViewState["frequencytype"] = null;
            ViewState["frequency"] = null;

            if (Request.QueryString["groupId"] != null)
                ViewState["groupId"] = Request.QueryString["groupId"].ToString();

        }

    }

    protected void MenuComponentType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            Guid? groupId = Guid.Empty;

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["compCategoryId"] = null;
                ViewState["disciplineId"] = null;
                ViewState["frequencytype"] = null;
                ViewState["frequency"] = null;
                ViewState["filterDiscipline"] = "";
                ViewState["FilterFrequencyType"] = "";

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvComponentJob.Rebind();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                NameValueCollection nvc = new NameValueCollection();

                //string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "fnReloadList('codehelp1','ifMoreInfo');";
                //Script += "</script>" + "\n";

                string csvjobList = string.Empty;
         
                foreach (GridDataItem gr in gvComponentJob.Items)
                {
                    CheckBox ChkPlan = (CheckBox)gr["ClientSelectColumn"].Controls[0];

                    if (ChkPlan.Checked && ChkPlan.Enabled)
                    {
                        csvjobList += ((RadLabel)gr.FindControl("lblWorkOrderId")).Text + ",";
                    }
                }
                if (csvjobList == string.Empty)
                {
                    ucError.ErrorMessage = "Select atleast one or more Component Jobs";
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceWorkOrderGroup.PlannedGroupWoInsert(csvjobList,ref groupId);
                //ucStatus.Text = "Jobs Added successfully";
                BindData();
                gvComponentJob.Rebind();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);

                //var Frame = string.IsNullOrEmpty(Request.QueryString["ifr"]) ? "null" : "'ifMoreInfo'";
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1'," + Frame + ", true);", true);
                //nvc.Add("lblJobCode", strNumber.TrimEnd(','));
                //nvc.Add("lnkJobName", strName.TrimEnd(','));
                //nvc.Add("lblComponentJobId", strId.TrimEnd(','));
                //Filter.CurrentPickListSelection = nvc;
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.RoundsComponentJobSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                       null,
                       ViewState["CompNoFilter"].ToString() != string.Empty ? ViewState["CompNoFilter"].ToString() : null,
                       ViewState["CompNameFilter"].ToString() != string.Empty ? ViewState["CompNameFilter"].ToString() : null,
                       null, null, sortexpression, sortdirection,
                       int.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvComponentJob.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount,
                       ViewState["disciplineId"] != null ? General.GetNullableInteger(ViewState["disciplineId"].ToString()) : General.GetNullableInteger(ViewState["filterDiscipline"].ToString()),
                       ViewState["frequency"] != null ? General.GetNullableInteger(ViewState["frequency"].ToString()) : null,
                       ViewState["frequencytype"] != null ? General.GetNullableInteger(ViewState["frequencytype"].ToString()): General.GetNullableInteger(ViewState["FilterFrequencyType"].ToString()),
                       ViewState["compCategoryId"] != null ? General.GetNullableInteger(ViewState["compCategoryId"].ToString()) : null );

            gvComponentJob.DataSource = ds;
            gvComponentJob.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvComponentJob_Sorting(object sender, GridViewSortEventArgs se)
    {

    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    CheckBox obj = sender as CheckBox;
        //    GridViewRow gvr = (GridViewRow)obj.Parent.Parent;
        //    string componentjobid = ((Label)gvr.FindControl("lblComponentJobId")).Text;
        //    ViewState["COMPONENTJOBID"] = componentjobid;
        //    int iMessageCode = 0;
        //    string iMessageText = "";
        //    if (obj.Checked)
        //        PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(Request.QueryString["COMPONENTJOBID"]), componentjobid, null, ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText);
        //    else
        //        PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(Request.QueryString["COMPONENTJOBID"]), null, componentjobid, ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText);

        //    if (iMessageCode == 1)
        //        throw new ApplicationException(iMessageText);

        //    string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
        //    Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
        //    Script += "</script>" + "\n";
        //    NameValueCollection nvc = new NameValueCollection();

        //    Label lblComponentNumber = (Label)gvr.FindControl("lblJobCode");
        //    nvc.Add("lblJobCode", lblComponentNumber.Text);
        //    LinkButton lnkJob = (LinkButton)gvr.FindControl("lnkJobName");
        //    nvc.Add("lnkJobName", lnkJob.Text);
        //    nvc.Add("lblComponentJobId", componentjobid);
        //    Filter.CurrentPickListSelection = nvc;
        //    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        //}
        //catch (ApplicationException aex)
        //{
        //    ucConfirm.HeaderMessage = "Please Confirm";
        //    ucConfirm.ErrorMessage = aex.Message;
        //    ucConfirm.Visible = true;
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        //int iMessageCode = 0;
        //string iMessageText = "";
        //PhoenixCommonPlannedMaintenance.RoundsJobUpdate(new Guid(Request.QueryString["COMPONENTJOBID"]), ViewState["COMPONENTJOBID"].ToString(), null, ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText);
        //string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
        //Script += "</script>" + "\n";
        //NameValueCollection nvc = new NameValueCollection();

        //nvc.Add("lblJobCode", string.Empty);        
        //nvc.Add("lnkJobName", string.Empty);
        //nvc.Add("lblComponentJobId", ViewState["COMPONENTJOBID"].ToString());
        //Filter.CurrentPickListSelection = nvc;
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void gvComponentJob_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponentJob.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvComponentJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if(e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else if (e.CommandName.ToUpper().Equals("FIND"))
        {
            
            ViewState["compCategoryId"] = ((RadLabel)e.Item.FindControl("lblCompCategory")).Text;
            ViewState["disciplineId"] = ((RadLabel)e.Item.FindControl("lblDisciplineId")).Text;
            ViewState["frequencytype"] = ((RadLabel)e.Item.FindControl("lblFrequencyType")).Text;
            ViewState["frequency"] = ((RadLabel)e.Item.FindControl("lblFrequency")).Text;

            gvComponentJob.CurrentPageIndex = 0;
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvComponentJob.Rebind();
        }
    }

    protected void gvComponentJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Item is GridDataItem)
        {
            //LinkButton lnk = (LinkButton)e.Item.FindControl("lnkJobName");
            //if (lnk != null && Request.QueryString["mode"] == "multi")
            //{
            //    lnk.Enabled = false;
            //}
            //RadCheckBox chksel = (RadCheckBox)e.Item.FindControl("chkSelect");
            //if (chksel != null && Request.QueryString["mode"] != "multi")
            //    chksel.Visible = false;
        }
    }
    //protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //        if (CommandName.ToUpper().Equals("CLEAR"))
    //        {
    //            ViewState["compCategoryId"] = null;
    //            ViewState["disciplineId"] = null;
    //            ViewState["frequencytype"] = null;
    //            ViewState["frequency"] = null;
    //            ViewState["filterDiscipline"] = null;
    //            ViewState["FilterFrequencyType"] = null;

    //            gvComponentJob.CurrentPageIndex = 0;
    //            ViewState["PAGENUMBER"] = 1;
    //            BindData();
    //            gvComponentJob.Rebind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    
    protected void GriducDiscipline_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["compCategoryId"] = null;
        ViewState["disciplineId"] = null;
        ViewState["frequencytype"] = null;
        ViewState["frequency"] = null;
        ViewState["CompNoFilter"] = "";
        ViewState["CompNameFilter"] = "";

        ViewState["PAGENUMBER"] = 1;

        UserControlDiscipline discipline = sender as UserControlDiscipline;
        ViewState["filterDiscipline"] = discipline.SelectedDiscipline;
        gvComponentJob.DataSource = null;
        gvComponentJob.MasterTableView.Rebind();
    }

    protected void ucFrequencyType_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["compCategoryId"] = null;
        ViewState["disciplineId"] = null;
        ViewState["frequencytype"] = null;
        ViewState["frequency"] = null;
        ViewState["CompNoFilter"] = "";
        ViewState["CompNameFilter"] = "";

        ViewState["PAGENUMBER"] = 1;

        UserControlHard frequencyType = sender as UserControlHard;
        ViewState["FilterFrequencyType"] = frequencyType.SelectedHard;
        gvComponentJob.DataSource = null;
        gvComponentJob.MasterTableView.Rebind();
        
    }

    protected void gvComponentJob_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void txtCompNoFilter_TextChanged(object sender, EventArgs e)
    {
        ViewState["compCategoryId"] = null;
        ViewState["Discipline"] = null;
        ViewState["frequencytype"] = null;
        ViewState["frequency"] = null;
        ViewState["CompNoFilter"] = "";
        ViewState["CompNameFilter"] = "";

        ViewState["PAGENUMBER"] = 1;

        RadTextBox compNo = sender as RadTextBox;
        ViewState["CompNoFilter"] = compNo.Text;

        gvComponentJob.DataSource = null;
        gvComponentJob.MasterTableView.Rebind();
    }

    protected void txtCompNameFilter_TextChanged(object sender, EventArgs e)
    {
        ViewState["compCategoryId"] = null;
        ViewState["Discipline"] = null;
        ViewState["frequencytype"] = null;
        ViewState["frequency"] = null;
        ViewState["CompNoFilter"] = "";
        ViewState["CompNameFilter"] = "";

        ViewState["PAGENUMBER"] = 1;

        RadTextBox compName = sender as RadTextBox;
        ViewState["CompNameFilter"] = compName.Text;

        gvComponentJob.DataSource = null;
        gvComponentJob.MasterTableView.Rebind();
    }
}
