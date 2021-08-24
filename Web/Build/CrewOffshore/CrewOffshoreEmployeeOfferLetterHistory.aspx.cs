using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewOffshoreEmployeeOfferLetterHistory : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvCrewAppointmentLetter.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$lnkDoubleClick");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$lnkDoubleClickEdit");
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["newapplicant"] != null)
            {
                ViewState["empid"] = Filter.CurrentNewApplicantSelection;
                ViewState["type"] = "n";
            }
            else
            {
                ViewState["empid"] = Filter.CurrentCrewSelection;
                ViewState["type"] = "p";
            }

            SetEmployeePrimaryDetails();
            gvCrewAppointmentLetter.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployeeOfferLetterHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewAppointmentLetter')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");


        MenuCrewAppointmentHistory.AccessRights = this.ViewState;
        MenuCrewAppointmentHistory.MenuList = toolbar.Show();
       // MenuCrewAppointmentHistory.SetTrigger(pnlAppointmentHistroy);

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        //toolbarsub.AddButton("Back", "BACK");
        CrewQuery.AccessRights = this.ViewState;
        CrewQuery.MenuList = toolbarsub.Show();

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");


       //BindData();
       
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("CrewOffshoreCrewList.aspx", true);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDRANKCODE", "FLDSALAGREEDUSD", "FLDPLUSORMINUSRANGE" };
        string[] alCaptions = { "Vessel", "Rank", "Wage Agreed (USD)", "Contract Period" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixCrewOffshoreOfferLetter.EmployeeOfferLetterHistorySearch(
            Int32.Parse(ViewState["empid"].ToString())
            , sortexpression, sortdirection
            , 1
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount);

        General.ShowExcel("Crew Offer Letter History", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void CrewAppointmentHistory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
           
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = null;
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDRANKCODE", "FLDSALAGREEDUSD", "FLDPLUSORMINUSRANGE" };
        string[] alCaptions = { "Vessel", "Rank", "Wage Agreed (USD)", "Contract Period" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreOfferLetter.EmployeeOfferLetterHistorySearch(
            Int32.Parse(ViewState["empid"].ToString())
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvCrewAppointmentLetter.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        General.SetPrintOptions("gvCrewAppointmentLetter", "Crew Offer Letter History", alCaptions, alColumns, ds);

        gvCrewAppointmentLetter.DataSource = ds;
        gvCrewAppointmentLetter.VirtualItemCount = iRowCount;
       
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

    protected void gvCrewAppointmentLetter_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
      
    }
    protected void gvCrewAppointmentLetter_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
       
    }



    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvCrewAppointmentLetter_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewAppointmentLetter.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewAppointmentLetter_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvCrewAppointmentLetter_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
      
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                RadLabel l = (RadLabel)e.Item.FindControl("lblLetterId");
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkRank");


                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton cmdOfferletter = (LinkButton)e.Item.FindControl("cmdOfferletter");
                if (cmdOfferletter != null)
                {
                    cmdOfferletter.Attributes.Add("onclick", "javascript:openNewWindow('chml', '', '"+Session["sitepath"]+"/Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=OFFERLETTER&showmenu=0"
                        + "&offerletterid=" + l.Text + "');return false;");
                }
            }
        }
    }
}
