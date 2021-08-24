using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class Purchase_PurchaseEfficiency : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
      

            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Show Report", "SHOWREPORT");
            toolbar.AddButton("Visual", "SHOWVISUAL",ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            //toolbar1.AddFontAwesomeButton("../Purchase/PurchaseEfficiency.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar1.AddFontAwesomeButton("../Purchase/PurchaseEfficiency.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            //toolbar1.AddFontAwesomeButton("../Purchase/PurchaseEfficiency.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            toolbar1.AddImageButton("../Purchase/PurchaseEfficiency.aspx", "Find", "search.png", "FIND");
            toolbar1.AddImageButton("../Purchase/PurchaseEfficiency.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar1.AddImageButton("../Purchase/PurchaseEfficiency.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            sessionFilterValues();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SUMMARY"] = "1";
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                BindYear();
                BindVesselGroupList();
                bindPurchaseLocation();
            }

            //ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //    // bind filter criteria if any
    //    BindFilterCriteria();
    //}
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("FIND"))
            {
                filterPurchase.CurrentPurchaseEfficiencyFilter = null;
                sessionFilterValues();
            }

           

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                filterPurchase.CurrentPurchaseEfficiencyFilter = null;
             //   ucTitle.Text = "Purchase Efficiency";
                ddlYear.SelectedValue = System.DateTime.Today.Year.ToString();
                ddlType.SelectedValue = 1.ToString();
                lstQuarter.SelectedValue = null;
                lstMonth.SelectedValue = null;
                //chkGroupList.ClearSelection();
                //lstPurchaseLocation.ClearSelection();
                foreach (ButtonListItem item in chkGroupList.Items)
                {
                    item.Selected = false;
                }
                foreach(ButtonListItem item in lstPurchaseLocation.Items)
                {
                    item.Selected = false;
                }
                
                ShowReport();
                gvCrew.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            //{
            //    filterPurchase.CurrentPurchaseEfficiencyFilter = null;
            //    sessionFilterValues();
            //}

            if (CommandName.ToUpper().Equals("SHOWVISUAL"))
            {
                Response.Redirect("../Purchase/PurchaseEfficiencyVisual.aspx");
            }

            ViewState["PAGENUMBER"] = 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = filterPurchase.CurrentPurchaseEfficiencyFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }

        string Year = CheckIsNull(nvc.Get("ddlYear"));
        CheckUncheck(Year, "ddlYear");
        ddlType.SelectedValue = CheckIsNull(nvc.Get("ddlType"));
        string Quarter = CheckIsNull(nvc.Get("lstQuarter"));
        CheckUncheck(Quarter, "Quarter");
        string Month = CheckIsNull(nvc.Get("lstMonth"));
        CheckUncheck(Month, "Month");
        //chkGroupList.SelectedValue = CheckIsNull(nvc.Get("chkGroupList")) == ""?null: CheckIsNull(nvc.Get("chkGroupList"));
        //lstPurchaseLocation.SelectedValue = CheckIsNull(nvc.Get("lstPurchaseLocation")) =="" ? null : CheckIsNull(nvc.Get("lstPurchaseLocation"));
        ShowReport();
    }

    private string CheckIsNull(string value)
    {
        return value == null ? "" : value;
    }
    public NameValueCollection sessionFilterValues()
    {
        NameValueCollection nvc = new NameValueCollection();

        if (IsPostBack)
        {
            nvc.Clear();
            nvc.Add("ddlYear", YearList());
            nvc.Add("chkGroupList", selectedPurchaseGroupList());
            nvc.Add("lstPurchaseLocation", selectedPurchaseList());
            nvc.Add("lstMonth", selectedMonthList());
            nvc.Add("lstQuarter", selectedQuarterList());
            nvc.Add("ddlType", ddlType.SelectedValue);
            nvc.Add("ddlTypeName", ddlType.SelectedItem.ToString());
            filterPurchase.CurrentPurchaseEfficiencyFilter = nvc;
        }
        else
        {
            filterPurchase.CurrentPurchaseEfficiencyFilter = null;
        }
        return filterPurchase.CurrentPurchaseEfficiencyFilter;
    }

    protected void CheckUncheck(string Values, string ID)
    {
        string[] values = Values.Split(',');
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = values[i].Trim();
            if (ID == "ddlYear")
            {
                //ddlYear.Items.FindByValue(values[i]).Selected = true;
            }
            //else if (ID == "Quarter")
            //    lstQuarter.Items.FindByValue(values[i]).Selected = true;
            //else if (ID == "Month")
            //    lstMonth.Items.FindByValue(values[i]).Selected = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();
        
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVENDORNAME", "FLDFORMNO", "FLDVESSELNAME", "FLDSTOCKTYPE", "FLDLOCATIONNAME", "FLDGROUPNAME", "FLDPURCHASER","FLDPOAMOUNTTOTAL"};
        string[] alCaptions = { "Vendor",  "Form No.", "Vessel", "Stock Type", "Purchase Location", "'Purchase Group", "Purchaser", "PO Total(USD)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (filterPurchase.CurrentPurchaseEfficiencyFilter != null)
        {
            NameValueCollection Filter = filterPurchase.CurrentPurchaseEfficiencyFilter;

            ds = PhoenixPurchaseEfficiency.PurchaseEfficiency(General.GetNullableString(Filter.Get("ddlYear").ToString())
                                                            , General.GetNullableString(Filter.Get("lstQuarter").ToString())
                                                            , General.GetNullableString(Filter.Get("lstMonth").ToString())
                                                            , General.GetNullableString(Filter.Get("chkGroupList").ToString())
                                                            , General.GetNullableString(Filter.Get("lstPurchaseLocation").ToString())
                                                            , General.GetNullableString(Filter.Get("ddlType"))
                                                            , 1
                                                            , gvCrew.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPurchaseEfficiency.PurchaseEfficiency(YearList()
                                                            , selectedQuarterList()
                                                            , selectedMonthList()
                                                            , selectedPurchaseGroupList()
                                                            , selectedPurchaseList()
                                                            , ddlType.SelectedValue
                                                            , 1
                                                            , gvCrew.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=Purchase Efficiency.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>EXECUTIVE SHIP MANAGEMENT</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Purchase Efficiency</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");

        if (ViewState["SUMMARY"].ToString() == "1")
        {
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            Response.Write("<tr>");
            //for (int i = 0; i < alCaptions1.Length; i++)
            //{
            //    Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            //    Response.Write("<b>" + alCaptions1[i] + "</b>");
            //    Response.Write("</td>");
            //}
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
    }

    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;
        //divPage.Visible = true;
        //divTab1.Visible = true;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVENDORNAME", "FLDFORMNO", "FLDVESSELNAME", "FLDSTOCKTYPE", "FLDLOCATIONNAME", "FLDGROUPNAME", "FLDPURCHASER", "FLDPOAMOUNTTOTAL" };
        string[] alCaptions = { "Vendor", "Form No.", "Vessel", "Stock Type", "Purchase Location", "'Purchase Group", "Purchaser", "PO Total(USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (filterPurchase.CurrentPurchaseEfficiencyFilter != null)
        {
            NameValueCollection Filter = filterPurchase.CurrentPurchaseEfficiencyFilter;

            ds = PhoenixPurchaseEfficiency.PurchaseEfficiency(General.GetNullableString(Filter.Get("ddlYear").ToString())
                                                            , General.GetNullableString(Filter.Get("lstQuarter").ToString())
                                                            , General.GetNullableString(Filter.Get("lstMonth").ToString())
                                                            , General.GetNullableString(Filter.Get("chkGroupList").ToString())
                                                            , General.GetNullableString(Filter.Get("lstPurchaseLocation").ToString()) 
                                                            , General.GetNullableString(Filter.Get("ddlType"))
                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvCrew.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPurchaseEfficiency.PurchaseEfficiency(YearList()
                                                            , selectedQuarterList()
                                                            , selectedMonthList()
                                                            , selectedPurchaseGroupList()
                                                            , selectedPurchaseList()
                                                            , ddlType.SelectedValue
                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvCrew.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        }


      
            gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
     
        ViewState["ROWSINGRIDVIEW"] = 0;
        //}
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

   
    protected void gvCrew_RowDataBound(object sender, GridItemEventArgs e)
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
    }

    protected void gvCrew_Sorting(object sender, GridSortCommandEventArgs e)
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
    protected void gvCrew_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
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

    protected void BindYear()
    {
        int index = 1;
        ddlYear.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
        for (int i = (DateTime.Today.Year); i >= 2010; i--)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Insert(index, new RadListBoxItem(li.ToString(), li.ToString()));
            index++;
        }

        if (filterPurchase.CurrentPurchaseEfficiencyFilter == null)
        {
            if (ddlYear.SelectedValue == "")
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        }
    }

    private string YearList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in ddlYear.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }


    private void BindVesselGroupList()
    {
        DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 246); //246 Vessel Group hard type
        chkGroupList.DataSource = ds;
        chkGroupList.DataBindings.DataTextField = "FLDHARDNAME";
        chkGroupList.DataBindings.DataValueField = "FLDHARDCODE";

        ButtonListItem li = new ButtonListItem("--Check All--", "0");
        //li.Attributes.Add("onclick", "checkUnchekAll(this);");
       
        chkGroupList.Items.Insert(0, li);

        chkGroupList.DataBind();
       
    }
    public static DataSet ListPurchaseLocation()
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ds = DataAccess.ExecSPReturnDataSet("PRPURCHASELOCATIONLIST", ParameterList);
           
            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void DataBound(object sender, EventArgs e)
    {
        ButtonListItem li = new ButtonListItem("--Check All--", "0");
        //li.Attributes.Add("onclick", "checkUnchekAll(this);");
        lstPurchaseLocation.Items.Insert(0, li);
        chkGroupList.Items.Insert(0, li);
    }

    public void bindPurchaseLocation()
    {
        lstPurchaseLocation.DataSource = ListPurchaseLocation();
        lstPurchaseLocation.DataBindings.DataTextField = "FLDLOCATIONNAME";
        lstPurchaseLocation.DataBindings.DataValueField = "FLDLOCATIONID";
       
        lstPurchaseLocation.DataBind();
    }

    private string selectedPurchaseList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ButtonListItem item in lstPurchaseLocation.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }
    private string selectedPurchaseGroupList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ButtonListItem item in chkGroupList.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    private string selectedMonthList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstMonth.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    private string selectedQuarterList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstQuarter.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
         ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

