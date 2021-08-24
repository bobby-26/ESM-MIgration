using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Collections;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionReportsSealUsage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsSealUsage.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSealUsage')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsSealUsage.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsSealUsage.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuSealExport.AccessRights = this.ViewState;
            MenuSealExport.MenuList = toolbar.Show();
           // MenuSealExport.SetTrigger(pnlSealUsage);
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindLocation();
                gvSealUsage.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

    protected void BindLocation()
    {
        ddlLocation.DataSource = PhoenixInspectionSealLocation.SealLocationTreeList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlLocation.DataTextField = "FLDLOCATIONNAME";
        ddlLocation.DataValueField = "FLDLOCATIONID";
        ddlLocation.DataBind();
       // ddlLocation.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds;
        string[] alColumns = { "FLDLOCATIONNAME", "FLDSEALPOINTNAME", "FLDSEALNO", "FLDSEALTYPENAME", "FLDPERSONAFFIXINGSEAL", "FLDDATEAFFIXED", "FLDREASONNAME" };
        string[] alCaptions = { "Location", "Seal Point", "Seal Number", "Seal Type", "Seal Affixed by", "Date Affixed", "Reason" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionSealUsage.SealUsageSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
            , General.GetNullableInteger(ucSealType.SelectedQuick)
            , General.GetNullableInteger(ddlLocation.SelectedValue)
            , General.GetNullableString(txtSealNumber.Text)
            , General.GetNullableString(txtSealAffixedby.Text)
            , General.GetNullableDateTime(txtFromDate.Text)
            , General.GetNullableDateTime(txtToDate.Text)
            , General.GetNullableInteger(ucReason.SelectedQuick)
            , sortexpression, sortdirection,
            gvSealUsage.CurrentPageIndex+1,
            gvSealUsage.PageSize,
            ref iRowCount,
            ref iTotalPageCount
            , General.GetNullableInteger(chkShowall.Checked==true?"1":"0"));

        General.SetPrintOptions("gvSealUsage", "Seal Usage Report", alCaptions, alColumns, ds);
        gvSealUsage.DataSource = ds;
        gvSealUsage.VirtualItemCount = iRowCount;
   
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuSeal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                BindData();
                gvSealUsage.Rebind();
               // SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuSealExport_TabStripCommand(object sender, EventArgs e)
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
                ucSealType.SelectedQuick = "";
                txtSealNumber.Text = "";
                txtSealAffixedby.Text = "";
                //ddlLocation.SelectedIndex = 0;
                ddlLocation.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlLocation.Text = "--Select--";
                BindLocation();
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ucReason.SelectedQuick = "";
                chkShowall.Checked = false;
                BindData();
                gvSealUsage.Rebind();
               // SetPageNavigator();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvSealUsage.Rebind();
                //SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealUsage_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvSealUsage_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvSealUsage_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{        
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DataRowView dv = (DataRowView)e.Row.DataItem;
    //        Image imgFlag = e.Row.FindControl("imgFlag") as Image;
    //        if (imgFlag != null && dv["FLDREPLACEMENTDUE"].ToString().Equals("3"))
    //        {
    //            imgFlag.Visible = true;
    //            imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
    //            imgFlag.ToolTip = "Replacement Overdue";
    //        }
    //        else if (imgFlag != null && dv["FLDREPLACEMENTDUE"].ToString().Equals("2"))
    //        {
    //            imgFlag.Visible = true;
    //            imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
    //            imgFlag.ToolTip = "Replacement Due within 30 days";
    //        }
    //        else if (imgFlag != null && dv["FLDREPLACEMENTDUE"].ToString().Equals("1"))
    //        {
    //            imgFlag.Visible = true;
    //            imgFlag.ImageUrl = Session["images"] + "/" + "green-symbol.png";
    //            imgFlag.ToolTip = "Replacement Due within 60 days";
    //        }
    //        else
    //        {
    //            if (imgFlag != null) imgFlag.Visible = false;
    //        }
    //    }
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDLOCATIONNAME", "FLDSEALPOINTNAME", "FLDSEALNO", "FLDSEALTYPENAME", "FLDPERSONAFFIXINGSEAL", "FLDDATEAFFIXED", "FLDREASONNAME" };
        string[] alCaptions = { "Location", "Seal Point", "Seal Number", "Seal Type", "Seal Affixed by", "Date Affixed", "Reason" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionSealUsage.SealUsageSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
            , General.GetNullableInteger(ucSealType.SelectedQuick)
            , General.GetNullableInteger(ddlLocation.SelectedValue)
            , General.GetNullableString(txtSealNumber.Text)
            , General.GetNullableString(txtSealAffixedby.Text)
            , General.GetNullableDateTime(txtFromDate.Text)
            , General.GetNullableDateTime(txtToDate.Text)
            , General.GetNullableInteger(ucReason.SelectedQuick)
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
            , General.GetNullableInteger(chkShowall.Checked==true ? "1" : "0"));

        Response.AddHeader("Content-Disposition", "attachment; filename=SealUsage.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Seal Usage Report</h3></td>");
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


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
        //SetPageNavigator();
    }

    protected void gvSealUsage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvSealUsage_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
          
            Image imgFlag = e.Item.FindControl("imgFlag") as Image;
            if (imgFlag != null && DataBinder.Eval(e.Item.DataItem, "FLDREPLACEMENTDUE").ToString().Equals("3"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                imgFlag.ToolTip = "Replacement Overdue";
            }
            else if (imgFlag != null && DataBinder.Eval(e.Item.DataItem, "FLDREPLACEMENTDUE").ToString().Equals("2"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                imgFlag.ToolTip = "Replacement Due within 30 days";
            }
            else if (imgFlag != null && DataBinder.Eval(e.Item.DataItem, "FLDREPLACEMENTDUE").ToString().Equals("1"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "green-symbol.png";
                imgFlag.ToolTip = "Replacement Due within 60 days";
            }
            else
            {
                if (imgFlag != null) imgFlag.Visible = false;
            }
        }
    }
}
