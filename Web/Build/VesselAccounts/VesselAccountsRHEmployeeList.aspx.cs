using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccountsRHEmployeeList : PhoenixBasePage
{
    public string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            MenuWorkHour.AccessRights = this.ViewState;
            MenuWorkHour.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkHour_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CANCEL"))
        {
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo','');";
            Script += "parent.CloseCodeHelpWindow('MoreInfo');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

            String script = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        //string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDRANKNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSTARTDATE", "FLDGTOVERTIME", "FLDOVERTIME" };
        //string[] alCaptions = { "Sr.No", "Name", "Rank", "SignOn Date", "Releif Due Date", "Start Date", "Guran. OT", "OT" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        NameValueCollection nvc = Filter.CurrentCrewListSelection;

        DataSet ds = PhoenixVesselAccountsRH.SearchRestHourEmployee(PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , gvCrewList.PageSize
                                            , ref iRowCount
                                            , ref iTotalPageCount);

        //General.SetPrintOptions("gvCrewList", "Crew List", alCaptions, alColumns, ds);

        gvCrewList.DataSource = ds;
        gvCrewList.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

        }
    }
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                PhoenixVesselAccountsRH.InsertRestHourEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt64(((RadLabel)e.Item.FindControl("lblEmpId")).Text),
                        Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.VesselID),
                        null, null, null, null, 1, General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblusercode")).Text));
                Rebind();
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                ucStatus.Text = "Crew List Saved Successfully";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidStart(string startdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(startdate) == null)
            ucError.ErrorMessage = "Start Date is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvCrewList_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //if (!IsValidStart(((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucStartDate")).Text))
            //{
            //    ucError.Visible = true;
            //    return;
            //}
            //PhoenixVesselAccountsRH.InsertRestHourStart(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //            General.GetNullableGuid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblRHstartid")).Text),
            //            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            //             Convert.ToInt32(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblEmpId")).Text),
            //             Convert.ToDateTime(((TextBox)_gridView.Rows[nCurrentRow].FindControl("ucStartDate")).Text),
            //             int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShipCalendarId")).Text));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewList_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrewList.SelectedIndexes.Clear();
        gvCrewList.EditIndexes.Clear();
        gvCrewList.DataSource = null;
        gvCrewList.Rebind();
    }
}
