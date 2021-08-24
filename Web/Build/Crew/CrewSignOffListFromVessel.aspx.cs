using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class CrewSignOffListFromVessel : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvCrewList.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvCrewList.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }

    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewSignOffListFromVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrewList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCrewList.AccessRights = this.ViewState;
            MenuCrewList.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["empid"] = null;
                ViewState["signonoffid"] = null;
                ifMoreInfo.Attributes["src"] = "CrewSignOffConfirm.aspx";

                gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindDataForVessel(object sender, EventArgs e)
    {
        gvCrewList.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDVESSELSIGNONDATE", "FLDEMPLOYEESIGNONDATE", "FLDVESSELSIGNOFFDATE", "FLDEMPLOYEESIGNOFFDATE" };
        string[] alCaptions = { "S.no", "File No", "Name", "Rank", "Vessel", "ACCOUNT SIGN-ON DATE", "CREW SIGN-ON DATE", "ACCOUNT SIGN-OFF DATE", "CREW SIGN-OFF DATE" };

        DataSet ds = PhoenixCrewSignOffListFromVessel.GetCrewSignOffListFromVessel(General.GetNullableInteger(ddlVessel.SelectedVessel),
                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                gvCrewList.PageSize,
                                ref iRowCount,
                                ref iTotalPageCount);

        General.SetPrintOptions("gvCrewList", "Pending Crew Sign-Off List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewList.DataSource = ds;

            if (ViewState["signonoffid"] == null)
            {

                ViewState["signonoffid"] = ds.Tables[0].Rows[0]["FLDSIGNONOFFID"].ToString();
                ViewState["empid"] = ds.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();
            }
            ifMoreInfo.Attributes["src"] = "CrewSignOffConfirm.aspx?empid=" + ViewState["empid"].ToString() + "&signonoffid=" + ViewState["signonoffid"].ToString();
        }
        else
        {
            gvCrewList.DataSource = ds;
            ifMoreInfo.Attributes["src"] = "CrewSignOffConfirm.aspx";
        }
        gvCrewList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDVESSELSIGNONDATE", "FLDEMPLOYEESIGNONDATE", "FLDVESSELSIGNOFFDATE", "FLDEMPLOYEESIGNOFFDATE" };
            string[] alCaptions = { "S.no", "File No", "Name", "Rank", "Vessel", "ACCOUNT SIGN-ON DATE", "CREW SIGN-ON DATE", "ACCOUNT SIGN-OFF DATE", "CREW SIGN-OFF DATE" };
            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds = PhoenixCrewSignOffListFromVessel.GetCrewSignOffListFromVessel(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                , 1,
                                iRowCount,
                                ref iRowCount,
                                ref iTotalPageCount);

            General.ShowExcel("Pending Crew Sign-Off List", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCrewList_TabStripCommand(object sender, EventArgs e)
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



    //protected void gvCrewList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    try
    //    {
    //        if (IsGridSelectable)
    //        {
    //GridView _gridView = (GridView)sender;
    //_gridView.SelectedIndex = e.NewSelectedIndex;
    //ViewState["signonoffid"] = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblSignOnOffId")).Text;
    //ViewState["empid"] = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblCrewId")).Text;
    //ifMoreInfo.Attributes["src"] = "CrewSignOffConfirm.aspx?empid=" + ViewState["empid"].ToString() + "&signonoffid=" + ViewState["signonoffid"].ToString();

    //            //BindData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}



    private bool IsValidSignOff(string date, string SeaPort, string reason, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign-Off Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign-Off Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(SeaPort).HasValue)
        {
            ucError.ErrorMessage = "Sign-Off Sea Port is required.";
        }
        if (!General.GetNullableInteger(reason).HasValue)
        {
            ucError.ErrorMessage = "Sign-Off Reason is required.";
        }
        if (String.IsNullOrEmpty(remarks))
        {
            ucError.ErrorMessage = "Sign Off Remarks is required.";
        }
        return (!ucError.IsError);
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

    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                e.Item.Selected = true;
                ViewState["signonoffid"] = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                ViewState["empid"] = ((RadLabel)e.Item.FindControl("lblCrewId")).Text;
                ifMoreInfo.Attributes["src"] = "CrewSignOffConfirm.aspx?empid=" + ViewState["empid"].ToString() + "&signonoffid=" + ViewState["signonoffid"].ToString();
                gvCrewList.Rebind();
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

    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblCrewId = (RadLabel)e.Item.FindControl("lblCrewId");

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
            lb.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + lblCrewId.Text + "'); return false;");

            UserControlSignOffReason ddlReason = (UserControlSignOffReason)e.Item.FindControl("ddlReason");
            if (ddlReason != null) ddlReason.SelectedSignOffReason = drv["FLDSIGNOFFREASONID"].ToString();

            UserControlSeaport ddlSignOffPort = (UserControlSeaport)e.Item.FindControl("ddlSignOffPort");
            if (ddlSignOffPort != null) ddlSignOffPort.SelectedSeaport = drv["FLDSIGNOFFSEAPORTID"].ToString();

        }
    }
}
