using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class DashboardCrewListSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
           
            if (!IsPostBack)
            {
                ViewState["SearchValue"] = "";
                ViewState["FileNo"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["RANK"] = string.Empty;

                if (Request.QueryString["qSearch"] != null && Request.QueryString["qSearch"].ToString() != "")
                    ViewState["SearchValue"] = Request.QueryString["qSearch"].ToString();
                if (Request.QueryString["Fileno"] != null && Request.QueryString["Fileno"].ToString() != "")
                    ViewState["FileNo"] = Request.QueryString["Fileno"].ToString();

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
              
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
       
    }

    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        gvCrewSearch.Rebind();
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("GETEMPLOYEE"))
            {

            }
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
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                grid.CurrentPageIndex = 0;
                ViewState["RANK"] = gvCrewSearch.MasterTableView.GetColumn("FLDRANKPOSTEDNAME").CurrentFilterValue;
               // ViewState["NATIONALITYACTUAL"] = gvCrewSearch.MasterTableView.GetColumn("FLDNATIONALITY").CurrentFilterValue;
               // string daterange = gvCrewSearch.MasterTableView.GetColumn("FLDSIGNONDATE").CurrentFilterValue;
                //string[] dates = daterange.Split('~');
                //ViewState["FDATE"] = (dates.Length > 0 ? dates[0] : string.Empty);
                //ViewState["TDATE"] = (dates.Length > 1 ? dates[1] : string.Empty);
                // ViewState["VESSELID"] = gvCrewSearch.MasterTableView.GetColumn("FLDVESSELID").CurrentFilterValue;
                // UcVessel.SelectedVessel = gvCrewSearch.MasterTableView.GetColumn("FLDVESSELID").CurrentFilterValue; 
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlRank_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlrank = sender as RadComboBox;
        ddlrank.DataSource = PhoenixRegistersRank.ListRank();
        ddlrank.DataTextField = "FLDRANKCODE";
        ddlrank.DataValueField = "FLDRANKCODE";
        ddlrank.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlrank.SelectedValue = ViewState["RANK"].ToString();
    }
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeid");
        RadLabel status = (RadLabel)e.Item.FindControl("lblStatus");

        ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
        if (ed != null)
            ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

        LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");

        if (lnkName != null)
        {
            lnkName.Visible = SessionUtil.CanAccess(this.ViewState, lnkName.CommandName);
            if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
            {
                //if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                //{
                    lnkName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + empid.Text + "&launchedfrom=offshore'); return false;");
                //}
                //else
                //{
                //    lnkName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                //}
            }
            else
            {
                //if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                //{
                    lnkName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&launchedfrom=offshore'); return false;");
                //}
                //else
                //{
                //    lnkName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                //}
            }          
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDNAME", "FLDRANKPOSTEDNAME", "FLDEMPLOYEECODE", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE",
                               "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDPLANNEDVESSELCODES", "FLDDOA", "FLDSTATUSDESCRIPTION","FLDZONE" };
        string[] alCaptions = { "Name", "Rank", "File No", "Last Vessel", "Sign-Off Date", "Present Vessel",
                                  "Sign-On Date","Next Vessel","DOA","Status","Zone"  };

        DataSet ds = PhoenixCommonDashboard.DashboardEmployeeList(
                null, PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                1, "");

        //General.ShowExcel("PersonnelMaster", ds.Tables[0], alColumns, alCaptions);
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = null;//(ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            //if (ViewState["SORTDIRECTION"] != null)
            //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = PhoenixCommonDashboard.DashboardEmployeeSearch(
                null, PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                null, ViewState["SearchValue"].ToString(), General.GetNullableString(ViewState["FileNo"].ToString())
                 , sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCrewSearch.PageSize,
            ref iRowCount,
            ref iTotalPageCount
                );


            gvCrewSearch.DataSource = ds;
            gvCrewSearch.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            try
            {
                ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
                BindData();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
