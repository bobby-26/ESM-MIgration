using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentManagementFMSReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            //gvFMSReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvFMSReport.PageSize = 2000;
        }
    }

    private void BindData()
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFMSReport.CurrentPageIndex + 1;
        gvFMSReport.DataSource = PhoenixDocumentManagementFMSReports.MenuAccessTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode , 0, null);
    }

    protected void gvFMSReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvFMSReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    protected void gvFMSReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGERIGHTS"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel menucode = (RadLabel)item.FindControl("lblMenuCode");
                RadLabel menuname = (RadLabel)item.FindControl("lblOrgMenuName");
                RadLabel menuvalue = (RadLabel)item.FindControl("lblMenuValue");
                RadLabel appliesto = (RadLabel)item.FindControl("lblAppliesTo");
                RadLabel parentcode = (RadLabel)item.FindControl("lblParentCode");
                RadLabel sortorder = (RadLabel)item.FindControl("lblsortorder");
                RadLabel url = (RadLabel)item.FindControl("lblurl");

                int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                if (url.Text == string.Empty)
                {
                    ucError.ErrorMessage = "Please Select the child node";
                    ucError.Visible = true;
                    return;
                }
                PhoenixDocumentManagementFMSReports.FMSMenuInsert(usercode, menuname.Text,Convert.ToInt32(menucode.Text), menuvalue.Text, Convert.ToInt32(parentcode.Text), sortorder.Text, Convert.ToInt32(appliesto.Text), url.Text);
                ucStatus.Text = "Data Saved Successfully";
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
        try
        {
            gvFMSReport.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}