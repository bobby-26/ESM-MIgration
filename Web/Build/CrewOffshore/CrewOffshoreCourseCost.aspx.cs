using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshoreCourseCost : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Course", "COURSE");
            toolbarmain.AddButton("Cost of Course", "COSTOFCOURSE");
            MnuCourseCost.AccessRights = this.ViewState;
            MnuCourseCost.MenuList = toolbarmain.Show();
            MnuCourseCost.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreCourseCost.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCourseCost')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuCourseCost.AccessRights = this.ViewState;
            MenuCourseCost.MenuList = toolbar.Show();
          
            if (!IsPostBack)
            {
                BindCourses(ddlCourse);    
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCourseCost.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
                   
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
    protected void CourseCost_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;

        if (CommandName.ToUpper().Equals("COURSE"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                Response.Redirect("../Registers/RegistersOffshoreDocumentCourse.aspx");
            else
                Response.Redirect("../Registers/RegistersDocumentCourse.aspx");
        }
        else if (CommandName.ToUpper().Equals("COSTOFCOURSE"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCourseCost.aspx");
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDINSTITUTENAME", "FLDCURRENCYNAME", "FLDCOST", "FLDDURATION" };
        string[] alCaptions = { "Institute", "Currency", "Cost", "Duration (In Days)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreCourseCost.CourseCostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(ddlCourse.SelectedValue), null,                               
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCourseCost.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CourseCost.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Course Cost</h3></td>");        
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");        
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuCourseCost_TabStripCommand(object sender, EventArgs e)
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

        string[] alColumns = { "FLDINSTITUTENAME", "FLDCURRENCYNAME", "FLDCOST", "FLDDURATION" };
        string[] alCaptions = { "Institute","Currency", "Cost","Duration (In Days)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreCourseCost.CourseCostSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               General.GetNullableInteger(ddlCourse.SelectedValue), null,          
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCourseCost.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvCourseCost", "Course Cost", alCaptions, alColumns, ds);

        gvCourseCost.DataSource = ds;
        gvCourseCost.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
     
    }

    protected void gvCourseCost_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        //BindData();
    }

    protected void gvCourseCost_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblCourseCostId")).Text) != null)
                    DeleteCourseCost(Int32.Parse(((RadLabel)e.Item.FindControl("lblCourseCostId")).Text));

                BindData();
                gvCourseCost.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCourseCost(((UserControlMaskNumber)e.Item.FindControl("txtCostAdd")).Text
                    , ((UserControlAddressType)e.Item.FindControl("ucInstitutionAdd")).SelectedAddress
                    , ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreCourseCost.InsertCourseCost(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(((UserControlAddressType)e.Item.FindControl("ucInstitutionAdd")).SelectedAddress),
                    General.GetNullableInteger(ddlCourse.SelectedValue),
                    General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtCostAdd")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtDurationAdd")).Text)
                    );

                BindData();
                gvCourseCost.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidCourseCost(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text
                        , ((UserControlAddressType)e.Item.FindControl("ucInstitutionEdit")).SelectedAddress
                        , ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateCourseCost(
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblCourseCostIdEdit")).Text),
                    General.GetNullableInteger(((UserControlAddressType)e.Item.FindControl("ucInstitutionEdit")).SelectedAddress),
                    General.GetNullableInteger(ddlCourse.SelectedValue),
                    General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtCost")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtDuration")).Text)
                    );

                
                BindData();
                gvCourseCost.Rebind();
            }
            else if(e.CommandName.ToUpper().Equals("PAGE"))
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

    protected void gvCourseCost_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrencyEdit");            
            DataRowView drview = (DataRowView)e.Item.DataItem;

            if (ucCurrency != null)
            {
                string CID = drview["FLDCURRENCYID"].ToString();
                ucCurrency.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(1, true);
                ucCurrency.SelectedCurrency = CID; //default : USD
            }

            //DropDownList ddlCourseEdit = (DropDownList)e.Row.FindControl("ddlCourseEdit");
            //if (ddlCourseEdit != null)
            //{
            //    BindCourses(ddlCourseEdit);
            //    ddlCourseEdit.SelectedValue = drview["FLDDOCUMENTID"].ToString();
            //}

            UserControlAddressType ucIns = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
            if (ucIns != null)
            {
                string Ins = drview["FLDADDRESSID"].ToString();
                ucIns.AddressList = PhoenixRegistersAddress.ListAddress("138");
                ucIns.SelectedAddress = Ins;
            }
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkInstituteName");
            //if (lb != null) lb.Text = lb.Text.TrimEnd(trimChar);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }

        if (e.Item is GridFooterItem)
        {

            //DropDownList ddlCourseAdd = (DropDownList)e.Row.FindControl("ddlCourseAdd");
            //if (ddlCourseAdd != null) BindCourses(ddlCourseAdd);

            UserControlAddressType ucInstitute = (UserControlAddressType)e.Item.FindControl("ucInstitutionAdd");
            if(ucInstitute != null)
            {

            }
            UserControlCurrency ucCurrencyAdd = (UserControlCurrency)e.Item.FindControl("ucCurrencyAdd");
            if (ucCurrencyAdd != null)
            {
                ucCurrencyAdd.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(1, true);
                ucCurrencyAdd.SelectedCurrency = "10"; //USD
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void BindCourses(RadComboBox ddl)
    {
        string doctype = PhoenixCommonRegisters.GetHardCode(1, 103, "6"); // STCW
        ddl.Items.Clear();
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            ddl.DataSource = PhoenixRegistersDocumentCourse.ListOffshoreDocumentCourse(null);
        else
            ddl.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);

        ddl.DataTextField = "FLDDOCUMENTNAME";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.DataBind();
       // ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void InsertCourseCost(int? Addressid, int? courseid, int? currencyid, decimal? cost, int? duration)
    {
       PhoenixCrewOffshoreCourseCost.InsertCourseCost(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Addressid, courseid, currencyid, cost, duration);
    }

    private void UpdateCourseCost(int coursecostid,int? Addressid,int? courseid, int? currencyid, decimal? cost, int? duration)
    {
        PhoenixCrewOffshoreCourseCost.UpdateCourseCost(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , coursecostid, Addressid,courseid, currencyid, cost,duration);
    }

    private bool IsValidCourseCost(string cost, string institute, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlCourse.SelectedValue) == null)
            ucError.ErrorMessage = "Course is required.";

        if (General.GetNullableInteger(currency) == null)
            ucError.ErrorMessage = "Currency is required.";
        
        if (cost == "0.00" || General.GetNullableDecimal(cost) == null)
            ucError.ErrorMessage = "Cost is required.";

        if (General.GetNullableInteger(institute) == null)
            ucError.ErrorMessage = "Institute is required.";

        return (!ucError.IsError);
    }

    private void DeleteCourseCost(int Coursecostid)
    {
       PhoenixCrewOffshoreCourseCost.DeleteCourseCost(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Coursecostid);
    }

    //protected void ucConsulate_TextChanged(object sender, EventArgs e)
    //{
    //    DataSet ds = PhoenixRegistersAddress.EditAddress(1, long.Parse(ucConsulate.SelectedAddress));

    //    ucCurrency.SelectedCurrency = ds.Tables[0].Rows[0]["FLDCLINICCURRENCY"].ToString();
    //}

    protected void gvCourseCost_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCourseCost.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
