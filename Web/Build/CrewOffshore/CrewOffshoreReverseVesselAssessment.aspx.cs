using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using System.ComponentModel;

public partial class CrewOffshoreReverseVesselAssessment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReverseVesselAssessment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            //toolbar.AddFontAwesomeButton("javascript:CallPrint('gvreversevessel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            gvreversevesselTab.AccessRights = this.ViewState;
            gvreversevesselTab.MenuList = toolbar.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["EMPID"] = null;
                ViewState["RANKID"] = null;
                ViewState["VESSELNAME"] = "";
                ViewState["STATUS"] = "";

                if (Request.QueryString["employeeid"] != null)
                    ViewState["EMPID"] = Request.QueryString["employeeid"];

                if (Request.QueryString["rankid"] != null)
                    ViewState["RANKID"] = Request.QueryString["rankid"];

                SetEmployeePrimaryDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvreversevesselTab_TabStripCommand(object sender, EventArgs e)
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


    protected void ShowExcel()
    {
        int iRowCount = 0;
      

        DataSet ds = new DataSet();

        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //ds = PhoenixCrewOffshoreReverseAssessment.CrewOffshoreReverseVesselList.Search(


        //          );

        if (ds.Tables.Count > 0)
            General.ShowExcel("", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvreversevessel.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvreversevessel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvreversevessel.CurrentPageIndex + 1;
            BindData();
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
      

        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreReverseAssessment.CrewOffshoreReverseVesselList(General.GetNullableInteger(ViewState["EMPID"].ToString())
                                                                    , General.GetNullableInteger(ViewState["RANKID"].ToString())
                                                                    , General.GetNullableDateTime(txtdoa.Text)
                                                                    , General.GetNullableInteger(ddldays.SelectedValue.ToString()));


        General.SetPrintOptions("gvreversevessel", "", alCaptions, alColumns, ds);

        gvreversevessel.DataSource = ds.Tables[1];
        gvreversevessel.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    private void BindDataDetails()
    {
        int iRowCount = 0;
       

        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
       

        DataSet ds = PhoenixCrewOffshoreReverseAssessment.CrewOffshoreReverseVesselList(General.GetNullableInteger(ViewState["EMPID"].ToString())
                                                                    , General.GetNullableInteger(ViewState["RANKID"].ToString())
                                                                    ,General.GetNullableDateTime(txtdoa.Text)
                                                                    ,General.GetNullableInteger(ddldays.SelectedValue.ToString())
                                                                    ,General.GetNullableString(ViewState["VESSELNAME"].ToString())
                                                                    , General.GetNullableString(ViewState["STATUS"].ToString()));



        General.SetPrintOptions("gvSuitability", "", alCaptions, alColumns, ds);

        gvSuitability.DataSource = ds.Tables[0];
        gvSuitability.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvreversevessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if(e.CommandName.ToUpper()=="TRAVEL")
            {
                ViewState["VESSELNAME"] = ((RadLabel)e.Item.FindControl("lblvessel")).Text;
                ViewState["STATUS"] = "TRAVEL";
                BindDataDetails();                
                gvSuitability.Rebind();
            }
            else if (e.CommandName.ToUpper() == "APPROVAL")
            {
                ViewState["VESSELNAME"] = ((RadLabel)e.Item.FindControl("lblvessel")).Text;
                ViewState["STATUS"] = "APPROVAL";
                BindDataDetails();
                gvSuitability.Rebind();
            }
            else if (e.CommandName.ToUpper() == "SIGNON")
            {
                ViewState["VESSELNAME"] = ((RadLabel)e.Item.FindControl("lblvessel")).Text;
                ViewState["STATUS"] = "SIGNON";
                BindDataDetails();
                gvSuitability.Rebind();
            }
            else if (e.CommandName.ToUpper() == "PROPOSAL")
            {
                ViewState["VESSELNAME"] = ((RadLabel)e.Item.FindControl("lblvessel")).Text;
                ViewState["STATUS"] = "PROPOSAL";
                BindDataDetails();
                gvSuitability.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvreversevessel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton imgSuitableCheck = (LinkButton)e.Item.FindControl("imgSuitableCheck");
            if(imgSuitableCheck!=null)imgSuitableCheck.Attributes.Add("onclick", "javascript:openNewWindow('crew','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheckWithDocument.aspx?empid=" + ViewState["EMPID"].ToString() + "&personalmaster=true&popup=1'); return false;");
           // imgSuitableCheck.Attributes.Add("click",)
            //RadLabel lblstage1 = (RadLabel)e.Item.FindControl("lblstage1");
            //GridDataItem dataItem = (GridDataItem)e.Item;

            //if (Convert.ToInt16(lblstage1.Text) > 0)
            //    lblstage1.ForeColor = Color.Red;

            //if (Convert.ToInt16(lblstage1.Text) == 0)
            //    lblstage1.ForeColor = Color.Green;

            ////RadLabel lblstage2 = (RadLabel)e.Item.FindControl("lblstage2");

            ////if (Convert.ToInt16(lblstage2.Text) > 0)
            ////    lblstage2.ForeColor = Color.Red;

            ////if (Convert.ToInt16(lblstage2.Text) == 0)
            ////    lblstage2.ForeColor = Color.Green;

            //RadLabel lblstage3 = (RadLabel)e.Item.FindControl("lblstage3");

            //if (Convert.ToInt16(lblstage3.Text) > 0)
            //    lblstage3.ForeColor = Color.Red;

            //if (Convert.ToInt16(lblstage3.Text) == 0)
            //    lblstage3.ForeColor = Color.Green;

            //TableCell myCell1 = dataItem["Travel"];
            //if (Convert.ToInt16(((RadLabel)myCell1.FindControl("lblstage2")).Text) > 0)           
            //    myCell1.BackColor = System.Drawing.Color.Red;

            //if (Convert.ToInt16(((RadLabel)myCell1.FindControl("lblstage2")).Text) == 0)           
            //    myCell1.BackColor = System.Drawing.Color.Green;


            //TableCell myCell2 = dataItem["Approval"];
            //if (Convert.ToInt16(((RadLabel)myCell2.FindControl("lblstage3")).Text) > 0)         
            //    myCell2.BackColor = System.Drawing.Color.Red;

            //if (Convert.ToInt16(((RadLabel)myCell2.FindControl("lblstage3")).Text) == 0)          
            //    myCell2.BackColor = System.Drawing.Color.Green;


            //TableCell myCell3 = dataItem["Signon"];
            //if (Convert.ToInt16(((RadLabel)myCell3.FindControl("lblstage4")).Text) > 0)           
            //    myCell3.BackColor = System.Drawing.Color.Red;

            //if (Convert.ToInt16(((RadLabel)myCell3.FindControl("lblstage4")).Text) == 0)           
            //    myCell3.BackColor = System.Drawing.Color.Green;            

        }
    }

    protected void gvreversevessel_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace("ASC", "").Replace("DESC", "");
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



    protected void gvSuitability_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataDetails();
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["EMPID"].ToString()));

            //txtEmployeeNumber.Visible = true;

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtdoa.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDOA"].ToString())); // dt.Rows[0]["FLDDOA"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddldays_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        BindData();
        gvreversevessel.Rebind();
    }
}
