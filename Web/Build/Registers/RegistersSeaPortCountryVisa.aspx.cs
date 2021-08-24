using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersSeaPortCountryVisa : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersSeaPortCountryVisa.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCountryVisa')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuRegistersCountryVisa.AccessRights = this.ViewState;
            MenuRegistersCountryVisa.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Seaport", "SEAPORT", ToolBarDirection.Right);
            MenuRegistersCountryVisaMain.AccessRights = this.ViewState;
            MenuRegistersCountryVisaMain.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["countryid"] = "";

                gvCountryVisa.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            if (Request.QueryString["countryid"] != null)
                ViewState["countryid"] = Request.QueryString["countryid"].ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersCountryVisa_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOUNTRYNAME", "FLDVISATYPENAME", "FLDTIMETAKEN", "FLDDAYSREQUIREDFORVISA", "FLDPHYSICALPRESENCEYESNO", "FLDPHYSICALPRESENCESPECIFICATION", "FLDURGENTPROCEDURE", "FLDREMARKS", "FLDMODIFIEDBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Country Name", "Visa Type", "Time Taken", "Days Required", "Physical Presence Y/N", "Physical Presence Specification", "Urgent Procedure", "Remarks", "Last Modified By", "Modified Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCountryVisa.CountryVisaSearch(General.GetNullableInteger(ViewState["countryid"].ToString())
                                        , null
                                        , sortexpression
                                        , sortdirection
                                        , (int)ViewState["PAGENUMBER"]
                                        , gvCountryVisa.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount);

        General.SetPrintOptions("gvCountryVisa", "Country Visa", alCaptions, alColumns, ds);


        gvCountryVisa.DataSource = ds;
        gvCountryVisa.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCOUNTRYNAME", "FLDVISATYPENAME", "FLDTIMETAKEN", "FLDDAYSREQUIREDFORVISA", "FLDPHYSICALPRESENCEYESNO", "FLDPHYSICALPRESENCESPECIFICATION", "FLDURGENTPROCEDURE", "FLDREMARKS", "FLDMODIFIEDBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Country Name", "Visa Type", "Time Taken", "Days Required", "Physical Presence Y/N", "Physical Presence Specification", "Urgent Procedure", "Remarks", "Last Modified By", "Modified Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCountryVisa.CountryVisaSearch(General.GetNullableInteger(ViewState["countryid"].ToString()), null,
                                sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        if (ds.Tables.Count > 0)
            General.ShowExcel("Country Visa", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }
    protected void MenuRegistersCountryVisaMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEAPORT"))
        {
            Response.Redirect("../Registers/RegistersSeaport.aspx?countryid=" + ViewState["countryid"].ToString(), true);
        }
    }


    protected void gvCountryVisa_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCountryVisa.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvCountryVisa_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

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

    protected void gvCountryVisa_ItemDataBound1(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel lblVisaId = (RadLabel)e.Item.FindControl("lblVisaID");
            RadLabel lblCountry = (RadLabel)e.Item.FindControl("lblCountryName");
            
            LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");
            if (imgRemarks != null)
            {
                imgRemarks.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Registers/RegistersCountryVisaRemarks.aspx?id=" + lblVisaId.Text + "', 'xlarge')");         
            }
            
            RadLabel lblPhysicalPresenceSpecification = (RadLabel)e.Item.FindControl("lblPhysicalPresenceSpecification");

            if (lblPhysicalPresenceSpecification != null)
            {
                UserControlToolTip ucPhyPresenceTT = (UserControlToolTip)e.Item.FindControl("ucPhyPresenceTT");
                ucPhyPresenceTT.Position = ToolTipPosition.TopCenter;
                ucPhyPresenceTT.TargetControlId = lblPhysicalPresenceSpecification.ClientID;
            }
            
            RadLabel lblUrgentProcedure = (RadLabel)e.Item.FindControl("lblUrgentProcedure");

            if (lblUrgentProcedure != null)
            {
                UserControlToolTip ucUrgentProcTT = (UserControlToolTip)e.Item.FindControl("ucUrgentProcTT");
                ucUrgentProcTT.Position = ToolTipPosition.TopCenter;
                ucUrgentProcTT.TargetControlId = lblUrgentProcedure.ClientID;
            }
        }

    }
    protected void gvCountryVisa_SortCommand(object sender, GridSortCommandEventArgs e)
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
