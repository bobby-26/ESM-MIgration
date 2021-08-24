using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Data;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReApprovalforSeafarersList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewReApprovalforSeafarersList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReApproval')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewReApprovalforSeafarersFilters.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewReApprovalforSeafarersList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuCrewReApproval.AccessRights = this.ViewState;
            MenuCrewReApproval.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 0;                
                ViewState["EMPLOYEEID"] = null;

                gvReApproval.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        ViewState["EMPLOYEEID"] = null;
        BindData();
        gvReApproval.Rebind();
    }

    protected void MenuCrewReApproval_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentCrewReEmploymentFilterSelection = null;
                ViewState["EMPLOYEEID"] = null;
                gvReApproval.CurrentPageIndex = 0;
                BindData();
                gvReApproval.Rebind();
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
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "File No.", " Name", "Rank", "Last Vessel", "Sign off" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentCrewReEmploymentFilterSelection;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
            nvc.Add("ucDate", string.Empty);
            nvc.Add("ucDate1", string.Empty);
            nvc.Add("ucRank", string.Empty);
            nvc.Add("ucVessel", string.Empty);
        }

        DataSet ds = PhoenixCrewReApprovalSeafarer.SearchReApprovalSeafarrerList((nvc.Get("ucRank")) == "," ? null : General.GetNullableString(nvc.Get("ucRank"))
           , General.GetNullableString(nvc.Get("ucVessel").Replace("Dummy", "").TrimStart(','))
           , General.GetNullableDateTime(nvc.Get("ucDate"))
           , General.GetNullableDateTime(nvc.Get("ucDate1"))
           , General.GetNullableInteger(nvc.Get("chkApprovedYN"))
           , General.GetNullableString(nvc.Get("txtEmployeeFileNo"))
           , General.GetNullableString(nvc.Get("txtEmployeeName"))
           , sortexpression
           , sortdirection
           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
           , gvReApproval.PageSize
           , ref iRowCount
           , ref iTotalPageCount
        );

        General.ShowExcel("Re-Employment Approvals", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "File No.", "Name", "Rank", "Last Vessel", "Sign off" };
        string sortexpression;
        int? sortdirection = 0;
        
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            NameValueCollection nvc = Filter.CurrentCrewReEmploymentFilterSelection;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc.Add("ucDate", string.Empty);
                nvc.Add("ucDate1", string.Empty);
                nvc.Add("ucRank", string.Empty);
                nvc.Add("ucVessel", string.Empty);
            }
            DataSet ds = PhoenixCrewReApprovalSeafarer.SearchReApprovalSeafarrerList((nvc.Get("ucRank")) == "," ? null : General.GetNullableString(nvc.Get("ucRank"))
               , General.GetNullableString(nvc.Get("ucVessel").Replace("Dummy", "").TrimStart(','))
               , General.GetNullableDateTime(nvc.Get("ucDate"))
               , General.GetNullableDateTime(nvc.Get("ucDate1"))
               , General.GetNullableInteger(nvc.Get("chkApprovedYN"))
               , General.GetNullableString(nvc.Get("txtEmployeeFileNo"))
               , General.GetNullableString(nvc.Get("txtEmployeeName"))
               , sortexpression
               , sortdirection
               , Int32.Parse(ViewState["PAGENUMBER"].ToString())
               , gvReApproval.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );

            General.SetPrintOptions("gvReApproval", "Re-Employment Approvals", alCaptions, alColumns, ds);

            gvReApproval.DataSource = ds.Tables[0];
            gvReApproval.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["EMPLOYEEID"] == null)
                {
                    ViewState["EMPLOYEEID"] = ds.Tables[0].Rows[0]["FLDEMPLOYEEID"].ToString();

                }
            }
            else
            {

                ifMoreInfo.Attributes["src"] = "../Crew/CrewReApprovalforSeafarers.aspx?EmployeeId=&RankId=&VesselId=&SignoffDate=&docid=&approvedyn=";
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }

    private void SetRowSelection()
    {
        try
        {
            gvReApproval.SelectedIndexes.Clear();

            foreach (GridDataItem item in gvReApproval.Items)
            {
                if (item.GetDataKeyValue("FLDEMPLOYEEID").ToString() == ViewState["EMPLOYEEID"].ToString())
                {
                   
                    gvReApproval.SelectedIndexes.Add(item.ItemIndex);

                    foreach (GridDataItem Dataitem in gvReApproval.SelectedItems)
                    {
                        string sRankId = ((RadLabel)Dataitem.FindControl("lblRankId")).Text.Trim();
                        string sVesselId = ((RadLabel)Dataitem.FindControl("lblLastVesselID")).Text.Trim();
                        string sSignoffDate = ((RadLabel)Dataitem.FindControl("lblsignoffDate")).Text.Trim();
                        string sDtKey = ((RadLabel)Dataitem.FindControl("lblDtKey")).Text.Trim();
                        string approvedyn = ((RadLabel)Dataitem.FindControl("lblApprovedYN")).Text.Trim();

                        ifMoreInfo.Attributes["src"] = "../Crew/CrewReApprovalforSeafarers.aspx?EmployeeId=" + ViewState["EMPLOYEEID"].ToString() + "&RankId=" + sRankId + "&VesselId=" + sVesselId + "&SignoffDate=" + sSignoffDate + "&docid=" + sDtKey + "&approvedyn=" + approvedyn;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvReApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReApproval.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvReApproval_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT")
                return;


            if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["EMPLOYEEID"] = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text.Trim();
               
            }
            if (e.CommandName.ToUpper() == "APPROVE")
            {
                ViewState["EMPLOYEEID"] = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text.Trim();
                string sRankId = ((RadLabel)e.Item.FindControl("lblRankId")).Text.Trim();
                string sVesselId = ((RadLabel)e.Item.FindControl("lblLastVesselID")).Text.Trim();
                string sSignoffDate = ((RadLabel)e.Item.FindControl("lblsignoffDate")).Text.Trim();
                string sRankLevel = ((RadLabel)e.Item.FindControl("lbllevel")).Text.Trim();
                string sTech = ((RadLabel)e.Item.FindControl("lblSupt")).Text.Trim();
                string sDtKey = ((RadLabel)e.Item.FindControl("lblDtKey")).Text.Trim();
                string approvedyn = ((RadLabel)e.Item.FindControl("lblApprovedYN")).Text.Trim();
                string stype = string.Empty;


                if (Convert.ToInt32(sRankLevel) <= 4)
                    stype = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 98, "SRA");
                else
                    stype = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 98, "JRA");

                PhoenixCrewReApprovalSeafarer.SearchSeafarerReEmploymentCompletad(General.GetNullableGuid(sDtKey));
                LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");

                string sScript = "javascript:openNewWindow('approval', '', '" + Session["sitepath"] + "/Common/CommonCrewApproval.aspx?docid=" + sDtKey + "&mod=" + PhoenixModule.CREW
                + "&type=" + stype + "&user=" + sTech + "&vslid=" + sVesselId + "&empid=" + ViewState["EMPLOYEEID"].ToString() + "&pdtype=3')";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
                
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
    }

    protected void gvReApproval_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");

                if (lbr != null)
                {
                    lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                }

                LinkButton ibapr = (LinkButton)e.Item.FindControl("cmdApprove");
                RadLabel lblapr = (RadLabel)e.Item.FindControl("lblApprovedYN");
                if (ibapr != null && lblapr != null)
                {
                    if (lblapr.Text == "1")
                    {
                        ibapr.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvReApproval_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
