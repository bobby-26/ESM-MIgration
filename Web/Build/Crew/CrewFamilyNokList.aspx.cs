using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewFamilyNokList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbarsub = new PhoenixToolbar();            
            toolbarsub.AddFontAwesomeButton("../Crew/CrewFamilyNokList.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewFamilyNokFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewFamilyNokList.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["LAUNCHEDFROM"] = "";
                ViewState["pl"] = "";

                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();

                if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                    ViewState["pl"] = Request.QueryString["pl"].ToString();

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

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
                BindData();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;

                Filter.CurrentNewApplicantFilterCriteria = null;
                gvCrewSearch.CurrentPageIndex = 0;
                BindData();
                gvCrewSearch.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewSearch.Rebind();
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;

        BindData();

    }
    
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "NOKNAME")
            {
                Filter.CurrentCrewSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                string familyid = ((RadLabel)e.Item.FindControl("lblfamlyid")).Text;

                Session["REFRESHFLAG"] = null;
                Response.Redirect("..\\Crew\\CrewFamilyNok.aspx?familyid=" + familyid, false);
            }
            if (e.CommandName.ToUpper() == "GETEMP")
            {
                Filter.CurrentCrewSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                string familyid = ((RadLabel)e.Item.FindControl("lblfamlyid")).Text;

                Session["REFRESHFLAG"] = null;
                Response.Redirect("..\\Crew\\CrewPersonalGeneral.aspx?empid=" + Filter.CurrentCrewSelection, false);
            }
            if (e.CommandName.ToUpper() == "ADD")
            {

                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                DataRowView groupDataRow = (DataRowView)e.Item.DataItem;

                Filter.CurrentCrewSelection = groupDataRow["FLDEMPLOYEEID"].ToString();
               
                Session["REFRESHFLAG"] = null;
                Response.Redirect("..\\Crew\\CrewFamilyNok.aspx", false);
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

    
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            LinkButton lnknokname = (LinkButton)e.Item.FindControl("lnknokname");
         
            if ((lnknokname.Text == "") || (lnknokname.Text == null))
            {
                e.Item.Visible = false;
            }
            
        }
    }
   
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFILENO", "FLDRANKPOSTEDNAME", "FLDNAME", "FLDNOKNAME", "FLDRELATIONSHIP", "FLDDOB", "FLDGENDER", "FLDNOK" };
        string[] alCaptions = { "File No", "Rank", "Employee", "Family/Nok", "Relationship", "Date of Birth", " Gender", "NOK Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
        if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
        {
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc.Add("txtName", string.Empty);
                nvc.Add("txtFileNumber", string.Empty);
                nvc.Add("txtPassortNo", string.Empty);
                nvc.Add("txtSeamanbookNo", string.Empty);
                nvc.Add("ddlZone", string.Empty);
                nvc.Add("lstRank", string.Empty);
                nvc.Add("lstNationality", string.Empty);
                nvc.Add("ddlActiveYN", string.Empty);
                nvc.Add("ddlCity", string.Empty);
                nvc.Add("txtNOKName", string.Empty);
                nvc.Add("ddlVessel", string.Empty);
                nvc.Add("ucPrincipal", string.Empty);
            }
        }
        DataTable dt = PhoenixCommonCrew.familynok(nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                    , nvc != null ? nvc.Get("txtPassortNo") : string.Empty
                                                                    , nvc != null ? nvc.Get("txtSeamanbookNo") : string.Empty
                                                                    , nvc != null ? nvc.Get("txtFileNumber") : string.Empty
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlZone") : string.Empty)
                                                                    , nvc != null ? nvc.Get("lstRank") : string.Empty
                                                                    , nvc != null ? nvc.Get("txtName") : string.Empty
                                                                    , nvc != null ? nvc.Get("ddlStatus") : string.Empty
                                                                    , nvc != null ? (byte?)General.GetNullableInteger(nvc.Get("ddlActiveYN")) : 1
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvCrewSearch.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , nvc != null ? nvc.Get("txtNOKName") : string.Empty
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ucPrincipal") : string.Empty));


        General.ShowExcel("Family/NOK", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFILENO", "FLDRANKPOSTEDNAME", "FLDNAME", "FLDNOKNAME", "FLDRELATIONSHIP", "FLDDOB", "FLDGENDER", "FLDNOK" };
        string[] alCaptions = { "File No", "Rank", "Employee", "Family/Nok", "Relationship", "Date of Birth", " Gender", "NOK Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;

            if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
            {
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    nvc.Add("txtName", string.Empty);
                    nvc.Add("txtFileNumber", string.Empty);
                    nvc.Add("txtPassortNo", string.Empty);
                    nvc.Add("txtSeamanbookNo", string.Empty);
                    nvc.Add("lstRank", string.Empty);
                    nvc.Add("lstNationality", string.Empty);
                    nvc.Add("ddlActiveYN", string.Empty);
                    nvc.Add("ddlStatus", string.Empty);
                    nvc.Add("txtNOKName", string.Empty);
                    nvc.Add("ddlVessel", string.Empty);
                    nvc.Add("ucPrincipal", string.Empty);
                }
                else
                {
                    nvc["ddlPool"] = Request.QueryString["pl"];
                }
            }
            DataTable dt = PhoenixCommonCrew.familynok(nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                    , nvc != null ? nvc.Get("txtPassortNo") : string.Empty
                                                                    , nvc != null ? nvc.Get("txtSeamanbookNo") : string.Empty
                                                                    , nvc != null ? nvc.Get("txtFileNumber") : string.Empty
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlZone") : string.Empty)
                                                                    , nvc != null ? nvc.Get("lstRank") : string.Empty
                                                                    , nvc != null ? nvc.Get("txtName") : string.Empty
                                                                    , nvc != null ? nvc.Get("ddlStatus") : string.Empty
                                                                    , nvc != null ? (byte?)General.GetNullableInteger(nvc.Get("ddlActiveYN")) : 1
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvCrewSearch.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , nvc != null ? nvc.Get("txtNOKName") : string.Empty
                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("ucPrincipal") : string.Empty));


            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvCrewSearch", "Family/NOK", alCaptions, alColumns, ds);


            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
   
}
