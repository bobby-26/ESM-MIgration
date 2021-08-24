using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class CrewMatrixComplianceOnboard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewMatrixComplianceOnboard.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewMatrixComplianceOnboard.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");

            MenuCrew.AccessRights = this.ViewState;
            MenuCrew.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["VESSELID"] = "";
                ViewState["OILMAJORID"] = "";
                ViewState["CONTRACTID"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["oilmajorid"] != null && Request.QueryString["oilmajorid"].ToString() != "")
                    ViewState["OILMAJORID"] = Request.QueryString["oilmajorid"].ToString();

                if (Request.QueryString["contractid"] != null && Request.QueryString["contractid"].ToString() != "")
                    ViewState["CONTRACTID"] = Request.QueryString["contractid"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuCrew_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDFILENO", "FLDRANKCODE", "FLDNAME", "FLDCOUNTRYCODE", "FLDEMPLOYEEAGE", "FLDMONTHSONBOARD", "FLDOPERATOREXPINYEARS", "FLDRANKEXPINYEARS", "FLDALLTYPEEXPINYEARS", "FLDSIGNONDATE" };
                string[] alCaptions = { "File No.", "Rank", "Name", "Nationality", "Age", "Month Onboard", "Years with Operator", "Years in Rank", "Years on All Type of Tankers", "Sign On" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewMatrixCompliance.SearchMatrixComplianceOnboard(
                                                                 General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                , sortexpression, sortdirection,
                                                                 int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                 gvCrew.PageSize,
                                                                 ref iRowCount,
                                                                 ref iTotalPageCount);


                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Event", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

            else if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;

                BindData();
                gvCrew.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuMatrixReq_TabStripCommand(object sender, EventArgs e)
    {

    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFILENO", "FLDRANKCODE", "FLDNAME", "FLDCOUNTRYCODE", "FLDEMPLOYEEAGE", "FLDMONTHSONBOARD", "FLDOPERATOREXPINYEARS", "FLDRANKEXPINYEARS", "FLDALLTYPEEXPINYEARS", "FLDSIGNONDATE" };
            string[] alCaptions = { "File No.", "Rank", "Name", "Nationality", "Age", "Month Onboard", "Years with Operator", "Years in Rank", "Years on All Type of Tankers","Sign On" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewMatrixCompliance.SearchMatrixComplianceOnboard(
                                                                   General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                   , sortexpression, sortdirection,
                                                                  int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                  gvCrew.PageSize,
                                                                  ref iRowCount,
                                                                  ref iTotalPageCount);

            General.SetPrintOptions("gvCrew", "Crew Onboard", alCaptions, alColumns, ds);

            gvCrew.DataSource = ds;
            gvCrew.VirtualItemCount = iRowCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrew.Rebind();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
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
        gvCrew.Rebind();
    }


    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkname = (LinkButton)e.Item.FindControl("lnkname");

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
            {
                if (drv["FLDISNEWAPPLICANT"].ToString() == "1")
                {
                    lnkname.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "',false,800,500); return false;");
                }
                else
                {
                    lnkname.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "',false,800,500); return false;");
                }

            }
            else
            {
                if (drv["FLDISNEWAPPLICANT"].ToString() == "1")
                {
                    lnkname.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "',false,800,500); return false;");
                }
                else
                {
                    lnkname.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "',false,800,500); return false;");
                }
            }
        }

    }

    private void BindMatrixReq()
    {
        try
        {
            string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME", "FLDSTATUSNAME" };
            string[] alCaptions = { "Medical", "Place of Issue", "Issued", "Expiry", "Flag", "Status" };


            DataTable dt = PhoenixCrewMatrixCompliance.SearchMatrixComplianceOnboardReq(
                       General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ViewState["CONTRACTID"].ToString()), General.GetNullableInteger(ViewState["OILMAJORID"].ToString()));

            //General.SetPrintOptions("gvMatrixReq", "Matrix Requirement", alCaptions, alColumns, ds);

            gvMatrixReq.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvMatrixReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMatrixReq();
    }


    protected void gvMatrixReq_PreRender(object sender, EventArgs e)
    {

        //foreach (GridDataItem dataItem in gvMatrixReq.Items)
        //{
        //    if ((dataItem.OwnerTableView.Name == "gvMatrix"))
        //    {
        //        //Merge rows 
        //        GridTableView tv = (GridTableView)dataItem.OwnerTableView;
        //        for (int rowIndex = tv.Items.Count - 2; rowIndex >= 0; rowIndex--)
        //        {
        //            GridDataItem row = tv.Items[rowIndex];
        //            GridDataItem previousRow = tv.Items[rowIndex + 1];
        //            if (row["Requirements"].Text == previousRow["Requirements"].Text)
        //            {
        //                row["Requirements"].RowSpan = previousRow["Requirements"].RowSpan < 2 ? 2 : previousRow["Requirements"].RowSpan + 1;
        //                previousRow["Requirements"].Visible = false;
        //                previousRow["Requirements"].Text = "&nbsp;";
        //            }
        //        }
        //    }
        //}
    }
    
    protected void gvMatrixReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item.OwnerTableView.Name == "gvMatrix")
        //{
        //    GridTableView tv = (GridTableView)e.Item.OwnerTableView;
            
        //    //int rowspan = 1;

        //    for (int rowIndex = tv.Items.Count - 2; rowIndex >= 0; rowIndex--)
        //    {

        //        GridDataItem row = tv.Items[rowIndex];

        //        GridDataItem previousRow = tv.Items[rowIndex + 1];
                
        //        if (row["Requirements"].Text == previousRow["Requirements"].Text)
        //        {
        //            //rowspan = rowspan + 1;

        //            row["Requirements"].RowSpan = 2;

        //            previousRow["Requirements"].Visible = false;
        //            previousRow["Requirements"].Text = "&nbsp;";
        //        }
        //        //else
        //        //{
        //        //    rowspan = 1;
        //        //}

        //    }
        //}
    }

    protected void gvMatrixReq_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }


}