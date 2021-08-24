using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class VesselAccountsEmployeeBankAccountBulkUpdateDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["id"] = Request.QueryString["ID"].ToString();
            ViewState["status"] = Request.QueryString["Status"].ToString();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsEmployeeBankAccountBulkUpdateDetails.aspx?ID=" + ViewState["id"].ToString() + "&Status=" + ViewState["status"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvEmployeeBankDetails')", "Print Grid", "icon_print.png", "PRINT");
            MenuExcelUploadItem.MenuList = toolbargrid.Show();
            MenuExcelUploadItem.AccessRights = this.ViewState;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["status"].ToString() == "Active")
                toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuEmployeeDetails.AccessRights = this.ViewState;
            MenuEmployeeDetails.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvEmployeeBankDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    public void BindData()
    {

        DataSet ds = new DataSet();

        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns1 = { "FLDFILENO", "FLDACCOUNTNO", "FLDACCOUNTHOLDERNAME", "FLDBANKNAME", "FLDIFSCCODE", "FLDSWIFTCODE", "FLDCURRENCYCODE", "FLDACCOUNTOPENEDBY" };
        string[] alCaptions1 = { "File No", "Account No", "Account Holder Name", "Bank Name", "IFSC Code", "Swift Code", "Currency Code", "Account Opened By" };

        ds = PhoenixVesselAccountsEmployeeBankAccount.EmployeeBankAccountBulkDetailsSearch(General.GetNullableGuid(ViewState["id"].ToString()), int.Parse(ddltype.SelectedValue)
                                                                                           , sortexpression, sortdirection
                                                                                           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                           , General.ShowRecords(null)
                                                                                           , ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvEmployeeBankDetails", "Employee Bank Details", alCaptions1, alColumns1, ds);
        gvEmployeeBankDetails.DataSource = ds;
        gvEmployeeBankDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void MenuEmployeeDetails_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                PhoenixVesselAccountsEmployeeBankAccount.EmployeeBankBulkUpdateConfirm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["id"].ToString()));
                BindData();
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("VesselAccountsEmployeeBankAccountBulkUpdate.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuExcelUploadItem_TabStripCommand(object sender, EventArgs e)
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
        try
        {
            DataSet ds = new DataSet();

            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? sortdirection = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns1 = { "FLDFILENO", "FLDACCOUNTNO", "FLDACCOUNTHOLDERNAME", "FLDBANKNAME", "FLDIFSCCODE", "FLDSWIFTCODE", "FLDCURRENCYCODE", "FLDACCOUNTOPENEDBY" };
            string[] alCaptions1 = { "File No", "Account No", "Account Holder Name", "Bank Name", "IFSC Code", "Swift Code", "Currency Code", "Account Opened By" };

            ds = PhoenixVesselAccountsEmployeeBankAccount.EmployeeBankAccountBulkDetailsSearch(General.GetNullableGuid(ViewState["id"].ToString()), int.Parse(ddltype.SelectedValue)
                                                                                         , sortexpression, sortdirection
                                                                                         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                         , General.ShowRecords(null)
                                                                                         , ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename= EmployeeBankDetails.xls");
            Response.ContentType = "application/vnd.msexcel";

            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns1.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Employee Bank Details </center></h3></td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions1.Length; i++)
            {
                Response.Write("<td class='text'>");
                Response.Write("<b>" + alCaptions1[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns1.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns1[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvEmployeeBankDetails_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel acno = (RadLabel)e.Item.FindControl("acno");

            Image imgFlag = e.Item.FindControl("imgFlag") as Image;
            if (drv["FLDBANKACCOUNTID"].ToString() == "")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "green-symbol.png";
                imgFlag.ToolTip = "Updated";
            }
            if (imgFlag != null && drv["FLDEMPLOYEEID"].ToString() == "" && drv["FLDSTATUS"].ToString() == "")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                imgFlag.ToolTip = "MisMatch";
            }
            //else if (imgFlag != null && dv["FLDDUEOVERDUEYN"].ToString().Equals("1"))
            //{
            //    imgFlag.Visible = true;
            //    imgFlag.ImageUrl = Session["images"] + "/" + "green-symbol.png";
            //    imgFlag.ToolTip = "Having Change";
            //}
            //else
            //{
            //    if (imgFlag != null) imgFlag.Visible = false;
            //}
        }
    }

    protected void gvEmployeeBankDetails_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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

    protected void gvEmployeeBankDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmployeeBankDetails.CurrentPageIndex + 1;
        BindData();
    }
}